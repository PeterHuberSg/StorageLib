using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageLib;
using TestContext;


namespace StorageTest {


  [TestClass]
  public class SampleTest {


    class expectedDataClass {
      public readonly Dictionary<int, string> Masters = new Dictionary<int, string>();
      public readonly Dictionary<int, string> Samples = new Dictionary<int, string>();
      public readonly Dictionary<int, string> Details = new Dictionary<int, string>();
    }
    expectedDataClass? expectedData;


    CsvConfig? csvConfig;


    [TestMethod]
    public void TestSample() {
      var directoryInfo = new DirectoryInfo("TestCsv");

      for (int configurationIndex = 0; configurationIndex < 2; configurationIndex++) {
        switch (configurationIndex) {
        case 0: csvConfig = null; break;
        case 1: csvConfig = new CsvConfig(directoryInfo.FullName, reportException: reportException); break;
        }
        try {
          directoryInfo.Refresh();
          if (directoryInfo.Exists) {
            directoryInfo.Delete(recursive: true);
            directoryInfo.Refresh();
          }

          directoryInfo.Create();

          expectedData = new expectedDataClass();
          initDL();
          assertDL();

          addMaster(0);
          addMaster(1);
          addMaster(2);
          addMaster(3);
          addSample(0, null);
          addSample(1, 1);
          addSample(2, 2);
          addDetail(0, 1);
          addDetail(1, 1);
          addDetail(2, 2);

          updateMaster(1, "a");
          //todo: What should happen if the child has two parent property for the same parent. Presently, updateSample() doesn't work
          //updateSample(1, "b", DC.Data.SampleMasters[0]);
          //updateSample(1, "c", null);
          //updateSample(1, "d", DC.Data.SampleMasters[1]);
          //updateDetail(1, "a");

          //removeDetail(2); need to find out why this is not working. Maybe because some code above got commented out ?
          //removeSample(2);
          //removeMaster(2);
          //removeMaster(1);
          //showStructure()
        } finally {
          DC.DisposeData();
        }
      }
    }


    private void initDL() {
      new DC(csvConfig);
    }


    private void reportException(Exception obj) {
      System.Diagnostics.Debug.WriteLine(obj);
      System.Diagnostics.Debugger.Break();
      Assert.Fail();
    }


    private void addMaster(int key) {
      var masterText = "Master" + key;
      expectedData!.Masters.Add(key, masterText);
      var master = new SampleMaster(masterText, isStoring: false);
      master.Store();
      assertData();
    }


    private void updateMaster(int masterKey, string text) {
      var masterText = "Master" + masterKey + text;
      expectedData!.Masters.Remove(masterKey);
      expectedData!.Masters.Add(masterKey, masterText);
      var master = DC.Data.SampleMasters[masterKey];
      master.Update(masterText, 1);
      foreach (var sample in master.SampleX) {
        expectedData!.Samples.Remove(sample.Key);
        expectedData!.Samples.Add(sample.Key, sample.ToString());
      }
      assertData();
    }


    private void addSample(int sampleKey, int? masterKey) {
      var sampleText = "Sample" + sampleKey;
      Sample sample;
      if (masterKey is null) {
        sample = new Sample(
          text: sampleText,
          flag: false,
          number: sampleKey,
          amount: sampleKey,
          amount4: sampleKey==0 ? 0 : sampleKey * 0.0001m,
          amount5: sampleKey==0 ? (decimal?)null : sampleKey * 0.00001m,
          preciseDecimal: sampleKey/1000000m,
          sampleState: SampleStateEnum.None,
          dateOnly: DateTime.Now.Date.AddDays(-sampleKey),
          timeOnly: TimeSpan.FromHours(sampleKey),
          dateTimeTicks: DateTime.Now.Date.AddDays(-sampleKey) + TimeSpan.FromTicks(sampleKey),
          dateTimeMinute: DateTime.Now.Date.AddDays(-sampleKey) + TimeSpan.FromMinutes(sampleKey),
          dateTimeSecond: DateTime.Now.Date.AddDays(-sampleKey) + TimeSpan.FromSeconds(sampleKey),
          oneMaster: null,
          otherMaster: null,
          optional: null,
          isStoring: false); ; ;
      } else {
        sample = new Sample(
          text: sampleText,
          flag: true,
          number: sampleKey,
          amount: sampleKey,
          amount4: sampleKey==0 ? 0 : sampleKey * 0.0001m,
          amount5: sampleKey==0 ? (decimal?)null : sampleKey * 0.00001m,
          preciseDecimal: sampleKey/1000000m,
          sampleState: SampleStateEnum.Some,
          dateOnly: DateTime.Now.Date.AddDays(-sampleKey),
          timeOnly: TimeSpan.FromHours(sampleKey),
          dateTimeTicks: DateTime.Now.Date.AddDays(-sampleKey) + TimeSpan.FromTicks(sampleKey),
          dateTimeMinute: DateTime.Now.Date.AddDays(-sampleKey) + TimeSpan.FromMinutes(sampleKey),
          dateTimeSecond: DateTime.Now.Date.AddDays(-sampleKey) + TimeSpan.FromSeconds(sampleKey),
          oneMaster: DC.Data.SampleMasters[masterKey.Value], 
          otherMaster: DC.Data.SampleMasters[masterKey.Value + 1],
          "option" + sampleKey,
          isStoring: false);
      }
      sample.Store();
      expectedData!.Samples.Add(sampleKey, sample.ToString());
      assertData();
    }


