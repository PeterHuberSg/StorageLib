//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by StorageClassGenerator
//
//     Do not change code in this file, it will get lost when the file gets 
//     auto generated again. Write your code into Child.cs.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using StorageLib;


namespace YourNamespace  {


  public partial class Child: IStorageItemGeneric<Child> {

    #region Properties
    //      ----------

    /// <summary>
    /// Unique identifier for Child. Gets set once Child gets added to DC.Data.
    /// </summary>
    public int Key { get; private set; }
    internal static void SetKey(IStorageItem child, int key, bool _) {
      ((Child)child).Key = key;
    }


    public string Name { get; private set; }


    /// <summary>
    /// The child will get added to its parent Children collection.
    /// </summary>
    public Parent Parent { get; private set; }


    /// <summary>
    /// Headers written to first line in CSV file
    /// </summary>
    internal static readonly string[] Headers = {"Key", "Name", "Parent"};


    /// <summary>
    /// None existing Child, used as a temporary place holder when reading a CSV file
    /// which was not compacted. It might create first a later deleted item linking to a 
    /// deleted parent. In this case, the parent property gets set to NoChild. Once the CSV
    /// file is completely read, that child will actually be deleted (released) and Verify()
    /// ensures that there are no stored children with links to NoChild.
    /// </summary>
    internal static Child NoChild = new Child("NoName", Parent.NoParent, isStoring: false);
    #endregion


    #region Events
    //      ------

    /// <summary>
    /// Content of Child has changed. Gets only raised for changes occurring after loading DC.Data with previously stored data.
    /// </summary>
    public event Action</*old*/Child, /*new*/Child>? HasChanged;
    #endregion


    #region Constructors
    //      ------------

