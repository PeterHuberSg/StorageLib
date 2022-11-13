using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageLib;
using TestContext;


namespace StorageTest {


  [TestClass]
  public class ListTreeTest {

    CsvConfig? csvConfig;
    BakCsvFileSwapper? bakCsvFileSwapper;
    string presentStructure = ""; //a string representing the content of all parents and children


    [TestMethod]
    public void TestListTree() {
      var directoryInfo = new DirectoryInfo("TestCsv");

      try {
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
          directoryInfo.Refresh();
          if (directoryInfo.Exists) {
            directoryInfo.Delete(recursive: true);
            directoryInfo.Refresh();
          }

          directoryInfo.Create();

          _ = new DC(csvConfig);
          presentStructure = "";
          assertData(presentStructure);

          int? n = null;
          createBranch(0, n, "B0");
          createBranch(1, n, "B0 B1");
          createBranch(2, 0, "B0C2 B1 B2P0");
          createBranch(3, 2, "B0C2 B1 B2P0C3 B3P2");
          createBranch(4, 2, "B0C2 B1 B2P0C34 B3P2 B4P2");

          updateBranch(4, n, "B0C2 B1 B2P0C3 B3P2 B4");
          updateBranch(4, 1, "B0C2 B1C4 B2P0C3 B3P2 B4P1");
          updateBranch(4, 3, "B0C2 B1 B2P0C3 B3P2C4 B4P3");
          updateBranch(1, 2, "B0C2 B1P2 B2P0C31 B3P2C4 B4P3");
          DC.DisposeData();
        }
      } finally {
        DC.DisposeData();
      }
    }


    private void reportException(Exception obj) {
      System.Diagnostics.Debug.WriteLine(obj);
      System.Diagnostics.Debugger.Break();
      Assert.Fail();
    }


    private void createBranch(int key, int? parentNo, string structure) {
      void transaction() {
        var parent = parentNo is null ? null : DC.Data.ListTreeBranchs[parentNo.Value];
        _ = new ListTreeBranch("Branch" + key, parent);
      }
      execute(transaction, structure);
    }


    private void updateBranch(int key, int? parentNo, string structure) {
      void transaction() {
        var branch = DC.Data.ListTreeBranchs[key];
        var parent = parentNo is null ? null : DC.Data.ListTreeBranchs[parentNo.Value];
        branch.Update(branch.Text, parent);
      }
      execute(transaction, structure);
    }


    private void execute(Action transaction, string structureCurrentDC, string? structureNewDC = null) {
      //test rolled back transaction
      //----------------------------
      DC.Data.StartTransaction();
      transaction();
      assertData(structureCurrentDC);
      DC.Data.RollbackTransaction();
      assertData(presentStructure);
      if (csvConfig is not null) {
        assertDisposalRecreactionDC();
      }

      //test committed transaction
      //--------------------------
      DC.Data.StartTransaction();
      transaction();
      DC.Data.CommitTransaction();
      assertData(structureCurrentDC);
      presentStructure = structureCurrentDC;
      if (csvConfig is null) return;

      if (structureNewDC is not null) {
        presentStructure = structureNewDC;
      }
      assertDisposalRecreactionDC();
    }


    private void assertDisposalRecreactionDC() {
      DC.DisposeData();

      if (bakCsvFileSwapper!.UseBackupFiles()) {
        //restore DC from .bak file
        _ = new DC(csvConfig); ;
        assertData(presentStructure);
        DC.DisposeData();
        bakCsvFileSwapper.SwapBack();
      }

      //reopen DC from .csv file
      _ = new DC(csvConfig);
      assertData(presentStructure);
    }


    private void assertData(string structure) {
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
        var errorMessage = structure + Environment.NewLine +
          "Both:" + expectedStructure[0..charIndex] + Environment.NewLine + Environment.NewLine +
          "Expected:" + Environment.NewLine  + expectedStructure[charIndex..] + Environment.NewLine + Environment.NewLine +
          "Actual: "+ Environment.NewLine  + actualStructure[charIndex..];
        Assert.Fail(errorMessage);
      }
    }


    readonly StringBuilder sb = new();

    private string dataToNiceString() {
      sb.Clear();
      foreach (var branch in DC.Data.ListTreeBranchs) {
        sb.Append($"Branch{branch.Key}");
        if (branch.Parent is not null) {
          sb.Append($" Parent{branch.Parent.Key}");
        }
        if (branch.Children.Count>0) {
          sb.Append($" Children: ");
        }
        foreach (var childBranch in branch.Children) {
          sb.Append($"{childBranch.Key}");
        }
        sb.AppendLine();
      }
      return sb.ToString();
    }


    enum sStateEnum {
      Branch,
      BranchNo,
      ParentChildren,
      ParentNo,
      Children,
      ChildrenNo
    }

    private string toNiceString(string structure) {
      // |Branch0 without parent nor children
      //"B0": 

      // |Branch0 without parent and children 1
      // |    |Branch1 parent0 and no children
      //"B0C1 B1P0"

      // |Branch0 without parent and children 1
      // |    |Branch1 parent0 and children 2
      // |    |      |Branch2 parent1 and no children
      //"B0C1 B1P0C2 B2P1"

      // |Branch0 without parent and children 1
      // |    |Branch1 parent0 and children 2,3
      // |    |      |Branch2 parent1 and no children
      // |    |      |     |Branch3 parent1 and no children
      //"B0C1 B1P0C23 B2P1 B3P1"
      if (structure.Length==0) return "";

      sb.Clear();
      var sState = sStateEnum.Branch;
      var charIndex = 0;
      foreach (var ch in structure) {
        charIndex++;
        switch (sState) {
        case sStateEnum.Branch:
          if (ch!='B') throw new NotSupportedException(structure[..charIndex]);

          sb.Append("Branch");
          sState++;
          break;

        case sStateEnum.BranchNo:
          if (ch<'0' || ch>'9') throw new NotSupportedException(structure[..charIndex]);

          sb.Append(ch);
          sState++;
          break;

        case sStateEnum.ParentChildren:
          if (ch=='P') {
            sb.Append(" Parent");
            sState = sStateEnum.ParentNo;
          } else if (ch=='C') {
            sb.Append(" Children: ");
            sState = sStateEnum.ChildrenNo;
          } else if (ch==' ') {
            sb.AppendLine();
            sState = sStateEnum.Branch;
          } else throw new NotSupportedException(structure[..charIndex]);

          break;

        case sStateEnum.ParentNo:
          if (ch<'0' || ch>'9') throw new NotSupportedException(structure[..charIndex]);

          sb.Append(ch);
          sState++;
          break;

        case sStateEnum.Children:
          if (ch=='C') {
            sb.Append(" Children: ");
            sState++;
          }else if(ch==' ') {
            sb.AppendLine();
            sState = sStateEnum.Branch;
          } else throw new NotSupportedException(structure[..charIndex]);

          break;

        case sStateEnum.ChildrenNo:
          if (ch>='0' && ch<='9') {
            sb.Append(ch);
          } else if (ch==' ') {
            sb.AppendLine();
            sState = sStateEnum.Branch;
          } else throw new NotSupportedException(structure[..charIndex]);

          break;

        default:
          throw new NotSupportedException(structure[..charIndex]);
        }
      }

      sb.AppendLine();
      return sb.ToString();
    }
  }
}