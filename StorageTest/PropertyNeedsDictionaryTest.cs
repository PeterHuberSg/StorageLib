using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageLib;
using StorageDataContext;

namespace StorageTest {


  [TestClass]
  public class PropertyNeedsDictionaryTest {

    static readonly DirectoryInfo directoryInfo =  new DirectoryInfo("TestCsv");
    static readonly CsvConfig csvConfig = new CsvConfig(directoryInfo.FullName, reportException: reportException);
    static readonly Dictionary<int, string> expectedSamples = new Dictionary<int, string>();
    static readonly Dictionary<int, string> expectedIdInts = new Dictionary<int, string>();
    static readonly Dictionary<string, string> expectedIdStrings = new Dictionary<string, string>();
    static readonly Dictionary<string, string> expectedTextLowers = new Dictionary<string, string>();
    static readonly Dictionary<string, string> expectedTextNullableLowers = new Dictionary<string, string>();
    static readonly Dictionary<string, string> expectedTextReadonlyLowers = new Dictionary<string, string>();


    [TestMethod]
    public void TestPropertyNeedsDictionary() {
      try {
        directoryInfo.Refresh();
        if (directoryInfo.Exists) {
          directoryInfo.Delete(recursive: true);
          directoryInfo.Refresh();
        }

        directoryInfo.Create();
        directoryInfo.Refresh();

        new DC(csvConfig);
        assertData();
        var key0 = addData(1, "One", "FirstLower", "FirstLowerNull", "FirstReadonly");
        var key1 = addData(2, null, "SecondLower", "SecondLowerNull", "SecondReadonly");

        update(key0, 1, "One", "FirstLower", "FirstLowerNull");
        update(key0, 11, "One.one", "FirstLower", "FirstLowerNull Changed");
        update(key0, 10, null, "FirstLower Changed", null);
        update(key1, 20, null, "Two.zero", "SecondLowerNull");
        update(key1, 21, null, "Two.one", null);

        delete(key0);
        delete(key1);

      } finally {
        DC.Data.Dispose();
        directoryInfo.Delete(recursive: true);
      }

    }


    private static void reportException(Exception obj) {
      System.Diagnostics.Debug.WriteLine(obj);
      System.Diagnostics.Debugger.Break();
      Assert.Fail();
    }


    private int addData(int idInt, string? idString, string text, string? textNullable, string textReadonly) {
      DC.Data.StartTransaction();
      new PropertyNeedsDictionaryClass(idInt, idString, text, textNullable, textReadonly, isStoring: false);
      DC.Data.RollbackTransaction();
      assertData();

      DC.Data.StartTransaction();
      new PropertyNeedsDictionaryClass(idInt, idString, text, textNullable, textReadonly);
      DC.Data.RollbackTransaction();
      assertData();

      DC.Data.StartTransaction();
      var sample = new PropertyNeedsDictionaryClass(idInt, idString, text, textNullable, textReadonly, isStoring: false);
      DC.Data.CommitTransaction();
      DC.Data.StartTransaction();
      sample.Store();
      DC.Data.CommitTransaction();
      sample = DC.Data.PropertyNeedsDictionaryClasses[sample.Key];
      var sampleString = sample.ToString();
      expectedSamples.Add(sample.Key, sampleString);
      expectedIdInts.Add(idInt, sampleString);
      if (idString!=null) {
        expectedIdStrings.Add(idString, sampleString);
      }
      expectedTextLowers.Add(sample.TextLower, sampleString);
      if (sample.TextNullableLower!=null) {
        expectedTextNullableLowers.Add(sample.TextNullableLower, sampleString);
      }
      expectedTextReadonlyLowers.Add(sample.TextReadonlyLower, sampleString);
      assertData();
      return sample.Key;
    }


    private void update(int key, int idInt, string? idString, string text, string? textNullable) {
      var sample = DC.Data.PropertyNeedsDictionaryClasses[key];
      DC.Data.StartTransaction();
      sample.Update(idInt, idString, text, textNullable);
      DC.Data.RollbackTransaction();
      assertData();

      sample = DC.Data.PropertyNeedsDictionaryClasses[key];
      expectedIdInts.Remove(sample.IdInt);
      if (sample.IdString!=null) {
        expectedIdStrings.Remove(sample.IdString);
      }
      expectedTextLowers.Remove(sample.TextLower);
      if (sample.TextNullableLower!=null) {
        expectedTextNullableLowers.Remove(sample.TextNullableLower);
      }
      expectedTextReadonlyLowers.Remove(sample.TextReadonlyLower);
      DC.Data.StartTransaction();
      sample.Update(idInt, idString, text, textNullable);
      DC.Data.CommitTransaction();
      sample = DC.Data.PropertyNeedsDictionaryClasses[sample.Key];
      var sampleString = sample.ToString();
      expectedSamples[sample.Key] = sampleString;
      expectedIdInts.Add(idInt, sampleString);
      if (idString!=null) {
        expectedIdStrings.Add(idString, sampleString);
      }
      expectedTextLowers.Add(sample.TextLower, sampleString);
      if (sample.TextNullableLower!=null) {
        expectedTextNullableLowers.Add(sample.TextNullableLower, sampleString);
      }
      expectedTextReadonlyLowers.Add(sample.TextReadonlyLower, sampleString);
      assertData();
    }


