/**************************************************************************************

SampleDataConsole Program
=========================

This console program shows how the classes defined in SampleDataModel can be used.

Written in 2021 by Jürgpeter Huber 
Contact: PeterCode at Peterbox dot com

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/
using System;
using System.IO;
using DataModelSamples;
using StorageLib;


namespace SampleDataConsole {


  public class Program {


    public static void Main(string[] args) {

      var directoryInfo = new DirectoryInfo("SampleDataCsv");
      if (directoryInfo.Exists) {
        directoryInfo.Delete(recursive: true);
        directoryInfo.Refresh();
      }

      directoryInfo.Create();

      var dc = new DC(new CsvConfig(directoryInfo.FullName, reportException: reportException));
      Console.WriteLine("StorageLib Sample Data");
      Console.WriteLine("======================");
      Console.WriteLine();
      Console.WriteLine("Directory: " + directoryInfo.FullName);
      try {

        #region DataModel Data Types
        //      --------------------

        var dataModelDataTypes = new DataModelDataTypes(
          date: new DateTime(9999, 12, 21),
          time: new TimeSpan(23, 59, 59),
          dateMinutes: new DateTime(9999, 12, 21, 23, 59, 0),
          dateSeconds: new DateTime(9999, 12, 21, 23, 59, 59),
          dateTimeTicks: DateTime.MaxValue,
          timeSpanTicks: TimeSpan.MaxValue,
          decimal_: decimal.MaxValue,
          decimal2: 123456789.12m,
          decimal4: 123456789.1234m,
          decimal5: 123456789.12345m,
          bool_: true,
          int_: int.MaxValue,
          long_: long.MaxValue,
          char_: char.MaxValue,
          string_: "A String with Unicode char smiley ☺");
        Console.WriteLine();
        Console.WriteLine("DataModelDataTypes: " + dataModelDataTypes);
        #endregion
      } finally {
        dc.Dispose();
      }
    }


    private static void reportException(Exception exception) {
      Console.WriteLine("Exception occured: " + exception);
    }
  }
}
