using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageLib;
using System.Linq;


namespace StorageTest {


  [TestClass]
  public class DataStoreTest {

    #pragma warning disable CS8618 // dataStore is uninitialized. 
    DataStore<TestItem> dataStore;
    #pragma warning restore CS8618 //
    /*+
    bool wasAdded = false;
    bool wasChanged = false;
    bool wasDeleted = false;
    +*/

    const bool cont = true;
    const bool notC = false;


    [TestMethod]
    public void TestDataStore() {
      dataStore = new DataStore<TestItem>(
        null,
        1,
        TestItem.SetKey,
        TestItem.RollbackItemNew,
        TestItem.RollbackItemStore,
        TestItem.RollbackItemUpdate,
        TestItem.RollbackItemRelease,
        areInstancesUpdatable: true, 
        areInstancesReleasable: true);
      /*+
      dataStore.Added += dictionary_Added;
      dataStore.Updated += dictionary_Changed;
      dataStore.Removed += dictionary_Deleted;
      +*/
      var expectedList = new List<string>();
      assert(expectedList, cont, dataStore);

      add(dataStore, expectedList, 0, "0", cont);
      add(dataStore, expectedList, 1, "1", cont);
      add(dataStore, expectedList, 2, "2", cont);
      add(dataStore, expectedList, 3, "3", cont);
      add(dataStore, expectedList, 4, "4", cont);

      remove(dataStore, expectedList, 3, notC);
      add(dataStore, expectedList, 5, "5", notC);
      remove(dataStore, expectedList, 5, notC);
      remove(dataStore, expectedList, 4, cont);

      add(dataStore, expectedList, 3, "3a", cont);

      update(dataStore, expectedList, 2, "2a", cont);

      remove(dataStore, expectedList, 1, notC);
      remove(dataStore, expectedList, 0, cont);
      remove(dataStore, expectedList, 2, cont);
      remove(dataStore, expectedList, 3, cont);

      add(dataStore, expectedList, 0, "0b", cont);
      add(dataStore, expectedList, 1, "1b", cont);
      add(dataStore, expectedList, 2, "2b", cont);
      add(dataStore, expectedList, 3, "3b", cont);
      remove(dataStore, expectedList, 0, cont);
      add(dataStore, expectedList, 4, "4b", cont);
      remove(dataStore, expectedList, 2, notC);
      add(dataStore, expectedList, 5, "5b", notC);
      remove(dataStore, expectedList, 3, notC);
      remove(dataStore, expectedList, 1, cont);
      remove(dataStore, expectedList, 5, cont);
    }


    private void add(DataStore<TestItem> dataStore, List<string> expectedList, int key, string text, bool cont) {
      var dataString = $"{key}|{text}";
      expectedList.Add(dataString);
      var testItem = new TestItem(text);
      Assert.AreEqual(StorageExtensions.NoKey, testItem.Key);
      dataStore.Add(testItem);
      assert(expectedList, cont, dataStore);
    }


    private void update(DataStore<TestItem> dataStore, List<string> expectedList, int key, string text, bool cont) {
      removeExpected(expectedList, key);
      var dataString = $"{key}|{text}";
      expectedList.Add(dataString);
      var item = dataStore[key];
      item.Update(text);
      assert(expectedList, cont, dataStore);
    }


    private void remove(DataStore<TestItem> dataStore, List<string> expectedList, int key, bool cont) {
      removeExpected(expectedList, key);
      dataStore.Remove(key);
      assert(expectedList, cont, dataStore);
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


    private void assert(List<string> expectedList, bool areKeysContinuous, DataStore<TestItem> dataStore) {
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
  }
}
