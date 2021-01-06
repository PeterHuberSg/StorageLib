//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.IO;
//using System.Reflection;
//using System.Text;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using StorageLib;
//using StorageDataContext;


//namespace StorageTest {


//  [TestClass]
//  public class ChildrenListTest {
//    /*
//    Tests in detail if DataStore internal data is correct after each basic operation (add, update, remove) including
//    RollbackTransaction() and CommitTransaction. After both transaction variants, the DataStore gets tested first,
//    then disposed and initialised a new, then tested again.
//    Because the data gets read fresh from the file each time, always the newest version of parents and children
//    need to be used.
//    */


//    #region TestChildrenList
//    //      ----------------

//    public class DataStoreStats {
//      public bool IsContinuous;
//      public bool IsUpdated;
//      public bool IsDeleted;
//      public int FirstIndex;
//      public int LastIndex;
//      public readonly Dictionary<int, string> Items;

//      public DataStoreStats() {
//        IsContinuous = true;
//        FirstIndex = -1;
//        LastIndex = -1;
//        Items = new Dictionary<int, string>();
//      }

//      public void Set(bool isContinuous, bool isUpdated, bool isDeleted, int firstIndex, int lastIndex) {
//        IsContinuous = isContinuous;
//        IsUpdated = isUpdated;
//        IsDeleted = isDeleted;
//        FirstIndex = firstIndex;
//        LastIndex = lastIndex;
//      }

//      public override string ToString() {
//        return $"Continuous: {IsContinuous}, Updated: {IsUpdated}, Deleted: {IsDeleted}, First: {FirstIndex}, " +
//          $"Last: {LastIndex}, Count: {Items.Count}";
//      }
//    }


//    CsvConfig? csvConfig;
//    #pragma warning disable CS8618 // Non-nullable field is uninitialized.
//    DataStoreStats parents;
//    DataStoreStats parentReadonlies;
//    DataStoreStats parentNullables;
//    DataStoreStats parentNullableReadonlies;
//    DataStoreStats coParents;
//    DataStoreStats coParentReadonlies;
//    DataStoreStats coParentNullables;
//    DataStoreStats coParentNullableReadonlies;
//    DataStoreStats children;
//    DataStoreStats coChildren;
//    #pragma warning restore CS8618 // Non-nullable field is uninitialized.

//    //uses ToTestString() instead of ToString()
//    Dictionary<int, string> childrenTestString = new Dictionary<int, string>();


//    [TestMethod]
//    public void TestChildrenList() {
//      try {
//        parents = new DataStoreStats();
//        parentReadonlies = new DataStoreStats();
//        parentNullables = new DataStoreStats();
//        parentNullableReadonlies = new DataStoreStats();
//        coParents = new DataStoreStats();
//        coParentReadonlies = new DataStoreStats();
//        coParentNullables = new DataStoreStats();
//        coParentNullableReadonlies = new DataStoreStats();
//        children = new DataStoreStats();
//        coChildren = new DataStoreStats();
//        childrenTestString = new Dictionary<int, string>();


//        var directoryInfo = new DirectoryInfo("TestCsv");
//        if (directoryInfo.Exists) {
//          directoryInfo.Delete(recursive: true);
//          directoryInfo.Refresh();
//        }

//        directoryInfo.Create();

//        csvConfig = new CsvConfig(directoryInfo.FullName, reportException: reportException);
//        DC.Trace = dcTrace;
//        initDC();
//        assertData();

//        traceHeader("--- construct and store parent P0, PN0, CP0, CPN0 ---");
//        DC.Data.StartTransaction();
//        var p0 = new ChildrenList_Parent("P0");
//        var p0r = new ChildrenList_ParentReadonly("P0r");
//        var pn0 = new ChildrenList_ParentNullable("PN0", isStoring: false);
//        var pn0r = new ChildrenList_ParentNullableReadonly("PN0r", isStoring: false);
//        var cp0 = new ChildrenList_CreateOnlyParent("CP0");
//        var cp0r = new ChildrenList_CreateOnlyParentReadonly("CP0r");
//        var cpn0 = new ChildrenList_CreateOnlyParentNullable("CPN0", isStoring: false);
//        var cpn0r = new ChildrenList_CreateOnlyParentNullableReadonly("CPN0r", isStoring: false);
//        DC.Data.RollbackTransaction();
//        assertData();
//        DC.Data.StartTransaction();
//        p0.Store();
//        var p0Key = p0.Key;
//        p0r.Store();
//        var p0rKey = p0r.Key;
//        pn0.Store();
//        var pn0Key = pn0.Key;
//        pn0r.Store();
//        var pn0rKey = pn0r.Key;
//        cp0.Store();
//        var cp0Key = cp0.Key;
//        cp0r.Store();
//        var cp0rKey = cp0r.Key;
//        cpn0.Store();
//        var cpn0Key = cpn0.Key;
//        cpn0r.Store();
//        var cpn0rKey = cpn0r.Key;
//        DC.Data.CommitTransaction();
//        parents.Items[p0Key] = p0.ToString();
//        parents.Set(isContinuous: true, isUpdated: false, isDeleted: false, firstIndex: 0, lastIndex: 0);
//        parentReadonlies.Items[p0rKey] = p0r.ToString();
//        parentReadonlies.Set(isContinuous: true, isUpdated: false, isDeleted: false, firstIndex: 0, lastIndex: 0);
//        coParents.Items[cp0Key] = cp0.ToString();
//        coParents.Set(isContinuous: true, isUpdated: false, isDeleted: false, firstIndex: 0, lastIndex: 0);
//        coParentReadonlies.Items[cp0rKey] = cp0r.ToString();
//        coParentReadonlies.Set(isContinuous: true, isUpdated: false, isDeleted: false, firstIndex: 0, lastIndex: 0);
//        parentNullables.Items[pn0Key] = pn0.ToString();
//        parentNullables.Set(isContinuous: true, isUpdated: false, isDeleted: false, firstIndex: 0, lastIndex: 0);
//        parentNullableReadonlies.Items[pn0rKey] = pn0r.ToString();
//        parentNullableReadonlies.Set(isContinuous: true, isUpdated: false, isDeleted: false, firstIndex: 0, lastIndex: 0);
//        coParentNullables.Items[cpn0Key] = cpn0.ToString();
//        coParentNullables.Set(isContinuous: true, isUpdated: false, isDeleted: false, firstIndex: 0, lastIndex: 0);
//        coParentNullableReadonlies.Items[cpn0rKey] = cpn0r.ToString();
//        coParentNullableReadonlies.Set(isContinuous: true, isUpdated: false, isDeleted: false, firstIndex: 0, lastIndex: 0);
//        assertData();

