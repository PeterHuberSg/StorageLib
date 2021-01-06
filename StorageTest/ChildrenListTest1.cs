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
//  public class ChildrenListTest1 {

//    const int maxInstanceCount = 2;
//#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor.
//    CsvConfig csvConfig;
//    ListParentCreNullRon[] pCreNullRons;
//    ListParentCreNullNRo[] pCreNullNRos;
//    ListParentCreNNulRon[] pCreNNulRons;
//    ListParentCreNNulNRo[] pCreNNulNRos;
//    ListParentUpdNullRon[] pUpdNullRons;
//    ListParentUpdNullNRo[] pUpdNullNRos;
//    ListParentUpdNNulRon[] pUpdNNulRons;
//    ListParentUpdNNulNRo[] pUpdNNulNRos;
//    ListParentDelNullRon[] pDelNullRons;
//    ListParentDelNullNRo[] pDelNullNRos;
//    ListParentDelNNulRon[] pDelNNulRons;
//    ListParentDelNNulNRo[] pDelNNulNRos;
//    ListParentDupNullRon[] pDupNullRons;
//    ListParentDupNullNRo[] pDupNullNRos;
//    ListParentDupNNulRon[] pDupNNulRons;
//    ListParentDupNNulNRo[] pDupNNulNRos;

//    ListParentCreNullRonRaw[] pCreNullRonRaws;
//    ListParentCreNullNRoRaw[] pCreNullNRoRaws;
//    ListParentCreNNulRonRaw[] pCreNNulRonRaws;
//    ListParentCreNNulNRoRaw[] pCreNNulNRoRaws;
//    ListParentUpdNullRonRaw[] pUpdNullRonRaws;
//    ListParentUpdNullNRoRaw[] pUpdNullNRoRaws;
//    ListParentUpdNNulRonRaw[] pUpdNNulRonRaws;
//    ListParentUpdNNulNRoRaw[] pUpdNNulNRoRaws;
//    ListParentDelNullRonRaw[] pDelNullRonRaws;
//    ListParentDelNullNRoRaw[] pDelNullNRoRaws;
//    ListParentDelNNulRonRaw[] pDelNNulRonRaws;
//    ListParentDelNNulNRoRaw[] pDelNNulNRoRaws;
//    ListParentDupNullRonRaw[] pDupNullRonRaws;
//    ListParentDupNullNRoRaw[] pDupNullNRoRaws;
//    ListParentDupNNulRonRaw[] pDupNNulRonRaws;
//    ListParentDupNNulNRoRaw[] pDupNNulNRoRaws;

//    ListChildCre[] cCres;
//    ListChildUpd[] cUpds;
//    ListChildDel[] cDels;
//    ListChildDup[] cDups;

//    ListChildCreRaw[] cCreRaws;
//    ListChildUpdRaw[] cUpdRaws;
//    ListChildDelRaw[] cDelRaws;
//    ListChildDupRaw[] cDupRaws;
//#pragma warning restore CS8618


//    const int maxInstances = 3;
//    private void intiTestData() {
//      pCreNullRons = new ListParentCreNullRon[maxInstances];
//      pCreNullNRos = new ListParentCreNullNRo[maxInstances];
//      pCreNNulRons = new ListParentCreNNulRon[maxInstances];
//      pCreNNulNRos = new ListParentCreNNulNRo[maxInstances];
//      pUpdNullRons = new ListParentUpdNullRon[maxInstances];
//      pUpdNullNRos = new ListParentUpdNullNRo[maxInstances];
//      pUpdNNulRons = new ListParentUpdNNulRon[maxInstances];
//      pUpdNNulNRos = new ListParentUpdNNulNRo[maxInstances];
//      pDelNullRons = new ListParentDelNullRon[maxInstances];
//      pDelNullNRos = new ListParentDelNullNRo[maxInstances];
//      pDelNNulRons = new ListParentDelNNulRon[maxInstances];
//      pDelNNulNRos = new ListParentDelNNulNRo[maxInstances];
//      pDupNullRons = new ListParentDupNullRon[maxInstances];
//      pDupNullNRos = new ListParentDupNullNRo[maxInstances];
//      pDupNNulRons = new ListParentDupNNulRon[maxInstances];
//      pDupNNulNRos = new ListParentDupNNulNRo[maxInstances];

