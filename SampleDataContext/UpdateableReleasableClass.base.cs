//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by StorageClassGenerator
//
//     Do not change code in this file, it will get lost when the file gets 
//     auto generated again. Write your code into UpdateableReleasableClass.cs.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using StorageLib;


namespace DataModelSamples  {


  public partial class UpdateableReleasableClass: IStorageItemGeneric<UpdateableReleasableClass> {

    #region Properties
    //      ----------

    /// <summary>
    /// Unique identifier for UpdateableReleasableClass. Gets set once UpdateableReleasableClass gets added to DC.Data.
    /// </summary>
    public int Key { get; private set; }
    internal static void SetKey(IStorageItem updateableReleasableClass, int key, bool _) {
      ((UpdateableReleasableClass)updateableReleasableClass).Key = key;
    }


    public string Name { get; private set; }


    /// <summary>
    /// Headers written to first line in CSV file
    /// </summary>
    internal static readonly string[] Headers = {"Key", "Name"};


    /// <summary>
    /// None existing UpdateableReleasableClass
    /// </summary>
    internal static UpdateableReleasableClass NoUpdateableReleasableClass = new UpdateableReleasableClass("NoName", isStoring: false);
    #endregion


    #region Events
    //      ------

    /// <summary>
    /// Content of UpdateableReleasableClass has changed. Gets only raised for changes occurring after loading DC.Data with previously stored data.
    /// </summary>
    public event Action</*old*/UpdateableReleasableClass, /*new*/UpdateableReleasableClass>? HasChanged;
    #endregion


    #region Constructors
    //      ------------

    /// <summary>
    /// UpdateableReleasableClass Constructor. If isStoring is true, adds UpdateableReleasableClass to DC.Data.UpdateableReleasableClasss.
    /// </summary>
    public UpdateableReleasableClass(string name, bool isStoring = true) {
      Key = StorageExtensions.NoKey;
      Name = name;
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
    public UpdateableReleasableClass(UpdateableReleasableClass original) {
    #pragma warning restore CS8618 //
      Key = StorageExtensions.NoKey;
      Name = original.Name;
      onCloned(this);
    }
    partial void onCloned(UpdateableReleasableClass clone);


    /// <summary>
    /// Constructor for UpdateableReleasableClass read from CSV file
    /// </summary>
    private UpdateableReleasableClass(int key, CsvReader csvReader){
      Key = key;
      Name = csvReader.ReadString();
      onCsvConstruct();
    }
    partial void onCsvConstruct();


    /// <summary>
    /// New UpdateableReleasableClass read from CSV file
    /// </summary>
    internal static UpdateableReleasableClass Create(int key, CsvReader csvReader) {
      return new UpdateableReleasableClass(key, csvReader);
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Adds UpdateableReleasableClass to DC.Data.UpdateableReleasableClasss.<br/>
    /// Throws an Exception when UpdateableReleasableClass is already stored.
    /// </summary>
    public void Store() {
      if (Key>=0) {
        throw new Exception($"UpdateableReleasableClass cannot be stored again in DC.Data, key {Key} is greater equal 0." + Environment.NewLine + ToString());
      }

      var isCancelled = false;
      onStoring(ref isCancelled);
      if (isCancelled) return;

      DC.Data._UpdateableReleasableClasss.Add(this);
      onStored();
    }
    partial void onStoring(ref bool isCancelled);
    partial void onStored();


    /// <summary>
    /// Estimated number of UTF8 characters needed to write UpdateableReleasableClass to CSV file
    /// </summary>
    public const int EstimatedLineLength = 150;


    /// <summary>
    /// Write UpdateableReleasableClass to CSV file
    /// </summary>
    internal static void Write(UpdateableReleasableClass updateableReleasableClass, CsvWriter csvWriter) {
      updateableReleasableClass.onCsvWrite();
      csvWriter.Write(updateableReleasableClass.Name);
    }
    partial void onCsvWrite();


    /// <summary>
    /// Updates UpdateableReleasableClass with the provided values
    /// </summary>
    public void Update(string name) {
      var clone = new UpdateableReleasableClass(this);
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
          DC.Data._UpdateableReleasableClasss.ItemHasChanged(clone, this);
        } else if (DC.Data.IsTransaction) {
          DC.Data.AddTransaction(new TransactionItem(0, TransactionActivityEnum.Update, Key, this, oldItem: clone));
        }
        HasChanged?.Invoke(clone, this);
      }
    }
    partial void onUpdating(string name, ref bool isCancelled);
    partial void onUpdated(UpdateableReleasableClass old);


    /// <summary>
    /// Updates this UpdateableReleasableClass with values from CSV file
    /// </summary>
    internal static void Update(UpdateableReleasableClass updateableReleasableClass, CsvReader csvReader){
      updateableReleasableClass.Name = csvReader.ReadString();
      updateableReleasableClass.onCsvUpdate();
    }
    partial void onCsvUpdate();


    /// <summary>
    /// Removes UpdateableReleasableClass from DC.Data.UpdateableReleasableClasss.
    /// </summary>
    public void Release() {
      if (Key<0) {
        throw new Exception($"UpdateableReleasableClass.Release(): UpdateableReleasableClass '{this}' is not stored in DC.Data, key is {Key}.");
      }
      onReleased();
      DC.Data._UpdateableReleasableClasss.Remove(Key);
    }
    partial void onReleased();


    /// <summary>
    /// Undoes the new() statement as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemNew(IStorageItem item) {
      var updateableReleasableClass = (UpdateableReleasableClass) item;
      updateableReleasableClass.onRollbackItemNew();
    }
    partial void onRollbackItemNew();


    /// <summary>
    /// Releases UpdateableReleasableClass from DC.Data.UpdateableReleasableClasss as part of a transaction rollback of Store().
    /// </summary>
    internal static void RollbackItemStore(IStorageItem item) {
      var updateableReleasableClass = (UpdateableReleasableClass) item;
      updateableReleasableClass.onRollbackItemStored();
    }
    partial void onRollbackItemStored();


    /// <summary>
    /// Restores the UpdateableReleasableClass item data as it was before the last update as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemUpdate(IStorageItem oldStorageItem, IStorageItem newStorageItem) {
      var oldItem = (UpdateableReleasableClass) oldStorageItem;//an item clone with the values before item was updated
      var item = (UpdateableReleasableClass) newStorageItem;//is the instance whose values should be restored

      // updated item: restore old values
      item.Name = oldItem.Name;
      item.onRollbackItemUpdated(oldItem);
    }
    partial void onRollbackItemUpdated(UpdateableReleasableClass oldUpdateableReleasableClass);


    /// <summary>
    /// Adds UpdateableReleasableClass to DC.Data.UpdateableReleasableClasss as part of a transaction rollback of Release().
    /// </summary>
    internal static void RollbackItemRelease(IStorageItem item) {
      var updateableReleasableClass = (UpdateableReleasableClass) item;
      updateableReleasableClass.onRollbackItemRelease();
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
        $" Name: {Name};";
      onToString(ref returnString);
      return returnString;
    }
    partial void onToString(ref string returnString);
    #endregion
  }
}
