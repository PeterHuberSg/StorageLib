//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by StorageClassGenerator
//
//     Do not change code in this file, it will get lost when the file gets 
//     auto generated again. Write your code into PrivateConstructor.cs.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using StorageLib;


namespace TestContext  {


    /// <summary>
    /// Example with private constructor.
    /// </summary>
  public partial class PrivateConstructor: IStorageItem<PrivateConstructor> {

    #region Properties
    //      ----------

    /// <summary>
    /// Unique identifier for PrivateConstructor. Gets set once PrivateConstructor gets added to DC.Data.
    /// </summary>
    public int Key { get; private set; }
    internal static void SetKey(IStorageItem privateConstructor, int key, bool isRollback) {
#if DEBUG
      if (isRollback) {
        if (key==StorageExtensions.NoKey) {
          DC.Trace?.Invoke($"Release PrivateConstructor key @{privateConstructor.Key} #{privateConstructor.GetHashCode()}");
        } else {
          DC.Trace?.Invoke($"Store PrivateConstructor key @{key} #{privateConstructor.GetHashCode()}");
        }
      }
#endif
      ((PrivateConstructor)privateConstructor).Key = key;
    }


    /// <summary>
    /// Some Text
    /// </summary>
    public string Text { get; private set; }


    /// <summary>
    /// Headers written to first line in CSV file
    /// </summary>
    internal static readonly string[] Headers = {"Key", "Text"};


    /// <summary>
    /// None existing PrivateConstructor, used as a temporary place holder when reading a CSV file
    /// which was not compacted. It might create first a later deleted item linking to a 
    /// deleted parent. In this case, the parent property gets set to NoPrivateConstructor. Once the CSV
    /// file is completely read, that child will actually be deleted (released) and Verify()
    /// ensures that there are no stored children with links to NoPrivateConstructor.
    /// </summary>
    internal static PrivateConstructor NoPrivateConstructor = new PrivateConstructor("NoText", isStoring: false);
    #endregion


    #region Events
    //      ------

    /// <summary>
    /// Content of PrivateConstructor has changed. Gets only raised for changes occurring after loading DC.Data with previously stored data.
    /// </summary>
    public event Action</*old*/PrivateConstructor, /*new*/PrivateConstructor>? HasChanged;
    #endregion


    #region Constructors
    //      ------------

