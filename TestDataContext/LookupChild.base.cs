//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by StorageClassGenerator
//
//     Do not change code in this file, it will get lost when the file gets 
//     auto generated again. Write your code into LookupChild.cs.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using StorageLib;


namespace TestContext  {


    /// <summary>
    /// Example of a child with a none nullable and a nullable lookup parent. The child maintains links
    /// to its parents, but the parents don't have children collections.
    /// </summary>
  public partial class LookupChild: IStorageItemGeneric<LookupChild> {

    #region Properties
    //      ----------

    /// <summary>
    /// Unique identifier for LookupChild. Gets set once LookupChild gets added to DC.Data.
    /// </summary>
    public int Key { get; private set; }
    internal static void SetKey(IStorageItem lookupChild, int key, bool isRollback) {
#if DEBUG
      if (isRollback) {
        if (key==StorageExtensions.NoKey) {
          DC.Trace?.Invoke($"Release LookupChild key @{lookupChild.Key} #{lookupChild.GetHashCode()}");
        } else {
          DC.Trace?.Invoke($"Store LookupChild key @{key} #{lookupChild.GetHashCode()}");
        }
      }
#endif
      ((LookupChild)lookupChild).Key = key;
    }


    /// <summary>
    /// Some info
    /// </summary>
    public string Text { get; private set; }


    public LookupParent Parent { get; private set; }


    public LookupParentN? ParentN { get; private set; }


    public LookupParentR ParentR { get; }


    public LookupParentNR? ParentNR { get; }


    /// <summary>
    /// Headers written to first line in CSV file
    /// </summary>
    internal static readonly string[] Headers = {
      "Key", 
      "Text", 
      "Parent", 
      "ParentN", 
      "ParentR", 
      "ParentNR"
    };


    /// <summary>
    /// None existing LookupChild
    /// </summary>
    internal static LookupChild NoLookupChild = new LookupChild("NoText", LookupParent.NoLookupParent, null, LookupParentR.NoLookupParentR, null, isStoring: false);
    #endregion


    #region Events
    //      ------

    /// <summary>
    /// Content of LookupChild has changed. Gets only raised for changes occurring after loading DC.Data with previously stored data.
    /// </summary>
    public event Action</*old*/LookupChild, /*new*/LookupChild>? HasChanged;
    #endregion


    #region Constructors
    //      ------------

    /// <summary>
    /// LookupChild Constructor. If isStoring is true, adds LookupChild to DC.Data.LookupChildren.
    /// </summary>
    public LookupChild(
      string text, 
      LookupParent parent, 
      LookupParentN? parentN, 
      LookupParentR parentR, 
      LookupParentNR? parentNR, 
      bool isStoring = true)
    {
      Key = StorageExtensions.NoKey;
      Text = text;
      Parent = parent;
      ParentN = parentN;
      ParentR = parentR;
      ParentNR = parentNR;
#if DEBUG
      DC.Trace?.Invoke($"new LookupChild: {ToTraceString()}");
#endif
      onConstruct();
      if (DC.Data.IsTransaction) {
        DC.Data.AddTransaction(new TransactionItem(4,TransactionActivityEnum.New, Key, this));
      }

      if (isStoring) {
        Store();
      }
    }
    partial void onConstruct();


    /// <summary>
    /// Cloning constructor. It will copy all data from original except any collection (children).
    /// </summary>
    #pragma warning disable CS8618 // Children collections are uninitialized.
    public LookupChild(LookupChild original) {
    #pragma warning restore CS8618 //
      Key = StorageExtensions.NoKey;
      Text = original.Text;
      Parent = original.Parent;
      ParentN = original.ParentN;
      ParentR = original.ParentR;
      ParentNR = original.ParentNR;
      onCloned(this);
    }
    partial void onCloned(LookupChild clone);


    /// <summary>
    /// Constructor for LookupChild read from CSV file
    /// </summary>
    private LookupChild(int key, CsvReader csvReader){
      Key = key;
      Text = csvReader.ReadString();
      var lookupParentKey = csvReader.ReadInt();
      Parent = DC.Data._LookupParents.GetItem(lookupParentKey)??
        throw new Exception($"Read LookupChild from CSV file: Cannot find Parent with key {lookupParentKey}." + Environment.NewLine + 
          csvReader.PresentContent);
      var parentNKey = csvReader.ReadIntNull();
      if (parentNKey.HasValue) {
        ParentN = DC.Data._LookupParentNs.GetItem(parentNKey.Value)?? LookupParentN.NoLookupParentN;
      }
      var lookupParentRKey = csvReader.ReadInt();
      ParentR = DC.Data._LookupParentRs.GetItem(lookupParentRKey)??
        throw new Exception($"Read LookupChild from CSV file: Cannot find ParentR with key {lookupParentRKey}." + Environment.NewLine + 
          csvReader.PresentContent);
      var parentNRKey = csvReader.ReadIntNull();
      if (parentNRKey.HasValue) {
        ParentNR = DC.Data._LookupParentNRs.GetItem(parentNRKey.Value)?? LookupParentNR.NoLookupParentNR;
      }
      onCsvConstruct();
    }
    partial void onCsvConstruct();


