using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageLib;
using System;
using System.IO;
using System.Linq;
using TestContext;


namespace StorageTest {


  [TestClass]
  public class NotMatchingChildrenListNameTest {


    CsvConfig? csvConfig;


    [TestMethod]
    public void TestNotMatchingChildrenListName() {
      try {
        var directoryInfo = new DirectoryInfo("TestCsv");
        if (directoryInfo.Exists) {
          directoryInfo.Delete(recursive: true);
          directoryInfo.Refresh();
        }

        directoryInfo.Create();

        csvConfig = new CsvConfig(directoryInfo.FullName, reportException: reportException);
        _ = new DC(csvConfig);

        var p0 = new NotMatchingChildrenListName_Parent("P0", isStoring: false);
        Assert.AreEqual(0, p0.Children.Count);
        Assert.AreEqual(0, p0.Children.CountStoredItems);

        var c0 = new NotMatchingChildrenListName_Child("c0.0", p0, isStoring: false);
        Assert.AreEqual(0, p0.Children.CountStoredItems);
        Assert.AreEqual(1, p0.Children.Count);
        Assert.AreEqual(c0, p0.Children[0]);

        var c1 = new NotMatchingChildrenListName_Child("c1.0", p0, isStoring: false);
        Assert.AreEqual(0, p0.Children.CountStoredItems);
        Assert.AreEqual(2, p0.Children.Count);
        Assert.AreEqual(c0, p0.Children[0]);
        Assert.AreEqual(c1, p0.Children[1]);

        p0.Store();
        Assert.AreEqual(0, p0.Children.CountStoredItems);
        Assert.AreEqual(2, p0.Children.Count);
        Assert.AreEqual(c0, p0.Children[0]);
        Assert.AreEqual(c1, p0.Children[1]);

        c0.Store();
        Assert.AreEqual(1, p0.Children.CountStoredItems);
        Assert.AreEqual(2, p0.Children.Count);
        Assert.AreEqual(c0, p0.Children.GetStoredItems().First());
        Assert.AreEqual(c0, p0.Children[0]);
        Assert.AreEqual(c1, p0.Children[1]);

        var p1 = new NotMatchingChildrenListName_Parent("P1", isStoring: true);
        c0.Update("c0.1", p1);
        Assert.AreEqual(0, p0.Children.CountStoredItems);
        Assert.AreEqual(1, p0.Children.Count);
        Assert.AreEqual(1, p1.Children.CountStoredItems);
        Assert.AreEqual(1, p1.Children.Count);
        Assert.AreEqual(c0, p1.Children.GetStoredItems().First());
        Assert.AreEqual(c0, p1.Children[0]);
        Assert.AreEqual(c1, p0.Children[0]);

        c0.Release();

      } finally {
        DC.DisposeData();
      }
    }


    private void reportException(Exception obj) {
      System.Diagnostics.Debug.WriteLine(obj);
      System.Diagnostics.Debugger.Break();
      Assert.Fail();
    }
  }
}
