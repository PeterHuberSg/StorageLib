//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by StorageClassGenerator
//
//     Do not change code in this file, it will get lost when the file gets 
//     auto generated again. Write your code into SingleChildParent.cs.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using StorageLib;


namespace TestContext  {


  public partial class SingleChildParent: IStorageItemGeneric<SingleChildParent> {

    #region Properties
    //      ----------

    /// <summary>
    /// Unique identifier for SingleChildParent. Gets set once SingleChildParent gets added to DC.Data.
    /// </summary>
    public int Key { get; private set; }
    internal static void SetKey(IStorageItem singleChildParent, int key, bool isRollback) {
#if DEBUG
      if (isRollback) {
        if (key==StorageExtensions.NoKey) {
          DC.Trace?.Invoke($"Release SingleChildParent key @{singleChildParent.Key} #{singleChildParent.GetHashCode()}");
        } else {
          DC.Trace?.Invoke($"Store SingleChildParent key @{key} #{singleChildParent.GetHashCode()}");
        }
      }
#endif
      ((SingleChildParent)singleChildParent).Key = key;
    }


    public string Text { get; private set; }


    public SingleChildChild? Child { get; private set; }


    /// <summary>
    /// Headers written to first line in CSV file
    /// </summary>
    internal static readonly string[] Headers = {"Key", "Text"};


    /// <summary>
    /// None existing SingleChildParent, used as a temporary place holder when reading a CSV file
    /// which was not compacted. It might create first a later deleted item linking to a 
    /// deleted parent. In this case, the parent property gets set to NoSingleChildParent. Once the CSV
    /// file is completely read, that child will actually be deleted (released) and Verify()
    /// ensures that there are no stored children with links to NoSingleChildParent.
    /// </summary>
    internal static SingleChildParent NoSingleChildParent = new SingleChildParent("NoText", isStoring: false);
    #endregion


    #region Events
    //      ------

    /// <summary>
    /// Content of SingleChildParent has changed. Gets only raised for changes occurring after loading DC.Data with previously stored data.
    /// </summary>
    public event Action</*old*/SingleChildParent, /*new*/SingleChildParent>? HasChanged;
    #endregion


    #region Constructors
    //      ------------

    /// <summary>
    /// SingleChildParent Constructor. If isStoring is true, adds SingleChildParent to DC.Data.SingleChildParents.
    /// </summary>
    public SingleChildParent(string text, bool isStoring = true) {
      Key = StorageExtensions.NoKey;
      Text = text;
#if DEBUG
      DC.Trace?.Invoke($"new SingleChildParent: {ToTraceString()}");
#endif
      onConstruct();
      if (DC.Data.IsTransaction) {
        DC.Data.AddTransaction(new TransactionItem(5,TransactionActivityEnum.New, Key, this));
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
    public SingleChildParent(SingleChildParent original) {
    #pragma warning restore CS8618 //
      Key = StorageExtensions.NoKey;
      Text = original.Text;
      onCloned(this);
    }
    partial void onCloned(SingleChildParent clone);


    /// <summary>
    /// Constructor for SingleChildParent read from CSV file
    /// </summary>
    private SingleChildParent(int key, CsvReader csvReader){
      Key = key;
      Text = csvReader.ReadString();
      onCsvConstruct();
    }
    partial void onCsvConstruct();


    /// <summary>
    /// New SingleChildParent read from CSV file
    /// </summary>
    internal static SingleChildParent Create(int key, CsvReader csvReader) {
      return new SingleChildParent(key, csvReader);
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Adds SingleChildParent to DC.Data.SingleChildParents.<br/>
    /// Throws an Exception when SingleChildParent is already stored.
    /// </summary>
    public void Store() {
      if (Key>=0) {
        throw new Exception($"SingleChildParent cannot be stored again in DC.Data, key {Key} is greater equal 0." + Environment.NewLine + ToString());
      }

      var isCancelled = false;
      onStoring(ref isCancelled);
      if (isCancelled) return;

      DC.Data._SingleChildParents.Add(this);
      onStored();
#if DEBUG
      DC.Trace?.Invoke($"Stored SingleChildParent #{GetHashCode()} @{Key}");
#endif
    }
    partial void onStoring(ref bool isCancelled);
    partial void onStored();


    /// <summary>
    /// Estimated number of UTF8 characters needed to write SingleChildParent to CSV file
    /// </summary>
    public const int EstimatedLineLength = 150;


    /// <summary>
    /// Write SingleChildParent to CSV file
    /// </summary>
    internal static void Write(SingleChildParent singleChildParent, CsvWriter csvWriter) {
      singleChildParent.onCsvWrite();
      csvWriter.Write(singleChildParent.Text);
    }
    partial void onCsvWrite();


    /// <summary>
    /// Updates SingleChildParent with the provided values
    /// </summary>
    public void Update(string text) {
      var clone = new SingleChildParent(this);
      var isCancelled = false;
      onUpdating(text, ref isCancelled);
      if (isCancelled) return;

#if DEBUG
      DC.Trace?.Invoke($"Updating SingleChildParent: {ToTraceString()}");
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
          DC.Data._SingleChildParents.ItemHasChanged(clone, this);
        } else if (DC.Data.IsTransaction) {
          DC.Data.AddTransaction(new TransactionItem(5, TransactionActivityEnum.Update, Key, this, oldItem: clone));
        }
        HasChanged?.Invoke(clone, this);
      }
#if DEBUG
      DC.Trace?.Invoke($"Updated SingleChildParent: {ToTraceString()}");
#endif
    }
    partial void onUpdating(string text, ref bool isCancelled);
    partial void onUpdated(SingleChildParent old);


    /// <summary>
    /// Updates this SingleChildParent with values from CSV file
    /// </summary>
    internal static void Update(SingleChildParent singleChildParent, CsvReader csvReader){
      singleChildParent.Text = csvReader.ReadString();
      singleChildParent.onCsvUpdate();
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
        throw new Exception($"SingleChildParent.AddToChild(): '{Child}' is already assigned to Child, it is not possible to assign now '{singleChildChild}'.");
      }
      Child = singleChildChild;
      onAddedToChild(singleChildChild);
#if DEBUG
      DC.Trace?.Invoke($"Add SingleChildChild {singleChildChild.GetKeyOrHash()} to " +
        $"{this.GetKeyOrHash()} SingleChildParent.Child");
#endif
    }
    partial void onAddedToChild(SingleChildChild singleChildChild);


