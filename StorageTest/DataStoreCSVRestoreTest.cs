using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageLib;


namespace StorageTest {


  [TestClass]
  public class DataStoreCSVRestoreTest {


    [TestMethod]
    public void TestDataStoreCSVRestore() {
      var directoryInfo = new DirectoryInfo("TestCsv");
      if (directoryInfo.Exists) {
        directoryInfo.Delete(recursive: true);
        directoryInfo.Refresh();
      }

      directoryInfo.Create();
      directoryInfo.Refresh();

      var csvConfig = new CsvConfig(directoryInfo.FullName, reportException: reportException);
      var dataStore = new DataStoreCSV<TestItemCsv>(
        null,
        1,
        csvConfig!,
        TestItemCsv.MaxLineLength,
        TestItemCsv.Headers,
        TestItemCsv.SetKey,
        TestItemCsv.Create,
        null,
        TestItemCsv.Update,
        TestItemCsv.Write,
        TestItemCsv.Disconnect,
        TestItem.RollbackItemNew,
        TestItem.RollbackItemStore,
        TestItem.RollbackItemUpdate,
        TestItem.RollbackItemRelease,
        areInstancesUpdatable: true,
        areInstancesReleasable: true);
      try {
        var testItem0 = new TestItemCsv("testItem0");
        dataStore.Add(testItem0);
        var testItem1 = new TestItemCsv("testItem1");
        dataStore.Add(testItem1);
        var testItem2 = new TestItemCsv("testItem2");
        dataStore.Add(testItem2);
        testItem1.Remove(dataStore);
        testItem2.Update("testItem2 updated", dataStore);
        var expectedtestItem0 = testItem0.ToString();
        var expectedtestItem2 = testItem2.ToString();
        dataStore.Dispose();

        directoryInfo.Refresh();
        File.Delete(directoryInfo.FullName + @"\TestItemCsv.csv");
        File.Move(directoryInfo.FullName + @"\TestItemCsv.bak", directoryInfo.FullName + @"\TestItemCsv.csv");

        dataStore = new DataStoreCSV<TestItemCsv>(
          null,
          1,
          csvConfig!,
          TestItemCsv.MaxLineLength,
          TestItemCsv.Headers,
          TestItemCsv.SetKey,
          TestItemCsv.Create,
          null,
          TestItemCsv.Update,
          TestItemCsv.Write,
          TestItemCsv.Disconnect,
          TestItem.RollbackItemNew,
          TestItem.RollbackItemStore,
          TestItem.RollbackItemUpdate,
          TestItem.RollbackItemRelease,
          areInstancesUpdatable: true,
          areInstancesReleasable: true);
        Assert.AreEqual(expectedtestItem0, dataStore[0].ToString());
        Assert.AreEqual(expectedtestItem2, dataStore[2].ToString());
      } finally {
        dataStore?.Dispose();
      }
    }


    private void reportException(Exception ex) {
      Assert.Fail();
    }


  }
}
