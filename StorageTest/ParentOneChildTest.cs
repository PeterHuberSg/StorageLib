//using System;
//using System.Collections.Generic;
//using System.IO;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using StorageLib;
//using StorageDataContext;


//namespace StorageTest {

//  [TestClass]
//  public class ParentOneChildTest {


//    CsvConfig? csvConfig;
//    readonly Dictionary<int, string> expectedParents= new Dictionary<int, string>();
//    readonly Dictionary<int, string> expectedParentsNullable= new Dictionary<int, string>();
//    readonly Dictionary<int, string> expectedChildren = new Dictionary<int, string>();


//    [TestMethod]
//    public void TestParentOneChild() {
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

//        var parent = new ParentOneChild_Parent("P0");
//        expectedParents[parent.Key] = parent.ToString();
//        assertData();

//        //create child
//        DC.Data.StartTransaction();
//        parent = DC.Data.ParentOneChild_Parents[parent.Key];
//        new ParentOneChild_Child("C0", parent, null);
//        DC.Data.RollbackTransaction();
//        assertData();
//        DC.Data.StartTransaction();
//        parent = DC.Data.ParentOneChild_Parents[parent.Key];
//        var child = new ParentOneChild_Child("C0", parent, null);
//        expectedChildren[child.Key] = child.ToString();
//        expectedParents[parent.Key] = parent.ToString();
//        DC.Data.CommitTransaction();
//        assertData();

//        //update child
//        //the following 2 parents get stored immediately without transaction
//        var parent1 = new ParentOneChild_Parent("P1");
//        expectedParents[parent1.Key] = parent1.ToString();
//        var parentNullable = new ParentOneChild_ParentNullable("P0N");
//        expectedParentsNullable[parentNullable.Key] = parentNullable.ToString();
//        child = DC.Data.ParentOneChild_Children[child.Key];
//        DC.Data.StartTransaction();
//        child.Update("C0U", parent1, parentNullable);
//        DC.Data.RollbackTransaction();
//        assertData();
//        parent = DC.Data.ParentOneChild_Parents[parent.Key];
//        parent1 = DC.Data.ParentOneChild_Parents[parent1.Key];
//        parentNullable = DC.Data.ParentOneChild_ParentNullables[parentNullable.Key];
//        child = DC.Data.ParentOneChild_Children[child.Key];
//        DC.Data.StartTransaction();
//        child.Update("C0U", parent1, parentNullable);
//        DC.Data.CommitTransaction();
//        expectedChildren[child.Key] = child.ToString();
//        expectedParents[parent.Key] = parent.ToString();
//        expectedParents[parent1.Key] = parent1.ToString();
//        expectedParentsNullable[parentNullable.Key] = parentNullable.ToString();
//        assertData();

//        //update child
//        var parentNullable1 = new ParentOneChild_ParentNullable("P1N");
//        expectedParentsNullable[parentNullable1.Key] = parentNullable1.ToString();
//        parent = DC.Data.ParentOneChild_Parents[parent.Key];
//        child = DC.Data.ParentOneChild_Children[child.Key];
//        DC.Data.StartTransaction();
//        child.Update("C0U1", parent, parentNullable1);
//        DC.Data.RollbackTransaction();
//        assertData();
//        parent = DC.Data.ParentOneChild_Parents[parent.Key];
//        parent1 = DC.Data.ParentOneChild_Parents[parent1.Key];
//        parentNullable = DC.Data.ParentOneChild_ParentNullables[parentNullable.Key];
//        parentNullable1 = DC.Data.ParentOneChild_ParentNullables[parentNullable1.Key];
//        child = DC.Data.ParentOneChild_Children[child.Key];
//        DC.Data.StartTransaction();
//        child.Update("C0U1", parent, parentNullable1);
//        DC.Data.CommitTransaction();
//        expectedChildren[child.Key] = child.ToString();
//        expectedParents[parent.Key] = parent.ToString();
//        expectedParents[parent1.Key] = parent1.ToString();
//        expectedParentsNullable[parentNullable.Key] = parentNullable.ToString();
//        expectedParentsNullable[parentNullable1.Key] = parentNullable1.ToString();
//        assertData();

//        //delete children
//        child = DC.Data.ParentOneChild_Children[child.Key];
//        DC.Data.StartTransaction();
//        child.Release();
//        DC.Data.RollbackTransaction();
//        assertData();
//        parent = DC.Data.ParentOneChild_Parents[parent.Key];
//        parentNullable1 = DC.Data.ParentOneChild_ParentNullables[parentNullable1.Key];
//        child = DC.Data.ParentOneChild_Children[child.Key];
//        DC.Data.StartTransaction();
//        child.Release();
//        DC.Data.CommitTransaction();
//        expectedChildren.Remove(child.Key);
//        expectedParents[parent.Key] = parent.ToString();
//        expectedParentsNullable[parentNullable1.Key] = parentNullable1.ToString();

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
//      Assert.AreEqual(expectedParents.Count, DC.Data.ParentOneChild_Parents.Count);
//      foreach (var parent in DC.Data.ParentOneChild_Parents) {
//        Assert.AreEqual(expectedParents[parent.Key], parent.ToString());
//      }
//      Assert.AreEqual(expectedParentsNullable.Count, DC.Data.ParentOneChild_ParentNullables.Count);
//      foreach (var parent in DC.Data.ParentOneChild_ParentNullables) {
//        Assert.AreEqual(expectedParentsNullable[parent.Key], parent.ToString());
//      }
//      Assert.AreEqual(expectedChildren.Count, DC.Data.ParentOneChild_Children.Count);
//      foreach (var child in DC.Data.ParentOneChild_Children) {
//        Assert.AreEqual(expectedChildren[child.Key], child.ToString());
//      }
//    }


//    private void assertData() {
//      assertDL();
//      DC.DisposeData();

//      initDL();
//      assertDL();
//    }
//  }
//}