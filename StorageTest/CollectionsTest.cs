/* Test explanation:
 items can be stored or not stored, legal combinations
                                         child variable name
 + child not stored, parent not stored   child_
 + child not stored, parent stored       child__
 + child stored, parent stored           child

for each combination above some test code is written

parent variabls names:
Parent: the parent property in the child is not nullable
ParentN: the parent property in the child is nullable

each activity like create, update or delete is done first in a rooled back transaction and none of the data should
be changed, then the same activity is executed in a commited transactions and the data should change accordingly.

After a transaction has been committed, the datacontext gets disposed, opened again and verified that the data
is still the same. This is done twice, first using the .bak files, then the .csv files. There is one exception: 
stored parents might have some not stored children. In the new data context, those parents have no longer those 
children.

For convenience, the variables parent0, parentN1, child1, etc. contain always the data from the latest data context. They
get updated, each time assertDataDisposeDCRecreateDCassertData() gets called.

Variables with names ending with '_' or '__' are not stored in the data context. They do not get updated by 
assertDataDisposeDCRecreateDCassertData() and can therefore contain children or parents stored in previous data
contexts.
*/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageLib;
using TestContext;
using System.Linq;

namespace StorageTest {


  [TestClass]
  public class CollectionsTest {


    [TestMethod]
    public void TestCollections() {
      _ = new CollectionTestBase<ListParent, ListParentN, ListParentR, ListParentNR, ListChild>();
      _ = new CollectionTestBase<DictionaryParent, DictionaryParentN, DictionaryParentR, DictionaryParentNR, DictionaryChild>();
      _ = new CollectionTestBase<SortedListParent, SortedListParentN, SortedListParentR, SortedListParentNR, SortedListChild>();
      _ = new CollectionTestBase<SortedBucketCollectionParent, SortedBucketCollectionParentN, SortedBucketCollectionParentR, SortedBucketCollectionParentNR, SortedBucketCollectionChild>();
    }
  }


  enum CollectionTypeEnum {
    list,
    dictionary,
    sortedList,
    sortedBuckets,
  }