    /// <summary>
    /// PrivateConstructor Constructor. If isStoring is true, adds PrivateConstructor to DC.Data.PrivateConstructors.
    /// </summary>
    private PrivateConstructor(string text, bool isStoring = true) {
      Key = StorageExtensions.NoKey;
      Text = text;
#if DEBUG
      DC.Trace?.Invoke($"new PrivateConstructor: {ToTraceString()}");
#endif
      onConstruct();
      if (DC.Data.IsTransaction) {
        DC.Data.AddTransaction(new TransactionItem(26,TransactionActivityEnum.New, Key, this));
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
    public PrivateConstructor(PrivateConstructor original) {
    #pragma warning restore CS8618 //
      Key = StorageExtensions.NoKey;
      Text = original.Text;
      onCloned(this);
    }
    partial void onCloned(PrivateConstructor clone);


    /// <summary>
    /// Constructor for PrivateConstructor read from CSV file
    /// </summary>
    private PrivateConstructor(int key, CsvReader csvReader){
      Key = key;
      Text = csvReader.ReadString();
      onCsvConstruct();
    }
    partial void onCsvConstruct();


    /// <summary>
    /// New PrivateConstructor read from CSV file
    /// </summary>
    internal static PrivateConstructor Create(int key, CsvReader csvReader) {
      return new PrivateConstructor(key, csvReader);
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Adds PrivateConstructor to DC.Data.PrivateConstructors.<br/>
    /// Throws an Exception when PrivateConstructor is already stored.
    /// </summary>
    public void Store() {
      if (Key>=0) {
        throw new Exception($"PrivateConstructor cannot be stored again in DC.Data, key {Key} is greater equal 0." + Environment.NewLine + ToString());
      }

      var isCancelled = false;
      onStoring(ref isCancelled);
      if (isCancelled) return;

      DC.Data._PrivateConstructors.Add(this);
      onStored();
#if DEBUG
      DC.Trace?.Invoke($"Stored PrivateConstructor #{GetHashCode()} @{Key}");
#endif
    }
    partial void onStoring(ref bool isCancelled);
    partial void onStored();


    /// <summary>
    /// Estimated number of UTF8 characters needed to write PrivateConstructor to CSV file
    /// </summary>
    public const int EstimatedLineLength = 150;


    /// <summary>
    /// Write PrivateConstructor to CSV file
    /// </summary>
    internal static void Write(PrivateConstructor privateConstructor, CsvWriter csvWriter) {
      privateConstructor.onCsvWrite();
      csvWriter.Write(privateConstructor.Text);
    }
    partial void onCsvWrite();


    /// <summary>
    /// Updates PrivateConstructor with the provided values
    /// </summary>
    public void Update(string text) {
      var clone = new PrivateConstructor(this);
      var isCancelled = false;
      onUpdating(text, ref isCancelled);
      if (isCancelled) return;

#if DEBUG
      DC.Trace?.Invoke($"Updating PrivateConstructor: {ToTraceString()}");
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
          DC.Data._PrivateConstructors.ItemHasChanged(clone, this);
        } else if (DC.Data.IsTransaction) {
          DC.Data.AddTransaction(new TransactionItem(26, TransactionActivityEnum.Update, Key, this, oldItem: clone));
        }
        HasChanged?.Invoke(clone, this);
      }
#if DEBUG
      DC.Trace?.Invoke($"Updated PrivateConstructor: {ToTraceString()}");
#endif
    }
    partial void onUpdating(string text, ref bool isCancelled);
    partial void onUpdated(PrivateConstructor old);


    /// <summary>
    /// Updates this PrivateConstructor with values from CSV file
    /// </summary>
    internal static void Update(PrivateConstructor privateConstructor, CsvReader csvReader){
      privateConstructor.Text = csvReader.ReadString();
      privateConstructor.onCsvUpdate();
    }
    partial void onCsvUpdate();


    /// <summary>
    /// Removes PrivateConstructor from DC.Data.PrivateConstructors.
    /// </summary>
    public void Release() {
      if (Key<0) {
        throw new Exception($"PrivateConstructor.Release(): PrivateConstructor '{this}' is not stored in DC.Data, key is {Key}.");
      }
      onReleasing();
      DC.Data._PrivateConstructors.Remove(Key);
      onReleased();
#if DEBUG
      DC.Trace?.Invoke($"Released PrivateConstructor @{Key} #{GetHashCode()}");
#endif
    }
    partial void onReleasing();
    partial void onReleased();


    /// <summary>
    /// Undoes the new() statement as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemNew(IStorageItem item) {
      var privateConstructor = (PrivateConstructor) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback new PrivateConstructor(): {privateConstructor.ToTraceString()}");
#endif
      privateConstructor.onRollbackItemNew();
    }
    partial void onRollbackItemNew();


    /// <summary>
    /// Releases PrivateConstructor from DC.Data.PrivateConstructors as part of a transaction rollback of Store().
    /// </summary>
    internal static void RollbackItemStore(IStorageItem item) {
      var privateConstructor = (PrivateConstructor) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback PrivateConstructor.Store(): {privateConstructor.ToTraceString()}");
#endif
      privateConstructor.onRollbackItemStored();
    }
    partial void onRollbackItemStored();


    /// <summary>
    /// Restores the PrivateConstructor item data as it was before the last update as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemUpdate(IStorageItem oldStorageItem, IStorageItem newStorageItem) {
      var oldItem = (PrivateConstructor) oldStorageItem;//an item clone with the values before item was updated
      var item = (PrivateConstructor) newStorageItem;//is the instance whose values should be restored
#if DEBUG
      DC.Trace?.Invoke($"Rolling back PrivateConstructor.Update(): {item.ToTraceString()}");
#endif

      // updated item: restore old values
      item.Text = oldItem.Text;
      item.onRollbackItemUpdated(oldItem);
#if DEBUG
      DC.Trace?.Invoke($"Rolled back PrivateConstructor.Update(): {item.ToTraceString()}");
#endif
    }
    partial void onRollbackItemUpdated(PrivateConstructor oldPrivateConstructor);


    /// <summary>
    /// Adds PrivateConstructor to DC.Data.PrivateConstructors as part of a transaction rollback of Release().
    /// </summary>
    internal static void RollbackItemRelease(IStorageItem item) {
      var privateConstructor = (PrivateConstructor) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback PrivateConstructor.Release(): {privateConstructor.ToTraceString()}");
#endif
      privateConstructor.onRollbackItemRelease();
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
