//using System;
//using System.Collections.Generic;
//using System.IO;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using StorageLib;
//using TestContext;


//namespace StorageTest {


//  [TestClass]
//  public class CreateOnlyTest {
//    CsvConfig? csvConfig;
//    readonly Dictionary<int, string> expectedParents = new Dictionary<int, string>();
//    readonly Dictionary<int, string> expectedParentsNullable = new Dictionary<int, string>();
//    readonly Dictionary<int, string> expectedChildren= new Dictionary<int, string>();


//    [TestMethod]
//    public void TestCreateOnly() {
//      try {
//        var directoryInfo = new DirectoryInfo("TestCsv");
//        if (directoryInfo.Exists) {
//          directoryInfo.Delete(recursive: true);
//          directoryInfo.Refresh();
//        }

//        directoryInfo.Create();

//        csvConfig = new CsvConfig(directoryInfo.FullName, reportException: reportException);
//        initDL();
//        assertDL();

//        //no transaction is used to have also testing if everything works without transactions.
//        addParent("1");
//        addChild(0, "11");

//        addParent("2");
//        addChild(1, "21");
//        addChild(1, "22");
//      } finally {
//        DC.DisposeData();
//      }
//    }


//    private void reportException(Exception obj) {
//      System.Diagnostics.Debug.WriteLine(obj);
//      System.Diagnostics.Debugger.Break();
//      Assert.Fail();
//    }


//    private void initDL() {
//      new DC(csvConfig);
//    }


//    private void assertDL() {
//      Assert.AreEqual(expectedParents.Count, DC.Data.CreateOnly_Parents.Count);
//      foreach (var parent in DC.Data.CreateOnly_Parents) {
//        Assert.AreEqual(expectedParents[parent.Key], parent.ToString());
//      }

//      Assert.AreEqual(expectedChildren.Count, DC.Data.CreateOnly_Children.Count);
//      foreach (var child in DC.Data.CreateOnly_Children) {
//        Assert.AreEqual(expectedChildren[child.Key], child.ToString());
//      }
//    }


//    private void addParent(string someText) {
//      var newCreateOnlyParent = new CreateOnly_Parent(someText, isStoring: true);
//      expectedParents.Add(newCreateOnlyParent.Key, newCreateOnlyParent.ToString());
//      var newCreateOnlyParenNullablet = new CreateOnly_ParentNullable(someText, isStoring: true);
//      expectedParentsNullable.Add(newCreateOnlyParenNullablet.Key, newCreateOnlyParenNullablet.ToString());
//      assertData();
//    }


//    private void addChild(int parentKey, string text) {
//      var parentDictionary = DC.Data.CreateOnly_Parents[parentKey];
//      var parentDictionaryNullable = DC.Data.CreateOnly_ParentNullables[parentKey];
//      var newChild = new CreateOnly_Child(text, parentDictionary, parentDictionaryNullable, isStoring: true);
//      expectedChildren.Add(newChild.Key, newChild.ToString());
//      expectedParents[parentDictionary.Key] = parentDictionary.ToString();
//      assertData();
//    }


//    private void assertData() {
//      assertDL();
//      DC.DisposeData();

//      initDL();
//      assertDL();
//    }
//  }
//}
