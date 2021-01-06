//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by StorageClassGenerator
//
//     Do not change code in this file, it will get lost when the file gets 
//     auto generated again. Write your code into List_C_MC_Parent.cs.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using StorageLib;


namespace DataModelSamples  {


  public partial class List_C_MC_Parent: IStorageItemGeneric<List_C_MC_Parent> {

    #region Properties
    //      ----------

    /// <summary>
    /// Unique identifier for List_C_MC_Parent. Gets set once List_C_MC_Parent gets added to DC.Data.
    /// </summary>
    public int Key { get; private set; }
    internal static void SetKey(IStorageItem list_C_MC_Parent, int key, bool _) {
      ((List_C_MC_Parent)list_C_MC_Parent).Key = key;
    }


    public string Name { get; private set; }


    public IReadOnlyList<List_C_MC_Child> Children => children;
    readonly List<List_C_MC_Child> children;


    /// <summary>
    /// Headers written to first line in CSV file
    /// </summary>
    internal static readonly string[] Headers = {"Key", "Name"};


    /// <summary>
    /// None existing List_C_MC_Parent
    /// </summary>
    internal static List_C_MC_Parent NoList_C_MC_Parent = new List_C_MC_Parent("NoName", isStoring: false);
    #endregion


    #region Events
    //      ------

    /// <summary>
    /// Content of List_C_MC_Parent has changed. Gets only raised for changes occurring after loading DC.Data with previously stored data.
    /// </summary>
    public event Action</*old*/List_C_MC_Parent, /*new*/List_C_MC_Parent>? HasChanged;
    #endregion


    #region Constructors
    //      ------------