    /// <summary>
    /// New LookupChild read from CSV file
    /// </summary>
    internal static LookupChild Create(int key, CsvReader csvReader) {
      return new LookupChild(key, csvReader);
    }


    /// <summary>
    /// Verify that lookupChild.Parent exists.
    /// Verify that lookupChild.ParentN exists.
    /// Verify that lookupChild.ParentR exists.
    /// Verify that lookupChild.ParentNR exists.
    /// </summary>
    internal static bool Verify(LookupChild lookupChild) {
      if (lookupChild.Parent==LookupParent.NoLookupParent) return false;
      if (lookupChild.ParentN==LookupParentN.NoLookupParentN) return false;
      if (lookupChild.ParentR==LookupParentR.NoLookupParentR) return false;
      if (lookupChild.ParentNR==LookupParentNR.NoLookupParentNR) return false;
      return true;
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Adds LookupChild to DC.Data.LookupChildren.<br/>
    /// Throws an Exception when LookupChild is already stored.
    /// </summary>
    public void Store() {
      if (Key>=0) {
        throw new Exception($"LookupChild cannot be stored again in DC.Data, key {Key} is greater equal 0." + Environment.NewLine + ToString());
      }

      var isCancelled = false;
      onStoring(ref isCancelled);
      if (isCancelled) return;

      if (Parent.Key<0) {
        throw new Exception($"Cannot store child LookupChild '{this}'.Parent to LookupParent '{Parent}' because parent is not stored yet.");
      }
      if (ParentN?.Key<0) {
        throw new Exception($"Cannot store child LookupChild '{this}'.ParentN to LookupParentN '{ParentN}' because parent is not stored yet.");
      }
      if (ParentR.Key<0) {
        throw new Exception($"Cannot store child LookupChild '{this}'.ParentR to LookupParentR '{ParentR}' because parent is not stored yet.");
      }
      if (ParentNR?.Key<0) {
        throw new Exception($"Cannot store child LookupChild '{this}'.ParentNR to LookupParentNR '{ParentNR}' because parent is not stored yet.");
      }
      DC.Data._LookupChildren.Add(this);
      onStored();
#if DEBUG
      DC.Trace?.Invoke($"Stored LookupChild #{GetHashCode()} @{Key}");
#endif
    }
    partial void onStoring(ref bool isCancelled);
    partial void onStored();


    /// <summary>
    /// Estimated number of UTF8 characters needed to write LookupChild to CSV file
    /// </summary>
    public const int EstimatedLineLength = 150;


    /// <summary>
    /// Write LookupChild to CSV file
    /// </summary>
    internal static void Write(LookupChild lookupChild, CsvWriter csvWriter) {
      lookupChild.onCsvWrite();
      csvWriter.Write(lookupChild.Text);
      if (lookupChild.Parent.Key<0) throw new Exception($"Cannot write lookupChild '{lookupChild}' to CSV File, because Parent is not stored in DC.Data.LookupParents.");

      csvWriter.Write(lookupChild.Parent.Key.ToString());
      if (lookupChild.ParentN is null) {
        csvWriter.WriteNull();
      } else {
        if (lookupChild.ParentN.Key<0) throw new Exception($"Cannot write lookupChild '{lookupChild}' to CSV File, because ParentN is not stored in DC.Data.LookupParentNs.");

        csvWriter.Write(lookupChild.ParentN.Key.ToString());
      }
      if (lookupChild.ParentR.Key<0) throw new Exception($"Cannot write lookupChild '{lookupChild}' to CSV File, because ParentR is not stored in DC.Data.LookupParentRs.");

      csvWriter.Write(lookupChild.ParentR.Key.ToString());
      if (lookupChild.ParentNR is null) {
        csvWriter.WriteNull();
      } else {
        if (lookupChild.ParentNR.Key<0) throw new Exception($"Cannot write lookupChild '{lookupChild}' to CSV File, because ParentNR is not stored in DC.Data.LookupParentNRs.");

        csvWriter.Write(lookupChild.ParentNR.Key.ToString());
      }
    }
    partial void onCsvWrite();


    /// <summary>
    /// Updates LookupChild with the provided values
    /// </summary>
    public void Update(string text, LookupParent parent, LookupParentN? parentN) {
      if (Key>=0){
        if (parent.Key<0) {
          throw new Exception($"LookupChild.Update(): It is illegal to add stored LookupChild '{this}'" + Environment.NewLine + 
            $"to Parent '{parent}', which is not stored.");
        }
        if (parentN?.Key<0) {
          throw new Exception($"LookupChild.Update(): It is illegal to add stored LookupChild '{this}'" + Environment.NewLine + 
            $"to ParentN '{parentN}', which is not stored.");
        }
      }
      var clone = new LookupChild(this);
      var isCancelled = false;
      onUpdating(text, parent, parentN, ref isCancelled);
      if (isCancelled) return;

#if DEBUG
      DC.Trace?.Invoke($"Updating LookupChild: {ToTraceString()}");
#endif

      //update properties and detect if any value has changed
      var isChangeDetected = false;
      if (Text!=text) {
        Text = text;
        isChangeDetected = true;
      }
      if (Parent!=parent) {
        Parent = parent;
        isChangeDetected = true;
      }
      if (ParentN!=parentN) {
        ParentN = parentN;
        isChangeDetected = true;
      }
      if (isChangeDetected) {
        onUpdated(clone);
        if (Key>=0) {
          DC.Data._LookupChildren.ItemHasChanged(clone, this);
        } else if (DC.Data.IsTransaction) {
          DC.Data.AddTransaction(new TransactionItem(4, TransactionActivityEnum.Update, Key, this, oldItem: clone));
        }
        HasChanged?.Invoke(clone, this);
      }
#if DEBUG
      DC.Trace?.Invoke($"Updated LookupChild: {ToTraceString()}");
#endif
    }
    partial void onUpdating(string text, LookupParent parent, LookupParentN? parentN, ref bool isCancelled);
    partial void onUpdated(LookupChild old);


    /// <summary>
    /// Updates this LookupChild with values from CSV file
    /// </summary>
    internal static void Update(LookupChild lookupChild, CsvReader csvReader){
      lookupChild.Text = csvReader.ReadString();
        var parent = DC.Data._LookupParents.GetItem(csvReader.ReadInt())??
          LookupParent.NoLookupParent;
      if (lookupChild.Parent!=parent) {
        lookupChild.Parent = parent;
      }
      var parentNKey = csvReader.ReadIntNull();
      LookupParentN? parentN;
      if (parentNKey is null) {
        parentN = null;
      } else {
        parentN = DC.Data._LookupParentNs.GetItem(parentNKey.Value)??
          LookupParentN.NoLookupParentN;
      }
      if (lookupChild.ParentN is null) {
        if (parentN is null) {
          //nothing to do
        } else {
          lookupChild.ParentN = parentN;
        }
      } else {
        if (parentN is null) {
          lookupChild.ParentN = null;
        } else {
          lookupChild.ParentN = parentN;
        }
      }
        var parentR = DC.Data._LookupParentRs.GetItem(csvReader.ReadInt())??
          LookupParentR.NoLookupParentR;
      if (lookupChild.ParentR!=parentR) {
        throw new Exception($"LookupChild.Update(): Property ParentR '{lookupChild.ParentR}' is " +
          $"readonly, parentR '{parentR}' read from the CSV file should be the same." + Environment.NewLine + 
          lookupChild.ToString());
      }
      var parentNRKey = csvReader.ReadIntNull();
      LookupParentNR? parentNR;
      if (parentNRKey is null) {
        parentNR = null;
      } else {
        parentNR = DC.Data._LookupParentNRs.GetItem(parentNRKey.Value)??
          LookupParentNR.NoLookupParentNR;
      }
      if (lookupChild.ParentNR!=parentNR) {
        throw new Exception($"LookupChild.Update(): Property ParentNR '{lookupChild.ParentNR}' is " +
          $"readonly, parentNR '{parentNR}' read from the CSV file should be the same." + Environment.NewLine + 
          lookupChild.ToString());
      }
      lookupChild.onCsvUpdate();
    }
    partial void onCsvUpdate();


    /// <summary>
    /// Removes LookupChild from DC.Data.LookupChildren.
    /// </summary>
    public void Release() {
      if (Key<0) {
        throw new Exception($"LookupChild.Release(): LookupChild '{this}' is not stored in DC.Data, key is {Key}.");
      }
      onReleased();
      DC.Data._LookupChildren.Remove(Key);
#if DEBUG
      DC.Trace?.Invoke($"Released LookupChild @{Key} #{GetHashCode()}");
#endif
    }
    partial void onReleased();


    /// <summary>
    /// Undoes the new() statement as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemNew(IStorageItem item) {
      var lookupChild = (LookupChild) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback new LookupChild(): {lookupChild.ToTraceString()}");
#endif
      lookupChild.onRollbackItemNew();
    }
    partial void onRollbackItemNew();


