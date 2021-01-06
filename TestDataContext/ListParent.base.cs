//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by StorageClassGenerator
//
//     Do not change code in this file, it will get lost when the file gets 
//     auto generated again. Write your code into ListParent.cs.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using StorageLib;


namespace TestContext  {


  public partial class ListParent: IStorageItemGeneric<ListParent> {

    #region Properties
    //      ----------

    /// <summary>
    /// Unique identifier for ListParent. Gets set once ListParent gets added to DC.Data.
    /// </summary>
    public int Key { get; private set; }
    internal static void SetKey(IStorageItem listParent, int key, bool isRollback) {
#if DEBUG
      if (isRollback) {
        if (key==StorageExtensions.NoKey) {
          DC.Trace?.Invoke($"Release ListParent key @{listParent.Key} #{listParent.GetHashCode()}");
        } else {
          DC.Trace?.Invoke($"Store ListParent key @{key} #{listParent.GetHashCode()}");
        }
      }
#endif
      ((ListParent)listParent).Key = key;
    }


    public string Text { get; private set; }


    public IReadOnlyList<ListChild> Children => children;
    readonly List<ListChild> children;


    /// <summary>
    /// Headers written to first line in CSV file
    /// </summary>
    internal static readonly string[] Headers = {"Key", "Text"};


    /// <summary>
    /// None existing ListParent
    /// </summary>
    internal static ListParent NoListParent = new ListParent("NoText", isStoring: false);
    #endregion


    #region Events
    //      ------

    /// <summary>
    /// Content of ListParent has changed. Gets only raised for changes occurring after loading DC.Data with previously stored data.
    /// </summary>
    public event Action</*old*/ListParent, /*new*/ListParent>? HasChanged;
    #endregion


    #region Constructors
    //      ------------

