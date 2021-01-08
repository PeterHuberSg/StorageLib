//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by StorageClassGenerator
//
//     Do not change code in this file, it will get lost when the file gets 
//     auto generated again. Write your code into ListParentR.cs.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using StorageLib;


namespace TestContext  {


  public partial class ListParentR: IStorageItemGeneric<ListParentR> {

    #region Properties
    //      ----------

    /// <summary>
    /// Unique identifier for ListParentR. Gets set once ListParentR gets added to DC.Data.
    /// </summary>
    public int Key { get; private set; }
    internal static void SetKey(IStorageItem listParentR, int key, bool isRollback) {
#if DEBUG
      if (isRollback) {
        if (key==StorageExtensions.NoKey) {
          DC.Trace?.Invoke($"Release ListParentR key @{listParentR.Key} #{listParentR.GetHashCode()}");
        } else {
          DC.Trace?.Invoke($"Store ListParentR key @{key} #{listParentR.GetHashCode()}");
        }
      }
#endif
      ((ListParentR)listParentR).Key = key;
    }


    public string Text { get; private set; }


    public IReadOnlyList<ListChild> Children => children;
    readonly List<ListChild> children;


    /// <summary>
    /// Headers written to first line in CSV file
    /// </summary>
    internal static readonly string[] Headers = {"Key", "Text"};


    /// <summary>
    /// None existing ListParentR, used as a temporary place holder when reading a CSV file
    /// which was not compacted. It might create first a later deleted item linking to a 
    /// deleted parent. In this case, the parent property gets set to NoListParentR. Once the CSV
    /// file is completely read, that child will actually be deleted (released) and Verify()
    /// ensures that there are no stored children with links to NoListParentR.
    /// </summary>
    internal static ListParentR NoListParentR = new ListParentR("NoText", isStoring: false);
    #endregion


    #region Events
    //      ------

    /// <summary>
    /// Content of ListParentR has changed. Gets only raised for changes occurring after loading DC.Data with previously stored data.
    /// </summary>
    public event Action</*old*/ListParentR, /*new*/ListParentR>? HasChanged;
    #endregion


    #region Constructors
    //      ------------

    /// <summary>
    /// ListParentR Constructor. If isStoring is true, adds ListParentR to DC.Data.ListParentRs.
    /// </summary>
    public ListParentR(string text, bool isStoring = true) {
      Key = StorageExtensions.NoKey;
      Text = text;
      children = new List<ListChild>();
#if DEBUG
      DC.Trace?.Invoke($"new ListParentR: {ToTraceString()}");
#endif
      onConstruct();
      if (DC.Data.IsTransaction) {
        DC.Data.AddTransaction(new TransactionItem(12,TransactionActivityEnum.New, Key, this));
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
    public ListParentR(ListParentR original) {
    #pragma warning restore CS8618 //
      Key = StorageExtensions.NoKey;
      Text = original.Text;
      onCloned(this);
    }
    partial void onCloned(ListParentR clone);


    /// <summary>
    /// Constructor for ListParentR read from CSV file
    /// </summary>
    private ListParentR(int key, CsvReader csvReader){
      Key = key;
      Text = csvReader.ReadString();
      children = new List<ListChild>();
      onCsvConstruct();
    }
    partial void onCsvConstruct();


    /// <summary>
    /// New ListParentR read from CSV file
    /// </summary>
    internal static ListParentR Create(int key, CsvReader csvReader) {
      return new ListParentR(key, csvReader);
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Adds ListParentR to DC.Data.ListParentRs.<br/>
    /// Throws an Exception when ListParentR is already stored.
    /// </summary>
    public void Store() {
      if (Key>=0) {
        throw new Exception($"ListParentR cannot be stored again in DC.Data, key {Key} is greater equal 0." + Environment.NewLine + ToString());
      }

      var isCancelled = false;
      onStoring(ref isCancelled);
      if (isCancelled) return;

      DC.Data._ListParentRs.Add(this);
      onStored();
#if DEBUG
      DC.Trace?.Invoke($"Stored ListParentR #{GetHashCode()} @{Key}");
#endif
    }
    partial void onStoring(ref bool isCancelled);
    partial void onStored();


    /// <summary>
    /// Estimated number of UTF8 characters needed to write ListParentR to CSV file
    /// </summary>
    public const int EstimatedLineLength = 150;


    /// <summary>
    /// Write ListParentR to CSV file
    /// </summary>
    internal static void Write(ListParentR listParentR, CsvWriter csvWriter) {
      listParentR.onCsvWrite();
      csvWriter.Write(listParentR.Text);
    }
    partial void onCsvWrite();


    /// <summary>
    /// Updates ListParentR with the provided values
    /// </summary>
    public void Update(string text) {
      var clone = new ListParentR(this);
      var isCancelled = false;
      onUpdating(text, ref isCancelled);
      if (isCancelled) return;

#if DEBUG
      DC.Trace?.Invoke($"Updating ListParentR: {ToTraceString()}");
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
          DC.Data._ListParentRs.ItemHasChanged(clone, this);
        } else if (DC.Data.IsTransaction) {
          DC.Data.AddTransaction(new TransactionItem(12, TransactionActivityEnum.Update, Key, this, oldItem: clone));
        }
        HasChanged?.Invoke(clone, this);
      }
#if DEBUG
      DC.Trace?.Invoke($"Updated ListParentR: {ToTraceString()}");
#endif
    }
    partial void onUpdating(string text, ref bool isCancelled);
    partial void onUpdated(ListParentR old);


    /// <summary>
    /// Updates this ListParentR with values from CSV file
    /// </summary>
    internal static void Update(ListParentR listParentR, CsvReader csvReader){
      listParentR.Text = csvReader.ReadString();
      listParentR.onCsvUpdate();
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
        $"{this.GetKeyOrHash()} ListParentR.Children");
#endif
    }
    partial void onAddedToChildren(ListChild listChild);


