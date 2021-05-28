using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StorageLib;


namespace StorageTest {


  public class BakCsvFileSwapper {


    readonly CsvConfig csvConfig;
    readonly DirectoryInfo directoryInfo;
    readonly List<(string OrgFile, string CsvFile)> copyInstructions;


    public BakCsvFileSwapper(CsvConfig csvConfig) {
      this.csvConfig = csvConfig;
      directoryInfo = new DirectoryInfo(csvConfig.DirectoryPath);
      copyInstructions = new();
    }


    public bool UseBackupFiles() {
      copyInstructions.Clear();
      directoryInfo.Refresh();
      var bakFiles = directoryInfo.GetFiles("*.bak");
      foreach (var file in bakFiles) {
        var fileNoExtension = file.FullName[..^3];
        var orgFile = fileNoExtension + "org";
        var csvFile = fileNoExtension + "csv";
        File.Move(csvFile, orgFile);
        file.CopyTo(csvFile);
        copyInstructions.Add((orgFile, csvFile));
      }
      return bakFiles.Length>0;
    }


    public void SwapBack() {
      foreach (var (OrgFile, CsvFile) in copyInstructions) {
        File.Move(OrgFile, CsvFile, overwrite: true);
      }
    }


    public void DeleteBakFiles() {
      directoryInfo.Refresh();
      foreach (var backFile in directoryInfo.GetFiles("*.bak")) {
        backFile.Delete();
      }
    }
  }
}