//        traceHeader("--- construct and store children C0 and CC0 ---");
//        p0 = DC.Data.ChildrenList_Parents[p0Key];
//        p0r = DC.Data.ChildrenList_ParentReadonlys[p0rKey];
//        pn0 = DC.Data.ChildrenList_ParentNullables[pn0Key];
//        pn0r = DC.Data.ChildrenList_ParentNullableReadonlys[pn0rKey];
//        cp0 = DC.Data.ChildrenList_CreateOnlyParents[cp0Key];
//        cp0r = DC.Data.ChildrenList_CreateOnlyParentReadonlys[cp0rKey];
//        cpn0 = DC.Data.ChildrenList_CreateOnlyParentNullables[cpn0Key];
//        cpn0r = DC.Data.ChildrenList_CreateOnlyParentNullableReadonlys[cpn0rKey];
//        DC.Data.StartTransaction();
//        var c0 = new  ChildrenList_Child("C0", p0, p0r, pn0, pn0r, cp0, cp0r, cpn0, cpn0r);
//        var cc0 = new  ChildrenList_CreateOnlyChild("CC0", cp0, cp0r, cpn0, cpn0r);
//        DC.Data.RollbackTransaction();
//        assertData();
//        p0 = DC.Data.ChildrenList_Parents[p0Key];
//        p0r = DC.Data.ChildrenList_ParentReadonlys[p0rKey];
//        pn0 = DC.Data.ChildrenList_ParentNullables[pn0Key];
//        pn0r = DC.Data.ChildrenList_ParentNullableReadonlys[pn0rKey];
//        cp0 = DC.Data.ChildrenList_CreateOnlyParents[cp0Key];
//        cp0r = DC.Data.ChildrenList_CreateOnlyParentReadonlys[cp0rKey];
//        cpn0 = DC.Data.ChildrenList_CreateOnlyParentNullables[cpn0Key];
//        cpn0r = DC.Data.ChildrenList_CreateOnlyParentNullableReadonlys[cpn0rKey];
//        DC.Data.StartTransaction();
//        c0 = new  ChildrenList_Child("C0", p0, p0r, pn0, pn0r, cp0, cp0r, cpn0, cpn0r);
//        var c0Key = c0.Key;
//        cc0 = new  ChildrenList_CreateOnlyChild("CC0", cp0, cp0r, cpn0, cpn0r);
//        var cc0Key = cc0.Key;
//        DC.Data.CommitTransaction();
//        parents.Items[p0Key] = p0.ToString();
//        parentReadonlies.Items[p0rKey] = p0r.ToString();
//        coParents.Items[cp0Key] = cp0.ToString();
//        coParentReadonlies.Items[cp0rKey] = cp0r.ToString();
//        parentNullables.Items[pn0Key] = pn0.ToString();
//        parentNullableReadonlies.Items[pn0rKey] = pn0r.ToString();
//        coParentNullables.Items[cpn0Key] = cpn0.ToString();
//        coParentNullableReadonlies.Items[cpn0rKey] = cpn0r.ToString();
//        children.Items[c0Key] = c0.ToString();
//        childrenTestString[c0Key] = c0.ToTestString();
//        coChildren.Items[cc0Key] = cc0.ToString();
//        children.Set(isContinuous: true, isUpdated: false, isDeleted: false, firstIndex: 0, lastIndex: 0);
//        coChildren.Set(isContinuous: true, isUpdated: false, isDeleted: false, firstIndex: 0, lastIndex: 0);
//        assertData();