//      pCreNullRonRaws = new ListParentCreNullRonRaw[maxInstances];
//      pCreNullNRoRaws = new ListParentCreNullNRoRaw[maxInstances];
//      pCreNNulRonRaws = new ListParentCreNNulRonRaw[maxInstances];
//      pCreNNulNRoRaws = new ListParentCreNNulNRoRaw[maxInstances];
//      pUpdNullRonRaws = new ListParentUpdNullRonRaw[maxInstances];
//      pUpdNullNRoRaws = new ListParentUpdNullNRoRaw[maxInstances];
//      pUpdNNulRonRaws = new ListParentUpdNNulRonRaw[maxInstances];
//      pUpdNNulNRoRaws = new ListParentUpdNNulNRoRaw[maxInstances];
//      pDelNullRonRaws = new ListParentDelNullRonRaw[maxInstances];
//      pDelNullNRoRaws = new ListParentDelNullNRoRaw[maxInstances];
//      pDelNNulRonRaws = new ListParentDelNNulRonRaw[maxInstances];
//      pDelNNulNRoRaws = new ListParentDelNNulNRoRaw[maxInstances];
//      pDupNullRonRaws = new ListParentDupNullRonRaw[maxInstances];
//      pDupNullNRoRaws = new ListParentDupNullNRoRaw[maxInstances];
//      pDupNNulRonRaws = new ListParentDupNNulRonRaw[maxInstances];
//      pDupNNulNRoRaws = new ListParentDupNNulNRoRaw[maxInstances];

//      cCres = new ListChildCre[maxInstances];
//      cUpds = new ListChildUpd[maxInstances];
//      cDels = new ListChildDel[maxInstances];
//      cDups = new ListChildDup[maxInstances];

//      cCreRaws = new ListChildCreRaw[maxInstances];
//      cUpdRaws = new ListChildUpdRaw[maxInstances];
//      cDelRaws = new ListChildDelRaw[maxInstances];
//      cDupRaws = new ListChildDupRaw[maxInstances];

//      for (int instanceIndex = 0; instanceIndex < maxInstances; instanceIndex++) {
//        //var instanceNoString = instanceIndex.ToString();
//        //pCreNullRons[instanceIndex] = new ListParentCreNullRon("pCreNullRon" + instanceNoString);
//        //pCreNullNRos[instanceIndex] = new ListParentCreNullNRo("pCreNullNRo" + instanceNoString);
//        //pCreNNulRons[instanceIndex] = new ListParentCreNNulRon("pCreNNulRon" + instanceNoString);
//        //pCreNNulNRos[instanceIndex] = new ListParentCreNNulNRo("pCreNNulNRo" + instanceNoString);
//        //pUpdNullRons[instanceIndex] = new ListParentUpdNullRon("pUpdNullRon" + instanceNoString);
//        //pUpdNullNRos[instanceIndex] = new ListParentUpdNullNRo("pUpdNullNRo" + instanceNoString);
//        //pUpdNNulRons[instanceIndex] = new ListParentUpdNNulRon("pUpdNNulRon" + instanceNoString);
//        //pUpdNNulNRos[instanceIndex] = new ListParentUpdNNulNRo("pUpdNNulNRo" + instanceNoString);
//        //pDelNullRons[instanceIndex] = new ListParentDelNullRon("pDelNullRon" + instanceNoString);
//        //pDelNullNRos[instanceIndex] = new ListParentDelNullNRo("pDelNullNRo" + instanceNoString);
//        //pDelNNulRons[instanceIndex] = new ListParentDelNNulRon("pDelNNulRon" + instanceNoString);
//        //pDelNNulNRos[instanceIndex] = new ListParentDelNNulNRo("pDelNNulNRo" + instanceNoString);
//        //pDupNullRons[instanceIndex] = new ListParentDupNullRon("pDupNullRon" + instanceNoString);
//        //pDupNullNRos[instanceIndex] = new ListParentDupNullNRo("pDupNullNR" + instanceNoString);
//        //pDupNNulRons[instanceIndex] = new ListParentDupNNulRon("pDupNNulRon" + instanceNoString);
//        //pDupNNulNRos[instanceIndex] = new ListParentDupNNulNRo("pDupNNulNRo" + instanceNoString);
//      }
//    }


//    [TestMethod]
//    public void TestChildrenList1() {
//      var directoryInfo = new DirectoryInfo("TestCsv");
//      if (directoryInfo.Exists) {
//        directoryInfo.Delete(recursive: true);
//        directoryInfo.Refresh();
//      }

//      directoryInfo.Create();

//      try {
//        csvConfig = new CsvConfig(directoryInfo.FullName, reportException: reportException);
//        _ = new DC(csvConfig);
//        intiTestData();
//        assertData();

//        createParents(1);
//        createChild(0);
//      } finally {
//        DC.DisposeData();
//      }
//    }


