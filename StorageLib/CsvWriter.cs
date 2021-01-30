/**************************************************************************************

StorageLib.CsvWriter
====================

Write strings, integers, etc. to a CSV file

Written in 2020 by Jürgpeter Huber 
Contact: PeterCode at Peterbox dot com

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/
using System;
using System.IO;
using System.Text;
using System.Threading;


namespace StorageLib {


  /// <summary>
  /// Write strings, integers, etc. to a CSV file. The file can be kept open until the end of the application, but then
  /// disposed.<para/>
  /// Only one thread at a time is allowed to write a line. StartNewLine() or WriteFirstLineChar() lock byteArray, 
  /// WriteEndOfLine() unlocks byteArray again. If another thread tries to write, it has to wait.<para/>
  /// All characters get written into a byteArray. WriteLine() checks if byteArray is nearly full and writes it to
  /// the file, then starts the flush timer. When nothing else get written into byteArray, the flush timer starts
  /// and empties byteArray into the file. Also Dispose() empties the byteArray into the file.
  /// </summary>
  public class CsvWriter: IDisposable {

    #region Properties
    //      ----------

    /// <summary>
    /// Name of CSV file to be written
    /// </summary>
    public string FileName { get; }


    /// <summary>
    /// CsvConfig parameters used to write file
    /// </summary>
    public CsvConfig CsvConfig { get; }


    /// <summary>
    /// Estimated chars in a line for average values. With max values and long strings, the actual length might be much longer.
    /// </summary>
    public int EstimatedLineLenght { get; }


    /// <summary>
    /// How many bytes can a line max contain ? (25% of CsvConfig.BufferSize)
    /// </summary>
    public int MaxLineByteLenght { get; private set; }


    /// <summary>
    /// Delay in millisecond before flush gets executed after the last write
    /// </summary>
    public readonly int FlushDelay;


    /// <summary>
    /// Number of bytes not written to file yet
    /// </summary>
    public int ByteBufferLength { get { return writePos; } }

    #endregion


    #region Constructor
    //      -----------

    #pragma warning disable IDE0069 // Disposable fields should be disposed
    FileStream? fileStream;
    #pragma warning restore IDE0069 // Disposable fields should be disposed
    readonly bool isFileStreamOwner;
    readonly byte[] byteArray;
    int writePos;
    readonly int maxBufferWriteLength;
    readonly byte delimiter;
    Timer? flushTimer;
    //byte[] tempBytes;
    readonly char[] tempChars;


    /// <summary>
    /// Constructor
    /// </summary>
    public CsvWriter(
      string? fileName, 
      CsvConfig csvConfig, 
      int estimatedLineLenght, 
      FileStream? existingFileStream = null, 
      int flushDelay = 200) 
    {
      if (!string.IsNullOrEmpty(fileName) && existingFileStream!=null) 
        throw new Exception("CsvWriter constructor: There was neither an existingFileStream nor a fileName provided.");

      if (existingFileStream!=null) {
        FileName = existingFileStream.Name;
      } else {
        FileName = fileName!;
      }
      CsvConfig = csvConfig;
      if (csvConfig.Encoding!=Encoding.UTF8) 
        throw new Exception($"CsvWriter constructor: Only reading from UTF8 files is supported, but the Encoding was " +
          $"{csvConfig.Encoding.EncodingName} for file {fileName}.");
      
      delimiter = (byte)csvConfig.Delimiter;
      MaxLineByteLenght = CsvConfig.BufferSize/Csv.ByteBufferToReserveRatio;
      if (estimatedLineLenght*Csv.Utf8BytesPerChar>MaxLineByteLenght)
        throw new Exception($"CsvWriter constructor: BufferSize {CsvConfig.BufferSize} should be at least " + 
          $"{Csv.ByteBufferToReserveRatio*Csv.Utf8BytesPerChar} times bigger than the estimated length needed of " +
          $"{estimatedLineLenght} for file {fileName}.");

      EstimatedLineLenght = estimatedLineLenght;
      tempChars = new char[50]; //tempChars is only used for formating decimals, which needs maybe 10-30 chars
      FlushDelay = flushDelay;
      if (existingFileStream is null) {
        isFileStreamOwner = true;
        //fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, CsvConfig.BufferSize, FileOptions.SequentialScan | FileOptions.WriteThrough);
        fileStream = new FileStream(fileName!, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, CsvConfig.BufferSize);
      } else {
        isFileStreamOwner = false;
        fileStream = existingFileStream;
      }
      //byteArray = new byte[csvConfig.BufferSize];
      //writePos = 0;
      //maxBufferWriteLength = CsvConfig.BufferSize - maxLineLenght;
      byteArray = new byte[csvConfig.BufferSize + MaxLineByteLenght];
      writePos = 0;
      maxBufferWriteLength = CsvConfig.BufferSize;
      flushTimer = new Timer(flushTimerMethod, null, Timeout.Infinite, Timeout.Infinite);
    }
    #endregion


    #region Disposable Interface
    //     ---------------------

    /// <summary>
    /// Executes disposal of CsvReader exactly once.
    /// </summary>
    public void Dispose() {
      Dispose(true);

      GC.SuppressFinalize(this);
    }


    /// <summary>
    /// Is CsvReader already exposed ?
    /// </summary>
    protected bool IsDisposed {
      get { return isDisposed==1; }
    }


    int isDisposed = 0;


    /// <summary>
    /// Inheritors should call Dispose(false) from their destructor
    /// </summary>
    protected void Dispose(bool disposing) {
      var wasDisposed = Interlocked.Exchange(ref isDisposed, 1);//prevents that 2 threads dispose simultaneously
      if (wasDisposed==1) return; // already disposed

      OnDispose(disposing);
    }


    /// <summary>
    /// Inheritors should overwrite OnDispose() and put the disposal code in there. 
    /// </summary>
    /// <param name="disposing">is false if it is called from a destructor.</param>
    protected virtual void OnDispose(bool disposing) {
      var wasflushTimer = Interlocked.Exchange(ref flushTimer, null);
      if (wasflushTimer!=null) {
        try {
          wasflushTimer.Dispose();//Dispose() is multi-threading safe
        } catch (Exception ex) {
          CsvConfig.ReportException?.Invoke(ex);
        }
      }

      releaseFileStream();
    }


    private void releaseFileStream() {
      lock (byteArray) {
        if (fileStream==null) return;

        if (writePos>0) {
          fileStream.Write(byteArray, 0, writePos);
          writePos = 0;
        }
        if (isFileStreamOwner) {
          fileStream.Dispose();
        }
        fileStream = null;
      }
    }
    #endregion


    #region Methods
    //      -------

    #region Write
    //      -----

#if DEBUG
    bool isLocked = false;
#endif
    int lineStart = 0;


    /// <summary>
    /// Locks byteArray until WriteEndOfLine() gets called.
    /// </summary>
    public void StartNewLine() {
      Monitor.Enter(byteArray);
#if DEBUG
      if (isLocked) {
        throw new Exception();
      }
      isLocked = true;
#endif
      lineStart = writePos;
    }


    /// <summary>
    /// Locks byteArray until WriteEndOfLine() gets called and writes 1 character at the start of line, indicating if
    /// it is a new item, changing a item or deleting a item.
    /// </summary>
    public void WriteFirstLineChar(char c) {
      Monitor.Enter(byteArray);
#if DEBUG
      if (isLocked) {
        throw new Exception();
      }
      isLocked = true;
#endif
      lineStart = writePos;
      if (c==CsvConfig.Delimiter || c=='\r' || c=='\n') 
        throw new Exception($"CsvWriter.WriteFirstLineChar(char) '{FileName}':illegal character '{c.ToPureString()}'." + Environment.NewLine + GetPresentContent());

      if (c<0x80) {
        byteArray[writePos++] = (byte)c;
      } else {
        throw new Exception();
      }
    }


    /// <summary>
    /// If an exception occurs when writing data, the writer gets blocked forever. Calling CleanupAfterException() removes
    /// from byteArray of the new line, since the line is most likely incomplete and unlocks the buffer.
    /// </summary>
    public void CleanupAfterException() {
#if DEBUG
      if (!isLocked) {
        throw new Exception();
      }
      isLocked = false;
#endif
      writePos = lineStart; //removes the probably half finished line
      Monitor.Exit(byteArray);
    }


    /// <summary>
    /// Writes Carriage Return and Line Feed to the bytêArray. If byteArray is nearly full, it gets written to the file. The
    /// flushTimer gets started. If WriteEndOfLine() is not called again, the flushTimer writes the remaining bytes to the file.
    /// </summary>
    public void WriteEndOfLine() {
#if DEBUG
      if (!isLocked) {
        throw new Exception();
      }
      isLocked = false;
#endif
      try {
        byteArray[writePos++] = 0x0D;
        byteArray[writePos++] = 0x0A;

        var actualLineLength = writePos-lineStart;
        if (actualLineLength>MaxLineByteLenght) {
          //actually written line is longer than expected.
          throw new Exception($"MaxLineByteLenght {MaxLineByteLenght} should be at least {writePos-lineStart}.");

          /*most likely it's not a problem to write the longer than expected string and it would be possible to
          write the new line length into the CSV file header so the CsvReader knows what to expect. But that solution
          gets complicated and has some ugly consequences. Better stick with the simple solution: If a line is longer
          than 25% of the byte buffer, an exception is thrown during write
          */
        }

        if (writePos>maxBufferWriteLength) {
          fileStream!.Write(byteArray, 0, maxBufferWriteLength);
          var numberOfBytesToCopy = writePos - maxBufferWriteLength;
          if (numberOfBytesToCopy>0) {
            Array.Copy(byteArray, maxBufferWriteLength, byteArray, 0, numberOfBytesToCopy);
            writePos = numberOfBytesToCopy;
          } else {
            writePos = 0;
          }
        }
        kickFlushTimer();

      } finally {
        Monitor.Exit(byteArray);
      }
    }


    /// <summary>
    /// Write null, i.e. write only delimiter.
    /// </summary>
    public void WriteNull() {
      byteArray[writePos++] = delimiter;
    }


    /// <summary>
    /// Write boolean as 0 or 1 to UTF8 FileStream followed by delimiter.
    /// </summary>
    public void Write(bool b) {
      if (b) {
        byteArray[writePos++] = (byte)'1';
      } else {
        byteArray[writePos++] = (byte)'0';
      }
      byteArray[writePos++] = delimiter;
    }


    /// <summary>
    /// Write boolean? as '', 0 or 1 to UTF8 FileStream followed by delimiter.
    /// </summary>
    public void Write(bool? b) {
      if (b.HasValue) {
        if (b.Value) {
          byteArray[writePos++] = (byte)'1';
        } else {
          byteArray[writePos++] = (byte)'0';
        }
      }
      byteArray[writePos++] = delimiter;
    }


    const byte minusByte = (byte)'-';


    /// <summary>
    /// Write nullable integer to UTF8 FileStream followed by delimiter.
    /// </summary>
    public void Write(int? i) {
      if (i is null) {
        byteArray[writePos++] = delimiter;
      } else {
        Write(i.Value);
      }
    }


    /// <summary>
    /// Write integer to UTF8 FileStream followed by delimiter.
    /// </summary>
    public void Write(int i) {
      int start;
      if (i<0) {
        byteArray[writePos++] = minusByte;
        start = writePos;
        //since -int.MinValue is bigger than int.MaxValue, i=-i does not work for int.Minvalue.
        //therefore write 1 digit first and guarantee that i>int.MinValue
        byteArray[writePos++] = (byte)(-(i % 10) + '0');
        i /= 10;
        if (i==0) {
          byteArray[writePos++] = delimiter;
          return;
        }
        i = -i;
      } else {
        start = writePos;
      }

      while (i>9) {
        byteArray[writePos++] = (byte)((i % 10) + '0');
        i /= 10;
      }
      byteArray[writePos++] = (byte)(i + '0');
      var end = writePos-1;
      while (end>start) {
        var temp = byteArray[end];
        byteArray[end--] = byteArray[start];
        byteArray[start++] = temp;
      }
      byteArray[writePos++] = delimiter;
    }


    /// <summary>
    /// Write nullable long to UTF8 FileStream followed by delimiter.
    /// </summary>
    public void Write(long? i) {
      if (i is null) {
        byteArray[writePos++] = delimiter;
      } else {
        Write(i.Value);
      }
    }


    /// <summary>
    /// Write integer to UTF8 FileStream followed by delimiter.
    /// </summary>
    public void Write(long l) {
      int start;
      if (l<0) {
        byteArray[writePos++] = minusByte;
        start = writePos;
        //since -long.MinValue is bigger than long.MaxValue, l=-l does not work for long.Minvalue.
        //therefore write 1 digit first and guarantee that l>long.MinValue
        byteArray[writePos++] = (byte)(-(l % 10) + '0');
        l /= 10;
        if (l==0) {
          byteArray[writePos++] = delimiter;
          return;
        }
        l = -l;
      } else {
        start = writePos;
      }

      while (l>9) {
        byteArray[writePos++] = (byte)((l % 10) + '0');
        l /= 10;
      }
      byteArray[writePos++] = (byte)(l + '0');
      var end = writePos-1;
      while (end>start) {
        var temp = byteArray[end];
        byteArray[end--] = byteArray[start];
        byteArray[start++] = temp;
      }
      byteArray[writePos++] = delimiter;
    }


    static readonly string[] formats = {
      "0",
      ".#",
      ".##",
      ".###",
      ".####",
      ".#####",
      ".######",
      ".#######",
      ".########",
    };


    /// <summary>
    /// Writes at most 2 digits after comma, if they are not zero, followed by delimiter. Trailing zeros get truncated.
    /// </summary>
    public void WriteDecimal2(decimal d) {
      Write(d, 2);
    }


    /// <summary>
    /// Writes at most 2 digits after comma, if they are not zero, followed by delimiter. Trailing zeros get truncated.
    /// </summary>
    public void WriteDecimal2(decimal? d) {
      Write(d, 2);
    }


    /// <summary>
    /// Writes at most 4 digits after comma, if they are not zero, followed by delimiter. Trailing zeros get truncated.
    /// </summary>
    public void WriteDecimal4(decimal? d) {
      Write(d, 4);
    }


    /// <summary>
    /// Writes at most 4 digits after comma, if they are not zero, followed by delimiter. Trailing zeros get truncated.
    /// </summary>
    public void WriteDecimal4(decimal d) {
      Write(d, 4);
    }


    /// <summary>
    /// Writes at most 4 digits after comma, if they are not zero, followed by delimiter. Trailing zeros get truncated.
    /// </summary>
    public void WriteDecimal5(decimal? d) {
      Write(d, 5);
    }


    /// <summary>
    /// Writes at most 4 digits after comma, if they are not zero, followed by delimiter. Trailing zeros get truncated.
    /// </summary>
    public void WriteDecimal5(decimal d) {
      Write(d, 5);
    }


    /// <summary>
    /// Writes at most number of digitsAfterComma, if they are not zero, followed by delimiter. Trailing zeros get truncated.
    /// </summary>
    public void Write(decimal? d, int digitsAfterComma = int.MaxValue) {
      if (d is null) {
        byteArray[writePos++] = delimiter;
        return;
      }

      int charsWritten;
      if (digitsAfterComma<=8) {
        if (!d.Value.TryFormat(tempChars, out charsWritten, formats[digitsAfterComma])) throw new Exception($"CsvWriter.Write(decimal) '{FileName}': Cannot format {d}." + Environment.NewLine + GetPresentContent());
      } else {
        if (!d.Value.TryFormat(tempChars, out charsWritten)) throw new Exception($"CsvWriter.Write(decimal) '{FileName}': Cannot format {d}." + Environment.NewLine + GetPresentContent());
      }

      if (charsWritten==0) {
        //code like csvWriter.Write(0.4m, 0) results in empty string (charsWritten==0)
        byteArray[writePos++] = (byte)'0';
        byteArray[writePos++] = delimiter;
        return;
      } else if (charsWritten==1) {
        byteArray[writePos++] = (byte)tempChars[0];
        byteArray[writePos++] = delimiter;
        return;
      }

      for (int copyIndex = 0; copyIndex < charsWritten; copyIndex++) {
        byteArray[writePos++] = (byte)tempChars[copyIndex];
      }
      byteArray[writePos++] = delimiter;
    }


    /// <summary>
    /// Writes at most number of digitsAfterComma, if they are not zero, followed by delimiter. Trailing zeros get truncated.
    /// </summary>
    public void Write(decimal d, int digitsAfterComma = int.MaxValue) {
      int charsWritten;
      if (digitsAfterComma<=8) {
        if (!d.TryFormat(tempChars, out charsWritten, formats[digitsAfterComma])) 
          throw new Exception($"CsvWriter.Write(decimal) '{FileName}': Cannot format {d}." + Environment.NewLine + GetPresentContent());
      } else {
        if (!d.TryFormat(tempChars, out charsWritten)) 
          throw new Exception($"CsvWriter.Write(decimal) '{FileName}': Cannot format {d}." + Environment.NewLine + GetPresentContent());
      }

      //deal with zero here, this simplifies the code for removing trailing 0. Any other single digit value
      //can also be handled here.
      if (charsWritten==0) {
        //code like csvWriter.Write(0.4m, 0) results in charsWritten==0
        byteArray[writePos++] = (byte)'0';
        byteArray[writePos++] = delimiter;
        return;
      } else if (charsWritten==1) {
        byteArray[writePos++] = (byte)tempChars[0];
        byteArray[writePos++] = delimiter;
        return;
      }

      for (int copyIndex = 0; copyIndex<charsWritten; copyIndex++) {
        byteArray[writePos++] = (byte)tempChars[copyIndex];
      }
      byteArray[writePos++] = delimiter;
    }


    /// <summary>
    /// Writes a Unicode char, which might take several bytes, or null into the CSV file, followed by delimiter. If the 
    /// character itself is the same as the delimiter char, it gets written as "\t". Carriage Return as "\r". Line feed 
    /// as "\n". The character '\' as "\\"
    /// </summary>
    public void Write(char? c) {
      if (c is null) {
        byteArray[writePos++] = delimiter;
      } else {
        Write(c.Value);
      }
    }


    /// <summary>
    /// Writes a Unicode char, which might take several bytes, into the CSV file, followed by delimiter. If the character
    /// itself is the same as the delimiter char, it gets written as "\t". Carriage Return as "\r". Line feed as "\n". 
    /// The character '\' as "\\"
    /// </summary>
    public void Write(char c) {
      if (c==CsvConfig.Delimiter) {
        byteArray[writePos++] = (byte)'\\';
        byteArray[writePos++] = (byte)'t';
      } else if (c=='\r') {
        byteArray[writePos++] = (byte)'\\';
        byteArray[writePos++] = (byte)'r';
      } else if (c=='\n') {
        byteArray[writePos++] = (byte)'\\';
        byteArray[writePos++] = (byte)'n';
      } else if (c=='\\') {
        byteArray[writePos++] = (byte)'\\';
        byteArray[writePos++] = (byte)'\\';
      } else if (c<0x80) {
        //one byte ASCII
        byteArray[writePos++] = (byte)c;
      } else {
        //UTF
        foreach (var utf8Byte in Encoding.UTF8.GetBytes(new string(c, 1))) {
          byteArray[writePos++] = utf8Byte;
        }
      }
      byteArray[writePos++] = delimiter;
    }


    /// <summary>
    /// Writes a Unicode string to the CSV file, followed by a delimiter. A null string writes only the delimiter, an
    /// empty string writes "\ ". If string contains a character with the value of the delimiter char, it gets written as 
    /// "\t". Carriage Return as "\r". Line Feed as "\n". The character '\' as "\\".
    /// </summary>
    public void Write(string? s) {
      if (s!=null) {
        if (s.Length==0) {
          byteArray[writePos++] = (byte)'\\';
          byteArray[writePos++] = (byte)' ';

        } else {
          if (s.Length>MaxLineByteLenght) {
            throw new Exception($"CsvWriter.Write() '{FileName}': The length {s.Length} of string 's.[..30]' is longer " +
              $"than MaxLineLenght {MaxLineByteLenght}." + Environment.NewLine + GetPresentContent());
          }
          var startWritePos = writePos;
          for (int readIndex = 0; readIndex < s.Length; readIndex++) {
            var c = s[readIndex];
            if (c==CsvConfig.Delimiter) {
              byteArray[writePos++] = (byte)'\\';
              byteArray[writePos++] = (byte)'t';
            } else if (c=='\r') {
              byteArray[writePos++] = (byte)'\\';
              byteArray[writePos++] = (byte)'r';
            } else if (c=='\n') {
              byteArray[writePos++] = (byte)'\\';
              byteArray[writePos++] = (byte)'n';
            } else if (c=='\\') {
              byteArray[writePos++] = (byte)'\\';
              byteArray[writePos++] = (byte)'\\';
            } else if (c<0x80) {
              //one byte ASCII
              byteArray[writePos++] = (byte)c;
            } else {
              //UTF
              //start again, write string with replaced control characters into chars on the stack and copy that as UTF8 to
              //byteArray
              Span<char> chars = stackalloc char[s.Length*2];//too big, but exact size needed is not known
              var charsIndex = 0;
              for (readIndex = 0; readIndex < s.Length; readIndex++) {
                c = s[readIndex];
                if (c==CsvConfig.Delimiter) {
                  chars[charsIndex++] = '\\';
                  chars[charsIndex++] = 't';
                } else if (c=='\r') {
                  chars[charsIndex++] = '\\';
                  chars[charsIndex++] = 'r';
                } else if (c=='\n') {
                  chars[charsIndex++] = '\\';
                  chars[charsIndex++] = 'n';
                } else if (c=='\\') {
                  chars[charsIndex++] = '\\';
                  chars[charsIndex++] = '\\';
                } else {
                  chars[charsIndex++] = c;
                }
              }

              //var byteLength = Encoding.UTF8.GetBytes(chars[..charsIndex], byteArray[startWritePos..]);
              var byteLength = Encoding.UTF8.GetBytes(chars[..charsIndex], byteArray.AsSpan()[startWritePos..]);
              writePos = startWritePos + byteLength;
              break;
            }
          }
        }
      }
      byteArray[writePos++] = delimiter;
    }


    /// <summary>
    /// Writes a Unicode string to the CSV file, followed by a carriage return and line feed. An empty string writes only 
    /// the carriage return and line feed. If string contains a Carriage Return or Line Feed, an Exception gets thrown.
    /// </summary>
    public void WriteLine(string line) {
      if (line.Length>MaxLineByteLenght) {
        throw new Exception($"CsvWriter.Write() '{FileName}': The length {line.Length} of string 'line.[..30]' is longer " +
          $"than MaxLineLenght {MaxLineByteLenght}." + Environment.NewLine + GetPresentContent());
      }

      StartNewLine();
      for (int readIndex = 0; readIndex<line.Length; readIndex++) {
        var c = line[readIndex];
        if (c=='\r' || c=='\n') {
          throw new Exception($"CsvWriter.Write() '{FileName}': Carriage Return and Line Feed characters are not supported." +
            Environment.NewLine + GetPresentContent());

        } else if (c<0x80) {
          //ASCII
          byteArray[writePos++] = (byte)c;
        } else {
          //higher than ASCII
          for (int readIndex2 = readIndex; readIndex2<line.Length; readIndex2++) {
            c = line[readIndex];
            if (c=='\r' || c=='\n') {
              throw new Exception($"CsvWriter.Write() '{FileName}': Carriage Return and Line Feed characters are not supported." +
                Environment.NewLine + GetPresentContent());
            }
          }
          var byteLength = Encoding.UTF8.GetBytes(line, readIndex, line.Length-readIndex, byteArray, writePos);
          writePos += byteLength;
          break;
        }
      }
      WriteEndOfLine();
    }


    /// <summary>
    /// Writes a date without time to the CSV file, followed by delimiter.
    /// </summary>
    public void WriteDate(DateTime? date) {
      if (date.HasValue) {
        if (date!=date.Value.Date) throw new Exception($"CsvWriter.WriteDate() '{FileName}':does not support storing time '{date}'." + Environment.NewLine + GetPresentContent());

        writeDate(date.Value);
      }
      byteArray[writePos++] = delimiter;
    }


    /// <summary>
    /// Writes a date without time to the CSV file, followed by delimiter.
    /// </summary>
    public void WriteDate(DateTime date) {
      if (date!=date.Date) throw new Exception($"CsvWriter.WriteDate() '{FileName}':does not support storing time '{date}'." + Environment.NewLine + GetPresentContent());

      writeDate(date);
      byteArray[writePos++] = delimiter;
    }


    private void writeDate(DateTime date) {
      var day = date.Day;
      if (day>=30) {
        byteArray[writePos++] = (byte)'3';
        day -= 30;
      } else if (day>=20) {
        byteArray[writePos++] = (byte)'2';
        day -= 20;
      } else if (day>=10) {
        byteArray[writePos++] = (byte)'1';
        day -= 10;
      }
      byteArray[writePos++] = (byte)(day + '0');
      byteArray[writePos++] = (byte)('.');

      var month = date.Month;
      if (month>=10) {
        byteArray[writePos++] = (byte)'1';
        month -= 10;
      }
      byteArray[writePos++] = (byte)(month + '0');
      byteArray[writePos++] = (byte)('.');

      var year = date.Year;
      byteArray[writePos++] = (byte)(('0') + year / 1000);
      year %= 1000;
      byteArray[writePos++] = (byte)(('0') + year / 100);
      year %= 100;
      byteArray[writePos++] = (byte)(('0') + year / 10);
      year %= 10;
      byteArray[writePos++] = (byte)(('0') + year);
    }


    /// <summary>
    /// Writes a time with a precision of seconds or nothing for null to the CSV file, followed by delimiter.
    /// </summary>
    public void WriteTime(TimeSpan? time) {
      if (time.HasValue) {
        WriteTime(time.Value);
      } else {
        byteArray[writePos++] = delimiter;
      }
    }


    /// <summary>
    /// Writes a time with a precision of seconds to the CSV file, followed by delimiter.
    /// </summary>
    public void WriteTime(TimeSpan time) {
      var hours = time.Hours;
      var minutes = time.Minutes;
      var seconds = time.Seconds;
      if (hours>=24) throw new Exception($"CsvWriter.WriteTime() '{FileName}':does not support storing 24 hours or more." + Environment.NewLine + GetPresentContent());
      write(hours, minutes, seconds);
      byteArray[writePos++] = delimiter;
    }


    private void write(int hours, int minutes, int seconds) {
      if (hours>=20) {
        byteArray[writePos++] = (byte)'2';
        hours -= 20;
      } else if (hours>=10) {
        byteArray[writePos++] = (byte)'1';
        hours -= 10;
      }
      byteArray[writePos++] = (byte)('0' + hours);
      if (minutes==0 && seconds==0) {
        return;
      }

      byteArray[writePos++] = (byte)(':');
      var minutesTens = minutes / 10;
      if (minutesTens>0) {
        byteArray[writePos++] = (byte)('0' + minutesTens);
        minutes %= 10;
      }
      byteArray[writePos++] = (byte)('0' + minutes);
      if (seconds==0) {
        return;
      }

      byteArray[writePos++] = (byte)(':');
      var secondTens = seconds / 10;
      if (secondTens>0) {
        byteArray[writePos++] = (byte)('0' + secondTens);
        seconds %= 10;
      }
      byteArray[writePos++] = (byte)('0' + seconds);
    }


    /// <summary>
    /// Writes a date with a precision of minutes or nothing for null to the CSV file, followed by delimiter.
    /// </summary>
    public void WriteDateMinutes(DateTime? time) {
      if (time.HasValue) {
        WriteDateMinutes(time.Value);
      } else {
        byteArray[writePos++] = delimiter;
      }
    }


    /// <summary>
    /// Writes a date with a precision of minutes to the CSV file, followed by delimiter.
    /// </summary>
    public void WriteDateMinutes(DateTime dateTime) {
      writeDate(dateTime.Date);
      byteArray[writePos++] = (byte)' ';
      var hour = dateTime.Hour;
      var minute = dateTime.Minute;
      if (dateTime.Second>=30) minute++;
      var second = 0;
      write(hour, minute, second);
      byteArray[writePos++] = delimiter;
    }


    /// <summary>
    /// Writes a date with a precision of seconds or nothing for null to the CSV file, followed by delimiter.
    /// </summary>
    public void WriteDateSeconds(DateTime? time) {
      if (time.HasValue) {
        WriteDateSeconds(time.Value);
      } else {
        byteArray[writePos++] = delimiter;
      }
    }


    /// <summary>
    /// Writes date with a precision of seconds to the CSV file, followed by delimiter.
    /// </summary>
    public void WriteDateSeconds(DateTime dateTime) {
      writeDate(dateTime.Date);
      byteArray[writePos++] = (byte)' ';
      var hour = dateTime.Hour;
      var minute = dateTime.Minute;
      var second = dateTime.Second;
      write(hour, minute, second);
      byteArray[writePos++] = delimiter;
    }


    /// <summary>
    /// Writes DateTime with a precision of ticks to the CSV file, followed by delimiter.
    /// </summary>
    public void WriteDateTimeTicks(DateTime? dateTime) {
      if (dateTime.HasValue) {
        Write(dateTime.Value.Ticks);
      } else {
        byteArray[writePos++] = delimiter;
      }
    }


    /// <summary>
    /// Writes DateTime with a precision of ticks to the CSV file, followed by delimiter.
    /// </summary>
    public void WriteDateTimeTicks(DateTime dateTime) {
      Write(dateTime.Ticks);
    }


    /// <summary>
    /// Writes TimeSpan with a precision of ticks to the CSV file, followed by delimiter.
    /// </summary>
    public void WriteTimeSpanTicks(TimeSpan? duration) {
      if (duration.HasValue) {
        Write(duration.Value.Ticks);
      } else {
        byteArray[writePos++] = delimiter;
      }
    }


    /// <summary>
    /// Writes TimeSpan with a precision of ticks to the CSV file, followed by delimiter.
    /// </summary>
    public void WriteTimeSpanTicks(TimeSpan duration) {
      Write(duration.Ticks);
    }
    #endregion


    #region Flushing
    //      --------

    private void kickFlushTimer() {
      flushTimer?.Change(FlushDelay, Timeout.Infinite);
    }


    private void flushTimerMethod(object? state) {
      try {
        flush();
      } catch (Exception ex) {
        CsvConfig.ReportException?.Invoke(ex);
      }
    }


    private void flush() {
      lock (byteArray) {
        if (IsDisposed || fileStream==null || writePos<=0) return;

        fileStream.Write(byteArray, 0, writePos);
        writePos = 0;
        fileStream.Flush();
        stopFlushTimer();
      }
    }


    /// <summary>
    /// Asks the OS to flush the data immediately to the disk. Normally, this happens FlushDelay milliseconds after last write. 
    /// Flush() is multi-threading safe.
    /// </summary>
    public void Flush() {
      stopFlushTimer();
      flush();
    }


    private void stopFlushTimer() {
      flushTimer?.Change(Timeout.Infinite, Timeout.Infinite);//change is multi-threading safe
    }
    #endregion


    #region Transaction
    //      -----------

    int transactionStart = int.MinValue;
    long transactionFilePos;


    public void StartTransaction() {
      if (transactionStart!=int.MinValue) {
        throw new Exception($"CsvWriter.StartTransaction() '{FileName}': Cannot start new transaction, there " +
          "is already one active.");
      }
      if (Monitor.IsEntered(byteArray)) {
        throw new Exception($"CsvWriter.StartTransaction() '{FileName}': Cannot start new transaction, because " +
          "CsvWriter is busy writing. Start a transaction before writing any data.");
      }
      transactionStart = writePos;
      transactionFilePos = fileStream!.Position;
    }


    public void CommitTransaction() {
      if (transactionStart==int.MinValue) {
        throw new Exception($"CsvWriter.CommitTransaction() '{FileName}': Cannot commit transaction, there " +
          "is no transaction running.");
      }
      if (Monitor.IsEntered(byteArray)) {
        // It might not be possible to ever arrive here, but better be on the safe side. CleanupAfterException() should
        // prevent that there is still a lock.
        throw new Exception($"CsvWriter.CommitTransaction() '{FileName}': Cannot commit transaction, because " +
          "CsvWriter is busy writing. Commit the transaction once all its data is written.");
      }
      //nothing to do
      transactionStart = int.MinValue;
    }


    public void RollbackTransaction() {
      if (!Monitor.IsEntered(byteArray)) {
        Monitor.Enter(byteArray);
      }
      if (transactionStart==int.MinValue) {
        throw new Exception($"CsvWriter.RollbackTransaction() '{FileName}': Cannot rollback transaction, there " +
          "is no transaction running.");
      }

      if (transactionFilePos!=fileStream!.Position) {
        //some data has been written to the file since the transaction started. Shorten the file to the transaction
        //start and discard the content of byteArray
        fileStream.SetLength(transactionFilePos + transactionStart);
        writePos = 0;
      } else {
        //nothing has been written to the file since the transaction started. Just shorten the byteArray to the 
        //start of the transaction
        writePos = transactionStart;
      }
      transactionStart = int.MinValue;
#if DEBUG
      isLocked = false;
#endif
      Monitor.Exit(byteArray);
    }
    #endregion


    public string GetPresentContent() {
      //byteArray[readPos++]
      int fromPos;
      if (writePos>100) {
        fromPos = writePos - 100;
      } else {
        fromPos = 0;
      }
      var presentPos = writePos - fromPos;

      return UTF8Encoding.UTF8.GetString(byteArray, fromPos, presentPos).Replace(CsvConfig.Delimiter, '|') + '^';
    }
    #endregion
  }
}