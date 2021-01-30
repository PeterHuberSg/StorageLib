//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by StorageClassGenerator
//
//     Do not change code in this file, it will get lost when the file gets 
//     auto generated again. Write your code into SingleChild_C_C_Child.cs.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using StorageLib;


namespace DataModelSamples  {


  public partial class SingleChild_C_C_Child: IStorageItem<SingleChild_C_C_Child> {

    #region Properties
    //      ----------

    /// <summary>
    /// Unique identifier for SingleChild_C_C_Child. Gets set once SingleChild_C_C_Child gets added to DC.Data.
    /// </summary>
    public int Key { get; private set; }
    internal static void SetKey(IStorageItem singleChild_C_C_Child, int key, bool _) {
      ((SingleChild_C_C_Child)singleChild_C_C_Child).Key = key;
    }


    public string Name { get; private set; }


    public SingleChild_C_C_Parent? Parent { get; private set; }


    /// <summary>
    /// Headers written to first line in CSV file
    /// </summary>
    internal static readonly string[] Headers = {"Key", "Name", "Parent"};


    /// <summary>
    /// None existing SingleChild_C_C_Child, used as a temporary place holder when reading a CSV file
    /// which was not compacted. It might create first a later deleted item linking to a 
    /// deleted parent. In this case, the parent property gets set to NoSingleChild_C_C_Child. Once the CSV
    /// file is completely read, that child will actually be deleted (released) and Verify()
    /// ensures that there are no stored children with links to NoSingleChild_C_C_Child.
    /// </summary>
    internal static SingleChild_C_C_Child NoSingleChild_C_C_Child = new SingleChild_C_C_Child("NoName", null, isStoring: false);
    #endregion


    #region Events
    //      ------

    /// <summary>
    /// Content of SingleChild_C_C_Child has changed. Gets only raised for changes occurring after loading DC.Data with previously stored data.
    /// </summary>
    public event Action</*old*/SingleChild_C_C_Child, /*new*/SingleChild_C_C_Child>? HasChanged;
    #endregion


    #region Constructors
    //      ------------

