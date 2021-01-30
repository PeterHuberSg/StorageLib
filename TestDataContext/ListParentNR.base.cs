//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by StorageClassGenerator
//
//     Do not change code in this file, it will get lost when the file gets 
//     auto generated again. Write your code into ListParentNR.cs.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using StorageLib;


namespace TestContext  {


  public partial class ListParentNR: IStorageItem<ListParentNR> {

    #region Properties
    //      ----------

    /// <summary>
    /// Unique identifier for ListParentNR. Gets set once ListParentNR gets added to DC.Data.
    /// </summary>
    public int Key { get; private set; }
    internal static void SetKey(IStorageItem listParentNR, int key, bool isRollback) {
#if DEBUG
      if (isRollback) {
        if (key==StorageExtensions.NoKey) {
          DC.Trace?.Invoke($"Release ListParentNR key @{listParentNR.Key} #{listParentNR.GetHashCode()}");
        } else {
          DC.Trace?.Invoke($"Store ListParentNR key @{key} #{listParentNR.GetHashCode()}");
        }
      }
#endif
      ((ListParentNR)listParentNR).Key = key;
    }


    public string Text { get; private set; }


    public IStorageReadOnlyList<ListChild> Children => children;
    readonly StorageList<ListChild> children;


    /// <summary>
    /// Headers written to first line in CSV file
    /// </summary>
    internal static readonly string[] Headers = {"Key", "Text"};


    /// <summary>
    /// None existing ListParentNR, used as a temporary place holder when reading a CSV file
    /// which was not compacted. It might create first a later deleted item linking to a 
    /// deleted parent. In this case, the parent property gets set to NoListParentNR. Once the CSV
    /// file is completely read, that child will actually be deleted (released) and Verify()
    /// ensures that there are no stored children with links to NoListParentNR.
    /// </summary>
    internal static ListParentNR NoListParentNR = new ListParentNR("NoText", isStoring: false);
    #endregion


    #region Events
    //      ------

    /// <summary>
    /// Content of ListParentNR has changed. Gets only raised for changes occurring after loading DC.Data with previously stored data.
    /// </summary>
    public event Action</*old*/ListParentNR, /*new*/ListParentNR>? HasChanged;
    #endregion


    #region Constructors
    //      ------------

    /// <summary>
    /// ListParentNR Constructor. If isStoring is true, adds ListParentNR to DC.Data.ListParentNRs.
    /// </summary>
    public ListParentNR(string text, bool isStoring = true) {
      Key = StorageExtensions.NoKey;
      Text = text;
      children = new StorageList<ListChild>();
#if DEBUG
      DC.Trace?.Invoke($"new ListParentNR: {ToTraceString()}");
#endif
      onConstruct();
      if (DC.Data.IsTransaction) {
        DC.Data.AddTransaction(new TransactionItem(13,TransactionActivityEnum.New, Key, this));
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
    public ListParentNR(ListParentNR original) {
    #pragma warning restore CS8618 //
      Key = StorageExtensions.NoKey;
      Text = original.Text;
      onCloned(this);
    }
    partial void onCloned(ListParentNR clone);


    /// <summary>
    /// Constructor for ListParentNR read from CSV file
    /// </summary>
    private ListParentNR(int key, CsvReader csvReader){
      Key = key;
      Text = csvReader.ReadString();
      children = new StorageList<ListChild>();
      onCsvConstruct();
    }
    partial void onCsvConstruct();


    /// <summary>
    /// New ListParentNR read from CSV file
    /// </summary>
    internal static ListParentNR Create(int key, CsvReader csvReader) {
      return new ListParentNR(key, csvReader);
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Adds ListParentNR to DC.Data.ListParentNRs.<br/>
    /// Throws an Exception when ListParentNR is already stored.
    /// </summary>
    public void Store() {
      if (Key>=0) {
        throw new Exception($"ListParentNR cannot be stored again in DC.Data, key {Key} is greater equal 0." + Environment.NewLine + ToString());
      }

      var isCancelled = false;
      onStoring(ref isCancelled);
      if (isCancelled) return;

      DC.Data._ListParentNRs.Add(this);
      onStored();
#if DEBUG
      DC.Trace?.Invoke($"Stored ListParentNR #{GetHashCode()} @{Key}");
#endif
    }
    partial void onStoring(ref bool isCancelled);
    partial void onStored();


    /// <summary>
    /// Estimated number of UTF8 characters needed to write ListParentNR to CSV file
    /// </summary>
    public const int EstimatedLineLength = 150;


    /// <summary>
    /// Write ListParentNR to CSV file
    /// </summary>
    internal static void Write(ListParentNR listParentNR, CsvWriter csvWriter) {
      listParentNR.onCsvWrite();
      csvWriter.Write(listParentNR.Text);
    }
    partial void onCsvWrite();


    /// <summary>
    /// Updates ListParentNR with the provided values
    /// </summary>
    public void Update(string text) {
      var clone = new ListParentNR(this);
      var isCancelled = false;
      onUpdating(text, ref isCancelled);
      if (isCancelled) return;

#if DEBUG
      DC.Trace?.Invoke($"Updating ListParentNR: {ToTraceString()}");
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
          DC.Data._ListParentNRs.ItemHasChanged(clone, this);
        } else if (DC.Data.IsTransaction) {
          DC.Data.AddTransaction(new TransactionItem(13, TransactionActivityEnum.Update, Key, this, oldItem: clone));
        }
        HasChanged?.Invoke(clone, this);
      }
#if DEBUG
      DC.Trace?.Invoke($"Updated ListParentNR: {ToTraceString()}");
#endif
    }
    partial void onUpdating(string text, ref bool isCancelled);
    partial void onUpdated(ListParentNR old);


    /// <summary>
    /// Updates this ListParentNR with values from CSV file
    /// </summary>
    internal static void Update(ListParentNR listParentNR, CsvReader csvReader){
      listParentNR.Text = csvReader.ReadString();
      listParentNR.onCsvUpdate();
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
        $"{this.GetKeyOrHash()} ListParentNR.Children");
#endif
    }
    partial void onAddedToChildren(ListChild listChild);


