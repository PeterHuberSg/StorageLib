//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by StorageClassGenerator
//
//     Do not change code in this file, it will get lost when the file gets 
//     auto generated again. Write your code into DictionaryParent.cs.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using StorageLib;


namespace StorageDataContext  {


  public partial class DictionaryParent: IStorageItemGeneric<DictionaryParent> {

    #region Properties
    //      ----------

    /// <summary>
    /// Unique identifier for DictionaryParent. Gets set once DictionaryParent gets added to DC.Data.
    /// </summary>
    public int Key { get; private set; }
    internal static void SetKey(IStorageItem dictionaryParent, int key, bool isRollback) {
#if DEBUG
      if (isRollback) {
        if (key==StorageExtensions.NoKey) {
          DC.Trace?.Invoke($"Release DictionaryParent key @{dictionaryParent.Key} #{dictionaryParent.GetHashCode()}");
        } else {
          DC.Trace?.Invoke($"Store DictionaryParent key @{key} #{dictionaryParent.GetHashCode()}");
        }
      }
#endif
      ((DictionaryParent)dictionaryParent).Key = key;
    }


    public string Text { get; private set; }


    public IReadOnlyDictionary<string, DictionaryChild> DictionaryChidren => dictionaryChidren;
    readonly Dictionary<string, DictionaryChild> dictionaryChidren;


    /// <summary>
    /// Headers written to first line in CSV file
    /// </summary>
    internal static readonly string[] Headers = {"Key", "Text"};


    /// <summary>
    /// None existing DictionaryParent
    /// </summary>
    internal static DictionaryParent NoDictionaryParent = new DictionaryParent("NoText", isStoring: false);
    #endregion


    #region Events
    //      ------

    /// <summary>
    /// Content of DictionaryParent has changed. Gets only raised for changes occurring after loading DC.Data with previously stored data.
    /// </summary>
    public event Action</*old*/DictionaryParent, /*new*/DictionaryParent>? HasChanged;
    #endregion


    #region Constructors
    //      ------------

    /// <summary>
    /// DictionaryParent Constructor. If isStoring is true, adds DictionaryParent to DC.Data.DictionaryParents.
    /// </summary>
    public DictionaryParent(string text, bool isStoring = true) {
      Key = StorageExtensions.NoKey;
      Text = text;
      dictionaryChidren = new Dictionary<string, DictionaryChild>();
#if DEBUG
      DC.Trace?.Invoke($"new DictionaryParent: {ToTraceString()}");
#endif
      onConstruct();
      if (DC.Data.IsTransaction) {
        DC.Data.AddTransaction(new TransactionItem(15,TransactionActivityEnum.New, Key, this));
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
    public DictionaryParent(DictionaryParent original) {
    #pragma warning restore CS8618 //
      Key = StorageExtensions.NoKey;
      Text = original.Text;
      onCloned(this);
    }
    partial void onCloned(DictionaryParent clone);


    /// <summary>
    /// Constructor for DictionaryParent read from CSV file
    /// </summary>
    private DictionaryParent(int key, CsvReader csvReader){
      Key = key;
      Text = csvReader.ReadString();
      dictionaryChidren = new Dictionary<string, DictionaryChild>();
      onCsvConstruct();
    }
    partial void onCsvConstruct();


    /// <summary>
    /// New DictionaryParent read from CSV file
    /// </summary>
    internal static DictionaryParent Create(int key, CsvReader csvReader) {
      return new DictionaryParent(key, csvReader);
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Adds DictionaryParent to DC.Data.DictionaryParents.<br/>
    /// Throws an Exception when DictionaryParent is already stored.
    /// </summary>
    public void Store() {
      if (Key>=0) {
        throw new Exception($"DictionaryParent cannot be stored again in DC.Data, key {Key} is greater equal 0." + Environment.NewLine + ToString());
      }

      var isCancelled = false;
      onStoring(ref isCancelled);
      if (isCancelled) return;

      DC.Data._DictionaryParents.Add(this);
      onStored();
#if DEBUG
      DC.Trace?.Invoke($"Stored DictionaryParent #{GetHashCode()} @{Key}");
#endif
    }
    partial void onStoring(ref bool isCancelled);
    partial void onStored();


    /// <summary>
    /// Estimated number of UTF8 characters needed to write DictionaryParent to CSV file
    /// </summary>
    public const int EstimatedLineLength = 150;


    /// <summary>
    /// Write DictionaryParent to CSV file
    /// </summary>
    internal static void Write(DictionaryParent dictionaryParent, CsvWriter csvWriter) {
      dictionaryParent.onCsvWrite();
      csvWriter.Write(dictionaryParent.Text);
    }
    partial void onCsvWrite();


    /// <summary>
    /// Updates DictionaryParent with the provided values
    /// </summary>
    public void Update(string text) {
      var clone = new DictionaryParent(this);
      var isCancelled = false;
      onUpdating(text, ref isCancelled);
      if (isCancelled) return;

#if DEBUG
      DC.Trace?.Invoke($"Updating DictionaryParent: {ToTraceString()}");
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
          DC.Data._DictionaryParents.ItemHasChanged(clone, this);
        } else if (DC.Data.IsTransaction) {
          DC.Data.AddTransaction(new TransactionItem(15, TransactionActivityEnum.Update, Key, this, oldItem: clone));
        }
        HasChanged?.Invoke(clone, this);
      }
#if DEBUG
      DC.Trace?.Invoke($"Updated DictionaryParent: {ToTraceString()}");
#endif
    }
    partial void onUpdating(string text, ref bool isCancelled);
    partial void onUpdated(DictionaryParent old);


    /// <summary>
    /// Updates this DictionaryParent with values from CSV file
    /// </summary>
    internal static void Update(DictionaryParent dictionaryParent, CsvReader csvReader){
      dictionaryParent.Text = csvReader.ReadString();
      dictionaryParent.onCsvUpdate();
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
        $"{this.GetKeyOrHash()} DictionaryParent.DictionaryChidren");
#endif
    }
    partial void onAddedToDictionaryChidren(DictionaryChild dictionaryChild);


