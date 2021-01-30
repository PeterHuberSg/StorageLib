//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by StorageClassGenerator
//
//     Do not change code in this file, it will get lost when the file gets 
//     auto generated again. Write your code into PluralNameNoneStandardClass.cs.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using StorageLib;


namespace DataModelSamples  {


  public partial class PluralNameNoneStandardClass: IStorageItem<PluralNameNoneStandardClass> {

    #region Properties
    //      ----------

    /// <summary>
    /// Unique identifier for PluralNameNoneStandardClass. Gets set once PluralNameNoneStandardClass gets added to DC.Data.
    /// </summary>
    public int Key { get; private set; }
    internal static void SetKey(IStorageItem pluralNameNoneStandardClass, int key, bool _) {
      ((PluralNameNoneStandardClass)pluralNameNoneStandardClass).Key = key;
    }


    public string Name { get; private set; }


    /// <summary>
    /// Headers written to first line in CSV file
    /// </summary>
    internal static readonly string[] Headers = {"Key", "Name"};


    /// <summary>
    /// None existing PluralNameNoneStandardClass, used as a temporary place holder when reading a CSV file
    /// which was not compacted. It might create first a later deleted item linking to a 
    /// deleted parent. In this case, the parent property gets set to NoPluralNameNoneStandardClass. Once the CSV
    /// file is completely read, that child will actually be deleted (released) and Verify()
    /// ensures that there are no stored children with links to NoPluralNameNoneStandardClass.
    /// </summary>
    internal static PluralNameNoneStandardClass NoPluralNameNoneStandardClass = new PluralNameNoneStandardClass("NoName", isStoring: false);
    #endregion


    #region Events
    //      ------

    /// <summary>
    /// Content of PluralNameNoneStandardClass has changed. Gets only raised for changes occurring after loading DC.Data with previously stored data.
    /// </summary>
    public event Action</*old*/PluralNameNoneStandardClass, /*new*/PluralNameNoneStandardClass>? HasChanged;
    #endregion


    #region Constructors
    //      ------------

