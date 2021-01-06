//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by StorageClassGenerator
//
//     Do not change code in this file, it will get lost when the file gets 
//     auto generated again. Write your code into ListWithPropertyNameParent.cs.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using StorageLib;


namespace DataModelSamples  {


  public partial class ListWithPropertyNameParent: IStorageItemGeneric<ListWithPropertyNameParent> {

    #region Properties
    //      ----------

    /// <summary>
    /// Unique identifier for ListWithPropertyNameParent. Gets set once ListWithPropertyNameParent gets added to DC.Data.
    /// </summary>
    public int Key { get; private set; }
    internal static void SetKey(IStorageItem listWithPropertyNameParent, int key, bool _) {
      ((ListWithPropertyNameParent)listWithPropertyNameParent).Key = key;
    }


    public string Name { get; private set; }


    public IReadOnlyList<ListWithPropertyNameChild> Children => children;
    readonly List<ListWithPropertyNameChild> children;


    /// <summary>
    /// Headers written to first line in CSV file
    /// </summary>
    internal static readonly string[] Headers = {"Key", "Name"};


    /// <summary>
    /// None existing ListWithPropertyNameParent
    /// </summary>
    internal static ListWithPropertyNameParent NoListWithPropertyNameParent = new ListWithPropertyNameParent("NoName", isStoring: false);
    #endregion


    #region Events
    //      ------

    /// <summary>
    /// Content of ListWithPropertyNameParent has changed. Gets only raised for changes occurring after loading DC.Data with previously stored data.
    /// </summary>
    public event Action</*old*/ListWithPropertyNameParent, /*new*/ListWithPropertyNameParent>? HasChanged;
    #endregion


    #region Constructors
    //      ------------

