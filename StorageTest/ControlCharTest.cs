using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageLib;


namespace StorageTest {


  [TestClass()]
  public class ControlCharTest {


    [TestMethod()]
    public void TestControlChar() {
      var directoryInfo = new DirectoryInfo("ControlCharTest");
      try {
        if (directoryInfo.Exists) {
          directoryInfo.Delete(recursive: true);
          directoryInfo.Refresh();
        }

        directoryInfo.Create();
        directoryInfo.Refresh();

        var csvConfig = new CsvConfig(directoryInfo.FullName, reportException: reportException);
        var fileName = csvConfig.DirectoryPath + @"\ControlCharTest.csv";
        var header = "Characters from ASCII 0 up to '!'";
        var characters = new char[34];
        var maxLineLenght = 250;
        string? nullString = null;
        string asciiString;
        var utfString = "Smiley: 😀;Tab: \t; CR: \r; LF: \n; Slash: \\; Burmese: ၑ;";
        using (var csvWriter = new CsvWriter(fileName, csvConfig, maxLineLenght)) {
          csvWriter.WriteLine(header);
          csvWriter.StartNewLine();
          for (int i = 0; i < characters.Length; i++) {
            var c = (char)i;
            characters[i] = c;
            csvWriter.Write(c);
          }
          csvWriter.WriteEndOfLine();

          csvWriter.StartNewLine();
          csvWriter.Write(nullString);
          csvWriter.Write("");
          csvWriter.Write(" ");
          csvWriter.Write("a");
          csvWriter.Write(csvConfig.Delimiter.ToString());
          csvWriter.Write("\\");
          csvWriter.Write("\\n");
          csvWriter.Write("\\r");
          csvWriter.Write("\\r\\n");
          csvWriter.Write("😀");
          csvWriter.Write("ၑ");
          asciiString = new string(characters);
          csvWriter.Write(asciiString);
          csvWriter.Write(utfString);
          csvWriter.WriteEndOfLine();
        }

        using var csvReader = new CsvReader(fileName, csvConfig, maxLineLenght);
        var line = csvReader.ReadLine();
        Assert.AreEqual(header, line);

        for (int i = 0; i < characters.Length; i++) {
          var c = csvReader.ReadChar();
          characters[i] = c;
          Assert.AreEqual(characters[i], c);
        }
        csvReader.ReadEndOfLine();
        Assert.IsNull(csvReader.ReadStringNull());
        Assert.AreEqual("", csvReader.ReadStringNull());
        Assert.AreEqual(" ", csvReader.ReadStringNull());
        Assert.AreEqual("a", csvReader.ReadStringNull());
        Assert.AreEqual(csvConfig.Delimiter.ToString(), csvReader.ReadStringNull());
        Assert.AreEqual("\\", csvReader.ReadStringNull());
        Assert.AreEqual("\\n", csvReader.ReadStringNull());
        Assert.AreEqual("\\r", csvReader.ReadStringNull());
        Assert.AreEqual("\\r\\n", csvReader.ReadStringNull());
        Assert.AreEqual("😀", csvReader.ReadStringNull());
        Assert.AreEqual("ၑ", csvReader.ReadStringNull());
        Assert.AreEqual(asciiString, csvReader.ReadStringNull());
        Assert.AreEqual(utfString, csvReader.ReadStringNull());

      } finally {
        directoryInfo.Delete(recursive: true);
      }
    }


    private void reportException(Exception obj) {
      System.Diagnostics.Debug.WriteLine(obj);
      System.Diagnostics.Debugger.Break();
      Assert.Fail();
    }
  }
}