//    private void createParents(int instanceIndex) {
//      var instanceNoString = instanceIndex.ToString();
//      pCreNullRons[instanceIndex] = new ListParentCreNullRon("pCreNullRon" + instanceNoString);
//      pCreNullNRos[instanceIndex] = new ListParentCreNullNRo("pCreNullNRo" + instanceNoString);
//      pCreNNulRons[instanceIndex] = new ListParentCreNNulRon("pCreNNulRon" + instanceNoString);
//      pCreNNulNRos[instanceIndex] = new ListParentCreNNulNRo("pCreNNulNRo" + instanceNoString);
//      pUpdNullRons[instanceIndex] = new ListParentUpdNullRon("pUpdNullRon" + instanceNoString);
//      pUpdNullNRos[instanceIndex] = new ListParentUpdNullNRo("pUpdNullNRo" + instanceNoString);
//      pUpdNNulRons[instanceIndex] = new ListParentUpdNNulRon("pUpdNNulRon" + instanceNoString);
//      pUpdNNulNRos[instanceIndex] = new ListParentUpdNNulNRo("pUpdNNulNRo" + instanceNoString);
//      pDelNullRons[instanceIndex] = new ListParentDelNullRon("pDelNullRon" + instanceNoString);
//      pDelNullNRos[instanceIndex] = new ListParentDelNullNRo("pDelNullNRo" + instanceNoString);
//      pDelNNulRons[instanceIndex] = new ListParentDelNNulRon("pDelNNulRon" + instanceNoString);
//      pDelNNulNRos[instanceIndex] = new ListParentDelNNulNRo("pDelNNulNRo" + instanceNoString);
//      pDupNullRons[instanceIndex] = new ListParentDupNullRon("pDupNullRon" + instanceNoString);
//      pDupNullNRos[instanceIndex] = new ListParentDupNullNRo("pDupNullNR" + instanceNoString);
//      pDupNNulRons[instanceIndex] = new ListParentDupNNulRon("pDupNNulRon" + instanceNoString);
//      pDupNNulNRos[instanceIndex] = new ListParentDupNNulNRo("pDupNNulNRo" + instanceNoString);
//    }

//    private void assertData() {
//      assertAreEqual(DC.Data.ListParentCreNullRons, pCreNullRonRaws);
//    }

//    private void assertAreEqual(DataStore<ListParentCreNullRon> listParentCreNullRons, ListParentCreNullRonRaw[] pCreNullRonRaws) {
//      Assert.AreEqual(listParentCreNullRons.Count, pCreNullRonRaws.Length);
//      for (int i = 0; i < listParentCreNullRons.Count; i++) {
//        var actual = listParentCreNullRons[i];
//        var expected = pCreNullRonRaws[i];
//        Assert.AreEqual(expected.Text, actual.Text);
//        Assert.AreEqual(expected.Text, actual.Text);

//      }
//    }

//    private void reportException(Exception obj) {
//      System.Diagnostics.Debug.WriteLine(obj);
//      System.Diagnostics.Debugger.Break();
//      Assert.Fail();
//    }


//    private void createChild(int key) {
//      var keyString = key.ToString();
//      DC.Data.StartTransaction();
//      cCres[key] = new ListChildCre("cCres " + keyString, 
//        pCreNullRons[key], pCreNullNRos[key], pCreNNulRons[key], pCreNNulNRos[key],
//        pUpdNullRons[key], pUpdNullNRos[key], pUpdNNulRons[key], pUpdNNulNRos[key],
//        pDelNullRons[key], pDelNullNRos[key], pDelNNulRons[key], pDelNNulNRos[key],
//        pDupNullRons[key], pDupNullNRos[key], pDupNNulRons[key], pDupNNulNRos[key]);
//      DC.Data.RollbackTransaction();
//      assertData();
//      DC.Data.StartTransaction();
//      cCres[key] = new ListChildCre("cCres " + keyString,
//        pCreNullRons[key], pCreNullNRos[key], pCreNNulRons[key], pCreNNulNRos[key],
//        pUpdNullRons[key], pUpdNullNRos[key], pUpdNNulRons[key], pUpdNNulNRos[key],
//        pDelNullRons[key], pDelNullNRos[key], pDelNNulRons[key], pDelNNulNRos[key],
//        pDupNullRons[key], pDupNullNRos[key], pDupNNulRons[key], pDupNNulNRos[key]);
//      cCreRaws[key] = new ListChildCreRaw(cCres[key]);
//      DC.Data.CommitTransaction();
//      DC.Data.CommitTransaction();
//      assertData();

//      disposeAndCreateNewDC();
//      assertData();


//      cCres[key].clone
//    }


//    private void disposeAndCreateNewDC() {
//      DC.DisposeData();
//      _ = new DC(csvConfig);
//    }
//  }
//}
