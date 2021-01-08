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
    DC dc;
#pragma warning restore CS8618

    //these variables get updated by assertDataDisposeDCRecreateDCassertData() each time a new data context gets created
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

      parent variabls names:
      Parent: the parent property in the child is not nullable
      ParentN: the parent property in the child is nullable
      ParentR: the parent property in the child is readonly
      ParentNR: the parent property in the child is nullable and readonly

      each activity like create, update or delete is done first in a rolled back transaction and none of the data should
      be changed, then the same activity is executed in a commited transactions and the data should change accordingly.

      After a transaction has been committed, the datacontext gets disposed, opened again and verified, that the data
      is still the same. There is one exception: stored parents might have some not stored children. In the new data 
      context, those parents have no longer those children.

      For convenience, the variables parent0, parentN1, child1, etc. contain always the data from the latest data context. They
      get updated, each time assertDataDisposeDCRecreateDCassertData() gets called.

      child_, child__, parent_ and parent___ are not stored in the data context. They do not get updated by 
      assertDataDisposeDCRecreateDCassertData() and can therefore contain children or parents stored in previous data
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
        assertDataDisposeDCRecreateDCassertData("");


        traceHeader("create stored parent");
        dc.StartTransaction();
        parent0 = new LookupParent("p0Temp", isStoring: true);
        dc.RollbackTransaction();
        assertData("");

        dc.StartTransaction();
        parent0 = new LookupParent("p0", isStoring: true);
        dc.CommitTransaction();
        assertDataDisposeDCRecreateDCassertData("p0|");

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
        assertDataDisposeDCRecreateDCassertData("p0|");


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
        assertDataDisposeDCRecreateDCassertData("p0|p0__|pR0__|pNR0__|", "p0|p0__|pR0__|pNR0__|");


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
        assertDataDisposeDCRecreateDCassertData("p0|p0__|pR0__|pR0|pNR0__|pNR0|c0:p0,pR0,pNR0|");

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
        assertDataDisposeDCRecreateDCassertData("p0|p0__|pR0__|pR0|pNR0__|pNR0|c0:p0,pR0,pNR0|");

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
        assertDataDisposeDCRecreateDCassertData("p0|p0__|pR0__|pR0|pNR0__|pNR0|c0:p0,pR0,pNR0|");


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
        assertDataDisposeDCRecreateDCassertData(
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
        assertDataDisposeDCRecreateDCassertData("p0|p0__|p1__|p1|pN0__|pN0|pR0__|pR0|pNR0__|pNR0|c0.1:p1,pN0,pR0,pNR0|");

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
        assertDataDisposeDCRecreateDCassertData("p0|p0__|p1__|p1|pN0__|pN0|pR0__|pR0|pNR0__|pNR0|c0.1:p1,pN0,pR0,pNR0|");


        traceHeader("not stored child: update stored ParentN");
        parentN1__ = new LookupParentN("pN1__", isStoring: true);
        dc.StartTransaction();
        child0__.Update("c0__.2Temp", parent1__, parentN1__);
        dc.RollbackTransaction();
        Assert.AreEqual("c0__.1", child0__.Text);
        Assert.AreEqual(parent1__.Text, child0__.Parent.Text);
        Assert.AreEqual(parentN0__.Text, child0__.ParentN!.Text);
        assertDataDisposeDCRecreateDCassertData(
          "p0|p0__|p1__|p1|pN0__|pN0|pN1__|pR0__|pR0|pNR0__|pNR0|c0.1:p1,pN0,pR0,pNR0|");

        dc.StartTransaction();
        child0__.Update("c0__.2", parent1__, parentN1__);
        dc.CommitTransaction();
        Assert.AreEqual("c0__.2", child0__.Text);
        Assert.AreEqual(parent1__, child0__.Parent);
        Assert.AreEqual(parentN1__, child0__.ParentN);
        assertDataDisposeDCRecreateDCassertData(
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
        assertDataDisposeDCRecreateDCassertData(
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
        assertDataDisposeDCRecreateDCassertData(
          "p0|p0__|p1__|p1|pN0__|pN0|pN1__|pN1|pR0__|pR0|pNR0__|pNR0|c0.2:p1,pN1,pR0,pNR0|");


        traceHeader("not stored child: update stored ParentN to null");
        dc.StartTransaction();
        child0__.Update("c0__.3Temp", parent1__, null);
        dc.RollbackTransaction();
        Assert.AreEqual("c0__.2", child0__.Text);
        Assert.AreEqual(parent1__.Text, child0__.Parent.Text);
        Assert.AreEqual(parentN1__.Text, child0__.ParentN!.Text);
        assertDataDisposeDCRecreateDCassertData(
          "p0|p0__|p1__|p1|pN0__|pN0|pN1__|pN1|pR0__|pR0|pNR0__|pNR0|c0.2:p1,pN1,pR0,pNR0|");

        dc.StartTransaction();
        child0__.Update("c0__.3", parent1__, null);
        dc.CommitTransaction();
        Assert.AreEqual("c0__.3", child0__.Text);
        Assert.AreEqual(parent1__, child0__.Parent);
        Assert.IsNull(child0__.ParentN);
        assertDataDisposeDCRecreateDCassertData(
          "p0|p0__|p1__|p1|pN0__|pN0|pN1__|pN1|pR0__|pR0|pNR0__|pNR0|c0.2:p1,pN1,pR0,pNR0|");


        traceHeader("stored child: update stored ParentN to null");
        dc.StartTransaction();
        child0.Update("c0.3Temp", parent1, null);
        dc.RollbackTransaction();
        assertData("p0|p0__|p1__|p1|pN0__|pN0|pN1__|pN1|pR0__|pR0|pNR0__|pNR0|c0.2:p1,pN1,pR0,pNR0|");

        dc.StartTransaction();
        child0.Update("c0.3", parent1, null);
        dc.CommitTransaction();
        assertDataDisposeDCRecreateDCassertData(
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

        //Release child
        //-------------
        parentR1 = new LookupParentR("pR1", isStoring: true);
        parentNR1 = new LookupParentNR("pNR1", isStoring: true);
        child1 = new LookupChild("c1", parent0, parentN1, parentR1, parentNR1, isStoring: true);
        assertDataDisposeDCRecreateDCassertData(
          "p0|p0__|p1__|p1|pN0__|pN0|pN1__|pN1|pR0__|pR0|pR1|pNR0__|pNR0|pNR1|c0.3:p1,pR0,pNR0|c1:p0,pN1,pR1,pNR1|");
        dc.StartTransaction();
        child1.Release();
        dc.RollbackTransaction();
        assertData("p0|p0__|p1__|p1|pN0__|pN0|pN1__|pN1|pR0__|pR0|pR1|pNR0__|pNR0|pNR1|c0.3:p1,pR0,pNR0|c1:p0,pN1,pR1,pNR1|");

        dc.StartTransaction();
        child1.Release();
        dc.CommitTransaction();
        child1 = null;
        assertDataDisposeDCRecreateDCassertData(
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


    private void assertDataDisposeDCRecreateDCassertData(string expectedDcString1, string? expectedDcString2 = null) {
      assertData(expectedDcString1);
      DC.DisposeData();
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


    static readonly StringBuilder traceStrinBuilder = new StringBuilder();

    public static string TracesString {
      get {
        return traceStrinBuilder.ToString();
      }
    }


    private static void traceHeader(string line) {
      traceStrinBuilder.AppendLine();
      traceStrinBuilder.AppendLine(line);
    }


    static bool isInitialisingDcTrace;


    private static void dcTrace(string message) {
      if (message=="Context DC initialised") {
        isInitialisingDcTrace = false;
      }
      if (!isInitialisingDcTrace) {
        traceStrinBuilder.AppendLine(message);
      }
      if (message=="Context DC initialising") {
        isInitialisingDcTrace = true;
      }
    }
  }
}


#region Old Test
//      --------

//////////////////////////////////////////////////////////////
// Todo: Lookups should only be allowed for undeletable parents or copy
//
// Lookups should only be allowed for undeletable parents. Otherwise it's too complicated what to
// do when the parent gets deleted. Idea: Lookup could copy the actual values into the child, so when the
// lookup parent gets deleted, the value is still valid in the child
//////////////////////////////////////////////////////////////
//using System;
//using System.Collections.Generic;
//using System.IO;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using StorageLib;
//using TestContext;


//namespace StorageTest {


//  [TestClass]
//  public class LookupTest {


//    CsvConfig? csvConfig;
//    readonly Dictionary<int, string> expectedParents = new Dictionary<int, string>();
//    readonly Dictionary<int, string> expectedParentsNullable = new Dictionary<int, string>();
//    readonly Dictionary<int, string> expectedChildren= new Dictionary<int, string>();


//    [TestMethod]
//    public void TestLookup() {
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

//        //stored immediately
//        var now = DateTime.Now.Date;
//        var dayIndex = 1;
//        var parent1Key = addParent(now, 1, isStoring: true).Key;
//        var parent1NullableKey = addParentNullable(now, 1, isStoring: true).Key;
//        var child1Key = addChild("1", parent1Key, parent1NullableKey, isStoring: true).Key;

//        var parent2Key = addParent(now.AddDays(dayIndex), 2, isStoring: true).Key;
//        var parent2NullableKey = addParentNullable(now.AddDays(dayIndex++), 2, isStoring: true).Key;
//        addChild("2", parent2Key, parent2NullableKey, isStoring: true);
//        addChild("3", parent2Key, parent2NullableKey, isStoring: true);

//        //not stored
//        var parent3 = addParent(now.AddDays(dayIndex++), 3, isStoring: false);
//        var child4 = addChild("4", parent3, null, isStoring: false);
//        DC.Data.StartTransaction();
//        parent3.Store();
//        child4.Store();
//        DC.Data.RollbackTransaction();
//        DC.Data.StartTransaction();
//        store(parent3);
//        store(child4);
//        DC.Data.CommitTransaction();
//        assertData();

//        var parent4 = addParent(now.AddDays(dayIndex), 4, isStoring: false);
//        var parent4Nullable = addParentNullable(now.AddDays(dayIndex++), 4, isStoring: false);
//        var child5 = addChild("5", parent4, parent4Nullable, isStoring: false);
//        var child6 = addChild("6", parent4, parent4Nullable, isStoring: false);
//        DC.Data.StartTransaction();
//        parent4.Store();
//        parent4Nullable.Store();
//        child5.Store();
//        child6.Store();
//        DC.Data.RollbackTransaction();
//        DC.Data.StartTransaction();
//        store(parent4);
//        store(parent4Nullable);
//        store(child5);
//        store(child6);
//        DC.Data.CommitTransaction();
//        assertData();

//        //test update()
//        updateParent(parent2Key, 2.1m);
//        updateParentNullable(parent2NullableKey, 2.1m);
//        updateChild(child1Key, parent2Key, parent2NullableKey, "11.U1");
//        updateChild(child1Key, parent2Key, parent2NullableKey, "11.U2");
//        updateChild(child1Key, parent1Key, parent1NullableKey, "11.U3");
//        updateChild(child1Key, parent1Key, null, "11.U4");
//        updateChild(child1Key, parent1Key, parent1NullableKey, "11.U5");

//        removeChild(child1Key);
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
//      Assert.AreEqual(expectedParents.Count, DC.Data.Lookup_Parents.Count);
//      foreach (var parent in DC.Data.Lookup_Parents) {
//        Assert.AreEqual(expectedParents[parent.Key], parent.ToString());
//      }

//      Assert.AreEqual(expectedParentsNullable.Count, DC.Data.Lookup_ParentNullables.Count);
//      foreach (var parentNullable in DC.Data.Lookup_ParentNullables) {
//        Assert.AreEqual(expectedParentsNullable[parentNullable.Key], parentNullable.ToString());
//      }

//      Assert.AreEqual(expectedChildren.Count, DC.Data.Lookup_Children.Count);
//      foreach (var child in DC.Data.Lookup_Children) {
//        Assert.AreEqual(expectedChildren[child.Key], child.ToString());
//      }
//    }


//    private Lookup_Parent addParent(DateTime date, decimal someValue, bool isStoring) {
//      //var newParent = new Lookup_Parent(date, someValue, isStoring);
//      //if (isStoring) {
//      //  expectedParents.Add(newParent.Key, newParent.ToString());
//      //  assertData();
//      //}
//      //return newParent;
//      if (isStoring) {
//        DC.Data.StartTransaction();
//        new Lookup_Parent(date, someValue, isStoring);
//        DC.Data.RollbackTransaction();
//        assertData();

//        DC.Data.StartTransaction();
//        var newParent = new Lookup_Parent(date, someValue, isStoring);
//        DC.Data.CommitTransaction();
//        expectedParents.Add(newParent.Key, newParent.ToString());
//        assertData();
//        return newParent;
//      } else {
//        return new Lookup_Parent(date, someValue, isStoring);
//      }
//    }


//    private void store(Lookup_Parent newParent) {
//      newParent.Store();
//      expectedParents.Add(newParent.Key, newParent.ToString());
//    }


//    private Lookup_ParentNullable addParentNullable(DateTime date, decimal someValue, bool isStoring) {
//      //var newParentNullable = new Lookup_ParentNullable(date, someValue, isStoring);
//      //if (isStoring) {
//      //  expectedParentsNullable.Add(newParentNullable.Key, newParentNullable.ToString());
//      //  assertData();
//      //}
//      //return newParentNullable;
//      if (isStoring) {
//        DC.Data.StartTransaction();
//        new Lookup_ParentNullable(date, someValue, isStoring);
//        DC.Data.RollbackTransaction();
//        assertData();

//        DC.Data.StartTransaction();
//        var newParentNullable = new Lookup_ParentNullable(date, someValue, isStoring);
//        DC.Data.CommitTransaction();
//        expectedParentsNullable.Add(newParentNullable.Key, newParentNullable.ToString());
//        assertData();
//        return newParentNullable;
//      } else {
//        return new Lookup_ParentNullable(date, someValue, isStoring);
//      }
//    }


//    private void store(Lookup_ParentNullable newParentNullable) {
//      newParentNullable.Store();
//      expectedParentsNullable.Add(newParentNullable.Key, newParentNullable.ToString());
//    }


//    private Lookup_Child addChild(string info, int parentKey, int? parentNullableKey, bool isStoring) {
//      //var parent = DC.Data.Lookup_Parents[parentKey];
//      //Lookup_ParentNullable? parentNullable = null;
//      //if (parentNullableKey.HasValue) {
//      //  parentNullable = DC.Data.Lookup_ParentNullables[parentNullableKey.Value];
//      //}
//      //var newChild = new Lookup_Child(info, parent, parentNullable, isStoring);
//      //if (isStoring) {
//      //  expectedChildren.Add(newChild.Key, newChild.ToString());
//      //  assertData();
//      //}
//      //return newChild;
//      var parent = DC.Data.Lookup_Parents[parentKey];
//      Lookup_ParentNullable? parentNullable = null;
//      if (parentNullableKey.HasValue) {
//        parentNullable = DC.Data.Lookup_ParentNullables[parentNullableKey.Value];
//      }

//      return addChild(info, parent, parentNullable, isStoring);
//    }


//    private Lookup_Child addChild(string info, Lookup_Parent parent, Lookup_ParentNullable? parentNullable, bool isStoring){
//      //var newChild = new Lookup_Child(info, parent, parentNullable, isStoring);
//      //if (isStoring) {
//      //  expectedChildren.Add(newChild.Key, newChild.ToString());
//      //  assertData();
//      //}
//      //return newChild;
//      if (isStoring) {
//        DC.Data.StartTransaction();
//        new Lookup_Child(info, parent, parentNullable, isStoring: true);
//        DC.Data.RollbackTransaction();
//        assertData();

//        parent = DC.Data.Lookup_Parents[parent.Key];
//        if (parentNullable!=null) {
//          parentNullable = DC.Data.Lookup_ParentNullables[parentNullable.Key];
//        }
//        DC.Data.StartTransaction();
//        var newChild = new Lookup_Child(info, parent, parentNullable, isStoring: true);
//        DC.Data.CommitTransaction();
//        expectedChildren.Add(newChild.Key, newChild.ToString());
//        assertData();
//        return newChild;
//      } else {
//        return new Lookup_Child(info, parent, parentNullable, isStoring: false);
//      }
//    }


//    private void store(Lookup_Child newChild) {
//      newChild.Store();
//      expectedChildren.Add(newChild.Key, newChild.ToString());
//    }


//    private void updateParent(int parentKey, decimal newValue) {
//      var parent = DC.Data.Lookup_Parents[parentKey];
//      DC.Data.StartTransaction();
//      parent.Update(parent.Date, newValue);
//      DC.Data.RollbackTransaction();
//      assertData();
//      parent = DC.Data.Lookup_Parents[parentKey];
//      DC.Data.StartTransaction();
//      parent.Update(parent.Date, newValue);
//      DC.Data.CommitTransaction();
//      expectedParents[parent.Key] = parent.ToString();
//      foreach (var child in DC.Data.Lookup_Children) {
//        if (child.LookupParent==parent) {
//          expectedChildren[child.Key] = child.ToString();
//        }
//      }
//      assertData();
//    }


//    private void updateParentNullable(int parentNullableKey, decimal newValue) {
//      var parentNullable = DC.Data.Lookup_ParentNullables[parentNullableKey];
//      DC.Data.StartTransaction();
//      parentNullable.Update(parentNullable.Date, newValue);
//      DC.Data.RollbackTransaction();
//      assertData();
//      parentNullable = DC.Data.Lookup_ParentNullables[parentNullableKey];
//      DC.Data.StartTransaction();
//      parentNullable.Update(parentNullable.Date, newValue);
//      DC.Data.CommitTransaction();
//      expectedParentsNullable[parentNullable.Key] = parentNullable.ToString();
//      foreach (var child in DC.Data.Lookup_Children) {
//        if (child.LookupParentNullable==parentNullable) {
//          expectedChildren[child.Key] = child.ToString();
//        }
//      }
//      assertData();
//    }


//    private void updateChild(int childKey, int parentKey, int? parentNullableKey, string text) {
//      var child = DC.Data.Lookup_Children[childKey];
//      var newParent = DC.Data.Lookup_Parents[parentKey];
//      Lookup_ParentNullable? newParentNullable = null;
//      if (parentNullableKey!=null) {
//        newParentNullable = DC.Data.Lookup_ParentNullables[parentNullableKey.Value];
//      }
//      DC.Data.StartTransaction();
//      child.Update(text, newParent, newParentNullable);
//      DC.Data.RollbackTransaction();
//      assertData();

//      child = DC.Data.Lookup_Children[childKey];
//      newParent = DC.Data.Lookup_Parents[parentKey];
//      var oldParent = child.LookupParent;
//      newParentNullable = null;
//      if (parentNullableKey!=null) {
//        newParentNullable = DC.Data.Lookup_ParentNullables[parentNullableKey.Value];
//      }
//      var oldParentNullable = child.LookupParentNullable;
//      DC.Data.StartTransaction();
//      child.Update(text, newParent, newParentNullable);
//      DC.Data.CommitTransaction();
//      expectedChildren[child.Key] = child.ToString();
//      assertData();
//    }


//    private void removeChild(int childKey) {
//      var child = DC.Data.Lookup_Children[childKey];
//      DC.Data.StartTransaction();
//      child.Release();
//      DC.Data.RollbackTransaction();
//      assertData();
//      child = DC.Data.Lookup_Children[childKey];
//      expectedChildren.Remove(child.Key);
//      DC.Data.StartTransaction();
//      child.Release();
//      DC.Data.CommitTransaction();
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
#endregion