//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by StorageClassGenerator
//
//     Do not change code in this file, it will get lost when the file gets 
//     auto generated again. Write your code into DictionaryParentR.cs.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using StorageLib;


namespace TestContext  {


  public partial class DictionaryParentR: IStorageItemGeneric<DictionaryParentR> {

    #region Properties
    //      ----------

    /// <summary>
    /// Unique identifier for DictionaryParentR. Gets set once DictionaryParentR gets added to DC.Data.
    /// </summary>
    public int Key { get; private set; }
    internal static void SetKey(IStorageItem dictionaryParentR, int key, bool isRollback) {
#if DEBUG
      if (isRollback) {
        if (key==StorageExtensions.NoKey) {
          DC.Trace?.Invoke($"Release DictionaryParentR key @{dictionaryParentR.Key} #{dictionaryParentR.GetHashCode()}");
        } else {
          DC.Trace?.Invoke($"Store DictionaryParentR key @{key} #{dictionaryParentR.GetHashCode()}");
        }
      }
#endif
      ((DictionaryParentR)dictionaryParentR).Key = key;
    }


    public string Text { get; private set; }


    public IReadOnlyDictionary<string, DictionaryChild> DictionaryChidren => dictionaryChidren;
    readonly Dictionary<string, DictionaryChild> dictionaryChidren;


    /// <summary>
    /// Headers written to first line in CSV file
    /// </summary>
    internal static readonly string[] Headers = {"Key", "Text"};


    /// <summary>
    /// None existing DictionaryParentR, used as a temporary place holder when reading a CSV file
    /// which was not compacted. It might create first a later deleted item linking to a 
    /// deleted parent. In this case, the parent property gets set to NoDictionaryParentR. Once the CSV
    /// file is completely read, that child will actually be deleted (released) and Verify()
    /// ensures that there are no stored children with links to NoDictionaryParentR.
    /// </summary>
    internal static DictionaryParentR NoDictionaryParentR = new DictionaryParentR("NoText", isStoring: false);
    #endregion


    #region Events
    //      ------

    /// <summary>
    /// Content of DictionaryParentR has changed. Gets only raised for changes occurring after loading DC.Data with previously stored data.
    /// </summary>
    public event Action</*old*/DictionaryParentR, /*new*/DictionaryParentR>? HasChanged;
    #endregion


    #region Constructors
    //      ------------

