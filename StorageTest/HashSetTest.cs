using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageLib;
using TestContext;
#pragma warning disable IDE0045 // Convert to conditional expression


namespace StorageTest {


  [TestClass]
  public class HashSetTest {


    CsvConfig? csvConfig;
    BakCsvFileSwapper? bakCsvFileSwapper;
    string presentStructure = ""; //a string representing the content of all parents and children


    [TestMethod]
    public void TestHashSet() {
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

          //Expected parent and children content
          //------------------------------------
          //P01 C00_001_00,10_0_1_1_
          //P0: parent0
          //P01: parent0, parent1
          //P01 C00_001_00: parent0, parent1, child0 Parent1:0, ParentN1: null, ParentR1: 0, ParentNR1: 0, Parent2: 0, ...
          //P01 C00_001_00,10_0_1_1_: parent0, parent1, child0, child1
          //P01 C00_001_00,-0_0_1_1_: parent0, parent1, child0, deleted child? marked with '-'

          addParents(0, "P0");
          addParents(1, "P01");
          addParents(2, "P012");
          releaseParent(2, "P01");
          //      child parameters|Expected parent and children content
          //                      |Prnt|Children
          //           P0  P0R    |    | P0  P0R
          //            P1  P1R   |    |  P1  P1R
          //             P0N P0NR |    |   P0N P0NR
          //              P1N P1NR|    |    P1N P1NR
          addChild(0, "00__00__", "P01 C000__00__");
          addChild(1, "01__11_1", "P01 C000__00__,101__11_1");
          addChild(2, "10101010", "P01 C000__00__,101__11_1,210101010");
          addChild(3, "11111111", "P01 C000__00__,101__11_1,210101010,311111111");

          //  child parameters|Expected parent and children content      
          //                  |Parents|Children
          //           P0     |       | P0  P0R
          //            P1    |       |  P1  P1R
          //             P0N  |       |   P0N P0NR
          //              P1N |       |    P1N P1NR
          updateChild(0, "0110", "P01 C0011000__,101__11_1,210101010,311111111");
          updateChild(0, "1001", "P01 C0100100__,101__11_1,210101010,311111111");
          updateChild(0, "00__", "P01 C000__00__,101__11_1,210101010,311111111");

          //              |               -:child 1 no longer stored, but parents still link to it
          //              |                                               |after restart DC
          releaseChild(1, "P01 C000__00__,-01__11_1,210101010,311111111", "P01 C000__00__,210101010,311111111");

          //ensure that parent cannot get released as long it has not released children
          Assert.ThrowsException<Exception>(() => {
            DC.Data.HashSetParents[1].Release();
          });
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


    private void addParents(int parentKey, string structure) {
      void transaction() {
        _ = new HashSetParent($"Parent{parentKey}");
        _ = new HashSetParentN($"ParentN{parentKey}");
        _ = new HashSetParentR($"ParentR{parentKey}");
        _ = new HashSetParentNR($"ParentNR{parentKey}");
      }
      execute(transaction, structure);
    }


    private void releaseParent(int parentKey, string structure) {
      void transaction() {
        DC.Data.HashSetParents[parentKey].Release();
        DC.Data.HashSetParentNs[parentKey].Release();
        DC.Data.HashSetParentRs[parentKey].Release();
        DC.Data.HashSetParentNRs[parentKey].Release();
      }
      execute(transaction, structure);
    }


    private void addChild(int childKey, string parentsString, string structure) {
      void transaction() {
        var parentIndex = 0;
        _ = new HashSetChild($"Child{childKey}",
          (HashSetParent)getParent(parentIndex++, parentsString),
          (HashSetParent)getParent(parentIndex++, parentsString),
          (HashSetParentN?)getParent(parentIndex++, parentsString),
          (HashSetParentN?)getParent(parentIndex++, parentsString),
          (HashSetParentR)getParent(parentIndex++, parentsString),
          (HashSetParentR)getParent(parentIndex++, parentsString),
          (HashSetParentNR?)getParent(parentIndex++, parentsString),
          (HashSetParentNR?)getParent(parentIndex++, parentsString));
      }
      execute(transaction, structure);
    }


    private void updateChild(int childKey, string parentsString, string structure) {
      void transaction() {
        var parentIndex = 0;
        var child = DC.Data.HashSetChildren[childKey];
        child.Update(child.Text,
          (HashSetParent)getParent(parentIndex++, parentsString),
          (HashSetParent)getParent(parentIndex++, parentsString),
          (HashSetParentN?)getParent(parentIndex++, parentsString),
          (HashSetParentN?)getParent(parentIndex++, parentsString));
      }
      execute(transaction, structure);
    }


    private void releaseChild(int childKey, string structureCurrentDC, string structureNewDC) {
      void transaction() {
        var child = DC.Data.HashSetChildren[childKey];
        child.Release();
      }
      execute(transaction, structureCurrentDC, structureNewDC);
    }


    private static object getParent(int parentIndex, string parentsString) {
      var parentChar = parentsString[parentIndex];
      if (parentChar is '_') {
        return null!;
      }
      var parentKey = parentChar - '0';
      return parentIndex switch {
        0 => DC.Data.HashSetParents[parentKey],
        1 => DC.Data.HashSetParents[parentKey],
        2 => DC.Data.HashSetParentNs[parentKey],
        3 => DC.Data.HashSetParentNs[parentKey],
        4 => DC.Data.HashSetParentRs[parentKey],
        5 => DC.Data.HashSetParentRs[parentKey],
        6 => DC.Data.HashSetParentNRs[parentKey],
        7 => DC.Data.HashSetParentNRs[parentKey],
        _ => throw new NotSupportedException(),
      };
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

      foreach (var child in DC.Data.HashSetChildren) {
        sb.Append(Environment.NewLine + $"Child{child.Key}");
        sb.Append($" Parent0: {child.Parent0.Key}");
        sb.Append($" Parent1: {child.Parent1.Key}");
        sb.Append($" Parent0N: {child.Parent0N?.Key.ToString()??"_"}");
        sb.Append($" Parent1N: {child.Parent1N?.Key.ToString()??"_"}");
        sb.Append($" Parent0R: {child.Parent0R.Key}");
        sb.Append($" Parent1R: {child.Parent1R.Key}");
        sb.Append($" Parent0NR: {child.Parent0NR?.Key.ToString()??"_"}");
        sb.Append($" Parent1NR: {child.Parent1NR?.Key.ToString()??"_"}");
      }

      foreach (var parent in DC.Data.HashSetParents) {
        add(sb, parent.Key, "", parent.Children);
      }
      foreach (var parent in DC.Data.HashSetParentNs) {
        add(sb, parent.Key, "N", parent.Children);
      }
      foreach (var parent in DC.Data.HashSetParentRs) {
        add(sb, parent.Key, "R", parent.Children);
      }
      foreach (var parent in DC.Data.HashSetParentNRs) {
        add(sb, parent.Key, "NR", parent.Children);
      }
      return sb.ToString();
    }


    private static void add(
      StringBuilder sb,
      int parentKey,
      string parentType,
      IStorageReadOnlySet<HashSetChild> children) 
    {
      sb.Append(Environment.NewLine + $"Parent{parentKey}{parentType}");
      if (children.Count>0) {
        sb.Append($" Children:");
        foreach (var child in children.OrderBy(c=>c.Key)) {
          sb.Append(child.Key);
        }
      }
    }


    enum sStateEnum {
      classType,

      Parent,

      Child,
      ChildParent0,
      ChildParent1,
      ChildParent0N,
      ChildParent1N,
      ChildParent0R,
      ChildParent1R,
      ChildParent0NR,
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


    const int pNoCount = 2;//parent property numbers, a HashSetPChild has always 8 parents (pTypeCount * pNoCount)
    const int pCount = 3;//number of parents getting tested, can increase in the future
    const int cCount = 4;//number of children getting tested, can increase in the future


    private string toNiceString(string structure) {
      //P01 C00_001_00,10_0_1_1_
      //P0: parent0
      //P01: parent0, parent1
      //P01 C00_001_00: parent0, parent1, child0 Parent1:0, ParentN1: null, ParentR1: 0, ParentNR1: 0, Parent2: 0, ...
      //P01 C00_001_00,10_0_1_1_: parent0, parent1, child0, child1
      //P01 C00_001_00,-0_0_1_1_: parent0, parent1, child0, deleted child? marked with '-'
      sb.Clear();
      var parents = new Boolean[pTypeCount];
      var parentsChildren = new HashSet<int>?[pTypeCount, pCount];
      var childrenParents = new int?[cCount, pTypeCount, pNoCount];
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
              parentsChildren[pTypeIndex, pIndex] = new HashSet<int>();
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
              childrenParents[cIndex, pTypeIndex, pNoIndex] = parentId;
            }
            parentsChildren[pTypeIndex, parentId]!.Add(cIndex);

          } else if (ch=='_') {
            //this child.ParentXY is null. Nothing to do

          } else if (ch==' ') {
            sState = sStateEnum.classType;
            break;

          } else throw new NotSupportedException(sb.ToString());

          pNoIndex++;
          if (pNoIndex>=pNoCount) {
            pNoIndex = 0;
            pTypeIndex++;
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
        if (childrenParents[cIndex, pR_, 0] is not null) {
          sb.Append(Environment.NewLine + $"Child{cIndex}");
          for (pTypeIndex=0; pTypeIndex<pTypeCount; pTypeIndex++) {
            for (pNoIndex=0; pNoIndex<pNoCount; pNoIndex++) {
              sb.Append($" Parent{pNoIndex}{toString(pTypeIndex)}: " +
               $"{(childrenParents[cIndex, pTypeIndex, pNoIndex]?.ToString()??"_")}");
            }
          }
        }
      }
      for (pTypeIndex=0; pTypeIndex<pTypeCount; pTypeIndex++) {
        for (int pIndex = 0; pIndex<pCount; pIndex++) {
          if (parents[pIndex]) {
            sb.Append(Environment.NewLine + $"Parent{pIndex}{toString(pTypeIndex)}");
            var childrenHashSet = parentsChildren[pTypeIndex, pIndex]!;
            if (childrenHashSet.Count>0) {
              sb.Append($" Children:");
              foreach (var childNo in childrenHashSet.OrderBy(cN=>cN)) {
                sb.Append(childNo);
              }
            }
          }
        }
      }
      return sb.ToString();
    }
  }
}
