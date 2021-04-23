//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by StorageClassGenerator
//
//     Do not change code in this file, it will get lost when the file gets 
//     auto generated again. Write your code into SortedList_1_MC_Parent.cs.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using StorageLib;


namespace DataModelSamples  {


  public partial class SortedList_1_MC_Parent: IStorageItem<SortedList_1_MC_Parent> {

    #region Properties
    //      ----------

    /// <summary>
    /// Unique identifier for SortedList_1_MC_Parent. Gets set once SortedList_1_MC_Parent gets added to DC.Data.
    /// </summary>
    public int Key { get; private set; }
    internal static void SetKey(IStorageItem sortedList_1_MC_Parent, int key, bool _) {
      ((SortedList_1_MC_Parent)sortedList_1_MC_Parent).Key = key;
    }


    public string Name { get; private set; }


    public IStorageReadOnlyDictionary<string, SortedList_1_MC_Child> Children => children;
    readonly StorageSortedList<string, SortedList_1_MC_Child> children;


    /// <summary>
    /// Headers written to first line in CSV file
    /// </summary>
    internal static readonly string[] Headers = {"Key", "Name"};


    /// <summary>
    /// None existing SortedList_1_MC_Parent, used as a temporary place holder when reading a CSV file
    /// which was not compacted. It might create first a later deleted item linking to a 
    /// deleted parent. In this case, the parent property gets set to NoSortedList_1_MC_Parent. Once the CSV
    /// file is completely read, that child will actually be deleted (released) and Verify()
    /// ensures that there are no stored children with links to NoSortedList_1_MC_Parent.
    /// </summary>
    internal static SortedList_1_MC_Parent NoSortedList_1_MC_Parent = new SortedList_1_MC_Parent("NoName", isStoring: false);
    #endregion


    #region Events
    //      ------

    /// <summary>
    /// Content of SortedList_1_MC_Parent has changed. Gets only raised for changes occurring after loading DC.Data with previously stored data.
    /// </summary>
    public event Action</*old*/SortedList_1_MC_Parent, /*new*/SortedList_1_MC_Parent>? HasChanged;
    #endregion


    #region Constructors
    //      ------------

