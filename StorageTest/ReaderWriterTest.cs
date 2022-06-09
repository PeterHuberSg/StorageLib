using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageLib;
using TestContext;


namespace StorageTest {


  [TestClass]
  public class ReaderWriterTest {

    [TestMethod]
    public void TestReaderWriter() {
      var directoryInfo = new DirectoryInfo("TestCsv");
      directoryInfo.Refresh();
      if (directoryInfo.Exists) {
        directoryInfo.Delete(recursive: true);
        directoryInfo.Refresh();
      }

      directoryInfo.Create();

      try {
        var csvConfig = new CsvConfig("TestCsv");
        var filePath = csvConfig.DirectoryPath + "\\ImmutableLog.csv";
        var testDate = new DateTime(2020, 1, 1);

        //Empty file
        //----------
        using (var writer = new ImmutableLogWriter(filePath, csvConfig)) {
        }
        using (var reader = new ImmutableLogReader(filePath, csvConfig)) {
          Assert.IsFalse(reader.ReadLine(out var immutableLogRaw));
        }
        Assert.IsTrue(File.Exists(filePath));
        _ = new DC(csvConfig);
        Assert.AreEqual(0, DC.Data.ImmutableLogs.Count);
        DC.DisposeData();
        File.Delete(filePath);

        //File with 1 entry
        //-----------------
        using (var writer = new ImmutableLogWriter(filePath, csvConfig)) {
          writer.Write(new ImmutableLogRaw { Key = 0, Date = testDate, Text = "SomeText" });
        }
        using (var reader = new ImmutableLogReader(filePath, csvConfig)) {
          Assert.IsTrue(reader.ReadLine(out var immutableLogRaw));
          Assert.AreEqual("Key: 0, Date: 01.01.2020, Text: SomeText;", immutableLogRaw!.ToString());
          Assert.IsFalse(reader.ReadLine(out immutableLogRaw));
        }
        _ = new DC(csvConfig);
        Assert.AreEqual(1, DC.Data.ImmutableLogs.Count);
        Assert.AreEqual("Key: 0, Date: 01.01.2020, Text: SomeText;", DC.Data.ImmutableLogs[0].ToString());

        //Read entry created by using DC
        //------------------------------
        _ = new ImmutableLog(testDate.AddDays(1), "AnotherText");
        DC.DisposeData();
        using (var reader = new ImmutableLogReader(filePath, csvConfig)) {
          Assert.IsTrue(reader.ReadLine(out var immutableLogRaw));
          Assert.AreEqual("Key: 0, Date: 01.01.2020, Text: SomeText;", immutableLogRaw!.ToString());
          Assert.IsTrue(reader.ReadLine(out immutableLogRaw));
          Assert.AreEqual("Key: 1, Date: 02.01.2020, Text: AnotherText;", immutableLogRaw!.ToString());
          Assert.IsFalse(reader.ReadLine(out immutableLogRaw));
        }

        //Delete first entry
        //------------------
        _ = new DC(csvConfig);
        var filePathNew = filePath[..^3] + "new";
        using (var writer = new ImmutableLogWriter(filePathNew, csvConfig)) {
          var newKey = 0;
          foreach (var immutableLog in DC.Data.ImmutableLogs) {
            var immutableLogRaw = new ImmutableLogRaw(immutableLog);
            if (testDate!=immutableLogRaw.Date) {
              immutableLogRaw.Key = newKey++;
              writer.Write(immutableLogRaw);
            }
          }
          DC.DisposeData();
        }
        File.Move(filePathNew, filePath, overwrite: true);
        _ = new DC(csvConfig);
        Assert.AreEqual(1, DC.Data.ImmutableLogs.Count);
        Assert.AreEqual("Key: 0, Date: 02.01.2020, Text: AnotherText;", DC.Data.ImmutableLogs[0].ToString());

      } finally {
        DC.DisposeData();
      }
    }

    #region DataModel.md Sample Code ImmutableLog
    //      -------------------------------------

    public const string DataDirectoryPath = "TestCsv";


    public void YearlyLogMaintenance() {
      createData();
      var csvConfig = new CsvConfig(DataDirectoryPath);
      var deleteDate = DateTime.Now.Date.AddYears(-10);
      using var reader = new ImmutableLogReader(DataDirectoryPath + "\\ImmutableLog.csv", csvConfig);
      using var writer = new ImmutableLogWriter(DataDirectoryPath + "\\ImmutableLog.new", csvConfig);
      var newKey = 0;
      while (reader.ReadLine(out var immutableLogRaw)) {
        if (deleteDate<immutableLogRaw.Date) {
          immutableLogRaw.Key = newKey++;
          immutableLogRaw.Text = "Log: " + immutableLogRaw.Text;
          writer.Write(immutableLogRaw);
        }
      }
    }


    public void YearlyLogMaintenance2() {
      createData();
      var csvConfig = new CsvConfig(DataDirectoryPath);
      _ = new DC(csvConfig);
      var deleteDate = DateTime.Now.Date.AddYears(-10);
      using var writer = new ImmutableLogWriter(DataDirectoryPath + "\\ImmutableLog.new", csvConfig);
      var newKey = 0;
      foreach (var immutableLog in DC.Data.ImmutableLogs) {
        var immutableLogRaw = new ImmutableLogRaw(immutableLog);
        if (deleteDate<immutableLogRaw.Date) {
          immutableLogRaw.Key = newKey++;
          immutableLogRaw.Text = "Log: " + immutableLogRaw.Text;
          writer.Write(immutableLogRaw);
        }
      }
      DC.DisposeData();
    }



    private static void createData() {
      var directoryInfo = new DirectoryInfo("TestCsv");
      if (directoryInfo.Exists) {
        directoryInfo.Delete(recursive: true);
        directoryInfo.Refresh();
      }

      directoryInfo.Create();

      var csvConfig = new CsvConfig(DataDirectoryPath);
      _ = new DC(csvConfig);
      _ = new ImmutableLog(DateTime.Now, "Some text.");
      _ = new ImmutableLog(DateTime.Now.AddMinutes(1), "Some more text.");
      DC.DisposeData();
    }
    #endregion
  }
}
