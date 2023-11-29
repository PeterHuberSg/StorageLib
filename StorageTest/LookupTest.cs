using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TestContext;


namespace StorageTest {


  [TestClass]
  public class LookupTest {


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor.
    CsvConfig csvConfig;
    BakCsvFileSwapper bakCsvFileSwapper;
    DC dc;
#pragma warning restore CS8618

    //these variables get updated by assertDataDisposeDCRecreateDCAssertData() each time a new data context gets created
    LookupParent? parent0;
    LookupParent? parent0__;
    LookupParent? parent1;
    LookupParent? parent1__;
    LookupParentN? parentN0;
    LookupParentN? parentN0__;
    LookupParentN? parentN1;
    LookupParentN? parentN1__;
    LookupParentR? parentR0;
    LookupParentR? parentR0__;
    LookupParentR? parentR1;
    LookupParentNR? parentNR0;
    LookupParentNR? parentNR0__;
    LookupParentNR? parentNR1;
    LookupChild? child0;
    LookupChild? child1;


    [TestMethod]
    public void TestLookup() {
      /* Test explanation:
       items can be stored or not stored, legal combinations:
       child variable name | parent variable name
       --------------------+--------------------
       child_: not stored  | parent_: not stored
       child__: not stored | parent__:stored
       child: stored       | parent: stored
       child___: stored    | parent___: not stored    this combination is illegal. Test: is an exception thrown ?

      for each combination above some test code is written

      parent variables names:
      Parent: the parent property in the child is not nullable
      ParentN: the parent property in the child is nullable
      ParentR: the parent property in the child is readonly
      ParentNR: the parent property in the child is nullable and readonly

      each activity like create, update or delete is done first in a rolled back transaction and none of the data should
      be changed, then the same activity is executed in a committed transactions and the data should change accordingly.

      After a transaction has been committed, the dataContext gets disposed, opened again and verified, that the data
      is still the same. This is done twice, first using the .bak files, then the .csv files. There is one exception: 
      stored parents might have some not stored children. In the new data context, those parents have no longer those 
      children.

      For convenience, the variables parent0, parentN1, child1, etc. contain always the data from the latest data context. They
      get updated, each time assertDataDisposeDCRecreateDCAssertData() gets called.

      child_, child__, parent_ and parent___ are not stored in the data context. They do not get updated by 
      assertDataDisposeDCRecreateDCAssertData() and can therefore contain children or parents stored in previous data
      contexts.
      */
      var directoryInfo = new DirectoryInfo("TestCsv");
      if (directoryInfo.Exists) {
        directoryInfo.Delete(recursive: true);
        directoryInfo.Refresh();
      }

      directoryInfo.Create();


      try {
        csvConfig = new CsvConfig(directoryInfo.FullName, reportException: reportException);
        bakCsvFileSwapper = new BakCsvFileSwapper(csvConfig);
        DC.Trace = dcTrace;
        dc = new DC(csvConfig);
        assertData("");

        // Create
        // ======
        // Create parent
        // -------------
        traceHeader("create not stored parent");
        dc.StartTransaction();
        var parent0_ = new LookupParent("p_Temp", isStoring: false);
        dc.RollbackTransaction();
        assertData("");

        dc.StartTransaction();
        parent0_ = new LookupParent("p_", isStoring: false);
        dc.CommitTransaction();
        assertDataDisposeDCRecreateDCAssertData("");


        traceHeader("create stored parent");
        dc.StartTransaction();
        parent0 = new LookupParent("p0Temp", isStoring: true);
        dc.RollbackTransaction();
        assertData("");

        dc.StartTransaction();
        parent0 = new LookupParent("p0", isStoring: true);
        dc.CommitTransaction();
        assertDataDisposeDCRecreateDCAssertData("p0|");

        // Create child with Parent==parent0, ParentN==null, ParentR==parentR0, ParentNR=parentNR0
        // ---------------------------------------------------------------------------------------
        traceHeader("create not stored child with not stored parent");
        var parentR0_ = new LookupParentR("pR0_", isStoring: false);
        var parentNR0_ = new LookupParentNR("pRN0_", isStoring: false);
        dc.StartTransaction();
        var child0_ = new LookupChild("c0_Temp", parent0_, null, parentR0_, parentNR0_, isStoring: false);
        dc.RollbackTransaction();
        assertData("p0|");

        dc.StartTransaction();
        child0_ = new LookupChild("c0_", parent0_, null, parentR0_, parentNR0_, isStoring: false);
        dc.CommitTransaction();
        Assert.AreEqual("c0_", child0_.Text);
        Assert.AreEqual(child0_.Parent, parent0_);
        Assert.AreEqual(child0_.ParentR, parentR0_);
        Assert.AreEqual(child0_.ParentNR, parentNR0_);
        assertDataDisposeDCRecreateDCAssertData("p0|");


        traceHeader("create not stored child with stored parent");
        parent0__ = new LookupParent("p0__", isStoring: true);
        parentR0__ = new LookupParentR("pR0__", isStoring: true);
        parentNR0__ = new LookupParentNR("pNR0__", isStoring: true);
        dc.StartTransaction();
        var child0__ = new LookupChild("c0__Temp", parent0__, null, parentR0__, parentNR0__, isStoring: false);
        dc.RollbackTransaction();
        assertData("p0|p0__|pR0__|pNR0__|");

        dc.StartTransaction();
        child0__ = new LookupChild("c0__", parent0__, null, parentR0__, parentNR0__, isStoring: false);
        dc.CommitTransaction();
        Assert.AreEqual("c0__", child0__.Text);
        Assert.AreEqual(parent0__, child0__.Parent);
        Assert.AreEqual(parentR0__, child0__.ParentR);
        Assert.AreEqual(parentNR0__, child0__.ParentNR);
        assertDataDisposeDCRecreateDCAssertData("p0|p0__|pR0__|pNR0__|", "p0|p0__|pR0__|pNR0__|");


        traceHeader("create stored child with stored parent");
        parentR0 = new LookupParentR("pR0", isStoring: true);
        parentNR0 = new LookupParentNR("pNR0", isStoring: true);
        dc.StartTransaction();
        child0 = new LookupChild("c0Temp", parent0, null, parentR0, parentNR0, isStoring: true);
        dc.RollbackTransaction();
        assertData("p0|p0__|pR0__|pR0|pNR0__|pNR0|");

        dc.StartTransaction();
        child0 = new LookupChild("c0", parent0, null, parentR0, parentNR0, isStoring: true);
        dc.CommitTransaction();
        assertDataDisposeDCRecreateDCAssertData("p0|p0__|pR0__|pR0|pNR0__|pNR0|c0:p0,pR0,pNR0|");

        //Fail to create stored child with not stored parent
        traceHeader("fail to create stored child with not stored parents");
        try {
          var parent0___ = new LookupParent("p0___", isStoring: false);
          var parentN0___ = new LookupParentN("pN0___", isStoring: false);
          var parentR0___ = new LookupParentR("pR0___", isStoring: false);
          var parentNR0___ = new LookupParentNR("pRN0___", isStoring: false);
          var child___ = new LookupChild("failed child", parent0___, parentN0___, parentR0___, parentNR0___);
          Assert.Fail();
        } catch {
        }
        //Todo: Ideally, an exception during create, store or remove should not change any data. Is additional code needed undoing 
        //any potentially changed data ? 
        //Assert.AreEqual(0, parent0_.Children.Count);
        assertDataDisposeDCRecreateDCAssertData("p0|p0__|pR0__|pR0|pNR0__|pNR0|c0:p0,pR0,pNR0|");

        // Update
        // ======
        // Update child.Parent to parent1, child.ParentN to parent0N
        // ---------------------------------------------------------

        traceHeader("not stored child: update with not stored parents");
        var parent1_ = new LookupParent("p1_", isStoring: false);
        var parentN0_ = new LookupParentN("pN0___", isStoring: false);
        dc.StartTransaction();
        child0_.Update("c0_.1Temp", parent1_, parentN0_);
        dc.RollbackTransaction();
        Assert.AreEqual(child0_.Parent, parent0_);
        assertData("p0|p0__|pR0__|pR0|pNR0__|pNR0|c0:p0,pR0,pNR0|");

        dc.StartTransaction();
        child0_.Update("c0_.1", parent1_, parentN0_);
        dc.CommitTransaction();
        Assert.AreEqual(child0_.ParentN, parentN0_);
        Assert.AreEqual(child0_.Parent, parent1_);
        assertDataDisposeDCRecreateDCAssertData("p0|p0__|pR0__|pR0|pNR0__|pNR0|c0:p0,pR0,pNR0|");


        traceHeader("not stored child: update with stored parents");
        parent1__ = new LookupParent("p1__", isStoring: true);
        parentN0__ = new LookupParentN("pN0__", isStoring: true);
        dc.StartTransaction();
        child0__.Update("c0__.1Temp", parent1__, parentN0__);
        dc.RollbackTransaction();
        Assert.AreEqual("c0__", child0__.Text);
        Assert.AreEqual(parent0__.Text, child0__.Parent.Text);
        Assert.IsNull(child0__.ParentN);
        assertData("p0|p0__|p1__|pN0__|pR0__|pR0|pNR0__|pNR0|c0:p0,pR0,pNR0|");

        dc.StartTransaction();
        child0__.Update("c0__.1", parent1__, parentN0__);
        dc.CommitTransaction();
        Assert.AreEqual("c0__.1", child0__.Text);
        Assert.AreEqual(parent1__, child0__.Parent);
        Assert.AreEqual(parentN0__, child0__.ParentN);
        assertDataDisposeDCRecreateDCAssertData(
          "p0|p0__|p1__|pN0__|pR0__|pR0|pNR0__|pNR0|c0:p0,pR0,pNR0|");


        traceHeader("stored child: update with stored parents");
        parent1 = new LookupParent("p1", isStoring: true);
        parentN0 = new LookupParentN("pN0", isStoring: true);
        dc.StartTransaction();
        child0.Update("c0.1Temp", parent1, parentN0);
        dc.RollbackTransaction();
        assertData("p0|p0__|p1__|p1|pN0__|pN0|pR0__|pR0|pNR0__|pNR0|c0:p0,pR0,pNR0|");

        dc.StartTransaction();
        child0.Update("c0.1", parent1, parentN0);
        dc.CommitTransaction();
        assertDataDisposeDCRecreateDCAssertData("p0|p0__|p1__|p1|pN0__|pN0|pR0__|pR0|pNR0__|pNR0|c0.1:p1,pN0,pR0,pNR0|");

        // Update child.ParentN to parent1N
        // --------------------------------

        traceHeader("not stored child: update not stored ParentN");
        var parentN1_ = new LookupParentN("pN1_", isStoring: false);
        dc.StartTransaction();
        child0_.Update("c0_.2Temp", parent1_, parentN1_);
        dc.RollbackTransaction();
        Assert.AreEqual("c0_.1", child0_.Text);
        Assert.AreEqual(child0_.Parent, parent1_);
        Assert.AreEqual(child0_.ParentN, parentN0_);
        assertData("p0|p0__|p1__|p1|pN0__|pN0|pR0__|pR0|pNR0__|pNR0|c0.1:p1,pN0,pR0,pNR0|");

        dc.StartTransaction();
        child0_.Update("c0_.2", parent1_, parentN1_);
        dc.CommitTransaction();
        Assert.AreEqual("c0_.2", child0_.Text);
        Assert.AreEqual(child0_.Parent, parent1_);
        Assert.AreEqual(child0_.ParentN, parentN1_);
        assertDataDisposeDCRecreateDCAssertData("p0|p0__|p1__|p1|pN0__|pN0|pR0__|pR0|pNR0__|pNR0|c0.1:p1,pN0,pR0,pNR0|");


        traceHeader("not stored child: update stored ParentN");
        parentN1__ = new LookupParentN("pN1__", isStoring: true);
        dc.StartTransaction();
        child0__.Update("c0__.2Temp", parent1__, parentN1__);
        dc.RollbackTransaction();
        Assert.AreEqual("c0__.1", child0__.Text);
        Assert.AreEqual(parent1__.Text, child0__.Parent.Text);
        Assert.AreEqual(parentN0__.Text, child0__.ParentN!.Text);
        assertDataDisposeDCRecreateDCAssertData(
          "p0|p0__|p1__|p1|pN0__|pN0|pN1__|pR0__|pR0|pNR0__|pNR0|c0.1:p1,pN0,pR0,pNR0|");

        dc.StartTransaction();
        child0__.Update("c0__.2", parent1__, parentN1__);
        dc.CommitTransaction();
        Assert.AreEqual("c0__.2", child0__.Text);
        Assert.AreEqual(parent1__, child0__.Parent);
        Assert.AreEqual(parentN1__, child0__.ParentN);
        assertDataDisposeDCRecreateDCAssertData(
          "p0|p0__|p1__|p1|pN0__|pN0|pN1__|pR0__|pR0|pNR0__|pNR0|c0.1:p1,pN0,pR0,pNR0|");


        traceHeader("stored child: update stored ParentN");
        parentN1 = new LookupParentN("pN1", isStoring: true);
        dc.StartTransaction();
        child0.Update("c0.2Temp", parent1, parentN1);
        dc.RollbackTransaction();
        assertData("p0|p0__|p1__|p1|pN0__|pN0|pN1__|pN1|pR0__|pR0|pNR0__|pNR0|c0.1:p1,pN0,pR0,pNR0|");

        dc.StartTransaction();
        child0.Update("c0.2", parent1, parentN1);
        dc.CommitTransaction();
        assertDataDisposeDCRecreateDCAssertData(
          "p0|p0__|p1__|p1|pN0__|pN0|pN1__|pN1|pR0__|pR0|pNR0__|pNR0|c0.2:p1,pN1,pR0,pNR0|");

        // Update child.ParentN to null
        // ----------------------------

        traceHeader("not stored child: update not stored ParentN to null");
        dc.StartTransaction();
        child0_.Update("c0_.3Temp", parent1_, null);
        dc.RollbackTransaction();
        Assert.AreEqual("c0_.2", child0_.Text);
        Assert.AreEqual(child0_.Parent, parent1_);
        Assert.AreEqual(child0_.ParentN, parentN1_);
        assertData("p0|p0__|p1__|p1|pN0__|pN0|pN1__|pN1|pR0__|pR0|pNR0__|pNR0|c0.2:p1,pN1,pR0,pNR0|");

        dc.StartTransaction();
        child0_.Update("c0_.3", parent1_, null);
        dc.CommitTransaction();
        Assert.AreEqual("c0_.3", child0_.Text);
        Assert.AreEqual(child0_.Parent, parent1_);
        assertDataDisposeDCRecreateDCAssertData(
          "p0|p0__|p1__|p1|pN0__|pN0|pN1__|pN1|pR0__|pR0|pNR0__|pNR0|c0.2:p1,pN1,pR0,pNR0|");


        traceHeader("not stored child: update stored ParentN to null");
        dc.StartTransaction();
        child0__.Update("c0__.3Temp", parent1__, null);
        dc.RollbackTransaction();
        Assert.AreEqual("c0__.2", child0__.Text);
        Assert.AreEqual(parent1__.Text, child0__.Parent.Text);
        Assert.AreEqual(parentN1__.Text, child0__.ParentN!.Text);
        assertDataDisposeDCRecreateDCAssertData(
          "p0|p0__|p1__|p1|pN0__|pN0|pN1__|pN1|pR0__|pR0|pNR0__|pNR0|c0.2:p1,pN1,pR0,pNR0|");

        dc.StartTransaction();
        child0__.Update("c0__.3", parent1__, null);
        dc.CommitTransaction();
        Assert.AreEqual("c0__.3", child0__.Text);
        Assert.AreEqual(parent1__, child0__.Parent);
        Assert.IsNull(child0__.ParentN);
        assertDataDisposeDCRecreateDCAssertData(
          "p0|p0__|p1__|p1|pN0__|pN0|pN1__|pN1|pR0__|pR0|pNR0__|pNR0|c0.2:p1,pN1,pR0,pNR0|");


        traceHeader("stored child: update stored ParentN to null");
        dc.StartTransaction();
        child0.Update("c0.3Temp", parent1, null);
        dc.RollbackTransaction();
        assertData("p0|p0__|p1__|p1|pN0__|pN0|pN1__|pN1|pR0__|pR0|pNR0__|pNR0|c0.2:p1,pN1,pR0,pNR0|");

        dc.StartTransaction();
        child0.Update("c0.3", parent1, null);
        dc.CommitTransaction();
        assertDataDisposeDCRecreateDCAssertData(
          "p0|p0__|p1__|p1|pN0__|pN0|pN1__|pN1|pR0__|pR0|pNR0__|pNR0|c0.3:p1,pR0,pNR0|");


        // Release
        // =======
        // Fail to release parent with stored child
        // ----------------------------------------
        traceHeader("stored child: fail to release parent");
        try {
          parent1.Release();
          Assert.Fail();
        } catch {
        }
        assertData("p0|p0__|p1__|p1|pN0__|pN0|pN1__|pN1|pR0__|pR0|pNR0__|pNR0|c0.3:p1,pR0,pNR0|");
        bakCsvFileSwapper.DeleteBakFiles();

        //Release child
        //-------------
        parentR1 = new LookupParentR("pR1", isStoring: true);
        parentNR1 = new LookupParentNR("pNR1", isStoring: true);
        child1 = new LookupChild("c1", parent0, parentN1, parentR1, parentNR1, isStoring: true);
        assertDataDisposeDCRecreateDCAssertData(
          "p0|p0__|p1__|p1|pN0__|pN0|pN1__|pN1|pR0__|pR0|pR1|pNR0__|pNR0|pNR1|c0.3:p1,pR0,pNR0|c1:p0,pN1,pR1,pNR1|");
        dc.StartTransaction();
        child1.Release();
        dc.RollbackTransaction();
        assertData("p0|p0__|p1__|p1|pN0__|pN0|pN1__|pN1|pR0__|pR0|pR1|pNR0__|pNR0|pNR1|c0.3:p1,pR0,pNR0|c1:p0,pN1,pR1,pNR1|");

        dc.StartTransaction();
        child1.Release();
        dc.CommitTransaction();
        child1 = null;
        assertDataDisposeDCRecreateDCAssertData(
          "p0|p0__|p1__|p1|pN0__|pN0|pN1__|pN1|pR0__|pR0|pR1|pNR0__|pNR0|pNR1|c0.3:p1,pR0,pNR0|");

      } finally {
        DC.DisposeData();
      }
    }