  public class CollectionTestBase<TP, TPN, TPR, TPNR, TChild>
    where TP : class, ICollectionParent<TP, TPN, TPR, TPNR, TChild>
    where TPN : class, ICollectionParent<TP, TPN, TPR, TPNR, TChild>
    where TPR : class, ICollectionParent<TP, TPN, TPR, TPNR, TChild>
    where TPNR : class, ICollectionParent<TP, TPN, TPR, TPNR, TChild>
    where TChild : ITestChild<TP, TPN, TPR, TPNR> {
    readonly CollectionTypeEnum collectionType;
    readonly CsvConfig csvConfig;
    readonly BakCsvFileSwapper bakCsvFileSwapper;
    DC dc;

    //these variables get updated by assertDataDisposeDCRecreateDCassertData() each time a new data context gets created
    ICollectionParent<TP, TPN, TPR, TPNR, TChild>? parent0;
    ICollectionParent<TP, TPN, TPR, TPNR, TChild>? parent1;
    ICollectionParent<TP, TPN, TPR, TPNR, TChild>? parentN0;
    ICollectionParent<TP, TPN, TPR, TPNR, TChild>? parentN1;
    ICollectionParent<TP, TPN, TPR, TPNR, TChild>? parentR0;
    ICollectionParent<TP, TPN, TPR, TPNR, TChild>? parentNR0;
    ITestChild<TP, TPN, TPR, TPNR>? child0;
    ITestChild<TP, TPN, TPR, TPNR>? child1;


    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. 
    public CollectionTestBase() {
    #pragma warning restore CS8618
      #pragma warning disable IDE0045 // Convert to conditional expression
      if (typeof(TChild)==typeof(ListChild)) {
        collectionType = CollectionTypeEnum.list;
      } else if (typeof(TChild)==typeof(DictionaryChild)) {
        collectionType = CollectionTypeEnum.dictionary;
      } else if (typeof(TChild)==typeof(SortedListChild)) {
        collectionType = CollectionTypeEnum.sortedList;
      } else if (typeof(TChild)==typeof(SortedBucketCollectionChild)) {
        collectionType = CollectionTypeEnum.sortedBuckets;
      } else {
        throw new NotSupportedException();
      }
      #pragma warning restore IDE0045

      var directoryInfo = new DirectoryInfo("TestCsv");
      if (directoryInfo.Exists) {
        directoryInfo.Delete(recursive: true);
        directoryInfo.Refresh();
      }

      directoryInfo.Create();
      //initVariables();


      try {
        csvConfig = new CsvConfig(directoryInfo.FullName, reportException: reportException);
        bakCsvFileSwapper = new BakCsvFileSwapper(csvConfig);
        DC.Trace = dcTrace;
        dc = new DC(csvConfig);
        initialiseDcParents();
        assertData("");

        // Create
        // ======
        // Create parent
        // -------------
        traceHeader("create not stored parent");
        dc.StartTransaction();
        var parent0_ = createParent("p_Temp", isStoring: false);
        dc.RollbackTransaction();
        assertData("");

        dc.StartTransaction();
        parent0_ = createParent("p_", isStoring: false);
        dc.CommitTransaction();
        assertDataDisposeDCRecreateDCassertData("");


        traceHeader("create stored parent");
        dc.StartTransaction();
        parent0 = createParent("p0Temp", isStoring: true);
        dc.RollbackTransaction();
        assertData("");

        dc.StartTransaction();
        parent0 = createParent("p0", isStoring: true);
        dc.CommitTransaction();
        assertDataDisposeDCRecreateDCassertData("p0|");

        // Create child with Parent==parent0, ParentN==null, ParentR==parentR0, ParentNR=parentNR0
        // ---------------------------------------------------------------------------------------
        traceHeader("create not stored child with not stored parent");
        var parentR0_ = createParentR("pR0_", isStoring: false);
        var parentNR0_ = createParentNR("pRN0_", isStoring: false);
        dc.StartTransaction();
        var child0_ = createChild("c0_Temp", parent0_, null, parentR0_, parentNR0_, isStoring: false);
        dc.RollbackTransaction();
        Assert.AreEqual(0, parent0_.CountAllChildren);
        Assert.AreEqual(0, parentR0_.CountAllChildren);
        Assert.AreEqual(0, parentNR0_.CountAllChildren);
        assertData("p0|");

        dc.StartTransaction();
        child0_ = createChild("c0_", parent0_, null, parentR0_, parentNR0_, isStoring: false);
        dc.CommitTransaction();
        Assert.AreEqual("c0_", child0_.Text);
        Assert.AreEqual(child0_, parent0_.AllChildrenFirst);
        Assert.AreEqual(child0_, parentR0_.AllChildrenFirst);
        Assert.AreEqual(child0_, parentNR0_.AllChildrenFirst);
        assertDataDisposeDCRecreateDCassertData("p0|");


        traceHeader("create not stored child with stored parent");
        parentR0 = createParentR("pR0", isStoring: true);
        parentNR0 = createParentNR("pNR0", isStoring: true);
        dc.StartTransaction();
        var child0__ = createChild("c0__Temp", parent0, null, parentR0, parentNR0, isStoring: false);
        dc.RollbackTransaction();
        Assert.AreEqual(0, parent0.CountAllChildren);
        Assert.AreEqual(0, parentR0.CountAllChildren);
        Assert.AreEqual(0, parentNR0.CountAllChildren);
        assertData("p0|pR0|pNR0|");

        dc.StartTransaction();
        child0__ = createChild("c0__", parent0, null, parentR0, parentNR0, isStoring: false);
        dc.CommitTransaction();
        Assert.AreEqual("c0__", child0__.Text);
        Assert.AreEqual(parent0, child0__.Parent);
        Assert.AreEqual(parentR0, child0__.ParentR);
        Assert.AreEqual(parentNR0, child0__.ParentNR);
        assertDataDisposeDCRecreateDCassertData("p0:c0__|pR0:c0__|pNR0:c0__|", "p0|pR0|pNR0|");


        traceHeader("create stored child with stored parent");
        dc.StartTransaction();
        child0 = createChild("c0Temp", parent0, null, parentR0, parentNR0, isStoring: true);
        dc.RollbackTransaction();
        assertData("p0|pR0|pNR0|");

        dc.StartTransaction();
        child0 = createChild("c0", parent0, null, parentR0, parentNR0, isStoring: true);
        dc.CommitTransaction();
        assertDataDisposeDCRecreateDCassertData("p0:;c0|pR0:;c0|pNR0:;c0|c0:p0,pR0,pNR0|");

        //Fail to create stored child with not stored parent
        traceHeader("fail to create stored child with not stored parents");
        try {
          var parent0___ = createParent("p0___", isStoring: false);
          var parentN0___ = createParentN("pN0___", isStoring: false);
          var parentR0___ = createParentR("pR0___", isStoring: false);
          var parentNR0___ = createParentNR("pRN0___", isStoring: false);
          var _ = createChild("failed child", parent0___, parentN0___, parentR0___, parentNR0___);
          Assert.Fail();
        } catch {
        }
        //Todo: Ideally, an exception during create, store or remove should not change any data. Is additional code needed undoing 
        //any potentially changed data ? 
        //Assert.AreEqual(0, parent0_.CountAllChildren);
        assertDataDisposeDCRecreateDCassertData("p0:;c0|pR0:;c0|pNR0:;c0|c0:p0,pR0,pNR0|");

        // Update
        // ======
        // Update child.Parent to parent1, child.ParentN to parentN0
        // ---------------------------------------------------------

        traceHeader("not stored child: update with not stored parents");
        var parent1_ = createParent("p1_", isStoring: false);
        var parentN0_ = createParentN("pN0___", isStoring: false);
        //var parentR1_ = createParent("pR1_", isStoring: false);
        //var parentNR0_ = createParentN("pNR0_", isStoring: false);
        dc.StartTransaction();
        child0_.Update("c0_.1Temp", (TP)parent1_, (TPN)parentN0_);
        dc.RollbackTransaction();
        Assert.AreEqual(child0_, parent0_.AllChildrenFirst);
        Assert.AreEqual(0, parentN0_.CountAllChildren);
        Assert.AreEqual(0, parent1_.CountAllChildren);
        assertData("p0:;c0|pR0:;c0|pNR0:;c0|c0:p0,pR0,pNR0|");

        dc.StartTransaction();
        child0_.Update("c0_.1", (TP)parent1_, (TPN)parentN0_);
        dc.CommitTransaction();
        Assert.AreEqual(0, parent0_.CountAllChildren);
        Assert.AreEqual(child0_, parentN0_.AllChildrenFirst);
        Assert.AreEqual(child0_, parent1_.AllChildrenFirst);
        assertDataDisposeDCRecreateDCassertData("p0:;c0|pR0:;c0|pNR0:;c0|c0:p0,pR0,pNR0|");


        traceHeader("not stored child: update with stored parents");
        parent1 = createParent("p1", isStoring: true);
        parentN0 = createParentN("pN0", isStoring: true);
        dc.StartTransaction();
        child0__.Update("c0__.1Temp", (TP)parent1, (TPN)parentN0);
        dc.RollbackTransaction();
        Assert.AreEqual("c0__", child0__.Text);
        Assert.AreEqual(parent0.Text, child0__.Parent.Text);
        Assert.IsNull(child0__.ParentN);
        assertData("p0:;c0|p1|pN0|pR0:;c0|pNR0:;c0|c0:p0,pR0,pNR0|");

        dc.StartTransaction();
        child0__.Update("c0__.1", (TP)parent1, (TPN)parentN0);
        dc.CommitTransaction();
        Assert.AreEqual("c0__.1", child0__.Text);
        Assert.AreEqual(parent1, child0__.Parent);
        Assert.AreEqual(parentN0, child0__.ParentN);
        assertDataDisposeDCRecreateDCassertData(
          "p0:;c0|p1:c0__.1|pN0:c0__.1|pR0:;c0|pNR0:;c0|c0:p0,pR0,pNR0|",
          "p0:;c0|p1|pN0|pR0:;c0|pNR0:;c0|c0:p0,pR0,pNR0|");


        traceHeader("stored child: update with stored parents");
        dc.StartTransaction();
        child0.Update("c0.1Temp", (TP)parent1, (TPN)parentN0);
        dc.RollbackTransaction();
        assertData("p0:;c0|p1|pN0|pR0:;c0|pNR0:;c0|c0:p0,pR0,pNR0|");

        dc.StartTransaction();
        child0.Update("c0.1", (TP)parent1, (TPN)parentN0);
        dc.CommitTransaction();
        assertDataDisposeDCRecreateDCassertData("p0|p1:;c0.1|pN0:;c0.1|pR0:;c0.1|pNR0:;c0.1|c0.1:p1,pN0,pR0,pNR0|");

        // Update child.ParentN to parentN1
        // --------------------------------

        traceHeader("not stored child: update not stored ParentN");
        var parentN1_ = createParentN("pN1_", isStoring: false);
        dc.StartTransaction();
        child0_.Update("c0_.2Temp", (TP)parent1_, (TPN)parentN1_);
        dc.RollbackTransaction();
        Assert.AreEqual("c0_.1", child0_.Text);
        Assert.AreEqual(child0_, parent1_.AllChildrenFirst);
        Assert.AreEqual(child0_, parentN0_.AllChildrenFirst);
        Assert.AreEqual(0, parentN1_.CountAllChildren);
        assertData("p0|p1:;c0.1|pN0:;c0.1|pR0:;c0.1|pNR0:;c0.1|c0.1:p1,pN0,pR0,pNR0|");

        dc.StartTransaction();
        child0_.Update("c0_.2", (TP)parent1_, (TPN)parentN1_);
        dc.CommitTransaction();
        Assert.AreEqual("c0_.2", child0_.Text);
        Assert.AreEqual(0, parent0_.CountAllChildren);
        Assert.AreEqual(0, parentN0_.CountAllChildren);
        Assert.AreEqual(child0_, parent1_.AllChildrenFirst);
        Assert.AreEqual(child0_, parentN1_.AllChildrenFirst);
        assertDataDisposeDCRecreateDCassertData("p0|p1:;c0.1|pN0:;c0.1|pR0:;c0.1|pNR0:;c0.1|c0.1:p1,pN0,pR0,pNR0|");


        traceHeader("not stored child: update stored ParentN");
        parentN1 = createParentN("pN1", isStoring: true);
        dc.StartTransaction();
        child0__.Update("c0__.2Temp", (TP)parent1, (TPN)parentN1);
        dc.RollbackTransaction();
        Assert.AreEqual("c0__.1", child0__.Text);
        Assert.AreEqual(parent1.Text, child0__.Parent.Text);
        Assert.AreEqual(parentN0.Text, child0__.ParentN!.Text);
        assertDataDisposeDCRecreateDCassertData("p0|p1:;c0.1|pN0:;c0.1|pN1|pR0:;c0.1|pNR0:;c0.1|c0.1:p1,pN0,pR0,pNR0|");

        dc.StartTransaction();
        child0__.Update("c0__.2", (TP)parent1, (TPN)parentN1);
        dc.CommitTransaction();
        Assert.AreEqual("c0__.2", child0__.Text);
        Assert.AreEqual(parent1, child0__.Parent);
        Assert.AreEqual(parentN1, child0__.ParentN);
        assertDataDisposeDCRecreateDCassertData(
          "p0|p1:c0__.2;,c0.1|pN0:;c0.1|pN1:c0__.2|pR0:;c0.1|pNR0:;c0.1|c0.1:p1,pN0,pR0,pNR0|",
          "p0|p1:;c0.1|pN0:;c0.1|pN1|pR0:;c0.1|pNR0:;c0.1|c0.1:p1,pN0,pR0,pNR0|");


        traceHeader("stored child: update stored ParentN");
        dc.StartTransaction();
        child0.Update("c0.2Temp", (TP)parent1, (TPN)parentN1);
        dc.RollbackTransaction();
        assertData("p0|p1:;c0.1|pN0:;c0.1|pN1|pR0:;c0.1|pNR0:;c0.1|c0.1:p1,pN0,pR0,pNR0|");

        dc.StartTransaction();
        child0.Update("c0.2", (TP)parent1, (TPN)parentN1);
        dc.CommitTransaction();
        assertDataDisposeDCRecreateDCassertData("p0|p1:;c0.2|pN0|pN1:;c0.2|pR0:;c0.2|pNR0:;c0.2|c0.2:p1,pN1,pR0,pNR0|");

        // Update child.ParentN to null
        // ----------------------------

        traceHeader("not stored child: update not stored ParentN to null");
        dc.StartTransaction();
        child0_.Update("c0_.3Temp", (TP)parent1_, null);
        dc.RollbackTransaction();
        Assert.AreEqual("c0_.2", child0_.Text);
        Assert.AreEqual(child0_, parent1_.AllChildrenFirst);
        Assert.AreEqual(child0_, parentN1_.AllChildrenFirst);
        assertData("p0|p1:;c0.2|pN0|pN1:;c0.2|pR0:;c0.2|pNR0:;c0.2|c0.2:p1,pN1,pR0,pNR0|");

        dc.StartTransaction();
        child0_.Update("c0_.3", (TP)parent1_, null);
        dc.CommitTransaction();
        Assert.AreEqual("c0_.3", child0_.Text);
        Assert.AreEqual(0, parentN0_.CountAllChildren);
        Assert.AreEqual(child0_, parent1_.AllChildrenFirst);
        assertDataDisposeDCRecreateDCassertData("p0|p1:;c0.2|pN0|pN1:;c0.2|pR0:;c0.2|pNR0:;c0.2|c0.2:p1,pN1,pR0,pNR0|");


        traceHeader("not stored child: update stored ParentN to null");
        dc.StartTransaction();
        child0__.Update("c0__.3Temp", (TP)parent1, null);
        dc.RollbackTransaction();
        Assert.AreEqual("c0__.2", child0__.Text);
        Assert.AreEqual(parent1.Text, child0__.Parent.Text);
        Assert.AreEqual(parentN1.Text, child0__.ParentN!.Text);
        assertDataDisposeDCRecreateDCassertData("p0|p1:;c0.2|pN0|pN1:;c0.2|pR0:;c0.2|pNR0:;c0.2|c0.2:p1,pN1,pR0,pNR0|");

        dc.StartTransaction();
        child0__.Update("c0__.3", (TP)parent1, null);
        dc.CommitTransaction();
        Assert.AreEqual("c0__.3", child0__.Text);
        Assert.AreEqual(parent1, child0__.Parent);
        Assert.IsNull(child0__.ParentN);
        assertDataDisposeDCRecreateDCassertData(
          "p0|p1:c0__.3;,c0.2|pN0|pN1:;c0.2|pR0:;c0.2|pNR0:;c0.2|c0.2:p1,pN1,pR0,pNR0|",
          "p0|p1:;c0.2|pN0|pN1:;c0.2|pR0:;c0.2|pNR0:;c0.2|c0.2:p1,pN1,pR0,pNR0|");


        traceHeader("stored child: update stored ParentN to null");
        dc.StartTransaction();
        child0.Update("c0.3Temp", (TP)parent1, null);
        dc.RollbackTransaction();
        assertData("p0|p1:;c0.2|pN0|pN1:;c0.2|pR0:;c0.2|pNR0:;c0.2|c0.2:p1,pN1,pR0,pNR0|");

        dc.StartTransaction();
        child0.Update("c0.3", (TP)parent1, null);
        dc.CommitTransaction();
        assertDataDisposeDCRecreateDCassertData("p0|p1:;c0.3|pN0|pN1|pR0:;c0.3|pNR0:;c0.3|c0.3:p1,pR0,pNR0|");
        bakCsvFileSwapper.DeleteBakFiles();


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
        assertData("p0|p1:;c0.3|pN0|pN1|pR0:;c0.3|pNR0:;c0.3|c0.3:p1,pR0,pNR0|");

        //Release child
        //-------------
        traceHeader("stored child: release child");
        child1 = createChild("c1", parent1, parentN1, parentR0, parentNR0, isStoring: true);
        assertDataDisposeDCRecreateDCassertData(
          "p0|p1:;c0.3,c1|pN0|pN1:;c1|pR0:;c0.3,c1|pNR0:;c0.3,c1|c0.3:p1,pR0,pNR0|c1:p1,pN1,pR0,pNR0|");
        dc.StartTransaction();
        child1.Release();
        dc.RollbackTransaction();
        assertData("p0|p1:;c0.3,c1|pN0|pN1:;c1|pR0:;c0.3,c1|pNR0:;c0.3,c1|c0.3:p1,pR0,pNR0|c1:p1,pN1,pR0,pNR0|");

        dc.StartTransaction();
        child1.Release();
        dc.CommitTransaction();
        child1 = null;
        assertDataDisposeDCRecreateDCassertData(
          "p0|p1:;c0.3,c1|pN0|pN1:c1|pR0:;c0.3,c1|pNR0:;c0.3,c1|c0.3:p1,pR0,pNR0|",
          "p0|p1:;c0.3|pN0|pN1|pR0:;c0.3|pNR0:;c0.3|c0.3:p1,pR0,pNR0|");

        //Release parent
        //--------------
        traceHeader("stored parent: release parent");
        dc.StartTransaction();
        parent0.Release();
        dc.RollbackTransaction();
        assertData("p0|p1:;c0.3|pN0|pN1|pR0:;c0.3|pNR0:;c0.3|c0.3:p1,pR0,pNR0|");

        dc.StartTransaction();
        parent0.Release();
        dc.CommitTransaction();
        parent0 = null;
        assertDataDisposeDCRecreateDCassertData("p1:;c0.3|pN0|pN1|pR0:;c0.3|pNR0:;c0.3|c0.3:p1,pR0,pNR0|");


        //test .bak file with complicated data structure
        // =============================================
        traceHeader("create, update and release complicated data structure");
        var parent2 = createParent("p2", isStoring: true);
        var parent3 = createParent("p3", isStoring: true);
        var parentN2 = createParentN("pN2", isStoring: true);
        var parentR2 = createParentR("pR2", isStoring: true);
        var parentNR2 = createParentNR("pRN2", isStoring: true);
        var child2 = createChild("c2", parent2, null, parentR2, parentNR2, isStoring: true);
        child2.Update("c2.0", (TP)parent2, null);
        parent2.Update("p2.0");
        parent3.Update("p3.0");
        parentN2.Update("pN2.0");
        parentR2.Update("pR2.0");
        parentNR2.Update("pRN2.0");
        child2.Update("c2.0", (TP)parent3, (TPN)parentN2);
        child2.Release();
        parent2.Release();
        parent3.Release();
        parentN2.Release();
        parentR2.Release();
        parentNR2.Release();
        assertDataDisposeDCRecreateDCassertData("p1:;c0.3|pN0|pN1|pR0:;c0.3|pNR0:;c0.3|c0.3:p1,pR0,pNR0|");

      } finally {
        DC.DisposeData();
      }
    }


    private ICollectionParent<TP, TPN, TPR, TPNR, TChild> createParent(string text, bool isStoring) {
      return collectionType switch {
        CollectionTypeEnum.list => (ICollectionParent<TP, TPN, TPR, TPNR, TChild>)new ListParent(text, isStoring),
        CollectionTypeEnum.dictionary => (ICollectionParent<TP, TPN, TPR, TPNR, TChild>)new DictionaryParent(text, isStoring),
        CollectionTypeEnum.sortedList => (ICollectionParent<TP, TPN, TPR, TPNR, TChild>)new SortedListParent(text, isStoring),
        CollectionTypeEnum.sortedBuckets => (ICollectionParent<TP, TPN, TPR, TPNR, TChild>)new SortedBucketCollectionParent(text, isStoring),
        _ => throw new NotSupportedException(),
      };
    }


    private ICollectionParent<TP, TPN, TPR, TPNR, TChild> createParentN(string text, bool isStoring) {
      return collectionType switch {
        CollectionTypeEnum.list => (ICollectionParent<TP, TPN, TPR, TPNR, TChild>)new ListParentN(text, isStoring),
        CollectionTypeEnum.dictionary => (ICollectionParent<TP, TPN, TPR, TPNR, TChild>)new DictionaryParentN(text, isStoring),
        CollectionTypeEnum.sortedList => (ICollectionParent<TP, TPN, TPR, TPNR, TChild>)new SortedListParentN(text, isStoring),
        CollectionTypeEnum.sortedBuckets => (ICollectionParent<TP, TPN, TPR, TPNR, TChild>)new SortedBucketCollectionParentN(text, isStoring),
        _ => throw new NotSupportedException(),
      };
    }


    private ICollectionParent<TP, TPN, TPR, TPNR, TChild> createParentR(string text, bool isStoring) {
      return collectionType switch {
        CollectionTypeEnum.list => (ICollectionParent<TP, TPN, TPR, TPNR, TChild>)new ListParentR(text, isStoring),
        CollectionTypeEnum.dictionary => (ICollectionParent<TP, TPN, TPR, TPNR, TChild>)new DictionaryParentR(text, isStoring),
        CollectionTypeEnum.sortedList => (ICollectionParent<TP, TPN, TPR, TPNR, TChild>)new SortedListParentR(text, isStoring),
        CollectionTypeEnum.sortedBuckets => (ICollectionParent<TP, TPN, TPR, TPNR, TChild>)new SortedBucketCollectionParentR(text, isStoring),
        _ => throw new NotSupportedException(),
      };
    }


    private ICollectionParent<TP, TPN, TPR, TPNR, TChild> createParentNR(string text, bool isStoring) {
      return collectionType switch {
        CollectionTypeEnum.list => (ICollectionParent<TP, TPN, TPR, TPNR, TChild>)new ListParentNR(text, isStoring),
        CollectionTypeEnum.dictionary => (ICollectionParent<TP, TPN, TPR, TPNR, TChild>)new DictionaryParentNR(text, isStoring),
        CollectionTypeEnum.sortedList => (ICollectionParent<TP, TPN, TPR, TPNR, TChild>)new SortedListParentNR(text, isStoring),
        CollectionTypeEnum.sortedBuckets => (ICollectionParent<TP, TPN, TPR, TPNR, TChild>)new SortedBucketCollectionParentNR(text, isStoring),
        _ => throw new NotSupportedException(),
      };
    }


    static readonly DateTime testDate = DateTime.Now.Date;


    private ITestChild<TP, TPN, TPR, TPNR> createChild
      (string text,
      ICollectionParent<TP, TPN, TPR, TPNR, TChild> parent,
      ICollectionParent<TP, TPN, TPR, TPNR, TChild>? parentN,
      ICollectionParent<TP, TPN, TPR, TPNR, TChild> parentR,
      ICollectionParent<TP, TPN, TPR, TPNR, TChild>? parentNR,
      bool isStoring = true) {
      return collectionType switch {
        CollectionTypeEnum.list => (ITestChild<TP, TPN, TPR, TPNR>)new
          ListChild(text, (ListParent)parent, (ListParentN?)parentN, (ListParentR)parentR, (ListParentNR?)parentNR, isStoring),
        CollectionTypeEnum.dictionary => (ITestChild<TP, TPN, TPR, TPNR>)new
          DictionaryChild(text, (DictionaryParent)parent, (DictionaryParentN?)parentN, (DictionaryParentR)parentR, (DictionaryParentNR?)parentNR, isStoring),
        CollectionTypeEnum.sortedList => (ITestChild<TP, TPN, TPR, TPNR>)new
          SortedListChild(text, (SortedListParent)parent, (SortedListParentN?)parentN, (SortedListParentR)parentR, (SortedListParentNR?)parentNR, isStoring),
        CollectionTypeEnum.sortedBuckets => (ITestChild<TP, TPN, TPR, TPNR>)new
          SortedBucketCollectionChild(text, testDate, (SortedBucketCollectionParent)parent, (SortedBucketCollectionParentN?)parentN, (SortedBucketCollectionParentR)parentR, (SortedBucketCollectionParentNR?)parentNR, isStoring),
        _ => throw new NotSupportedException(),
      };
    }


    private enum parentTypeEnum {
      parent,
      parentN,
      parentR,
      parentNR,
      count
    }


    private string dcAsString() {
      if (dc is null) return "";

      var s = "";
      append(ref s, parentTypeEnum.parent);
      append(ref s, parentTypeEnum.parentN);
      append(ref s, parentTypeEnum.parentR);
      append(ref s, parentTypeEnum.parentNR);

      foreach (var child in dcChildren) {
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

    readonly IReadonlyDataStore<ICollectionParent<TP, TPN, TPR, TPNR, TChild>>[] dcParents = new IReadonlyDataStore<ICollectionParent<TP, TPN, TPR, TPNR, TChild>>[(int)parentTypeEnum.count];
    IReadonlyDataStore<ITestChild<TP, TPN, TPR, TPNR>> dcChildren;


    private void initialiseDcParents() {
      switch (collectionType) {
      case CollectionTypeEnum.list:
        dcParents[(int)parentTypeEnum.parent] = (IReadonlyDataStore<ICollectionParent<TP, TPN, TPR, TPNR, TChild>>)dc.ListParents;
        dcParents[(int)parentTypeEnum.parentN] = (IReadonlyDataStore<ICollectionParent<TP, TPN, TPR, TPNR, TChild>>)dc.ListParentNs;
        dcParents[(int)parentTypeEnum.parentR] = (IReadonlyDataStore<ICollectionParent<TP, TPN, TPR, TPNR, TChild>>)dc.ListParentRs;
        dcParents[(int)parentTypeEnum.parentNR] = (IReadonlyDataStore<ICollectionParent<TP, TPN, TPR, TPNR, TChild>>)dc.ListParentNRs;
        dcChildren = (IReadonlyDataStore<ITestChild<TP, TPN, TPR, TPNR>>)dc.ListChildren;
        break;
      case CollectionTypeEnum.dictionary:
        dcParents[(int)parentTypeEnum.parent] = (IReadonlyDataStore<ICollectionParent<TP, TPN, TPR, TPNR, TChild>>)dc.DictionaryParents;
        dcParents[(int)parentTypeEnum.parentN] = (IReadonlyDataStore<ICollectionParent<TP, TPN, TPR, TPNR, TChild>>)dc.DictionaryParentNs;
        dcParents[(int)parentTypeEnum.parentR] = (IReadonlyDataStore<ICollectionParent<TP, TPN, TPR, TPNR, TChild>>)dc.DictionaryParentRs;
        dcParents[(int)parentTypeEnum.parentNR] = (IReadonlyDataStore<ICollectionParent<TP, TPN, TPR, TPNR, TChild>>)dc.DictionaryParentNRs;
        dcChildren = (IReadonlyDataStore<ITestChild<TP, TPN, TPR, TPNR>>)dc.DictionaryChildren;
        break;
      case CollectionTypeEnum.sortedList:
        dcParents[(int)parentTypeEnum.parent] = (IReadonlyDataStore<ICollectionParent<TP, TPN, TPR, TPNR, TChild>>)dc.SortedListParents;
        dcParents[(int)parentTypeEnum.parentN] = (IReadonlyDataStore<ICollectionParent<TP, TPN, TPR, TPNR, TChild>>)dc.SortedListParentNs;
        dcParents[(int)parentTypeEnum.parentR] = (IReadonlyDataStore<ICollectionParent<TP, TPN, TPR, TPNR, TChild>>)dc.SortedListParentRs;
        dcParents[(int)parentTypeEnum.parentNR] = (IReadonlyDataStore<ICollectionParent<TP, TPN, TPR, TPNR, TChild>>)dc.SortedListParentNRs;
        dcChildren = (IReadonlyDataStore<ITestChild<TP, TPN, TPR, TPNR>>)dc.SortedListChildren;
        break;
      case CollectionTypeEnum.sortedBuckets:
        dcParents[(int)parentTypeEnum.parent] = (IReadonlyDataStore<ICollectionParent<TP, TPN, TPR, TPNR, TChild>>)dc.SortedBucketCollectionParents;
        dcParents[(int)parentTypeEnum.parentN] = (IReadonlyDataStore<ICollectionParent<TP, TPN, TPR, TPNR, TChild>>)dc.SortedBucketCollectionParentNs;
        dcParents[(int)parentTypeEnum.parentR] = (IReadonlyDataStore<ICollectionParent<TP, TPN, TPR, TPNR, TChild>>)dc.SortedBucketCollectionParentRs;
        dcParents[(int)parentTypeEnum.parentNR] = (IReadonlyDataStore<ICollectionParent<TP, TPN, TPR, TPNR, TChild>>)dc.SortedBucketCollectionParentNRs;
        dcChildren = (IReadonlyDataStore<ITestChild<TP, TPN, TPR, TPNR>>)dc.SortedBucketCollectionChildren;
        break;
      default:
        throw new NotSupportedException();
      }
    }


    readonly List<(bool IsStored,string Text)> childrenStrings = new();


    private void append(ref string s, parentTypeEnum parentType) {
      var parents = dcParents![(int)parentType];

      foreach (var parent in parents) {
        s += parent.Text;
        if (parent.CountAllChildren>0) {
          s += ':';
          childrenStrings.Clear();
          foreach (var child in parent.GetAllChildren) {
            childrenStrings.Add((child.Key>=0, child.Text));
          }
          var isFirstChild = true;
          var isFirstStoredMissing = true;
          foreach (var (isStored, text) in childrenStrings.OrderBy(cs => cs.Text).ThenBy(cs => cs.IsStored)) {
            if (isFirstStoredMissing && isStored) {
              isFirstStoredMissing = false;
              s += ';';
            }
            if (isFirstChild) {
              isFirstChild = false;
            } else {
              s += ',';
            }
            s += text;
          }
        }
        s += '|';
      }
    }


    private void assertDataDisposeDCRecreateDCassertData(string expectedDcString1, string? expectedDcString2 = null) {
      assertData(expectedDcString1);
      DC.DisposeData();

      if (bakCsvFileSwapper.UseBackupFiles()) {
        dc = new DC(csvConfig);
        initialiseDcParents();
        assertData(expectedDcString2??expectedDcString1);
        DC.DisposeData();
        bakCsvFileSwapper.SwapBack();
      }

      dc = new DC(csvConfig);
      initialiseDcParents();
      assertData(expectedDcString2??expectedDcString1);
      if (parent0 is not null) parent0 = dcParents[(int)parentTypeEnum.parent][0];
      if (parent1 is not null) parent1 = dcParents[(int)parentTypeEnum.parent][1];
      if (parentN0 is not null) parentN0 = dcParents[(int)parentTypeEnum.parentN][0];
      if (parentN1 is not null) parentN1 = dcParents[(int)parentTypeEnum.parentN][1];
      if (parentR0 is not null) parentR0 = dcParents[(int)parentTypeEnum.parentR][0];
      if (parentNR0 is not null) parentNR0 = dcParents[(int)parentTypeEnum.parentNR][0];
      if (child0 is not null) child0 = dcChildren[0];
      if (child1 is not null) child1 = dcChildren[1];
    }


    private void assertData(string expectedDcString) {
      Assert.AreEqual(expectedDcString, dcAsString());
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
