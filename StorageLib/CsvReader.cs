/**************************************************************************************

StorageLib.CsvReader
====================

Reads strings, integers, etc. from a CSV file

Written in 2020 by Jürgpeter Huber 
Contact: https://github.com/PeterHuberSg/StorageLib

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
#pragma warning disable IDE0046 // Convert to conditional expression
#pragma warning disable IDE0045 // Convert to conditional expression


namespace StorageLib {


  /// <summary>
  /// Reads strings, integers, etc. from a CSV file. Dispose once reading finished.
  /// </summary>
  public class CsvReader: IDisposable {

    #region Properties
    //      ----------

    /// <summary>
    /// Name of the file read.
    /// </summary>
    public string FileName { get; }


    /// <summary>
    /// CsvConfig parameters used to read the file.
    /// </summary>
    public CsvConfig CsvConfig { get; }


    /// <summary>
    /// Estimated chars in a line for average values. With max values and long strings, the actual length might be much longer.
    /// </summary>
    public int EstimatedLineLength { get; }


    /// <summary>
    /// How many bytes can a line max contain ? (25% of CsvConfig.BufferSize)
    /// </summary>
    public int MaxLineByteLength { get; }


    /// <summary>
    /// Is file completely read ?
    /// </summary>
    public bool IsEof { get; private set; }


    /// <summary>
    /// Used to see read buffer content as string in debugger
    /// </summary>
    public string PresentContent { get { return this.GetPresentContent(); } }
    #endregion


    #region Constructor
    //      -----------
    FileStream? fileStream;
    readonly bool isFileStreamOwner;
    readonly byte[] byteArray;
    int readIndex;
    int endIndex;
    readonly int delimiter;
    readonly char[] tempChars;


    public CsvReader(
      string? fileName, 
      CsvConfig csvConfig, 
      int estimatedLineLength, 
      FileStream? existingFileStream = null) 
    {
      if (!string.IsNullOrEmpty(fileName) && existingFileStream!=null) 
        throw new Exception("CsvReader constructor: There were an existingFileStream and a fileName provided.");

      if (string.IsNullOrEmpty(fileName) && existingFileStream==null)
        throw new Exception("CsvReader constructor: There was neither an existingFileStream nor a fileName provided.");

      if (existingFileStream is null) {
        if (string.IsNullOrEmpty(fileName)) throw new Exception("CsvReader constructor: File name is missing.");
        FileName = fileName!;
      } else {
        FileName = existingFileStream.Name;
      }
      CsvConfig = csvConfig;
      if (csvConfig.Encoding!=Encoding.UTF8)
        throw new Exception($"CsvReader constructor '{FileName}': Only reading from UTF8 files is supported, but the " +
          $"Encoding was {csvConfig.Encoding.EncodingName}.");

      delimiter = (int)csvConfig.Delimiter;

      MaxLineByteLength = CsvConfig.BufferSize/Csv.ByteBufferToReserveRatio;
      if (estimatedLineLength*Csv.Utf8BytesPerChar>MaxLineByteLength)
        throw new Exception($"CsvReader constructor: BufferSize {CsvConfig.BufferSize} should be at least " + 
          $"{Csv.ByteBufferToReserveRatio*Csv.Utf8BytesPerChar} times bigger than MaxLineCharLength {estimatedLineLength} for file {fileName}.");

      EstimatedLineLength = estimatedLineLength;
      if (existingFileStream is null) {
        isFileStreamOwner = true;
        fileStream = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.None, CsvConfig.BufferSize, 
          FileOptions.SequentialScan);
      } else {
        isFileStreamOwner = false;
        fileStream = existingFileStream;
      }
      byteArray = new byte[CsvConfig.BufferSize + MaxLineByteLength];
      readIndex = 0;
      endIndex = 0;
      IsEof = false;
      tempChars = new char[MaxLineByteLength/Csv.Utf8BytesPerChar];
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
    /// Is CsvReader already disposed ?
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
      releaseFileStream();
    }


    private void releaseFileStream() {
      if (!isFileStreamOwner) return;

      var wasFileStream = Interlocked.Exchange(ref fileStream, null);//prevents that 2 threads release simultaneously
      wasFileStream?.Dispose();
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// is last character from file read ?
    /// </summary>
    /// <returns></returns>
    public bool IsEndOfFileReached() {
      if (readIndex<endIndex) {
        return false;
      }
      if (endIndex<CsvConfig.BufferSize) {
        IsEof = true;
        return true;
      }

      //in very rare cases the file fits exactly into the buffer. Read again to see if there are more bytes.
      if (!fillBufferFromFileStream(0)) {
        IsEof = true;
        return true;
      } else {
        return false;
      }
    }


#if DEBUG
    int lineStart = 0;
#endif


    /// <summary>
    /// Read carriage return and line feed or throw exception.
    /// </summary>
    public void ReadEndOfLine() {
      var remainingBytesCount = endIndex - readIndex;
      if (remainingBytesCount<=MaxLineByteLength) {
        if (!fillBufferFromFileStream(remainingBytesCount)) throw new Exception($"CsvReader.ReadEndOfLine() '{FileName}': premature EOF found:" + Environment.NewLine + GetPresentContent());
      }

      // test for Carriage Return
      if (byteArray[readIndex++]!=0x0D) { //carriage return) {
        throw new Exception($"CsvReader.ReadEndOfLine() '{FileName}': Carriage return missing: " + Environment.NewLine + GetPresentContent());
      }

      //test for line feed
      if (byteArray[readIndex++]!=0x0A) { //line feed) {
        throw new Exception($"CsvReader.ReadEndOfLine() '{FileName}': Line feed missing: " + Environment.NewLine + GetPresentContent());
      }
#if DEBUG
      if ((readIndex-lineStart)%CsvConfig.BufferSize > MaxLineByteLength) throw new Exception();
      lineStart = readIndex;
#endif
    }


    /// <summary>
    /// Skip until end of line, i.e. carriage return and line feed
    /// </summary>
    public void SkipToEndOfLine() {
      while (true) {
        if (byteArray[readIndex++]==0x0D) { //carriage return) {
          if (byteArray[readIndex++]==0x0A) { //line feed) {
            return;
          } else {
            throw new Exception($"CsvReader.SkipToEndOfLine() '{FileName}': Line feed missing: " + Environment.NewLine + GetPresentContent());
          }
        }
      }
    }


    bool areAllBytesRead = false;


    private bool fillBufferFromFileStream(int remainingBytesCount) {
      if (areAllBytesRead) {
        return remainingBytesCount>0;
      }

      if (remainingBytesCount>0) {
        Array.Copy(byteArray, readIndex, byteArray, 0, remainingBytesCount);
      }
      readIndex = 0;
      var bytesRead = fileStream!.Read(byteArray, remainingBytesCount, CsvConfig.BufferSize);
      if (bytesRead<CsvConfig.BufferSize) areAllBytesRead = true;

      endIndex = remainingBytesCount + bytesRead;
      if (endIndex<=0) {
        IsEof = true;
        return false;
      }
      return true;
    }


    /// <summary>
    /// Read boolean as 0 or 1 from UTF8 FileStream including closing delimiter.
    /// </summary>
    public bool ReadBool() {
      bool b;
      int readByteAsInt = (int)byteArray[readIndex++];
      if (readByteAsInt=='0') {
        b = false;
      } else if (readByteAsInt=='1') {
        b = true;
      } else {
        throw new Exception($"CsvReader.ReadBool() '{FileName}': Illegal character found: " + Environment.NewLine + GetPresentContent());
      }
      readByteAsInt = (int)byteArray[readIndex++];
      if (readByteAsInt!=CsvConfig.Delimiter) {
        throw new Exception($"CsvReader.ReadBool() '{FileName}': Illegal character found" + Environment.NewLine + GetPresentContent());
      }
      return b;
    }


    /// <summary>
    /// Read boolean? as '', 0 or 1 from UTF8 FileStream including closing delimiter.
    /// </summary>
    public bool? ReadBoolNull() {
      int readByteAsInt = (int)byteArray[readIndex++];
      if (readByteAsInt==CsvConfig.Delimiter) {
        return null;
      }
      bool b;
      if (readByteAsInt=='0') {
        b = false;
      } else if (readByteAsInt=='1') {
        b = true;
      } else {
        throw new Exception($"CsvReader.ReadBoolNull() '{FileName}': Illegal character found: " + Environment.NewLine + GetPresentContent());
      }
      readByteAsInt = (int)byteArray[readIndex++];
      if (readByteAsInt!=CsvConfig.Delimiter) {
        throw new Exception($"CsvReader.ReadBoolNull() '{FileName}': Illegal character found" + Environment.NewLine + GetPresentContent());
      }
      return b;
    }


    /// <summary>
    /// Read integer from UTF8 FileStream including closing delimiter.
    /// </summary>
    public int ReadInt() {
      //check for minus sign
      int readByteAsInt = (int)byteArray[readIndex++];
      var isMinus = readByteAsInt=='-';
      if (isMinus) {
        readByteAsInt = (int)byteArray[readIndex++];
      }

      //read first digit. There must be at least 1
      var i = 0;
      if (readByteAsInt>='0' && readByteAsInt<='9') {
        i = 10*i + readByteAsInt - '0';
      } else {
        throw new Exception($"CsvReader.ReadInt() '{FileName}': Illegal character found: " + Environment.NewLine + GetPresentContent());
      }

      //read other digits until delimiter is reached
      while (true) {
        readByteAsInt = (int)byteArray[readIndex++];
        if (readByteAsInt>='0' && readByteAsInt<='9') {
          i = 10*i + readByteAsInt - '0';
          continue;
        }

        if (readByteAsInt!=delimiter) throw new Exception($"CsvReader.ReadInt() '{FileName}': Illegal character found: " + Environment.NewLine + GetPresentContent());

        return isMinus ? -i : i;
      }
    }


    /// <summary>
    /// Read integer or null from UTF8 FileStream including closing delimiter.
    /// </summary>
    public int? ReadIntNull() {
      int readByteAsInt = (int)byteArray[readIndex++];
      //check for null
      if (readByteAsInt==delimiter) {
        return null;
      }

      //check for minus sign
      var isMinus = readByteAsInt=='-';
      if (isMinus) {
        readByteAsInt = (int)byteArray[readIndex++];
      }

      //read first digit. There must be at least 1
      var i = 0;
      if (readByteAsInt>='0' && readByteAsInt<='9') {
        i = 10*i + readByteAsInt - '0';
      } else {
        throw new Exception($"CsvReader '{FileName}': Illegal integer: " + Environment.NewLine + GetPresentContent());
      }

      //read other digits until delimiter is reached
      while (true) {
        readByteAsInt = (int)byteArray[readIndex++];
        if (readByteAsInt>='0' && readByteAsInt<='9') {
          i = 10*i + readByteAsInt - '0';
          continue;
        }

        if (readByteAsInt!=delimiter) 
          throw new Exception($"CsvReader '{FileName}': Illegal integer: " + Environment.NewLine + GetPresentContent());

        return isMinus ? -i : i;
      }
    }


    /// <summary>
    /// Read long from UTF8 FileStream including closing delimiter.
    /// </summary>
    public long ReadLong() {
      //check for minus sign
      int readByteAsInt = (int)byteArray[readIndex++];
      var isMinus = readByteAsInt=='-';
      if (isMinus) {
        readByteAsInt = (int)byteArray[readIndex++];
      }

      //read first digit. There must be at least 1
      var l = 0L;
      if (readByteAsInt>='0' && readByteAsInt<='9') {
        l = 10*l + readByteAsInt - '0';
      } else {
        throw new Exception($"CsvReader.ReadLong() '{FileName}': Illegal character found:" + Environment.NewLine + GetPresentContent());
      }

      //read other digits until delimiter is reached
      while (true) {
        readByteAsInt = (int)byteArray[readIndex++];
        if (readByteAsInt>='0' && readByteAsInt<='9') {
          l = 10*l + readByteAsInt - '0';
          continue;
        }

        if (readByteAsInt!=delimiter)
          throw new Exception($"CsvReader.ReadLong() '{FileName}': Illegal character found" + Environment.NewLine + GetPresentContent());

        return isMinus ? -l : l;
      }
    }


    /// <summary>
    /// Read long or null from UTF8 FileStream including closing delimiter.
    /// </summary>
    public long? ReadLongNull() {
      int readByteAsInt = (int)byteArray[readIndex++];
      //check for null
      if (readByteAsInt==delimiter) {
        return null;
      }

      //check for minus sign
      var isMinus = readByteAsInt=='-';
      if (isMinus) {
        readByteAsInt = (int)byteArray[readIndex++];
      }

      //read first digit. There must be at least 1
      var l = 0L;
      if (readByteAsInt>='0' && readByteAsInt<='9') {
        l = 10*l + readByteAsInt - '0';
      } else {
        throw new Exception($"CsvReader.ReadLong() '{FileName}': Illegal character found:" + Environment.NewLine + GetPresentContent());
      }

      //read other digits until delimiter is reached
      while (true) {
        readByteAsInt = (int)byteArray[readIndex++];
        if (readByteAsInt>='0' && readByteAsInt<='9') {
          l = 10*l + readByteAsInt - '0';
          continue;
        }

        if (readByteAsInt!=delimiter)
          throw new Exception($"CsvReader.ReadLong() '{FileName}': Illegal character found" + Environment.NewLine + GetPresentContent());

        return isMinus ? -l : l;
      }
    }


    /// <summary>
    /// Read decimal from UTF8 FileStream including closing delimiter.
    /// </summary>
    public decimal ReadDecimal() {
      var tempCharsIndex = 0;
      while (true) {
        int readByteAsInt = (int)byteArray[readIndex++];
        if (readByteAsInt>=0x80) throw new Exception($"CsvReader.ReadDecimal() '{FileName}': Illegal character found:" + Environment.NewLine + GetPresentContent());

        if (readByteAsInt==delimiter) {
          var tempCharsSpan = new ReadOnlySpan<char>(tempChars, 0, tempCharsIndex);
          return Decimal.Parse(tempCharsSpan);
        }
        tempChars[tempCharsIndex++] = (char)readByteAsInt;
      }
    }


    /// <summary>
    /// Read decimal or null from UTF8 FileStream including closing delimiter.
    /// </summary>
    public decimal? ReadDecimalNull() {
      var tempCharsIndex = 0;
      while (true) {
        int readByteAsInt = (int)byteArray[readIndex++];
        if (readByteAsInt==delimiter) {
          if (tempCharsIndex==0) return null;
          var tempCharsSpan = new ReadOnlySpan<char>(tempChars, 0, tempCharsIndex);
          return Decimal.Parse(tempCharsSpan);
        }
        if (readByteAsInt>=0x80) throw new Exception($"CsvReader.ReadDecimalNull() '{FileName}': Illegal character found:" + Environment.NewLine + GetPresentContent());

        tempChars[tempCharsIndex++] = (char)readByteAsInt;
      }
    }


    /// <summary>
    /// Read UTF8 character as Unicode or null from  FileStream including closing delimiter. Throw exception if Unicode 
    /// character does not fit in 16 bits. If all Unicode characters need to be supported, use ReadString().
    /// </summary>
    public char? ReadCharNull() {
      int readByteAsInt = (int)byteArray[readIndex];
      if (readByteAsInt==CsvConfig.Delimiter) {
        readIndex++;
        return null;
      }
      return ReadChar();
    }


    /// <summary>
    /// Read UTF8 character as Unicode from  FileStream including closing delimiter. Throw exception if Unicode character does not 
    /// fit in 16 bits. If all Unicode characters need to be supported, use ReadString().
    /// </summary>
    public char ReadChar() {
      char returnChar;
      byte readByte = byteArray[readIndex++];
      if (readByte=='\\') {
        //control character
        readByte = byteArray[readIndex++];
        returnChar = readByte switch
        {
          (byte)'t' => CsvConfig.Delimiter,
          (byte)'r' => '\r',
          (byte)'n' => '\n',
          (byte)'\\' => '\\',
          _ => throw new Exception($"CsvReader.ReadChar() '{FileName}': More than 1 character found: " + Environment.NewLine + GetPresentContent()),
        };
        readByte = byteArray[readIndex++];
        if (readByte!=delimiter) throw new Exception($"CsvReader.ReadChar() '{FileName}': More than 1 character found: " +
          Environment.NewLine + GetPresentContent());
        return returnChar;

      } else if (readByte<0x80) {
        //single byte ASCII character
        returnChar = (char)readByte;
        readByte = byteArray[readIndex++];
        if (readByte!=delimiter) throw new Exception($"CsvReader.ReadChar() '{FileName}': More than 1 character found: " + 
          Environment.NewLine + GetPresentContent());
        return returnChar;

      } else {
        //UTF character
        var startIndex = readIndex-1;
        do {
          readByte = byteArray[readIndex++];
        } while (readByte!=delimiter);
        var length = Encoding.UTF8.GetChars(byteArray, startIndex, readIndex-startIndex-1, tempChars, 0);
        if (length>1) throw new Exception($"CsvReader.ReadChar() '{FileName}': More than 1 character found: " + Environment.NewLine + GetPresentContent());
        return tempChars[0];
      }
    }


    /// <summary>
    /// Reads the very first character from a new line. It also ensures that enough bytes are read from the file 
    /// for the whole line. ReadLeadingLineChar() must be called before any other ReadXxx() except ReadLine().
    /// </summary>
    public char ReadFirstLineChar() {
      var remainingBytesCount = endIndex - readIndex;
      if (remainingBytesCount<=MaxLineByteLength) {
        if (!fillBufferFromFileStream(remainingBytesCount)) throw new Exception($"CsvReader.ReadFirstLineChar() '{FileName}': Premature EOF found: " + Environment.NewLine + GetPresentContent());
      }

      char readByteAsChar = (char)byteArray[readIndex++];
      if (readByteAsChar>=0x80) throw new Exception($"CsvReader.ReadFirstLineChar() '{FileName}': Illegal character found: " + Environment.NewLine + GetPresentContent());

      return readByteAsChar;
    }


    /// <summary>
    /// Read UTF8 characters as Unicode string or null from  FileStream including closing delimiter.
    /// </summary>
    public string? ReadStringNull() {
      int readByteAsInt = (int)byteArray[readIndex];
      if (readByteAsInt==CsvConfig.Delimiter) {
        readIndex++;
        return null;
      }
      return ReadString();
    }


    /// <summary>
    /// Read UTF8 characters as Unicode string from FileStream including closing delimiter.
    /// </summary>
    public string ReadString() {
      if (byteArray[readIndex]==(byte)'\\' && byteArray[readIndex+1]==(byte)' ' && byteArray[readIndex+2]==CsvConfig.Delimiter) {
        readIndex += 3;
        return "";
      }

      var tempCharsIndex = 0;
      while (true) {
        var readByte = byteArray[readIndex++];
        var readChar = (char)readByte;

        if (readChar==delimiter) {
          if (tempCharsIndex==0) {
            throw new Exception($"CsvReader.ReadString() '{FileName}': string was null: " + Environment.NewLine + GetPresentContent());
          }
          return new string(tempChars, 0, tempCharsIndex);

        } else if (readChar=='\\') {
          readByte = byteArray[readIndex++];
          tempChars[tempCharsIndex++] =readByte switch {
            (byte)'t' => CsvConfig.Delimiter,
            (byte)'r' => '\r',
            (byte)'n' => '\n',
            (byte)'\\' => '\\',
            _ => throw new Exception($"CsvReader.ReadString() '{FileName}': Illegal control character combination '\\{readByte}' " +
                   $"found. Valid combinations are '\\{CsvConfig.Delimiter}', '\\r' and '\\n'.: " + Environment.NewLine +
                   GetPresentContent()),
          };
        } else if (readChar<0x80) {
          tempChars[tempCharsIndex++] = readChar;

        } else {
          //UTF8 character found
          var startReadIndex = readIndex - 1;
          while (true) {
            if (byteArray[readIndex++]==delimiter) {
              var readString = Encoding.UTF8.GetString(byteArray, startReadIndex, readIndex-startReadIndex-1);
              var isSlashFound = false;
              foreach (var ch in readString) {
                if (isSlashFound) {
                  isSlashFound = false;
                  switch (ch) {
                  case 't': tempChars[tempCharsIndex++] = CsvConfig.Delimiter; break;
                  case 'r': tempChars[tempCharsIndex++] = '\r'; break;
                  case 'n': tempChars[tempCharsIndex++] = '\n'; break;
                  case '\\': tempChars[tempCharsIndex++] = '\\'; break;
                    throw new Exception($"CsvReader.ReadString() '{FileName}': Illegal control character combination '\\{ch}' " +
                      $"found. Valid combinations are '\\{CsvConfig.Delimiter}', '\\r' and '\\n'.: " + Environment.NewLine +
                      GetPresentContent());
                  }
                } else {
                  if (ch=='\\') {
                    isSlashFound = true;
                  } else {
                    tempChars[tempCharsIndex++] = ch;
                  }
                }
              }
              return new string(tempChars, 0, tempCharsIndex);
            }
          }
        }
      }
    }


    /// <summary>
    /// Read date without time or null from UTF8 FileStream including closing delimiter.
    /// </summary>
    public DateTime? ReadDateNull() {
      int readByteAsInt = (int)byteArray[readIndex];
      if (readByteAsInt==CsvConfig.Delimiter) {
        readIndex++;
        return null;
      }
      return ReadDate();
    }


    /// <summary>
    /// Read date without time from UTF8 FileStream including closing delimiter.
    /// </summary>
    public DateTime ReadDate() {
      var day = (int)(byteArray[readIndex++] - '0');
      var readByteAsChar = (char)byteArray[readIndex++];
      if (readByteAsChar!='.') {
        day = day*10 + (int)(readByteAsChar - '0');
        if ((char)byteArray[readIndex++]!='.') throw new Exception($"CsvReader.ReadDate() '{FileName}': Day has more than 2 chars: " + Environment.NewLine + GetPresentContent());
      }

      var month = (int)(byteArray[readIndex++] - '0');
      readByteAsChar = (char)byteArray[readIndex++];
      if (readByteAsChar!='.') {
        month = month*10 + (int)(readByteAsChar - '0');
        if ((char)byteArray[readIndex++]!='.') throw new Exception($"CsvReader.ReadDate() '{FileName}': Month has more than 2 chars: " + Environment.NewLine + GetPresentContent());
      }

      var year = (int)(byteArray[readIndex++] - '0');
      year = 10*year + (int)(byteArray[readIndex++] - '0');
      year = 10*year + (int)(byteArray[readIndex++] - '0');
      year = 10*year + (int)(byteArray[readIndex++] - '0');

      if ((char)byteArray[readIndex++]!=CsvConfig.Delimiter) throw new Exception($"CsvReader.ReadDate() '{FileName}': delimiter not found after 4 characters for year: " + Environment.NewLine + GetPresentContent());

      return new DateTime(year, month, day);
    }


    /// <summary>
    /// Read date and time down to seconds or null from UTF8 FileStream including closing delimiter.
    /// </summary>
    public DateTime? ReadDateSecondsNull() {
      int readByteAsInt = (int)byteArray[readIndex];
      if (readByteAsInt==CsvConfig.Delimiter) {
        readIndex++;
        return null;
      }
      return ReadDateSeconds();
    }


    /// <summary>
    /// Read date and time down to seconds from UTF8 FileStream including closing delimiter.
    /// </summary>
    public DateTime ReadDateSeconds() {
      var day = (int)(byteArray[readIndex++] - '0');
      var readByteAsChar = (char)byteArray[readIndex++];
      if (readByteAsChar!='.') {
        day = day*10 + (int)(readByteAsChar - '0');
        if ((char)byteArray[readIndex++]!='.') throw new Exception($"CsvReader.ReadDateSeconds() '{FileName}': Day has more than 2 chars: " + Environment.NewLine + GetPresentContent());
      }

      var month = (int)(byteArray[readIndex++] - '0');
      readByteAsChar = (char)byteArray[readIndex++];
      if (readByteAsChar!='.') {
        month = month*10 + (int)(readByteAsChar - '0');
        if ((char)byteArray[readIndex++]!='.') throw new Exception($"CsvReader.ReadDateSeconds() '{FileName}': Month has more than 2 chars: " + Environment.NewLine + GetPresentContent());
      }

      var year = (int)(byteArray[readIndex++] - '0');
      year = 10*year + (int)(byteArray[readIndex++] - '0');
      year = 10*year + (int)(byteArray[readIndex++] - '0');
      year = 10*year + (int)(byteArray[readIndex++] - '0');
      if ((char)byteArray[readIndex++]!=' ') throw new Exception($"CsvReader.ReadDateSeconds() '{FileName}': ' ' missing after date: " + Environment.NewLine + GetPresentContent());

      var timeSpan = ReadTime();

      return new DateTime(year, month, day) + timeSpan;
    }


    /// <summary>
    /// Read time down to seconds or null from UTF8 FileStream including closing delimiter.
    /// </summary>
    public TimeSpan? ReadTimeNull() {
      int readByteAsInt = (int)byteArray[readIndex];
      if (readByteAsInt==CsvConfig.Delimiter) {
        readIndex++;
        return null;
      }
      return ReadTime();
    }


    /// <summary>
    /// Read time down to seconds from UTF8 FileStream including closing delimiter.
    /// </summary>
    public TimeSpan ReadTime() {
      // 0: 0: 0 => "0"
      // 0: 0: 1 => "0:0:1"
      // 0: 1: 1 => "0:1:1"
      // 1: 1: 1 => "1:1:1"
      //23: 0: 0 => "23"
      //23:59: 0 => "23:59"
      //23:59:59 => "23:59:59"
      var hour = (int)(byteArray[readIndex++] - '0');
      var readByteAsChar = (char)byteArray[readIndex++];
      if (readByteAsChar==CsvConfig.Delimiter) return TimeSpan.FromHours(hour);

      if (readByteAsChar!=':') {
        hour = hour*10 + (int)(readByteAsChar - '0');
        readByteAsChar = (char)byteArray[readIndex++];
        if (readByteAsChar==CsvConfig.Delimiter) return TimeSpan.FromHours(hour);

        if (readByteAsChar!=':') throw new Exception($"CsvReader.ReadTime() '{FileName}': Hour has more than 2 chars: " + Environment.NewLine + GetPresentContent());
      }

      var minute = (int)(byteArray[readIndex++] - '0');
      readByteAsChar = (char)byteArray[readIndex++];
      if (readByteAsChar==CsvConfig.Delimiter) return new TimeSpan(hour, minute, 0);
      
      if (readByteAsChar!=':') {
        minute = minute*10 + (int)(readByteAsChar - '0');
        readByteAsChar = (char)byteArray[readIndex++];
        if (readByteAsChar==CsvConfig.Delimiter) return new TimeSpan(hour, minute, 0);
        
        if (readByteAsChar!=':') throw new Exception($"CsvReader.ReadTime() '{FileName}': Minute has more than 2 chars: " + Environment.NewLine + GetPresentContent());
      }

      var second = (int)(byteArray[readIndex++] - '0');
      readByteAsChar = (char)byteArray[readIndex++];
      if (readByteAsChar==CsvConfig.Delimiter) return new TimeSpan(hour, minute, second);

      if (readByteAsChar!=':') {
        second = second*10 + (int)(readByteAsChar - '0');
        readByteAsChar = (char)byteArray[readIndex++];
        if (readByteAsChar==CsvConfig.Delimiter) return new TimeSpan(hour, minute, second);
      }

      throw new Exception($"CsvReader.ReadTime() '{FileName}': Second has more than 2 chars: " + Environment.NewLine + GetPresentContent());
    }


    /// <summary>
    /// Read DateTime as Ticks or null from UTF8 FileStream including closing delimiter.
    /// </summary>
    public DateTime? ReadDateTimeTicksNull() {
      int readByteAsInt = (int)byteArray[readIndex];
      if (readByteAsInt==CsvConfig.Delimiter) {
        readIndex++;
        return null;
      }
      var ticks = ReadLong();
      return new DateTime(ticks);
    }


    /// <summary>
    /// Read DateTime as Ticks from UTF8 FileStream including closing delimiter.
    /// </summary>
    public DateTime ReadDateTimeTicks() {
      var ticks = ReadLong();
      return new DateTime(ticks);
    }


    /// <summary>
    /// Read TimeSpan as Ticks or null from UTF8 FileStream including closing delimiter.
    /// </summary>
    public TimeSpan? ReadTimeSpanTicksNull() {
      int readByteAsInt = (int)byteArray[readIndex];
      if (readByteAsInt==CsvConfig.Delimiter) {
        readIndex++;
        return null;
      }
      var ticks = ReadLong();
      return new TimeSpan(ticks);
    }


    /// <summary>
    /// Read TimeSpan as Ticks from UTF8 FileStream including closing delimiter.
    /// </summary>
    public TimeSpan ReadTimeSpanTicks() {
      var ticks = ReadLong();
      return new TimeSpan(ticks);
    }


    /// <summary>
    /// reads one complete line as string. ReadLine() should be avoided, because of the string creation overhead.
    /// </summary>
    public string ReadLine() {
      var remainingBytesCount = endIndex - readIndex;
      if (remainingBytesCount<=MaxLineByteLength) {
        if (!fillBufferFromFileStream(remainingBytesCount)) throw new Exception($"CsvReader.ReadLine() '{FileName}': Premature EOF found: " + Environment.NewLine + GetPresentContent());
      }

      var tempCharsIndex = 0;
      var startReadIndex = readIndex;
      while (true) {
        var readByteAsChar = (char)byteArray[readIndex++];
        if (readByteAsChar==0x0D) { //carriage return) {
          if (byteArray[readIndex++]==0x0A) { //line feed) {
            return new string(tempChars, 0, tempCharsIndex);
          } else {
            throw new Exception($"CsvReader.ReadLine() '{FileName}': Line feed missing after carriage return: " + Environment.NewLine + GetPresentContent());
          }
        }
        if (readByteAsChar<0x80) {
          tempChars[tempCharsIndex++] = readByteAsChar;
        } else {
          while (true) {
            readByteAsChar = (char)byteArray[readIndex++];
            if (readByteAsChar==0x0D) { //carriage return) {
              if (byteArray[readIndex++]==0x0A) { //line feed) {
                return Encoding.UTF8.GetString(byteArray, startReadIndex, readIndex-startReadIndex-2);
              } else {
                throw new Exception($"CsvReader.ReadLine() '{FileName}': Line feed missing after carriage return: " + Environment.NewLine + GetPresentContent());
              }
            }
          }
        }
      }
    }


    const int maxCharsDisplayed = 150;//maximal number of characters displayed before error location. The same number of characters
                                      //get maximally displayed after the error location. Only 1 complete line before the error
                                      //is displayed, which is normally more restrictive then maxCharsDisplayed.


    /// <summary>
    /// Returns some characters around the position presently red in the buffer
    /// </summary>
    /// <returns></returns>
    public string GetPresentContent() {
      if (readIndex>endIndex) {
        return $"Cannot display part of byteArray, readIndex {readIndex} should be smaller than endIndex {endIndex}.";
      }
      var maxBytesDisplayed = maxCharsDisplayed*4;
      int fromPos = Math.Max(0, readIndex - maxBytesDisplayed);
      //fromPos = GetCharStart(ref byteArray, fromPos);
      int toPos = Math.Min(readIndex + maxBytesDisplayed, endIndex);
      //toPos = GetCharEnd(ref byteArray, toPos, lastReadByte);
      //var splitPos = GetCharStart(ref byteArray, readIndex);
      var splitPos = readIndex;
      var startString = UTF8Encoding.UTF8.GetString(byteArray, fromPos, splitPos-fromPos).Replace(CsvConfig.Delimiter, '|');
      var isLineFeedFound = false;
      var linesCount = 0;
      int startIndex = startString.Length-1;
      var limit = Math.Max(0, startString.Length-maxCharsDisplayed);
      for (; startIndex >= limit; startIndex--) {
        var c = startString[startIndex];
        if (isLineFeedFound) {
          if (c==Environment.NewLine[0]) {
            linesCount++;
            if (linesCount==2) {
              startIndex +=1;//should be +2, but another +1 is done in startString = startString[(startIndex+1)..]
              break;
            }
          }
          isLineFeedFound = false;
        } else {
          isLineFeedFound = c==Environment.NewLine[1];
        }
      }
      startString = startString[(startIndex+1)..];//correct +1, because when for loop terminates normally, it has subtracted 1 
                                                  //once too often
      var endString = UTF8Encoding.UTF8.GetString(byteArray, splitPos, toPos-splitPos+1).Replace(CsvConfig.Delimiter, '|');
      isLineFeedFound = false;
      linesCount = 0;
      var end1Index = 0;
      limit = Math.Min(maxCharsDisplayed, endString.Length);
      for (; end1Index<limit; end1Index++) {
        var c = endString[end1Index];
        if (isLineFeedFound) {
          if (c==Environment.NewLine[1]) {
            linesCount++;
            if (linesCount==2) {
              end1Index -= 1;//actually, 2 should be subtracted, but it should also be endString = endString[..(endIndex+1)];, which
                            // gives a problem when for loop terminates normally and endIndex is 1 too high
              break;
            }
          }
          isLineFeedFound = false;
        } else {
          isLineFeedFound = c==Environment.NewLine[0];
        }
      }
      endString = endString[..(end1Index)];
      return startString + '^' + endString;
    }
    //public string GetPresentContent() {
    //  int fromPos;
    //  if (readIndex>bytesDisplayed) {
    //    fromPos = readIndex - bytesDisplayed;
    //  } else {
    //    fromPos = 0;
    //  }
    //  var presentPos = readIndex - fromPos;

    //  int toPos = readIndex + bytesDisplayed;
    //  toPos = Math.Min(toPos, endIndex);
    //  var byteString1 = UTF8Encoding.UTF8.GetString(byteArray, fromPos, readIndex-fromPos).Replace(CsvConfig.Delimiter, '|');
    //  var byteString2 = UTF8Encoding.UTF8.GetString(byteArray, readIndex, toPos-fromPos+1).Replace(CsvConfig.Delimiter, '|');
    //  var byteString3 = UTF8Encoding.UTF8.GetString(byteArray, fromPos, toPos-fromPos+1).Replace(CsvConfig.Delimiter, '|');
    //  return byteString1 + '^' + byteString2;
    //}
    /**********************************************************************************
private static void testUtf8() {
  var testString = "😍1🤐ä😶ã😷😎😱💀👻";
  var testBytes = UTF8Encoding.UTF8.GetBytes(testString);
  string hex = BitConverter.ToString(testBytes);
  //0  1  2  3  4  5  6  7  8  9  10 11 12 13 14 15 16 17 18 19
  //😍          1  🤐         ä     😶          ã     😷         😎          😱         💀          👻
  //F0-9F-98-8D 31 F0-9F-A4-90 C3-A4 F0-9F-98-B6 C3-A3 F0-9F-98-B7 F0-9F-98-8E-F0-9F-98-B1-F0-9F-92-80-F0-9F-91-BB
  var p0 = GetCharStart(ref testBytes, 0);
  var p1 = GetCharStart(ref testBytes, 1);
  var p2 = GetCharStart(ref testBytes, 2);
  var p3 = GetCharStart(ref testBytes, 3);
  var p4 = GetCharStart(ref testBytes, 4);
  var p5 = GetCharStart(ref testBytes, 5);
  var p6 = GetCharStart(ref testBytes, 6);
  var p7 = GetCharStart(ref testBytes, 7);
  var p8 = GetCharStart(ref testBytes, 8);
  var p9 = GetCharStart(ref testBytes, 9);
  var p10 = GetCharStart(ref testBytes, 10);
  var p11 = GetCharStart(ref testBytes, 11);
  var p12 = GetCharStart(ref testBytes, 12);
  var p13 = GetCharStart(ref testBytes, 13);
  var p14 = GetCharStart(ref testBytes, 14);
  var p15 = GetCharStart(ref testBytes, 15);
  var p16 = GetCharStart(ref testBytes, 16);
  var p17 = GetCharStart(ref testBytes, 17);
  var p18 = GetCharStart(ref testBytes, 18);
  var p19 = GetCharStart(ref testBytes, 19);

  var s0 = GetPresentContent(ref testBytes, 21, 100, testBytes.Length-1);
  for (int byteIndex = 0; byteIndex < testBytes.Length; byteIndex++) {
    //0  1  2  3  4  5  6  7  8  9  10 11 12 13 14 15 16 17 18 19 20 21
    //😍          1  🤐         ä     😶          ã     😷         😎          😱         💀          👻
    var s1 = GetPresentContent(ref testBytes, byteIndex, 100, testBytes.Length-1);
  }

  var crlf = Environment.NewLine;
  testString = "😍1" + crlf + "🤐Ö" + crlf + "😶ã" + crlf + "😷😎" + crlf + "😱💀" + crlf + "👻";
  testBytes = UTF8Encoding.UTF8.GetBytes(testString);
  hex = BitConverter.ToString(testBytes);
  //0  1  2  3  4  5  6  7  8  9  10 11 12 13 14 15 16 17 18 19 20 21 22 23 24 25 26 27 28 29 30 31 32 33 34 35 36 37 38 39
  //😍          1 crlf  🤐          Ö     crlf  😶          ã     crlf  😷         😎          crlf  😱         💀          crlf  👻
  //F0-9F-98-8D-31-0D-0A-F0-9F-A4-90-C3-96-0D-0A-F0-9F-98-B6-C3-A3-0D-0A-F0-9F-98-B7-F0-9F-98-8E-0D-0A-F0-9F-98-B1-F0-9F-92-80-0D-0A-F0-9F-91-BB
  for (int byteIndex = 0; byteIndex < testBytes.Length; byteIndex++) {
    var s1 = GetPresentContent(ref testBytes, byteIndex, 100, testBytes.Length-1);
  }
}


public static int GetCharStart(ref byte[] arr, int index) {
  var step = index>3 ? -1 : 1;
  while ((arr[index] & 0xC0)==0x80) {
    index += step;
  }
  return index;
}


public static int GetCharEnd(ref byte[] arr, int index, int end) {
  var step = index<end-3 ? 1 : -1;
  while ((arr[index] & 0xC0)==0x80) {
    index += step;
  }
  return index;
}


public const char CsvConfigDelimiter = '\t';



public static string GetPresentContent(ref byte[] byteArray, int readIndex, int maxCharsDisplayed, int lastReadByte) {
  var maxBytesDisplayed = maxCharsDisplayed*4;
  int fromPos = Math.Max(0, readIndex - maxBytesDisplayed);
  //fromPos = GetCharStart(ref byteArray, fromPos);
  int toPos = Math.Min(readIndex + maxBytesDisplayed, lastReadByte);
  //toPos = GetCharEnd(ref byteArray, toPos, lastReadByte);
  //var splitPos = GetCharStart(ref byteArray, readIndex);
  var splitPos = readIndex;
  var ss = byteArray[fromPos..(splitPos-fromPos)];
  var startString = UTF8Encoding.UTF8.GetString(byteArray, fromPos, splitPos-fromPos).Replace(CsvConfigDelimiter, '|');
  var isLineFeedFound = false;
  var linesCount = 0;
  int startIndex = startString.Length-1;
  var limit = Math.Max(0, startString.Length-maxCharsDisplayed);
  for (; startIndex >= limit; startIndex--) {
    var c = startString[startIndex];
    if (isLineFeedFound) {
      if (c==Environment.NewLine[0]) {
        linesCount++;
        if (linesCount==2) {
          startIndex +=1;//should be +2, but another +1 is done in startString = startString[(startIndex+1)..]
          break;
        }
      }
      isLineFeedFound = false;
    } else {
      isLineFeedFound = c==Environment.NewLine[1];
    }
  }
  startString = startString[(startIndex+1)..];//correct +1, because when for loop terminates normally, it has subtracted 1 
                                              //once too often
  var sss = byteArray[splitPos..(toPos+1)];
  var endString = UTF8Encoding.UTF8.GetString(byteArray, splitPos, toPos-splitPos+1).Replace(CsvConfigDelimiter, '|');
  isLineFeedFound = false;
  linesCount = 0;
  var endIndex = 0;
  limit = Math.Min(maxCharsDisplayed, endString.Length);
  for (; endIndex<limit; endIndex++) {
    var c = endString[endIndex];
    if (isLineFeedFound) {
      if (c==Environment.NewLine[1]) {
        linesCount++;
        if (linesCount==2) {
          endIndex -= 1;//actually, 2 should be subtracted, but it should also be endString = endString[..(endIndex+1)];, which
                        // gives a problem when for loop terminates normally and endIndex is 1 too high
          break;
        }
      }
      isLineFeedFound = false;
    } else {
      isLineFeedFound = c==Environment.NewLine[0];
    }
  }
  endString = endString[..(endIndex)];
  return startString + '^' + endString;
}

****************************************************************************/


    #endregion
  }
}
