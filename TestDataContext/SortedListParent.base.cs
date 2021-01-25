//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by StorageClassGenerator
//
//     Do not change code in this file, it will get lost when the file gets 
//     auto generated again. Write your code into SortedListParent.cs.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using StorageLib;


namespace TestContext  {


  public partial class SortedListParent: IStorageItemGeneric<SortedListParent> {

    #region Properties
    //      ----------

    /// <summary>
    /// Unique identifier for SortedListParent. Gets set once SortedListParent gets added to DC.Data.
    /// </summary>
    public int Key { get; private set; }
    internal static void SetKey(IStorageItem sortedListParent, int key, bool isRollback) {
#if DEBUG
      if (isRollback) {
        if (key==StorageExtensions.NoKey) {
          DC.Trace?.Invoke($"Release SortedListParent key @{sortedListParent.Key} #{sortedListParent.GetHashCode()}");
        } else {
          DC.Trace?.Invoke($"Store SortedListParent key @{key} #{sortedListParent.GetHashCode()}");
        }
      }
#endif
      ((SortedListParent)sortedListParent).Key = key;
    }


    public string Text { get; private set; }


    public IStorageReadOnlyDictionary<string, SortedListChild> SortedListChidren => sortedListChidren;
    readonly StorageSortedList<SortedListParent, string, SortedListChild> sortedListChidren;


    /// <summary>
    /// Headers written to first line in CSV file
    /// </summary>
    internal static readonly string[] Headers = {"Key", "Text"};


    /// <summary>
    /// None existing SortedListParent, used as a temporary place holder when reading a CSV file
    /// which was not compacted. It might create first a later deleted item linking to a 
    /// deleted parent. In this case, the parent property gets set to NoSortedListParent. Once the CSV
    /// file is completely read, that child will actually be deleted (released) and Verify()
    /// ensures that there are no stored children with links to NoSortedListParent.
    /// </summary>
    internal static SortedListParent NoSortedListParent = new SortedListParent("NoText", isStoring: false);
    #endregion


    #region Events
    //      ------

    /// <summary>
    /// Content of SortedListParent has changed. Gets only raised for changes occurring after loading DC.Data with previously stored data.
    /// </summary>
    public event Action</*old*/SortedListParent, /*new*/SortedListParent>? HasChanged;
    #endregion


    #region Constructors
    //      ------------

    /// <summary>
    /// SortedListParent Constructor. If isStoring is true, adds SortedListParent to DC.Data.SortedListParents.
    /// </summary>
    public SortedListParent(string text, bool isStoring = true) {
      Key = StorageExtensions.NoKey;
      Text = text;
      sortedListChidren = new StorageSortedList<SortedListParent, string, SortedListChild>(this);
#if DEBUG
      DC.Trace?.Invoke($"new SortedListParent: {ToTraceString()}");
#endif
      onConstruct();
      if (DC.Data.IsTransaction) {
        DC.Data.AddTransaction(new TransactionItem(20,TransactionActivityEnum.New, Key, this));
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
    public SortedListParent(SortedListParent original) {
    #pragma warning restore CS8618 //
      Key = StorageExtensions.NoKey;
      Text = original.Text;
      onCloned(this);
    }
    partial void onCloned(SortedListParent clone);


    /// <summary>
    /// Constructor for SortedListParent read from CSV file
    /// </summary>
    private SortedListParent(int key, CsvReader csvReader){
      Key = key;
      Text = csvReader.ReadString();
      sortedListChidren = new StorageSortedList<SortedListParent, string, SortedListChild>(this);
      onCsvConstruct();
    }
    partial void onCsvConstruct();


    /// <summary>
    /// New SortedListParent read from CSV file
    /// </summary>
    internal static SortedListParent Create(int key, CsvReader csvReader) {
      return new SortedListParent(key, csvReader);
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Adds SortedListParent to DC.Data.SortedListParents.<br/>
    /// Throws an Exception when SortedListParent is already stored.
    /// </summary>
    public void Store() {
      if (Key>=0) {
        throw new Exception($"SortedListParent cannot be stored again in DC.Data, key {Key} is greater equal 0." + Environment.NewLine + ToString());
      }

      var isCancelled = false;
      onStoring(ref isCancelled);
      if (isCancelled) return;

      DC.Data._SortedListParents.Add(this);
      onStored();
#if DEBUG
      DC.Trace?.Invoke($"Stored SortedListParent #{GetHashCode()} @{Key}");
#endif
    }
    partial void onStoring(ref bool isCancelled);
    partial void onStored();


    /// <summary>
    /// Estimated number of UTF8 characters needed to write SortedListParent to CSV file
    /// </summary>
    public const int EstimatedLineLength = 150;


    /// <summary>
    /// Write SortedListParent to CSV file
    /// </summary>
    internal static void Write(SortedListParent sortedListParent, CsvWriter csvWriter) {
      sortedListParent.onCsvWrite();
      csvWriter.Write(sortedListParent.Text);
    }
    partial void onCsvWrite();


    /// <summary>
    /// Updates SortedListParent with the provided values
    /// </summary>
    public void Update(string text) {
      var clone = new SortedListParent(this);
      var isCancelled = false;
      onUpdating(text, ref isCancelled);
      if (isCancelled) return;

#if DEBUG
      DC.Trace?.Invoke($"Updating SortedListParent: {ToTraceString()}");
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
          DC.Data._SortedListParents.ItemHasChanged(clone, this);
        } else if (DC.Data.IsTransaction) {
          DC.Data.AddTransaction(new TransactionItem(20, TransactionActivityEnum.Update, Key, this, oldItem: clone));
        }
        HasChanged?.Invoke(clone, this);
      }
#if DEBUG
      DC.Trace?.Invoke($"Updated SortedListParent: {ToTraceString()}");
#endif
    }
    partial void onUpdating(string text, ref bool isCancelled);
    partial void onUpdated(SortedListParent old);


    /// <summary>
    /// Updates this SortedListParent with values from CSV file
    /// </summary>
    internal static void Update(SortedListParent sortedListParent, CsvReader csvReader){
      sortedListParent.Text = csvReader.ReadString();
      sortedListParent.onCsvUpdate();
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
        $"{this.GetKeyOrHash()} SortedListParent.SortedListChidren");
#endif
    }
    partial void onAddedToSortedListChidren(SortedListChild sortedListChild);