    /// <summary>
    /// ListWithPropertyNameParent Constructor. If isStoring is true, adds ListWithPropertyNameParent to DC.Data.ListWithPropertyNameParents.
    /// </summary>
    public ListWithPropertyNameParent(string name, bool isStoring = true) {
      Key = StorageExtensions.NoKey;
      Name = name;
      children = new List<ListWithPropertyNameChild>();
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
    public ListWithPropertyNameParent(ListWithPropertyNameParent original) {
    #pragma warning restore CS8618 //
      Key = StorageExtensions.NoKey;
      Name = original.Name;
      onCloned(this);
    }
    partial void onCloned(ListWithPropertyNameParent clone);


    /// <summary>
    /// Constructor for ListWithPropertyNameParent read from CSV file
    /// </summary>
    private ListWithPropertyNameParent(int key, CsvReader csvReader){
      Key = key;
      Name = csvReader.ReadString();
      children = new List<ListWithPropertyNameChild>();
      onCsvConstruct();
    }
    partial void onCsvConstruct();


    /// <summary>
    /// New ListWithPropertyNameParent read from CSV file
    /// </summary>
    internal static ListWithPropertyNameParent Create(int key, CsvReader csvReader) {
      return new ListWithPropertyNameParent(key, csvReader);
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Adds ListWithPropertyNameParent to DC.Data.ListWithPropertyNameParents.<br/>
    /// Throws an Exception when ListWithPropertyNameParent is already stored.
    /// </summary>
    public void Store() {
      if (Key>=0) {
        throw new Exception($"ListWithPropertyNameParent cannot be stored again in DC.Data, key {Key} is greater equal 0." + Environment.NewLine + ToString());
      }

      var isCancelled = false;
      onStoring(ref isCancelled);
      if (isCancelled) return;

      DC.Data._ListWithPropertyNameParents.Add(this);
      onStored();
    }
    partial void onStoring(ref bool isCancelled);
    partial void onStored();


    /// <summary>
    /// Estimated number of UTF8 characters needed to write ListWithPropertyNameParent to CSV file
    /// </summary>
    public const int EstimatedLineLength = 150;


    /// <summary>
    /// Write ListWithPropertyNameParent to CSV file
    /// </summary>
    internal static void Write(ListWithPropertyNameParent listWithPropertyNameParent, CsvWriter csvWriter) {
      listWithPropertyNameParent.onCsvWrite();
      csvWriter.Write(listWithPropertyNameParent.Name);
    }
    partial void onCsvWrite();


    /// <summary>
    /// Updates ListWithPropertyNameParent with the provided values
    /// </summary>
    public void Update(string name) {
      var clone = new ListWithPropertyNameParent(this);
      var isCancelled = false;
      onUpdating(name, ref isCancelled);
      if (isCancelled) return;


      //update properties and detect if any value has changed
      var isChangeDetected = false;
      if (Name!=name) {
        Name = name;
        isChangeDetected = true;
      }
      if (isChangeDetected) {
        onUpdated(clone);
        if (Key>=0) {
          DC.Data._ListWithPropertyNameParents.ItemHasChanged(clone, this);
        } else if (DC.Data.IsTransaction) {
          DC.Data.AddTransaction(new TransactionItem(23, TransactionActivityEnum.Update, Key, this, oldItem: clone));
        }
        HasChanged?.Invoke(clone, this);
      }
    }
    partial void onUpdating(string name, ref bool isCancelled);
    partial void onUpdated(ListWithPropertyNameParent old);


    /// <summary>
    /// Updates this ListWithPropertyNameParent with values from CSV file
    /// </summary>
    internal static void Update(ListWithPropertyNameParent listWithPropertyNameParent, CsvReader csvReader){
      listWithPropertyNameParent.Name = csvReader.ReadString();
      listWithPropertyNameParent.onCsvUpdate();
    }
    partial void onCsvUpdate();


    /// <summary>
    /// Add listWithPropertyNameChild to Children.
    /// </summary>
    internal void AddToChildren(ListWithPropertyNameChild listWithPropertyNameChild) {
#if DEBUG
      if (listWithPropertyNameChild==ListWithPropertyNameChild.NoListWithPropertyNameChild) throw new Exception();
      if ((listWithPropertyNameChild.Key>=0)&&(Key<0)) throw new Exception();
      if (children.Contains(listWithPropertyNameChild)) throw new Exception();
#endif
      children.Add(listWithPropertyNameChild);
      onAddedToChildren(listWithPropertyNameChild);
    }
    partial void onAddedToChildren(ListWithPropertyNameChild listWithPropertyNameChild);


    /// <summary>
    /// Removes listWithPropertyNameChild from ListWithPropertyNameParent.
    /// </summary>
    internal void RemoveFromChildren(ListWithPropertyNameChild listWithPropertyNameChild) {
#if DEBUG
      if (!children.Remove(listWithPropertyNameChild)) throw new Exception();
#else
        children.Remove(listWithPropertyNameChild);
#endif
      onRemovedFromChildren(listWithPropertyNameChild);
    }
    partial void onRemovedFromChildren(ListWithPropertyNameChild listWithPropertyNameChild);


    /// <summary>
    /// Removes ListWithPropertyNameParent from DC.Data.ListWithPropertyNameParents.
    /// </summary>
    public void Release() {
      if (Key<0) {
        throw new Exception($"ListWithPropertyNameParent.Release(): ListWithPropertyNameParent '{this}' is not stored in DC.Data, key is {Key}.");
      }
      foreach (var listWithPropertyNameChild in Children) {
        if (listWithPropertyNameChild?.Key>=0) {
          throw new Exception($"Cannot release ListWithPropertyNameParent '{this}' " + Environment.NewLine + 
            $"because '{listWithPropertyNameChild}' in ListWithPropertyNameParent.Children is still stored.");
        }
      }
      onReleased();
      DC.Data._ListWithPropertyNameParents.Remove(Key);
    }
    partial void onReleased();


    /// <summary>
    /// Undoes the new() statement as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemNew(IStorageItem item) {
      var listWithPropertyNameParent = (ListWithPropertyNameParent) item;
      listWithPropertyNameParent.onRollbackItemNew();
    }
    partial void onRollbackItemNew();


    /// <summary>
    /// Releases ListWithPropertyNameParent from DC.Data.ListWithPropertyNameParents as part of a transaction rollback of Store().
    /// </summary>
    internal static void RollbackItemStore(IStorageItem item) {
      var listWithPropertyNameParent = (ListWithPropertyNameParent) item;
      listWithPropertyNameParent.onRollbackItemStored();
    }
    partial void onRollbackItemStored();


    /// <summary>
    /// Restores the ListWithPropertyNameParent item data as it was before the last update as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemUpdate(IStorageItem oldStorageItem, IStorageItem newStorageItem) {
      var oldItem = (ListWithPropertyNameParent) oldStorageItem;//an item clone with the values before item was updated
      var item = (ListWithPropertyNameParent) newStorageItem;//is the instance whose values should be restored

      // updated item: restore old values
      item.Name = oldItem.Name;
      item.onRollbackItemUpdated(oldItem);
    }
    partial void onRollbackItemUpdated(ListWithPropertyNameParent oldListWithPropertyNameParent);


    /// <summary>
    /// Adds ListWithPropertyNameParent to DC.Data.ListWithPropertyNameParents as part of a transaction rollback of Release().
    /// </summary>
    internal static void RollbackItemRelease(IStorageItem item) {
      var listWithPropertyNameParent = (ListWithPropertyNameParent) item;
      listWithPropertyNameParent.onRollbackItemRelease();
    }
    partial void onRollbackItemRelease();


    /// <summary>
    /// Returns property values for tracing. Parents are shown with their key instead their content.
    /// </summary>
    public string ToTraceString() {
      var returnString =
        $"{this.GetKeyOrHash()}|" +
        $" {Name}";
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
        $" {Name}";
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
        $" Name: {Name}," +
        $" Children: {Children.Count};";
      onToString(ref returnString);
      return returnString;
    }
    partial void onToString(ref string returnString);
    #endregion
  }
}
