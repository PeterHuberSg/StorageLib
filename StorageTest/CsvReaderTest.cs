using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageLib;


namespace StorageTest {


  [TestClass()]
  public class CsvReaderTest {


    [TestMethod()]
    public void TestCsvReader() {
      var directoryInfo = new DirectoryInfo("TestCsv");
      try {
        if (directoryInfo.Exists) {
          directoryInfo.Delete(recursive: true);
          directoryInfo.Refresh();
        }

        directoryInfo.Create();
        directoryInfo.Refresh();

        var csvConfig = new CsvConfig(directoryInfo.FullName, reportException: reportException);
        var fileName = csvConfig.DirectoryPath + @"\TestCsvReader.csv";

        var expectedInts = new List<int>();
        var expectedNullInts = new List<int?>();
        var expectedLongs = new List<long>();
        var expectedDecimals = new List<decimal?>();
        var expectedDecimalNulls = new List<decimal?>();
        using (var fileStream = new FileStream(fileName, FileMode.Create)) {
          using var streamWriter = new StreamWriter(fileStream);
          streamWriter.Write(csvConfig.LineCharAdd);
          streamWriter.Write("0" + csvConfig.Delimiter);
          streamWriter.Write("1" + csvConfig.Delimiter);
          streamWriter.WriteLine();

          streamWriter.Write(csvConfig.LineCharAdd);
          writeInt(streamWriter, int.MaxValue, expectedInts, csvConfig.Delimiter);
          writeInt(streamWriter, 1, expectedInts, csvConfig.Delimiter);
          writeInt(streamWriter, 0, expectedInts, csvConfig.Delimiter);
          writeInt(streamWriter, -1, expectedInts, csvConfig.Delimiter);
          writeInt(streamWriter, -10, expectedInts, csvConfig.Delimiter);
          writeInt(streamWriter, -111, expectedInts, csvConfig.Delimiter);
          writeInt(streamWriter, int.MinValue, expectedInts, csvConfig.Delimiter);
          streamWriter.WriteLine();

          streamWriter.Write(csvConfig.LineCharAdd);
          writeIntNull(streamWriter, null, expectedNullInts, csvConfig.Delimiter);
          writeIntNull(streamWriter, int.MaxValue, expectedNullInts, csvConfig.Delimiter);
          writeIntNull(streamWriter, 1, expectedNullInts, csvConfig.Delimiter);
          writeIntNull(streamWriter, 0, expectedNullInts, csvConfig.Delimiter);
          writeIntNull(streamWriter, -1, expectedNullInts, csvConfig.Delimiter);
          writeIntNull(streamWriter, -10, expectedNullInts, csvConfig.Delimiter);
          writeIntNull(streamWriter, -111, expectedNullInts, csvConfig.Delimiter);
          writeIntNull(streamWriter, int.MinValue, expectedNullInts, csvConfig.Delimiter);
          streamWriter.WriteLine();

          streamWriter.Write(csvConfig.LineCharAdd);
          writeLong(streamWriter, long.MaxValue, expectedLongs, csvConfig.Delimiter);
          writeLong(streamWriter, 1, expectedLongs, csvConfig.Delimiter);
          writeLong(streamWriter, 0, expectedLongs, csvConfig.Delimiter);
          writeLong(streamWriter, -1, expectedLongs, csvConfig.Delimiter);
          writeLong(streamWriter, -10, expectedLongs, csvConfig.Delimiter);
          writeLong(streamWriter, -111, expectedLongs, csvConfig.Delimiter);
          writeLong(streamWriter, long.MinValue, expectedLongs, csvConfig.Delimiter);
          streamWriter.WriteLine();

          streamWriter.Write(csvConfig.LineCharAdd);
          writeDecimal(streamWriter, decimal.MaxValue, expectedDecimals, csvConfig.Delimiter);
          writeDecimal(streamWriter, 1234567890.1234567890m, expectedDecimals, csvConfig.Delimiter);
          writeDecimal(streamWriter, 1234567890.12m, expectedDecimals, csvConfig.Delimiter);
          writeDecimal(streamWriter, decimal.One, expectedDecimals, csvConfig.Delimiter);
          writeDecimal(streamWriter, decimal.Zero, expectedDecimals, csvConfig.Delimiter);
          writeDecimal(streamWriter, decimal.MinusOne, expectedDecimals, csvConfig.Delimiter);
          writeDecimal(streamWriter, -1234567890.12m, expectedDecimals, csvConfig.Delimiter);
          writeDecimal(streamWriter, -1234567890.1234567890m, expectedDecimals, csvConfig.Delimiter);
          writeDecimal(streamWriter, decimal.MinValue, expectedDecimals, csvConfig.Delimiter);
          streamWriter.WriteLine();

          streamWriter.Write(csvConfig.LineCharAdd);
          writeDecimal(streamWriter, null, expectedDecimalNulls, csvConfig.Delimiter);
          writeDecimal(streamWriter, decimal.MaxValue, expectedDecimalNulls, csvConfig.Delimiter);
          writeDecimal(streamWriter, 1234567890.1234567890m, expectedDecimalNulls, csvConfig.Delimiter);
          writeDecimal(streamWriter, 1234567890.12m, expectedDecimalNulls, csvConfig.Delimiter);
          writeDecimal(streamWriter, decimal.One, expectedDecimalNulls, csvConfig.Delimiter);
          writeDecimal(streamWriter, decimal.Zero, expectedDecimalNulls, csvConfig.Delimiter);
          writeDecimal(streamWriter, decimal.MinusOne, expectedDecimalNulls, csvConfig.Delimiter);
          writeDecimal(streamWriter, -1234567890.12m, expectedDecimalNulls, csvConfig.Delimiter);
          writeDecimal(streamWriter, -1234567890.1234567890m, expectedDecimalNulls, csvConfig.Delimiter);
          writeDecimal(streamWriter, decimal.MinValue, expectedDecimalNulls, csvConfig.Delimiter);
          streamWriter.WriteLine();

          streamWriter.Write(csvConfig.LineCharAdd);
          streamWriter.Write("a" + csvConfig.Delimiter);
          streamWriter.Write("Ä" + csvConfig.Delimiter);
          streamWriter.Write("☹" + csvConfig.Delimiter);
          streamWriter.WriteLine();

          streamWriter.Write(csvConfig.LineCharAdd);
          streamWriter.Write(csvConfig.Delimiter);
          streamWriter.Write("a" + csvConfig.Delimiter);
          streamWriter.Write("abc" + csvConfig.Delimiter);
          streamWriter.Write("Ä" + csvConfig.Delimiter);
          streamWriter.Write("aÄ" + csvConfig.Delimiter);
          streamWriter.Write("abcÄ ☹de" + csvConfig.Delimiter);
          streamWriter.WriteLine();

          streamWriter.Write(csvConfig.LineCharAdd);
          streamWriter.Write("31.12.9999" + csvConfig.Delimiter);
          streamWriter.Write("1.1.0001" + csvConfig.Delimiter);
          streamWriter.WriteLine();

          streamWriter.Write("0" + csvConfig.Delimiter);
          streamWriter.Write("0:0:1" + csvConfig.Delimiter);
          streamWriter.Write("0:1:1" + csvConfig.Delimiter);
          streamWriter.Write("1:1:1" + csvConfig.Delimiter);
          streamWriter.Write("23" + csvConfig.Delimiter);
          streamWriter.Write("23:59" + csvConfig.Delimiter);
          streamWriter.Write("23:59:59" + csvConfig.Delimiter);
          streamWriter.WriteLine();

          streamWriter.Write("3155378975999999999" + csvConfig.Delimiter); //DateTime.MaxValue
          streamWriter.Write("0" + csvConfig.Delimiter);                   //DateTime.MinValue
          streamWriter.Write("630822816000000000" + csvConfig.Delimiter);  //new DateTime(2000, 1, 1)
          streamWriter.Write("630822852610010000" + csvConfig.Delimiter);  //new DateTime(2000, 1, 1, 1, 1, 1, 1)
          streamWriter.Write("633978144000000000" + csvConfig.Delimiter);  //new DateTime(2009, 12, 31)
          streamWriter.Write("633979007999990000" + csvConfig.Delimiter);  //new DateTime(2009, 12, 31, 23, 59, 59, 999)
          streamWriter.WriteLine();

          streamWriter.Write(csvConfig.LineCharAdd);
          streamWriter.Write("31.12.9999 23:59:59" + csvConfig.Delimiter);
          streamWriter.Write("1.1.0001 0:0:1" + csvConfig.Delimiter);
          streamWriter.Write("31.1.2000 0:12" + csvConfig.Delimiter);
          streamWriter.Write("31.1.2001 12" + csvConfig.Delimiter);
          streamWriter.WriteLine();

          for (int i = -csvConfig.BufferSize; i < csvConfig.BufferSize; i++) {
            streamWriter.WriteLine(csvConfig.LineCharAdd + i.ToString() + csvConfig.Delimiter);
          }
        }

        string s;
        using (var fileStream = new FileStream(fileName, FileMode.Open)) {
          using var streamReader = new StreamReader(fileStream);
          s = streamReader.ReadToEnd();
        }

        int maxLineLenght = 150;
        using var csvReader = new CsvReader(fileName, csvConfig, maxLineLenght);
        //bool
        Assert.AreEqual(csvConfig.LineCharAdd, csvReader.ReadFirstLineChar());
        Assert.AreEqual(false, csvReader.ReadBool());
        Assert.AreEqual(true, csvReader.ReadBool());
        csvReader.ReadEndOfLine();
        Assert.IsFalse(csvReader.IsEof);

        //int
        Assert.AreEqual(csvConfig.LineCharAdd, csvReader.ReadFirstLineChar());
        foreach (var expectedInt in expectedInts) {
          Assert.AreEqual(expectedInt, csvReader.ReadInt());
          Assert.IsFalse(csvReader.IsEndOfFileReached());
          Assert.IsFalse(csvReader.IsEof);
        }
        csvReader.ReadEndOfLine();
        Assert.IsFalse(csvReader.IsEof);

        //int?
        Assert.AreEqual(csvConfig.LineCharAdd, csvReader.ReadFirstLineChar());
        foreach (var expectedInt in expectedNullInts) {
          var actualInt = csvReader.ReadIntNull();
          if (expectedInt.HasValue) {
            Assert.AreEqual(expectedInt, actualInt);
          } else {
            Assert.IsNull(actualInt);
          }
          Assert.IsFalse(csvReader.IsEndOfFileReached());
          Assert.IsFalse(csvReader.IsEof);
        }
        csvReader.ReadEndOfLine();
        Assert.IsFalse(csvReader.IsEof);

        //long
        Assert.AreEqual(csvConfig.LineCharAdd, csvReader.ReadFirstLineChar());
        foreach (var expectedLong in expectedLongs) {
          Assert.AreEqual(expectedLong, csvReader.ReadLong());
          Assert.IsFalse(csvReader.IsEndOfFileReached());
          Assert.IsFalse(csvReader.IsEof);
        }
        csvReader.ReadEndOfLine();
        Assert.IsFalse(csvReader.IsEof);

        //decimal
        Assert.AreEqual(csvConfig.LineCharAdd, csvReader.ReadFirstLineChar());
        foreach (var expectedDecimal in expectedDecimals) {
          Assert.AreEqual(expectedDecimal, csvReader.ReadDecimal());
          Assert.IsFalse(csvReader.IsEndOfFileReached());
          Assert.IsFalse(csvReader.IsEof);
        }
        csvReader.ReadEndOfLine();
        Assert.IsFalse(csvReader.IsEof);

        //decimal or null
        Assert.AreEqual(csvConfig.LineCharAdd, csvReader.ReadFirstLineChar());
        foreach (var expectedDecimal in expectedDecimalNulls) {
          Assert.AreEqual(expectedDecimal, csvReader.ReadDecimalNull());
          Assert.IsFalse(csvReader.IsEndOfFileReached());
          Assert.IsFalse(csvReader.IsEof);
        }
        csvReader.ReadEndOfLine();
        Assert.IsFalse(csvReader.IsEof);

        //char
        Assert.AreEqual(csvConfig.LineCharAdd, csvReader.ReadFirstLineChar());
        Assert.AreEqual('a', csvReader.ReadChar());
        Assert.AreEqual('Ä', csvReader.ReadChar());
        Assert.AreEqual('☹', csvReader.ReadChar());
        csvReader.ReadEndOfLine();
        Assert.IsFalse(csvReader.IsEof);

        //string?
        Assert.AreEqual(csvConfig.LineCharAdd, csvReader.ReadFirstLineChar());
        Assert.IsNull(csvReader.ReadStringNull());
        Assert.AreEqual("a", csvReader.ReadString());
        Assert.AreEqual("abc", csvReader.ReadString());
        Assert.AreEqual("Ä", csvReader.ReadString());
        Assert.AreEqual("aÄ", csvReader.ReadString());
        Assert.AreEqual("abcÄ ☹de", csvReader.ReadString());
        csvReader.ReadEndOfLine();

        //Date
        Assert.AreEqual(csvConfig.LineCharAdd, csvReader.ReadFirstLineChar());
        Assert.AreEqual(DateTime.MaxValue.Date, csvReader.ReadDate());
        Assert.AreEqual(DateTime.MinValue.Date, csvReader.ReadDate());
        csvReader.ReadEndOfLine();

        //Time
        Assert.AreEqual(new TimeSpan(0, 0, 0), csvReader.ReadTime());
        Assert.AreEqual(new TimeSpan(0, 0, 1), csvReader.ReadTime());
        Assert.AreEqual(new TimeSpan(0, 1, 1), csvReader.ReadTime());
        Assert.AreEqual(new TimeSpan(1, 1, 1), csvReader.ReadTime());
        Assert.AreEqual(new TimeSpan(23, 0, 0), csvReader.ReadTime());
        Assert.AreEqual(new TimeSpan(23, 59, 0), csvReader.ReadTime());
        Assert.AreEqual(new TimeSpan(23, 59, 59), csvReader.ReadTime());
        csvReader.ReadEndOfLine();

        //DateTime
        Assert.AreEqual(DateTime.MaxValue, csvReader.ReadDateTimeTicks());
        Assert.AreEqual(DateTime.MinValue, csvReader.ReadDateTimeTicks());
        Assert.AreEqual(new DateTime(2000, 1, 1), csvReader.ReadDateTimeTicks());
        Assert.AreEqual(new DateTime(2000, 1, 1, 1, 1, 1, 1), csvReader.ReadDateTimeTicks());
        Assert.AreEqual(new DateTime(2009, 12, 31), csvReader.ReadDateTimeTicks());
        Assert.AreEqual(new DateTime(2009, 12, 31, 23, 59, 59, 999), csvReader.ReadDateTimeTicks());
        csvReader.ReadEndOfLine();

        //DateSeconds
        Assert.AreEqual(csvConfig.LineCharAdd, csvReader.ReadFirstLineChar());
        Assert.AreEqual(new DateTime(9999, 12, 31, 23, 59, 59, 0), csvReader.ReadDateSeconds());
        Assert.AreEqual(new DateTime(1, 1, 1, 0, 0, 1, 0), csvReader.ReadDateSeconds());
        Assert.AreEqual(new DateTime(2000, 1, 31, 0, 12, 0, 0), csvReader.ReadDateSeconds());
        Assert.AreEqual(new DateTime(2001, 1, 31, 12, 0, 0, 0), csvReader.ReadDateSeconds());
        csvReader.ReadEndOfLine();

        //more than 1 buffer data
        for (int i = -csvConfig.BufferSize; i < csvConfig.BufferSize; i++) {
          Assert.AreEqual(csvConfig.LineCharAdd, csvReader.ReadFirstLineChar());
          Assert.AreEqual(i, csvReader.ReadInt());
          Assert.IsFalse(csvReader.IsEndOfFileReached());
          Assert.IsFalse(csvReader.IsEof);
          csvReader.ReadEndOfLine();
          if (i < csvConfig.BufferSize-1) {
            Assert.IsFalse(csvReader.IsEndOfFileReached());
            Assert.IsFalse(csvReader.IsEof);
          } else {
            Assert.IsTrue(csvReader.IsEndOfFileReached());
            Assert.IsTrue(csvReader.IsEof);
          }
        }
      } finally {
        directoryInfo.Delete(recursive: true);
      }
    }


