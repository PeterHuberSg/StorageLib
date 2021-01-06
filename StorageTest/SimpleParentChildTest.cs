//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.IO;
//using System.Reflection;
//using System.Text;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using StorageLib;
//using TestContext;


//namespace StorageTest {


//  [TestClass]
//  public class SimpleParentChildTest {


//    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor.
//    CsvConfig csvConfig;
//    DC dc;
//    #pragma warning restore CS8618

//    //these variables get updated by assertDataDisposeDCRecreateDCassertData() each time a new data context gets created
//    SimpleParent? parent0;
//    SimpleParent? parent1;
//    SimpleParentN? parentN0;
//    SimpleParentN? parentN1;
//    SimpleParentR? parentR0;
//    SimpleParentNR? parentNR0;
//    SimpleChild? child0;
//    SimpleChild? child1;


//    [TestMethod]
//    public void TestSimpleParentChild() {
//      /* Test explanation:
//       items can be stored or not stored, legal combinations
//                                               child variable name
//       + child not stored, parent not stored   child_
//       + child not stored, parent stored       child__
//       + child stored, parent stored           child

//      for each combination above some test code is written

//      parent variabls names:
//      Parent: the parent property in the child is not nullable
//      ParentN: the parent property in the child is nullable

//      each activity like create, update or delete is done first in a rooled back transaction and none of the data should
//      be changed, then the same activity is executed in a commited transactions and the data should change accordingly.

//      After a transaction has been committed, the datacontext gets disposed, opened again and verified, that the data
//      is still the same. There is one exception: stored parents might have some not stored children. In the new data 
//      context, those parents have no longer those children.

//      For convenience, the variables parent0, parentN1, child1, etc. contain always the data from the latest data context. They
//      get updated, each time assertDataDisposeDCRecreateDCassertData() gets called.

//      Variables with names ending with '_' or '__' are not stored in the data context. They do not get updated by 
//      assertDataDisposeDCRecreateDCassertData() and can therefore contain children or parents stored in previous data
//      contexts.
//      */
//      var directoryInfo = new DirectoryInfo("TestCsv");
//      if (directoryInfo.Exists) {
//        directoryInfo.Delete(recursive: true);
//        directoryInfo.Refresh();
//      }

//      directoryInfo.Create();
//      //initVariables();


//      try {
//        csvConfig = new CsvConfig(directoryInfo.FullName, reportException: reportException);
//        DC.Trace = dcTrace;
//        dc = new DC(csvConfig);
//        assertData("");

//        // Create
//        // ======
//        // Create parent
//        // -------------
//        traceHeader("create not stored parent");
//        dc.StartTransaction();
//        var parent0_ = new SimpleParent("p_Temp", isStoring: false);
//        dc.RollbackTransaction();
//        assertData("");

//        dc.StartTransaction();
//        parent0_ = new SimpleParent("p_", isStoring: false);
//        dc.CommitTransaction();
//        //parent0 = null;
//        assertDataDisposeDCRecreateDCassertData("");


//        traceHeader("create stored parent");
//        dc.StartTransaction();
//        parent0 = new SimpleParent("p0Temp", isStoring: true);
//        dc.RollbackTransaction();
//        assertData("");

//        dc.StartTransaction();
//        parent0 = new SimpleParent("p0", isStoring: true);
//        //parents.Add(parent0);
//        dc.CommitTransaction();
//        assertDataDisposeDCRecreateDCassertData("p0|");

//        // Create child with Parent==parent0, ParentN==null, ParentR==parentR0, ParentNR=parentNR0
//        // ---------------------------------------------------------------------------------------
//        traceHeader("create not stored child with not stored parent");
//        var parentR0_ = new SimpleParentR("pR0_", isStoring: false);
//        var parentNR0_ = new SimpleParentNR("pRN0_", isStoring: false);
//        dc.StartTransaction();
//        var child0_ = new SimpleChild("c0_Temp", parent0_, null, parentR0_, parentNR0_, isStoring: false);
//        dc.RollbackTransaction();
//        Assert.AreEqual(0, parent0_.Children.Count);
//        Assert.AreEqual(0, parentR0_.Children.Count);
//        Assert.AreEqual(0, parentNR0_.Children.Count);
//        assertData("p0|");
//        Assert.AreEqual(0, parent0_.Children.Count);