    /// <summary>
    /// DictionaryParentR Constructor. If isStoring is true, adds DictionaryParentR to DC.Data.DictionaryParentRs.
    /// </summary>
    public DictionaryParentR(string text, bool isStoring = true) {
      Key = StorageExtensions.NoKey;
      Text = text;
      dictionaryChidren = new Dictionary<string, DictionaryChild>();
#if DEBUG
      DC.Trace?.Invoke($"new DictionaryParentR: {ToTraceString()}");
#endif
      onConstruct();
      if (DC.Data.IsTransaction) {
        DC.Data.AddTransaction(new TransactionItem(17,TransactionActivityEnum.New, Key, this));
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
    public DictionaryParentR(DictionaryParentR original) {
    #pragma warning restore CS8618 //
      Key = StorageExtensions.NoKey;
      Text = original.Text;
      onCloned(this);
    }
    partial void onCloned(DictionaryParentR clone);


    /// <summary>
    /// Constructor for DictionaryParentR read from CSV file
    /// </summary>
    private DictionaryParentR(int key, CsvReader csvReader){
      Key = key;
      Text = csvReader.ReadString();
      dictionaryChidren = new Dictionary<string, DictionaryChild>();
      onCsvConstruct();
    }
    partial void onCsvConstruct();


    /// <summary>
    /// New DictionaryParentR read from CSV file
    /// </summary>
    internal static DictionaryParentR Create(int key, CsvReader csvReader) {
      return new DictionaryParentR(key, csvReader);
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Adds DictionaryParentR to DC.Data.DictionaryParentRs.<br/>
    /// Throws an Exception when DictionaryParentR is already stored.
    /// </summary>
    public void Store() {
      if (Key>=0) {
        throw new Exception($"DictionaryParentR cannot be stored again in DC.Data, key {Key} is greater equal 0." + Environment.NewLine + ToString());
      }

      var isCancelled = false;
      onStoring(ref isCancelled);
      if (isCancelled) return;

      DC.Data._DictionaryParentRs.Add(this);
      onStored();
#if DEBUG
      DC.Trace?.Invoke($"Stored DictionaryParentR #{GetHashCode()} @{Key}");
#endif
    }
    partial void onStoring(ref bool isCancelled);
    partial void onStored();


    /// <summary>
    /// Estimated number of UTF8 characters needed to write DictionaryParentR to CSV file
    /// </summary>
    public const int EstimatedLineLength = 150;


    /// <summary>
    /// Write DictionaryParentR to CSV file
    /// </summary>
    internal static void Write(DictionaryParentR dictionaryParentR, CsvWriter csvWriter) {
      dictionaryParentR.onCsvWrite();
      csvWriter.Write(dictionaryParentR.Text);
    }
    partial void onCsvWrite();


    /// <summary>
    /// Updates DictionaryParentR with the provided values
    /// </summary>
    public void Update(string text) {
      var clone = new DictionaryParentR(this);
      var isCancelled = false;
      onUpdating(text, ref isCancelled);
      if (isCancelled) return;

#if DEBUG
      DC.Trace?.Invoke($"Updating DictionaryParentR: {ToTraceString()}");
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
          DC.Data._DictionaryParentRs.ItemHasChanged(clone, this);
        } else if (DC.Data.IsTransaction) {
          DC.Data.AddTransaction(new TransactionItem(17, TransactionActivityEnum.Update, Key, this, oldItem: clone));
        }
        HasChanged?.Invoke(clone, this);
      }
#if DEBUG
      DC.Trace?.Invoke($"Updated DictionaryParentR: {ToTraceString()}");
#endif
    }
    partial void onUpdating(string text, ref bool isCancelled);
    partial void onUpdated(DictionaryParentR old);


    /// <summary>
    /// Updates this DictionaryParentR with values from CSV file
    /// </summary>
    internal static void Update(DictionaryParentR dictionaryParentR, CsvReader csvReader){
      dictionaryParentR.Text = csvReader.ReadString();
      dictionaryParentR.onCsvUpdate();
    }
    partial void onCsvUpdate();


    /// <summary>
    /// Add dictionaryChild to DictionaryChidren.
    /// </summary>
    internal void AddToDictionaryChidren(DictionaryChild dictionaryChild) {
#if DEBUG
      if (dictionaryChild==DictionaryChild.NoDictionaryChild) throw new Exception();
      if ((dictionaryChild.Key>=0)&&(Key<0)) throw new Exception();
      if (dictionaryChidren.ContainsKey(dictionaryChild.Text)) throw new Exception();
#endif
      dictionaryChidren.Add(dictionaryChild.Text, dictionaryChild);
      onAddedToDictionaryChidren(dictionaryChild);
#if DEBUG
      DC.Trace?.Invoke($"Add DictionaryChild {dictionaryChild.GetKeyOrHash()} to " +
        $"{this.GetKeyOrHash()} DictionaryParentR.DictionaryChidren");
#endif
    }
    partial void onAddedToDictionaryChidren(DictionaryChild dictionaryChild);


    /// <summary>
    /// Removes dictionaryChild from DictionaryParentR.
    /// </summary>
    internal void RemoveFromDictionaryChidren(DictionaryChild dictionaryChild) {
#if DEBUG
      if (!dictionaryChidren.Remove(dictionaryChild.Text)) throw new Exception();
#else
        dictionaryChidren.Remove(dictionaryChild.Text);
#endif
      onRemovedFromDictionaryChidren(dictionaryChild);
#if DEBUG
      DC.Trace?.Invoke($"Remove DictionaryChild {dictionaryChild.GetKeyOrHash()} from " +
        $"{this.GetKeyOrHash()} DictionaryParentR.DictionaryChidren");
#endif
    }
    partial void onRemovedFromDictionaryChidren(DictionaryChild dictionaryChild);


    /// <summary>
    /// Removes DictionaryParentR from DC.Data.DictionaryParentRs.
    /// </summary>
    public void Release() {
      if (Key<0) {
        throw new Exception($"DictionaryParentR.Release(): DictionaryParentR '{this}' is not stored in DC.Data, key is {Key}.");
      }
      foreach (var dictionaryChild in DictionaryChidren.Values) {
        if (dictionaryChild?.Key>=0) {
          throw new Exception($"Cannot release DictionaryParentR '{this}' " + Environment.NewLine + 
            $"because '{dictionaryChild}' in DictionaryParentR.DictionaryChidren is still stored.");
        }
      }
      onReleased();
      DC.Data._DictionaryParentRs.Remove(Key);
#if DEBUG
      DC.Trace?.Invoke($"Released DictionaryParentR @{Key} #{GetHashCode()}");
#endif
    }
    partial void onReleased();


    /// <summary>
    /// Undoes the new() statement as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemNew(IStorageItem item) {
      var dictionaryParentR = (DictionaryParentR) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback new DictionaryParentR(): {dictionaryParentR.ToTraceString()}");
#endif
      dictionaryParentR.onRollbackItemNew();
    }
    partial void onRollbackItemNew();


    /// <summary>
    /// Releases DictionaryParentR from DC.Data.DictionaryParentRs as part of a transaction rollback of Store().
    /// </summary>
    internal static void RollbackItemStore(IStorageItem item) {
      var dictionaryParentR = (DictionaryParentR) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback DictionaryParentR.Store(): {dictionaryParentR.ToTraceString()}");
#endif
      dictionaryParentR.onRollbackItemStored();
    }
    partial void onRollbackItemStored();


    /// <summary>
    /// Restores the DictionaryParentR item data as it was before the last update as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemUpdate(IStorageItem oldStorageItem, IStorageItem newStorageItem) {
      var oldItem = (DictionaryParentR) oldStorageItem;//an item clone with the values before item was updated
      var item = (DictionaryParentR) newStorageItem;//is the instance whose values should be restored
#if DEBUG
      DC.Trace?.Invoke($"Rolling back DictionaryParentR.Update(): {item.ToTraceString()}");
#endif

      // updated item: restore old values
      item.Text = oldItem.Text;
      item.onRollbackItemUpdated(oldItem);
#if DEBUG
      DC.Trace?.Invoke($"Rolled back DictionaryParentR.Update(): {item.ToTraceString()}");
#endif
    }
    partial void onRollbackItemUpdated(DictionaryParentR oldDictionaryParentR);


    /// <summary>
    /// Adds DictionaryParentR to DC.Data.DictionaryParentRs as part of a transaction rollback of Release().
    /// </summary>
    internal static void RollbackItemRelease(IStorageItem item) {
      var dictionaryParentR = (DictionaryParentR) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback DictionaryParentR.Release(): {dictionaryParentR.ToTraceString()}");
#endif
      dictionaryParentR.onRollbackItemRelease();
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
        $" DictionaryChidren: {DictionaryChidren.Count};";
      onToString(ref returnString);
      return returnString;
    }
    partial void onToString(ref string returnString);
    #endregion
  }
}
