//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by StorageClassGenerator
//
//     Do not change code in this file, it will get lost when the file gets 
//     auto generated again. Write your code into ReadonlyPropertyClass.cs.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using StorageLib;


namespace DataModelSamples  {


  public partial class ReadonlyPropertyClass: IStorageItemGeneric<ReadonlyPropertyClass> {

    #region Properties
    //      ----------

    /// <summary>
    /// Unique identifier for ReadonlyPropertyClass. Gets set once ReadonlyPropertyClass gets added to DC.Data.
    /// </summary>
    public int Key { get; private set; }
    internal static void SetKey(IStorageItem readonlyPropertyClass, int key, bool _) {
      ((ReadonlyPropertyClass)readonlyPropertyClass).Key = key;
    }


    public string Name { get; }


    public string Address { get; private set; }


    /// <summary>
    /// Headers written to first line in CSV file
    /// </summary>
    internal static readonly string[] Headers = {"Key", "Name", "Address"};


    /// <summary>
    /// None existing ReadonlyPropertyClass, used as a temporary place holder when reading a CSV file
    /// which was not compacted. It might create first a later deleted item linking to a 
    /// deleted parent. In this case, the parent property gets set to NoReadonlyPropertyClass. Once the CSV
    /// file is completely read, that child will actually be deleted (released) and Verify()
    /// ensures that there are no stored children with links to NoReadonlyPropertyClass.
    /// </summary>
    internal static ReadonlyPropertyClass NoReadonlyPropertyClass = new ReadonlyPropertyClass("NoName", "NoAddress", isStoring: false);
    #endregion


    #region Events
    //      ------

    /// <summary>
    /// Content of ReadonlyPropertyClass has changed. Gets only raised for changes occurring after loading DC.Data with previously stored data.
    /// </summary>
    public event Action</*old*/ReadonlyPropertyClass, /*new*/ReadonlyPropertyClass>? HasChanged;
    #endregion


    #region Constructors
    //      ------------

