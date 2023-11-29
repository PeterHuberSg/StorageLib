/**************************************************************************************

StorageLib.CharArrayExtensions
==============================

Extension methods to read from and write to char[]

Written in 2020 by Jürgpeter Huber 
Contact: https://github.com/PeterHuberSg/StorageLib

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/
using System.Text;


namespace StorageLib {


  /// <summary>
  /// Extension methods to read from and write to char[]
  /// </summary>
  public static class CharArrayExtensions {

    #region Integer
    //      -------

    /// <summary>
    /// Write integer i at position index in charArray
    /// </summary>
    public static void Write(this char[] charArray, int i, ref int index){
      int start;
      if (i<0) {
        charArray[index++] = '-';
        start = index;
        //since -int.MinValue is bigger than int.MaxValue, i=-i does not work of int.MinValue.
        //therefore write 1 character first and guarantee that i>int.MinValue
        charArray[index++] = (char)(-(i % 10) + '0');
        i /= 10;
        if (i==0) return;
        i = -i;
      } else {
        start = index;
      }

      while (i>9) {
        charArray[index++] = (char)((i % 10) + '0');
        i /= 10;
      }
      charArray[index++] = (char)(i + '0');
      var end = index-1;
      while (end>start) {
        var temp = charArray[end];
        charArray[end--] = charArray[start];
        charArray[start++] = temp;
      }
    }


    /// <summary>
    /// Parse charArray into int
    /// </summary>
    public static int ReadInt(
      this char[] charArray, 
      ref int index, 
      int length,
      int lineStart, 
      int lineLength,
      string fieldName,
      StringBuilder errorStringBuilder) 
    {
      var startIndex = index;
      if (index>=length) {
        if (index==length) {
          errorStringBuilder.AppendLine($"{fieldName} should be int, but was empty '' in line: '{new string(charArray[lineStart..lineLength])}'.");
        } else {
          errorStringBuilder.AppendLine($"{fieldName} invalid index {index}, field ends at {length} in line: '{new string(charArray[lineStart..lineLength])}'.");
        }
        return int.MinValue;
      }
      var i = 0;
      var isMinus = charArray[index]=='-';
      if (isMinus) {
        index++;
      }
      while (index<length) {
        var inChar = charArray[index++];
        if (inChar<'0' || inChar>'9') {
          errorStringBuilder.AppendLine($"{fieldName} should be int, but contained illegal character '{inChar}' in field '{new string(charArray[startIndex..length])}' and line: '{new string(charArray[lineStart..lineLength])}'.");
          return int.MinValue;
        }
        i = 10*i + inChar - '0';
      }
      if (isMinus) {
        return -i;
      } else {
        return i;
      }
    }
    #endregion

  }
}