    /// <summary>
    /// Releases LookupChild from DC.Data.LookupChildren as part of a transaction rollback of Store().
    /// </summary>
    internal static void RollbackItemStore(IStorageItem item) {
      var lookupChild = (LookupChild) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback LookupChild.Store(): {lookupChild.ToTraceString()}");
#endif
      lookupChild.onRollbackItemStored();
    }
    partial void onRollbackItemStored();


    /// <summary>
    /// Restores the LookupChild item data as it was before the last update as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemUpdate(IStorageItem oldStorageItem, IStorageItem newStorageItem) {
      var oldItem = (LookupChild) oldStorageItem;//an item clone with the values before item was updated
      var item = (LookupChild) newStorageItem;//is the instance whose values should be restored
#if DEBUG
      DC.Trace?.Invoke($"Rolling back LookupChild.Update(): {item.ToTraceString()}");
#endif

      // if possible, throw exceptions before changing anything
      if (item.ParentR!=oldItem.ParentR) {
        throw new Exception($"LookupChild.Update(): Property ParentR '{item.ParentR}' is " +
          $"readonly, ParentR '{oldItem.ParentR}' should be the same." + Environment.NewLine + 
          item.ToString());
      }
      if (item.ParentNR!=oldItem.ParentNR) {
        throw new Exception($"LookupChild.Update(): Property ParentNR '{item.ParentNR}' is " +
          $"readonly, ParentNR '{oldItem.ParentNR}' should be the same." + Environment.NewLine + 
          item.ToString());
      }

