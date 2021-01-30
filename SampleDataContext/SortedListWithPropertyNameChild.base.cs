//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by StorageClassGenerator
//
//     Do not change code in this file, it will get lost when the file gets 
//     auto generated again. Write your code into SortedListWithPropertyNameChild.cs.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using StorageLib;


namespace DataModelSamples  {


  public partial class SortedListWithPropertyNameChild: IStorageItem<SortedListWithPropertyNameChild> {

    #region Properties
    //      ----------

    /// <summary>
    /// Unique identifier for SortedListWithPropertyNameChild. Gets set once SortedListWithPropertyNameChild gets added to DC.Data.
    /// </summary>
    public int Key { get; private set; }
    internal static void SetKey(IStorageItem sortedListWithPropertyNameChild, int key, bool _) {
      ((SortedListWithPropertyNameChild)sortedListWithPropertyNameChild).Key = key;
    }


    public string Name { get; private set; }


    public string Address { get; private set; }


    public SortedListWithPropertyNameParent Parent { get; private set; }


    /// <summary>
    /// Headers written to first line in CSV file
    /// </summary>
    internal static readonly string[] Headers = {"Key", "Name", "Address", "Parent"};


    /// <summary>
    /// None existing SortedListWithPropertyNameChild, used as a temporary place holder when reading a CSV file
    /// which was not compacted. It might create first a later deleted item linking to a 
    /// deleted parent. In this case, the parent property gets set to NoSortedListWithPropertyNameChild. Once the CSV
    /// file is completely read, that child will actually be deleted (released) and Verify()
    /// ensures that there are no stored children with links to NoSortedListWithPropertyNameChild.
    /// </summary>
    internal static SortedListWithPropertyNameChild NoSortedListWithPropertyNameChild = new SortedListWithPropertyNameChild("NoName", "NoAddress", SortedListWithPropertyNameParent.NoSortedListWithPropertyNameParent, isStoring: false);
    #endregion


    #region Events
    //      ------

    /// <summary>
    /// Content of SortedListWithPropertyNameChild has changed. Gets only raised for changes occurring after loading DC.Data with previously stored data.
    /// </summary>
    public event Action</*old*/SortedListWithPropertyNameChild, /*new*/SortedListWithPropertyNameChild>? HasChanged;
    #endregion


    #region Constructors
    //      ------------

