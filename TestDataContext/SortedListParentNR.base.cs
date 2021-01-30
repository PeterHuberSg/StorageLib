//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by StorageClassGenerator
//
//     Do not change code in this file, it will get lost when the file gets 
//     auto generated again. Write your code into SortedListParentNR.cs.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using StorageLib;


namespace TestContext  {


  public partial class SortedListParentNR: IStorageItem<SortedListParentNR> {

    #region Properties
    //      ----------

    /// <summary>
    /// Unique identifier for SortedListParentNR. Gets set once SortedListParentNR gets added to DC.Data.
    /// </summary>
    public int Key { get; private set; }
    internal static void SetKey(IStorageItem sortedListParentNR, int key, bool isRollback) {
#if DEBUG
      if (isRollback) {
        if (key==StorageExtensions.NoKey) {
          DC.Trace?.Invoke($"Release SortedListParentNR key @{sortedListParentNR.Key} #{sortedListParentNR.GetHashCode()}");
        } else {
          DC.Trace?.Invoke($"Store SortedListParentNR key @{key} #{sortedListParentNR.GetHashCode()}");
        }
      }
#endif
      ((SortedListParentNR)sortedListParentNR).Key = key;
    }


    public string Text { get; private set; }


    public IStorageReadOnlyDictionary<string, SortedListChild> SortedListChildren => sortedListChildren;
    readonly StorageSortedList<string, SortedListChild> sortedListChildren;


    /// <summary>
    /// Headers written to first line in CSV file
    /// </summary>
    internal static readonly string[] Headers = {"Key", "Text"};


    /// <summary>
    /// None existing SortedListParentNR, used as a temporary place holder when reading a CSV file
    /// which was not compacted. It might create first a later deleted item linking to a 
    /// deleted parent. In this case, the parent property gets set to NoSortedListParentNR. Once the CSV
    /// file is completely read, that child will actually be deleted (released) and Verify()
    /// ensures that there are no stored children with links to NoSortedListParentNR.
    /// </summary>
    internal static SortedListParentNR NoSortedListParentNR = new SortedListParentNR("NoText", isStoring: false);
    #endregion


    #region Events
    //      ------

    /// <summary>
    /// Content of SortedListParentNR has changed. Gets only raised for changes occurring after loading DC.Data with previously stored data.
    /// </summary>
    public event Action</*old*/SortedListParentNR, /*new*/SortedListParentNR>? HasChanged;
    #endregion


    #region Constructors
    //      ------------