    /// <summary>
    /// Removes listChild from ListParentNR.
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
        $"{this.GetKeyOrHash()} ListParentNR.Children");
#endif
    }
    partial void onRemovedFromChildren(ListChild listChild);


    /// <summary>
    /// Removes ListParentNR from DC.Data.ListParentNRs.
    /// </summary>
    public void Release() {
      if (Key<0) {
        throw new Exception($"ListParentNR.Release(): ListParentNR '{this}' is not stored in DC.Data, key is {Key}.");
      }
      foreach (var listChild in Children) {
        if (listChild?.Key>=0) {
          throw new Exception($"Cannot release ListParentNR '{this}' " + Environment.NewLine + 
            $"because '{listChild}' in ListParentNR.Children is still stored.");
        }
      }
      DC.Data._ListParentNRs.Remove(Key);
      onReleased();
#if DEBUG
      DC.Trace?.Invoke($"Released ListParentNR @{Key} #{GetHashCode()}");
#endif
    }
    partial void onReleased();


    /// <summary>
    /// Undoes the new() statement as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemNew(IStorageItem item) {
      var listParentNR = (ListParentNR) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback new ListParentNR(): {listParentNR.ToTraceString()}");
#endif
      listParentNR.onRollbackItemNew();
    }
    partial void onRollbackItemNew();


    /// <summary>
    /// Releases ListParentNR from DC.Data.ListParentNRs as part of a transaction rollback of Store().
    /// </summary>
    internal static void RollbackItemStore(IStorageItem item) {
      var listParentNR = (ListParentNR) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback ListParentNR.Store(): {listParentNR.ToTraceString()}");
#endif
      listParentNR.onRollbackItemStored();
    }
    partial void onRollbackItemStored();


    /// <summary>
    /// Restores the ListParentNR item data as it was before the last update as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemUpdate(IStorageItem oldStorageItem, IStorageItem newStorageItem) {
      var oldItem = (ListParentNR) oldStorageItem;//an item clone with the values before item was updated
      var item = (ListParentNR) newStorageItem;//is the instance whose values should be restored
#if DEBUG
      DC.Trace?.Invoke($"Rolling back ListParentNR.Update(): {item.ToTraceString()}");
#endif

      // updated item: restore old values
      item.Text = oldItem.Text;
      item.onRollbackItemUpdated(oldItem);
#if DEBUG
      DC.Trace?.Invoke($"Rolled back ListParentNR.Update(): {item.ToTraceString()}");
#endif
    }
    partial void onRollbackItemUpdated(ListParentNR oldListParentNR);


    /// <summary>
    /// Adds ListParentNR to DC.Data.ListParentNRs as part of a transaction rollback of Release().
    /// </summary>
    internal static void RollbackItemRelease(IStorageItem item) {
      var listParentNR = (ListParentNR) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback ListParentNR.Release(): {listParentNR.ToTraceString()}");
#endif
      listParentNR.onRollbackItemRelease();
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
        $" Children: {Children.Count}," +
        $" ChildrenStored: {Children.CountStoredItems};";
      onToString(ref returnString);
      return returnString;
    }
    partial void onToString(ref string returnString);
    #endregion
  }
}