//        traceHeader("--- update child C0 with null parents ---");
//        c0 = DC.Data.ChildrenList_Children[c0Key];
//        DC.Data.StartTransaction();
//        c0.Update("C0", c0.Parent, null, c0.CreateOnlyParent, null);
//        DC.Data.RollbackTransaction();
//        assertData();
//        c0 = DC.Data.ChildrenList_Children[c0Key];
//        DC.Data.StartTransaction();
//        c0.Update("C0", c0.Parent, null, c0.CreateOnlyParent, null);
//        DC.Data.CommitTransaction();
//        pn0 = DC.Data.ChildrenList_ParentNullables[pn0Key];
//        parentNullables.Items[pn0Key] = pn0.ToString();
//        cpn0 = DC.Data.ChildrenList_CreateOnlyParentNullables[cpn0Key];
//        coParentNullables.Items[cpn0Key] = cpn0.ToString();
//        children.Items[c0Key] = c0.ToString();
//        childrenTestString[c0Key] = c0.ToTestString();
//        children.Set(isContinuous: true, isUpdated: true, isDeleted: false, firstIndex: 0, lastIndex: 0);
//        assertData();
//        children.Set(isContinuous: true, isUpdated: false, isDeleted: false, firstIndex: 0, lastIndex: 0);

//        traceHeader("--- update child C0 with parents 1 ---");
//        c0 = DC.Data.ChildrenList_Children[c0Key];
//        DC.Data.StartTransaction();
//        var p1 = new ChildrenList_Parent("P1");
//        var pn1 = new ChildrenList_ParentNullable("PN1");
//        var cp1 = new ChildrenList_CreateOnlyParent("CP1");
//        var cpn1 = new ChildrenList_CreateOnlyParentNullable("CPN1");
//        c0.Update("C0U", p1, pn1, cp1, cpn1);
//        DC.Data.RollbackTransaction();
//        assertData();
//        c0 = DC.Data.ChildrenList_Children[c0Key];
//        DC.Data.StartTransaction();
//        p1 = new ChildrenList_Parent("P1");
//        var p1Key = p1.Key;
//        pn1 = new ChildrenList_ParentNullable("PN1");
//        var pn1Key = pn1.Key;
//        cp1 = new ChildrenList_CreateOnlyParent("CP1");
//        var cp1Key = cp1.Key;
//        cpn1 = new ChildrenList_CreateOnlyParentNullable("CPN1");
//        var cpn1Key = cpn1.Key;
//        c0.Update("C0U", p1, pn1, cp1, cpn1);
//        DC.Data.CommitTransaction();
//        p0 = DC.Data.ChildrenList_Parents[p0Key];
//        parents.Items[p0Key] = p0.ToString();
//        parents.Items[p1Key] = p1.ToString();
//        parents.Set(isContinuous: true, isUpdated: false, isDeleted: false, firstIndex: 0, lastIndex: 1);
//        pn0 = DC.Data.ChildrenList_ParentNullables[pn0Key];
//        parentNullables.Items[pn0Key] = pn0.ToString();
//        parentNullables.Items[pn1Key] = pn1.ToString();
//        parentNullables.Set(isContinuous: true, isUpdated: false, isDeleted: false, firstIndex: 0, lastIndex: 1);
//        cp0 = DC.Data.ChildrenList_CreateOnlyParents[cp0Key];
//        coParents.Items[cp0Key] = cp0.ToString();
//        coParents.Items[cp1Key] = cp1.ToString();
//        coParents.Set(isContinuous: true, isUpdated: false, isDeleted: false, firstIndex: 0, lastIndex: 1);
//        cpn0 = DC.Data.ChildrenList_CreateOnlyParentNullables[cpn0Key];
//        coParentNullables.Items[cpn0Key] = cpn0.ToString();
//        coParentNullables.Items[cpn1Key] = cpn1.ToString();
//        coParentNullables.Set(isContinuous: true, isUpdated: false, isDeleted: false, firstIndex: 0, lastIndex: 1);
//        children.Items[c0Key] = c0.ToString();
//        childrenTestString[c0Key] = c0.ToTestString();
//        children.Set(isContinuous: true, isUpdated: true, isDeleted: false, firstIndex: 0, lastIndex: 0);
//        assertData();
//        children.Set(isContinuous: true, isUpdated: false, isDeleted: false, firstIndex: 0, lastIndex: 0);

//        traceHeader("--- add children C1, CC1 with parents 0 ---");
//        p0 = DC.Data.ChildrenList_Parents[p0Key];
//        p0r = DC.Data.ChildrenList_ParentReadonlys[p0rKey];
//        cp0 = DC.Data.ChildrenList_CreateOnlyParents[cp0Key];
//        cp0r = DC.Data.ChildrenList_CreateOnlyParentReadonlys[cp0rKey];
//        DC.Data.StartTransaction();
//        var c1 = new ChildrenList_Child("C1", p0, p0r, null, null, cp0, cp0r, null, null);
//        var c1Key = c1.Key;
//        var cc1 = new ChildrenList_CreateOnlyChild("CC1", cp0, cp0r, null, null);
//        var cc1Key = cc1.Key;
//        DC.Data.RollbackTransaction();
//        assertData();
//        p0 = DC.Data.ChildrenList_Parents[p0Key];
//        p0r = DC.Data.ChildrenList_ParentReadonlys[p0rKey];
//        cp0 = DC.Data.ChildrenList_CreateOnlyParents[cp0Key];
//        cp0r = DC.Data.ChildrenList_CreateOnlyParentReadonlys[cp0rKey];
//        DC.Data.StartTransaction();
//        c1 = new ChildrenList_Child("C1", p0, p0r, null, null, cp0, cp0r, null, null);
//        cc1 = new ChildrenList_CreateOnlyChild("CC1", cp0, cp0r, null, null);
//        DC.Data.CommitTransaction();
//        children.Items[c1Key] = c1.ToString();
//        childrenTestString[c1Key] = c1.ToTestString();
//        coChildren.Items[cc1Key] = cc1.ToString();
//        parents.Items[p0Key] = p0.ToString();
//        coParents.Items[cp0Key] = cp0.ToString();
//        children.Set(isContinuous: true, isUpdated: false, isDeleted: false, firstIndex: 0, lastIndex: 1);
//        coChildren.Set(isContinuous: true, isUpdated: false, isDeleted: false, firstIndex: 0, lastIndex: 1);
//        assertData();