//        dc.StartTransaction();
//        child0_ = new SimpleChild("c0_", parent0_, null, parentR0_, parentNR0_, isStoring: false);
//        dc.CommitTransaction();
//        Assert.AreEqual("c0_", child0_.Text);
//        Assert.AreEqual(child0_, parent0_.Children[0]);
//        Assert.AreEqual(child0_, parentR0_.Children[0]);
//        Assert.AreEqual(child0_, parentNR0_.Children[0]);
//        assertDataDisposeDCRecreateDCassertData("p0|");


//        traceHeader("create not stored child with stored parent");
//        parentR0 = new SimpleParentR("pR0", isStoring: true);
//        parentNR0 = new SimpleParentNR("pNR0", isStoring: true);
//        dc.StartTransaction();
//        var child0__ = new SimpleChild("c0__Temp", parent0, null, parentR0, parentNR0, isStoring: false);
//        dc.RollbackTransaction();
//        Assert.AreEqual(0, parent0.Children.Count);
//        Assert.AreEqual(0, parentR0.Children.Count);
//        Assert.AreEqual(0, parentNR0.Children.Count);
//        assertData("p0|pR0|pNR0|");

//        dc.StartTransaction();
//        child0__ = new SimpleChild("c0__", parent0, null, parentR0, parentNR0, isStoring: false);
//        dc.CommitTransaction();
//        Assert.AreEqual("c0__", child0__.Text);
//        Assert.AreEqual(parent0, child0__.Parent);
//        Assert.AreEqual(parentR0, child0__.ParentR);
//        Assert.AreEqual(parentNR0, child0__.ParentNR);
//        assertDataDisposeDCRecreateDCassertData("p0:c0__|pR0:c0__|pNR0:c0__|", "p0|pR0|pNR0|");


//        traceHeader("create stored child with stored parent");
//        dc.StartTransaction();
//        child0 = new SimpleChild("c0Temp", parent0, null, parentR0, parentNR0, isStoring: true);
//        dc.RollbackTransaction();
//        assertData("p0|pR0|pNR0|");

//        dc.StartTransaction();
//        child0 = new SimpleChild("c0", parent0, null, parentR0, parentNR0, isStoring: true);
//        dc.CommitTransaction();
//        assertDataDisposeDCRecreateDCassertData("p0:c0|pR0:c0|pNR0:c0|c0:p0,pR0,pNR0|");

//        //Fail to create stored child with not stored parent
//        traceHeader("fail to create stored child with not stored parents");
//        try {
//          var parent0___ = new SimpleParent("p0___", isStoring: false);
//          var parentN0___ = new SimpleParentN("pN0___", isStoring: false);
//          var parentR0___ = new SimpleParentR("pR0___", isStoring: false);
//          var parentNR0___ = new SimpleParentNR("pRN0___", isStoring: false);
//          var _ = new SimpleChild("failed child", parent0___, parentN0___, parentR0___, parentNR0___);
//          Assert.Fail();
//        } catch {
//        }
//        //Todo: Ideally, an exception during create, store or remove should not change any data. Is additional code needed undoing 
//        //any potentially changed data ? 
//        //Assert.AreEqual(0, parent0_.Children.Count);
//        assertDataDisposeDCRecreateDCassertData("p0:c0|pR0:c0|pNR0:c0|c0:p0,pR0,pNR0|");

//        // Update
//        // ======
//        // Update child.Parent to parent1, child.ParentN to parent0N
//        // ---------------------------------------------------------

