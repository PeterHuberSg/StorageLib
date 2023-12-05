using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageLib;
using static System.Net.Mime.MediaTypeNames;


namespace StorageTest {


  [TestClass]
  public class DataStoreCSVTest {

    CsvConfig? csvConfig;
    DataStoreCSV<TestItemCsv>? dataStoreCSV;

    const bool cont = true;  //keys are continuous
    const bool notC = false; //keys are not continuous


    #region Readonly
    //      --------

    //items are not updatable and not removable => Keys don't get stored in the CSV file, they can never be out of sequence 
    //and there is never a missing key, i.e. newKey = oldKey + 1

    [TestMethod]
    public void TestDataStoreCSVReadonly() {
      var directoryInfo = new DirectoryInfo("TestCsv");
      try {
        //clean up from previous failed tests
        dataStoreCSV?.Dispose();
        if (directoryInfo.Exists) {
          directoryInfo.Delete(recursive: true);
          directoryInfo.Refresh();
        }

        directoryInfo.Create();
        directoryInfo.Refresh();

        csvConfig = new CsvConfig(directoryInfo.FullName, reportException: reportException);
        dataStoreCSV = createDataStoreReadonly();
        var expectedList = new List<string>();
        assertRewriteReadonly(expectedList, cont, ref dataStoreCSV);

        addReadonly(dataStoreCSV, expectedList, 0, "0");
        addReadonly(dataStoreCSV, expectedList, 1, "1");
        addReadonly(dataStoreCSV, expectedList, 2, "2");
        addReadonly(dataStoreCSV, expectedList, 3, "3");
      } finally {
        dataStoreCSV?.Dispose();
      }
    }


    private DataStoreCSV<TestItemCsv> createDataStoreReadonly() {
      dataStoreCSV = new DataStoreCSV<TestItemCsv>(
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
        null,
        null);
      Assert.IsTrue(dataStoreCSV.IsReadOnly);
      return dataStoreCSV;
    }


    private void assertRewriteReadonly(List<string> expectedList, bool areKeysContinuous, 
      ref DataStoreCSV<TestItemCsv> dataStoreCSV) 
    {
      assert(expectedList, areKeysContinuous, ref dataStoreCSV);
      dataStoreCSV.Dispose();

      dataStoreCSV = createDataStoreReadonly();
      assert(expectedList, areKeysContinuous, ref dataStoreCSV);
    }


    private void addReadonly(DataStoreCSV<TestItemCsv> dataStoreCSV, List<string> expectedList, int key, string text) {
      var dataString = $"{key}|{text}";
      expectedList.Add(dataString);
      var testItemCsv = new TestItemCsv(text);
      dataStoreCSV.Add(testItemCsv);
      assertRewriteReadonly(expectedList, cont, ref dataStoreCSV);
    }
    #endregion


    #region Not Readonly
    //      ------------

    //items are updatable and/or removable => Keys get stored in the CSV file.
    //updatable but not removable and not updatable but removable are subcases of updatable and removable, so only
    //updatable and removable get tested.
    //New keys should always be greater than any existing key, but some keys can be missing => dataStore.AreKeysContinuous = false

