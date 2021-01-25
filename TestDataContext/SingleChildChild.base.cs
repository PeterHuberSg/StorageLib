//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by StorageClassGenerator
//
//     Do not change code in this file, it will get lost when the file gets 
//     auto generated again. Write your code into SingleChildChild.cs.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using StorageLib;


namespace TestContext  {


  public partial class SingleChildChild: IStorageItemGeneric<SingleChildChild> {

    #region Properties
    //      ----------

    /// <summary>
    /// Unique identifier for SingleChildChild. Gets set once SingleChildChild gets added to DC.Data.
    /// </summary>
    public int Key { get; private set; }
    internal static void SetKey(IStorageItem singleChildChild, int key, bool isRollback) {
#if DEBUG
      if (isRollback) {
        if (key==StorageExtensions.NoKey) {
          DC.Trace?.Invoke($"Release SingleChildChild key @{singleChildChild.Key} #{singleChildChild.GetHashCode()}");
        } else {
          DC.Trace?.Invoke($"Store SingleChildChild key @{key} #{singleChildChild.GetHashCode()}");
        }
      }
#endif
      ((SingleChildChild)singleChildChild).Key = key;
    }


    public string Text { get; private set; }


    public SingleChildParent Parent { get; private set; }


    public SingleChildParentN? ParentN { get; private set; }


    public SingleChildParentR ParentR { get; }


    public SingleChildParentNR? ParentNR { get; }


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
    /// None existing SingleChildChild, used as a temporary place holder when reading a CSV file
    /// which was not compacted. It might create first a later deleted item linking to a 
    /// deleted parent. In this case, the parent property gets set to NoSingleChildChild. Once the CSV
    /// file is completely read, that child will actually be deleted (released) and Verify()
    /// ensures that there are no stored children with links to NoSingleChildChild.
    /// </summary>
    internal static SingleChildChild NoSingleChildChild = new SingleChildChild("NoText", SingleChildParent.NoSingleChildParent, null, SingleChildParentR.NoSingleChildParentR, null, isStoring: false);
    #endregion


    #region Events
    //      ------

    /// <summary>
    /// Content of SingleChildChild has changed. Gets only raised for changes occurring after loading DC.Data with previously stored data.
    /// </summary>
    public event Action</*old*/SingleChildChild, /*new*/SingleChildChild>? HasChanged;
    #endregion


    #region Constructors
    //      ------------