    /// <summary>
    /// Removes dictionaryChild from DictionaryParent.
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
        $"{this.GetKeyOrHash()} DictionaryParent.DictionaryChidren");
#endif
    }
    partial void onRemovedFromDictionaryChidren(DictionaryChild dictionaryChild);


    /// <summary>
    /// Removes DictionaryParent from DC.Data.DictionaryParents.
    /// </summary>
    public void Release() {
      if (Key<0) {
        throw new Exception($"DictionaryParent.Release(): DictionaryParent '{this}' is not stored in DC.Data, key is {Key}.");
      }
      foreach (var dictionaryChild in DictionaryChidren.Values) {
        if (dictionaryChild?.Key>=0) {
          throw new Exception($"Cannot release DictionaryParent '{this}' " + Environment.NewLine + 
            $"because '{dictionaryChild}' in DictionaryParent.DictionaryChidren is still stored.");
        }
      }
      onReleased();
      DC.Data._DictionaryParents.Remove(Key);
#if DEBUG
      DC.Trace?.Invoke($"Released DictionaryParent @{Key} #{GetHashCode()}");
#endif
    }
    partial void onReleased();


    /// <summary>
    /// Undoes the new() statement as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemNew(IStorageItem item) {
      var dictionaryParent = (DictionaryParent) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback new DictionaryParent(): {dictionaryParent.ToTraceString()}");
#endif
      dictionaryParent.onRollbackItemNew();
    }
    partial void onRollbackItemNew();


    /// <summary>
    /// Releases DictionaryParent from DC.Data.DictionaryParents as part of a transaction rollback of Store().
    /// </summary>
    internal static void RollbackItemStore(IStorageItem item) {
      var dictionaryParent = (DictionaryParent) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback DictionaryParent.Store(): {dictionaryParent.ToTraceString()}");
#endif
      dictionaryParent.onRollbackItemStored();
    }
    partial void onRollbackItemStored();


    /// <summary>
    /// Restores the DictionaryParent item data as it was before the last update as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemUpdate(IStorageItem oldStorageItem, IStorageItem newStorageItem) {
      var oldItem = (DictionaryParent) oldStorageItem;//an item clone with the values before item was updated
      var item = (DictionaryParent) newStorageItem;//is the instance whose values should be restored
#if DEBUG
      DC.Trace?.Invoke($"Rolling back DictionaryParent.Update(): {item.ToTraceString()}");
#endif

      // updated item: restore old values
      item.Text = oldItem.Text;
      item.onRollbackItemUpdated(oldItem);
#if DEBUG
      DC.Trace?.Invoke($"Rolled back DictionaryParent.Update(): {item.ToTraceString()}");
#endif
    }
    partial void onRollbackItemUpdated(DictionaryParent oldDictionaryParent);


    /// <summary>
    /// Adds DictionaryParent to DC.Data.DictionaryParents as part of a transaction rollback of Release().
    /// </summary>
    internal static void RollbackItemRelease(IStorageItem item) {
      var dictionaryParent = (DictionaryParent) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback DictionaryParent.Release(): {dictionaryParent.ToTraceString()}");
#endif
      dictionaryParent.onRollbackItemRelease();
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