    /// <summary>
    /// Removes sortedListChild from SortedListParent.
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
        $"{this.GetKeyOrHash()} SortedListParent.SortedListChidren");
#endif
    }
    partial void onRemovedFromSortedListChidren(SortedListChild sortedListChild);


    /// <summary>
    /// Removes SortedListParent from DC.Data.SortedListParents.
    /// </summary>
    public void Release() {
      if (Key<0) {
        throw new Exception($"SortedListParent.Release(): SortedListParent '{this}' is not stored in DC.Data, key is {Key}.");
      }
      foreach (var sortedListChild in SortedListChidren.Values) {
        if (sortedListChild?.Key>=0) {
          throw new Exception($"Cannot release SortedListParent '{this}' " + Environment.NewLine + 
            $"because '{sortedListChild}' in SortedListParent.SortedListChidren is still stored.");
        }
      }
      DC.Data._SortedListParents.Remove(Key);
      onReleased();
#if DEBUG
      DC.Trace?.Invoke($"Released SortedListParent @{Key} #{GetHashCode()}");
#endif
    }
    partial void onReleased();


    /// <summary>
    /// Undoes the new() statement as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemNew(IStorageItem item) {
      var sortedListParent = (SortedListParent) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback new SortedListParent(): {sortedListParent.ToTraceString()}");
#endif
      sortedListParent.onRollbackItemNew();
    }
    partial void onRollbackItemNew();


    /// <summary>
    /// Releases SortedListParent from DC.Data.SortedListParents as part of a transaction rollback of Store().
    /// </summary>
    internal static void RollbackItemStore(IStorageItem item) {
      var sortedListParent = (SortedListParent) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback SortedListParent.Store(): {sortedListParent.ToTraceString()}");
#endif
      sortedListParent.onRollbackItemStored();
    }
    partial void onRollbackItemStored();


    /// <summary>
    /// Restores the SortedListParent item data as it was before the last update as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemUpdate(IStorageItem oldStorageItem, IStorageItem newStorageItem) {
      var oldItem = (SortedListParent) oldStorageItem;//an item clone with the values before item was updated
      var item = (SortedListParent) newStorageItem;//is the instance whose values should be restored
#if DEBUG
      DC.Trace?.Invoke($"Rolling back SortedListParent.Update(): {item.ToTraceString()}");
#endif

      // updated item: restore old values
      item.Text = oldItem.Text;
      item.onRollbackItemUpdated(oldItem);
#if DEBUG
      DC.Trace?.Invoke($"Rolled back SortedListParent.Update(): {item.ToTraceString()}");
#endif
    }
    partial void onRollbackItemUpdated(SortedListParent oldSortedListParent);


    /// <summary>
    /// Adds SortedListParent to DC.Data.SortedListParents as part of a transaction rollback of Release().
    /// </summary>
    internal static void RollbackItemRelease(IStorageItem item) {
      var sortedListParent = (SortedListParent) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback SortedListParent.Release(): {sortedListParent.ToTraceString()}");
#endif
      sortedListParent.onRollbackItemRelease();
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
        $" SortedListChidren: {SortedListChidren.Count}," +
        $" SortedListChidrenAll: {SortedListChidren.CountAll};";
      onToString(ref returnString);
      return returnString;
    }
    partial void onToString(ref string returnString);
    #endregion
  }
}