    /// <summary>
    /// Child Constructor. If isStoring is true, adds Child to DC.Data.Children.
    /// </summary>
    public Child(string name, Parent parent, bool isStoring = true) {
      Key = StorageExtensions.NoKey;
      Name = name;
      Parent = parent;
      Parent.AddToChildren(this);
      onConstruct();
      if (DC.Data.IsTransaction) {
        DC.Data.AddTransaction(new TransactionItem(1,TransactionActivityEnum.New, Key, this));
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
    public Child(Child original) {
    #pragma warning restore CS8618 //
      Key = StorageExtensions.NoKey;
      Name = original.Name;
      Parent = original.Parent;
      onCloned(this);
    }
    partial void onCloned(Child clone);


    /// <summary>
    /// Constructor for Child read from CSV file
    /// </summary>
    private Child(int key, CsvReader csvReader){
      Key = key;
      Name = csvReader.ReadString();
      var parentKey = csvReader.ReadInt();
      Parent = DC.Data._Parents.GetItem(parentKey)?? Parent.NoParent;
      if (Parent!=Parent.NoParent) {
        Parent.AddToChildren(this);
      }
      onCsvConstruct();
    }
    partial void onCsvConstruct();


    /// <summary>
    /// New Child read from CSV file
    /// </summary>
    internal static Child Create(int key, CsvReader csvReader) {
      return new Child(key, csvReader);
    }


    /// <summary>
    /// Verify that child.Parent exists.
    /// </summary>
    internal static bool Verify(Child child) {
      if (child.Parent==Parent.NoParent) return false;
      return true;
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Adds Child to DC.Data.Children.<br/>
    /// Throws an Exception when Child is already stored.
    /// </summary>
    public void Store() {
      if (Key>=0) {
        throw new Exception($"Child cannot be stored again in DC.Data, key {Key} is greater equal 0." + Environment.NewLine + ToString());
      }

      var isCancelled = false;
      onStoring(ref isCancelled);
      if (isCancelled) return;

      if (Parent.Key<0) {
        throw new Exception($"Cannot store child Child '{this}'.Parent to Parent '{Parent}' because parent is not stored yet.");
      }
      DC.Data._Children.Add(this);
      onStored();
    }
    partial void onStoring(ref bool isCancelled);
    partial void onStored();


    /// <summary>
    /// Estimated number of UTF8 characters needed to write Child to CSV file
    /// </summary>
    public const int EstimatedLineLength = 150;


    /// <summary>
    /// Write Child to CSV file
    /// </summary>
    internal static void Write(Child child, CsvWriter csvWriter) {
      child.onCsvWrite();
      csvWriter.Write(child.Name);
      if (child.Parent.Key<0) throw new Exception($"Cannot write child '{child}' to CSV File, because Parent is not stored in DC.Data.Parents.");

      csvWriter.Write(child.Parent.Key.ToString());
    }
    partial void onCsvWrite();


    /// <summary>
    /// Updates Child with the provided values
    /// </summary>
    public void Update(string name, Parent parent) {
      if (Key>=0){
        if (parent.Key<0) {
          throw new Exception($"Child.Update(): It is illegal to add stored Child '{this}'" + Environment.NewLine + 
            $"to Parent '{parent}', which is not stored.");
        }
      }
      var clone = new Child(this);
      var isCancelled = false;
      onUpdating(name, parent, ref isCancelled);
      if (isCancelled) return;


      //remove not yet updated item from parents which will be removed by update
      var hasParentChanged = Parent!=parent;
      if (hasParentChanged) {
        Parent.RemoveFromChildren(this);
      }

      //update properties and detect if any value has changed
      var isChangeDetected = false;
      if (Name!=name) {
        Name = name;
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
          DC.Data._Children.ItemHasChanged(clone, this);
        } else if (DC.Data.IsTransaction) {
          DC.Data.AddTransaction(new TransactionItem(1, TransactionActivityEnum.Update, Key, this, oldItem: clone));
        }
        HasChanged?.Invoke(clone, this);
      }
    }
    partial void onUpdating(string name, Parent parent, ref bool isCancelled);
    partial void onUpdated(Child old);


    /// <summary>
    /// Updates this Child with values from CSV file
    /// </summary>
    internal static void Update(Child child, CsvReader csvReader){
      child.Name = csvReader.ReadString();
        var parent = DC.Data._Parents.GetItem(csvReader.ReadInt())??
          Parent.NoParent;
      if (child.Parent!=parent) {
        if (child.Parent!=Parent.NoParent) {
          child.Parent.RemoveFromChildren(child);
        }
        child.Parent = parent;
        child.Parent.AddToChildren(child);
      }
      child.onCsvUpdate();
    }
    partial void onCsvUpdate();


    /// <summary>
    /// Removes Child from DC.Data.Children.
    /// </summary>
    public void Release() {
      if (Key<0) {
        throw new Exception($"Child.Release(): Child '{this}' is not stored in DC.Data, key is {Key}.");
      }
      DC.Data._Children.Remove(Key);
      onReleased();
    }
    partial void onReleased();


    /// <summary>
    /// Removes Child from parents as part of a transaction rollback of the new() statement.
    /// </summary>
    internal static void RollbackItemNew(IStorageItem item) {
      var child = (Child) item;
      if (child.Parent!=Parent.NoParent) {
        child.Parent.RemoveFromChildren(child);
      }
      child.onRollbackItemNew();
    }
    partial void onRollbackItemNew();


    /// <summary>
    /// Releases Child from DC.Data.Children as part of a transaction rollback of Store().
    /// </summary>
    internal static void RollbackItemStore(IStorageItem item) {
      var child = (Child) item;
      child.onRollbackItemStored();
    }
    partial void onRollbackItemStored();


    /// <summary>
    /// Restores the Child item data as it was before the last update as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemUpdate(IStorageItem oldStorageItem, IStorageItem newStorageItem) {
      var oldItem = (Child) oldStorageItem;//an item clone with the values before item was updated
      var item = (Child) newStorageItem;//is the instance whose values should be restored

      // remove updated item from parents
      var hasParentChanged = oldItem.Parent!=item.Parent;
      if (hasParentChanged) {
        item.Parent.RemoveFromChildren(item);
      }

      // updated item: restore old values
      item.Name = oldItem.Name;
      item.Parent = oldItem.Parent;

      // add item with previous values to parents
      if (hasParentChanged) {
        item.Parent.AddToChildren(item);
      }
      item.onRollbackItemUpdated(oldItem);
    }
    partial void onRollbackItemUpdated(Child oldChild);


    /// <summary>
    /// Adds Child to DC.Data.Children as part of a transaction rollback of Release().
    /// </summary>
    internal static void RollbackItemRelease(IStorageItem item) {
      var child = (Child) item;
      child.onRollbackItemRelease();
    }
    partial void onRollbackItemRelease();


    /// <summary>
    /// Returns property values for tracing. Parents are shown with their key instead their content.
    /// </summary>
    public string ToTraceString() {
      var returnString =
        $"{this.GetKeyOrHash()}|" +
        $" {Name}|" +
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
        $" Parent: {Parent.ToShortString()};";
      onToString(ref returnString);
      return returnString;
    }
    partial void onToString(ref string returnString);
    #endregion
  }
}