//        traceHeader("not stored child: update with not stored parents");
//        var parent1_ = new SimpleParent("p1_", isStoring: false);
//        var parentN0_ = new SimpleParentN("pN0___", isStoring: false);
//        //var parentR1_ = new SimpleParent("pR1_", isStoring: false);
//        //var parentNR0_ = new SimpleParentN("pNR0_", isStoring: false);
//        dc.StartTransaction();
//        child0_.Update("c0_.1Temp", parent1_, parentN0_);
//        dc.RollbackTransaction();
//        Assert.AreEqual(child0_, parent0_.Children[0]);
//        Assert.AreEqual(0, parentN0_.Children.Count);
//        Assert.AreEqual(0, parent1_.Children.Count);
//        assertData("p0:c0|pR0:c0|pNR0:c0|c0:p0,pR0,pNR0|");

//        dc.StartTransaction();
//        child0_.Update("c0_.1", parent1_, parentN0_);
//        dc.CommitTransaction();
//        Assert.AreEqual(0, parent0_.Children.Count);
//        Assert.AreEqual(child0_, parentN0_.Children[0]);
//        Assert.AreEqual(child0_, parent1_.Children[0]);
//        assertDataDisposeDCRecreateDCassertData("p0:c0|pR0:c0|pNR0:c0|c0:p0,pR0,pNR0|");


//        traceHeader("not stored child: update with stored parents");
//        parent1 = new SimpleParent("p1", isStoring: true);
//        parentN0 = new SimpleParentN("pN0", isStoring: true);
//        dc.StartTransaction();
//        child0__.Update("c0__.1Temp", parent1, parentN0);
//        dc.RollbackTransaction();
//        Assert.AreEqual("c0__", child0__.Text);
//        Assert.AreEqual(parent0.Text, child0__.Parent.Text);
//        Assert.IsNull(child0__.ParentN);
//        assertData("p0:c0|p1|pN0|pR0:c0|pNR0:c0|c0:p0,pR0,pNR0|");

//        dc.StartTransaction();
//        child0__.Update("c0__.1", parent1, parentN0);
//        dc.CommitTransaction();
//        Assert.AreEqual("c0__.1", child0__.Text);
//        Assert.AreEqual(parent1, child0__.Parent);
//        Assert.AreEqual(parentN0, child0__.ParentN);
//        assertDataDisposeDCRecreateDCassertData(
//          "p0:c0|p1:c0__.1|pN0:c0__.1|pR0:c0|pNR0:c0|c0:p0,pR0,pNR0|",
//          "p0:c0|p1|pN0|pR0:c0|pNR0:c0|c0:p0,pR0,pNR0|");


//        traceHeader("stored child: update with stored parents");
//        dc.StartTransaction();
//        child0.Update("c0.1Temp", parent1, parentN0);
//        dc.RollbackTransaction();
//        assertData("p0:c0|p1|pN0|pR0:c0|pNR0:c0|c0:p0,pR0,pNR0|");

//        dc.StartTransaction();
//        child0.Update("c0.1", parent1, parentN0);
//        dc.CommitTransaction();
//        assertDataDisposeDCRecreateDCassertData("p0|p1:c0.1|pN0:c0.1|pR0:c0.1|pNR0:c0.1|c0.1:p1,pN0,pR0,pNR0|");

//        // Update child.ParentN to parent1N
//        // --------------------------------

//        traceHeader("not stored child: update not stored ParentN");
//        var parentN1_ = new SimpleParentN("pN1_", isStoring: false);
//        dc.StartTransaction();
//        child0_.Update("c0_.2Temp", parent1_, parentN1_);
//        dc.RollbackTransaction();
//        Assert.AreEqual("c0_.1", child0_.Text);
//        Assert.AreEqual(child0_, parent1_.Children[0]);
//        Assert.AreEqual(child0_, parentN0_.Children[0]);
//        Assert.AreEqual(0, parentN1_.Children.Count);
//        assertData("p0|p1:c0.1|pN0:c0.1|pR0:c0.1|pNR0:c0.1|c0.1:p1,pN0,pR0,pNR0|");

