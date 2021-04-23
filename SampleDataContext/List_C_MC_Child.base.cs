//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by StorageClassGenerator
//
//     Do not change code in this file, it will get lost when the file gets 
//     auto generated again. Write your code into List_C_MC_Child.cs.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using StorageLib;


namespace DataModelSamples  {


  public partial class List_C_MC_Child: IStorageItem<List_C_MC_Child> {

    #region Properties
    //      ----------

    /// <summary>
    /// Unique identifier for List_C_MC_Child. Gets set once List_C_MC_Child gets added to DC.Data.
    /// </summary>
    public int Key { get; private set; }
    internal static void SetKey(IStorageItem list_C_MC_Child, int key, bool _) {
      ((List_C_MC_Child)list_C_MC_Child).Key = key;
    }


    /// <summary>
    /// Stores only dates but no times.
    ///  </summary>
    public DateTime Date { get; private set; }


    public List_C_MC_Parent? Parent { get; private set; }


    /// <summary>
    /// Headers written to first line in CSV file
    /// </summary>
    internal static readonly string[] Headers = {"Key", "Date", "Parent"};


    /// <summary>
    /// None existing List_C_MC_Child, used as a temporary place holder when reading a CSV file
    /// which was not compacted. It might create first a later deleted item linking to a 
    /// deleted parent. In this case, the parent property gets set to NoList_C_MC_Child. Once the CSV
    /// file is completely read, that child will actually be deleted (released) and Verify()
    /// ensures that there are no stored children with links to NoList_C_MC_Child.
    /// </summary>
    internal static List_C_MC_Child NoList_C_MC_Child = new List_C_MC_Child(DateTime.MinValue.Date, null, isStoring: false);
    #endregion


    #region Events
    //      ------

    /// <summary>
    /// Content of List_C_MC_Child has changed. Gets only raised for changes occurring after loading DC.Data with previously stored data.
    /// </summary>
    public event Action</*old*/List_C_MC_Child, /*new*/List_C_MC_Child>? HasChanged;
    #endregion


    #region Constructors
    //      ------------