    /// <summary>
    /// SortedListParentNR Constructor. If isStoring is true, adds SortedListParentNR to DC.Data.SortedListParentNRs.
    /// </summary>
    public SortedListParentNR(string text, bool isStoring = true) {
      Key = StorageExtensions.NoKey;
      Text = text;
      sortedListChildren = new StorageSortedList<string, SortedListChild>();
#if DEBUG
      DC.Trace?.Invoke($"new SortedListParentNR: {ToTraceString()}");
#endif
      onConstruct();
      if (DC.Data.IsTransaction) {
        DC.Data.AddTransaction(new TransactionItem(23,TransactionActivityEnum.New, Key, this));
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
    public SortedListParentNR(SortedListParentNR original) {
    #pragma warning restore CS8618 //
      Key = StorageExtensions.NoKey;
      Text = original.Text;
      onCloned(this);
    }
    partial void onCloned(SortedListParentNR clone);


    /// <summary>
    /// Constructor for SortedListParentNR read from CSV file
    /// </summary>
    private SortedListParentNR(int key, CsvReader csvReader){
      Key = key;
      Text = csvReader.ReadString();
      sortedListChildren = new StorageSortedList<string, SortedListChild>();
      onCsvConstruct();
    }
    partial void onCsvConstruct();


    /// <summary>
    /// New SortedListParentNR read from CSV file
    /// </summary>
    internal static SortedListParentNR Create(int key, CsvReader csvReader) {
      return new SortedListParentNR(key, csvReader);
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Adds SortedListParentNR to DC.Data.SortedListParentNRs.<br/>
    /// Throws an Exception when SortedListParentNR is already stored.
    /// </summary>
    public void Store() {
      if (Key>=0) {
        throw new Exception($"SortedListParentNR cannot be stored again in DC.Data, key {Key} is greater equal 0." + Environment.NewLine + ToString());
      }

      var isCancelled = false;
      onStoring(ref isCancelled);
      if (isCancelled) return;

      DC.Data._SortedListParentNRs.Add(this);
      onStored();
#if DEBUG
      DC.Trace?.Invoke($"Stored SortedListParentNR #{GetHashCode()} @{Key}");
#endif
    }
    partial void onStoring(ref bool isCancelled);
    partial void onStored();


    /// <summary>
    /// Estimated number of UTF8 characters needed to write SortedListParentNR to CSV file
    /// </summary>
    public const int EstimatedLineLength = 150;


    /// <summary>
    /// Write SortedListParentNR to CSV file
    /// </summary>
    internal static void Write(SortedListParentNR sortedListParentNR, CsvWriter csvWriter) {
      sortedListParentNR.onCsvWrite();
      csvWriter.Write(sortedListParentNR.Text);
    }
    partial void onCsvWrite();


    /// <summary>
    /// Updates SortedListParentNR with the provided values
    /// </summary>
    public void Update(string text) {
      var clone = new SortedListParentNR(this);
      var isCancelled = false;
      onUpdating(text, ref isCancelled);
      if (isCancelled) return;

#if DEBUG
      DC.Trace?.Invoke($"Updating SortedListParentNR: {ToTraceString()}");
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
          DC.Data._SortedListParentNRs.ItemHasChanged(clone, this);
        } else if (DC.Data.IsTransaction) {
          DC.Data.AddTransaction(new TransactionItem(23, TransactionActivityEnum.Update, Key, this, oldItem: clone));
        }
        HasChanged?.Invoke(clone, this);
      }
#if DEBUG
      DC.Trace?.Invoke($"Updated SortedListParentNR: {ToTraceString()}");
#endif
    }
    partial void onUpdating(string text, ref bool isCancelled);
    partial void onUpdated(SortedListParentNR old);


    /// <summary>
    /// Updates this SortedListParentNR with values from CSV file
    /// </summary>
    internal static void Update(SortedListParentNR sortedListParentNR, CsvReader csvReader){
      sortedListParentNR.Text = csvReader.ReadString();
      sortedListParentNR.onCsvUpdate();
    }
    partial void onCsvUpdate();


    /// <summary>
    /// Add sortedListChild to SortedListChildren.
    /// </summary>
    internal void AddToSortedListChildren(SortedListChild sortedListChild) {
#if DEBUG
      if (sortedListChild==SortedListChild.NoSortedListChild) throw new Exception();
      if ((sortedListChild.Key>=0)&&(Key<0)) throw new Exception();
      if (sortedListChildren.ContainsKey(sortedListChild.Text)) throw new Exception();
#endif
      sortedListChildren.Add(sortedListChild.Text, sortedListChild);
      onAddedToSortedListChildren(sortedListChild);
#if DEBUG
      DC.Trace?.Invoke($"Add SortedListChild {sortedListChild.GetKeyOrHash()} to " +
        $"{this.GetKeyOrHash()} SortedListParentNR.SortedListChildren");
#endif
    }
    partial void onAddedToSortedListChildren(SortedListChild sortedListChild);


    /// <summary>
    /// Removes sortedListChild from SortedListParentNR.
    /// </summary>
    internal void RemoveFromSortedListChildren(SortedListChild sortedListChild) {
#if DEBUG
      if (!sortedListChildren.Remove(sortedListChild.Text)) throw new Exception();
#else
        sortedListChildren.Remove(sortedListChild.Text);
#endif
      onRemovedFromSortedListChildren(sortedListChild);
#if DEBUG
      DC.Trace?.Invoke($"Remove SortedListChild {sortedListChild.GetKeyOrHash()} from " +
        $"{this.GetKeyOrHash()} SortedListParentNR.SortedListChildren");
#endif
    }
    partial void onRemovedFromSortedListChildren(SortedListChild sortedListChild);


    /// <summary>
    /// Removes SortedListParentNR from DC.Data.SortedListParentNRs.
    /// </summary>
    public void Release() {
      if (Key<0) {
        throw new Exception($"SortedListParentNR.Release(): SortedListParentNR '{this}' is not stored in DC.Data, key is {Key}.");
      }
      foreach (var sortedListChild in SortedListChildren.Values) {
        if (sortedListChild?.Key>=0) {
          throw new Exception($"Cannot release SortedListParentNR '{this}' " + Environment.NewLine + 
            $"because '{sortedListChild}' in SortedListParentNR.SortedListChildren is still stored.");
        }
      }
      DC.Data._SortedListParentNRs.Remove(Key);
      onReleased();
#if DEBUG
      DC.Trace?.Invoke($"Released SortedListParentNR @{Key} #{GetHashCode()}");
#endif
    }
    partial void onReleased();


    /// <summary>
    /// Undoes the new() statement as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemNew(IStorageItem item) {
      var sortedListParentNR = (SortedListParentNR) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback new SortedListParentNR(): {sortedListParentNR.ToTraceString()}");
#endif
      sortedListParentNR.onRollbackItemNew();
    }
    partial void onRollbackItemNew();


    /// <summary>
    /// Releases SortedListParentNR from DC.Data.SortedListParentNRs as part of a transaction rollback of Store().
    /// </summary>
    internal static void RollbackItemStore(IStorageItem item) {
      var sortedListParentNR = (SortedListParentNR) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback SortedListParentNR.Store(): {sortedListParentNR.ToTraceString()}");
#endif
      sortedListParentNR.onRollbackItemStored();
    }
    partial void onRollbackItemStored();


    /// <summary>
    /// Restores the SortedListParentNR item data as it was before the last update as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemUpdate(IStorageItem oldStorageItem, IStorageItem newStorageItem) {
      var oldItem = (SortedListParentNR) oldStorageItem;//an item clone with the values before item was updated
      var item = (SortedListParentNR) newStorageItem;//is the instance whose values should be restored
#if DEBUG
      DC.Trace?.Invoke($"Rolling back SortedListParentNR.Update(): {item.ToTraceString()}");
#endif

      // updated item: restore old values
      item.Text = oldItem.Text;
      item.onRollbackItemUpdated(oldItem);
#if DEBUG
      DC.Trace?.Invoke($"Rolled back SortedListParentNR.Update(): {item.ToTraceString()}");
#endif
    }
    partial void onRollbackItemUpdated(SortedListParentNR oldSortedListParentNR);


    /// <summary>
    /// Adds SortedListParentNR to DC.Data.SortedListParentNRs as part of a transaction rollback of Release().
    /// </summary>
    internal static void RollbackItemRelease(IStorageItem item) {
      var sortedListParentNR = (SortedListParentNR) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback SortedListParentNR.Release(): {sortedListParentNR.ToTraceString()}");
#endif
      sortedListParentNR.onRollbackItemRelease();
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
        $" SortedListChildren: {SortedListChildren.Count}," +
        $" SortedListChildrenStored: {SortedListChildren.CountStoredItems};";
      onToString(ref returnString);
      return returnString;
    }
    partial void onToString(ref string returnString);
    #endregion
  }
}
