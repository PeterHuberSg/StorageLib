//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by StorageClassGenerator
//
//     Do not change code in this file, it will get lost when the file gets 
//     auto generated again. Write your code into DictionaryWithPropertyNameParent.cs.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using StorageLib;


namespace DataModelSamples  {


  public partial class DictionaryWithPropertyNameParent: IStorageItem<DictionaryWithPropertyNameParent> {

    #region Properties
    //      ----------

    /// <summary>
    /// Unique identifier for DictionaryWithPropertyNameParent. Gets set once DictionaryWithPropertyNameParent gets added to DC.Data.
    /// </summary>
    public int Key { get; private set; }
    internal static void SetKey(IStorageItem dictionaryWithPropertyNameParent, int key, bool _) {
      ((DictionaryWithPropertyNameParent)dictionaryWithPropertyNameParent).Key = key;
    }


    public string Name { get; private set; }


    public IStorageReadOnlyDictionary<DateTime, DictionaryWithPropertyNameChild> Children => children;
    readonly StorageDictionary<DateTime, DictionaryWithPropertyNameChild> children;


    /// <summary>
    /// Headers written to first line in CSV file
    /// </summary>
    internal static readonly string[] Headers = {"Key", "Name"};


    /// <summary>
    /// None existing DictionaryWithPropertyNameParent, used as a temporary place holder when reading a CSV file
    /// which was not compacted. It might create first a later deleted item linking to a 
    /// deleted parent. In this case, the parent property gets set to NoDictionaryWithPropertyNameParent. Once the CSV
    /// file is completely read, that child will actually be deleted (released) and Verify()
    /// ensures that there are no stored children with links to NoDictionaryWithPropertyNameParent.
    /// </summary>
    internal static DictionaryWithPropertyNameParent NoDictionaryWithPropertyNameParent = new DictionaryWithPropertyNameParent("NoName", isStoring: false);
    #endregion


    #region Events
    //      ------

    /// <summary>
    /// Content of DictionaryWithPropertyNameParent has changed. Gets only raised for changes occurring after loading DC.Data with previously stored data.
    /// </summary>
    public event Action</*old*/DictionaryWithPropertyNameParent, /*new*/DictionaryWithPropertyNameParent>? HasChanged;
    #endregion


    #region Constructors
    //      ------------

