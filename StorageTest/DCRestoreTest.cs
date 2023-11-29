using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageLib;
using System;
using System.IO;
using TestContext;


namespace StorageTest {


  [TestClass]
  public class DCRestoreTest {


    [TestMethod]
    public void TestDCRestore() {
      var directoryInfo = new DirectoryInfo("TestCsv");
      var csvConfig = new CsvConfig(directoryInfo.FullName, reportException: reportException);
      directoryInfo.Refresh();
      if (directoryInfo.Exists) {
        directoryInfo.Delete(recursive: true);
        directoryInfo.Refresh();
      }

      directoryInfo.Create();
      directoryInfo.Refresh();

      try {
        var dc = new DC(csvConfig);
        var testParent0 = new TestParent("TestParent0");
        var testParent1 = new TestParent("TestParent1");
        var testParent2 = new TestParent("TestParent2");
        var testChild0 = new TestChild("TestChild0", testParent0);
        var testChild1 = new TestChild("TestChild1", testParent2);
        var testChild2 = new TestChild("TestChild2", testParent0);
        testChild1.Release();
        testParent2.Release();
        testChild2.Update("TestChild2 updated", testParent1);
        var expectedTestParent0 = testParent0.ToString();
        var expectedTestParent1 = testParent1.ToString();
        var expectedTestChild0 = testChild0.ToString();
        var expectedTestChild2 = testChild2.ToString();
        dc.Dispose();
        File.Delete(directoryInfo.FullName + @"\TestParent.csv");
        File.Move(directoryInfo.FullName + @"\TestParent.bak", directoryInfo.FullName + @"\TestParent.csv");
        File.Delete(directoryInfo.FullName + @"\TestChild.csv");
        File.Move(directoryInfo.FullName + @"\TestChild.bak", directoryInfo.FullName + @"\TestChild.csv");

        dc = new DC(csvConfig);
        Assert.AreEqual(expectedTestParent0, dc.TestParents[0].ToString());
        Assert.AreEqual(expectedTestParent1, dc.TestParents[1].ToString());
        Assert.AreEqual(expectedTestChild0, dc.TestChildren[0].ToString());
        Assert.AreEqual(expectedTestChild2, dc.TestChildren[2].ToString());
      } finally {
        DC.Data?.Dispose();
      }

    }


    private static void reportException(Exception obj) {
      System.Diagnostics.Debug.WriteLine(obj);
      System.Diagnostics.Debugger.Break();
      Assert.Fail();
    }
  }
}