    private void updateSample(int sampleKey, string text, SampleMaster? newSampleMaster) {
      var sampleText = "Sample" + sampleKey + text;
      Sample sample = DC.Data.SampleX[sampleKey];
      sample.Update(
        text: sampleText,
        flag: sample.Flag,
        number: sample.Number,
        amount: sample.Amount,
        amount4: sampleKey==0 ? 0 : sampleKey * 0.0001m,
        amount5: sampleKey==0 ? (decimal?)null : sampleKey * 0.00001m,
        preciseDecimal: sample.PreciseDecimal,
        sampleState: sample.SampleState,
        dateOnly: sample.DateOnly,
        timeOnly: sample.TimeOnly,
        dateTimeTicks: sample.DateTimeTicks,
        dateTimeMinute: sample.DateTimeMinute,
        dateTimeSecond: sample.DateTimeSecond,
        oneMaster: newSampleMaster,
        otherMaster: newSampleMaster,
        optional: sample.Optional);
      expectedData!.Samples.Remove(sampleKey);
      expectedData!.Samples.Add(sampleKey, sample.ToString());
      assertData();
    }


    private void removeSample(int sampleKey) {
      expectedData!.Samples.Remove(sampleKey);
      var sample = DC.Data.SampleX[sampleKey];
      sample.Release();
      //assertData(); doesn't work when data context gets recreated. Parent lost not stored child
      assertDL();
    }


    private void addDetail(int detailKey, int sampleKey) {
      var detailText = "Detail" + sampleKey + '.' + detailKey;
      expectedData!.Details.Add(detailKey, detailText);
      var sample = DC.Data.SampleX[sampleKey];
      var detail = new SampleDetail(detailText, sample, false);
      detail.Store();
      expectedData!.Samples.Remove(sampleKey);
      expectedData!.Samples.Add(sampleKey, sample.ToString());
      assertData();
    }


    private void updateDetail(int key, string text) {
      var detailText = "Detail" + key + text;
      expectedData!.Details.Remove(key);
      expectedData!.Details.Add(key, detailText);
      var detail = DC.Data.SampleDetails[key];
      detail.Update(detailText, detail.Sample);
      assertData();
    }


    private void removeDetail(int detailKey) {
      expectedData!.Details.Remove(detailKey);
      var detail = DC.Data.SampleDetails[detailKey];
      detail.Release();
      var sampleKey = detail.Sample.Key;
      expectedData!.Samples.Remove(sampleKey);
      expectedData!.Samples.Add(sampleKey, detail.Sample.ToString());
      assertData();
    }


    private void assertData() {
      assertDL();

      if (csvConfig is null) return;

      DC.DisposeData();
      initDL();
      assertDL();
    }


    private void assertDL() {
      Assert.AreEqual(expectedData!.Masters.Count, DC.Data.SampleMasters.Count);
      foreach (var master in expectedData!.Masters) {
        var dlMaster = DC.Data.SampleMasters[master.Key];
        Assert.AreEqual(master.Value, dlMaster.Text);
      }

      Assert.AreEqual(expectedData!.Samples.Count, DC.Data.SampleX.Count);
      foreach (var sample in expectedData!.Samples) {
        var dlSample = DC.Data.SampleX[sample.Key];
        Assert.AreEqual(sample.Value, dlSample.ToString());
        //assertMaster(dlSample.OneMaster, dlSample);
        //assertMaster(dlSample.OtherMaster, dlSample);
      }

      Assert.AreEqual(expectedData!.Details.Count, DC.Data.SampleDetails.Count);
      foreach (var detail in expectedData!.Details) {
        var dlDetail = DC.Data.SampleDetails[detail.Key];
        Assert.AreEqual(detail.Value, dlDetail.Text);
        var isFound = false;
        foreach (var sampleDetail in dlDetail.Sample.SampleDetails) {
          if (sampleDetail.Key==dlDetail.Key) {
            isFound = true;
            break;
          }
        }
        Assert.IsTrue(isFound);
      }

      var mastersFromSamples = new Dictionary<int, HashSet<Sample>>();
      foreach (var sample in DC.Data.SampleX.Values) {
        addSampleToMaster(sample, sample.OneMaster, mastersFromSamples);
        addSampleToMaster(sample, sample.OtherMaster, mastersFromSamples);
      }
      foreach (var master in DC.Data.SampleMasters.Values) {
        if (mastersFromSamples.TryGetValue(master.Key, out var samplesHashSet)) {
          Assert.AreEqual(samplesHashSet.Count, master.SampleX.Count);
          foreach (var sample in samplesHashSet) {
            Assert.IsTrue(master.SampleX.Contains(sample));
          }
        } else {
          Assert.AreEqual(0, master.SampleX.Count);
          //showStructure();
        }

      }
    }


    private void addSampleToMaster(Sample sample, SampleMaster? master, Dictionary<int, HashSet<Sample>> mastersFromSamples) {
      if (master is null) return;

      if (!mastersFromSamples.TryGetValue(master.Key, out var samplesHashSet)) {
        samplesHashSet = new HashSet<Sample>();
        mastersFromSamples.Add(master.Key, samplesHashSet);
      }
      samplesHashSet.Add(sample);
    }
  }
}
