//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by StorageClassGenerator
//
//     Do not change code in this file, it will get lost when the file gets 
//     auto generated again. Write your code into Parent.cs.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using StorageLib;


namespace YourNamespace  {


    /// <summary>
    /// Some comment for Parent.
    /// </summary>
  public partial class Parent: IStorageItem<Parent> {

    #region Properties
    //      ----------

    /// <summary>
    /// Unique identifier for Parent. Gets set once Parent gets added to DC.Data.
    /// </summary>
    public int Key { get; private set; }
    internal static void SetKey(IStorageItem parent, int key, bool _) {
      ((Parent)parent).Key = key;
    }


    /// <summary>
    /// Some Name comment
    /// </summary>
    public string Name { get; private set; }


    /// <summary>
    /// Any child created will automatically get added here.
    /// </summary>
    public IStorageReadOnlyList<Child> Children => children;
    readonly StorageList<Child> children;


    /// <summary>
    /// Headers written to first line in CSV file
    /// </summary>
    internal static readonly string[] Headers = {"Key", "Name"};


    /// <summary>
    /// None existing Parent, used as a temporary place holder when reading a CSV file
    /// which was not compacted. It might create first a later deleted item linking to a 
    /// deleted parent. In this case, the parent property gets set to NoParent. Once the CSV
    /// file is completely read, that child will actually be deleted (released) and Verify()
    /// ensures that there are no stored children with links to NoParent.
    /// </summary>
    internal static Parent NoParent = new Parent("NoName", isStoring: false);
    #endregion


    #region Events
    //      ------

    /// <summary>
    /// Content of Parent has changed. Gets only raised for changes occurring after loading DC.Data with previously stored data.
    /// </summary>
    public event Action</*old*/Parent, /*new*/Parent>? HasChanged;
    #endregion


    #region Constructors
    //      ------------

    /// <summary>
    /// Parent Constructor. If isStoring is true, adds Parent to DC.Data.Parents.
    /// </summary>
    public Parent(string name, bool isStoring = true) {
      Key = StorageExtensions.NoKey;
      Name = name;
      children = new StorageList<Child>();
      onConstruct();
      if (DC.Data.IsTransaction) {
        DC.Data.AddTransaction(new TransactionItem(0,TransactionActivityEnum.New, Key, this));
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
    public Parent(Parent original) {
    #pragma warning restore CS8618 //
      Key = StorageExtensions.NoKey;
      Name = original.Name;
      onCloned(this);
    }
    partial void onCloned(Parent clone);


    /// <summary>
    /// Constructor for Parent read from CSV file
    /// </summary>
    private Parent(int key, CsvReader csvReader){
      Key = key;
      Name = csvReader.ReadString();
      children = new StorageList<Child>();
      onCsvConstruct();
    }
    partial void onCsvConstruct();


    /// <summary>
    /// New Parent read from CSV file
    /// </summary>
    internal static Parent Create(int key, CsvReader csvReader) {
      return new Parent(key, csvReader);
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Adds Parent to DC.Data.Parents.<br/>
    /// Throws an Exception when Parent is already stored.
    /// </summary>
    public void Store() {
      if (Key>=0) {
        throw new Exception($"Parent cannot be stored again in DC.Data, key {Key} is greater equal 0." + Environment.NewLine + ToString());
      }

      var isCancelled = false;
      onStoring(ref isCancelled);
      if (isCancelled) return;

      DC.Data._Parents.Add(this);
      onStored();
    }
    partial void onStoring(ref bool isCancelled);
    partial void onStored();


    /// <summary>
    /// Estimated number of UTF8 characters needed to write Parent to CSV file
    /// </summary>
    public const int EstimatedLineLength = 150;


    /// <summary>
    /// Write Parent to CSV file
    /// </summary>
    internal static void Write(Parent parent, CsvWriter csvWriter) {
      parent.onCsvWrite();
      csvWriter.Write(parent.Name);
    }
    partial void onCsvWrite();


    /// <summary>
    /// Updates Parent with the provided values
    /// </summary>
    public void Update(string name) {
      var clone = new Parent(this);
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
          DC.Data._Parents.ItemHasChanged(clone, this);
        } else if (DC.Data.IsTransaction) {
          DC.Data.AddTransaction(new TransactionItem(0, TransactionActivityEnum.Update, Key, this, oldItem: clone));
        }
        HasChanged?.Invoke(clone, this);
      }
    }
    partial void onUpdating(string name, ref bool isCancelled);
    partial void onUpdated(Parent old);


    /// <summary>
    /// Updates this Parent with values from CSV file
    /// </summary>
    internal static void Update(Parent parent, CsvReader csvReader){
      parent.Name = csvReader.ReadString();
      parent.onCsvUpdate();
    }
    partial void onCsvUpdate();


    /// <summary>
    /// Add child to Children.
    /// </summary>
    internal void AddToChildren(Child child) {
#if DEBUG
      if (child==Child.NoChild) throw new Exception();
      if ((child.Key>=0)&&(Key<0)) throw new Exception();
      if (children.Contains(child)) throw new Exception();
#endif
      children.Add(child);
      onAddedToChildren(child);
    }
    partial void onAddedToChildren(Child child);


    /// <summary>
    /// Removes child from Parent.
    /// </summary>
    internal void RemoveFromChildren(Child child) {
#if DEBUG
      if (!children.Remove(child)) throw new Exception();
#else
        children.Remove(child);
#endif
      onRemovedFromChildren(child);
    }
    partial void onRemovedFromChildren(Child child);


    /// <summary>
    /// Removes Parent from DC.Data.Parents.
    /// </summary>
    public void Release() {
      if (Key<0) {
        throw new Exception($"Parent.Release(): Parent '{this}' is not stored in DC.Data, key is {Key}.");
      }
      foreach (var child in Children) {
        if (child?.Key>=0) {
          throw new Exception($"Cannot release Parent '{this}' " + Environment.NewLine + 
            $"because '{child}' in Parent.Children is still stored.");
        }
      }
      DC.Data._Parents.Remove(Key);
      onReleased();
    }
    partial void onReleased();


    /// <summary>
    /// Undoes the new() statement as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemNew(IStorageItem item) {
      var parent = (Parent) item;
      parent.onRollbackItemNew();
    }
    partial void onRollbackItemNew();


    /// <summary>
    /// Releases Parent from DC.Data.Parents as part of a transaction rollback of Store().
    /// </summary>
    internal static void RollbackItemStore(IStorageItem item) {
      var parent = (Parent) item;
      parent.onRollbackItemStored();
    }
    partial void onRollbackItemStored();


    /// <summary>
    /// Restores the Parent item data as it was before the last update as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemUpdate(IStorageItem oldStorageItem, IStorageItem newStorageItem) {
      var oldItem = (Parent) oldStorageItem;//an item clone with the values before item was updated
      var item = (Parent) newStorageItem;//is the instance whose values should be restored

      // updated item: restore old values
      item.Name = oldItem.Name;
      item.onRollbackItemUpdated(oldItem);
    }
    partial void onRollbackItemUpdated(Parent oldParent);


    /// <summary>
    /// Adds Parent to DC.Data.Parents as part of a transaction rollback of Release().
    /// </summary>
    internal static void RollbackItemRelease(IStorageItem item) {
      var parent = (Parent) item;
      parent.onRollbackItemRelease();
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