    /// <summary>
    /// PluralNameNoneStandardClass Constructor. If isStoring is true, adds PluralNameNoneStandardClass to DC.Data.PluralNameNoneStandardClasses.
    /// </summary>
    public PluralNameNoneStandardClass(string name, bool isStoring = true) {
      Key = StorageExtensions.NoKey;
      Name = name;
      onConstruct();
      if (DC.Data.IsTransaction) {
        DC.Data.AddTransaction(new TransactionItem(4,TransactionActivityEnum.New, Key, this));
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
    public PluralNameNoneStandardClass(PluralNameNoneStandardClass original) {
    #pragma warning restore CS8618 //
      Key = StorageExtensions.NoKey;
      Name = original.Name;
      onCloned(this);
    }
    partial void onCloned(PluralNameNoneStandardClass clone);


    /// <summary>
    /// Constructor for PluralNameNoneStandardClass read from CSV file
    /// </summary>
    private PluralNameNoneStandardClass(int key, CsvReader csvReader){
      Key = key;
      Name = csvReader.ReadString();
      onCsvConstruct();
    }
    partial void onCsvConstruct();


    /// <summary>
    /// New PluralNameNoneStandardClass read from CSV file
    /// </summary>
    internal static PluralNameNoneStandardClass Create(int key, CsvReader csvReader) {
      return new PluralNameNoneStandardClass(key, csvReader);
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Adds PluralNameNoneStandardClass to DC.Data.PluralNameNoneStandardClasses.<br/>
    /// Throws an Exception when PluralNameNoneStandardClass is already stored.
    /// </summary>
    public void Store() {
      if (Key>=0) {
        throw new Exception($"PluralNameNoneStandardClass cannot be stored again in DC.Data, key {Key} is greater equal 0." + Environment.NewLine + ToString());
      }

      var isCancelled = false;
      onStoring(ref isCancelled);
      if (isCancelled) return;

      DC.Data._PluralNameNoneStandardClasses.Add(this);
      onStored();
    }
    partial void onStoring(ref bool isCancelled);
    partial void onStored();


    /// <summary>
    /// Estimated number of UTF8 characters needed to write PluralNameNoneStandardClass to CSV file
    /// </summary>
    public const int EstimatedLineLength = 150;


    /// <summary>
    /// Write PluralNameNoneStandardClass to CSV file
    /// </summary>
    internal static void Write(PluralNameNoneStandardClass pluralNameNoneStandardClass, CsvWriter csvWriter) {
      pluralNameNoneStandardClass.onCsvWrite();
      csvWriter.Write(pluralNameNoneStandardClass.Name);
    }
    partial void onCsvWrite();


    /// <summary>
    /// Updates PluralNameNoneStandardClass with the provided values
    /// </summary>
    public void Update(string name) {
      var clone = new PluralNameNoneStandardClass(this);
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
          DC.Data._PluralNameNoneStandardClasses.ItemHasChanged(clone, this);
        } else if (DC.Data.IsTransaction) {
          DC.Data.AddTransaction(new TransactionItem(4, TransactionActivityEnum.Update, Key, this, oldItem: clone));
        }
        HasChanged?.Invoke(clone, this);
      }
    }
    partial void onUpdating(string name, ref bool isCancelled);
    partial void onUpdated(PluralNameNoneStandardClass old);


    /// <summary>
    /// Updates this PluralNameNoneStandardClass with values from CSV file
    /// </summary>
    internal static void Update(PluralNameNoneStandardClass pluralNameNoneStandardClass, CsvReader csvReader){
      pluralNameNoneStandardClass.Name = csvReader.ReadString();
      pluralNameNoneStandardClass.onCsvUpdate();
    }
    partial void onCsvUpdate();


    /// <summary>
    /// Removes PluralNameNoneStandardClass from DC.Data.PluralNameNoneStandardClasses.
    /// </summary>
    public void Release() {
      if (Key<0) {
        throw new Exception($"PluralNameNoneStandardClass.Release(): PluralNameNoneStandardClass '{this}' is not stored in DC.Data, key is {Key}.");
      }
      DC.Data._PluralNameNoneStandardClasses.Remove(Key);
      onReleased();
    }
    partial void onReleased();


    /// <summary>
    /// Undoes the new() statement as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemNew(IStorageItem item) {
      var pluralNameNoneStandardClass = (PluralNameNoneStandardClass) item;
      pluralNameNoneStandardClass.onRollbackItemNew();
    }
    partial void onRollbackItemNew();


    /// <summary>
    /// Releases PluralNameNoneStandardClass from DC.Data.PluralNameNoneStandardClasses as part of a transaction rollback of Store().
    /// </summary>
    internal static void RollbackItemStore(IStorageItem item) {
      var pluralNameNoneStandardClass = (PluralNameNoneStandardClass) item;
      pluralNameNoneStandardClass.onRollbackItemStored();
    }
    partial void onRollbackItemStored();


    /// <summary>
    /// Restores the PluralNameNoneStandardClass item data as it was before the last update as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemUpdate(IStorageItem oldStorageItem, IStorageItem newStorageItem) {
      var oldItem = (PluralNameNoneStandardClass) oldStorageItem;//an item clone with the values before item was updated
      var item = (PluralNameNoneStandardClass) newStorageItem;//is the instance whose values should be restored

      // updated item: restore old values
      item.Name = oldItem.Name;
      item.onRollbackItemUpdated(oldItem);
    }
    partial void onRollbackItemUpdated(PluralNameNoneStandardClass oldPluralNameNoneStandardClass);


    /// <summary>
    /// Adds PluralNameNoneStandardClass to DC.Data.PluralNameNoneStandardClasses as part of a transaction rollback of Release().
    /// </summary>
    internal static void RollbackItemRelease(IStorageItem item) {
      var pluralNameNoneStandardClass = (PluralNameNoneStandardClass) item;
      pluralNameNoneStandardClass.onRollbackItemRelease();
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
