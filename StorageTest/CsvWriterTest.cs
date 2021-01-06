using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageLib;


namespace StorageTest {


  [TestClass()]
  public class CsvWriterTest {


    [TestMethod()]
    public void TestCsvWriter() {
      var directoryInfo = new DirectoryInfo("TestCsv");
      try {
        if (directoryInfo.Exists) {
          directoryInfo.Delete(recursive: true);
          directoryInfo.Refresh();
        }

        directoryInfo.Create();
        directoryInfo.Refresh();

        var csvConfig = new CsvConfig(directoryInfo.FullName, reportException: reportException);
        var fileName = csvConfig.DirectoryPath + @"\TestCsvWriterInt.csv";
        using (var csvWriter = new CsvWriter(fileName, csvConfig, 250)) {
          csvWriter.WriteLine("Some header");

          csvWriter.WriteFirstLineChar(csvConfig.LineCharAdd);
          csvWriter.Write(false);
          csvWriter.Write(true);
          csvWriter.WriteEndOfLine();

          csvWriter.WriteFirstLineChar(csvConfig.LineCharAdd);
          csvWriter.Write(int.MaxValue);
          csvWriter.Write(1);
          csvWriter.Write(0);
          csvWriter.Write(-1);
          csvWriter.Write(int.MinValue);
          csvWriter.WriteEndOfLine();

          csvWriter.WriteFirstLineChar(csvConfig.LineCharAdd);
          csvWriter.Write((int?)null);
          csvWriter.Write((int?)int.MaxValue);
          csvWriter.Write((int?)1);
          csvWriter.Write((int?)0);
          csvWriter.Write((int?)-1);
          csvWriter.Write((int?)int.MinValue);
          csvWriter.WriteEndOfLine();

          csvWriter.WriteFirstLineChar(csvConfig.LineCharAdd);
          csvWriter.Write(long.MaxValue);
          csvWriter.Write(1L);
          csvWriter.Write(0L);
          csvWriter.Write(-1L);
          csvWriter.Write(long.MinValue);
          csvWriter.WriteEndOfLine();

          csvWriter.WriteFirstLineChar(csvConfig.LineCharAdd);
          csvWriter.Write(decimal.MaxValue, 0);
          csvWriter.Write(decimal.MaxValue);
          csvWriter.Write(1234567890.12345678m, 8);
          csvWriter.Write(30m, 2);
          csvWriter.Write(1.1m, 1);
          csvWriter.Write(1.1m, 0);
          csvWriter.Write(1m, 1);
          csvWriter.Write(decimal.One);
          csvWriter.Write(0.9m, 1);
          csvWriter.Write(0.9m, 0);
          csvWriter.Write(0.4m, 0);
          csvWriter.Write(decimal.Zero);
          csvWriter.Write(-0.4m, 0);
          csvWriter.Write(-0.9m, 0);
          csvWriter.Write(-0.9m, 3);
          csvWriter.Write(decimal.MinusOne);
          csvWriter.Write(-1m, 1);
          csvWriter.Write(-1.1m, 0);
          csvWriter.Write(-1.1m, 1);
          csvWriter.Write(-1234567890.12345678m, 8);
          csvWriter.Write(decimal.MinValue);
          csvWriter.Write(decimal.MinValue, 0);
          csvWriter.Write((decimal)Math.PI);
          csvWriter.WriteEndOfLine();

          csvWriter.WriteFirstLineChar(csvConfig.LineCharAdd);
          csvWriter.Write((decimal?)null, 0);
          csvWriter.Write((decimal?)decimal.MaxValue, 0);
          csvWriter.Write((decimal?)decimal.MaxValue);
          csvWriter.Write((decimal?)1234567890.12345678m, 8);
          csvWriter.Write((decimal?)30m, 2);
          csvWriter.Write((decimal?)1.1m, 1);
          csvWriter.Write((decimal?)1.1m, 0);
          csvWriter.Write((decimal?)1m, 1);
          csvWriter.Write((decimal?)decimal.One);
          csvWriter.Write((decimal?)0.9m, 1);
          csvWriter.Write((decimal?)0.9m, 0);
          csvWriter.Write((decimal?)0.4m, 0);
          csvWriter.Write((decimal?)decimal.Zero);
          csvWriter.Write((decimal?)-0.4m, 0);
          csvWriter.Write((decimal?)-0.9m, 0);
          csvWriter.Write((decimal?)-0.9m, 3);
          csvWriter.Write((decimal?)decimal.MinusOne);
          csvWriter.Write((decimal?)-1m, 1);
          csvWriter.Write((decimal?)-1.1m, 0);
          csvWriter.Write((decimal?)-1.1m, 1);
          csvWriter.Write((decimal?)-1234567890.12345678m, 8);
          csvWriter.Write((decimal?)decimal.MinValue);
          csvWriter.Write((decimal?)decimal.MinValue, 0);
          csvWriter.Write((decimal?)Math.PI);
          csvWriter.WriteEndOfLine();

          csvWriter.WriteFirstLineChar(csvConfig.LineCharAdd);
          csvWriter.Write('a');// 
          csvWriter.Write('Ä');// 
          csvWriter.Write('☹');// Smiley with white frowning face
          csvWriter.WriteEndOfLine();

          csvWriter.WriteFirstLineChar(csvConfig.LineCharAdd);
          csvWriter.Write((string?)null);
          csvWriter.Write("");
          csvWriter.Write("abc");
          csvWriter.Write("Ä");
          csvWriter.Write("aÄ☹");
          csvWriter.WriteEndOfLine();

          csvWriter.WriteFirstLineChar(csvConfig.LineCharAdd);
          csvWriter.WriteDate(DateTime.MaxValue.Date);
          csvWriter.WriteDate(DateTime.MinValue.Date);
          csvWriter.WriteDate(new DateTime(2000, 1, 1));
          csvWriter.WriteDate(new DateTime(2009, 12, 31));
          csvWriter.WriteDate(new DateTime(2010, 1, 1));
          csvWriter.WriteDate(new DateTime(2019, 12, 31));
          csvWriter.WriteDate(new DateTime(2020, 1, 1));
          csvWriter.WriteDate(new DateTime(2029, 12, 31));
          csvWriter.WriteDate(new DateTime(2020, 1, 1));
          csvWriter.WriteDate(new DateTime(2120, 1, 1));
          csvWriter.WriteEndOfLine();

          csvWriter.WriteFirstLineChar(csvConfig.LineCharAdd);
          csvWriter.WriteTime(new TimeSpan( 0,  0,  0));
          csvWriter.WriteTime(new TimeSpan( 0,  0,  1));
          csvWriter.WriteTime(new TimeSpan( 0,  1,  1));
          csvWriter.WriteTime(new TimeSpan( 1,  1,  1));
          csvWriter.WriteTime(new TimeSpan(23,  0,  0));
          csvWriter.WriteTime(new TimeSpan(23, 59,  0));
          csvWriter.WriteTime(new TimeSpan(23, 59, 59));
          csvWriter.WriteEndOfLine();

          csvWriter.WriteFirstLineChar(csvConfig.LineCharAdd);
          csvWriter.WriteDateTimeTicks(DateTime.MaxValue);
          csvWriter.WriteDateTimeTicks(DateTime.MinValue);
          csvWriter.WriteDateTimeTicks(new DateTime(2000, 1, 1));
          csvWriter.WriteDateTimeTicks(new DateTime(2000, 1, 1, 1, 1, 1, 1));
          csvWriter.WriteDateTimeTicks(new DateTime(2009, 12, 31));
          csvWriter.WriteDateTimeTicks(new DateTime(2009, 12, 31, 23, 59, 59, 999));
          csvWriter.WriteEndOfLine();

          csvWriter.WriteFirstLineChar(csvConfig.LineCharAdd);
          csvWriter.WriteDateMinutes(new DateTime(2000, 1, 1, 1, 1, 0, 0));
          csvWriter.WriteDateMinutes(new DateTime(2009, 12, 31, 23, 59, 0, 0));
          csvWriter.WriteDateMinutes(new DateTime(3009, 12, 31, 23, 0, 0, 0));
          csvWriter.WriteDateMinutes(new DateTime(4009, 12, 31, 23, 0, 30, 0));
          csvWriter.WriteEndOfLine();

          csvWriter.WriteFirstLineChar(csvConfig.LineCharAdd);
          csvWriter.WriteDateSeconds(new DateTime(2000, 1, 1, 1, 1, 1, 0));
          csvWriter.WriteDateSeconds(new DateTime(2009, 12, 31, 23, 59, 59, 0));
          csvWriter.WriteDateSeconds(new DateTime(3009, 12, 31, 23, 59, 0, 0));
          csvWriter.WriteDateSeconds(new DateTime(4009, 12, 31, 23, 0, 0, 0));
          csvWriter.WriteEndOfLine();

          for (int i = -csvConfig.BufferSize; i < csvConfig.BufferSize; i++) {
            csvWriter.WriteFirstLineChar(csvConfig.LineCharAdd);
            csvWriter.Write(i/1000m, 2);
            csvWriter.WriteEndOfLine();
          }
        }

        using var fileStream = new FileStream(fileName, FileMode.Open);
        using var streamReader = new StreamReader(fileStream);
        var line = streamReader.ReadLine();
        Assert.AreEqual("Some header", line);

        line = streamReader.ReadLine();
        Assert.AreEqual(csvConfig.LineCharAdd, line![0]);
        var fieldStrings = line![1..].Split(csvConfig.Delimiter);
        Assert.AreEqual("0", fieldStrings[0]);
        Assert.AreEqual("1", fieldStrings[1]);

        line = streamReader.ReadLine();
        Assert.AreEqual(csvConfig.LineCharAdd, line![0]);
        fieldStrings = line![1..].Split(csvConfig.Delimiter);
        Assert.AreEqual(int.MaxValue, int.Parse(fieldStrings[0]));
        Assert.AreEqual(1, int.Parse(fieldStrings[1]));
        Assert.AreEqual(0, int.Parse(fieldStrings[2]));
        Assert.AreEqual(-1, int.Parse(fieldStrings[3]));
        Assert.AreEqual(int.MinValue, int.Parse(fieldStrings[4]));

        line = streamReader.ReadLine();
        Assert.AreEqual(csvConfig.LineCharAdd, line![0]);
        fieldStrings = line![1..].Split(csvConfig.Delimiter);
        Assert.AreEqual("", fieldStrings[0]);
        Assert.AreEqual(int.MaxValue, int.Parse(fieldStrings[1]));
        Assert.AreEqual(1, int.Parse(fieldStrings[2]));
        Assert.AreEqual(0, int.Parse(fieldStrings[3]));
        Assert.AreEqual(-1, int.Parse(fieldStrings[4]));
        Assert.AreEqual(int.MinValue, int.Parse(fieldStrings[5]));

        line = streamReader.ReadLine();
        Assert.AreEqual(csvConfig.LineCharAdd, line![0]);
        fieldStrings = line![1..].Split(csvConfig.Delimiter);
        Assert.AreEqual(long.MaxValue, long.Parse(fieldStrings[0]));
        Assert.AreEqual(1L, long.Parse(fieldStrings[1]));
        Assert.AreEqual(0L, long.Parse(fieldStrings[2]));
        Assert.AreEqual(-1L, long.Parse(fieldStrings[3]));
        Assert.AreEqual(long.MinValue, long.Parse(fieldStrings[4]));

        line = streamReader.ReadLine();
        Assert.AreEqual(csvConfig.LineCharAdd, line![0]);
        fieldStrings = line![1..].Split(csvConfig.Delimiter);
        var fieldIndex = 0;
        //csvWriter.Write(decimal.MaxValue, 0);
        Assert.AreEqual(decimal.MaxValue, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write(decimal.MaxValue);
        Assert.AreEqual(decimal.MaxValue, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write(1234567890.12345678m, 8);
        Assert.AreEqual(1234567890.12345678m, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write(30m, 2);
        Assert.AreEqual(30m, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write(1.1m, 1);
        Assert.AreEqual(1.1m, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write(1.1m, 0);
        Assert.AreEqual(1m, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write(1m, 1);
        Assert.AreEqual(1m, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write(decimal.One);
        Assert.AreEqual(1m, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write(0.9m, 1);
        Assert.AreEqual(0.9m, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write(0.9m, 0);
        Assert.AreEqual(1m, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write(0.4m, 0);
        Assert.AreEqual(0m, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write(decimal.Zero);
        Assert.AreEqual(0m, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write(-0.4m, 0);
        Assert.AreEqual(0m, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write(-0.9m, 0);
        Assert.AreEqual(-1m, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write(-0.9m, 3);
        Assert.AreEqual(-0.9m, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write(decimal.MinusOne);
        Assert.AreEqual(-1m, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write(-1m, 1);
        Assert.AreEqual(-1m, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write(-1.1m, 0);
        Assert.AreEqual(-1m, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write(-1.1m, 1);
        Assert.AreEqual(-1.1m, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write(-1234567890.12345678m, 8);
        Assert.AreEqual(-1234567890.12345678m, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write(decimal.MinValue);
        Assert.AreEqual(decimal.MinValue, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write(decimal.MinValue, 1);
        Assert.AreEqual(decimal.MinValue, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write((decimal)Math.PI);
        Assert.AreEqual((decimal)Math.PI, decimal.Parse(fieldStrings[fieldIndex++]));

        line = streamReader.ReadLine();
        Assert.AreEqual(csvConfig.LineCharAdd, line![0]);
        fieldStrings = line![1..].Split(csvConfig.Delimiter);
        fieldIndex = 0;
        //csvWriter.Write((decimal?)null, 0);
        Assert.AreEqual(0, fieldStrings[fieldIndex++].Length);
        //csvWriter.Write((decimal?)decimal.MaxValue, 0);
        Assert.AreEqual(decimal.MaxValue, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write((decimal?)decimal.MaxValue);
        Assert.AreEqual(decimal.MaxValue, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write((decimal?)1234567890.12345678m, 8);
        Assert.AreEqual(1234567890.12345678m, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write((decimal?)30m, 2);
        Assert.AreEqual(30m, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write((decimal?)1.1m, 1);
        Assert.AreEqual(1.1m, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write((decimal?)1.1m, 0);
        Assert.AreEqual(1m, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write((decimal?)1m, 1);
        Assert.AreEqual(1m, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write((decimal?)decimal.One);
        Assert.AreEqual(1m, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write((decimal?)0.9m, 1);
        Assert.AreEqual(0.9m, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write((decimal?)0.9m, 0);
        Assert.AreEqual(1m, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write((decimal?)0.4m, 0);
        Assert.AreEqual(0m, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write((decimal?)decimal.Zero);
        Assert.AreEqual(0m, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write((decimal?)-0.4m, 0);
        Assert.AreEqual(0m, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write((decimal?)-0.9m, 0);
        Assert.AreEqual(-1m, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write((decimal?)-0.9m, 3);
        Assert.AreEqual(-0.9m, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write((decimal?)decimal.MinusOne);
        Assert.AreEqual(-1m, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write((decimal?)-1m, 1);
        Assert.AreEqual(-1m, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write((decimal?)-1.1m, 0);
        Assert.AreEqual(-1m, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write((decimal?)-1.1m, 1);
        Assert.AreEqual(-1.1m, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write((decimal?)-1234567890.12345678m, 8);
        Assert.AreEqual(-1234567890.12345678m, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write((decimal?)decimal.MinValue);
        Assert.AreEqual(decimal.MinValue, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write((decimal?)decimal.MinValue, 1);
        Assert.AreEqual(decimal.MinValue, decimal.Parse(fieldStrings[fieldIndex++]));
        //csvWriter.Write((decimal?)Math.PI);
        Assert.AreEqual((decimal?)Math.PI, decimal.Parse(fieldStrings[fieldIndex++]));

        line = streamReader.ReadLine();
        Assert.AreEqual(csvConfig.LineCharAdd, line![0]);
        fieldStrings = line![1..].Split(csvConfig.Delimiter);
        Assert.AreEqual("a", fieldStrings[0]);
        Assert.AreEqual("Ä", fieldStrings[1]);
        Assert.AreEqual("☹", fieldStrings[2]);

        line = streamReader.ReadLine();
        Assert.AreEqual(csvConfig.LineCharAdd, line![0]);
        fieldStrings = line![1..].Split(csvConfig.Delimiter);
        Assert.AreEqual("", fieldStrings[0]);
        Assert.AreEqual("\\ ", fieldStrings[1]);
        Assert.AreEqual("abc", fieldStrings[2]);
        Assert.AreEqual("Ä", fieldStrings[3]);
        Assert.AreEqual("aÄ☹", fieldStrings[4]);

        line = streamReader.ReadLine();
        Assert.AreEqual(csvConfig.LineCharAdd, line![0]);
        fieldStrings = line![1..].Split(csvConfig.Delimiter);
        Assert.AreEqual("31.12.9999", fieldStrings[0]);
        Assert.AreEqual("1.1.0001", fieldStrings[1]);
        Assert.AreEqual("1.1.2000", fieldStrings[2]);
        Assert.AreEqual("31.12.2009", fieldStrings[3]);
        Assert.AreEqual("1.1.2010", fieldStrings[4]);
        Assert.AreEqual("31.12.2019", fieldStrings[5]);
        Assert.AreEqual("1.1.2020", fieldStrings[6]);
        Assert.AreEqual("31.12.2029", fieldStrings[7]);
        Assert.AreEqual("1.1.2020", fieldStrings[8]);
        Assert.AreEqual("1.1.2120", fieldStrings[9]);

        line = streamReader.ReadLine();
        Assert.AreEqual(csvConfig.LineCharAdd, line![0]);
        fieldStrings = line![1..].Split(csvConfig.Delimiter);
        Assert.AreEqual("0", fieldStrings[0]);
        Assert.AreEqual("0:0:1", fieldStrings[1]);
        Assert.AreEqual("0:1:1", fieldStrings[2]);
        Assert.AreEqual("1:1:1", fieldStrings[3]);
        Assert.AreEqual("23", fieldStrings[4]);
        Assert.AreEqual("23:59", fieldStrings[5]);
        Assert.AreEqual("23:59:59", fieldStrings[6]);

        line = streamReader.ReadLine();
        Assert.AreEqual(csvConfig.LineCharAdd, line![0]);
        fieldStrings = line![1..].Split(csvConfig.Delimiter);
        Assert.AreEqual("3155378975999999999", fieldStrings[0]); //DateTime.MaxValue
        Assert.AreEqual("0", fieldStrings[1]);                   //DateTime.MinValue
        Assert.AreEqual("630822816000000000", fieldStrings[2]);  //new DateTime(2000, 1, 1)
        Assert.AreEqual("630822852610010000", fieldStrings[3]);  //new DateTime(2000, 1, 1, 1, 1, 1, 1)
        Assert.AreEqual("633978144000000000", fieldStrings[4]);  //new DateTime(2009, 12, 31)
        Assert.AreEqual("633979007999990000", fieldStrings[5]);  //new DateTime(2009, 12, 31, 23, 59, 59, 999)

        line = streamReader.ReadLine();
        Assert.AreEqual(csvConfig.LineCharAdd, line![0]);
        fieldStrings = line![1..].Split(csvConfig.Delimiter);
        Assert.AreEqual("1.1.2000 1:1", fieldStrings[0]);
        Assert.AreEqual("31.12.2009 23:59", fieldStrings[1]);
        Assert.AreEqual("31.12.3009 23", fieldStrings[2]);
        Assert.AreEqual("31.12.4009 23:1", fieldStrings[3]);

        line = streamReader.ReadLine();
        Assert.AreEqual(csvConfig.LineCharAdd, line![0]);
        fieldStrings = line![1..].Split(csvConfig.Delimiter);
        Assert.AreEqual("1.1.2000 1:1:1", fieldStrings[0]);
        Assert.AreEqual("31.12.2009 23:59:59", fieldStrings[1]);
        Assert.AreEqual("31.12.3009 23:59", fieldStrings[2]);
        Assert.AreEqual("31.12.4009 23", fieldStrings[3]);

        for (int i = -csvConfig.BufferSize; i < csvConfig.BufferSize; i++) {
          line = streamReader.ReadLine();
          if (i>-5 && i<5) {
            Assert.AreEqual("0\t", line![1..]);
          } else {
            Assert.AreEqual((i/1000m).ToString(".##") + '\t', line![1..]);
          }
        }

        //for (int i = -csvConfig.BufferSize; i < csvConfig.BufferSize; i++) {
        //  line = streamReader.ReadLine();
        //  Assert.AreEqual(i.ToString() + '\t', line![1..]);
        //}
        Assert.IsTrue(fileStream.ReadByte()<0);
      } finally {
        directoryInfo.Delete(recursive: true);
      }
    }


    private void reportException(Exception obj) {
      System.Diagnostics.Debug.WriteLine(obj);
      System.Diagnostics.Debugger.Break();
      Assert.Fail();
    }


    [TestMethod()]
    public void TestCsvWriterFlushTimer() {
      var directoryInfo = new DirectoryInfo("TestCsv");
      try {
        if (directoryInfo.Exists) {
          directoryInfo.Delete(recursive: true);
          directoryInfo.Refresh();
        }

        directoryInfo.Create();
        directoryInfo.Refresh();

        var csvConfig = new CsvConfig(directoryInfo.FullName, reportException: reportException);
        var fileName = csvConfig.DirectoryPath + @"\TestCsvWriterInt.csv";
        using (var csvWriter = new CsvWriter(fileName, csvConfig, estimatedLineLenght: 250, flushDelay: 50)) {
          csvWriter.WriteLine("Some header");

          csvWriter.WriteFirstLineChar(csvConfig.LineCharAdd);
          csvWriter.Write(int.MaxValue);
          csvWriter.Write(1);
          csvWriter.Write(0);
          csvWriter.Write(-1);
          csvWriter.Write(int.MinValue);
          csvWriter.WriteEndOfLine();

          Assert.IsTrue(csvWriter.ByteBufferLength>0);
          Thread.Sleep(300);
          Assert.AreEqual(0, csvWriter.ByteBufferLength);

          csvWriter.WriteFirstLineChar(csvConfig.LineCharAdd);
          csvWriter.Write((int?)null);
          csvWriter.Write((int?)int.MaxValue);
          csvWriter.Write((int?)1);
          csvWriter.Write((int?)0);
          csvWriter.Write((int?)-1);
          csvWriter.Write((int?)int.MinValue);
          csvWriter.WriteEndOfLine();
        }

        using var fileStream = new FileStream(fileName, FileMode.Open);
        using var streamReader = new StreamReader(fileStream);
        var line = streamReader.ReadLine();
        Assert.AreEqual("Some header", line);

        line = streamReader.ReadLine();
        Assert.AreEqual(csvConfig.LineCharAdd, line![0]);
        var fieldStrings = line![1..].Split(csvConfig.Delimiter);
        Assert.AreEqual(int.MaxValue, int.Parse(fieldStrings[0]));
        Assert.AreEqual(1, int.Parse(fieldStrings[1]));
        Assert.AreEqual(0, int.Parse(fieldStrings[2]));
        Assert.AreEqual(-1, int.Parse(fieldStrings[3]));
        Assert.AreEqual(int.MinValue, int.Parse(fieldStrings[4]));

        line = streamReader.ReadLine();
        Assert.AreEqual(csvConfig.LineCharAdd, line![0]);
        fieldStrings = line![1..].Split(csvConfig.Delimiter);
        Assert.AreEqual("", fieldStrings[0]);
        Assert.AreEqual(int.MaxValue, int.Parse(fieldStrings[1]));
        Assert.AreEqual(1, int.Parse(fieldStrings[2]));
        Assert.AreEqual(0, int.Parse(fieldStrings[3]));
        Assert.AreEqual(-1, int.Parse(fieldStrings[4]));
        Assert.AreEqual(int.MinValue, int.Parse(fieldStrings[5]));

        Assert.IsTrue(fileStream.ReadByte()<0);
      } finally {
        directoryInfo.Delete(recursive: true);
      }
    }
  }
}
