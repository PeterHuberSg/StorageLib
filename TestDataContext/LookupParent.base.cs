//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by StorageClassGenerator
//
//     Do not change code in this file, it will get lost when the file gets 
//     auto generated again. Write your code into LookupParent.cs.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using StorageLib;


namespace TestContext  {


  public partial class LookupParent: IStorageItemGeneric<LookupParent> {

    #region Properties
    //      ----------

    /// <summary>
    /// Unique identifier for LookupParent. Gets set once LookupParent gets added to DC.Data.
    /// </summary>
    public int Key { get; private set; }
    internal static void SetKey(IStorageItem lookupParent, int key, bool isRollback) {
#if DEBUG
      if (isRollback) {
        if (key==StorageExtensions.NoKey) {
          DC.Trace?.Invoke($"Release LookupParent key @{lookupParent.Key} #{lookupParent.GetHashCode()}");
        } else {
          DC.Trace?.Invoke($"Store LookupParent key @{key} #{lookupParent.GetHashCode()}");
        }
      }
#endif
      ((LookupParent)lookupParent).Key = key;
    }


    public string Text { get; private set; }


    /// <summary>
    /// Headers written to first line in CSV file
    /// </summary>
    internal static readonly string[] Headers = {"Key", "Text"};


    /// <summary>
    /// None existing LookupParent, used as a temporary place holder when reading a CSV file
    /// which was not compacted. It might create first a later deleted item linking to a 
    /// deleted parent. In this case, the parent property gets set to NoLookupParent. Once the CSV
    /// file is completely read, that child will actually be deleted (released) and Verify()
    /// ensures that there are no stored children with links to NoLookupParent.
    /// </summary>
    internal static LookupParent NoLookupParent = new LookupParent("NoText", isStoring: false);
    #endregion


    #region Events
    //      ------

    /// <summary>
    /// Content of LookupParent has changed. Gets only raised for changes occurring after loading DC.Data with previously stored data.
    /// </summary>
    public event Action</*old*/LookupParent, /*new*/LookupParent>? HasChanged;
    #endregion


    #region Constructors
    //      ------------