//        dc.StartTransaction();
//        child0_.Update("c0_.2", parent1_, parentN1_);
//        dc.CommitTransaction();
//        Assert.AreEqual("c0_.2", child0_.Text);
//        Assert.AreEqual(0, parent0_.Children.Count);
//        Assert.AreEqual(0, parentN0_.Children.Count);
//        Assert.AreEqual(child0_, parent1_.Children[0]);
//        Assert.AreEqual(child0_, parentN1_.Children[0]);
//        assertDataDisposeDCRecreateDCassertData("p0|p1:c0.1|pN0:c0.1|pR0:c0.1|pNR0:c0.1|c0.1:p1,pN0,pR0,pNR0|");


//        traceHeader("not stored child: update stored ParentN");
//        parentN1 = new SimpleParentN("pN1", isStoring: true);
//        dc.StartTransaction();
//        child0__.Update("c0__.2Temp", parent1, parentN1);
//        dc.RollbackTransaction();
//        Assert.AreEqual("c0__.1", child0__.Text);
//        Assert.AreEqual(parent1.Text, child0__.Parent.Text);
//        Assert.AreEqual(parentN0.Text, child0__.ParentN!.Text);
//        assertDataDisposeDCRecreateDCassertData("p0|p1:c0.1|pN0:c0.1|pN1|pR0:c0.1|pNR0:c0.1|c0.1:p1,pN0,pR0,pNR0|");

//        dc.StartTransaction();
//        child0__.Update("c0__.2", parent1, parentN1);
//        dc.CommitTransaction();
//        Assert.AreEqual("c0__.2", child0__.Text);
//        Assert.AreEqual(parent1, child0__.Parent);
//        Assert.AreEqual(parentN1, child0__.ParentN);
//        assertDataDisposeDCRecreateDCassertData(
//          "p0|p1:c0.1,c0__.2|pN0:c0.1|pN1:c0__.2|pR0:c0.1|pNR0:c0.1|c0.1:p1,pN0,pR0,pNR0|",
//          "p0|p1:c0.1|pN0:c0.1|pN1|pR0:c0.1|pNR0:c0.1|c0.1:p1,pN0,pR0,pNR0|");


//        traceHeader("stored child: update stored ParentN");
//        dc.StartTransaction();
//        child0.Update("c0.2Temp", parent1, parentN1);
//        dc.RollbackTransaction();
//        assertData("p0|p1:c0.1|pN0:c0.1|pN1|pR0:c0.1|pNR0:c0.1|c0.1:p1,pN0,pR0,pNR0|");

//        dc.StartTransaction();
//        child0.Update("c0.2", parent1, parentN1);
//        dc.CommitTransaction();
//        assertDataDisposeDCRecreateDCassertData("p0|p1:c0.2|pN0|pN1:c0.2|pR0:c0.2|pNR0:c0.2|c0.2:p1,pN1,pR0,pNR0|");

//        // Update child.ParentN to null
//        // ----------------------------

//        traceHeader("not stored child: update not stored ParentN to null");
//        dc.StartTransaction();
//        child0_.Update("c0_.3Temp", parent1_, null);
//        dc.RollbackTransaction();
//        Assert.AreEqual("c0_.2", child0_.Text);
//        Assert.AreEqual(child0_, parent1_.Children[0]);
//        Assert.AreEqual(child0_, parentN1_.Children[0]);
//        assertData("p0|p1:c0.2|pN0|pN1:c0.2|pR0:c0.2|pNR0:c0.2|c0.2:p1,pN1,pR0,pNR0|");

//        dc.StartTransaction();
//        child0_.Update("c0_.3", parent1_, null);
//        dc.CommitTransaction();
//        Assert.AreEqual("c0_.3", child0_.Text);
//        Assert.AreEqual(0, parentN0_.Children.Count);
//        Assert.AreEqual(child0_, parent1_.Children[0]);
//        assertDataDisposeDCRecreateDCassertData("p0|p1:c0.2|pN0|pN1:c0.2|pR0:c0.2|pNR0:c0.2|c0.2:p1,pN1,pR0,pNR0|");


