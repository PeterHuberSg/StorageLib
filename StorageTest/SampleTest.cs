using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageLib;
using TestContext;


namespace StorageTest {


  [TestClass]
  public class SampleTest {


    class expectedDataClass {
      //public readonly Dictionary<int, (string Text, List<int> Children)> Masters = new();
      //public readonly Dictionary<int, (string Text, List<int> Children, List<int> Parents)> Samples = new();
      //public readonly Dictionary<int, (string Text, List<int> Children)> Details = new();
      public readonly Dictionary<int, string> Masters = new();
      public readonly Dictionary<int, string> Samples = new();
      public readonly Dictionary<int, string> Details = new();
    }
    expectedDataClass? expectedData;


    class expectedStructureClass {
      public readonly Dictionary<int, List<int>/*Children*/> Masters = new();
      public readonly Dictionary<int, (List<int> Parents, List<int> Children)> Samples = new();
      public readonly Dictionary<int, int /*Parent*/> Details = new();
    }
    readonly expectedStructureClass expectedStructure = new expectedStructureClass();


    CsvConfig? csvConfig;
    BakCsvFileSwapper? bakCsvFileSwapper;


    [TestMethod]
    public void TestSample() {
      //toNiceString("M01c1,2c12,3c2 S01p12c01,2p23c2 D0p1,1p1,2p2");
      //updateStructure("M01c1,2c12,3c2 S01p12c01,2p23c2 D0p1,1p1,2p2");
      var directoryInfo = new DirectoryInfo("TestCsv");

      for (int configurationIndex = 0; configurationIndex < 2; configurationIndex++) {
        switch (configurationIndex) {
        case 0: 
          csvConfig = null;
          bakCsvFileSwapper = null;
          break;
        case 1: 
          csvConfig = new CsvConfig(directoryInfo.FullName, reportException: reportException);
          bakCsvFileSwapper = new BakCsvFileSwapper(csvConfig);
          break;
        }
        try {
          directoryInfo.Refresh();
          if (directoryInfo.Exists) {
            directoryInfo.Delete(recursive: true);
            directoryInfo.Refresh();
          }

          directoryInfo.Create();

          expectedData = new expectedDataClass();
          _ = new DC(csvConfig);
          assertDC("");

          int[] b = { 1 };
          addMaster(0, "M0");
          addMaster(1, "M01");
          addMaster(2, "M012");
          addMaster(3, "M0123");
          addSample(0, null, "M0123 S0");
          addSample(1, 1, "M01c1,2c1,3 S01p12");
          addSample(2, 2, "M01c1,2c12,3c2 S01p12,2p23");
          addDetail(0, 1, "M01c1,2c12,3c2 S01p12c0,2p23 D0p1");
          addDetail(1, 1, "M01c1,2c12,3c2 S01p12c01,2p23 D0p1,1p1");
          addDetail(2, 2, "M01c1,2c12,3c2 S01p12c01,2p23c2 D0p1,1p1,2p2");

          updateMaster(1, "a", "M01c1,2c12,3c2 S01p12c01,2p23c2 D0p1,1p1,2p2");
          updateSample(1, "b", DC.Data.SampleMasters[0], "M0c1,12c2,3c2 S01p0c01,2p23c2 D0p1,1p1,2p2");
          updateSample(1, "c", null, "M012c2,3c2 S01c01,2p23c2 D0p1,1p1,2p2");
          updateSample(1, "d", DC.Data.SampleMasters[1], "M01c1,2c2,3c2 S01p1c01,2p23c2 D0p1,1p1,2p2");
          updateDetail(1, "a", "M01c1,2c2,3c2 S01p1c01,2p23c2 D0p1,1p1,2p2");

          removeDetail(2, "M01c1,2c2,3c2 S01p1c01,2p23c- D0p1,1p1", "M01c1,2c2,3c2 S01p1c01,2p23 D0p1,1p1"); 
          removeSample(2, "M01c1,2c-,3c- S01p1c01 D0p1,1p1", "M01c1,23 S01p1c01 D0p1,1p1");
          //removeMaster(2, "M01c1,3 S01p1c01 D0p1,1p1");
        } finally {
          DC.DisposeData();
        }
      }
    }


    private void reportException(Exception obj) {
      System.Diagnostics.Debug.WriteLine(obj);
      System.Diagnostics.Debugger.Break();
      Assert.Fail();
    }


    private void addMaster(int key, string structure) {
      var masterText = "Master" + key;
      expectedData!.Masters.Add(key, masterText);
      var master = new SampleMaster(masterText, isStoring: false);
      master.Store();
      assertData(structure);
    }


    private void updateMaster(int masterKey, string text, string structure) {
      var masterText = "Master" + masterKey + text;
      expectedData!.Masters.Remove(masterKey);
      expectedData!.Masters.Add(masterKey, masterText);
      var master = DC.Data.SampleMasters[masterKey];
      master.Update(masterText, 1);
      foreach (var sample in master.SampleX) {
        expectedData!.Samples.Remove(sample.Key);
        expectedData!.Samples.Add(sample.Key, toString(sample));
      }
      assertData(structure);
    }