    /// <summary>
    /// SortedListWithPropertyNameChild Constructor. If isStoring is true, adds SortedListWithPropertyNameChild to DC.Data.SortedListWithPropertyNameChilds.
    /// </summary>
    public SortedListWithPropertyNameChild(string name, string address, SortedListWithPropertyNameParent parent, bool isStoring = true) {
      Key = StorageExtensions.NoKey;
      Name = name;
      Address = address;
      Parent = parent;
      Parent.AddToChildren(this);
      onConstruct();
      if (DC.Data.IsTransaction) {
        DC.Data.AddTransaction(new TransactionItem(36,TransactionActivityEnum.New, Key, this));
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
    public SortedListWithPropertyNameChild(SortedListWithPropertyNameChild original) {
    #pragma warning restore CS8618 //
      Key = StorageExtensions.NoKey;
      Name = original.Name;
      Address = original.Address;
      Parent = original.Parent;
      onCloned(this);
    }
    partial void onCloned(SortedListWithPropertyNameChild clone);


    /// <summary>
    /// Constructor for SortedListWithPropertyNameChild read from CSV file
    /// </summary>
    private SortedListWithPropertyNameChild(int key, CsvReader csvReader){
      Key = key;
      Name = csvReader.ReadString();
      Address = csvReader.ReadString();
      var sortedListWithPropertyNameParentKey = csvReader.ReadInt();
      Parent = DC.Data._SortedListWithPropertyNameParents.GetItem(sortedListWithPropertyNameParentKey)?? SortedListWithPropertyNameParent.NoSortedListWithPropertyNameParent;
      if (Parent!=SortedListWithPropertyNameParent.NoSortedListWithPropertyNameParent) {
        Parent.AddToChildren(this);
      }
      onCsvConstruct();
    }
    partial void onCsvConstruct();


    /// <summary>
    /// New SortedListWithPropertyNameChild read from CSV file
    /// </summary>
    internal static SortedListWithPropertyNameChild Create(int key, CsvReader csvReader) {
      return new SortedListWithPropertyNameChild(key, csvReader);
    }


    /// <summary>
    /// Verify that sortedListWithPropertyNameChild.Parent exists.
    /// </summary>
    internal static bool Verify(SortedListWithPropertyNameChild sortedListWithPropertyNameChild) {
      if (sortedListWithPropertyNameChild.Parent==SortedListWithPropertyNameParent.NoSortedListWithPropertyNameParent) return false;
      return true;
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Adds SortedListWithPropertyNameChild to DC.Data.SortedListWithPropertyNameChilds.<br/>
    /// Throws an Exception when SortedListWithPropertyNameChild is already stored.
    /// </summary>
    public void Store() {
      if (Key>=0) {
        throw new Exception($"SortedListWithPropertyNameChild cannot be stored again in DC.Data, key {Key} is greater equal 0." + Environment.NewLine + ToString());
      }

      var isCancelled = false;
      onStoring(ref isCancelled);
      if (isCancelled) return;

      if (Parent.Key<0) {
        throw new Exception($"Cannot store child SortedListWithPropertyNameChild '{this}'.Parent to SortedListWithPropertyNameParent '{Parent}' because parent is not stored yet.");
      }
      DC.Data._SortedListWithPropertyNameChilds.Add(this);
      onStored();
    }
    partial void onStoring(ref bool isCancelled);
    partial void onStored();


    /// <summary>
    /// Estimated number of UTF8 characters needed to write SortedListWithPropertyNameChild to CSV file
    /// </summary>
    public const int EstimatedLineLength = 300;


    /// <summary>
    /// Write SortedListWithPropertyNameChild to CSV file
    /// </summary>
    internal static void Write(SortedListWithPropertyNameChild sortedListWithPropertyNameChild, CsvWriter csvWriter) {
      sortedListWithPropertyNameChild.onCsvWrite();
      csvWriter.Write(sortedListWithPropertyNameChild.Name);
      csvWriter.Write(sortedListWithPropertyNameChild.Address);
      if (sortedListWithPropertyNameChild.Parent.Key<0) throw new Exception($"Cannot write sortedListWithPropertyNameChild '{sortedListWithPropertyNameChild}' to CSV File, because Parent is not stored in DC.Data.SortedListWithPropertyNameParents.");

      csvWriter.Write(sortedListWithPropertyNameChild.Parent.Key.ToString());
    }
    partial void onCsvWrite();


    /// <summary>
    /// Updates SortedListWithPropertyNameChild with the provided values
    /// </summary>
    public void Update(string name, string address, SortedListWithPropertyNameParent parent) {
      if (Key>=0){
        if (parent.Key<0) {
          throw new Exception($"SortedListWithPropertyNameChild.Update(): It is illegal to add stored SortedListWithPropertyNameChild '{this}'" + Environment.NewLine + 
            $"to Parent '{parent}', which is not stored.");
        }
      }
      var clone = new SortedListWithPropertyNameChild(this);
      var isCancelled = false;
      onUpdating(name, address, parent, ref isCancelled);
      if (isCancelled) return;


      //remove not yet updated item from parents which will be removed by update
      var hasParentChanged = Parent!=parent || Name!=name;
      if (hasParentChanged) {
        Parent.RemoveFromChildren(this);
      }

      //update properties and detect if any value has changed
      var isChangeDetected = false;
      if (Name!=name) {
        Name = name;
        isChangeDetected = true;
      }
      if (Address!=address) {
        Address = address;
        isChangeDetected = true;
      }
      if (Parent!=parent) {
        Parent = parent;
        isChangeDetected = true;
      }

      //add updated item to parents which have been newly added during update
      if (hasParentChanged) {
        Parent.AddToChildren(this);
      }
      if (isChangeDetected) {
        onUpdated(clone);
        if (Key>=0) {
          DC.Data._SortedListWithPropertyNameChilds.ItemHasChanged(clone, this);
        } else if (DC.Data.IsTransaction) {
          DC.Data.AddTransaction(new TransactionItem(36, TransactionActivityEnum.Update, Key, this, oldItem: clone));
        }
        HasChanged?.Invoke(clone, this);
      }
    }
    partial void onUpdating(string name, string address, SortedListWithPropertyNameParent parent, ref bool isCancelled);
    partial void onUpdated(SortedListWithPropertyNameChild old);


    /// <summary>
    /// Updates this SortedListWithPropertyNameChild with values from CSV file
    /// </summary>
    internal static void Update(SortedListWithPropertyNameChild sortedListWithPropertyNameChild, CsvReader csvReader){
      sortedListWithPropertyNameChild.Name = csvReader.ReadString();
      sortedListWithPropertyNameChild.Address = csvReader.ReadString();
        var parent = DC.Data._SortedListWithPropertyNameParents.GetItem(csvReader.ReadInt())??
          SortedListWithPropertyNameParent.NoSortedListWithPropertyNameParent;
      if (sortedListWithPropertyNameChild.Parent!=parent) {
        if (sortedListWithPropertyNameChild.Parent!=SortedListWithPropertyNameParent.NoSortedListWithPropertyNameParent) {
          sortedListWithPropertyNameChild.Parent.RemoveFromChildren(sortedListWithPropertyNameChild);
        }
        sortedListWithPropertyNameChild.Parent = parent;
        sortedListWithPropertyNameChild.Parent.AddToChildren(sortedListWithPropertyNameChild);
      }
      sortedListWithPropertyNameChild.onCsvUpdate();
    }
    partial void onCsvUpdate();


    /// <summary>
    /// Removes SortedListWithPropertyNameChild from DC.Data.SortedListWithPropertyNameChilds.
    /// </summary>
    public void Release() {
      if (Key<0) {
        throw new Exception($"SortedListWithPropertyNameChild.Release(): SortedListWithPropertyNameChild '{this}' is not stored in DC.Data, key is {Key}.");
      }
      DC.Data._SortedListWithPropertyNameChilds.Remove(Key);
      onReleased();
    }
    partial void onReleased();


    /// <summary>
    /// Removes SortedListWithPropertyNameChild from parents as part of a transaction rollback of the new() statement.
    /// </summary>
    internal static void RollbackItemNew(IStorageItem item) {
      var sortedListWithPropertyNameChild = (SortedListWithPropertyNameChild) item;
      if (sortedListWithPropertyNameChild.Parent!=SortedListWithPropertyNameParent.NoSortedListWithPropertyNameParent) {
        sortedListWithPropertyNameChild.Parent.RemoveFromChildren(sortedListWithPropertyNameChild);
      }
      sortedListWithPropertyNameChild.onRollbackItemNew();
    }
    partial void onRollbackItemNew();


    /// <summary>
    /// Releases SortedListWithPropertyNameChild from DC.Data.SortedListWithPropertyNameChilds as part of a transaction rollback of Store().
    /// </summary>
    internal static void RollbackItemStore(IStorageItem item) {
      var sortedListWithPropertyNameChild = (SortedListWithPropertyNameChild) item;
      sortedListWithPropertyNameChild.onRollbackItemStored();
    }
    partial void onRollbackItemStored();


    /// <summary>
    /// Restores the SortedListWithPropertyNameChild item data as it was before the last update as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemUpdate(IStorageItem oldStorageItem, IStorageItem newStorageItem) {
      var oldItem = (SortedListWithPropertyNameChild) oldStorageItem;//an item clone with the values before item was updated
      var item = (SortedListWithPropertyNameChild) newStorageItem;//is the instance whose values should be restored

      // remove updated item from parents
      var hasParentChanged = oldItem.Parent!=item.Parent || oldItem.Name!=item.Name;
      if (hasParentChanged) {
        item.Parent.RemoveFromChildren(item);
      }

      // updated item: restore old values
      item.Name = oldItem.Name;
      item.Address = oldItem.Address;
      item.Parent = oldItem.Parent;

      // add item with previous values to parents
      if (hasParentChanged) {
        item.Parent.AddToChildren(item);
      }
      item.onRollbackItemUpdated(oldItem);
    }
    partial void onRollbackItemUpdated(SortedListWithPropertyNameChild oldSortedListWithPropertyNameChild);


    /// <summary>
    /// Adds SortedListWithPropertyNameChild to DC.Data.SortedListWithPropertyNameChilds as part of a transaction rollback of Release().
    /// </summary>
    internal static void RollbackItemRelease(IStorageItem item) {
      var sortedListWithPropertyNameChild = (SortedListWithPropertyNameChild) item;
      sortedListWithPropertyNameChild.onRollbackItemRelease();
    }
    partial void onRollbackItemRelease();


    /// <summary>
    /// Returns property values for tracing. Parents are shown with their key instead their content.
    /// </summary>
    public string ToTraceString() {
      var returnString =
        $"{this.GetKeyOrHash()}|" +
        $" {Name}|" +
        $" {Address}|" +
        $" Parent {Parent.GetKeyOrHash()}";
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
        $" {Name}," +
        $" {Address}," +
        $" {Parent.ToShortString()}";
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
        $" Address: {Address}," +
        $" Parent: {Parent.ToShortString()};";
      onToString(ref returnString);
      return returnString;
    }
    partial void onToString(ref string returnString);
    #endregion
  }
}