    /// <summary>
    /// LookupParent Constructor. If isStoring is true, adds LookupParent to DC.Data.LookupParents.
    /// </summary>
    public LookupParent(string text, bool isStoring = true) {
      Key = StorageExtensions.NoKey;
      Text = text;
#if DEBUG
      DC.Trace?.Invoke($"new LookupParent: {ToTraceString()}");
#endif
      onConstruct();
      if (DC.Data.IsTransaction) {
        DC.Data.AddTransaction(new TransactionItem(0,TransactionActivityEnum.New, Key, this));
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
    public LookupParent(LookupParent original) {
    #pragma warning restore CS8618 //
      Key = StorageExtensions.NoKey;
      Text = original.Text;
      onCloned(this);
    }
    partial void onCloned(LookupParent clone);


    /// <summary>
    /// Constructor for LookupParent read from CSV file
    /// </summary>
    private LookupParent(int key, CsvReader csvReader){
      Key = key;
      Text = csvReader.ReadString();
      onCsvConstruct();
    }
    partial void onCsvConstruct();


    /// <summary>
    /// New LookupParent read from CSV file
    /// </summary>
    internal static LookupParent Create(int key, CsvReader csvReader) {
      return new LookupParent(key, csvReader);
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Adds LookupParent to DC.Data.LookupParents.<br/>
    /// Throws an Exception when LookupParent is already stored.
    /// </summary>
    public void Store() {
      if (Key>=0) {
        throw new Exception($"LookupParent cannot be stored again in DC.Data, key {Key} is greater equal 0." + Environment.NewLine + ToString());
      }

      var isCancelled = false;
      onStoring(ref isCancelled);
      if (isCancelled) return;

      DC.Data._LookupParents.Add(this);
      onStored();
#if DEBUG
      DC.Trace?.Invoke($"Stored LookupParent #{GetHashCode()} @{Key}");
#endif
    }
    partial void onStoring(ref bool isCancelled);
    partial void onStored();


    /// <summary>
    /// Estimated number of UTF8 characters needed to write LookupParent to CSV file
    /// </summary>
    public const int EstimatedLineLength = 150;


    /// <summary>
    /// Write LookupParent to CSV file
    /// </summary>
    internal static void Write(LookupParent lookupParent, CsvWriter csvWriter) {
      lookupParent.onCsvWrite();
      csvWriter.Write(lookupParent.Text);
    }
    partial void onCsvWrite();


    /// <summary>
    /// Updates LookupParent with the provided values
    /// </summary>
    public void Update(string text) {
      var clone = new LookupParent(this);
      var isCancelled = false;
      onUpdating(text, ref isCancelled);
      if (isCancelled) return;

#if DEBUG
      DC.Trace?.Invoke($"Updating LookupParent: {ToTraceString()}");
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
          DC.Data._LookupParents.ItemHasChanged(clone, this);
        } else if (DC.Data.IsTransaction) {
          DC.Data.AddTransaction(new TransactionItem(0, TransactionActivityEnum.Update, Key, this, oldItem: clone));
        }
        HasChanged?.Invoke(clone, this);
      }
#if DEBUG
      DC.Trace?.Invoke($"Updated LookupParent: {ToTraceString()}");
#endif
    }
    partial void onUpdating(string text, ref bool isCancelled);
    partial void onUpdated(LookupParent old);


    /// <summary>
    /// Updates this LookupParent with values from CSV file
    /// </summary>
    internal static void Update(LookupParent lookupParent, CsvReader csvReader){
      lookupParent.Text = csvReader.ReadString();
      lookupParent.onCsvUpdate();
    }
    partial void onCsvUpdate();


    /// <summary>
    /// Releasing LookupParent from DC.Data.LookupParents is not supported.
    /// </summary>
    public void Release() {
      throw new NotSupportedException("Release() is not supported, StorageClass attribute AreInstancesReleasable is false.");
    }


    /// <summary>
    /// Undoes the new() statement as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemNew(IStorageItem item) {
      var lookupParent = (LookupParent) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback new LookupParent(): {lookupParent.ToTraceString()}");
#endif
      lookupParent.onRollbackItemNew();
    }
    partial void onRollbackItemNew();


    /// <summary>
    /// Releases LookupParent from DC.Data.LookupParents as part of a transaction rollback of Store().
    /// </summary>
    internal static void RollbackItemStore(IStorageItem item) {
      var lookupParent = (LookupParent) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback LookupParent.Store(): {lookupParent.ToTraceString()}");
#endif
      lookupParent.onRollbackItemStored();
    }
    partial void onRollbackItemStored();


    /// <summary>
    /// Restores the LookupParent item data as it was before the last update as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemUpdate(IStorageItem oldStorageItem, IStorageItem newStorageItem) {
      var oldItem = (LookupParent) oldStorageItem;//an item clone with the values before item was updated
      var item = (LookupParent) newStorageItem;//is the instance whose values should be restored
#if DEBUG
      DC.Trace?.Invoke($"Rolling back LookupParent.Update(): {item.ToTraceString()}");
#endif

      // updated item: restore old values
      item.Text = oldItem.Text;
      item.onRollbackItemUpdated(oldItem);
#if DEBUG
      DC.Trace?.Invoke($"Rolled back LookupParent.Update(): {item.ToTraceString()}");
#endif
    }
    partial void onRollbackItemUpdated(LookupParent oldLookupParent);


    /// <summary>
    /// Adds LookupParent to DC.Data.LookupParents as part of a transaction rollback of Release().
    /// </summary>
    internal static void RollbackItemRelease(IStorageItem item) {
      var lookupParent = (LookupParent) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback LookupParent.Release(): {lookupParent.ToTraceString()}");
#endif
      lookupParent.onRollbackItemRelease();
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
        $" Text: {Text};";
      onToString(ref returnString);
      return returnString;
    }
    partial void onToString(ref string returnString);
    #endregion
  }
}
