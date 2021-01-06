using System;
using System.Collections.Generic;
using System.Text;

namespace StorageLib {


  /// <summary>
  /// Extension methods to generate compact strings for decimals, dates, etc, using as few bytes as possible in CSV files
  /// </summary>
  public static class ToStringExtensions {

    #region Boolean
    //      -------

    /// <summary>
    /// Converts true to "y" and false to "n".
    /// </summary>
    public static string ToYNString(this bool value) {
      if (value)
        return "y";

      return "n";
    }


    /// <summary>
    /// Converts null to "", true to "y" and false to "n".
    /// </summary>
    public static string ToYNNullString(this bool? value) {
      if (!value.HasValue) return "";

      if (value.Value) return "y";

      return "n";
    }
    #endregion


    #region decimal
    //      -------

    /// <summary>
    /// Converts a decimal to a string, without trailing 0s or decimal point
    /// </summary>
    public static string ToCompactString(this decimal value) {
      string returnString = value.ToString(".######");
      var newLength = returnString.Length-1;
      if (newLength<0) return "0";

      if (returnString[newLength]!='0') return returnString;

      if (!returnString.Contains(".")) return returnString;

      while (newLength>0) {
        newLength--;
        if (returnString[newLength]!='0') break;
      }

      if (returnString[newLength]=='.') newLength--;

      if (newLength<0) return "0";

      return returnString.Substring(0, newLength+1);
    }


    /// <summary>
    /// Converts a decimal to a string, without trailing 0s or decimal point
    /// </summary>
    public static string? ToCompactString(this decimal? value) {
      if (value.HasValue) return value.Value.ToCompactString();

      return null;
    }


    /// <summary>
    /// Converts a decimal to a string, without trailing 0s or decimal point
    /// </summary>
    public static string ToCompactString(this decimal value, int decimals) {
      value = Math.Round(value, decimals);
      return value.ToCompactString();
    }


    /// <summary>
    /// Converts a decimal to a string, without trailing 0s or decimal point
    /// </summary>
    public static string? ToCompactString(this decimal? value, int decimals) {
      if (value.HasValue) return value.Value.ToCompactString(decimals);

      return null;
    }
    #endregion


    #region Date
    //      ----

    /// <summary>
    /// Converts a DateTime to a string with only the date and no leading zeros for day and month
    /// </summary>
    public static string ToCompactDateString(this DateTime date) {
      string returnString = date.Date.ToString(CsvConfig.DateFormat);
      if (returnString[3]=='0') returnString = returnString.Substring(0, 3) + returnString[4..];

      if (returnString[0]=='0') returnString = returnString[1..];

      return returnString;
    }


    /// <summary>
    /// Converts a DateTime to a string with only the date and no leading zeros for day and month. A null value becomes an
    /// empty string.
    /// </summary>
    public static string ToCompactDateString(this DateTime? date) {
      if (!date.HasValue) return "";

      return date.Value.ToCompactDateString();
    }
    #endregion
  }
}