//        traceHeader("not stored child: update stored ParentN to null");
//        dc.StartTransaction();
//        child0__.Update("c0__.3Temp", parent1, null);
//        dc.RollbackTransaction();
//        Assert.AreEqual("c0__.2", child0__.Text);
//        Assert.AreEqual(parent1.Text, child0__.Parent.Text);
//        Assert.AreEqual(parentN1.Text, child0__.ParentN!.Text);
//        assertDataDisposeDCRecreateDCassertData("p0|p1:c0.2|pN0|pN1:c0.2|pR0:c0.2|pNR0:c0.2|c0.2:p1,pN1,pR0,pNR0|");

//        dc.StartTransaction();
//        child0__.Update("c0__.3", parent1, null);
//        dc.CommitTransaction();
//        Assert.AreEqual("c0__.3", child0__.Text);
//        Assert.AreEqual(parent1, child0__.Parent);
//        Assert.IsNull(child0__.ParentN);
//        assertDataDisposeDCRecreateDCassertData(
//          "p0|p1:c0.2,c0__.3|pN0|pN1:c0.2|pR0:c0.2|pNR0:c0.2|c0.2:p1,pN1,pR0,pNR0|",
//          "p0|p1:c0.2|pN0|pN1:c0.2|pR0:c0.2|pNR0:c0.2|c0.2:p1,pN1,pR0,pNR0|");


//        traceHeader("stored child: update stored ParentN to null");
//        dc.StartTransaction();
//        child0.Update("c0.3Temp", parent1, null);
//        dc.RollbackTransaction();
//        assertData("p0|p1:c0.2|pN0|pN1:c0.2|pR0:c0.2|pNR0:c0.2|c0.2:p1,pN1,pR0,pNR0|");

//        dc.StartTransaction();
//        child0.Update("c0.3", parent1, null);
//        dc.CommitTransaction();
//        assertDataDisposeDCRecreateDCassertData("p0|p1:c0.3|pN0|pN1|pR0:c0.3|pNR0:c0.3|c0.3:p1,pR0,pNR0|");


//        // Release
//        // =======
//        // Fail to release parent with stored child
//        // ----------------------------------------
//        traceHeader("stored child: fail to release parent");
//        try {
//          parent1.Release();
//          Assert.Fail();
//        } catch {
//        }
//        assertData("p0|p1:c0.3|pN0|pN1|pR0:c0.3|pNR0:c0.3|c0.3:p1,pR0,pNR0|");

//        //Release child
//        //-------------
//        child1 = new SimpleChild("c1", parent1, parentN1, parentR0, parentNR0, isStoring: true);
//        assertDataDisposeDCRecreateDCassertData(
//          "p0|p1:c0.3,c1|pN0|pN1:c1|pR0:c0.3,c1|pNR0:c0.3,c1|c0.3:p1,pR0,pNR0|c1:p1,pN1,pR0,pNR0|");
//        dc.StartTransaction();
//        child1.Release();
//        dc.RollbackTransaction();
//        assertData("p0|p1:c0.3,c1|pN0|pN1:c1|pR0:c0.3,c1|pNR0:c0.3,c1|c0.3:p1,pR0,pNR0|c1:p1,pN1,pR0,pNR0|");

//        dc.StartTransaction();
//        child1.Release();
//        dc.CommitTransaction();
//        child1 = null;
//        assertDataDisposeDCRecreateDCassertData(
//          "p0|p1:c0.3,c1|pN0|pN1:c1|pR0:c0.3,c1|pNR0:c0.3,c1|c0.3:p1,pR0,pNR0|",
//          "p0|p1:c0.3|pN0|pN1|pR0:c0.3|pNR0:c0.3|c0.3:p1,pR0,pNR0|");

