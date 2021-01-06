//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by StorageClassGenerator
//
//     Do not change code in this file, it will get lost when the file gets 
//     auto generated again. Write your code into SingleChild_C_C_Parent.cs.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using StorageLib;


namespace DataModelSamples  {


  public partial class SingleChild_C_C_Parent: IStorageItemGeneric<SingleChild_C_C_Parent> {

    #region Properties
    //      ----------

    /// <summary>
    /// Unique identifier for SingleChild_C_C_Parent. Gets set once SingleChild_C_C_Parent gets added to DC.Data.
    /// </summary>
    public int Key { get; private set; }
    internal static void SetKey(IStorageItem singleChild_C_C_Parent, int key, bool _) {
      ((SingleChild_C_C_Parent)singleChild_C_C_Parent).Key = key;
    }


    public string Name { get; private set; }


    public SingleChild_C_C_Child? Child { get; private set; }


    /// <summary>
    /// Headers written to first line in CSV file
    /// </summary>
    internal static readonly string[] Headers = {"Key", "Name"};


    /// <summary>
    /// None existing SingleChild_C_C_Parent
    /// </summary>
    internal static SingleChild_C_C_Parent NoSingleChild_C_C_Parent = new SingleChild_C_C_Parent("NoName", isStoring: false);
    #endregion


    #region Events
    //      ------

    /// <summary>
    /// Content of SingleChild_C_C_Parent has changed. Gets only raised for changes occurring after loading DC.Data with previously stored data.
    /// </summary>
    public event Action</*old*/SingleChild_C_C_Parent, /*new*/SingleChild_C_C_Parent>? HasChanged;
    #endregion


    #region Constructors
    //      ------------

