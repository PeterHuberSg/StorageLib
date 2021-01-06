using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageLib;


namespace StorageTest {


  [TestClass()]
  public class CsvWriterTransactionTest {


    [TestMethod()]
    public void TestCsvWriterTransaction() {
      var directoryInfo = new DirectoryInfo("TestCsv");
      try {
        if (directoryInfo.Exists) {
          directoryInfo.Delete(recursive: true);
          directoryInfo.Refresh();
        }

        directoryInfo.Create();
        directoryInfo.Refresh();

        var csvConfig = new CsvConfig(directoryInfo.FullName, reportException: reportException);
        var fileName = csvConfig.DirectoryPath + @"\TestCsvWriterTransaction.csv";
        var header = "Some long header";
        var someText = "Some Text followed by character a: ";
        var aCharacter = 'a';
        var expectedText =
            header + Environment.NewLine +
            someText + csvConfig.Delimiter + aCharacter + csvConfig.Delimiter + Environment.NewLine;
        using (var csvWriter = new CsvWriter(fileName, csvConfig, 250)) {
          csvWriter.WriteLine(header);
          csvWriter.StartTransaction();
          csvWriter.StartNewLine();
          csvWriter.Write(someText);
          csvWriter.Write(aCharacter);
          csvWriter.WriteEndOfLine();
          csvWriter.CommitTransaction();
        }
        assert(fileName, expectedText);

        header = "Header";
        someText = "b: ";
        aCharacter = 'b';
        expectedText +=
            header + Environment.NewLine;
        using (var fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 
          csvConfig.BufferSize, FileOptions.SequentialScan)) 
        {
          fileStream.Position = fileStream.Length;
          using var csvWriter = new CsvWriter(null, csvConfig, 250, fileStream);
          csvWriter.WriteLine(header);
          csvWriter.StartTransaction();
          csvWriter.StartNewLine();
          csvWriter.Write(someText);
          csvWriter.Write(aCharacter);
          csvWriter.WriteEndOfLine();
          csvWriter.RollbackTransaction();
        }
        assert(fileName, expectedText);

        header = "Header 3";
        someText = "CCCCC: ";
        aCharacter = 'c';
        expectedText +=
            header + Environment.NewLine;
        using (var fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None,
          csvConfig.BufferSize, FileOptions.SequentialScan)) 
        {
          fileStream.Position = fileStream.Length;
          using var csvWriter = new CsvWriter(null, csvConfig, 250, fileStream);
          csvWriter.WriteLine(header);
          csvWriter.StartTransaction();
          csvWriter.StartNewLine();
          csvWriter.Write(someText);
          csvWriter.RollbackTransaction();
        }
        assert(fileName, expectedText);

        header = "Header 4";
        someText = "DDDD: ";
        aCharacter = 'd';
        expectedText +=
            header + Environment.NewLine + 
            someText + csvConfig.Delimiter + aCharacter + csvConfig.Delimiter + Environment.NewLine;
        using (var fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None,
          csvConfig.BufferSize, FileOptions.SequentialScan)) 
        {
          fileStream.Position = fileStream.Length;
          using var csvWriter = new CsvWriter(null, csvConfig, 250, fileStream);
          csvWriter.WriteLine(header);
          csvWriter.StartTransaction();
          csvWriter.StartNewLine();
          csvWriter.Write("text gets rolled back");
          csvWriter.RollbackTransaction();
          csvWriter.StartNewLine();
          csvWriter.Write(someText);
          csvWriter.Write(aCharacter);
          csvWriter.WriteEndOfLine();
        }
        assert(fileName, expectedText);

        header = "Header 5";
        someText = "EEEE: ";
        aCharacter = 'e';
        expectedText += header + Environment.NewLine;
        using (var fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None,
          csvConfig.BufferSize, FileOptions.SequentialScan)) 
        {
          fileStream.Position = fileStream.Length;
          using var csvWriter = new CsvWriter(null, csvConfig, 250, fileStream);
          csvWriter.WriteLine(header);
          try {
            csvWriter.StartTransaction();
            csvWriter.StartNewLine();
            csvWriter.Write("WriteDate which has also time throws exception, catch executes rollback");
            csvWriter.WriteDate(new DateTime(1999, 9, 29, 19, 59, 59));

          } catch (Exception) {
            csvWriter.RollbackTransaction();
          }
        }
        assert(fileName, expectedText);

        header = "Header 6";
        someText = "FFFF: ";
        aCharacter = 'f';
        expectedText +=
            header + Environment.NewLine +
            someText + csvConfig.Delimiter + aCharacter + csvConfig.Delimiter + Environment.NewLine;
        using (var fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None,
          csvConfig.BufferSize, FileOptions.SequentialScan)) {
          fileStream.Position = fileStream.Length;
          using var csvWriter = new CsvWriter(null, csvConfig, 250, fileStream);
          csvWriter.WriteLine(header);
          try {
            csvWriter.StartTransaction();
            csvWriter.StartNewLine();
            csvWriter.Write("WriteDate which has also time throws exception, catch executes rollback");
            csvWriter.WriteDate(new DateTime(1999, 9, 29, 19, 59, 59));

          } catch (Exception) {
            csvWriter.RollbackTransaction();
          }
          csvWriter.StartNewLine();
          csvWriter.Write(someText);
          csvWriter.Write(aCharacter);
          csvWriter.WriteEndOfLine();
        }
        assert(fileName, expectedText);


        header = "Header 7";
        someText = "GGGGGG: ";
        aCharacter = 'g';
        var longString = new string('a', 1024-3);
        expectedText += header + Environment.NewLine;
        using (var fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None,
          csvConfig.BufferSize, FileOptions.SequentialScan)) 
        {
          fileStream.Position = fileStream.Length;
          using var csvWriter = new CsvWriter(null, csvConfig, 1024, fileStream);
          csvWriter.WriteLine(header);
          csvWriter.StartTransaction();
          //write more than CsvWriter buffer can hold;
          for (int i = 0; i < 5; i++) {
            csvWriter.WriteLine(longString); 
          }
          csvWriter.StartNewLine();
          csvWriter.Write(someText);
          csvWriter.Write(aCharacter);
          csvWriter.WriteEndOfLine();
          csvWriter.RollbackTransaction();
        }
        assert(fileName, expectedText);

      } finally {
        directoryInfo.Delete(recursive: true);
      }
    }


    private void assert(string fileName, string expectedText) {
      using var fileStream = new FileStream(fileName, FileMode.Open);
      using var streamReader = new StreamReader(fileStream);
      var content = streamReader.ReadToEnd();
      Assert.AreEqual(expectedText, content);
    }


    private void reportException(Exception obj) {
      System.Diagnostics.Debug.WriteLine(obj);
      System.Diagnostics.Debugger.Break();
      Assert.Fail();
    }
  }
}