    /// <summary>
    /// Removes listChild from ListParentR.
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
        $"{this.GetKeyOrHash()} ListParentR.Children");
#endif
    }
    partial void onRemovedFromChildren(ListChild listChild);


    /// <summary>
    /// Removes ListParentR from DC.Data.ListParentRs.
    /// </summary>
    public void Release() {
      if (Key<0) {
        throw new Exception($"ListParentR.Release(): ListParentR '{this}' is not stored in DC.Data, key is {Key}.");
      }
      foreach (var listChild in Children) {
        if (listChild?.Key>=0) {
          throw new Exception($"Cannot release ListParentR '{this}' " + Environment.NewLine + 
            $"because '{listChild}' in ListParentR.Children is still stored.");
        }
      }
      onReleased();
      DC.Data._ListParentRs.Remove(Key);
#if DEBUG
      DC.Trace?.Invoke($"Released ListParentR @{Key} #{GetHashCode()}");
#endif
    }
    partial void onReleased();


    /// <summary>
    /// Undoes the new() statement as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemNew(IStorageItem item) {
      var listParentR = (ListParentR) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback new ListParentR(): {listParentR.ToTraceString()}");
#endif
      listParentR.onRollbackItemNew();
    }
    partial void onRollbackItemNew();


    /// <summary>
    /// Releases ListParentR from DC.Data.ListParentRs as part of a transaction rollback of Store().
    /// </summary>
    internal static void RollbackItemStore(IStorageItem item) {
      var listParentR = (ListParentR) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback ListParentR.Store(): {listParentR.ToTraceString()}");
#endif
      listParentR.onRollbackItemStored();
    }
    partial void onRollbackItemStored();


    /// <summary>
    /// Restores the ListParentR item data as it was before the last update as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemUpdate(IStorageItem oldStorageItem, IStorageItem newStorageItem) {
      var oldItem = (ListParentR) oldStorageItem;//an item clone with the values before item was updated
      var item = (ListParentR) newStorageItem;//is the instance whose values should be restored
#if DEBUG
      DC.Trace?.Invoke($"Rolling back ListParentR.Update(): {item.ToTraceString()}");
#endif

      // updated item: restore old values
      item.Text = oldItem.Text;
      item.onRollbackItemUpdated(oldItem);
#if DEBUG
      DC.Trace?.Invoke($"Rolled back ListParentR.Update(): {item.ToTraceString()}");
#endif
    }
    partial void onRollbackItemUpdated(ListParentR oldListParentR);


    /// <summary>
    /// Adds ListParentR to DC.Data.ListParentRs as part of a transaction rollback of Release().
    /// </summary>
    internal static void RollbackItemRelease(IStorageItem item) {
      var listParentR = (ListParentR) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback ListParentR.Release(): {listParentR.ToTraceString()}");
#endif
      listParentR.onRollbackItemRelease();
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