    /// <summary>
    /// DictionaryWithPropertyNameParent Constructor. If isStoring is true, adds DictionaryWithPropertyNameParent to DC.Data.DictionaryWithPropertyNameParents.
    /// </summary>
    public DictionaryWithPropertyNameParent(string name, bool isStoring = true) {
      Key = StorageExtensions.NoKey;
      Name = name;
      children = new StorageDictionary<DateTime, DictionaryWithPropertyNameChild>();
      onConstruct();
      if (DC.Data.IsTransaction) {
        DC.Data.AddTransaction(new TransactionItem(29,TransactionActivityEnum.New, Key, this));
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
    public DictionaryWithPropertyNameParent(DictionaryWithPropertyNameParent original) {
    #pragma warning restore CS8618 //
      Key = StorageExtensions.NoKey;
      Name = original.Name;
      onCloned(this);
    }
    partial void onCloned(DictionaryWithPropertyNameParent clone);


    /// <summary>
    /// Constructor for DictionaryWithPropertyNameParent read from CSV file
    /// </summary>
    private DictionaryWithPropertyNameParent(int key, CsvReader csvReader){
      Key = key;
      Name = csvReader.ReadString();
      children = new StorageDictionary<DateTime, DictionaryWithPropertyNameChild>();
      onCsvConstruct();
    }
    partial void onCsvConstruct();


    /// <summary>
    /// New DictionaryWithPropertyNameParent read from CSV file
    /// </summary>
    internal static DictionaryWithPropertyNameParent Create(int key, CsvReader csvReader) {
      return new DictionaryWithPropertyNameParent(key, csvReader);
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Adds DictionaryWithPropertyNameParent to DC.Data.DictionaryWithPropertyNameParents.<br/>
    /// Throws an Exception when DictionaryWithPropertyNameParent is already stored.
    /// </summary>
    public void Store() {
      if (Key>=0) {
        throw new Exception($"DictionaryWithPropertyNameParent cannot be stored again in DC.Data, key {Key} is greater equal 0." + Environment.NewLine + ToString());
      }

      var isCancelled = false;
      onStoring(ref isCancelled);
      if (isCancelled) return;

      DC.Data._DictionaryWithPropertyNameParents.Add(this);
      onStored();
    }
    partial void onStoring(ref bool isCancelled);
    partial void onStored();


    /// <summary>
    /// Estimated number of UTF8 characters needed to write DictionaryWithPropertyNameParent to CSV file
    /// </summary>
    public const int EstimatedLineLength = 150;


    /// <summary>
    /// Write DictionaryWithPropertyNameParent to CSV file
    /// </summary>
    internal static void Write(DictionaryWithPropertyNameParent dictionaryWithPropertyNameParent, CsvWriter csvWriter) {
      dictionaryWithPropertyNameParent.onCsvWrite();
      csvWriter.Write(dictionaryWithPropertyNameParent.Name);
    }
    partial void onCsvWrite();


    /// <summary>
    /// Updates DictionaryWithPropertyNameParent with the provided values
    /// </summary>
    public void Update(string name) {
      var clone = new DictionaryWithPropertyNameParent(this);
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
          DC.Data._DictionaryWithPropertyNameParents.ItemHasChanged(clone, this);
        } else if (DC.Data.IsTransaction) {
          DC.Data.AddTransaction(new TransactionItem(29, TransactionActivityEnum.Update, Key, this, oldItem: clone));
        }
        HasChanged?.Invoke(clone, this);
      }
    }
    partial void onUpdating(string name, ref bool isCancelled);
    partial void onUpdated(DictionaryWithPropertyNameParent old);


    /// <summary>
    /// Updates this DictionaryWithPropertyNameParent with values from CSV file
    /// </summary>
    internal static void Update(DictionaryWithPropertyNameParent dictionaryWithPropertyNameParent, CsvReader csvReader){
      dictionaryWithPropertyNameParent.Name = csvReader.ReadString();
      dictionaryWithPropertyNameParent.onCsvUpdate();
    }
    partial void onCsvUpdate();


    /// <summary>
    /// Add dictionaryWithPropertyNameChild to Children.
    /// </summary>
    internal void AddToChildren(DictionaryWithPropertyNameChild dictionaryWithPropertyNameChild) {
#if DEBUG
      if (dictionaryWithPropertyNameChild==DictionaryWithPropertyNameChild.NoDictionaryWithPropertyNameChild) throw new Exception();
      if ((dictionaryWithPropertyNameChild.Key>=0)&&(Key<0)) throw new Exception();
      if (children.ContainsKey(dictionaryWithPropertyNameChild.Date1)) throw new Exception();
#endif
      children.Add(dictionaryWithPropertyNameChild.Date1, dictionaryWithPropertyNameChild);
      onAddedToChildren(dictionaryWithPropertyNameChild);
    }
    partial void onAddedToChildren(DictionaryWithPropertyNameChild dictionaryWithPropertyNameChild);


    /// <summary>
    /// Removes dictionaryWithPropertyNameChild from DictionaryWithPropertyNameParent.
    /// </summary>
    internal void RemoveFromChildren(DictionaryWithPropertyNameChild dictionaryWithPropertyNameChild) {
#if DEBUG
      if (!children.Remove(dictionaryWithPropertyNameChild.Date1)) throw new Exception();
#else
        children.Remove(dictionaryWithPropertyNameChild.Date1);
#endif
      onRemovedFromChildren(dictionaryWithPropertyNameChild);
    }
    partial void onRemovedFromChildren(DictionaryWithPropertyNameChild dictionaryWithPropertyNameChild);


    /// <summary>
    /// Removes DictionaryWithPropertyNameParent from DC.Data.DictionaryWithPropertyNameParents.
    /// </summary>
    public void Release() {
      if (Key<0) {
        throw new Exception($"DictionaryWithPropertyNameParent.Release(): DictionaryWithPropertyNameParent '{this}' is not stored in DC.Data, key is {Key}.");
      }
      foreach (var dictionaryWithPropertyNameChild in Children.Values) {
        if (dictionaryWithPropertyNameChild?.Key>=0) {
          throw new Exception($"Cannot release DictionaryWithPropertyNameParent '{this}' " + Environment.NewLine + 
            $"because '{dictionaryWithPropertyNameChild}' in DictionaryWithPropertyNameParent.Children is still stored.");
        }
      }
      onReleasing();
      DC.Data._DictionaryWithPropertyNameParents.Remove(Key);
      onReleased();
    }
    partial void onReleasing();
    partial void onReleased();


    /// <summary>
    /// Undoes the new() statement as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemNew(IStorageItem item) {
      var dictionaryWithPropertyNameParent = (DictionaryWithPropertyNameParent) item;
      dictionaryWithPropertyNameParent.onRollbackItemNew();
    }
    partial void onRollbackItemNew();


    /// <summary>
    /// Releases DictionaryWithPropertyNameParent from DC.Data.DictionaryWithPropertyNameParents as part of a transaction rollback of Store().
    /// </summary>
    internal static void RollbackItemStore(IStorageItem item) {
      var dictionaryWithPropertyNameParent = (DictionaryWithPropertyNameParent) item;
      dictionaryWithPropertyNameParent.onRollbackItemStored();
    }
    partial void onRollbackItemStored();


    /// <summary>
    /// Restores the DictionaryWithPropertyNameParent item data as it was before the last update as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemUpdate(IStorageItem oldStorageItem, IStorageItem newStorageItem) {
      var oldItem = (DictionaryWithPropertyNameParent) oldStorageItem;//an item clone with the values before item was updated
      var item = (DictionaryWithPropertyNameParent) newStorageItem;//is the instance whose values should be restored

      // updated item: restore old values
      item.Name = oldItem.Name;
      item.onRollbackItemUpdated(oldItem);
    }
    partial void onRollbackItemUpdated(DictionaryWithPropertyNameParent oldDictionaryWithPropertyNameParent);


    /// <summary>
    /// Adds DictionaryWithPropertyNameParent to DC.Data.DictionaryWithPropertyNameParents as part of a transaction rollback of Release().
    /// </summary>
    internal static void RollbackItemRelease(IStorageItem item) {
      var dictionaryWithPropertyNameParent = (DictionaryWithPropertyNameParent) item;
      dictionaryWithPropertyNameParent.onRollbackItemRelease();
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
