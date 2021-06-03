using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageLib;


namespace StorageTest {


  [TestClass]
  public class DataStoreCSVTest {

    CsvConfig? csvConfig;
    DataStore<TestItemCsv>? dataStore;

    const bool cont = true;
    const bool notC = false;


    #region Readonly
    //      --------

    [TestMethod]
    public void TestDataStoreReadonlyCSV() {
      var directoryInfo = new DirectoryInfo("TestCsv");
      try {
        dataStore?.Dispose();
        if (directoryInfo.Exists) {
          directoryInfo.Delete(recursive: true);
          directoryInfo.Refresh();
        }

        directoryInfo.Create();
        directoryInfo.Refresh();

        csvConfig = new CsvConfig(directoryInfo.FullName, reportException: reportException);
        dataStore = createDataStoreReadonly();
        var expectedList = new List<string>();
        assertRewriteReadonly(expectedList, cont, ref dataStore);

        addReadonly(dataStore, expectedList, 0, "0");
        addReadonly(dataStore, expectedList, 1, "1");
        addReadonly(dataStore, expectedList, 2, "2");
        addReadonly(dataStore, expectedList, 3, "3");
      } finally {
        dataStore?.Dispose();
      }
    }


    private DataStore<TestItemCsv> createDataStoreReadonly() {
      dataStore = new DataStoreCSV<TestItemCsv>(
        null,
        1, 
        csvConfig!,
        TestItemCsv.MaxLineLength,
        TestItemCsv.Headers,
        TestItemCsv.SetKey,
        TestItemCsv.Create,
        null,
        null,
        TestItemCsv.Write,
        TestItemCsv.Disconnect,
        TestItem.RollbackItemNew,
        TestItem.RollbackItemStore,
        TestItem.RollbackItemUpdate,
        TestItem.RollbackItemRelease);
      Assert.IsTrue(dataStore.IsReadOnly);
      return dataStore;
    }


    private void assertRewriteReadonly(List<string> expectedList, bool areKeysContinuous, ref DataStore<TestItemCsv> dataStore) {
      assert(expectedList, areKeysContinuous, ref dataStore);
      dataStore.Dispose();

      dataStore = createDataStoreReadonly();
      assert(expectedList, areKeysContinuous, ref dataStore);
    }


    private void addReadonly(DataStore<TestItemCsv> dataStore, List<string> expectedList, int key, string text) {
      var dataString = $"{key}|{text}";
      expectedList.Add(dataString);
      var testItemCsv = new TestItemCsv(text);
      dataStore.Add(testItemCsv);
      assertRewriteReadonly(expectedList, cont, ref dataStore);
    }
    #endregion


    #region Updatable
    //      ---------
    [TestMethod]
    public void TestStorageDataStoreCSV() {
      var directoryInfo = new DirectoryInfo("TestCsv");
      for (int configurationIndex = 0; configurationIndex < 2; configurationIndex++) {
        try {
          directoryInfo.Refresh();
          if (directoryInfo.Exists) {
            directoryInfo.Delete(recursive: true);
            directoryInfo.Refresh();
          }

          directoryInfo.Create();
          directoryInfo.Refresh();

          csvConfig = new CsvConfig(directoryInfo.FullName, reportException: reportException);
          dataStore = createDataStore();
          var expectedList = new List<string>();
          assertRewrite(expectedList, cont, ref dataStore);

          add(dataStore, expectedList, 0, "0", cont);
          add(dataStore, expectedList, 1, "1", cont);
          add(dataStore, expectedList, 2, "2", cont);
          add(dataStore, expectedList, 3, "3", cont);
          add(dataStore, expectedList, 4, "4", cont);

          remove(dataStore, expectedList, 3, notC);
          add(dataStore, expectedList, 5, "5", notC);
          remove(dataStore, expectedList, 4, notC);
          remove(dataStore, expectedList, 5, cont);

          add(dataStore, expectedList, 3, "3a", cont);
          update(dataStore, expectedList, 2, "2a", cont);

          remove(dataStore, expectedList, 2, notC);
          remove(dataStore, expectedList, 3, cont);
          remove(dataStore, expectedList, 0, cont);
          remove(dataStore, expectedList, 1, cont);

          add(dataStore, expectedList, 0, "0a", cont);
          add(dataStore, expectedList, 1, "1a", cont);

        } finally {
          dataStore?.Dispose();
          directoryInfo.Delete(recursive: true);
        }
      }
    }


    private DataStore<TestItemCsv> createDataStore() {
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
      Assert.IsFalse(dataStore.IsReadOnly);
      return dataStore;
    }


    private void reportException(Exception ex) {
      Assert.Fail();
    }


    private void add(DataStore<TestItemCsv> dataStore, List<string> expectedList, int key, string text, bool isCont) {
      var dataString = $"{key}|{text}";
      expectedList.Add(dataString);
      var testItemCsv = new TestItemCsv(text);
      Assert.AreEqual(StorageExtensions.NoKey, testItemCsv.Key);
      dataStore.Add(testItemCsv);
      assertRewrite(expectedList, isCont, ref dataStore);
    }


    private void update(DataStore<TestItemCsv> dataStore, List<string> expectedList, int key, string text, bool isCont) {
      removeExpected(expectedList, key);
      var dataString = $"{key}|{text}";
      expectedList.Add(dataString);
      var item = dataStore[key];
      item.Update(text, dataStore); //fires HasChanged event
      assertRewrite(expectedList, isCont, ref dataStore);
    }


    private void remove(DataStore<TestItemCsv> dataStore, List<string> expectedList, int key, bool isCont) {
      removeExpected(expectedList, key);
      dataStore.Remove(key);
      assertRewrite(expectedList, isCont, ref dataStore);
    }


    private void removeExpected(List<string> expectedList, int key) {
      var keyString = key.ToString();
      var hasFound = false;
      for (int index = 0; index < expectedList.Count; index++) {
        if (expectedList[index].Split("|")[0]==keyString) {
          expectedList.RemoveAt(index);
          hasFound = true;
        }
      }
      Assert.IsTrue(hasFound);
    }


    private void assertRewrite(List<string> expectedList, bool areKeysContinuous, ref DataStore<TestItemCsv> dataStore) {
      assert(expectedList, areKeysContinuous, ref dataStore);
      dataStore.Dispose();

      dataStore = createDataStore();
      assert(expectedList, areKeysContinuous, ref dataStore);
    }


    private void assert(List<string> expectedList, bool areKeysContinuous, ref DataStore<TestItemCsv> dataStore) {
      int count = expectedList.Count;
      Assert.AreEqual(count, dataStore.Count);
      Assert.AreEqual(count, dataStore.Keys.Count);
      Assert.AreEqual(count, dataStore.Values.Count);
      Assert.AreEqual(areKeysContinuous, dataStore.AreKeysContinuous);
      for (int index = 0; index < count; index++) {
        var fields = expectedList[index].Split("|");
        var key = int.Parse(fields[0]);
        var data = dataStore[key];
        Assert.AreEqual(fields[1], data.Text);
        Assert.IsTrue(dataStore.Keys.Contains(key));
        Assert.IsTrue(dataStore.Values.Contains(data));
      }
      var countedItems = 0;
      foreach (var data in dataStore) {
        countedItems++;
        var dataString = $"{data.Key}|{data.Text}";
        Assert.IsTrue(expectedList.Contains(dataString));
      }
      Assert.AreEqual(count, countedItems);
    }
    #endregion
  }
}