    /// <summary>
    /// SingleChild_C_C_Parent Constructor. If isStoring is true, adds SingleChild_C_C_Parent to DC.Data.SingleChild_C_C_Parents.
    /// </summary>
    public SingleChild_C_C_Parent(string name, bool isStoring = true) {
      Key = StorageExtensions.NoKey;
      Name = name;
      onConstruct();
      if (DC.Data.IsTransaction) {
        DC.Data.AddTransaction(new TransactionItem(17,TransactionActivityEnum.New, Key, this));
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
    public SingleChild_C_C_Parent(SingleChild_C_C_Parent original) {
    #pragma warning restore CS8618 //
      Key = StorageExtensions.NoKey;
      Name = original.Name;
      onCloned(this);
    }
    partial void onCloned(SingleChild_C_C_Parent clone);


    /// <summary>
    /// Constructor for SingleChild_C_C_Parent read from CSV file
    /// </summary>
    private SingleChild_C_C_Parent(int key, CsvReader csvReader){
      Key = key;
      Name = csvReader.ReadString();
      onCsvConstruct();
    }
    partial void onCsvConstruct();


    /// <summary>
    /// New SingleChild_C_C_Parent read from CSV file
    /// </summary>
    internal static SingleChild_C_C_Parent Create(int key, CsvReader csvReader) {
      return new SingleChild_C_C_Parent(key, csvReader);
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Adds SingleChild_C_C_Parent to DC.Data.SingleChild_C_C_Parents.<br/>
    /// Throws an Exception when SingleChild_C_C_Parent is already stored.
    /// </summary>
    public void Store() {
      if (Key>=0) {
        throw new Exception($"SingleChild_C_C_Parent cannot be stored again in DC.Data, key {Key} is greater equal 0." + Environment.NewLine + ToString());
      }

      var isCancelled = false;
      onStoring(ref isCancelled);
      if (isCancelled) return;

      DC.Data._SingleChild_C_C_Parents.Add(this);
      onStored();
    }
    partial void onStoring(ref bool isCancelled);
    partial void onStored();


    /// <summary>
    /// Estimated number of UTF8 characters needed to write SingleChild_C_C_Parent to CSV file
    /// </summary>
    public const int EstimatedLineLength = 150;


    /// <summary>
    /// Write SingleChild_C_C_Parent to CSV file
    /// </summary>
    internal static void Write(SingleChild_C_C_Parent singleChild_C_C_Parent, CsvWriter csvWriter) {
      singleChild_C_C_Parent.onCsvWrite();
      csvWriter.Write(singleChild_C_C_Parent.Name);
    }
    partial void onCsvWrite();


    /// <summary>
    /// Updates SingleChild_C_C_Parent with the provided values
    /// </summary>
    public void Update(string name) {
      var clone = new SingleChild_C_C_Parent(this);
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
          DC.Data._SingleChild_C_C_Parents.ItemHasChanged(clone, this);
        } else if (DC.Data.IsTransaction) {
          DC.Data.AddTransaction(new TransactionItem(17, TransactionActivityEnum.Update, Key, this, oldItem: clone));
        }
        HasChanged?.Invoke(clone, this);
      }
    }
    partial void onUpdating(string name, ref bool isCancelled);
    partial void onUpdated(SingleChild_C_C_Parent old);


    /// <summary>
    /// Updates this SingleChild_C_C_Parent with values from CSV file
    /// </summary>
    internal static void Update(SingleChild_C_C_Parent singleChild_C_C_Parent, CsvReader csvReader){
      singleChild_C_C_Parent.Name = csvReader.ReadString();
      singleChild_C_C_Parent.onCsvUpdate();
    }
    partial void onCsvUpdate();


    /// <summary>
    /// Add singleChild_C_C_Child to Child.
    /// </summary>
    internal void AddToChild(SingleChild_C_C_Child singleChild_C_C_Child) {
#if DEBUG
      if (singleChild_C_C_Child==SingleChild_C_C_Child.NoSingleChild_C_C_Child) throw new Exception();
      if ((singleChild_C_C_Child.Key>=0)&&(Key<0)) throw new Exception();
      if(Child==singleChild_C_C_Child) throw new Exception();
#endif
      if (Child!=null) {
        throw new Exception($"SingleChild_C_C_Parent.AddToChild(): '{Child}' is already assigned to Child, it is not possible to assign now '{singleChild_C_C_Child}'.");
      }
      Child = singleChild_C_C_Child;
      onAddedToChild(singleChild_C_C_Child);
    }
    partial void onAddedToChild(SingleChild_C_C_Child singleChild_C_C_Child);


    /// <summary>
    /// Removes singleChild_C_C_Child from SingleChild_C_C_Parent.
    /// </summary>
    internal void RemoveFromChild(SingleChild_C_C_Child singleChild_C_C_Child) {
#if DEBUG
      if (Child!=singleChild_C_C_Child) {
        throw new Exception($"SingleChild_C_C_Parent.RemoveFromChild(): Child does not link to singleChild_C_C_Child '{singleChild_C_C_Child}' but '{Child}'.");
      }
#endif
      Child = null;
      onRemovedFromChild(singleChild_C_C_Child);
    }
    partial void onRemovedFromChild(SingleChild_C_C_Child singleChild_C_C_Child);


    /// <summary>
    /// Removes SingleChild_C_C_Parent from DC.Data.SingleChild_C_C_Parents.
    /// </summary>
    public void Release() {
      if (Key<0) {
        throw new Exception($"SingleChild_C_C_Parent.Release(): SingleChild_C_C_Parent '{this}' is not stored in DC.Data, key is {Key}.");
      }
      if (Child?.Key>=0) {
        throw new Exception($"Cannot release SingleChild_C_C_Parent '{this}' " + Environment.NewLine + 
          $"because '{Child}' in SingleChild_C_C_Parent.Child is still stored.");
      }
      onReleased();
      DC.Data._SingleChild_C_C_Parents.Remove(Key);
    }
    partial void onReleased();


    /// <summary>
    /// Undoes the new() statement as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemNew(IStorageItem item) {
      var singleChild_C_C_Parent = (SingleChild_C_C_Parent) item;
      singleChild_C_C_Parent.onRollbackItemNew();
    }
    partial void onRollbackItemNew();


    /// <summary>
    /// Releases SingleChild_C_C_Parent from DC.Data.SingleChild_C_C_Parents as part of a transaction rollback of Store().
    /// </summary>
    internal static void RollbackItemStore(IStorageItem item) {
      var singleChild_C_C_Parent = (SingleChild_C_C_Parent) item;
      singleChild_C_C_Parent.onRollbackItemStored();
    }
    partial void onRollbackItemStored();


    /// <summary>
    /// Restores the SingleChild_C_C_Parent item data as it was before the last update as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemUpdate(IStorageItem oldStorageItem, IStorageItem newStorageItem) {
      var oldItem = (SingleChild_C_C_Parent) oldStorageItem;//an item clone with the values before item was updated
      var item = (SingleChild_C_C_Parent) newStorageItem;//is the instance whose values should be restored

      // updated item: restore old values
      item.Name = oldItem.Name;
      item.onRollbackItemUpdated(oldItem);
    }
    partial void onRollbackItemUpdated(SingleChild_C_C_Parent oldSingleChild_C_C_Parent);


    /// <summary>
    /// Adds SingleChild_C_C_Parent to DC.Data.SingleChild_C_C_Parents as part of a transaction rollback of Release().
    /// </summary>
    internal static void RollbackItemRelease(IStorageItem item) {
      var singleChild_C_C_Parent = (SingleChild_C_C_Parent) item;
      singleChild_C_C_Parent.onRollbackItemRelease();
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
        $" Child: {Child?.ToShortString()};";
      onToString(ref returnString);
      return returnString;
    }
    partial void onToString(ref string returnString);
    #endregion
  }
}