//        traceHeader("--- test reader and writer ---");
//        DC.DisposeData();
//        var fromFilePath = csvConfig.DirectoryPath + "\\ChildrenList_Child.csv";
//        var toFilePath = csvConfig.DirectoryPath + "\\ChildrenList_Child.new";
//        var c2Key = 2;
//        using (var reader = new ChildrenList_ChildReader(fromFilePath, csvConfig)) {
//          using var writer = new ChildrenList_ChildWriter(toFilePath, csvConfig);
//          var childIndex = 0;
//          while (reader.ReadLine(out var childRaw)) {
//            Assert.AreEqual(childrenTestString[childIndex++], childRaw.ToTestString());
//            writer.Write(childRaw);
//          }
//          var newChildRaw = new ChildrenList_ChildRaw{
//            Key = c2Key,
//            Text = "C2",
//            ParentKey = 0,
//            ParentNullableKey = 0,
//            CreateOnlyParentKey = 0,
//            CreateOnlyParentNullableKey = 0
//          };
//          writer.Write(newChildRaw);
//          childrenTestString[newChildRaw.Key] = newChildRaw.ToTestString();
//        }
//        File.Delete(fromFilePath);
//        File.Move(toFilePath, fromFilePath);
//        initDC();
//        children.Items[2] = DC.Data.ChildrenList_Children[2].ToString();
//        p0 = DC.Data.ChildrenList_Parents[p0Key];
//        pn0 = DC.Data.ChildrenList_ParentNullables[pn0Key];
//        cp0 = DC.Data.ChildrenList_CreateOnlyParents[cp0Key];
//        cpn0 = DC.Data.ChildrenList_CreateOnlyParentNullables[cpn0Key];
//        parents.Items[p0Key] = p0.ToString();
//        parentNullables.Items[pn0Key] = pn0.ToString();
//        coParents.Items[cp0Key] = cp0.ToString();
//        coParentNullables.Items[cpn0Key] = cpn0.ToString();
//        children.Set(isContinuous: true, isUpdated: false, isDeleted: false, firstIndex: 0, lastIndex: 2);
//        assertData();

//        //some tests without disposing DC between tests
//        //---------------------------------------------

//        traceHeader("--- release child C1 ---");
//        c1 = DC.Data.ChildrenList_Children[c1Key];
//        DC.Data.StartTransaction();
//        c1.Release();
//        DC.Data.RollbackTransaction();
//        assertData();
//        c1 = DC.Data.ChildrenList_Children[c1Key];
//        DC.Data.StartTransaction();
//        c1.Release();
//        DC.Data.CommitTransaction();
//        children.Items.Remove(c1Key);
//        childrenTestString.Remove(c1Key);
//        children.Set(isContinuous: false, isUpdated: false, isDeleted: true, firstIndex: 0, lastIndex: 2);
//        var v = parents.Items[p0Key]==DC.Data.ChildrenList_Parents[p0Key].ToString();
//        parents.Items[p0Key] = DC.Data.ChildrenList_Parents[p0Key].ToString();
//        coParents.Items[cp0Key] = DC.Data.ChildrenList_CreateOnlyParents[cp0Key].ToString();
//        //assertData(); doesn't work, because before closing the DC, the parent has still the released child in Children, but nor in new DC
//        assertDC(isAfterDispose: false);

//        traceHeader("--- release child C0 ---");
//        c0 = DC.Data.ChildrenList_Children[c0Key];
//        DC.Data.StartTransaction();
//        c0.Release();
//        DC.Data.RollbackTransaction();
//        assertDC(isAfterDispose: false);
//        c0 = DC.Data.ChildrenList_Children[c0Key];
//        DC.Data.StartTransaction();
//        c0.Release();
//        DC.Data.CommitTransaction();
//        children.Items.Remove(c0Key);
//        childrenTestString.Remove(c0Key);
//        children.Set(isContinuous: true, isUpdated: false, isDeleted: true, firstIndex: 2, lastIndex: 2);
//        parents.Items[p1Key] = DC.Data.ChildrenList_Parents[p1Key].ToString();
//        parentNullables.Items[pn1Key] = DC.Data.ChildrenList_ParentNullables[pn1Key].ToString();
//        coParents.Items[cp1Key] = DC.Data.ChildrenList_CreateOnlyParents[cp1Key].ToString();
//        coParentNullables.Items[cpn1Key] = DC.Data.ChildrenList_CreateOnlyParentNullables[cpn1Key].ToString();
//        assertDC(isAfterDispose: false);

