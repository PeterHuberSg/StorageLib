//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by StorageClassGenerator
//
//     Do not change code in this file, it will get lost when the file gets 
//     auto generated again. Write your code into SingleChildParentN.cs.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using StorageLib;


namespace TestContext  {


  public partial class SingleChildParentN: IStorageItemGeneric<SingleChildParentN> {

    #region Properties
    //      ----------

    /// <summary>
    /// Unique identifier for SingleChildParentN. Gets set once SingleChildParentN gets added to DC.Data.
    /// </summary>
    public int Key { get; private set; }
    internal static void SetKey(IStorageItem singleChildParentN, int key, bool isRollback) {
#if DEBUG
      if (isRollback) {
        if (key==StorageExtensions.NoKey) {
          DC.Trace?.Invoke($"Release SingleChildParentN key @{singleChildParentN.Key} #{singleChildParentN.GetHashCode()}");
        } else {
          DC.Trace?.Invoke($"Store SingleChildParentN key @{key} #{singleChildParentN.GetHashCode()}");
        }
      }
#endif
      ((SingleChildParentN)singleChildParentN).Key = key;
    }


    public string Text { get; private set; }


    public SingleChildChild? Child { get; private set; }


    /// <summary>
    /// Headers written to first line in CSV file
    /// </summary>
    internal static readonly string[] Headers = {"Key", "Text"};


    /// <summary>
    /// None existing SingleChildParentN
    /// </summary>
    internal static SingleChildParentN NoSingleChildParentN = new SingleChildParentN("NoText", isStoring: false);
    #endregion


    #region Events
    //      ------

    /// <summary>
    /// Content of SingleChildParentN has changed. Gets only raised for changes occurring after loading DC.Data with previously stored data.
    /// </summary>
    public event Action</*old*/SingleChildParentN, /*new*/SingleChildParentN>? HasChanged;
    #endregion


    #region Constructors
    //      ------------

