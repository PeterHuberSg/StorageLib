using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageLib;
using System;
using System.IO;


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
      var dataStore = openDataStoreCSV(csvConfig);
      try {
        var testItem0 = new TestItemCsv("testItem0");
        dataStore.Add(testItem0);
        var testItem1 = new TestItemCsv("testItem1");
        dataStore.Add(testItem1);
        var testItem2 = new TestItemCsv("testItem2");
        dataStore.Add(testItem2);
        testItem1.Remove(dataStore);
        testItem2.Update("testItem2 updated", dataStore);
        var expectedTestItem0 = testItem0.ToString();
        var expectedTestItem2 = testItem2.ToString();
        dispose(ref dataStore, directoryInfo, hasBakFile: true, "new file with updates and remove");

        directoryInfo.Refresh();
        File.Delete(directoryInfo.FullName + @"\TestItemCsv.csv");
        File.Move(directoryInfo.FullName + @"\TestItemCsv.bak", directoryInfo.FullName + @"\TestItemCsv.csv");

        dataStore = openDataStoreCSV(csvConfig);
        Assert.AreEqual(expectedTestItem0, dataStore[0].ToString());
        Assert.AreEqual(expectedTestItem2, dataStore[2].ToString());

        //Test if .bak files are properly created and deleted
        //---------------------------------------------------

        //.bak file was used to restore .csv file
        dispose(ref dataStore, directoryInfo, hasBakFile: true, "old .bak file was read, new .bak file created");

        //no .bak file when only adding item
        dataStore = openDataStoreCSV(csvConfig);
        var testItem3 = new TestItemCsv("testItem3");
        dataStore.Add(testItem3);
        dispose(ref dataStore, directoryInfo, hasBakFile: false, "when only adding new items, there should be no .bak file");

        //.bak file when only updating item
        dataStore = openDataStoreCSV(csvConfig);
        testItem3 = dataStore[3];
        testItem3.Update("testItem3 updated", dataStore);
        dispose(ref dataStore, directoryInfo, hasBakFile: true, ".bak file expected after update");

        //no .bak file when no change
        dataStore = openDataStoreCSV(csvConfig);
        dispose(ref dataStore, directoryInfo, hasBakFile: false, "no .bak file expected after no change");
 
        //.bak file when only removing item
        dataStore = openDataStoreCSV(csvConfig);
        testItem3 = dataStore[3];
        testItem3.Remove(dataStore);
        dispose(ref dataStore, directoryInfo, hasBakFile: true, ".bak file expected after removal");

      } finally {
        dataStore?.Dispose();
      }
    }


    private static DataStoreCSV<TestItemCsv> openDataStoreCSV(CsvConfig csvConfig) {
      return new DataStoreCSV<TestItemCsv>(
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
    }


    private static void dispose(
      ref DataStoreCSV<TestItemCsv>? dataStore, 
      DirectoryInfo directoryInfo, 
      bool hasBakFile, 
      string testMessage) 
    {
      dataStore!.Dispose();
      dataStore = null;
      directoryInfo.Refresh();
      Assert.AreEqual(hasBakFile, directoryInfo.GetFiles("*.bak").Length>0, testMessage);
    }


    private void reportException(Exception ex) {
      Assert.Fail();
    }


  }
}