    /// <summary>
    /// ListParent Constructor. If isStoring is true, adds ListParent to DC.Data.ListParents.
    /// </summary>
    public ListParent(string text, bool isStoring = true) {
      Key = StorageExtensions.NoKey;
      Text = text;
      children = new List<ListChild>();
#if DEBUG
      DC.Trace?.Invoke($"new ListParent: {ToTraceString()}");
#endif
      onConstruct();
      if (DC.Data.IsTransaction) {
        DC.Data.AddTransaction(new TransactionItem(10,TransactionActivityEnum.New, Key, this));
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
    public ListParent(ListParent original) {
    #pragma warning restore CS8618 //
      Key = StorageExtensions.NoKey;
      Text = original.Text;
      onCloned(this);
    }
    partial void onCloned(ListParent clone);


    /// <summary>
    /// Constructor for ListParent read from CSV file
    /// </summary>
    private ListParent(int key, CsvReader csvReader){
      Key = key;
      Text = csvReader.ReadString();
      children = new List<ListChild>();
      onCsvConstruct();
    }
    partial void onCsvConstruct();


    /// <summary>
    /// New ListParent read from CSV file
    /// </summary>
    internal static ListParent Create(int key, CsvReader csvReader) {
      return new ListParent(key, csvReader);
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Adds ListParent to DC.Data.ListParents.<br/>
    /// Throws an Exception when ListParent is already stored.
    /// </summary>
    public void Store() {
      if (Key>=0) {
        throw new Exception($"ListParent cannot be stored again in DC.Data, key {Key} is greater equal 0." + Environment.NewLine + ToString());
      }

      var isCancelled = false;
      onStoring(ref isCancelled);
      if (isCancelled) return;

      DC.Data._ListParents.Add(this);
      onStored();
#if DEBUG
      DC.Trace?.Invoke($"Stored ListParent #{GetHashCode()} @{Key}");
#endif
    }
    partial void onStoring(ref bool isCancelled);
    partial void onStored();


    /// <summary>
    /// Estimated number of UTF8 characters needed to write ListParent to CSV file
    /// </summary>
    public const int EstimatedLineLength = 150;


    /// <summary>
    /// Write ListParent to CSV file
    /// </summary>
    internal static void Write(ListParent listParent, CsvWriter csvWriter) {
      listParent.onCsvWrite();
      csvWriter.Write(listParent.Text);
    }
    partial void onCsvWrite();


    /// <summary>
    /// Updates ListParent with the provided values
    /// </summary>
    public void Update(string text) {
      var clone = new ListParent(this);
      var isCancelled = false;
      onUpdating(text, ref isCancelled);
      if (isCancelled) return;

#if DEBUG
      DC.Trace?.Invoke($"Updating ListParent: {ToTraceString()}");
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
          DC.Data._ListParents.ItemHasChanged(clone, this);
        } else if (DC.Data.IsTransaction) {
          DC.Data.AddTransaction(new TransactionItem(10, TransactionActivityEnum.Update, Key, this, oldItem: clone));
        }
        HasChanged?.Invoke(clone, this);
      }
#if DEBUG
      DC.Trace?.Invoke($"Updated ListParent: {ToTraceString()}");
#endif
    }
    partial void onUpdating(string text, ref bool isCancelled);
    partial void onUpdated(ListParent old);


    /// <summary>
    /// Updates this ListParent with values from CSV file
    /// </summary>
    internal static void Update(ListParent listParent, CsvReader csvReader){
      listParent.Text = csvReader.ReadString();
      listParent.onCsvUpdate();
    }
    partial void onCsvUpdate();


    /// <summary>
    /// Add listChild to Children.
    /// </summary>
    internal void AddToChildren(ListChild listChild) {
#if DEBUG
      if (listChild==ListChild.NoListChild) throw new Exception();
      if ((listChild.Key>=0)&&(Key<0)) throw new Exception();
      if (children.Contains(listChild)) throw new Exception();
#endif
      children.Add(listChild);
      onAddedToChildren(listChild);
#if DEBUG
      DC.Trace?.Invoke($"Add ListChild {listChild.GetKeyOrHash()} to " +
        $"{this.GetKeyOrHash()} ListParent.Children");
#endif
    }
    partial void onAddedToChildren(ListChild listChild);


    /// <summary>
    /// Removes listChild from ListParent.
    /// </summary>
    internal void RemoveFromChildren(ListChild listChild) {
#if DEBUG
      if (!children.Remove(listChild)) throw new Exception();
#else
        children.Remove(listChild);
#endif
      onRemovedFromChildren(listChild);
#if DEBUG
      DC.Trace?.Invoke($"Remove ListChild {listChild.GetKeyOrHash()} from " +
        $"{this.GetKeyOrHash()} ListParent.Children");
#endif
    }
    partial void onRemovedFromChildren(ListChild listChild);


    /// <summary>
    /// Removes ListParent from DC.Data.ListParents.
    /// </summary>
    public void Release() {
      if (Key<0) {
        throw new Exception($"ListParent.Release(): ListParent '{this}' is not stored in DC.Data, key is {Key}.");
      }
      foreach (var listChild in Children) {
        if (listChild?.Key>=0) {
          throw new Exception($"Cannot release ListParent '{this}' " + Environment.NewLine + 
            $"because '{listChild}' in ListParent.Children is still stored.");
        }
      }
      onReleased();
      DC.Data._ListParents.Remove(Key);
#if DEBUG
      DC.Trace?.Invoke($"Released ListParent @{Key} #{GetHashCode()}");
#endif
    }
    partial void onReleased();


    /// <summary>
    /// Undoes the new() statement as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemNew(IStorageItem item) {
      var listParent = (ListParent) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback new ListParent(): {listParent.ToTraceString()}");
#endif
      listParent.onRollbackItemNew();
    }
    partial void onRollbackItemNew();


    /// <summary>
    /// Releases ListParent from DC.Data.ListParents as part of a transaction rollback of Store().
    /// </summary>
    internal static void RollbackItemStore(IStorageItem item) {
      var listParent = (ListParent) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback ListParent.Store(): {listParent.ToTraceString()}");
#endif
      listParent.onRollbackItemStored();
    }
    partial void onRollbackItemStored();


    /// <summary>
    /// Restores the ListParent item data as it was before the last update as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemUpdate(IStorageItem oldStorageItem, IStorageItem newStorageItem) {
      var oldItem = (ListParent) oldStorageItem;//an item clone with the values before item was updated
      var item = (ListParent) newStorageItem;//is the instance whose values should be restored
#if DEBUG
      DC.Trace?.Invoke($"Rolling back ListParent.Update(): {item.ToTraceString()}");
#endif

      // updated item: restore old values
      item.Text = oldItem.Text;
      item.onRollbackItemUpdated(oldItem);
#if DEBUG
      DC.Trace?.Invoke($"Rolled back ListParent.Update(): {item.ToTraceString()}");
#endif
    }
    partial void onRollbackItemUpdated(ListParent oldListParent);


    /// <summary>
    /// Adds ListParent to DC.Data.ListParents as part of a transaction rollback of Release().
    /// </summary>
    internal static void RollbackItemRelease(IStorageItem item) {
      var listParent = (ListParent) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback ListParent.Release(): {listParent.ToTraceString()}");
#endif
      listParent.onRollbackItemRelease();
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
        $" Children: {Children.Count};";
      onToString(ref returnString);
      return returnString;
    }
    partial void onToString(ref string returnString);
    #endregion
  }
}