    /// <summary>
    /// SingleChild_C_C_Child Constructor. If isStoring is true, adds SingleChild_C_C_Child to DC.Data.SingleChild_C_C_Childs.
    /// </summary>
    public SingleChild_C_C_Child(string name, SingleChild_C_C_Parent? parent, bool isStoring = true) {
      Key = StorageExtensions.NoKey;
      Name = name;
      Parent = parent;
      if (Parent!=null) {
        Parent.AddToChild(this);
      }
      onConstruct();
      if (DC.Data.IsTransaction) {
        DC.Data.AddTransaction(new TransactionItem(18,TransactionActivityEnum.New, Key, this));
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
    public SingleChild_C_C_Child(SingleChild_C_C_Child original) {
    #pragma warning restore CS8618 //
      Key = StorageExtensions.NoKey;
      Name = original.Name;
      Parent = original.Parent;
      onCloned(this);
    }
    partial void onCloned(SingleChild_C_C_Child clone);


    /// <summary>
    /// Constructor for SingleChild_C_C_Child read from CSV file
    /// </summary>
    private SingleChild_C_C_Child(int key, CsvReader csvReader){
      Key = key;
      Name = csvReader.ReadString();
      var parentKey = csvReader.ReadIntNull();
      if (parentKey.HasValue) {
        Parent = DC.Data._SingleChild_C_C_Parents.GetItem(parentKey.Value)?? SingleChild_C_C_Parent.NoSingleChild_C_C_Parent;
      }
      if (parentKey.HasValue && Parent!=SingleChild_C_C_Parent.NoSingleChild_C_C_Parent) {
        Parent!.AddToChild(this);
      }
      onCsvConstruct();
    }
    partial void onCsvConstruct();


    /// <summary>
    /// New SingleChild_C_C_Child read from CSV file
    /// </summary>
    internal static SingleChild_C_C_Child Create(int key, CsvReader csvReader) {
      return new SingleChild_C_C_Child(key, csvReader);
    }


    /// <summary>
    /// Verify that singleChild_C_C_Child.Parent exists.
    /// </summary>
    internal static bool Verify(SingleChild_C_C_Child singleChild_C_C_Child) {
      if (singleChild_C_C_Child.Parent==SingleChild_C_C_Parent.NoSingleChild_C_C_Parent) return false;
      return true;
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Adds SingleChild_C_C_Child to DC.Data.SingleChild_C_C_Childs.<br/>
    /// Throws an Exception when SingleChild_C_C_Child is already stored.
    /// </summary>
    public void Store() {
      if (Key>=0) {
        throw new Exception($"SingleChild_C_C_Child cannot be stored again in DC.Data, key {Key} is greater equal 0." + Environment.NewLine + ToString());
      }

      var isCancelled = false;
      onStoring(ref isCancelled);
      if (isCancelled) return;

      if (Parent?.Key<0) {
        throw new Exception($"Cannot store child SingleChild_C_C_Child '{this}'.Parent to SingleChild_C_C_Parent '{Parent}' because parent is not stored yet.");
      }
      DC.Data._SingleChild_C_C_Childs.Add(this);
      onStored();
    }
    partial void onStoring(ref bool isCancelled);
    partial void onStored();


    /// <summary>
    /// Estimated number of UTF8 characters needed to write SingleChild_C_C_Child to CSV file
    /// </summary>
    public const int EstimatedLineLength = 150;


    /// <summary>
    /// Write SingleChild_C_C_Child to CSV file
    /// </summary>
    internal static void Write(SingleChild_C_C_Child singleChild_C_C_Child, CsvWriter csvWriter) {
      singleChild_C_C_Child.onCsvWrite();
      csvWriter.Write(singleChild_C_C_Child.Name);
      if (singleChild_C_C_Child.Parent is null) {
        csvWriter.WriteNull();
      } else {
        if (singleChild_C_C_Child.Parent.Key<0) throw new Exception($"Cannot write singleChild_C_C_Child '{singleChild_C_C_Child}' to CSV File, because Parent is not stored in DC.Data.SingleChild_C_C_Parents.");

        csvWriter.Write(singleChild_C_C_Child.Parent.Key.ToString());
      }
    }
    partial void onCsvWrite();


    /// <summary>
    /// Updates SingleChild_C_C_Child with the provided values
    /// </summary>
    public void Update(string name, SingleChild_C_C_Parent? parent) {
      if (Key>=0){
        if (parent?.Key<0) {
          throw new Exception($"SingleChild_C_C_Child.Update(): It is illegal to add stored SingleChild_C_C_Child '{this}'" + Environment.NewLine + 
            $"to Parent '{parent}', which is not stored.");
        }
      }
      var clone = new SingleChild_C_C_Child(this);
      var isCancelled = false;
      onUpdating(name, parent, ref isCancelled);
      if (isCancelled) return;


      //remove not yet updated item from parents which will be removed by update
      var hasParentChanged = Parent!=parent;
      if (Parent is not null && hasParentChanged) {
        Parent.RemoveFromChild(this);
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
      if (Parent is not null && hasParentChanged) {
        Parent.AddToChild(this);
      }
      if (isChangeDetected) {
        onUpdated(clone);
        if (Key>=0) {
          DC.Data._SingleChild_C_C_Childs.ItemHasChanged(clone, this);
        } else if (DC.Data.IsTransaction) {
          DC.Data.AddTransaction(new TransactionItem(18, TransactionActivityEnum.Update, Key, this, oldItem: clone));
        }
        HasChanged?.Invoke(clone, this);
      }
    }
    partial void onUpdating(string name, SingleChild_C_C_Parent? parent, ref bool isCancelled);
    partial void onUpdated(SingleChild_C_C_Child old);


    /// <summary>
    /// Updates this SingleChild_C_C_Child with values from CSV file
    /// </summary>
    internal static void Update(SingleChild_C_C_Child singleChild_C_C_Child, CsvReader csvReader){
      singleChild_C_C_Child.Name = csvReader.ReadString();
      var parentKey = csvReader.ReadIntNull();
      SingleChild_C_C_Parent? parent;
      if (parentKey is null) {
        parent = null;
      } else {
        parent = DC.Data._SingleChild_C_C_Parents.GetItem(parentKey.Value)??
          SingleChild_C_C_Parent.NoSingleChild_C_C_Parent;
      }
      if (singleChild_C_C_Child.Parent is null) {
        if (parent is null) {
          //nothing to do
        } else {
          singleChild_C_C_Child.Parent = parent;
          singleChild_C_C_Child.Parent.AddToChild(singleChild_C_C_Child);
        }
      } else {
        if (parent is null) {
          if (singleChild_C_C_Child.Parent!=SingleChild_C_C_Parent.NoSingleChild_C_C_Parent) {
            singleChild_C_C_Child.Parent.RemoveFromChild(singleChild_C_C_Child);
          }
          singleChild_C_C_Child.Parent = null;
        } else {
          if (singleChild_C_C_Child.Parent!=SingleChild_C_C_Parent.NoSingleChild_C_C_Parent) {
            singleChild_C_C_Child.Parent.RemoveFromChild(singleChild_C_C_Child);
          }
          singleChild_C_C_Child.Parent = parent;
          singleChild_C_C_Child.Parent.AddToChild(singleChild_C_C_Child);
        }
      }
      singleChild_C_C_Child.onCsvUpdate();
    }
    partial void onCsvUpdate();


    /// <summary>
    /// Removes SingleChild_C_C_Child from DC.Data.SingleChild_C_C_Childs.
    /// </summary>
    public void Release() {
      if (Key<0) {
        throw new Exception($"SingleChild_C_C_Child.Release(): SingleChild_C_C_Child '{this}' is not stored in DC.Data, key is {Key}.");
      }
      DC.Data._SingleChild_C_C_Childs.Remove(Key);
      onReleased();
    }
    partial void onReleased();


    /// <summary>
    /// Removes SingleChild_C_C_Child from parents as part of a transaction rollback of the new() statement.
    /// </summary>
    internal static void RollbackItemNew(IStorageItem item) {
      var singleChild_C_C_Child = (SingleChild_C_C_Child) item;
      if (singleChild_C_C_Child.Parent!=null && singleChild_C_C_Child.Parent!=SingleChild_C_C_Parent.NoSingleChild_C_C_Parent) {
        singleChild_C_C_Child.Parent.RemoveFromChild(singleChild_C_C_Child);
      }
      singleChild_C_C_Child.onRollbackItemNew();
    }
    partial void onRollbackItemNew();


    /// <summary>
    /// Releases SingleChild_C_C_Child from DC.Data.SingleChild_C_C_Childs as part of a transaction rollback of Store().
    /// </summary>
    internal static void RollbackItemStore(IStorageItem item) {
      var singleChild_C_C_Child = (SingleChild_C_C_Child) item;
      singleChild_C_C_Child.onRollbackItemStored();
    }
    partial void onRollbackItemStored();


    /// <summary>
    /// Restores the SingleChild_C_C_Child item data as it was before the last update as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemUpdate(IStorageItem oldStorageItem, IStorageItem newStorageItem) {
      var oldItem = (SingleChild_C_C_Child) oldStorageItem;//an item clone with the values before item was updated
      var item = (SingleChild_C_C_Child) newStorageItem;//is the instance whose values should be restored

      // remove updated item from parents
      var hasParentChanged = oldItem.Parent!=item.Parent;
      if (hasParentChanged && item.Parent is not null) {
        item.Parent.RemoveFromChild(item);
      }

      // updated item: restore old values
      item.Name = oldItem.Name;
      item.Parent = oldItem.Parent;

      // add item with previous values to parents
      if (hasParentChanged && item.Parent is not null) {
        item.Parent.AddToChild(item);
      }
      item.onRollbackItemUpdated(oldItem);
    }
    partial void onRollbackItemUpdated(SingleChild_C_C_Child oldSingleChild_C_C_Child);


    /// <summary>
    /// Adds SingleChild_C_C_Child to DC.Data.SingleChild_C_C_Childs as part of a transaction rollback of Release().
    /// </summary>
    internal static void RollbackItemRelease(IStorageItem item) {
      var singleChild_C_C_Child = (SingleChild_C_C_Child) item;
      singleChild_C_C_Child.onRollbackItemRelease();
    }
    partial void onRollbackItemRelease();


    /// <summary>
    /// Returns property values for tracing. Parents are shown with their key instead their content.
    /// </summary>
    public string ToTraceString() {
      var returnString =
        $"{this.GetKeyOrHash()}|" +
        $" {Name}|" +
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
        $" {Name}," +
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
        $" Name: {Name}," +
        $" Parent: {Parent?.ToShortString()};";
      onToString(ref returnString);
      return returnString;
    }
    partial void onToString(ref string returnString);
    #endregion
  }
}