    /// <summary>
    /// List_C_MC_Parent Constructor. If isStoring is true, adds List_C_MC_Parent to DC.Data.List_C_MC_Parents.
    /// </summary>
    public List_C_MC_Parent(string name, bool isStoring = true) {
      Key = StorageExtensions.NoKey;
      Name = name;
      children = new List<List_C_MC_Child>();
      onConstruct();
      if (DC.Data.IsTransaction) {
        DC.Data.AddTransaction(new TransactionItem(21,TransactionActivityEnum.New, Key, this));
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
    public List_C_MC_Parent(List_C_MC_Parent original) {
    #pragma warning restore CS8618 //
      Key = StorageExtensions.NoKey;
      Name = original.Name;
      onCloned(this);
    }
    partial void onCloned(List_C_MC_Parent clone);


    /// <summary>
    /// Constructor for List_C_MC_Parent read from CSV file
    /// </summary>
    private List_C_MC_Parent(int key, CsvReader csvReader){
      Key = key;
      Name = csvReader.ReadString();
      children = new List<List_C_MC_Child>();
      onCsvConstruct();
    }
    partial void onCsvConstruct();


    /// <summary>
    /// New List_C_MC_Parent read from CSV file
    /// </summary>
    internal static List_C_MC_Parent Create(int key, CsvReader csvReader) {
      return new List_C_MC_Parent(key, csvReader);
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Adds List_C_MC_Parent to DC.Data.List_C_MC_Parents.<br/>
    /// Throws an Exception when List_C_MC_Parent is already stored.
    /// </summary>
    public void Store() {
      if (Key>=0) {
        throw new Exception($"List_C_MC_Parent cannot be stored again in DC.Data, key {Key} is greater equal 0." + Environment.NewLine + ToString());
      }

      var isCancelled = false;
      onStoring(ref isCancelled);
      if (isCancelled) return;

      DC.Data._List_C_MC_Parents.Add(this);
      onStored();
    }
    partial void onStoring(ref bool isCancelled);
    partial void onStored();


    /// <summary>
    /// Estimated number of UTF8 characters needed to write List_C_MC_Parent to CSV file
    /// </summary>
    public const int EstimatedLineLength = 150;


    /// <summary>
    /// Write List_C_MC_Parent to CSV file
    /// </summary>
    internal static void Write(List_C_MC_Parent list_C_MC_Parent, CsvWriter csvWriter) {
      list_C_MC_Parent.onCsvWrite();
      csvWriter.Write(list_C_MC_Parent.Name);
    }
    partial void onCsvWrite();


    /// <summary>
    /// Updates List_C_MC_Parent with the provided values
    /// </summary>
    public void Update(string name) {
      var clone = new List_C_MC_Parent(this);
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
          DC.Data._List_C_MC_Parents.ItemHasChanged(clone, this);
        } else if (DC.Data.IsTransaction) {
          DC.Data.AddTransaction(new TransactionItem(21, TransactionActivityEnum.Update, Key, this, oldItem: clone));
        }
        HasChanged?.Invoke(clone, this);
      }
    }
    partial void onUpdating(string name, ref bool isCancelled);
    partial void onUpdated(List_C_MC_Parent old);


    /// <summary>
    /// Updates this List_C_MC_Parent with values from CSV file
    /// </summary>
    internal static void Update(List_C_MC_Parent list_C_MC_Parent, CsvReader csvReader){
      list_C_MC_Parent.Name = csvReader.ReadString();
      list_C_MC_Parent.onCsvUpdate();
    }
    partial void onCsvUpdate();


    /// <summary>
    /// Add list_C_MC_Child to Children.
    /// </summary>
    internal void AddToChildren(List_C_MC_Child list_C_MC_Child) {
#if DEBUG
      if (list_C_MC_Child==List_C_MC_Child.NoList_C_MC_Child) throw new Exception();
      if ((list_C_MC_Child.Key>=0)&&(Key<0)) throw new Exception();
      if (children.Contains(list_C_MC_Child)) throw new Exception();
#endif
      children.Add(list_C_MC_Child);
      onAddedToChildren(list_C_MC_Child);
    }
    partial void onAddedToChildren(List_C_MC_Child list_C_MC_Child);


    /// <summary>
    /// Removes list_C_MC_Child from List_C_MC_Parent.
    /// </summary>
    internal void RemoveFromChildren(List_C_MC_Child list_C_MC_Child) {
#if DEBUG
      if (!children.Remove(list_C_MC_Child)) throw new Exception();
#else
        children.Remove(list_C_MC_Child);
#endif
      onRemovedFromChildren(list_C_MC_Child);
    }
    partial void onRemovedFromChildren(List_C_MC_Child list_C_MC_Child);


    /// <summary>
    /// Removes List_C_MC_Parent from DC.Data.List_C_MC_Parents.
    /// </summary>
    public void Release() {
      if (Key<0) {
        throw new Exception($"List_C_MC_Parent.Release(): List_C_MC_Parent '{this}' is not stored in DC.Data, key is {Key}.");
      }
      foreach (var list_C_MC_Child in Children) {
        if (list_C_MC_Child?.Key>=0) {
          throw new Exception($"Cannot release List_C_MC_Parent '{this}' " + Environment.NewLine + 
            $"because '{list_C_MC_Child}' in List_C_MC_Parent.Children is still stored.");
        }
      }
      onReleased();
      DC.Data._List_C_MC_Parents.Remove(Key);
    }
    partial void onReleased();


    /// <summary>
    /// Undoes the new() statement as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemNew(IStorageItem item) {
      var list_C_MC_Parent = (List_C_MC_Parent) item;
      list_C_MC_Parent.onRollbackItemNew();
    }
    partial void onRollbackItemNew();


    /// <summary>
    /// Releases List_C_MC_Parent from DC.Data.List_C_MC_Parents as part of a transaction rollback of Store().
    /// </summary>
    internal static void RollbackItemStore(IStorageItem item) {
      var list_C_MC_Parent = (List_C_MC_Parent) item;
      list_C_MC_Parent.onRollbackItemStored();
    }
    partial void onRollbackItemStored();


    /// <summary>
    /// Restores the List_C_MC_Parent item data as it was before the last update as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemUpdate(IStorageItem oldStorageItem, IStorageItem newStorageItem) {
      var oldItem = (List_C_MC_Parent) oldStorageItem;//an item clone with the values before item was updated
      var item = (List_C_MC_Parent) newStorageItem;//is the instance whose values should be restored

      // updated item: restore old values
      item.Name = oldItem.Name;
      item.onRollbackItemUpdated(oldItem);
    }
    partial void onRollbackItemUpdated(List_C_MC_Parent oldList_C_MC_Parent);


    /// <summary>
    /// Adds List_C_MC_Parent to DC.Data.List_C_MC_Parents as part of a transaction rollback of Release().
    /// </summary>
    internal static void RollbackItemRelease(IStorageItem item) {
      var list_C_MC_Parent = (List_C_MC_Parent) item;
      list_C_MC_Parent.onRollbackItemRelease();
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