    private void removeMaster(int masterKey, string structure) {
      expectedData!.Masters.Remove(masterKey);
      var master = DC.Data.SampleX[masterKey];
      master.Release();
      assertData(structure);
    }


    private static string toString(Sample sample) {
      var sampleString = sample.ToString();
      var sampleDetailsIndex = sampleString.IndexOf("SampleDetails: ") + "SampleDetails: ".Length;
      sampleString = sampleString[..sampleDetailsIndex] + '?' + sampleString[(sampleDetailsIndex+1)..];
      return sampleString;
    }


    private void addSample(int sampleKey, int? masterKey, string structure) {
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
      expectedData!.Samples.Add(sampleKey, toString(sample));
      assertData(structure);
    }


    private void updateSample(int sampleKey, string text, SampleMaster? newSampleMaster, string structure) {
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
      expectedData!.Samples.Add(sampleKey, toString(sample));
      assertData(structure);
    }


    private void removeSample(int sampleKey, string structure0, string structure1) {
      expectedData!.Samples.Remove(sampleKey);
      var sample = DC.Data.SampleX[sampleKey];
      sample.Release();
      assertData(structure0, structure1);
    }


    private void addDetail(int detailKey, int sampleKey, string structure) {
      var detailText = "Detail" + sampleKey + '.' + detailKey;
      expectedData!.Details.Add(detailKey, detailText);
      var sample = DC.Data.SampleX[sampleKey];
      var detail = new SampleDetail(detailText, sample, false);
      detail.Store();
      expectedData!.Samples.Remove(sampleKey);
      expectedData!.Samples.Add(sampleKey, toString(sample));
      assertData(structure);
    }


    private void updateDetail(int key, string text, string structure) {
      var detailText = "Detail" + key + text;
      expectedData!.Details.Remove(key);
      expectedData!.Details.Add(key, detailText);
      var detail = DC.Data.SampleDetails[key];
      detail.Update(detailText, detail.Sample);
      assertData(structure);
    }


    private void removeDetail(int detailKey, string structure0, string? structure1) {
      expectedData!.Details.Remove(detailKey);
      var detail = DC.Data.SampleDetails[detailKey];
      detail.Release();
      var sampleKey = detail.Sample.Key;
      expectedData!.Samples.Remove(sampleKey);
      expectedData!.Samples.Add(sampleKey, toString(detail.Sample));
      assertData(structure0, structure1);
    }


    private void assertData(string structure0, string? structure1 = null) {
      assertDC(structure0);

      if (csvConfig is null) return;

      DC.DisposeData();

      if (structure1 is null) {
        structure1 = structure0;
      }

      if (bakCsvFileSwapper!.UseBackupFiles()) {
        _ = new DC(csvConfig);
        assertDC(structure1);
        DC.DisposeData();
        bakCsvFileSwapper.SwapBack();
      }

      _ = new DC(csvConfig);
      assertDC(structure1);
    }


    readonly StringBuilder sb = new();


    public string dataToNiceString() {
      /*

Master0
Master1 Sample1
Master2 Sample1 Sample2
Master3 Sample2
Sample0
Sample1 Master1 Master2 Detail0 Detail1
Sample2 Master2 Master3 Detail2
Detail0 Sample1
Detail1 Sample1
Detail2 Sample2*/
      sb.Clear();

      foreach (var master in DC.Data.SampleMasters) {
        sb.Append(Environment.NewLine + "Master" + master.Key);
        foreach (var sample in master.SampleX) {
          sb.Append(" Sample" + sample.Key);
        }
      }

      foreach (var sample in DC.Data.SampleX) {
        sb.Append(Environment.NewLine + "Sample" + sample.Key);
        int? oneMasterKey = null;
        if (sample.OneMaster is not null) {
          oneMasterKey = sample.OneMaster.Key;
          sb.Append(" Master" + sample.OneMaster.Key);
        }
        if (sample.OtherMaster is not null && oneMasterKey!=sample.OtherMaster.Key) {
          sb.Append(" Master" + sample.OtherMaster.Key);
        }
        foreach (var detail in sample.SampleDetails) {
          sb.Append(" Detail" + detail.Key);
        }
      }

      foreach (var detail in DC.Data.SampleDetails) {
        sb.Append(Environment.NewLine + "Detail" + detail.Key);
        sb.Append(" Sample" + detail.Sample.Key);
      }

      return sb.ToString();
    }


    enum sStateEnum {
      classType,

      Master,
      MasterChild,

      Sample,
      SampleParent,
      SampleChild,

      Detail,
      DetailParent,
    }


