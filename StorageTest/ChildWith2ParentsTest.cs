using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageLib;
using TestContext;
#pragma warning disable IDE0045 // Convert to conditional expression


namespace StorageTest {


  [TestClass]
  public class ChildWith2ParentsTest {


    CsvConfig? csvConfig;
    BakCsvFileSwapper? bakCsvFileSwapper;
    string presentStructure = ""; //a string representing the content of all parents and children


    [TestMethod]
    public void TestChildWith2Parents() {
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

          initDC();
          presentStructure = "";
          assertData(presentStructure);

          //P01 C00_001_00,10_0_1_1_
          //P0: parent0
          //P01: parent0, parent1
          //P01 C00_001_00: parent0, parent1, child0 Parent1:0, ParentN1: null, ParentR1: 0, ParentNR1: 0, Parent2: 0, ...
          //P01 C00_001_00,10_0_1_1_: parent0, parent1, child0, child1
          //P01 C00_001_00,-0_0_1_1_: parent0, parent1, child0, deleted child? marked with '-'
          addParents(0, "P0");
          addParents(1, "P01");
          //addParents(2, "P012");
          //releaseParent(2, "P01");

          int? _ = null;
          //          Ch Prnt1 Parent2    C0Pr1 Parent2
          addChild___(0, 0, 0, 1, 1, "P01 C000001111");
          updateChild(0, 0, _, 1, _, "P01 C00_001_11");

          //          Ch Prnt1 Parent2    C0Pr1 Pr2  C1Pr1 Parent2
          addChild___(1, 0, _, 1, _, "P01 C00_001_11,10_0_1_1_");
          updateChild(1, 0, 0, 1, 1, "P01 C00_001_11,1000_111_");
          updateChild(1, 0, 0, 0, 0, "P01 C00_001_11,1000_001_");
          updateChild(1, 1, _, 0, _, "P01 C00_001_11,11_0_0_1_");

          releaseChild(1, "P01 C00_001_11,-1_0_0_1_", "P01 C00_001_11");
          //releaseChild(0, "P01 C-0_001_11", "P01");
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


    private void initDC() {
      _ = new DC(csvConfig);
    }


    private void addParents(int key, string structure) {
      void transaction() {
        _ = new Cw2PParent("Parent" + key, isStoring: true);
        _ = new Cw2PParentN("ParentN" + key, isStoring: true);
        _ = new Cw2PParentR("ParentR" + key, isStoring: true);
        _ = new Cw2PParentNR("ParentNR" + key, isStoring: true);
      }
      execute(transaction, structure);
    }


    //private void releaseParent(int masterKey, string structure) {
    //  void transaction() {
    //    DC.Data.Cw2PParents[masterKey].Release();
    //    DC.Data.Cw2PParentNs[masterKey].Release();
    //    DC.Data.Cw2PParentRs[masterKey].Release();
    //    DC.Data.Cw2PParentNRs[masterKey].Release();
    //  }
    //  execute(transaction, structure);
    //}


    private void addChild___(
      int childKey,
      int parent1Key,
      int? parent1NKey,
      int parent2Key,
      int? parent2NKey,
      string structure) 
    {
      void transaction() {
        _ = new Cw2PChild("Child" + childKey,
          DC.Data.Cw2PParents[parent1Key],
          parent1NKey is null ? null : DC.Data.Cw2PParentNs[parent1NKey.Value],
          DC.Data.Cw2PParentRs[parent1Key],
          parent1NKey is null ? null : DC.Data.Cw2PParentNRs[parent1NKey.Value],

          DC.Data.Cw2PParents[parent2Key],
          parent2NKey is null ? null : DC.Data.Cw2PParentNs[parent2NKey.Value],
          DC.Data.Cw2PParentRs[parent2Key],
          parent2NKey is null ? null : DC.Data.Cw2PParentNRs[parent2NKey.Value]);
      }
      execute(transaction, structure);
    }


    private void updateChild(
      int childKey,
      int parent0Key,
      int? parent0NKey,
      int parent1Key,
      int? parent1NKey,
      string structure) 
    {
      void transaction() {
        var child = DC.Data.Cw2PChildren[childKey];
        child.Update(child.Text + 'a',
          DC.Data.Cw2PParents[parent0Key],
          parent0NKey is null ? null : DC.Data.Cw2PParentNs[parent0NKey.Value],

          DC.Data.Cw2PParents[parent1Key],
          parent1NKey is null ? null : DC.Data.Cw2PParentNs[parent1NKey.Value]);
      }
      execute(transaction, structure);
    }


    private void releaseChild(int childKey, string structureCurrentDC, string structureNewDC) {
      void transaction() {
        DC.Data.Cw2PChildren[childKey].Release();
      }
      execute(transaction, structureCurrentDC, structureNewDC);
    }


    //private void assertData(string structure0, string? structure1 = null) {
    //  assertDC(structure0);

    //  if (csvConfig is null) return;

    //  DC.DisposeData();

    //  if (structure1 is null) {
    //    structure1 = structure0;
    //  }

    //  if (bakCsvFileSwapper!.UseBackupFiles()) {
    //    initDC();
    //    assertDC(structure1);
    //    DC.DisposeData();
    //    bakCsvFileSwapper.SwapBack();
    //  }

    //  initDC();
    //  assertDC(structure1);
    //}


    private void execute(Action transaction, string structureCurrentDC, string? structureNewDC = null) {
      //test rolled back transaction
      //----------------------------
      DC.Data.StartTransaction();
      transaction();
      assertData(structureCurrentDC);
      DC.Data.RollbackTransaction();
      assertData(presentStructure);
      if (csvConfig is not null) {
        assertDisposalRecreationDC();
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
      assertDisposalRecreationDC();
    }


    private void assertDisposalRecreationDC() {
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
      /*
      P01 C00_001_00,10_0_1_1_
      Parent0 Children0:01
      Parent1 Children1:01
      Parent0N
      Parent1N
      Parent0R Children0:01 Children1:0
      Parent1R Children1:1
      Parent0NR Children0:0 Children1:0
      Parent1NR
      Child0 Parent0: 0 Parent0R: 0 Parent0NR: 0 Parent1: 1 Parent1R: 0 Parent1NR: 0
      Child1 Parent0: 0 Parent0R: 0 Parent1: 1 Parent1R: 1      
      */
      sb.Clear();

      foreach (var child in DC.Data.Cw2PChildren) {
        sb.Append(Environment.NewLine + $"Child{child.Key}");
        sb.Append($" Parent0: {child.Parent0.Key}");
        sb.Append($" Parent0N: {child.Parent0N?.Key.ToString()??"_"}");
        sb.Append($" Parent0R: {child.Parent0R.Key}");
        sb.Append($" Parent0NR: {child.Parent0NR?.Key.ToString()??"_"}");
        sb.Append($" Parent1: {child.Parent1.Key}");
        sb.Append($" Parent1N: {child.Parent1N?.Key.ToString()??"_"}");
        sb.Append($" Parent1R: {child.Parent1R.Key}");
        sb.Append($" Parent1NR: {child.Parent1NR?.Key.ToString()??"_"}");
      }

      foreach (var parent in DC.Data.Cw2PParents) {
        add(sb, parent.Key, "", parent.Children0, parent.Children1);
      }
      foreach (var parent in DC.Data.Cw2PParentNs) {
        add(sb, parent.Key, "N", parent.Children0, parent.Children1);
      }
      foreach (var parent in DC.Data.Cw2PParentRs) {
        add(sb, parent.Key, "R", parent.Children0, parent.Children1);
      }
      foreach (var parent in DC.Data.Cw2PParentNRs) {
        add(sb, parent.Key, "NR", parent.Children0, parent.Children1);
      }
      return sb.ToString();
    }

    private static void add(
      StringBuilder sb, 
      int parentKey,
      string parentType,
      IStorageReadOnlyList<Cw2PChild> children0, 
      IStorageReadOnlyList<Cw2PChild> children1) 
    {
      sb.Append(Environment.NewLine + $"Parent{parentKey}{parentType}");
      add(sb, '0', children0);
      add(sb, '1', children1);
    }

    private static void add(StringBuilder sb, char childNo, IStorageReadOnlyList<Cw2PChild> children) {
      if (children.Count>0) {
        sb.Append($" Children{childNo}:");
        foreach (var child in children) {
          sb.Append(child.Key);
        }
      }
    }

    enum sStateEnum {
      classType,

      Parent,

      Child,
      ChildParent0,
      ChildParent0N,
      ChildParent0R,
      ChildParent0NR,
      ChildParent1,
      ChildParent1N,
      ChildParent1R,
      ChildParent1NR,
      ChildParentDelimiter
    }

    /*
     parent0 parent0N parent0R parent0NR   parent1 parent1N parent1R parent1NR   parent2...
         🠛                🠛                   🠛                   🠛
       ┌─┴─────────┐    ┌─┴─────────┐       ┌─┴─────────┐    ┌────┴────┐
       🠛           🠛    🠛           🠛       🠛           🠛    🠛         🠛
     children0 childr1 children0 childr1   children0 childr1 children0 children1
       🠙                            🠙                   🠙     🠙         
       │                            *                   #     │         
    ┌──┘             ┌────────────────────────────────────────┘         
    │                │                  #                * childx.Parent1R links to parent0R.children1
    🠙                🠙                  🠙                🠙
    0       null     1        null      1       null     0        null
    parent0 parent0N parent0R parent0RN parent1 parent1N parent1R parent1RN
      🠙       🠙        🠙        🠙         🠙       🠙        🠙        🠙
      └┬──────┴────────┴────────┴─────────┴───────┴────────┴────────┘
       🠙
    childx
    */

    //parentTypes:
    const int p__ = 0;         //nothing special
    const int pN_ = p__+1;     //null
    const int pR_ = pN_+1;     //readonly
    const int pNR = pR_+1;     //null and readonly
    const int pTypeCount = pNR+1;  //there are always 4 different types of parents


    private static string toString(int pTypeIndex) {
      return pTypeIndex switch {
        p__ => "",
        pN_ => "N",
        pR_ => "R",
        pNR => "NR",
        _ => throw new NotImplementedException(),
      };
    }


    const int pNoCount = 2;//parent numbers, a Cw2PChild has always 2 parents
    const int pCount = 3;//number of parents getting tested, can increase in the future
    const int cCount = 2;//number of children getting tested, can increase in the future


    private string toNiceString(string structure) {
      //P01 C00_001_00,10_0_1_1_
      //P0: parent0
      //P01: parent0, parent1
      //P01 C00_001_00: parent0, parent1, child0 Parent1:0, ParentN1: null, ParentR1: 0, ParentNR1: 0, Parent2: 0, ...
      //P01 C00_001_00,10_0_1_1_: parent0, parent1, child0, child1
      //P01 C00_001_00,-0_0_1_1_: parent0, parent1, child0, deleted child? marked with '-'
      sb.Clear();
      var parents = new Boolean[pTypeCount];
      var parentsChildren = new List<int>?[pTypeCount, pCount, pNoCount];
      var children = new int?[cCount, pNoCount, pTypeCount];
      var sState = sStateEnum.classType;
      var cIndex = -1;
      var pNoIndex = -1;
      var pTypeIndex = -1;

      //parse structure string and fill parents and children
      foreach (var ch in structure) {
        sb.Append(ch);
        switch (sState) {
        case sStateEnum.classType:
          sState = ch switch {
            'P' => sStateEnum.Parent,
            'C' => sStateEnum.Child,
            _ => throw new NotSupportedException(sb.ToString()),
          };
          break;

        case sStateEnum.Parent:
          if (ch>='0' && ch<='9') {
            var pIndex = ch - '0';
            parents[pIndex] = true;
            for (pTypeIndex=0; pTypeIndex<pTypeCount; pTypeIndex++) {
              for (pNoIndex=0; pNoIndex<pNoCount; pNoIndex++) {
                parentsChildren[pTypeIndex, pIndex, pNoIndex] = new List<int>();
              }
            }

          } else if (ch==' ') {
            sState = sStateEnum.classType;
          } else throw new NotSupportedException(sb.ToString());
          break;

        case sStateEnum.Child:
          if (ch>='0' && ch<='9') {
            cIndex = ch - '0';
            pNoIndex = 0;
            pTypeIndex = 0;
            sState = sStateEnum.ChildParent0;

          } else if (ch=='-') {
            cIndex = -1; //child is released and has a key of -1
            pNoIndex = 0;
            pTypeIndex = 0;
            sState = sStateEnum.ChildParent0;
          } else if (ch==' ') {
            sState = sStateEnum.classType;

          } else throw new NotSupportedException(sb.ToString());
          break;

        case sStateEnum.ChildParent0:
        case sStateEnum.ChildParent0N:
        case sStateEnum.ChildParent0R:
        case sStateEnum.ChildParent0NR:
        case sStateEnum.ChildParent1:
        case sStateEnum.ChildParent1N:
        case sStateEnum.ChildParent1R:
        case sStateEnum.ChildParent1NR:
          if (ch>='0' && ch<='9') {
            var parentId = ch - '0';
            if (cIndex>=0) {
              //child is stored (not released)
              children[cIndex, pNoIndex, pTypeIndex] = parentId;
            }
            parentsChildren[pTypeIndex, parentId, pNoIndex]!.Add(cIndex);

          } else if (ch=='_') {
            //this child.ParentXY is null. Nothing to do

          } else if (ch==' ') {
            sState = sStateEnum.classType;
            break;

          } else throw new NotSupportedException(sb.ToString());

          pTypeIndex++;
          if (pTypeIndex>=pTypeCount) {
            pTypeIndex = 0;
            pNoIndex++;
          }
          sState++;
          break;

        case sStateEnum.ChildParentDelimiter:
          if (ch==',') {
            sState = sStateEnum.Child;
          } else if (ch==' ') {
            sState = sStateEnum.classType;
          }
          break;
        default:
          throw new NotSupportedException(sb.ToString());
        }
      }

      //convert content of parents and children into final string
      sb.Clear();
      for (cIndex=0; cIndex<cCount; cIndex++) {
        if (children[cIndex, 0, pR_] is not null) {
          sb.Append(Environment.NewLine + $"Child{cIndex}");
          for (pNoIndex=0; pNoIndex<pNoCount; pNoIndex++) {
            for (pTypeIndex=0; pTypeIndex<pTypeCount; pTypeIndex++) {
              //var pID = children[cIndex, pNoIndex, pTypeIndex];
              //if (pID is not null) {
              //  sb.Append($" Parent{pNoIndex}{toString(pTypeIndex)}: " +
              //    $"{(children[cIndex, pNoIndex, pTypeIndex]?.ToString()??"_")}");
              //}
              sb.Append($" Parent{pNoIndex}{toString(pTypeIndex)}: " +
               $"{(children[cIndex, pNoIndex, pTypeIndex]?.ToString()??"_")}");
            }
          }
        }
      }
      for (pTypeIndex=0; pTypeIndex<pTypeCount; pTypeIndex++) {
        for (int pIndex = 0; pIndex<pCount; pIndex++) {
          if (parents[pIndex]) {
            sb.Append(Environment.NewLine + $"Parent{pIndex}{toString(pTypeIndex)}");
            for (pNoIndex=0; pNoIndex<pNoCount; pNoIndex++) {
              List<int> childrenList = parentsChildren[pTypeIndex, pIndex, pNoIndex]!;
              if (childrenList.Count>0) {
                sb.Append($" Children{pNoIndex}:");
                foreach (var childNo in childrenList) {
                  sb.Append(childNo);
                }
              }
            }
          }
        }
      }
      return sb.ToString();
    }
  }
}