//        traceHeader("--- release child C2 ---");
//        var c2 = DC.Data.ChildrenList_Children[2];
//        DC.Data.StartTransaction();
//        c2.Release();
//        DC.Data.RollbackTransaction();
//        assertDC(isAfterDispose: false);
//        c2 = DC.Data.ChildrenList_Children[c2.Key];
//        DC.Data.StartTransaction();
//        c2.Release();
//        DC.Data.CommitTransaction();
//        children.Items.Remove(c2Key);
//        childrenTestString.Remove(c2Key);
//        children.Set(isContinuous: true, isUpdated: false, isDeleted: true, firstIndex: -1, lastIndex: -1);
//        parents.Items[p0Key] = DC.Data.ChildrenList_Parents[p0Key].ToString();
//        parentNullables.Items[pn0Key] = DC.Data.ChildrenList_ParentNullables[pn0Key].ToString();
//        coParents.Items[cp0Key] = DC.Data.ChildrenList_CreateOnlyParents[cp0Key].ToString();
//        coParentNullables.Items[cpn0Key] = DC.Data.ChildrenList_CreateOnlyParentNullables[cpn0Key].ToString();
//        assertDC(isAfterDispose: false);

//      } finally {
//        DC.DisposeData();
//      }
//    }


//    static readonly StringBuilder traceStrinBuilder = new StringBuilder();

//    public static string TracesString {
//      get {
//        return traceStrinBuilder.ToString();
//      } 
//    }


//    static bool isInitialisingDcTrace;


//    private static void traceHeader(string line) {
//      traceStrinBuilder.AppendLine();
//      traceStrinBuilder.AppendLine(line);
//    }


//    //private static void trace(string line) {
//    //  traceStrinBuilder.AppendLine(line);
//    //}


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


//    private void reportException(Exception obj) {
//      System.Diagnostics.Debug.WriteLine(obj);
//      System.Diagnostics.Debugger.Break();
//      Assert.Fail();
//    }


//    private void initDC() {
//      _ = new DC(csvConfig);
//    }


//    private void assertData() {
//      assertDC(isAfterDispose: false);
//      DC.DisposeData();

//      initDC();
//      assertDC(isAfterDispose: true);
//    }


//    private void assertDC(bool isAfterDispose, bool skipChildren = false) {
//      var store = DC.Data.ChildrenList_Parents;
//      assertStore(parents, store, isAfterDispose);
//      foreach (var parent in DC.Data.ChildrenList_Parents) {
//        Assert.AreEqual(parents.Items[parent.Key], parent.ToString());
//      }

//      var store1 = DC.Data.ChildrenList_ParentNullables;
//      assertStore(parentNullables, store1, isAfterDispose);
//      foreach (var parent in DC.Data.ChildrenList_ParentNullables) {
//        Assert.AreEqual(parentNullables.Items[parent.Key], parent.ToString());
//      }

//      var store2 = DC.Data.ChildrenList_CreateOnlyParents;
//      assertStore(coParents, store2, isAfterDispose);
//      foreach (var parent in DC.Data.ChildrenList_CreateOnlyParents) {
//        Assert.AreEqual(coParents.Items[parent.Key], parent.ToString());
//      }

//      var store3 = DC.Data.ChildrenList_CreateOnlyParentNullables;
//      assertStore(coParentNullables, store3, isAfterDispose);
//      foreach (var parent in DC.Data.ChildrenList_CreateOnlyParentNullables) {
//        Assert.AreEqual(coParentNullables.Items[parent.Key], parent.ToString());
//      }

//      var store4 = DC.Data.ChildrenList_Children;
//      if (!skipChildren) {
//        foreach (var child in DC.Data.ChildrenList_Children) {
//          Assert.AreEqual(children.Items[child.Key], child.ToString());
//        }

//        Assert.AreEqual(childrenTestString.Count, DC.Data.ChildrenList_Children.Count);
//        foreach (var child in DC.Data.ChildrenList_Children) {
//          Assert.AreEqual(childrenTestString[child.Key], child.ToTestString());
//        }
//      }
//      assertStore(children, store4, isAfterDispose, skipCount: skipChildren);

//      var store5 = DC.Data.ChildrenList_CreateOnlyChildren;
//      assertStore(coChildren, store5, isAfterDispose);
//      foreach (var child in DC.Data.ChildrenList_CreateOnlyChildren) {
//        Assert.AreEqual(coChildren.Items[child.Key], child.ToString());
//      }
//    }


//    static readonly PropertyInfo firstItemIndexProperty =  typeof(DataStore).GetProperty("FirstItemIndex",
//      BindingFlags.NonPublic | BindingFlags.Instance)!;
//    static readonly PropertyInfo lastItemIndexProperty = typeof(DataStore).GetProperty("LastItemIndex",
//      BindingFlags.NonPublic | BindingFlags.Instance)!;


//    private static void assertStore(DataStoreStats storeStats, DataStore store, bool isAfterDispose, bool skipCount = false) {
//      Assert.AreEqual(storeStats.IsContinuous, store.AreKeysContinuous);
//      if (isAfterDispose) {
//        Assert.AreEqual(false, store.AreItemsDeleted);
//        Assert.AreEqual(false, store.AreItemsUpdated);
//        if (storeStats.FirstIndex==-1) {
//          Assert.AreEqual(-1, firstItemIndexProperty.GetValue(store));
//          Assert.AreEqual(-1, lastItemIndexProperty.GetValue(store));
//        } else {
//          Assert.AreEqual(0, firstItemIndexProperty.GetValue(store));
//          Assert.AreEqual(storeStats.LastIndex-storeStats.FirstIndex, lastItemIndexProperty.GetValue(store));
//        }
//      } else {
//        Assert.AreEqual(storeStats.IsDeleted, store.AreItemsDeleted);
//        Assert.AreEqual(storeStats.IsUpdated, store.AreItemsUpdated);
//        Assert.AreEqual(storeStats.FirstIndex, firstItemIndexProperty.GetValue(store));
//        Assert.AreEqual(storeStats.LastIndex, lastItemIndexProperty.GetValue(store));
//      }
//      if (!skipCount) {
//        Assert.AreEqual(storeStats.Items.Count, store.Count);
//      }
//    }
//    #endregion


