//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by StorageClassGenerator
//
//     Do not change code in this file, it will get lost when the file gets 
//     auto generated again. Write your code into ToLowerCasePropertyClass.cs.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using StorageLib;


namespace DataModelSamples  {


  public partial class ToLowerCasePropertyClass: IStorageItem<ToLowerCasePropertyClass> {

    #region Properties
    //      ----------

    /// <summary>
    /// Unique identifier for ToLowerCasePropertyClass. Gets set once ToLowerCasePropertyClass gets added to DC.Data.
    /// </summary>
    public int Key { get; private set; }
    internal static void SetKey(IStorageItem toLowerCasePropertyClass, int key, bool _) {
      ((ToLowerCasePropertyClass)toLowerCasePropertyClass).Key = key;
    }


    public string Name { get; private set; }


    public string NameLower { get; private set; }


    /// <summary>
    /// Headers written to first line in CSV file
    /// </summary>
    internal static readonly string[] Headers = {"Key", "Name"};


    /// <summary>
    /// None existing ToLowerCasePropertyClass, used as a temporary place holder when reading a CSV file
    /// which was not compacted. It might create first a later deleted item linking to a 
    /// deleted parent. In this case, the parent property gets set to NoToLowerCasePropertyClass. Once the CSV
    /// file is completely read, that child will actually be deleted (released) and Verify()
    /// ensures that there are no stored children with links to NoToLowerCasePropertyClass.
    /// </summary>
    internal static ToLowerCasePropertyClass NoToLowerCasePropertyClass = new ToLowerCasePropertyClass("NoName", isStoring: false);
    #endregion


    #region Events
    //      ------

    /// <summary>
    /// Content of ToLowerCasePropertyClass has changed. Gets only raised for changes occurring after loading DC.Data with previously stored data.
    /// </summary>
    public event Action</*old*/ToLowerCasePropertyClass, /*new*/ToLowerCasePropertyClass>? HasChanged;
    #endregion


    #region Constructors
    //      ------------