    /// <summary>
    /// List_C_MC_Child Constructor. If isStoring is true, adds List_C_MC_Child to DC.Data.List_C_MC_Childs.
    /// </summary>
    public List_C_MC_Child(DateTime date, List_C_MC_Parent? parent, bool isStoring = true) {
      Key = StorageExtensions.NoKey;
      Date = date.Floor(Rounding.Days);
      Parent = parent;
      if (Parent!=null) {
        Parent.AddToChildren(this);
      }
      onConstruct();
      if (DC.Data.IsTransaction) {
        DC.Data.AddTransaction(new TransactionItem(22,TransactionActivityEnum.New, Key, this));
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
    public List_C_MC_Child(List_C_MC_Child original) {
    #pragma warning restore CS8618 //
      Key = StorageExtensions.NoKey;
      Date = original.Date;
      Parent = original.Parent;
      onCloned(this);
    }
    partial void onCloned(List_C_MC_Child clone);


    /// <summary>
    /// Constructor for List_C_MC_Child read from CSV file
    /// </summary>
    private List_C_MC_Child(int key, CsvReader csvReader){
      Key = key;
      Date = csvReader.ReadDate();
      var parentKey = csvReader.ReadIntNull();
      if (parentKey.HasValue) {
        Parent = DC.Data._List_C_MC_Parents.GetItem(parentKey.Value)?? List_C_MC_Parent.NoList_C_MC_Parent;
      }
      if (parentKey.HasValue && Parent!=List_C_MC_Parent.NoList_C_MC_Parent) {
        Parent!.AddToChildren(this);
      }
      onCsvConstruct();
    }
    partial void onCsvConstruct();


    /// <summary>
    /// New List_C_MC_Child read from CSV file
    /// </summary>
    internal static List_C_MC_Child Create(int key, CsvReader csvReader) {
      return new List_C_MC_Child(key, csvReader);
    }


    /// <summary>
    /// Verify that list_C_MC_Child.Parent exists.
    /// </summary>
    internal static bool Verify(List_C_MC_Child list_C_MC_Child) {
      if (list_C_MC_Child.Parent==List_C_MC_Parent.NoList_C_MC_Parent) return false;
      return true;
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Adds List_C_MC_Child to DC.Data.List_C_MC_Childs.<br/>
    /// Throws an Exception when List_C_MC_Child is already stored.
    /// </summary>
    public void Store() {
      if (Key>=0) {
        throw new Exception($"List_C_MC_Child cannot be stored again in DC.Data, key {Key} is greater equal 0." + Environment.NewLine + ToString());
      }

      var isCancelled = false;
      onStoring(ref isCancelled);
      if (isCancelled) return;

      if (Parent?.Key<0) {
        throw new Exception($"Cannot store child List_C_MC_Child '{this}'.Parent to List_C_MC_Parent '{Parent}' because parent is not stored yet.");
      }
      DC.Data._List_C_MC_Childs.Add(this);
      onStored();
    }
    partial void onStoring(ref bool isCancelled);
    partial void onStored();


    /// <summary>
    /// Estimated number of UTF8 characters needed to write List_C_MC_Child to CSV file
    /// </summary>
    public const int EstimatedLineLength = 16;


    /// <summary>
    /// Write List_C_MC_Child to CSV file
    /// </summary>
    internal static void Write(List_C_MC_Child list_C_MC_Child, CsvWriter csvWriter) {
      list_C_MC_Child.onCsvWrite();
      csvWriter.WriteDate(list_C_MC_Child.Date);
      if (list_C_MC_Child.Parent is null) {
        csvWriter.WriteNull();
      } else {
        if (list_C_MC_Child.Parent.Key<0) throw new Exception($"Cannot write list_C_MC_Child '{list_C_MC_Child}' to CSV File, because Parent is not stored in DC.Data.List_C_MC_Parents.");

        csvWriter.Write(list_C_MC_Child.Parent.Key.ToString());
      }
    }
    partial void onCsvWrite();


    /// <summary>
    /// Updates List_C_MC_Child with the provided values
    /// </summary>
    public void Update(DateTime date, List_C_MC_Parent? parent) {
      if (Key>=0){
        if (parent?.Key<0) {
          throw new Exception($"List_C_MC_Child.Update(): It is illegal to add stored List_C_MC_Child '{this}'" + Environment.NewLine + 
            $"to Parent '{parent}', which is not stored.");
        }
      }
      var clone = new List_C_MC_Child(this);
      var isCancelled = false;
      onUpdating(date, parent, ref isCancelled);
      if (isCancelled) return;


      //remove not yet updated item from parents which will be removed by update
      var hasParentChanged = Parent!=parent;
      if (Parent is not null && hasParentChanged) {
        Parent.RemoveFromChildren(this);
      }

      //update properties and detect if any value has changed
      var isChangeDetected = false;
      var dateRounded = date.Floor(Rounding.Days);
      if (Date!=dateRounded) {
        Date = dateRounded;
        isChangeDetected = true;
      }
      if (Parent!=parent) {
        Parent = parent;
        isChangeDetected = true;
      }

      //add updated item to parents which have been newly added during update
      if (Parent is not null && hasParentChanged) {
        Parent.AddToChildren(this);
      }
      if (isChangeDetected) {
        onUpdated(clone);
        if (Key>=0) {
          DC.Data._List_C_MC_Childs.ItemHasChanged(clone, this);
        } else if (DC.Data.IsTransaction) {
          DC.Data.AddTransaction(new TransactionItem(22, TransactionActivityEnum.Update, Key, this, oldItem: clone));
        }
        HasChanged?.Invoke(clone, this);
      }
    }
    partial void onUpdating(DateTime date, List_C_MC_Parent? parent, ref bool isCancelled);
    partial void onUpdated(List_C_MC_Child old);


    /// <summary>
    /// Updates this List_C_MC_Child with values from CSV file
    /// </summary>
    internal static void Update(List_C_MC_Child list_C_MC_Child, CsvReader csvReader){
      list_C_MC_Child.Date = csvReader.ReadDate();
      var parentKey = csvReader.ReadIntNull();
      List_C_MC_Parent? parent;
      if (parentKey is null) {
        parent = null;
      } else {
        parent = DC.Data._List_C_MC_Parents.GetItem(parentKey.Value)??
          List_C_MC_Parent.NoList_C_MC_Parent;
      }
      if (list_C_MC_Child.Parent is null) {
        if (parent is null) {
          //nothing to do
        } else {
          list_C_MC_Child.Parent = parent;
          list_C_MC_Child.Parent.AddToChildren(list_C_MC_Child);
        }
      } else {
        if (parent is null) {
          if (list_C_MC_Child.Parent!=List_C_MC_Parent.NoList_C_MC_Parent) {
            list_C_MC_Child.Parent.RemoveFromChildren(list_C_MC_Child);
          }
          list_C_MC_Child.Parent = null;
        } else {
          if (list_C_MC_Child.Parent!=List_C_MC_Parent.NoList_C_MC_Parent) {
            list_C_MC_Child.Parent.RemoveFromChildren(list_C_MC_Child);
          }
          list_C_MC_Child.Parent = parent;
          list_C_MC_Child.Parent.AddToChildren(list_C_MC_Child);
        }
      }
      list_C_MC_Child.onCsvUpdate();
    }
    partial void onCsvUpdate();


    /// <summary>
    /// Removes List_C_MC_Child from DC.Data.List_C_MC_Childs.
    /// </summary>
    public void Release() {
      if (Key<0) {
        throw new Exception($"List_C_MC_Child.Release(): List_C_MC_Child '{this}' is not stored in DC.Data, key is {Key}.");
      }
      onReleasing();
      DC.Data._List_C_MC_Childs.Remove(Key);
      onReleased();
    }
    partial void onReleasing();
    partial void onReleased();


    /// <summary>
    /// Removes List_C_MC_Child from parents as part of a transaction rollback of the new() statement.
    /// </summary>
    internal static void RollbackItemNew(IStorageItem item) {
      var list_C_MC_Child = (List_C_MC_Child) item;
      if (list_C_MC_Child.Parent!=null && list_C_MC_Child.Parent!=List_C_MC_Parent.NoList_C_MC_Parent) {
        list_C_MC_Child.Parent.RemoveFromChildren(list_C_MC_Child);
      }
      list_C_MC_Child.onRollbackItemNew();
    }
    partial void onRollbackItemNew();


    /// <summary>
    /// Releases List_C_MC_Child from DC.Data.List_C_MC_Childs as part of a transaction rollback of Store().
    /// </summary>
    internal static void RollbackItemStore(IStorageItem item) {
      var list_C_MC_Child = (List_C_MC_Child) item;
      list_C_MC_Child.onRollbackItemStored();
    }
    partial void onRollbackItemStored();


    /// <summary>
    /// Restores the List_C_MC_Child item data as it was before the last update as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemUpdate(IStorageItem oldStorageItem, IStorageItem newStorageItem) {
      var oldItem = (List_C_MC_Child) oldStorageItem;//an item clone with the values before item was updated
      var item = (List_C_MC_Child) newStorageItem;//is the instance whose values should be restored

      // remove updated item from parents
      var hasParentChanged = oldItem.Parent!=item.Parent;
      if (hasParentChanged && item.Parent is not null) {
        item.Parent.RemoveFromChildren(item);
      }

      // updated item: restore old values
      item.Date = oldItem.Date;
      item.Parent = oldItem.Parent;

      // add item with previous values to parents
      if (hasParentChanged && item.Parent is not null) {
        item.Parent.AddToChildren(item);
      }
      item.onRollbackItemUpdated(oldItem);
    }
    partial void onRollbackItemUpdated(List_C_MC_Child oldList_C_MC_Child);


    /// <summary>
    /// Adds List_C_MC_Child to DC.Data.List_C_MC_Childs as part of a transaction rollback of Release().
    /// </summary>
    internal static void RollbackItemRelease(IStorageItem item) {
      var list_C_MC_Child = (List_C_MC_Child) item;
      list_C_MC_Child.onRollbackItemRelease();
    }
    partial void onRollbackItemRelease();


    /// <summary>
    /// Returns property values for tracing. Parents are shown with their key instead their content.
    /// </summary>
    public string ToTraceString() {
      var returnString =
        $"{this.GetKeyOrHash()}|" +
        $" {Date.ToShortDateString()}|" +
        $" Parent {Parent?.GetKeyOrHash()}";
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
        $" {Date.ToShortDateString()}," +
        $" {Parent?.ToShortString()}";
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
        $" Date: {Date.ToShortDateString()}," +
        $" Parent: {Parent?.ToShortString()};";
      onToString(ref returnString);
      return returnString;
    }
    partial void onToString(ref string returnString);
    #endregion
  }
}
