using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageLib;
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
        var c = p0.Children.Count;
        Assert.AreEqual(0, p0.Children.Count);
        Assert.AreEqual(0, p0.Children.CountAll);

        var c0 = new NotMatchingChildrenListName_Child("c0.0", p0, isStoring: false);
        Assert.AreEqual(1, p0.Children.Count);
        Assert.AreEqual(1, p0.Children.CountAll);
        Assert.AreEqual(c0, p0.Children.First());

        var c1 = new NotMatchingChildrenListName_Child("c1.0", p0, isStoring: false);
        Assert.AreEqual(2, p0.Children.Count);
        Assert.AreEqual(2, p0.Children.CountAll);
        Assert.AreEqual(c0, p0.Children.First());
        Assert.AreEqual(c1, p0.Children.Skip(1).First());

        p0.Store();
        Assert.AreEqual(0, p0.Children.Count);
        Assert.AreEqual(2, p0.Children.CountAll);
        Assert.IsNull(p0.Children.FirstOrDefault());
        Assert.AreEqual(c0, p0.Children.GetAll().First());
        Assert.AreEqual(c1, p0.Children.GetAll().Skip(1).First());

        c0.Store();
        Assert.AreEqual(1, p0.Children.Count);
        Assert.AreEqual(2, p0.Children.CountAll);
        Assert.AreEqual(c0, p0.Children.First());
        Assert.AreEqual(c0, p0.Children.GetAll().First());
        Assert.AreEqual(c1, p0.Children.GetAll().Skip(1).First());

        var p1 = new NotMatchingChildrenListName_Parent("P1", isStoring: true);
        c0.Update("c0.1", p1);
        Assert.AreEqual(1, p1.Children.Count);
        Assert.AreEqual(c0, p1.Children.First());

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