//        //Release parent
//        //--------------
//        dc.StartTransaction();
//        parent0.Release();
//        dc.RollbackTransaction();
//        assertData("p0|p1:c0.3|pN0|pN1|pR0:c0.3|pNR0:c0.3|c0.3:p1,pR0,pNR0|");

//        dc.StartTransaction();
//        parent0.Release();
//        dc.CommitTransaction();
//        parent0 = null;
//        assertDataDisposeDCRecreateDCassertData("p1:c0.3|pN0|pN1|pR0:c0.3|pNR0:c0.3|c0.3:p1,pR0,pNR0|");


//      } finally {
//        DC.DisposeData();
//      }
//    }


//    private object dcAsString() {
//      if (dc is null) return "";

//      var s = "";
//      append(ref s, dc.SimpleParents);
//      append(ref s, dc.SimpleParentNs);
//      append(ref s, dc.SimpleParentRs);
//      append(ref s, dc.SimpleParentNRs);
//      foreach (var child in dc.SimpleChidren) {
//        s += child.Text + ':' + child.Parent.Text;
//        if (child.ParentN is not null) {
//          s += ',' + child.ParentN.Text;
//        }
//        s += ',' + child.ParentR.Text;
//        if (child.ParentNR is not null) {
//          s += ',' + child.ParentNR.Text;
//        }
//        s += '|';
//      }
//      return s;
//    }

//    private void append<TParent>(ref string s, IEnumerable<TParent> parents) where TParent: class, ITestSimpleParent<SimpleChild> {
//      //var isFirstParent = true;
//      foreach (var parent in parents) {
//        //if (isFirstParent) {
//        //  isFirstParent = false;
//        //} else {
//        //  s += ' ';
//        //}
//        s += parent.Text;
//        if (parent.Children.Count>0) {
//          s += ':';
//          var isFirstChild = true;
//          foreach (var child in parent.Children) {
//            if (isFirstChild) {
//              isFirstChild = false;
//            } else {
//              s += ',';
//            }
//            s += child.Text;
//          }
//        }
//        s += '|';
//      }
//    }

//    private void assertDataDisposeDCRecreateDCassertData(string expectedDcString1, string? expectedDcString2 = null) {
//      assertData(expectedDcString1);
//      DC.DisposeData();
//      dc = new DC(csvConfig);
//      assertData(expectedDcString2??expectedDcString1);
//      if (parent0 is not null) parent0 = dc.SimpleParents[0];
//      if (parent1 is not null) parent1 = dc.SimpleParents[1];
//      if (parentN0 is not null) parentN0 = dc.SimpleParentNs[0];
//      if (parentN1 is not null) parentN1 = dc.SimpleParentNs[1];
//      if (parentR0 is not null) parentR0 = dc.SimpleParentRs[0];
//      if (parentNR0 is not null) parentNR0 = dc.SimpleParentNRs[0];
//      if (child0 is not null) child0 = dc.SimpleChidren[0];
//      if (child1 is not null) child1 = dc.SimpleChidren[1];
//    }


//    private void assertData(string expectedDcString) {
//      Assert.AreEqual(expectedDcString, dcAsString());
//    }


//    private void reportException(Exception obj) {
//      System.Diagnostics.Debug.WriteLine(obj);
//      System.Diagnostics.Debugger.Break();
//      Assert.Fail();
//    }


//    static readonly StringBuilder traceStrinBuilder = new StringBuilder();

//    public static string TracesString {
//      get {
//        return traceStrinBuilder.ToString();
//      }
//    }


//    private static void traceHeader(string line) {
//      traceStrinBuilder.AppendLine();
//      traceStrinBuilder.AppendLine(line);
//    }


//    static bool isInitialisingDcTrace;


//    private static void dcTrace(string message) {
//      if (message=="Context DC initialised") {
//        isInitialisingDcTrace = false;
//      }
//      if (!isInitialisingDcTrace) {
//        traceStrinBuilder.AppendLine(message);
//      }
//      if (message=="Context DC initialising") {
//        isInitialisingDcTrace = true;
//      }
//    }
//  }
//}