    /// <summary>
    /// SingleChildChild Constructor. If isStoring is true, adds SingleChildChild to DC.Data.SingleChildChildren.
    /// </summary>
    public SingleChildChild(
      string text, 
      SingleChildParent parent, 
      SingleChildParentN? parentN, 
      SingleChildParentR parentR, 
      SingleChildParentNR? parentNR, 
      bool isStoring = true)
    {
      Key = StorageExtensions.NoKey;
      Text = text;
      Parent = parent;
      ParentN = parentN;
      ParentR = parentR;
      ParentNR = parentNR;
#if DEBUG
      DC.Trace?.Invoke($"new SingleChildChild: {ToTraceString()}");
#endif
      Parent.AddToChild(this);
      if (ParentN!=null) {
        ParentN.AddToChild(this);
      }
      ParentR.AddToChild(this);
      if (ParentNR!=null) {
        ParentNR.AddToChild(this);
      }
      onConstruct();
      if (DC.Data.IsTransaction) {
        DC.Data.AddTransaction(new TransactionItem(9,TransactionActivityEnum.New, Key, this));
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
    public SingleChildChild(SingleChildChild original) {
    #pragma warning restore CS8618 //
      Key = StorageExtensions.NoKey;
      Text = original.Text;
      Parent = original.Parent;
      ParentN = original.ParentN;
      ParentR = original.ParentR;
      ParentNR = original.ParentNR;
      onCloned(this);
    }
    partial void onCloned(SingleChildChild clone);


    /// <summary>
    /// Constructor for SingleChildChild read from CSV file
    /// </summary>
    private SingleChildChild(int key, CsvReader csvReader){
      Key = key;
      Text = csvReader.ReadString();
      var singleChildParentKey = csvReader.ReadInt();
      Parent = DC.Data._SingleChildParents.GetItem(singleChildParentKey)?? SingleChildParent.NoSingleChildParent;
      var parentNKey = csvReader.ReadIntNull();
      if (parentNKey.HasValue) {
        ParentN = DC.Data._SingleChildParentNs.GetItem(parentNKey.Value)?? SingleChildParentN.NoSingleChildParentN;
      }
      var singleChildParentRKey = csvReader.ReadInt();
      ParentR = DC.Data._SingleChildParentRs.GetItem(singleChildParentRKey)?? SingleChildParentR.NoSingleChildParentR;
      var parentNRKey = csvReader.ReadIntNull();
      if (parentNRKey.HasValue) {
        ParentNR = DC.Data._SingleChildParentNRs.GetItem(parentNRKey.Value)?? SingleChildParentNR.NoSingleChildParentNR;
      }
      if (Parent!=SingleChildParent.NoSingleChildParent) {
        Parent.AddToChild(this);
      }
      if (parentNKey.HasValue && ParentN!=SingleChildParentN.NoSingleChildParentN) {
        ParentN!.AddToChild(this);
      }
      if (ParentR!=SingleChildParentR.NoSingleChildParentR) {
        ParentR.AddToChild(this);
      }
      if (parentNRKey.HasValue && ParentNR!=SingleChildParentNR.NoSingleChildParentNR) {
        ParentNR!.AddToChild(this);
      }
      onCsvConstruct();
    }
    partial void onCsvConstruct();


    /// <summary>
    /// New SingleChildChild read from CSV file
    /// </summary>
    internal static SingleChildChild Create(int key, CsvReader csvReader) {
      return new SingleChildChild(key, csvReader);
    }


    /// <summary>
    /// Verify that singleChildChild.Parent exists.
    /// Verify that singleChildChild.ParentN exists.
    /// Verify that singleChildChild.ParentR exists.
    /// Verify that singleChildChild.ParentNR exists.
    /// </summary>
    internal static bool Verify(SingleChildChild singleChildChild) {
      if (singleChildChild.Parent==SingleChildParent.NoSingleChildParent) return false;
      if (singleChildChild.ParentN==SingleChildParentN.NoSingleChildParentN) return false;
      if (singleChildChild.ParentR==SingleChildParentR.NoSingleChildParentR) return false;
      if (singleChildChild.ParentNR==SingleChildParentNR.NoSingleChildParentNR) return false;
      return true;
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Adds SingleChildChild to DC.Data.SingleChildChildren.<br/>
    /// Throws an Exception when SingleChildChild is already stored.
    /// </summary>
    public void Store() {
      if (Key>=0) {
        throw new Exception($"SingleChildChild cannot be stored again in DC.Data, key {Key} is greater equal 0." + Environment.NewLine + ToString());
      }

      var isCancelled = false;
      onStoring(ref isCancelled);
      if (isCancelled) return;

      if (Parent.Key<0) {
        throw new Exception($"Cannot store child SingleChildChild '{this}'.Parent to SingleChildParent '{Parent}' because parent is not stored yet.");
      }
      if (ParentN?.Key<0) {
        throw new Exception($"Cannot store child SingleChildChild '{this}'.ParentN to SingleChildParentN '{ParentN}' because parent is not stored yet.");
      }
      if (ParentR.Key<0) {
        throw new Exception($"Cannot store child SingleChildChild '{this}'.ParentR to SingleChildParentR '{ParentR}' because parent is not stored yet.");
      }
      if (ParentNR?.Key<0) {
        throw new Exception($"Cannot store child SingleChildChild '{this}'.ParentNR to SingleChildParentNR '{ParentNR}' because parent is not stored yet.");
      }
      DC.Data._SingleChildChildren.Add(this);
      onStored();
#if DEBUG
      DC.Trace?.Invoke($"Stored SingleChildChild #{GetHashCode()} @{Key}");
#endif
    }
    partial void onStoring(ref bool isCancelled);
    partial void onStored();


    /// <summary>
    /// Estimated number of UTF8 characters needed to write SingleChildChild to CSV file
    /// </summary>
    public const int EstimatedLineLength = 150;


    /// <summary>
    /// Write SingleChildChild to CSV file
    /// </summary>
    internal static void Write(SingleChildChild singleChildChild, CsvWriter csvWriter) {
      singleChildChild.onCsvWrite();
      csvWriter.Write(singleChildChild.Text);
      if (singleChildChild.Parent.Key<0) throw new Exception($"Cannot write singleChildChild '{singleChildChild}' to CSV File, because Parent is not stored in DC.Data.SingleChildParents.");

      csvWriter.Write(singleChildChild.Parent.Key.ToString());
      if (singleChildChild.ParentN is null) {
        csvWriter.WriteNull();
      } else {
        if (singleChildChild.ParentN.Key<0) throw new Exception($"Cannot write singleChildChild '{singleChildChild}' to CSV File, because ParentN is not stored in DC.Data.SingleChildParentNs.");

        csvWriter.Write(singleChildChild.ParentN.Key.ToString());
      }
      if (singleChildChild.ParentR.Key<0) throw new Exception($"Cannot write singleChildChild '{singleChildChild}' to CSV File, because ParentR is not stored in DC.Data.SingleChildParentRs.");

      csvWriter.Write(singleChildChild.ParentR.Key.ToString());
      if (singleChildChild.ParentNR is null) {
        csvWriter.WriteNull();
      } else {
        if (singleChildChild.ParentNR.Key<0) throw new Exception($"Cannot write singleChildChild '{singleChildChild}' to CSV File, because ParentNR is not stored in DC.Data.SingleChildParentNRs.");

        csvWriter.Write(singleChildChild.ParentNR.Key.ToString());
      }
    }
    partial void onCsvWrite();


    /// <summary>
    /// Updates SingleChildChild with the provided values
    /// </summary>
    public void Update(string text, SingleChildParent parent, SingleChildParentN? parentN) {
      if (Key>=0){
        if (parent.Key<0) {
          throw new Exception($"SingleChildChild.Update(): It is illegal to add stored SingleChildChild '{this}'" + Environment.NewLine + 
            $"to Parent '{parent}', which is not stored.");
        }
        if (parentN?.Key<0) {
          throw new Exception($"SingleChildChild.Update(): It is illegal to add stored SingleChildChild '{this}'" + Environment.NewLine + 
            $"to ParentN '{parentN}', which is not stored.");
        }
      }
      var clone = new SingleChildChild(this);
      var isCancelled = false;
      onUpdating(text, parent, parentN, ref isCancelled);
      if (isCancelled) return;

#if DEBUG
      DC.Trace?.Invoke($"Updating SingleChildChild: {ToTraceString()}");
#endif

      //remove not yet updated item from parents which will be removed by update
      var hasParentChanged = Parent!=parent;
      if (hasParentChanged) {
        Parent.RemoveFromChild(this);
      }
      var hasParentNChanged = ParentN!=parentN;
      if (ParentN is not null && hasParentNChanged) {
        ParentN.RemoveFromChild(this);
      }

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

      //add updated item to parents which have been newly added during update
      if (hasParentChanged) {
        Parent.AddToChild(this);
      }
      if (ParentN is not null && hasParentNChanged) {
        ParentN.AddToChild(this);
      }
      if (isChangeDetected) {
        onUpdated(clone);
        if (Key>=0) {
          DC.Data._SingleChildChildren.ItemHasChanged(clone, this);
        } else if (DC.Data.IsTransaction) {
          DC.Data.AddTransaction(new TransactionItem(9, TransactionActivityEnum.Update, Key, this, oldItem: clone));
        }
        HasChanged?.Invoke(clone, this);
      }
#if DEBUG
      DC.Trace?.Invoke($"Updated SingleChildChild: {ToTraceString()}");
#endif
    }
    partial void onUpdating(string text, SingleChildParent parent, SingleChildParentN? parentN, ref bool isCancelled);
    partial void onUpdated(SingleChildChild old);


    /// <summary>
    /// Updates this SingleChildChild with values from CSV file
    /// </summary>
    internal static void Update(SingleChildChild singleChildChild, CsvReader csvReader){
      singleChildChild.Text = csvReader.ReadString();
        var parent = DC.Data._SingleChildParents.GetItem(csvReader.ReadInt())??
          SingleChildParent.NoSingleChildParent;
      if (singleChildChild.Parent!=parent) {
        if (singleChildChild.Parent!=SingleChildParent.NoSingleChildParent) {
          singleChildChild.Parent.RemoveFromChild(singleChildChild);
        }
        singleChildChild.Parent = parent;
        singleChildChild.Parent.AddToChild(singleChildChild);
      }
      var parentNKey = csvReader.ReadIntNull();
      SingleChildParentN? parentN;
      if (parentNKey is null) {
        parentN = null;
      } else {
        parentN = DC.Data._SingleChildParentNs.GetItem(parentNKey.Value)??
          SingleChildParentN.NoSingleChildParentN;
      }
      if (singleChildChild.ParentN is null) {
        if (parentN is null) {
          //nothing to do
        } else {
          singleChildChild.ParentN = parentN;
          singleChildChild.ParentN.AddToChild(singleChildChild);
        }
      } else {
        if (parentN is null) {
          if (singleChildChild.ParentN!=SingleChildParentN.NoSingleChildParentN) {
            singleChildChild.ParentN.RemoveFromChild(singleChildChild);
          }
          singleChildChild.ParentN = null;
        } else {
          if (singleChildChild.ParentN!=SingleChildParentN.NoSingleChildParentN) {
            singleChildChild.ParentN.RemoveFromChild(singleChildChild);
          }
          singleChildChild.ParentN = parentN;
          singleChildChild.ParentN.AddToChild(singleChildChild);
        }
      }
        var parentR = DC.Data._SingleChildParentRs.GetItem(csvReader.ReadInt())??
          SingleChildParentR.NoSingleChildParentR;
      if (singleChildChild.ParentR!=parentR) {
        throw new Exception($"SingleChildChild.Update(): Property ParentR '{singleChildChild.ParentR}' is " +
          $"readonly, parentR '{parentR}' read from the CSV file should be the same." + Environment.NewLine + 
          singleChildChild.ToString());
      }
      var parentNRKey = csvReader.ReadIntNull();
      SingleChildParentNR? parentNR;
      if (parentNRKey is null) {
        parentNR = null;
      } else {
        parentNR = DC.Data._SingleChildParentNRs.GetItem(parentNRKey.Value)??
          SingleChildParentNR.NoSingleChildParentNR;
      }
      if (singleChildChild.ParentNR!=parentNR) {
        throw new Exception($"SingleChildChild.Update(): Property ParentNR '{singleChildChild.ParentNR}' is " +
          $"readonly, parentNR '{parentNR}' read from the CSV file should be the same." + Environment.NewLine + 
          singleChildChild.ToString());
      }
      singleChildChild.onCsvUpdate();
    }
    partial void onCsvUpdate();


    /// <summary>
    /// Removes SingleChildChild from DC.Data.SingleChildChildren.
    /// </summary>
    public void Release() {
      if (Key<0) {
        throw new Exception($"SingleChildChild.Release(): SingleChildChild '{this}' is not stored in DC.Data, key is {Key}.");
      }
      DC.Data._SingleChildChildren.Remove(Key);
      onReleased();
#if DEBUG
      DC.Trace?.Invoke($"Released SingleChildChild @{Key} #{GetHashCode()}");
#endif
    }
    partial void onReleased();


    /// <summary>
    /// Removes SingleChildChild from parents as part of a transaction rollback of the new() statement.
    /// </summary>
    internal static void RollbackItemNew(IStorageItem item) {
      var singleChildChild = (SingleChildChild) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback new SingleChildChild(): {singleChildChild.ToTraceString()}");
#endif
      if (singleChildChild.Parent!=SingleChildParent.NoSingleChildParent) {
        singleChildChild.Parent.RemoveFromChild(singleChildChild);
      }
      if (singleChildChild.ParentN!=null && singleChildChild.ParentN!=SingleChildParentN.NoSingleChildParentN) {
        singleChildChild.ParentN.RemoveFromChild(singleChildChild);
      }
      if (singleChildChild.ParentR!=SingleChildParentR.NoSingleChildParentR) {
        singleChildChild.ParentR.RemoveFromChild(singleChildChild);
      }
      if (singleChildChild.ParentNR!=null && singleChildChild.ParentNR!=SingleChildParentNR.NoSingleChildParentNR) {
        singleChildChild.ParentNR.RemoveFromChild(singleChildChild);
      }
      singleChildChild.onRollbackItemNew();
    }
    partial void onRollbackItemNew();


    /// <summary>
    /// Releases SingleChildChild from DC.Data.SingleChildChildren as part of a transaction rollback of Store().
    /// </summary>
    internal static void RollbackItemStore(IStorageItem item) {
      var singleChildChild = (SingleChildChild) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback SingleChildChild.Store(): {singleChildChild.ToTraceString()}");
#endif
      singleChildChild.onRollbackItemStored();
    }
    partial void onRollbackItemStored();


    /// <summary>
    /// Restores the SingleChildChild item data as it was before the last update as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemUpdate(IStorageItem oldStorageItem, IStorageItem newStorageItem) {
      var oldItem = (SingleChildChild) oldStorageItem;//an item clone with the values before item was updated
      var item = (SingleChildChild) newStorageItem;//is the instance whose values should be restored
#if DEBUG
      DC.Trace?.Invoke($"Rolling back SingleChildChild.Update(): {item.ToTraceString()}");
#endif

      // if possible, throw exceptions before changing anything
      if (item.ParentR!=oldItem.ParentR) {
        throw new Exception($"SingleChildChild.Update(): Property ParentR '{item.ParentR}' is " +
          $"readonly, ParentR '{oldItem.ParentR}' should be the same." + Environment.NewLine + 
          item.ToString());
      }
      if (item.ParentNR!=oldItem.ParentNR) {
        throw new Exception($"SingleChildChild.Update(): Property ParentNR '{item.ParentNR}' is " +
          $"readonly, ParentNR '{oldItem.ParentNR}' should be the same." + Environment.NewLine + 
          item.ToString());
      }

      // remove updated item from parents
      var hasParentChanged = oldItem.Parent!=item.Parent;
      if (hasParentChanged) {
        item.Parent.RemoveFromChild(item);
      }
      var hasParentNChanged = oldItem.ParentN!=item.ParentN;
      if (hasParentNChanged && item.ParentN is not null) {
        item.ParentN.RemoveFromChild(item);
      }

      // updated item: restore old values
      item.Text = oldItem.Text;
      item.Parent = oldItem.Parent;
      item.ParentN = oldItem.ParentN;

      // add item with previous values to parents
      if (hasParentChanged) {
        item.Parent.AddToChild(item);
      }
      if (hasParentNChanged && item.ParentN is not null) {
        item.ParentN.AddToChild(item);
      }
      item.onRollbackItemUpdated(oldItem);
#if DEBUG
      DC.Trace?.Invoke($"Rolled back SingleChildChild.Update(): {item.ToTraceString()}");
#endif
    }
    partial void onRollbackItemUpdated(SingleChildChild oldSingleChildChild);


    /// <summary>
    /// Adds SingleChildChild to DC.Data.SingleChildChildren as part of a transaction rollback of Release().
    /// </summary>
    internal static void RollbackItemRelease(IStorageItem item) {
      var singleChildChild = (SingleChildChild) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback SingleChildChild.Release(): {singleChildChild.ToTraceString()}");
#endif
      singleChildChild.onRollbackItemRelease();
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