    /// <summary>
    /// ReadonlyPropertyClass Constructor. If isStoring is true, adds ReadonlyPropertyClass to DC.Data.ReadonlyPropertyClasss.
    /// </summary>
    public ReadonlyPropertyClass(string name, string address, bool isStoring = true) {
      Key = StorageExtensions.NoKey;
      Name = name;
      Address = address;
      onConstruct();
      if (DC.Data.IsTransaction) {
        DC.Data.AddTransaction(new TransactionItem(5,TransactionActivityEnum.New, Key, this));
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
    public ReadonlyPropertyClass(ReadonlyPropertyClass original) {
    #pragma warning restore CS8618 //
      Key = StorageExtensions.NoKey;
      Name = original.Name;
      Address = original.Address;
      onCloned(this);
    }
    partial void onCloned(ReadonlyPropertyClass clone);


    /// <summary>
    /// Constructor for ReadonlyPropertyClass read from CSV file
    /// </summary>
    private ReadonlyPropertyClass(int key, CsvReader csvReader){
      Key = key;
      Name = csvReader.ReadString();
      Address = csvReader.ReadString();
      onCsvConstruct();
    }
    partial void onCsvConstruct();


    /// <summary>
    /// New ReadonlyPropertyClass read from CSV file
    /// </summary>
    internal static ReadonlyPropertyClass Create(int key, CsvReader csvReader) {
      return new ReadonlyPropertyClass(key, csvReader);
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Adds ReadonlyPropertyClass to DC.Data.ReadonlyPropertyClasss.<br/>
    /// Throws an Exception when ReadonlyPropertyClass is already stored.
    /// </summary>
    public void Store() {
      if (Key>=0) {
        throw new Exception($"ReadonlyPropertyClass cannot be stored again in DC.Data, key {Key} is greater equal 0." + Environment.NewLine + ToString());
      }

      var isCancelled = false;
      onStoring(ref isCancelled);
      if (isCancelled) return;

      DC.Data._ReadonlyPropertyClasss.Add(this);
      onStored();
    }
    partial void onStoring(ref bool isCancelled);
    partial void onStored();


    /// <summary>
    /// Estimated number of UTF8 characters needed to write ReadonlyPropertyClass to CSV file
    /// </summary>
    public const int EstimatedLineLength = 300;


    /// <summary>
    /// Write ReadonlyPropertyClass to CSV file
    /// </summary>
    internal static void Write(ReadonlyPropertyClass readonlyPropertyClass, CsvWriter csvWriter) {
      readonlyPropertyClass.onCsvWrite();
      csvWriter.Write(readonlyPropertyClass.Name);
      csvWriter.Write(readonlyPropertyClass.Address);
    }
    partial void onCsvWrite();


    /// <summary>
    /// Updates ReadonlyPropertyClass with the provided values
    /// </summary>
    public void Update(string address) {
      var clone = new ReadonlyPropertyClass(this);
      var isCancelled = false;
      onUpdating(address, ref isCancelled);
      if (isCancelled) return;


      //update properties and detect if any value has changed
      var isChangeDetected = false;
      if (Address!=address) {
        Address = address;
        isChangeDetected = true;
      }
      if (isChangeDetected) {
        onUpdated(clone);
        if (Key>=0) {
          DC.Data._ReadonlyPropertyClasss.ItemHasChanged(clone, this);
        } else if (DC.Data.IsTransaction) {
          DC.Data.AddTransaction(new TransactionItem(5, TransactionActivityEnum.Update, Key, this, oldItem: clone));
        }
        HasChanged?.Invoke(clone, this);
      }
    }
    partial void onUpdating(string address, ref bool isCancelled);
    partial void onUpdated(ReadonlyPropertyClass old);


    /// <summary>
    /// Updates this ReadonlyPropertyClass with values from CSV file
    /// </summary>
    internal static void Update(ReadonlyPropertyClass readonlyPropertyClass, CsvReader csvReader){
      var name = csvReader.ReadString();
      if (readonlyPropertyClass.Name!=name) {
        throw new Exception($"ReadonlyPropertyClass.Update(): Property Name '{readonlyPropertyClass.Name}' is " +
          $"readonly, name '{name}' read from the CSV file should be the same." + Environment.NewLine + 
          readonlyPropertyClass.ToString());
      }
      readonlyPropertyClass.Address = csvReader.ReadString();
      readonlyPropertyClass.onCsvUpdate();
    }
    partial void onCsvUpdate();


    /// <summary>
    /// Removes ReadonlyPropertyClass from DC.Data.ReadonlyPropertyClasss.
    /// </summary>
    public void Release() {
      if (Key<0) {
        throw new Exception($"ReadonlyPropertyClass.Release(): ReadonlyPropertyClass '{this}' is not stored in DC.Data, key is {Key}.");
      }
      DC.Data._ReadonlyPropertyClasss.Remove(Key);
      onReleased();
    }
    partial void onReleased();


    /// <summary>
    /// Undoes the new() statement as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemNew(IStorageItem item) {
      var readonlyPropertyClass = (ReadonlyPropertyClass) item;
      readonlyPropertyClass.onRollbackItemNew();
    }
    partial void onRollbackItemNew();


    /// <summary>
    /// Releases ReadonlyPropertyClass from DC.Data.ReadonlyPropertyClasss as part of a transaction rollback of Store().
    /// </summary>
    internal static void RollbackItemStore(IStorageItem item) {
      var readonlyPropertyClass = (ReadonlyPropertyClass) item;
      readonlyPropertyClass.onRollbackItemStored();
    }
    partial void onRollbackItemStored();


    /// <summary>
    /// Restores the ReadonlyPropertyClass item data as it was before the last update as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemUpdate(IStorageItem oldStorageItem, IStorageItem newStorageItem) {
      var oldItem = (ReadonlyPropertyClass) oldStorageItem;//an item clone with the values before item was updated
      var item = (ReadonlyPropertyClass) newStorageItem;//is the instance whose values should be restored

      // if possible, throw exceptions before changing anything
      if (item.Name!=oldItem.Name) {
        throw new Exception($"ReadonlyPropertyClass.Update(): Property Name '{item.Name}' is " +
          $"readonly, Name '{oldItem.Name}' should be the same." + Environment.NewLine + 
          item.ToString());
      }

      // updated item: restore old values
      item.Address = oldItem.Address;
      item.onRollbackItemUpdated(oldItem);
    }
    partial void onRollbackItemUpdated(ReadonlyPropertyClass oldReadonlyPropertyClass);


    /// <summary>
    /// Adds ReadonlyPropertyClass to DC.Data.ReadonlyPropertyClasss as part of a transaction rollback of Release().
    /// </summary>
    internal static void RollbackItemRelease(IStorageItem item) {
      var readonlyPropertyClass = (ReadonlyPropertyClass) item;
      readonlyPropertyClass.onRollbackItemRelease();
    }
    partial void onRollbackItemRelease();


    /// <summary>
    /// Returns property values for tracing. Parents are shown with their key instead their content.
    /// </summary>
    public string ToTraceString() {
      var returnString =
        $"{this.GetKeyOrHash()}|" +
        $" {Name}|" +
        $" {Address}";
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
        $" {Address}";
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
        $" Address: {Address};";
      onToString(ref returnString);
      return returnString;
    }
    partial void onToString(ref string returnString);
    #endregion
  }
}
