//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by StorageClassGenerator
//
//     Do not change code in this file, it will get lost when the file gets 
//     auto generated again. Write your code into SortedListParentN.cs.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using StorageLib;


namespace StorageDataContext  {


  public partial class SortedListParentN: IStorageItemGeneric<SortedListParentN> {

    #region Properties
    //      ----------

    /// <summary>
    /// Unique identifier for SortedListParentN. Gets set once SortedListParentN gets added to DC.Data.
    /// </summary>
    public int Key { get; private set; }
    internal static void SetKey(IStorageItem sortedListParentN, int key, bool isRollback) {
#if DEBUG
      if (isRollback) {
        if (key==StorageExtensions.NoKey) {
          DC.Trace?.Invoke($"Release SortedListParentN key @{sortedListParentN.Key} #{sortedListParentN.GetHashCode()}");
        } else {
          DC.Trace?.Invoke($"Store SortedListParentN key @{key} #{sortedListParentN.GetHashCode()}");
        }
      }
#endif
      ((SortedListParentN)sortedListParentN).Key = key;
    }


    public string Text { get; private set; }


    public IReadOnlyDictionary<string, SortedListChild> SortedListChidren => sortedListChidren;
    readonly SortedList<string, SortedListChild> sortedListChidren;


    /// <summary>
    /// Headers written to first line in CSV file
    /// </summary>
    internal static readonly string[] Headers = {"Key", "Text"};


    /// <summary>
    /// None existing SortedListParentN
    /// </summary>
    internal static SortedListParentN NoSortedListParentN = new SortedListParentN("NoText", isStoring: false);
    #endregion


    #region Events
    //      ------

    /// <summary>
    /// Content of SortedListParentN has changed. Gets only raised for changes occurring after loading DC.Data with previously stored data.
    /// </summary>
    public event Action</*old*/SortedListParentN, /*new*/SortedListParentN>? HasChanged;
    #endregion


    #region Constructors
    //      ------------