    private object dcAsString() {
      if (dc is null) return "";

      var s = "";
      append(ref s, dc.LookupParents);
      append(ref s, dc.LookupParentNs);
      append(ref s, dc.LookupParentRs);
      append(ref s, dc.LookupParentNRs);
      foreach (var child in dc.LookupChildren) {
        s += child.Text + ':' + child.Parent.Text;
        if (child.ParentN is not null) {
          s += ',' + child.ParentN.Text;
        }
        s += ',' + child.ParentR.Text;
        if (child.ParentNR is not null) {
          s += ',' + child.ParentNR.Text;
        }
        s += '|';
      }
      return s;
    }


    private static void append(
      ref string s,
      IEnumerable<ILookupParent> parents) {
      foreach (var parent in parents) {
        s += parent.Text;
        s += '|';
      }
    }


    private void assertDataDisposeDCRecreateDCAssertData(string expectedDcString1, string? expectedDcString2 = null) {
      assertData(expectedDcString1);
      DC.DisposeData();

      if (bakCsvFileSwapper.UseBackupFiles()) {
        dc = new DC(csvConfig);
        assertData(expectedDcString2??expectedDcString1);
        DC.DisposeData();
        bakCsvFileSwapper.SwapBack();
      }

      dc = new DC(csvConfig);
      assertData(expectedDcString2??expectedDcString1);
      if (parent0__ is not null) parent0__ = dc.LookupParents[1];
      if (parent0 is not null) parent0 = dc.LookupParents[0];
      if (parent1__ is not null) parent1__ = dc.LookupParents[2];
      if (parent1 is not null) parent1 = dc.LookupParents[3];
      if (parentN0__ is not null) parentN0__ = dc.LookupParentNs[0];
      if (parentN0 is not null) parentN0 = dc.LookupParentNs[1];
      if (parentN1__ is not null) parentN1__ = dc.LookupParentNs[2];
      if (parentN1 is not null) parentN1 = dc.LookupParentNs[3];
      if (parentR0__ is not null) parentR0__ = dc.LookupParentRs[0];
      if (parentR0 is not null) parentR0 = dc.LookupParentRs[1];
      if (parentR1 is not null) parentR1 = dc.LookupParentRs[2];
      if (parentNR0__ is not null) parentNR0__ = dc.LookupParentNRs[0];
      if (parentNR0 is not null) parentNR0 = dc.LookupParentNRs[1];
      if (parentNR1 is not null) parentNR1 = dc.LookupParentNRs[2];
      if (child0 is not null) child0 = dc.LookupChildren[0];
      if (child1 is not null) child1 = dc.LookupChildren[1];
    }


    private void assertData(string expectedDcString) {
      var actualString = dcAsString();
      Assert.AreEqual(expectedDcString, actualString);
    }


    private void reportException(Exception obj) {
      System.Diagnostics.Debug.WriteLine(obj);
      System.Diagnostics.Debugger.Break();
      Assert.Fail();
    }


    static readonly StringBuilder traceStringBuilder = new StringBuilder();

    public static string TracesString {
      get {
        return traceStringBuilder.ToString();
      }
    }


    private static void traceHeader(string line) {
      traceStringBuilder.AppendLine();
      traceStringBuilder.AppendLine(line);
    }


    static bool isInitialisingDcTrace;


    private static void dcTrace(string message) {
      if (message=="Context DC initialised") {
        isInitialisingDcTrace = false;
      }
      if (!isInitialisingDcTrace) {
        traceStringBuilder.AppendLine(message);
      }
      if (message=="Context DC initialising") {
        isInitialisingDcTrace = true;
      }
    }
  }
}