//    #region TestChildrenListPerformance
//    //      ---------------------------

//    /*
//14.9.2020 (add none stored children to any parent, no more adding children to parents during item.Store()
//without tracing:

//with tracing:
//100000 activitiesCount
//10 childrenPerParent
//00:00:03.3604996 Create, commit
//00:00:02.0213678 Update, commit
//00:00:01.4107277 Remove, commit
//00:00:03.4889682 Create, rollback
//00:00:02.1190361 Update, rollback
//00:00:01.3925021 Remove, rollback
//00:00:04.0611886 Create, noTransaction
//00:00:01.7274017 Update, noTransaction
//00:00:01.2361274 Remove, noTransaction

//10000 activitiesCount
//10 childrenPerParent
//00:00:00.7318281 Create, commit
//00:00:00.3340424 Update, commit
//00:00:00.2507061 Remove, commit
//00:00:00.6656693 Create, rollback
//00:00:00.2050103 Update, rollback
//00:00:00.1498911 Remove, rollback
//00:00:00.3366192 Create, noTransaction
//00:00:00.2113934 Update, noTransaction
//00:00:00.1450116 Remove, noTransaction

//1000 activitiesCount
//10 childrenPerParent
//00:00:00.0452840 Create, commit
//00:00:00.0263526 Update, commit
//00:00:00.0232695 Remove, commit
//00:00:00.1137588 Create, rollback
//00:00:00.0382345 Update, rollback
//00:00:00.0282973 Remove, rollback
//00:00:00.0560378 Create, noTransaction
//00:00:00.0124336 Update, noTransaction
//00:00:00.0252250 Remove, noTransaction

//30.7.2020
//100000 activitiesCount
//10 childrenPerParent
//00:00:03.1164733 Create, commit
//00:00:01.8728488 Update, commit
//00:00:01.5257475 Remove, commit
//00:00:03.7565031 Create, rollback
//00:00:02.2642793 Update, rollback
//00:00:01.6454205 Remove, rollback
//00:00:04.6097188 Create, noTransaction
//00:00:02.0508278 Update, noTransaction
//00:00:01.7699474 Remove, noTransaction

//26.7.2020: Changed event item_HasChanged to call of ItemHasChanged()
//100000 activitiesCount
//10 childrenPerParent
//00:00:02.4873223 Create, noTransaction
//00:00:02.1326261 Update, noTransaction
//00:00:02.0027097 Remove, noTransaction
//00:00:03.1469637 Create, rollback
//00:00:02.7766883 Update, rollback
//00:00:02.2212682 Remove, rollback
//00:00:02.6006215 Create, commit
//00:00:02.2364214 Update, commit
//00:00:02.3673098 Remove, commit

//1000 activitiesCount
//10 childrenPerParent
//00:00:00.0168216 Create, noTransaction
//00:00:00.0154454 Update, noTransaction
//00:00:00.0143725 Remove, noTransaction
//00:00:00.0253890 Create, rollback
//00:00:00.0225073 Update, rollback
//00:00:00.0177456 Remove, rollback
//00:00:00.0179672 Create, commit
//00:00:00.0171397 Update, commit
//00:00:00.0142170 Remove, commit


//    25.7.2020
//100000 activitiesCount
//10 childrenPerParent
//00:00:02.6508348 Create, noTransaction
//00:00:02.2467978 Update, noTransaction
//00:00:02.0047998 Remove, noTransaction
//00:00:03.7706295 Create, rollback
//00:00:03.1206089 Update, rollback
//00:00:02.6421976 Remove, rollback
//00:00:03.8709616 Create, commit
//00:00:02.3105336 Update, commit
//00:00:01.8777740 Remove, commit

//10000 activitiesCount
//10 childrenPerParent
//00:00:00.2404470 Create, noTransaction
//00:00:00.2120004 Update, noTransaction
//00:00:00.1649583 Remove, noTransaction
//00:00:00.3387088 Create, rollback
//00:00:00.1864754 Update, rollback
//00:00:00.2326274 Remove, rollback
//00:00:00.3366866 Create, commit
//00:00:00.1783481 Update, commit
//00:00:00.3059561 Remove, commit

//1000 activitiesCount
//10 childrenPerParent
//00:00:00.0180422 Create, noTransaction
//00:00:00.0160481 Update, noTransaction
//00:00:00.0180843 Remove, noTransaction
//00:00:00.0301951 Create, rollback
//00:00:00.0175754 Update, rollback
//00:00:00.0204837 Remove, rollback
//00:00:00.0243624 Create, commit
//00:00:00.0130577 Update, commit
//00:00:00.0238833 Remove, commit

//    */

//    [TestMethod]
//    public void TestChildrenListPerformance() {
//      try {
//        var directoryInfo = new DirectoryInfo("TestCsv");
//        if (directoryInfo.Exists) {
//          directoryInfo.Delete(recursive: true);
//          directoryInfo.Refresh();
//        }

//        directoryInfo.Create();