    private void reportException(Exception obj) {
      System.Diagnostics.Debug.WriteLine(obj);
      System.Diagnostics.Debugger.Break();
      Assert.Fail();
    }


    private static void writeInt(StreamWriter streamWriter, int i, List<int> expectedInts, char delimiter) {
      streamWriter.Write(i.ToString());
      streamWriter.Write(delimiter);
      expectedInts.Add(i);
    }


    private static void writeIntNull(StreamWriter streamWriter, int? i, List<int?> expectedInts, char delimiter) {
      if (i.HasValue) {
        streamWriter.Write(i.ToString());
      }
      streamWriter.Write(delimiter);
      expectedInts.Add(i);
    }


    private static void writeLong(StreamWriter streamWriter, long l, List<long> expectedLongs, char delimiter) {
      streamWriter.Write(l.ToString());
      streamWriter.Write(delimiter);
      expectedLongs.Add(l);
    }


    private static void writeDecimal(StreamWriter streamWriter, decimal? d, List<decimal?> expectedDecimals, char delimiter) {
      if (d!=null) {
        streamWriter.Write(d.ToString());
      }
      streamWriter.Write(delimiter);
      expectedDecimals.Add(d);
    }


    #region 
    [TestMethod()]
    public void TestCsvReaderLine() {
      var directoryInfo = new DirectoryInfo("TestCsv");
      try {
        if (directoryInfo.Exists) {
          directoryInfo.Delete(recursive: true);
          directoryInfo.Refresh();
        }

        directoryInfo.Create();
        directoryInfo.Refresh();

        var csvConfig = new CsvConfig(directoryInfo.FullName, reportException: reportException);
        var fileName = csvConfig.DirectoryPath + @"\TestCsvReaderLine.csv";

        using (var fileStream = new FileStream(fileName, FileMode.Create)) {
          using var streamWriter = new StreamWriter(fileStream);
          streamWriter.WriteLine();
          streamWriter.WriteLine("a");
          streamWriter.WriteLine("abc" + csvConfig.Delimiter);
          streamWriter.WriteLine("Ä");
          streamWriter.WriteLine("aÄ" + csvConfig.Delimiter);
          streamWriter.WriteLine("abcÄ ☹de" + csvConfig.Delimiter);
        }

        int maxLineLenght = 6;
        using var csvReader = new CsvReader(fileName, csvConfig, maxLineLenght);
        Assert.AreEqual("", csvReader.ReadLine());
        Assert.AreEqual("a", csvReader.ReadLine());
        Assert.AreEqual("abc" + csvConfig.Delimiter, csvReader.ReadLine());
        Assert.AreEqual("Ä", csvReader.ReadLine());
        Assert.AreEqual("aÄ" + csvConfig.Delimiter, csvReader.ReadLine());
        Assert.AreEqual("abcÄ ☹de" + csvConfig.Delimiter, csvReader.ReadLine());
        Assert.IsTrue(csvReader.IsEndOfFileReached());
        Assert.IsTrue(csvReader.IsEof);
      } finally {
        directoryInfo.Delete(recursive: true);
      }
    }

    #endregion

  }
}