    /// <summary>
    /// ToLowerCasePropertyClass Constructor. If isStoring is true, adds ToLowerCasePropertyClass to DC.Data.ToLowerCasePropertyClasss.
    /// </summary>
    public ToLowerCasePropertyClass(string name, bool isStoring = true) {
      Key = StorageExtensions.NoKey;
      Name = name;
      NameLower = Name.ToLowerInvariant();
      onConstruct();
      if (DC.Data.IsTransaction) {
        DC.Data.AddTransaction(new TransactionItem(8,TransactionActivityEnum.New, Key, this));
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
    public ToLowerCasePropertyClass(ToLowerCasePropertyClass original) {
    #pragma warning restore CS8618 //
      Key = StorageExtensions.NoKey;
      Name = original.Name;
      NameLower = original.NameLower;
      onCloned(this);
    }
    partial void onCloned(ToLowerCasePropertyClass clone);


    /// <summary>
    /// Constructor for ToLowerCasePropertyClass read from CSV file
    /// </summary>
    private ToLowerCasePropertyClass(int key, CsvReader csvReader){
      Key = key;
      Name = csvReader.ReadString();
      NameLower = Name.ToLowerInvariant();
      onCsvConstruct();
    }
    partial void onCsvConstruct();


    /// <summary>
    /// New ToLowerCasePropertyClass read from CSV file
    /// </summary>
    internal static ToLowerCasePropertyClass Create(int key, CsvReader csvReader) {
      return new ToLowerCasePropertyClass(key, csvReader);
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Adds ToLowerCasePropertyClass to DC.Data.ToLowerCasePropertyClasss.<br/>
    /// Throws an Exception when ToLowerCasePropertyClass is already stored.
    /// </summary>
    public void Store() {
      if (Key>=0) {
        throw new Exception($"ToLowerCasePropertyClass cannot be stored again in DC.Data, key {Key} is greater equal 0." + Environment.NewLine + ToString());
      }

      var isCancelled = false;
      onStoring(ref isCancelled);
      if (isCancelled) return;

      DC.Data._ToLowerCasePropertyClasss.Add(this);
      onStored();
    }
    partial void onStoring(ref bool isCancelled);
    partial void onStored();


    /// <summary>
    /// Estimated number of UTF8 characters needed to write ToLowerCasePropertyClass to CSV file
    /// </summary>
    public const int EstimatedLineLength = 150;


    /// <summary>
    /// Write ToLowerCasePropertyClass to CSV file
    /// </summary>
    internal static void Write(ToLowerCasePropertyClass toLowerCasePropertyClass, CsvWriter csvWriter) {
      toLowerCasePropertyClass.onCsvWrite();
      csvWriter.Write(toLowerCasePropertyClass.Name);
    }
    partial void onCsvWrite();


    /// <summary>
    /// Updates ToLowerCasePropertyClass with the provided values
    /// </summary>
    public void Update(string name) {
      var clone = new ToLowerCasePropertyClass(this);
      var isCancelled = false;
      onUpdating(name, ref isCancelled);
      if (isCancelled) return;


      //update properties and detect if any value has changed
      var isChangeDetected = false;
      if (Name!=name) {
        Name = name;
        NameLower = Name.ToLowerInvariant();
        isChangeDetected = true;
      }
      if (isChangeDetected) {
        onUpdated(clone);
        if (Key>=0) {
          DC.Data._ToLowerCasePropertyClasss.ItemHasChanged(clone, this);
        } else if (DC.Data.IsTransaction) {
          DC.Data.AddTransaction(new TransactionItem(8, TransactionActivityEnum.Update, Key, this, oldItem: clone));
        }
        HasChanged?.Invoke(clone, this);
      }
    }
    partial void onUpdating(string name, ref bool isCancelled);
    partial void onUpdated(ToLowerCasePropertyClass old);


    /// <summary>
    /// Updates this ToLowerCasePropertyClass with values from CSV file
    /// </summary>
    internal static void Update(ToLowerCasePropertyClass toLowerCasePropertyClass, CsvReader csvReader){
      toLowerCasePropertyClass.Name = csvReader.ReadString();
      toLowerCasePropertyClass.NameLower = toLowerCasePropertyClass.Name.ToLowerInvariant();
      toLowerCasePropertyClass.onCsvUpdate();
    }
    partial void onCsvUpdate();


    /// <summary>
    /// Removes ToLowerCasePropertyClass from DC.Data.ToLowerCasePropertyClasss.
    /// </summary>
    public void Release() {
      if (Key<0) {
        throw new Exception($"ToLowerCasePropertyClass.Release(): ToLowerCasePropertyClass '{this}' is not stored in DC.Data, key is {Key}.");
      }
      DC.Data._ToLowerCasePropertyClasss.Remove(Key);
      onReleased();
    }
    partial void onReleased();


    /// <summary>
    /// Undoes the new() statement as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemNew(IStorageItem item) {
      var toLowerCasePropertyClass = (ToLowerCasePropertyClass) item;
      toLowerCasePropertyClass.onRollbackItemNew();
    }
    partial void onRollbackItemNew();


    /// <summary>
    /// Releases ToLowerCasePropertyClass from DC.Data.ToLowerCasePropertyClasss as part of a transaction rollback of Store().
    /// </summary>
    internal static void RollbackItemStore(IStorageItem item) {
      var toLowerCasePropertyClass = (ToLowerCasePropertyClass) item;
      toLowerCasePropertyClass.onRollbackItemStored();
    }
    partial void onRollbackItemStored();


    /// <summary>
    /// Restores the ToLowerCasePropertyClass item data as it was before the last update as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemUpdate(IStorageItem oldStorageItem, IStorageItem newStorageItem) {
      var oldItem = (ToLowerCasePropertyClass) oldStorageItem;//an item clone with the values before item was updated
      var item = (ToLowerCasePropertyClass) newStorageItem;//is the instance whose values should be restored

      // updated item: restore old values
      item.Name = oldItem.Name;
      item.NameLower = item.Name.ToLowerInvariant();
      item.onRollbackItemUpdated(oldItem);
    }
    partial void onRollbackItemUpdated(ToLowerCasePropertyClass oldToLowerCasePropertyClass);


    /// <summary>
    /// Adds ToLowerCasePropertyClass to DC.Data.ToLowerCasePropertyClasss as part of a transaction rollback of Release().
    /// </summary>
    internal static void RollbackItemRelease(IStorageItem item) {
      var toLowerCasePropertyClass = (ToLowerCasePropertyClass) item;
      toLowerCasePropertyClass.onRollbackItemRelease();
    }
    partial void onRollbackItemRelease();


    /// <summary>
    /// Returns property values for tracing. Parents are shown with their key instead their content.
    /// </summary>
    public string ToTraceString() {
      var returnString =
        $"{this.GetKeyOrHash()}|" +
        $" {Name}|" +
        $" {NameLower}";
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
        $" {NameLower}";
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
        $" NameLower: {NameLower};";
      onToString(ref returnString);
      return returnString;
    }
    partial void onToString(ref string returnString);
    #endregion
  }
}