    private string toNiceString(string structure) {
      //M.....S.....D.....: Masters, Samples, Details
      //M0123: Masters 0,1,2,3
      //M01c1: Masters 0,1. Master 1 has children 1
      //M01c1,2c12: Masters 0,1,2. Master 1 has children 1, Master 2 has children 1,2
      //S01p12c01: Samples 0,1. Sample 1 has parents (Masters) 1,2 and children (details) 0,1
      //D0p1,1p1,2p2: Details 0,1,2. Detail 0 has parent1, Detail 1 has parent1 and Detail 2 has parent2

      //"M01c1,2c12,3c2 S01p12c01,2p23c2 D0p1,1p1,2p2"
      sb.Clear();
      var sState = sStateEnum.classType;

      foreach (var ch in structure) {
        switch (sState) {
        case sStateEnum.classType:
          sState = ch switch {
            'M' => sStateEnum.Master,
            'S' => sStateEnum.Sample,
            'D' => sStateEnum.Detail,
            _ => throw new NotSupportedException(sb.ToString()),
          };
          break;

        case sStateEnum.Master:
          if (ch>='0' && ch<='9') {
            sb.Append(Environment.NewLine + "Master" + ch);
          } else if (ch=='c') {
            sState = sStateEnum.MasterChild;
          } else if (ch==' ') {
            sState = sStateEnum.classType;
          } else throw new NotSupportedException(sb.ToString());
          break;

        case sStateEnum.MasterChild:
          if (ch>='0' && ch<='9') {
            sb.Append(" Sample" + ch);
          } else if (ch=='-') {
            sb.Append(" Sample-1");
          } else if (ch==',') {
            sState = sStateEnum.Master;
          } else if (ch==' ') {
            sState = sStateEnum.classType;
          } else throw new NotSupportedException(sb.ToString());
          break;

        case sStateEnum.Sample:
          if (ch>='0' && ch<='9') {
            sb.Append(Environment.NewLine + "Sample" + ch);
          } else if (ch=='c') {
            sState = sStateEnum.SampleChild;
          } else if (ch=='p') {
            sState = sStateEnum.SampleParent;
          } else if (ch==' ') {
            sState = sStateEnum.classType;
          } else throw new NotSupportedException(sb.ToString());
          break;

        case sStateEnum.SampleParent:
          if (ch>='0' && ch<='9') {
            sb.Append(" Master" + ch);
          } else if (ch==',') {
            sState = sStateEnum.Sample;
          } else if (ch=='c') {
            sState = sStateEnum.SampleChild;
          } else if (ch==' ') {
            sState = sStateEnum.classType;
          } else throw new NotSupportedException(sb.ToString());
          break;

        case sStateEnum.SampleChild:
          if (ch>='0' && ch<='9') {
            sb.Append(" Detail" + ch);
          } else if (ch=='-') {
            sb.Append(" Detail-1");
          } else if (ch==',') {
            sState = sStateEnum.Sample;
          } else if (ch=='p') {
            sState = sStateEnum.SampleParent;
          } else if (ch==' ') {
            sState = sStateEnum.classType;
          } else throw new NotSupportedException(sb.ToString());
          break;

        case sStateEnum.Detail:
          if (ch>='0' && ch<='9') {
            sb.Append(Environment.NewLine + "Detail" + ch);
          } else if (ch=='p') {
            sState = sStateEnum.DetailParent;
          } else if (ch==' ') {
            sState = sStateEnum.classType;
          } else throw new NotSupportedException(sb.ToString());
          break;

        case sStateEnum.DetailParent:
          if (ch>='0' && ch<='9') {
            sb.Append(" Sample" + ch);
          } else if (ch==',') {
            sState = sStateEnum.Detail;
          } else if (ch==' ') {
            sState = sStateEnum.classType;
          } else throw new NotSupportedException(sb.ToString());
          break;
        default:
          break;
        }
      }

      return sb.ToString();
    }


    private void assertDC(string structure) {
      var expectedStructure = toNiceString(structure);
      var actualStructure = dataToNiceString();
      if (expectedStructure!=actualStructure) {
        var max = Math.Min(expectedStructure.Length, actualStructure.Length);
        int charIndex;
        for (charIndex = 0; charIndex < max; charIndex++) {
          if (actualStructure[charIndex]!=expectedStructure[charIndex]) {
            break;
          }
        }
        var errorMessage = "Both:" + expectedStructure[0..charIndex] + Environment.NewLine + Environment.NewLine +
          "Expected:" + Environment.NewLine  + expectedStructure[charIndex..] + Environment.NewLine + Environment.NewLine +
          "Actual: "+ Environment.NewLine  + actualStructure[charIndex..];
        Assert.Fail(errorMessage);
      }

      Assert.AreEqual(expectedData!.Masters.Count, DC.Data.SampleMasters.Count);
      foreach (var master in expectedData!.Masters) {
        var dlMaster = DC.Data.SampleMasters[master.Key];
        Assert.AreEqual(master.Value, dlMaster.Text);
      }

      Assert.AreEqual(expectedData!.Samples.Count, DC.Data.SampleX.Count);
      foreach (var sample in expectedData!.Samples) {
        var dlSample = DC.Data.SampleX[sample.Key];
        Assert.AreEqual(sample.Value, toString(dlSample));
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
    }
  }
}