//        GC.Collect(); //in case garbage collector would run soon because of previous tests
//        GC.WaitForPendingFinalizers();
//        csvConfig = new CsvConfig(directoryInfo.FullName, reportException: reportException);
//        var activitiesCount = 1_000;
//        var childrenPerParent = 10;
//        var sw = new Stopwatch();
//        Debug.WriteLine($"{activitiesCount} activitiesCount");
//        Debug.WriteLine($"{childrenPerParent} childrenPerParent");

//        //testNormal(activitiesCount, childrenPerParent, csvConfig, sw);
//        testCommit(activitiesCount, childrenPerParent, csvConfig, sw);
//        directoryInfo.Delete(recursive: true);
//        directoryInfo.Create();
//        GC.Collect();
//        GC.WaitForPendingFinalizers();

//        testRollback(activitiesCount, childrenPerParent, csvConfig, sw);
//        directoryInfo.Delete(recursive: true);
//        directoryInfo.Create();
//        GC.Collect();
//        GC.WaitForPendingFinalizers();

//        //testCommit(activitiesCount, childrenPerParent, csvConfig, sw);
//        testNormal(activitiesCount, childrenPerParent, csvConfig, sw);
//      } finally {
//        DC.DisposeData();
//      }
//    }


//    private static void testNormal(int activitiesCount, int childrenPerParent, CsvConfig csvConfig, Stopwatch sw) {
//      _ = new DC(csvConfig);
//      //new DC(null);
//      createParents(activitiesCount, out var parentP, out var parentPr, out var parentPC, out var parentPCr);
//      sw.Start();
//      createChildren(activitiesCount, childrenPerParent, parentP, parentPr, parentPC, parentPCr, out var childrenP, out var cIndex);
//      DC.Data.ChildrenList_Parents.Flush();
//      DC.Data.ChildrenList_CreateOnlyParents.Flush();
//      DC.Data.ChildrenList_Children.Flush();
//      sw.Stop();
//      Debug.WriteLine($"{sw.Elapsed} Create, noTransaction");

//      sw.Restart();
//      for (int i = 0; i < cIndex; i++) {
//        var child = childrenP[i];
//        child.Update(child.Text + "U", child.Parent, null, child.CreateOnlyParent, null);
//      }
//      DC.Data.ChildrenList_Parents.Flush();
//      DC.Data.ChildrenList_CreateOnlyParents.Flush();
//      DC.Data.ChildrenList_Children.Flush();
//      sw.Stop();
//      Debug.WriteLine($"{sw.Elapsed} Update, noTransaction");

//      sw.Restart();
//      for (int i = 0; i < cIndex; i++) {
//        childrenP[i].Release();
//      }
//      DC.Data.ChildrenList_Parents.Flush();
//      DC.Data.ChildrenList_CreateOnlyParents.Flush();
//      DC.Data.ChildrenList_Children.Flush();
//      sw.Stop();
//      Debug.WriteLine($"{sw.Elapsed} Remove, noTransaction");
//      DC.Data.Dispose();
//    }


//    private static void testRollback(int activitiesCount, int childrenPerParent, CsvConfig csvConfig, Stopwatch sw) {
//      _ = new DC(csvConfig);

//      createParents(activitiesCount, out var parentP, out var parentPr, out var parentPC, out var parentPCr);
//      sw.Restart();
//      DC.Data.StartTransaction();
//      createChildren(activitiesCount, childrenPerParent, parentP, parentPr, parentPC, parentPCr, out _, out _);
//      DC.Data.RollbackTransaction();
//      DC.Data.ChildrenList_Parents.Flush();
//      DC.Data.ChildrenList_CreateOnlyParents.Flush();
//      DC.Data.ChildrenList_Children.Flush();
//      sw.Stop();
//      Debug.WriteLine($"{sw.Elapsed} Create, rollback");
//      createChildren(activitiesCount, childrenPerParent, parentP, parentPr, parentPC, parentPCr, out var childrenP, out var cIndex);
//      DC.Data.ChildrenList_Children.Flush();
//      sw.Restart();
//      DC.Data.StartTransaction();
//      for (int i = 0; i < cIndex; i++) {
//        var child = childrenP[i];
//        child.Update(child.Text + "U", child.Parent, null, child.CreateOnlyParent, null);
//      }
//      DC.Data.RollbackTransaction();
//      DC.Data.ChildrenList_Parents.Flush();
//      DC.Data.ChildrenList_CreateOnlyParents.Flush();
//      DC.Data.ChildrenList_Children.Flush();
//      sw.Stop();
//      Debug.WriteLine($"{sw.Elapsed} Update, rollback");

//      sw.Restart();
//      DC.Data.StartTransaction();
//      for (int i = 0; i < cIndex; i++) {
//        childrenP[i].Release();
//      }
//      DC.Data.RollbackTransaction();
//      DC.Data.ChildrenList_Parents.Flush();
//      DC.Data.ChildrenList_CreateOnlyParents.Flush();
//      DC.Data.ChildrenList_Children.Flush();
//      sw.Stop();
//      Debug.WriteLine($"{sw.Elapsed} Remove, rollback");
//      DC.Data.Dispose();
//    }


//    private static void testCommit(int activitiesCount, int childrenPerParent, CsvConfig csvConfig, Stopwatch sw) {
//      _ = new DC(csvConfig);