    [TestMethod]
    public void TestDataStoreCSVNotReadOnly() {
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
          dataStoreCSV = createDataStoreUpdatableReleasable();
          var expectedList = new List<string>();
          assertRewrite(expectedList, cont, ref dataStoreCSV);

          add(dataStoreCSV, expectedList, 0, "0", cont);
          add(dataStoreCSV, expectedList, 1, "1", cont);
          add(dataStoreCSV, expectedList, 2, "2", cont);
          add(dataStoreCSV, expectedList, 3, "3", cont);
          add(dataStoreCSV, expectedList, 4, "4", cont);

          remove(dataStoreCSV, expectedList, 3, notC);
          add(dataStoreCSV, expectedList, 5, "5", notC);
          remove(dataStoreCSV, expectedList, 4, notC);
          remove(dataStoreCSV, expectedList, 5, cont);

          add(dataStoreCSV, expectedList, 3, "3a", cont);
          update(dataStoreCSV, expectedList, 2, "2a", cont);

          remove(dataStoreCSV, expectedList, 2, notC);
          remove(dataStoreCSV, expectedList, 3, cont);
          remove(dataStoreCSV, expectedList, 0, cont);
          remove(dataStoreCSV, expectedList, 1, cont);

          add(dataStoreCSV, expectedList, 0, "0a", cont);
          add(dataStoreCSV, expectedList, 1, "1a", cont);

        } finally {
          dataStoreCSV?.Dispose();
          directoryInfo.Delete(recursive: true);
        }
      }
    }


    private DataStoreCSV<TestItemCsv> createDataStoreUpdatableNotReleasable() {
      dataStoreCSV = new DataStoreCSV<TestItemCsv>(
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
        null,
        areInstancesUpdatable: true,
        areInstancesReleasable: false);
      Assert.IsFalse(dataStoreCSV.IsReadOnly);
      return dataStoreCSV;
    }


    private DataStoreCSV<TestItemCsv> createDataStoreUpdatableReleasable() {
      dataStoreCSV = new DataStoreCSV<TestItemCsv>(
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
      Assert.IsFalse(dataStoreCSV.IsReadOnly);
      return dataStoreCSV;
    }


    private void reportException(Exception ex) {
      Assert.Fail();
    }


    private void add(DataStoreCSV<TestItemCsv> dataStoreCSV, List<string> expectedList, int key, string text, bool isCont) {
      var dataString = $"{key}|{text}";
      expectedList.Add(dataString);
      var testItemCsv = new TestItemCsv(text);
      Assert.AreEqual(StorageExtensions.NoKey, testItemCsv.Key);
      dataStoreCSV.Add(testItemCsv);
      assertRewrite(expectedList, isCont, ref dataStoreCSV);
    }


    private void update(DataStoreCSV<TestItemCsv> dataStoreCSV, List<string> expectedList, int key, string text, bool isCont) {
      removeExpected(expectedList, key);
      var dataString = $"{key}|{text}";
      expectedList.Add(dataString);
      var item = dataStoreCSV[key];
      item.Update(text, dataStoreCSV); //fires HasChanged event
      assertRewrite(expectedList, isCont, ref dataStoreCSV);
    }


    private void remove(DataStoreCSV<TestItemCsv> dataStoreCSV, List<string> expectedList, int key, bool isCont) {
      removeExpected(expectedList, key);
      dataStoreCSV.Remove(key);
      assertRewrite(expectedList, isCont, ref dataStoreCSV);
    }


    private static void removeExpected(List<string> expectedList, int key) {
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


    private void assertRewrite(List<string> expectedList, bool areKeysContinuous, ref DataStoreCSV<TestItemCsv> dataStoreCSV) {
      assert(expectedList, areKeysContinuous, ref dataStoreCSV);
      dataStoreCSV.Dispose();

      dataStoreCSV = createDataStoreUpdatableReleasable();
      assert(expectedList, areKeysContinuous, ref dataStoreCSV);
    }


    private static void assert(List<string> expectedList, bool areKeysContinuous, ref DataStoreCSV<TestItemCsv> dataStoreCSV) {
      int count = expectedList.Count;
      Assert.AreEqual(count, dataStoreCSV.Count);
      Assert.AreEqual(count, dataStoreCSV.Keys.Count);
      Assert.AreEqual(count, dataStoreCSV.Values.Count);
      Assert.AreEqual(areKeysContinuous, dataStoreCSV.AreKeysContinuous);
      for (int index = 0; index < count; index++) {
        var fields = expectedList[index].Split("|");
        var key = int.Parse(fields[0]);
        var data = dataStoreCSV[key];
        Assert.AreEqual(fields[1], data.Text);
        Assert.IsTrue(dataStoreCSV.Keys.Contains(key));
        Assert.IsTrue(dataStoreCSV.Values.Contains(data));
      }
      var countedItems = 0;
      foreach (var data in dataStoreCSV) {
        countedItems++;
        var dataString = $"{data.Key}|{data.Text}";
        Assert.IsTrue(expectedList.Contains(dataString));
      }
      Assert.AreEqual(count, countedItems);
    }
    #endregion


    #region Restore From File
    //      -----------------

    //When the application gets closed properly, the .csv file might contain update and release records. However, in 
    //memory is only the latest updated data and upon Dispose() the existing .csv file gets renamed to .bak and a new
    //.csv file gets written containing only "add records". The original .csv file with updates and releases gets only
    //read, when dispose doesn't get executed because of an exception.

    [TestMethod]
    public void TestDataStoreCSVRestoreFromFile() {
      var directoryInfo = new DirectoryInfo("TestCsv");
      try {
        //clean up from previous failed tests
        dataStoreCSV?.Dispose();
        if (directoryInfo.Exists) {
          directoryInfo.Delete(recursive: true);
          directoryInfo.Refresh();
        }

        directoryInfo.Create();
        directoryInfo.Refresh();

        //This test verifies that the following old bug is corrected:
        //The old software only detected if the keys are not continuous after the complete file was read from the CSV file. This
        //let to problems if the keys were actually not continuous and an "update item" was performed, as if the keys were 
        //continuous.
        csvConfig = new CsvConfig(directoryInfo.FullName, reportException: reportException);
        dataStoreCSV = createDataStoreUpdatableReleasable();
        var item0 = new TestItemCsv("item0");
        dataStoreCSV.Add(item0);
        var item1 = new TestItemCsv("item1");
        dataStoreCSV.Add(item1);
        var item2 = new TestItemCsv("item2");
        dataStoreCSV.Add(item2);
        var item3 = new TestItemCsv("item3");
        dataStoreCSV.Add(item3);
        var item4 = new TestItemCsv("item4");
        dataStoreCSV.Add(item4);
        dataStoreCSV.Remove(item1);
        dataStoreCSV.Dispose();
        dataStoreCSV = createDataStoreUpdatableReleasable();
        item2.Update("item2 changed", dataStoreCSV);
        dataStoreCSV.Dispose();

        var fileNameCsv = dataStoreCSV.PathFileName;
        var fileNameBak = Path.ChangeExtension(fileNameCsv, "bak");
        File.Move(fileNameBak, fileNameCsv, overwrite: true);


        dataStoreCSV = createDataStoreUpdatableReleasable();
        var expectedList = new List<string> {
          "0|item0",
          "2|item2 changed",
          "3|item3",
          "4|item4",
        };
        assert(expectedList, areKeysContinuous: false, ref dataStoreCSV);
        dataStoreCSV.Dispose();

        ////////////////////////////////////////////////////////////////////////////////
        //test to verify that a new key smaller than an existing key raises an exception
        File.WriteAllText(fileNameCsv,
@"Key	Text	
+0	item0	
+2	item2	
+1	item1	
");
        try {
          dataStoreCSV = createDataStoreUpdatableReleasable();
          Assert.Fail("IndexOutOfRangeException is missing, keys are out of sequence.");

        } catch (IndexOutOfRangeException ex) {
          //test is successful
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Test: When reading an updateable but nor releasable file, does DataStoreCSV detect when the keys are not
        //continuous ?
        // It is ok that a file starts with a key>0, but key 4 is missing:
        File.WriteAllText(fileNameCsv,
@"Key	Text	
+2	item2	
+3	item3	
+5	item5	
");
        try {
          dataStoreCSV = createDataStoreUpdatableNotReleasable();
          Assert.Fail("IndexOutOfRangeException is missing, new key is too big.");

        } catch (IndexOutOfRangeException ex) {
          //test is successful
        }

      } finally {
        dataStoreCSV?.Dispose();
      }
    }
    #endregion
  }
}
