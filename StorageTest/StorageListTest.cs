using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestContext;

namespace StorageTest {


  [TestClass]
  public class StorageListTest {
 
    
    [TestMethod]
    public void TestStorageList() {
      _ = new DC(null);

      var parent = new TestParent("Parent", isStoring: false);
      var storageList = new StorageList<TestChild>();
      assert(storageList, "", "");

      var child0 = new TestChild("Child0", parent, isStoring: false);
      storageList.Add(child0);
      assert(storageList,
        @"",
        @"Key: noKey, Text: Child0, Parent: noKey, Parent;");

      var child1 = new TestChild("Child1", parent, isStoring: false);
      storageList.Add(child1);
      assert(storageList,
        @"",
        @"Key: noKey, Text: Child0, Parent: noKey, Parent;
          Key: noKey, Text: Child1, Parent: noKey, Parent;");

      parent.Store();
      assert(storageList,
        @"",
        @"Key: noKey, Text: Child0, Parent: 0, Parent;
          Key: noKey, Text: Child1, Parent: 0, Parent;");

      child0.Store();
      assert(storageList,
        @"Key: 0, Text: Child0, Parent: 0, Parent;",
        @"Key: 0, Text: Child0, Parent: 0, Parent;
          Key: noKey, Text: Child1, Parent: 0, Parent;");

      child1.Store();
      assert(storageList,
        @"Key: 0, Text: Child0, Parent: 0, Parent;
          Key: 1, Text: Child1, Parent: 0, Parent;",
        @"Key: 0, Text: Child0, Parent: 0, Parent;
          Key: 1, Text: Child1, Parent: 0, Parent;");

      child0.Release();
      assert(storageList,
        @"Key: 1, Text: Child1, Parent: 0, Parent;",
        @"Key: noKey, Text: Child0, Parent: 0, Parent;
          Key: 1, Text: Child1, Parent: 0, Parent;");
    }


    readonly StringBuilder sb = new StringBuilder();


    private void assert(StorageList<TestChild> storageList, string expectedStored, string expectedAll) {
      sb.Clear();
      foreach (var item in storageList.GetStoredItems()) {
        sb.AppendLine(item.ToString());
      }
      if (expectedStored.Length==0) {
        Assert.AreEqual(0, sb.Length);
      } else {
        expectedStored = expectedStored.Replace(Environment.NewLine + "          ", Environment.NewLine);
        Assert.AreEqual(expectedStored + Environment.NewLine, sb.ToString());
      }

      sb.Clear();
      foreach (var item in storageList) {
        sb.AppendLine(item.ToString());
      }
      if (expectedAll.Length==0) {
        Assert.AreEqual(0, sb.Length);
      } else {
        expectedAll = expectedAll.Replace(Environment.NewLine + "          ", Environment.NewLine);
        Assert.AreEqual(expectedAll + Environment.NewLine, sb.ToString());
      }

      //var parent = storageList.Parent;
      //sb.Clear();
      //foreach (var item in parent.Children) {
      //  sb.AppendLine(item.ToString());
      //}
      //if (expectedForEach.Length==0) {
      //  Assert.AreEqual(0, sb.Length);
      //} else {
      //  expectedForEach = expectedForEach.Replace(Environment.NewLine + "          ", Environment.NewLine);
      //  Assert.AreEqual(expectedForEach + Environment.NewLine, sb.ToString());
      //}
      //sb.Clear();
      //foreach (var item in parent.Children.GetAll()) {
      //  sb.AppendLine(item.ToString());
      //}
      //if (expectedGetAll.Length==0) {
      //  Assert.AreEqual(0, sb.Length);
      //} else {
      //  expectedGetAll = expectedGetAll.Replace(Environment.NewLine + "          ", Environment.NewLine);
      //  Assert.AreEqual(expectedGetAll + Environment.NewLine, sb.ToString());
      //}
    }
  }
}