    /// <summary>
    /// SingleChildParentN Constructor. If isStoring is true, adds SingleChildParentN to DC.Data.SingleChildParentNs.
    /// </summary>
    public SingleChildParentN(string text, bool isStoring = true) {
      Key = StorageExtensions.NoKey;
      Text = text;
#if DEBUG
      DC.Trace?.Invoke($"new SingleChildParentN: {ToTraceString()}");
#endif
      onConstruct();
      if (DC.Data.IsTransaction) {
        DC.Data.AddTransaction(new TransactionItem(6,TransactionActivityEnum.New, Key, this));
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
    public SingleChildParentN(SingleChildParentN original) {
    #pragma warning restore CS8618 //
      Key = StorageExtensions.NoKey;
      Text = original.Text;
      onCloned(this);
    }
    partial void onCloned(SingleChildParentN clone);


    /// <summary>
    /// Constructor for SingleChildParentN read from CSV file
    /// </summary>
    private SingleChildParentN(int key, CsvReader csvReader){
      Key = key;
      Text = csvReader.ReadString();
      onCsvConstruct();
    }
    partial void onCsvConstruct();


    /// <summary>
    /// New SingleChildParentN read from CSV file
    /// </summary>
    internal static SingleChildParentN Create(int key, CsvReader csvReader) {
      return new SingleChildParentN(key, csvReader);
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Adds SingleChildParentN to DC.Data.SingleChildParentNs.<br/>
    /// Throws an Exception when SingleChildParentN is already stored.
    /// </summary>
    public void Store() {
      if (Key>=0) {
        throw new Exception($"SingleChildParentN cannot be stored again in DC.Data, key {Key} is greater equal 0." + Environment.NewLine + ToString());
      }

      var isCancelled = false;
      onStoring(ref isCancelled);
      if (isCancelled) return;

      DC.Data._SingleChildParentNs.Add(this);
      onStored();
#if DEBUG
      DC.Trace?.Invoke($"Stored SingleChildParentN #{GetHashCode()} @{Key}");
#endif
    }
    partial void onStoring(ref bool isCancelled);
    partial void onStored();


    /// <summary>
    /// Estimated number of UTF8 characters needed to write SingleChildParentN to CSV file
    /// </summary>
    public const int EstimatedLineLength = 150;


    /// <summary>
    /// Write SingleChildParentN to CSV file
    /// </summary>
    internal static void Write(SingleChildParentN singleChildParentN, CsvWriter csvWriter) {
      singleChildParentN.onCsvWrite();
      csvWriter.Write(singleChildParentN.Text);
    }
    partial void onCsvWrite();


    /// <summary>
    /// Updates SingleChildParentN with the provided values
    /// </summary>
    public void Update(string text) {
      var clone = new SingleChildParentN(this);
      var isCancelled = false;
      onUpdating(text, ref isCancelled);
      if (isCancelled) return;

#if DEBUG
      DC.Trace?.Invoke($"Updating SingleChildParentN: {ToTraceString()}");
#endif

      //update properties and detect if any value has changed
      var isChangeDetected = false;
      if (Text!=text) {
        Text = text;
        isChangeDetected = true;
      }
      if (isChangeDetected) {
        onUpdated(clone);
        if (Key>=0) {
          DC.Data._SingleChildParentNs.ItemHasChanged(clone, this);
        } else if (DC.Data.IsTransaction) {
          DC.Data.AddTransaction(new TransactionItem(6, TransactionActivityEnum.Update, Key, this, oldItem: clone));
        }
        HasChanged?.Invoke(clone, this);
      }
#if DEBUG
      DC.Trace?.Invoke($"Updated SingleChildParentN: {ToTraceString()}");
#endif
    }
    partial void onUpdating(string text, ref bool isCancelled);
    partial void onUpdated(SingleChildParentN old);


    /// <summary>
    /// Updates this SingleChildParentN with values from CSV file
    /// </summary>
    internal static void Update(SingleChildParentN singleChildParentN, CsvReader csvReader){
      singleChildParentN.Text = csvReader.ReadString();
      singleChildParentN.onCsvUpdate();
    }
    partial void onCsvUpdate();


    /// <summary>
    /// Add singleChildChild to Child.
    /// </summary>
    internal void AddToChild(SingleChildChild singleChildChild) {
#if DEBUG
      if (singleChildChild==SingleChildChild.NoSingleChildChild) throw new Exception();
      if ((singleChildChild.Key>=0)&&(Key<0)) throw new Exception();
      if(Child==singleChildChild) throw new Exception();
#endif
      if (Child!=null) {
        throw new Exception($"SingleChildParentN.AddToChild(): '{Child}' is already assigned to Child, it is not possible to assign now '{singleChildChild}'.");
      }
      Child = singleChildChild;
      onAddedToChild(singleChildChild);
#if DEBUG
      DC.Trace?.Invoke($"Add SingleChildChild {singleChildChild.GetKeyOrHash()} to " +
        $"{this.GetKeyOrHash()} SingleChildParentN.Child");
#endif
    }
    partial void onAddedToChild(SingleChildChild singleChildChild);


    /// <summary>
    /// Removes singleChildChild from SingleChildParentN.
    /// </summary>
    internal void RemoveFromChild(SingleChildChild singleChildChild) {
#if DEBUG
      if (Child!=singleChildChild) {
        throw new Exception($"SingleChildParentN.RemoveFromChild(): Child does not link to singleChildChild '{singleChildChild}' but '{Child}'.");
      }
#endif
      Child = null;
      onRemovedFromChild(singleChildChild);
#if DEBUG
      DC.Trace?.Invoke($"Remove SingleChildChild {singleChildChild.GetKeyOrHash()} from " +
        $"{this.GetKeyOrHash()} SingleChildParentN.Child");
#endif
    }
    partial void onRemovedFromChild(SingleChildChild singleChildChild);


    /// <summary>
    /// Removes SingleChildParentN from DC.Data.SingleChildParentNs.
    /// </summary>
    public void Release() {
      if (Key<0) {
        throw new Exception($"SingleChildParentN.Release(): SingleChildParentN '{this}' is not stored in DC.Data, key is {Key}.");
      }
      if (Child?.Key>=0) {
        throw new Exception($"Cannot release SingleChildParentN '{this}' " + Environment.NewLine + 
          $"because '{Child}' in SingleChildParentN.Child is still stored.");
      }
      onReleased();
      DC.Data._SingleChildParentNs.Remove(Key);
#if DEBUG
      DC.Trace?.Invoke($"Released SingleChildParentN @{Key} #{GetHashCode()}");
#endif
    }
    partial void onReleased();


    /// <summary>
    /// Undoes the new() statement as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemNew(IStorageItem item) {
      var singleChildParentN = (SingleChildParentN) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback new SingleChildParentN(): {singleChildParentN.ToTraceString()}");
#endif
      singleChildParentN.onRollbackItemNew();
    }
    partial void onRollbackItemNew();


    /// <summary>
    /// Releases SingleChildParentN from DC.Data.SingleChildParentNs as part of a transaction rollback of Store().
    /// </summary>
    internal static void RollbackItemStore(IStorageItem item) {
      var singleChildParentN = (SingleChildParentN) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback SingleChildParentN.Store(): {singleChildParentN.ToTraceString()}");
#endif
      singleChildParentN.onRollbackItemStored();
    }
    partial void onRollbackItemStored();


    /// <summary>
    /// Restores the SingleChildParentN item data as it was before the last update as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemUpdate(IStorageItem oldStorageItem, IStorageItem newStorageItem) {
      var oldItem = (SingleChildParentN) oldStorageItem;//an item clone with the values before item was updated
      var item = (SingleChildParentN) newStorageItem;//is the instance whose values should be restored
#if DEBUG
      DC.Trace?.Invoke($"Rolling back SingleChildParentN.Update(): {item.ToTraceString()}");
#endif

      // updated item: restore old values
      item.Text = oldItem.Text;
      item.onRollbackItemUpdated(oldItem);
#if DEBUG
      DC.Trace?.Invoke($"Rolled back SingleChildParentN.Update(): {item.ToTraceString()}");
#endif
    }
    partial void onRollbackItemUpdated(SingleChildParentN oldSingleChildParentN);


    /// <summary>
    /// Adds SingleChildParentN to DC.Data.SingleChildParentNs as part of a transaction rollback of Release().
    /// </summary>
    internal static void RollbackItemRelease(IStorageItem item) {
      var singleChildParentN = (SingleChildParentN) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback SingleChildParentN.Release(): {singleChildParentN.ToTraceString()}");
#endif
      singleChildParentN.onRollbackItemRelease();
    }
    partial void onRollbackItemRelease();


    /// <summary>
    /// Returns property values for tracing. Parents are shown with their key instead their content.
    /// </summary>
    public string ToTraceString() {
      var returnString =
        $"{this.GetKeyOrHash()}|" +
        $" {Text}";
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
        $" {Text}";
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
        $" Child: {Child?.ToShortString()};";
      onToString(ref returnString);
      return returnString;
    }
    partial void onToString(ref string returnString);
    #endregion
  }
}
