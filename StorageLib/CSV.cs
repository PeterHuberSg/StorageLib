/**************************************************************************************

StorageLib.Csv
==============

Contains static helper methods for CommaSeparateValue file processing, like reading data from CSV files

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
using System.Linq;
using System.Text;



namespace StorageLib {


  /// <summary>
  /// Contains static helper methods for CommaSeparateValue file processing, like reading data from CSV files, although
  /// CsvReader has the faster methods.
  /// </summary>
  public static class Csv {

    #region ByteBuffer constants
    //      --------------------

    /*
    CsvReader and CsvWriter each read from or write to a file CsvConfig.BufferSize bytes at a time. CsvReader and 
    CsvWriter use each a byteArray which is 25% bigger than BufferSize. The maximal length of a line that can be read
    or written is therefore 25% of BufferSize. This arrangement guarantees there is always enough space in the buffer 
    for reading or writing entire lines.
    */

    /// <summary>
    /// UTF8 can use up to 4 bytes for 1 character
    /// </summary>
    public const int Utf8BytesPerChar = 4;

    /// <summary>
    /// The byteArray in CsvReader or CsvWriter should be 25% bigger than BufferSize
    /// </summary>
    public const int ByteBufferToReserveRatio = 4;

    /// <summary>
    /// CsvConfig.BufferSize should be at least 16 bigger than the number of characters in the longest expected line to write
    /// </summary>
    public const int LineToBufferRatio = Utf8BytesPerChar * ByteBufferToReserveRatio;
    #endregion


    #region Parsing
    //      -------

    #region Integer
    //      -------

    /// <summary>
    /// Parse field into int
    /// </summary>
    public static int ParseInt(string fieldName, string field, string line, StringBuilder errorStringBuilder) {
      if (int.TryParse(field, out var value))
        return value;

      errorStringBuilder.AppendLine(fieldName + " should be int, but was '" + field + "' in line: '" + line + "'.");
      return int.MinValue;
    }


    /// <summary>
    /// Parse field into int
    /// </summary>
    public static int ParseInt(String fieldName, String line, int lineLength, ref int charIndex, char delimiter, ref Boolean isLineError, StringBuilder errorStringBuilder) {
      if (isLineError) return int.MinValue;

      var number = 0;
      var isFoundNumber = false;
      var isNegative = 1;
      while (charIndex<lineLength) {
        var c = line[charIndex++];
        if (c>='0' && c<='9') {
          isFoundNumber = true;
          number = number * 10 + (c - '0');
        } else if (!isFoundNumber && c=='-') {
          isNegative = -1;
        } else if (c==delimiter && isFoundNumber) {
          return isNegative * number;
        } else {
          break;
        }
      };

      isLineError = true;
      errorStringBuilder.AppendLine(fieldName + " should be int in line '" + line + "' at position " + charIndex + ".");
      return int.MinValue;
    }



    /// <summary>
    /// Parse field into int?
    /// </summary>
    public static int? ParseIntNull(string fieldName, string field, string line, StringBuilder errorStringBuilder) {
      if (field.Length==0)
        return null;

      if (int.TryParse(field, out var value))
        return value;

      errorStringBuilder.AppendLine(fieldName + " should be int?, but was '" + field + "' in line: '" + line + "'.");
      return null;
    }
    #endregion


    #region Long
    //      ----

    /// <summary>
    /// Parse field into long
    /// </summary>
    public static long ParseLong(string fieldName, string field, string line, StringBuilder errorStringBuilder) {
      if (long.TryParse(field, out var value))
        return value;

      errorStringBuilder.AppendLine(fieldName + " should be long, but was '" + field + "' in line: '" + line + "'.");
      return long.MinValue;
    }
    #endregion


    #region Bool
    //      ----

    /// <summary>
    /// Parse field with 'y' or 'n' into bool
    /// </summary>
    public static bool ParseBoolYN(string fieldName, string field, string line, StringBuilder errorStringBuilder) {
      if (field=="y")
        return true;
      if (field=="n")
        return false;

      errorStringBuilder.AppendLine(fieldName + " should be 'y' or 'n' (boolean), but was '" + field + "' in line: '" + line + "'.");
      return false;
    }


    /// <summary>
    /// Parse y/n field into bool?
    /// </summary>
    public static bool? ParseBoolYNNull(string fieldName, string field, string line, StringBuilder errorStringBuilder) {
      if (field=="")
        return null;
      if (field=="y")
        return true;
      if (field=="n")
        return false;

      errorStringBuilder.AppendLine(fieldName + " should be 'y' or 'n' (boolean), but was '" + field + "' in line: '" + line + "'.");
      return null;
    }
    #endregion


    #region DateTime
    //      --------


    /// <summary>
    /// Parse field into DateTime
    /// </summary>
    public static DateTime ParseDateTime(string fieldName, string field, string line, StringBuilder errorStringBuilder) {
      if (DateTime.TryParse(field, out var value))
        return value;

      errorStringBuilder.AppendLine(fieldName + " should be DateTime, but was '" + field + "' in line: '" + line + "'.");
      return DateTime.MinValue;
    }


    /// <summary>
    /// Parse field into DateTime?
    /// </summary>
    public static DateTime? ParseDateTimeNull(string fieldName, string field, string line, StringBuilder errorStringBuilder) {
      if (field.Length==0)
        return null;

      if (DateTime.TryParse(field, out var value))
        return value;

      errorStringBuilder.AppendLine(fieldName + " should be DateTime, but was '" + field + "' in line: '" + line + "'.");
      return DateTime.MinValue;
    }


    enum dateStateEnum {
      day,
      month,
      year,
    }

    /// <summary>
    /// Parse field into DateTime
    /// </summary>
    public static DateTime ParseDateTime(String fieldName, String line, Int32 lineLength, ref Int32 charIndex, Char delimiter, ref Boolean isLineError, StringBuilder errorStringBuilder) {
      if (isLineError) return default;

      var day = 0;
      var month = 0;
      var year = 0;
      var dateState = dateStateEnum.day;
      var digitCount = 0;
      var dotCount = 0;
      while (charIndex<lineLength) {
        var c = line[charIndex++];
        if (c>='0' && c<='9') {
          digitCount++;
          if (dateState==dateStateEnum.day) {
            day = day * 10 + (c - '0');
          } else if (dateState==dateStateEnum.month) {
            month = month * 10 + (c - '0');
          } else {
            year = year * 10 + (c - '0');
          }
        } else if (c=='.') {
          if (digitCount<1 || digitCount>2) {
            break;
          }
          digitCount = 0;
          dotCount++;
          dateState++;
        } else if (c==delimiter) {
          if (dotCount!=2 || (digitCount!=4 && digitCount!=2) || day==0 || month==0) {
            break;
          }
          if (digitCount==2) {
            year += 2000;
          }
          try {
            return new DateTime(year, month, day);

          } catch (Exception) {
            break;
          }
        } else {
          break;
        }
      };

      isLineError = true;
      errorStringBuilder.AppendLine(fieldName + " should be DateTime in line: '" + line + "' at position " + charIndex + ".");
      return default;
    }
    #endregion


    #region Decimal
    //      -------

    /// <summary>
    /// Parse field into Decimal
    /// </summary>
    public static Decimal ParseDecimal(string fieldName, string field, string line, StringBuilder errorStringBuilder) {
      if (Decimal.TryParse(field, out var value))
        return value;

      errorStringBuilder.AppendLine(fieldName + " should be Decimal, but was '" + field + "' in line: '" + line + "'.");
      return Decimal.MinValue;
    }


    /// <summary>
    /// Parse field into Decimal?
    /// </summary>
    public static Decimal? ParseDecimalNull(string fieldName, string field, string line, StringBuilder errorStringBuilder) {
      if (field.Length==0)
        return null;

      if (Decimal.TryParse(field, out var value))
        return value;

      errorStringBuilder.AppendLine(fieldName + " should be Decimal?, but was '" + field + "' in line: '" + line + "'.");
      return null;
    }


    #endregion


    #region String
    //      ------

    /// <summary>
    /// Reads from line at position charIndex until delimiter and returns that string
    /// </summary>
    public static String ParseString(String fieldName, String line, Int32 lineLength, ref Int32 charIndex, Char delimiter, ref Boolean isLineError, StringBuilder errorStringBuilder) {
      if (isLineError) return "";

      var startCharIndex = charIndex;
      while (charIndex<lineLength) {
        var c = line[charIndex++];
        if (c==delimiter) {
          if (charIndex==startCharIndex) return "";

          return line.Substring(startCharIndex, charIndex-startCharIndex - 1);
        }
      };

      isLineError = true;
      errorStringBuilder.AppendLine(fieldName + " is empty at position " + charIndex + " in line: '" + line + "'.");
      return "";
    }
    #endregion
    #endregion


    #region AreEqual
    //      --------

    //When reading back a null string from a CVS file, it becomes an empty string.

    /// <summary>
    /// Tests if both strings are equal. If one is null and the other is an empty string, they are considered equal, because 
    /// when reading back a null string from a CVS file, it becomes an empty string.
    /// </summary>
    public static bool AreEqual(string? thisString, string? thatString) {
      return (thisString==thatString) ||
        ((thisString==null || thisString=="") && (thatString==null || thatString==""));
    }
    #endregion


    #region Backup
    //      ------

    public static string Backup(CsvConfig csvConfig, DateTime nowDate) {
      if (csvConfig.BackupPath is null) return "";

      if (csvConfig.BackupPeriodicity<1 || csvConfig.BackupCopies<1) throw new Exception("CSV.Backup: " +
        $"Both BackupPeriodicity {csvConfig.BackupPeriodicity} and BackupCopies {csvConfig.BackupCopies} must be greater" + 
        $"equal 1 for backup to {csvConfig.BackupPath}.");

      var backupDir = new DirectoryInfo(csvConfig.BackupPath);
      if (!backupDir.Exists) 
        throw new Exception($"Backup directory does not exist: {csvConfig.BackupPath}");
      

      var existingDirectories = backupDir.GetDirectories("csv*").OrderBy(d=>d.Name).ToList();
      if (existingDirectories.Count>0) {
        var newestDirectory = existingDirectories[^1];
        ReadOnlySpan<char> newestDirectoryName = newestDirectory.Name;
        var lastBackupDate = new DateTime(
        int.Parse(newestDirectory.Name[3..7]),
        int.Parse(newestDirectory.Name[7..9]),
        int.Parse(newestDirectory.Name[9..11]));
        var nextBackupDate = lastBackupDate.AddDays(csvConfig.BackupPeriodicity);
        if (nowDate<nextBackupDate) return 
            $"Last backup: {lastBackupDate:dd.MM.yy}; Today: {nowDate:dd.MM.yy}; Next backup: {nextBackupDate:dd.MM.yy};";
      }

      var result = "";
      var deleteCount = existingDirectories.Count - csvConfig.BackupCopies;
      for (int deleteIndex = 0; deleteIndex < deleteCount; deleteIndex++) {
        existingDirectories[deleteIndex].Delete(true);
        result += "Directory " + existingDirectories[deleteIndex].FullName + " deleted" + Environment.NewLine;
      }

      var newDirectory = backupDir.CreateSubdirectory("csv" + nowDate.ToString("yyyyMMdd"));
      var newPath = newDirectory.FullName + '\\';
      var activeDir = new DirectoryInfo(csvConfig.DirectoryPath);
      var fileCount = 0;
      foreach (var file in activeDir.GetFiles("*.csv")) {
        file.CopyTo(newPath + file.Name);
        fileCount++;
      }
      result += $"{fileCount} files copied from {csvConfig.DirectoryPath} {Environment.NewLine}to {newDirectory.FullName}.";

      return result;
    }
    #endregion


    #region other helpers
    //      -------------

    //public static bool SplitLine(string line, char delimiter, int length, StringBuilder errorStringBuilder, string typeName, out string[] fields) {
    //  fields = line.Split(delimiter);
    //  if (fields.Length-1!=length) {
    //    errorStringBuilder.AppendLine(typeName + " should have " + length + " fields, but had " + (fields.Length-1) + ": '" + line + "'.");
    //    return false;
    //  }
    //  return true;
    //}


    /// <summary>
    /// Adds path as defined in csvConfig to name and adds '.csv' as extension
    /// </summary>
    /// <param name="csvConfig"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    internal static string ToPathFileName(CsvConfig csvConfig, string name) {
      return csvConfig.DirectoryPath + @"\"  + name + ".csv";
    }


    /// <summary>
    /// Combines each string in headers into one string
    /// </summary>
    public static string ToCsvHeaderString(string[] headers, char delimiter) {
      string csvHeaderString = "";
      foreach (string header in headers) {
        csvHeaderString += header + delimiter;
      }
      return csvHeaderString;
    }
    #endregion
  }
}