    /// <summary>
    /// SortedListParentN Constructor. If isStoring is true, adds SortedListParentN to DC.Data.SortedListParentNs.
    /// </summary>
    public SortedListParentN(string text, bool isStoring = true) {
      Key = StorageExtensions.NoKey;
      Text = text;
      sortedListChidren = new SortedList<string, SortedListChild>();
#if DEBUG
      DC.Trace?.Invoke($"new SortedListParentN: {ToTraceString()}");
#endif
      onConstruct();
      if (DC.Data.IsTransaction) {
        DC.Data.AddTransaction(new TransactionItem(21,TransactionActivityEnum.New, Key, this));
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
    public SortedListParentN(SortedListParentN original) {
    #pragma warning restore CS8618 //
      Key = StorageExtensions.NoKey;
      Text = original.Text;
      onCloned(this);
    }
    partial void onCloned(SortedListParentN clone);


    /// <summary>
    /// Constructor for SortedListParentN read from CSV file
    /// </summary>
    private SortedListParentN(int key, CsvReader csvReader){
      Key = key;
      Text = csvReader.ReadString();
      sortedListChidren = new SortedList<string, SortedListChild>();
      onCsvConstruct();
    }
    partial void onCsvConstruct();


    /// <summary>
    /// New SortedListParentN read from CSV file
    /// </summary>
    internal static SortedListParentN Create(int key, CsvReader csvReader) {
      return new SortedListParentN(key, csvReader);
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Adds SortedListParentN to DC.Data.SortedListParentNs.<br/>
    /// Throws an Exception when SortedListParentN is already stored.
    /// </summary>
    public void Store() {
      if (Key>=0) {
        throw new Exception($"SortedListParentN cannot be stored again in DC.Data, key {Key} is greater equal 0." + Environment.NewLine + ToString());
      }

      var isCancelled = false;
      onStoring(ref isCancelled);
      if (isCancelled) return;

      DC.Data._SortedListParentNs.Add(this);
      onStored();
#if DEBUG
      DC.Trace?.Invoke($"Stored SortedListParentN #{GetHashCode()} @{Key}");
#endif
    }
    partial void onStoring(ref bool isCancelled);
    partial void onStored();


    /// <summary>
    /// Estimated number of UTF8 characters needed to write SortedListParentN to CSV file
    /// </summary>
    public const int EstimatedLineLength = 150;


    /// <summary>
    /// Write SortedListParentN to CSV file
    /// </summary>
    internal static void Write(SortedListParentN sortedListParentN, CsvWriter csvWriter) {
      sortedListParentN.onCsvWrite();
      csvWriter.Write(sortedListParentN.Text);
    }
    partial void onCsvWrite();


    /// <summary>
    /// Updates SortedListParentN with the provided values
    /// </summary>
    public void Update(string text) {
      var clone = new SortedListParentN(this);
      var isCancelled = false;
      onUpdating(text, ref isCancelled);
      if (isCancelled) return;

#if DEBUG
      DC.Trace?.Invoke($"Updating SortedListParentN: {ToTraceString()}");
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
          DC.Data._SortedListParentNs.ItemHasChanged(clone, this);
        } else if (DC.Data.IsTransaction) {
          DC.Data.AddTransaction(new TransactionItem(21, TransactionActivityEnum.Update, Key, this, oldItem: clone));
        }
        HasChanged?.Invoke(clone, this);
      }
#if DEBUG
      DC.Trace?.Invoke($"Updated SortedListParentN: {ToTraceString()}");
#endif
    }
    partial void onUpdating(string text, ref bool isCancelled);
    partial void onUpdated(SortedListParentN old);


    /// <summary>
    /// Updates this SortedListParentN with values from CSV file
    /// </summary>
    internal static void Update(SortedListParentN sortedListParentN, CsvReader csvReader){
      sortedListParentN.Text = csvReader.ReadString();
      sortedListParentN.onCsvUpdate();
    }
    partial void onCsvUpdate();


    /// <summary>
    /// Add sortedListChild to SortedListChidren.
    /// </summary>
    internal void AddToSortedListChidren(SortedListChild sortedListChild) {
#if DEBUG
      if (sortedListChild==SortedListChild.NoSortedListChild) throw new Exception();
      if ((sortedListChild.Key>=0)&&(Key<0)) throw new Exception();
      if (sortedListChidren.ContainsKey(sortedListChild.Text)) throw new Exception();
#endif
      sortedListChidren.Add(sortedListChild.Text, sortedListChild);
      onAddedToSortedListChidren(sortedListChild);
#if DEBUG
      DC.Trace?.Invoke($"Add SortedListChild {sortedListChild.GetKeyOrHash()} to " +
        $"{this.GetKeyOrHash()} SortedListParentN.SortedListChidren");
#endif
    }
    partial void onAddedToSortedListChidren(SortedListChild sortedListChild);


    /// <summary>
    /// Removes sortedListChild from SortedListParentN.
    /// </summary>
    internal void RemoveFromSortedListChidren(SortedListChild sortedListChild) {
#if DEBUG
      if (!sortedListChidren.Remove(sortedListChild.Text)) throw new Exception();
#else
        sortedListChidren.Remove(sortedListChild.Text);
#endif
      onRemovedFromSortedListChidren(sortedListChild);
#if DEBUG
      DC.Trace?.Invoke($"Remove SortedListChild {sortedListChild.GetKeyOrHash()} from " +
        $"{this.GetKeyOrHash()} SortedListParentN.SortedListChidren");
#endif
    }
    partial void onRemovedFromSortedListChidren(SortedListChild sortedListChild);


    /// <summary>
    /// Removes SortedListParentN from DC.Data.SortedListParentNs.
    /// </summary>
    public void Release() {
      if (Key<0) {
        throw new Exception($"SortedListParentN.Release(): SortedListParentN '{this}' is not stored in DC.Data, key is {Key}.");
      }
      foreach (var sortedListChild in SortedListChidren.Values) {
        if (sortedListChild?.Key>=0) {
          throw new Exception($"Cannot release SortedListParentN '{this}' " + Environment.NewLine + 
            $"because '{sortedListChild}' in SortedListParentN.SortedListChidren is still stored.");
        }
      }
      onReleased();
      DC.Data._SortedListParentNs.Remove(Key);
#if DEBUG
      DC.Trace?.Invoke($"Released SortedListParentN @{Key} #{GetHashCode()}");
#endif
    }
    partial void onReleased();


    /// <summary>
    /// Undoes the new() statement as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemNew(IStorageItem item) {
      var sortedListParentN = (SortedListParentN) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback new SortedListParentN(): {sortedListParentN.ToTraceString()}");
#endif
      sortedListParentN.onRollbackItemNew();
    }
    partial void onRollbackItemNew();


    /// <summary>
    /// Releases SortedListParentN from DC.Data.SortedListParentNs as part of a transaction rollback of Store().
    /// </summary>
    internal static void RollbackItemStore(IStorageItem item) {
      var sortedListParentN = (SortedListParentN) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback SortedListParentN.Store(): {sortedListParentN.ToTraceString()}");
#endif
      sortedListParentN.onRollbackItemStored();
    }
    partial void onRollbackItemStored();


    /// <summary>
    /// Restores the SortedListParentN item data as it was before the last update as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemUpdate(IStorageItem oldStorageItem, IStorageItem newStorageItem) {
      var oldItem = (SortedListParentN) oldStorageItem;//an item clone with the values before item was updated
      var item = (SortedListParentN) newStorageItem;//is the instance whose values should be restored
#if DEBUG
      DC.Trace?.Invoke($"Rolling back SortedListParentN.Update(): {item.ToTraceString()}");
#endif

      // updated item: restore old values
      item.Text = oldItem.Text;
      item.onRollbackItemUpdated(oldItem);
#if DEBUG
      DC.Trace?.Invoke($"Rolled back SortedListParentN.Update(): {item.ToTraceString()}");
#endif
    }
    partial void onRollbackItemUpdated(SortedListParentN oldSortedListParentN);


    /// <summary>
    /// Adds SortedListParentN to DC.Data.SortedListParentNs as part of a transaction rollback of Release().
    /// </summary>
    internal static void RollbackItemRelease(IStorageItem item) {
      var sortedListParentN = (SortedListParentN) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback SortedListParentN.Release(): {sortedListParentN.ToTraceString()}");
#endif
      sortedListParentN.onRollbackItemRelease();
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
        $" SortedListChidren: {SortedListChidren.Count};";
      onToString(ref returnString);
      return returnString;
    }
    partial void onToString(ref string returnString);
    #endregion
  }
}