      // updated item: restore old values
      item.Text = oldItem.Text;
      item.Parent = oldItem.Parent;
      item.ParentN = oldItem.ParentN;
      item.onRollbackItemUpdated(oldItem);
#if DEBUG
      DC.Trace?.Invoke($"Rolled back LookupChild.Update(): {item.ToTraceString()}");
#endif
    }
    partial void onRollbackItemUpdated(LookupChild oldLookupChild);


    /// <summary>
    /// Adds LookupChild to DC.Data.LookupChildren as part of a transaction rollback of Release().
    /// </summary>
    internal static void RollbackItemRelease(IStorageItem item) {
      var lookupChild = (LookupChild) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback LookupChild.Release(): {lookupChild.ToTraceString()}");
#endif
      lookupChild.onRollbackItemRelease();
    }
    partial void onRollbackItemRelease();


    /// <summary>
    /// Returns property values for tracing. Parents are shown with their key instead their content.
    /// </summary>
    public string ToTraceString() {
      var returnString =
        $"{this.GetKeyOrHash()}|" +
        $" {Text}|" +
        $" Parent {Parent.GetKeyOrHash()}|" +
        $" ParentN {ParentN?.GetKeyOrHash()}|" +
        $" ParentR {ParentR.GetKeyOrHash()}|" +
        $" ParentNR {ParentNR?.GetKeyOrHash()}";
      onToTraceString(ref returnString);
      return returnString;
    }
    partial void onToTraceString(ref string returnString);


    /// <summary>
    /// Returns property values
    /// </summary>
    public string ToShortString() {
      var returnString =
        $"{Key.ToKeyString()}," +
        $" {Text}," +
        $" {Parent.ToShortString()}," +
        $" {ParentN?.ToShortString()}," +
        $" {ParentR.ToShortString()}," +
        $" {ParentNR?.ToShortString()}";
      onToShortString(ref returnString);
      return returnString;
    }
    partial void onToShortString(ref string returnString);


    /// <summary>
    /// Returns all property names and values
    /// </summary>
    public override string ToString() {
      var returnString =
        $"Key: {Key.ToKeyString()}," +
        $" Text: {Text}," +
        $" Parent: {Parent.ToShortString()}," +
        $" ParentN: {ParentN?.ToShortString()}," +
        $" ParentR: {ParentR.ToShortString()}," +
        $" ParentNR: {ParentNR?.ToShortString()};";
      onToString(ref returnString);
      return returnString;
    }
    partial void onToString(ref string returnString);
    #endregion
  }
}