    /// <summary>
    /// Removes singleChildChild from SingleChildParent.
    /// </summary>
    internal void RemoveFromChild(SingleChildChild singleChildChild) {
#if DEBUG
      if (Child!=singleChildChild) {
        throw new Exception($"SingleChildParent.RemoveFromChild(): Child does not link to singleChildChild '{singleChildChild}' but '{Child}'.");
      }
#endif
      Child = null;
      onRemovedFromChild(singleChildChild);
#if DEBUG
      DC.Trace?.Invoke($"Remove SingleChildChild {singleChildChild.GetKeyOrHash()} from " +
        $"{this.GetKeyOrHash()} SingleChildParent.Child");
#endif
    }
    partial void onRemovedFromChild(SingleChildChild singleChildChild);


    /// <summary>
    /// Removes SingleChildParent from DC.Data.SingleChildParents.
    /// </summary>
    public void Release() {
      if (Key<0) {
        throw new Exception($"SingleChildParent.Release(): SingleChildParent '{this}' is not stored in DC.Data, key is {Key}.");
      }
      if (Child?.Key>=0) {
        throw new Exception($"Cannot release SingleChildParent '{this}' " + Environment.NewLine + 
          $"because '{Child}' in SingleChildParent.Child is still stored.");
      }
      DC.Data._SingleChildParents.Remove(Key);
      onReleased();
#if DEBUG
      DC.Trace?.Invoke($"Released SingleChildParent @{Key} #{GetHashCode()}");
#endif
    }
    partial void onReleased();


    /// <summary>
    /// Undoes the new() statement as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemNew(IStorageItem item) {
      var singleChildParent = (SingleChildParent) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback new SingleChildParent(): {singleChildParent.ToTraceString()}");
#endif
      singleChildParent.onRollbackItemNew();
    }
    partial void onRollbackItemNew();


    /// <summary>
    /// Releases SingleChildParent from DC.Data.SingleChildParents as part of a transaction rollback of Store().
    /// </summary>
    internal static void RollbackItemStore(IStorageItem item) {
      var singleChildParent = (SingleChildParent) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback SingleChildParent.Store(): {singleChildParent.ToTraceString()}");
#endif
      singleChildParent.onRollbackItemStored();
    }
    partial void onRollbackItemStored();


    /// <summary>
    /// Restores the SingleChildParent item data as it was before the last update as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemUpdate(IStorageItem oldStorageItem, IStorageItem newStorageItem) {
      var oldItem = (SingleChildParent) oldStorageItem;//an item clone with the values before item was updated
      var item = (SingleChildParent) newStorageItem;//is the instance whose values should be restored
#if DEBUG
      DC.Trace?.Invoke($"Rolling back SingleChildParent.Update(): {item.ToTraceString()}");
#endif

      // updated item: restore old values
      item.Text = oldItem.Text;
      item.onRollbackItemUpdated(oldItem);
#if DEBUG
      DC.Trace?.Invoke($"Rolled back SingleChildParent.Update(): {item.ToTraceString()}");
#endif
    }
    partial void onRollbackItemUpdated(SingleChildParent oldSingleChildParent);


    /// <summary>
    /// Adds SingleChildParent to DC.Data.SingleChildParents as part of a transaction rollback of Release().
    /// </summary>
    internal static void RollbackItemRelease(IStorageItem item) {
      var singleChildParent = (SingleChildParent) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback SingleChildParent.Release(): {singleChildParent.ToTraceString()}");
#endif
      singleChildParent.onRollbackItemRelease();
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