//      createParents(activitiesCount, out var parentP, out var parentPr, out var parentPC, out var parentPCr);
//      sw.Restart();
//      DC.Data.StartTransaction();
//      createChildren(activitiesCount, childrenPerParent, parentP, parentPr, parentPC, parentPCr, out var childrenP, out var cIndex);
//      DC.Data.CommitTransaction();
//      DC.Data.ChildrenList_Parents.Flush();
//      DC.Data.ChildrenList_CreateOnlyParents.Flush();
//      DC.Data.ChildrenList_Children.Flush();
//      sw.Stop();
//      Debug.WriteLine($"{sw.Elapsed} Create, commit");

//      sw.Restart();
//      DC.Data.StartTransaction();
//      for (int i = 0; i < cIndex; i++) {
//        var child = childrenP[i];
//        child.Update(child.Text + "U", child.Parent, null, child.CreateOnlyParent, null);
//      }
//      DC.Data.CommitTransaction();
//      DC.Data.ChildrenList_Parents.Flush();
//      DC.Data.ChildrenList_CreateOnlyParents.Flush();
//      DC.Data.ChildrenList_Children.Flush();
//      sw.Stop();
//      Debug.WriteLine($"{sw.Elapsed} Update, commit");

//      sw.Restart();
//      DC.Data.StartTransaction();
//      for (int i = 0; i < cIndex; i++) {
//        childrenP[i].Release();
//      }
//      DC.Data.CommitTransaction();
//      DC.Data.ChildrenList_Parents.Flush();
//      DC.Data.ChildrenList_CreateOnlyParents.Flush();
//      DC.Data.ChildrenList_Children.Flush();
//      sw.Stop();
//      Debug.WriteLine($"{sw.Elapsed} Remove, commit");
//      DC.Data.Dispose();
//    }


//    private static void createParents(
//      int activitiesCount, 
//      out ChildrenList_Parent[] parentP,
//      out ChildrenList_ParentReadonly[] parentPr, 
//      out ChildrenList_CreateOnlyParent[] parentPC,
//      out ChildrenList_CreateOnlyParentReadonly[] parentPCr) 
//    {
//      parentP = new ChildrenList_Parent[activitiesCount];
//      parentPr = new ChildrenList_ParentReadonly[activitiesCount];
//      parentPC = new ChildrenList_CreateOnlyParent[activitiesCount];
//      parentPCr = new ChildrenList_CreateOnlyParentReadonly[activitiesCount];
//      for (int i = 0; i < activitiesCount; i++) {
//        parentP[i] = new ChildrenList_Parent($"{i}");
//        parentPr[i] = new ChildrenList_ParentReadonly($"{i}");
//        parentPC[i] = new ChildrenList_CreateOnlyParent($"{i}");
//        parentPCr[i] = new ChildrenList_CreateOnlyParentReadonly($"{i}");
//      }
//    }


//    private static void createChildren(
//      int activitiesCount,
//      int childrenPerParent,
//      ChildrenList_Parent[] parentP,
//      ChildrenList_ParentReadonly[] parentPr,
//      ChildrenList_CreateOnlyParent[] parentPC,
//      ChildrenList_CreateOnlyParentReadonly[] parentPCr,
//      out ChildrenList_Child[] childrenP,
//      out int cIndex) 
//    {
//      cIndex = 0;
//      childrenP = new ChildrenList_Child[childrenPerParent * activitiesCount];
//      for (int i = 0; i < activitiesCount; i++) {
//        var pP = parentP[i];
//        var pPr = parentPr[i];
//        var pC = parentPC[i];
//        var pCr = parentPCr[i];
//        for (int j = 0; j < childrenPerParent; j++) {
//          childrenP[cIndex++] = new ChildrenList_Child($"{cIndex}", pP, pPr, null, null, pC, pCr, null, null);
//        }
//      }
//    }


//    //private void createChildrenParents(
//    //  int activitiesCount,
//    //  int childrenPerParent,
//    //  out ChildrenList_Parent[] parentP,
//    //  out ChildrenList_CreateOnlyParent[] parentPC,
//    //  out ChildrenList_Child[] childrenP,
//    //  out int cIndex) {
//    //  cIndex = 0;
//    //  parentP = new ChildrenList_Parent[activitiesCount];
//    //  parentPC = new ChildrenList_CreateOnlyParent[activitiesCount];
//    //  childrenP = new ChildrenList_Child[childrenPerParent * activitiesCount];
//    //  for (int i = 0; i < activitiesCount; i++) {
//    //    var pP = new ChildrenList_Parent($"{i}");
//    //    parentP[i] = pP;
//    //    var pC = new ChildrenList_CreateOnlyParent($"{i}");
//    //    parentPC[i] = pC;
//    //    for (int j = 0; j < childrenPerParent; j++) {
//    //      childrenP[cIndex++] = new ChildrenList_Child($"{cIndex}", pP, null, pC, null);
//    //    }
//    //  }
//    //}
//    #endregion
//  }


//  public static class ChildrenListTestExtensions {

//    public static string ToTestString(this ChildrenList_Child child) {
//      return $"{child.Key}, {child.Text}, {child.Parent.Key}, {child.ParentNullable?.Key}, {child.CreateOnlyParent.Key}, " +
//        $"{child.CreateOnlyParentNullable?.Key}";
//    }

//    public static string ToTestString(this ChildrenList_ChildRaw child) {
//      return $"{child.Key}, {child.Text}, {child.ParentKey}, {child.ParentNullableKey}, {child.CreateOnlyParentKey}, " +
//        $"{child.CreateOnlyParentNullableKey}";
//    }
//  }
//}