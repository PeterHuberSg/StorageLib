using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageLib;
using System;
using System.IO;
using TestContext;

namespace StorageTest {


  [TestClass()]
  public class DataTypeSampleTest {


    [TestMethod()]
    public void TestDataTypeSample() {
      try {
        var directoryInfo = new DirectoryInfo("TestCsv");
        if (directoryInfo.Exists) {
          directoryInfo.Delete(recursive: true);
          directoryInfo.Refresh();
        }

        directoryInfo.Create();

        var csvConfig = new CsvConfig(directoryInfo.FullName, reportException: reportException);
        _ = new DC(csvConfig);
        var now = DateTime.Now;
        var nowDate = now.Date;
        var nowMinute = nowDate.AddMinutes(1);
        var nowSecond = nowDate.AddSeconds(1);
        var nowTimeSpan = now - now.Date;
        var nowTime = TimeSpan.FromSeconds(Math.Floor(nowTimeSpan.TotalSeconds));
        var d7 = 1.2345678m;
        var d5 = 1.23456m;
        var d4 = 1.2345m;
        var d2 = 1.23m;
        var aLong = 123456789012;
        var oldDts = new DataTypeSample(nowDate, nowDate, nowTime, nowTime, nowMinute, nowMinute, nowSecond, nowSecond,
          now, now, nowTimeSpan, nowTimeSpan, d7, d7, d2, d2, d4, d4, d5, d5, true, true, 123, 123, aLong, aLong, 'a', 'a',
          "abc", "abc", SampleStateEnum.None, SampleStateEnum.Some, isStoring: true);
        DC.DisposeData();

        _ = new DC(csvConfig);
        var newDts = DC.Data.DataTypeSamples[0];
        assert(oldDts, newDts);
        oldDts = new DataTypeSample(nowDate, null, nowTime, null, nowMinute, null, nowSecond, null,
          now, null, nowTimeSpan, null, d7, null, d2, null, d4, null, d5, null, true, null, 123, null, aLong, null, 'a', null,
          "abc", null, SampleStateEnum.None, SampleStateEnum.Some, isStoring: true);
        DC.DisposeData();

        _ = new DC(csvConfig);
        newDts = DC.Data.DataTypeSamples[1];
        assert(oldDts, newDts);
        oldDts = new DataTypeSample(DateTime.MinValue.Date, DateTime.MaxValue.Date, TimeSpan.FromTicks(0), new TimeSpan(0, 23, 59, 59, 0),
          DateTime.MinValue.Date.AddMinutes(1), DateTime.MaxValue.Date.AddMinutes(-1),
          DateTime.MinValue.Date.AddSeconds(1), DateTime.MaxValue.Date.AddSeconds(-1),
          DateTime.MinValue, DateTime.MaxValue, TimeSpan.MinValue, TimeSpan.MaxValue, 
          decimal.MinValue, decimal.MaxValue, -123456789012.12m, 123456789012.12m, -123456789012.1234m, 123456789012.1234m,
          -123456789012.12345m, 123456789012.12345m, false, true, int.MinValue, int.MaxValue, long.MinValue, long.MaxValue, 
          char.MinValue, char.MaxValue, "", new string('a', 1000), (SampleStateEnum)int.MinValue, (SampleStateEnum)int.MaxValue, isStoring: true);
        DC.DisposeData();

        _ = new DC(csvConfig);
        newDts = DC.Data.DataTypeSamples[2];
        assert(oldDts, newDts);

      } finally {
        DC.DisposeData();
      }
    }


    private static void assert(DataTypeSample oldDts, DataTypeSample newDts) {
      Assert.AreEqual(oldDts.ADate, newDts.ADate);
      Assert.AreEqual(oldDts.ANullableDate, newDts.ANullableDate);
      Assert.AreEqual(oldDts.ANullableDate, newDts.ANullableDate);
      Assert.AreEqual(oldDts.ANullableTime, newDts.ANullableTime);
      Assert.AreEqual(oldDts.ADateMinutes, newDts.ADateMinutes);
      Assert.AreEqual(oldDts.ANullableDateMinutes, newDts.ANullableDateMinutes);
      Assert.AreEqual(oldDts.ADateSeconds, newDts.ADateSeconds);
      Assert.AreEqual(oldDts.ANullableDateSeconds, newDts.ANullableDateSeconds);
      Assert.AreEqual(oldDts.ADateTime, newDts.ADateTime);
      Assert.AreEqual(oldDts.ANullableDateTime, newDts.ANullableDateTime);
      Assert.AreEqual(oldDts.ATimeSpan, newDts.ATimeSpan);
      Assert.AreEqual(oldDts.ANullableTimeSpan, newDts.ANullableTimeSpan);
      Assert.AreEqual(oldDts.ADecimal, newDts.ADecimal);
      Assert.AreEqual(oldDts.ANullableDecimal, newDts.ANullableDecimal);
      Assert.AreEqual(oldDts.ADecimal2, newDts.ADecimal2);
      Assert.AreEqual(oldDts.ANullableDecimal2, newDts.ANullableDecimal2);
      Assert.AreEqual(oldDts.ADecimal4, newDts.ADecimal4);
      Assert.AreEqual(oldDts.ANullableDecimal4, newDts.ANullableDecimal4);
      Assert.AreEqual(oldDts.ADecimal5, newDts.ADecimal5);
      Assert.AreEqual(oldDts.ANullableDecimal5, newDts.ANullableDecimal5);
      Assert.AreEqual(oldDts.ABool, newDts.ABool);
      Assert.AreEqual(oldDts.ANullableBool, newDts.ANullableBool);
      Assert.AreEqual(oldDts.AInt, newDts.AInt);
      Assert.AreEqual(oldDts.ANullableInt, newDts.ANullableInt);
      Assert.AreEqual(oldDts.ALong, newDts.ALong);
      Assert.AreEqual(oldDts.ANullableLong, newDts.ANullableLong);
      Assert.AreEqual(oldDts.AChar, newDts.AChar);
      Assert.AreEqual(oldDts.ANullableChar, newDts.ANullableChar);
      Assert.AreEqual(oldDts.AString, newDts.AString);
      Assert.AreEqual(oldDts.ANullableString, newDts.ANullableString);
      Assert.AreEqual(oldDts.AEnum, newDts.AEnum);
    }


    private void reportException(Exception ex) {
      Console.WriteLine(ex.ToString());
      System.Diagnostics.Debugger.Break();
      Assert.Fail();
    }
  }
}