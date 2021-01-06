using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageLib;
using System;
using System.IO;

namespace StorageTest {


  [TestClass]
  public class BackupTest {



    [TestMethod]
    public void TestBackup() {
      var directoryInfo =  new DirectoryInfo("TestCsv");
      var directoryPath = directoryInfo.FullName;
      try {
        directoryInfo.Refresh();
        if (directoryInfo.Exists) {
          directoryInfo.Delete(recursive: true);
          directoryInfo.Refresh();
        }

        directoryInfo.Create();
        directoryInfo.Refresh();
        var activeDirectory = directoryInfo.CreateSubdirectory("Active");
        var backupDirectory = directoryInfo.CreateSubdirectory("Backup");
        var backupPeriodicity = 2;
        var backupCopies = 3;
        var csvConfig = new CsvConfig(activeDirectory.FullName, backupDirectory.FullName, backupPeriodicity, backupCopies);
        var now = new DateTime(2000, 12, 11);
        var result = Csv.Backup(csvConfig, now);
        Assert.AreEqual(@$"0 files copied from {directoryPath}\Active 
to {directoryPath}\Backup\csv20001211.", result);

        now = now.AddDays(1);
        result = Csv.Backup(csvConfig, now);
        Assert.AreEqual(@"Last backup: 11.12.00; Today: 12.12.00; Next backup: 13.12.00;", result);

        File.WriteAllText(activeDirectory.FullName + @"\Data.csv", "some text");
        now = now.AddDays(1);
        result = Csv.Backup(csvConfig, now);
        Assert.AreEqual(@$"1 files copied from {directoryPath}\Active 
to {directoryPath}\Backup\csv20001213.", result);

        now = now.AddDays(1);
        result = Csv.Backup(csvConfig, now);
        Assert.AreEqual(@"Last backup: 13.12.00; Today: 14.12.00; Next backup: 15.12.00;", result);

        File.WriteAllText(activeDirectory.FullName + @"\Data1.csv", "other text");
        now = now.AddDays(1);
        result = Csv.Backup(csvConfig, now);
        Assert.AreEqual(@$"2 files copied from {directoryPath}\Active 
to {directoryPath}\Backup\csv20001215.", result);

        File.WriteAllText(activeDirectory.FullName + @"\Data1.csv", "other text");
        now = now.AddDays(2);
        result = Csv.Backup(csvConfig, now);
        Assert.AreEqual(@$"2 files copied from {directoryPath}\Active 
to {directoryPath}\Backup\csv20001217.", result);

        File.WriteAllText(activeDirectory.FullName + @"\Data1.csv", "other text");
        now = now.AddDays(2);
        result = Csv.Backup(csvConfig, now);
        Assert.AreEqual(@$"Directory {directoryPath}\Backup\csv20001211 deleted
2 files copied from {directoryPath}\Active 
to {directoryPath}\Backup\csv20001219.", result);
      } finally {
        directoryInfo.Delete(recursive: true);
      }
    }
  }
}