    /// <summary>
    /// SortedList_1_MC_Parent Constructor. If isStoring is true, adds SortedList_1_MC_Parent to DC.Data.SortedList_1_MC_Parents.
    /// </summary>
    public SortedList_1_MC_Parent(string name, bool isStoring = true) {
      Key = StorageExtensions.NoKey;
      Name = name;
      children = new StorageSortedList<string, SortedList_1_MC_Child>();
      onConstruct();
      if (DC.Data.IsTransaction) {
        DC.Data.AddTransaction(new TransactionItem(31,TransactionActivityEnum.New, Key, this));
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
    public SortedList_1_MC_Parent(SortedList_1_MC_Parent original) {
    #pragma warning restore CS8618 //
      Key = StorageExtensions.NoKey;
      Name = original.Name;
      onCloned(this);
    }
    partial void onCloned(SortedList_1_MC_Parent clone);


    /// <summary>
    /// Constructor for SortedList_1_MC_Parent read from CSV file
    /// </summary>
    private SortedList_1_MC_Parent(int key, CsvReader csvReader){
      Key = key;
      Name = csvReader.ReadString();
      children = new StorageSortedList<string, SortedList_1_MC_Child>();
      onCsvConstruct();
    }
    partial void onCsvConstruct();


    /// <summary>
    /// New SortedList_1_MC_Parent read from CSV file
    /// </summary>
    internal static SortedList_1_MC_Parent Create(int key, CsvReader csvReader) {
      return new SortedList_1_MC_Parent(key, csvReader);
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Adds SortedList_1_MC_Parent to DC.Data.SortedList_1_MC_Parents.<br/>
    /// Throws an Exception when SortedList_1_MC_Parent is already stored.
    /// </summary>
    public void Store() {
      if (Key>=0) {
        throw new Exception($"SortedList_1_MC_Parent cannot be stored again in DC.Data, key {Key} is greater equal 0." + Environment.NewLine + ToString());
      }

      var isCancelled = false;
      onStoring(ref isCancelled);
      if (isCancelled) return;

      DC.Data._SortedList_1_MC_Parents.Add(this);
      onStored();
    }
    partial void onStoring(ref bool isCancelled);
    partial void onStored();


    /// <summary>
    /// Estimated number of UTF8 characters needed to write SortedList_1_MC_Parent to CSV file
    /// </summary>
    public const int EstimatedLineLength = 150;


    /// <summary>
    /// Write SortedList_1_MC_Parent to CSV file
    /// </summary>
    internal static void Write(SortedList_1_MC_Parent sortedList_1_MC_Parent, CsvWriter csvWriter) {
      sortedList_1_MC_Parent.onCsvWrite();
      csvWriter.Write(sortedList_1_MC_Parent.Name);
    }
    partial void onCsvWrite();


    /// <summary>
    /// Updates SortedList_1_MC_Parent with the provided values
    /// </summary>
    public void Update(string name) {
      var clone = new SortedList_1_MC_Parent(this);
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
          DC.Data._SortedList_1_MC_Parents.ItemHasChanged(clone, this);
        } else if (DC.Data.IsTransaction) {
          DC.Data.AddTransaction(new TransactionItem(31, TransactionActivityEnum.Update, Key, this, oldItem: clone));
        }
        HasChanged?.Invoke(clone, this);
      }
    }
    partial void onUpdating(string name, ref bool isCancelled);
    partial void onUpdated(SortedList_1_MC_Parent old);


    /// <summary>
    /// Updates this SortedList_1_MC_Parent with values from CSV file
    /// </summary>
    internal static void Update(SortedList_1_MC_Parent sortedList_1_MC_Parent, CsvReader csvReader){
      sortedList_1_MC_Parent.Name = csvReader.ReadString();
      sortedList_1_MC_Parent.onCsvUpdate();
    }
    partial void onCsvUpdate();


    /// <summary>
    /// Add sortedList_1_MC_Child to Children.
    /// </summary>
    internal void AddToChildren(SortedList_1_MC_Child sortedList_1_MC_Child) {
#if DEBUG
      if (sortedList_1_MC_Child==SortedList_1_MC_Child.NoSortedList_1_MC_Child) throw new Exception();
      if ((sortedList_1_MC_Child.Key>=0)&&(Key<0)) throw new Exception();
      if (children.ContainsKey(sortedList_1_MC_Child.Name)) throw new Exception();
#endif
      children.Add(sortedList_1_MC_Child.Name, sortedList_1_MC_Child);
      onAddedToChildren(sortedList_1_MC_Child);
    }
    partial void onAddedToChildren(SortedList_1_MC_Child sortedList_1_MC_Child);


    /// <summary>
    /// Removes sortedList_1_MC_Child from SortedList_1_MC_Parent.
    /// </summary>
    internal void RemoveFromChildren(SortedList_1_MC_Child sortedList_1_MC_Child) {
#if DEBUG
      if (!children.Remove(sortedList_1_MC_Child.Name)) throw new Exception();
#else
        children.Remove(sortedList_1_MC_Child.Name);
#endif
      onRemovedFromChildren(sortedList_1_MC_Child);
    }
    partial void onRemovedFromChildren(SortedList_1_MC_Child sortedList_1_MC_Child);


    /// <summary>
    /// Removes SortedList_1_MC_Parent from DC.Data.SortedList_1_MC_Parents.
    /// </summary>
    public void Release() {
      if (Key<0) {
        throw new Exception($"SortedList_1_MC_Parent.Release(): SortedList_1_MC_Parent '{this}' is not stored in DC.Data, key is {Key}.");
      }
      foreach (var sortedList_1_MC_Child in Children.Values) {
        if (sortedList_1_MC_Child?.Key>=0) {
          throw new Exception($"Cannot release SortedList_1_MC_Parent '{this}' " + Environment.NewLine + 
            $"because '{sortedList_1_MC_Child}' in SortedList_1_MC_Parent.Children is still stored.");
        }
      }
      onReleasing();
      DC.Data._SortedList_1_MC_Parents.Remove(Key);
      onReleased();
    }
    partial void onReleasing();
    partial void onReleased();


    /// <summary>
    /// Undoes the new() statement as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemNew(IStorageItem item) {
      var sortedList_1_MC_Parent = (SortedList_1_MC_Parent) item;
      sortedList_1_MC_Parent.onRollbackItemNew();
    }
    partial void onRollbackItemNew();


    /// <summary>
    /// Releases SortedList_1_MC_Parent from DC.Data.SortedList_1_MC_Parents as part of a transaction rollback of Store().
    /// </summary>
    internal static void RollbackItemStore(IStorageItem item) {
      var sortedList_1_MC_Parent = (SortedList_1_MC_Parent) item;
      sortedList_1_MC_Parent.onRollbackItemStored();
    }
    partial void onRollbackItemStored();


    /// <summary>
    /// Restores the SortedList_1_MC_Parent item data as it was before the last update as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemUpdate(IStorageItem oldStorageItem, IStorageItem newStorageItem) {
      var oldItem = (SortedList_1_MC_Parent) oldStorageItem;//an item clone with the values before item was updated
      var item = (SortedList_1_MC_Parent) newStorageItem;//is the instance whose values should be restored

      // updated item: restore old values
      item.Name = oldItem.Name;
      item.onRollbackItemUpdated(oldItem);
    }
    partial void onRollbackItemUpdated(SortedList_1_MC_Parent oldSortedList_1_MC_Parent);


    /// <summary>
    /// Adds SortedList_1_MC_Parent to DC.Data.SortedList_1_MC_Parents as part of a transaction rollback of Release().
    /// </summary>
    internal static void RollbackItemRelease(IStorageItem item) {
      var sortedList_1_MC_Parent = (SortedList_1_MC_Parent) item;
      sortedList_1_MC_Parent.onRollbackItemRelease();
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
        $" Children: {Children.Count}," +
        $" ChildrenStored: {Children.CountStoredItems};";
      onToString(ref returnString);
      return returnString;
    }
    partial void onToString(ref string returnString);
    #endregion
  }
}