    private void delete(int key) {
      var sample = DC.Data.PropertyNeedsDictionaryClasses[key];
      DC.Data.StartTransaction();
      sample.Release();
      DC.Data.RollbackTransaction();
      assertData();

      sample = DC.Data.PropertyNeedsDictionaryClasses[key];
      expectedSamples.Remove(key);
      expectedIdInts.Remove(sample.IdInt);
      if (sample.IdString!=null) {
        expectedIdStrings.Remove(sample.IdString);
      }
      expectedTextLowers.Remove(sample.TextLower);
      if (sample.TextNullableLower!=null) {
        expectedTextNullableLowers.Remove(sample.TextNullableLower);
      }
      expectedTextReadonlyLowers.Remove(sample.TextReadonlyLower);
      DC.Data.StartTransaction();
      sample.Release();
      DC.Data.CommitTransaction();
      assertData();
    }


    private void assertData() {
      assertDictionaries();
      DC.Data.Dispose();
      new DC(csvConfig);
      assertDictionaries();
    }


    private void assertDictionaries() {
      Assert.AreEqual(expectedSamples.Count, DC.Data.PropertyNeedsDictionaryClasses.Count);
      if (expectedSamples.Count>0) {
        foreach (var expectedSample in expectedSamples) {
          Assert.AreEqual(expectedSample.Value,
            DC.Data.PropertyNeedsDictionaryClasses[expectedSample.Key].ToString());
        }
      }
      Assert.AreEqual(expectedIdInts.Count, DC.Data.PropertyNeedsDictionaryClassesByIdInt.Count);
      if (expectedIdInts.Count>0) {
        foreach (var expectedIntKVP in expectedIdInts) {
          Assert.AreEqual(expectedIntKVP.Value,
            DC.Data.PropertyNeedsDictionaryClassesByIdInt[expectedIntKVP.Key].ToString());
        }
      }
      Assert.AreEqual(expectedIdStrings.Count, DC.Data.PropertyNeedsDictionaryClassesByIdString.Count);
      if (expectedIdStrings.Count>0) {
        foreach (var expectedIntKVP in expectedIdStrings) {
          Assert.AreEqual(expectedIntKVP.Value,
            DC.Data.PropertyNeedsDictionaryClassesByIdString[expectedIntKVP.Key].ToString());
        }
      }
      Assert.AreEqual(expectedTextLowers.Count, DC.Data.PropertyNeedsDictionaryClassesByTextLower.Count);
      if (expectedTextLowers.Count>0) {
        foreach (var expectedTextLower in expectedTextLowers) {
          Assert.AreEqual(expectedTextLower.Value,
            DC.Data.PropertyNeedsDictionaryClassesByTextLower[expectedTextLower.Key].ToString());
        }
      }
      Assert.AreEqual(expectedTextNullableLowers.Count, DC.Data.PropertyNeedsDictionaryClassesByTextNullableLower.Count);
      if (expectedTextNullableLowers.Count>0) {
        foreach (var expectedTextNullableLower in expectedTextNullableLowers) {
          Assert.AreEqual(expectedTextNullableLower.Value,
            DC.Data.PropertyNeedsDictionaryClassesByTextNullableLower[expectedTextNullableLower.Key].ToString());
        }
      }
      Assert.AreEqual(expectedTextReadonlyLowers.Count, DC.Data.PropertyNeedsDictionaryClassesByTextReadonlyLower.Count);
      if (expectedTextReadonlyLowers.Count>0) {
        foreach (var expectedTextReadonlyLower in expectedTextReadonlyLowers) {
          Assert.AreEqual(expectedTextReadonlyLower.Value,
            DC.Data.PropertyNeedsDictionaryClassesByTextReadonlyLower[expectedTextReadonlyLower.Key].ToString());
        }
      }
    }
  }
}
