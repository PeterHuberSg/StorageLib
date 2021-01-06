//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by StorageClassGenerator
//
//     Do not change code in this file, it will get lost when the file gets 
//     auto generated again. Write your code into DefaultValuePropertyClass.cs.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using StorageLib;


namespace DataModelSamples  {


  public partial class DefaultValuePropertyClass: IStorageItemGeneric<DefaultValuePropertyClass> {

    #region Properties
    //      ----------

    /// <summary>
    /// Unique identifier for DefaultValuePropertyClass. Gets set once DefaultValuePropertyClass gets added to DC.Data.
    /// </summary>
    public int Key { get; private set; }
    internal static void SetKey(IStorageItem defaultValuePropertyClass, int key, bool _) {
      ((DefaultValuePropertyClass)defaultValuePropertyClass).Key = key;
    }


    public string Name { get; private set; }


    public string DefaultValueProperty { get; private set; }


    /// <summary>
    /// Headers written to first line in CSV file
    /// </summary>
    internal static readonly string[] Headers = {"Key", "Name", "DefaultValueProperty"};


    /// <summary>
    /// None existing DefaultValuePropertyClass
    /// </summary>
    internal static DefaultValuePropertyClass NoDefaultValuePropertyClass = new DefaultValuePropertyClass("NoName", "NoDefaultValueProperty", isStoring: false);
    #endregion


    #region Events
    //      ------

    /// <summary>
    /// Content of DefaultValuePropertyClass has changed. Gets only raised for changes occurring after loading DC.Data with previously stored data.
    /// </summary>
    public event Action</*old*/DefaultValuePropertyClass, /*new*/DefaultValuePropertyClass>? HasChanged;
    #endregion


    #region Constructors
    //      ------------

    /// <summary>
    /// DefaultValuePropertyClass Constructor. If isStoring is true, adds DefaultValuePropertyClass to DC.Data.DefaultValuePropertyClasss.
    /// </summary>
    public DefaultValuePropertyClass(string name, string defaultValueProperty = "NoName", bool isStoring = true) {
      Key = StorageExtensions.NoKey;
      Name = name;
      DefaultValueProperty = defaultValueProperty;
      onConstruct();
      if (DC.Data.IsTransaction) {
        DC.Data.AddTransaction(new TransactionItem(7,TransactionActivityEnum.New, Key, this));
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
    public DefaultValuePropertyClass(DefaultValuePropertyClass original) {
    #pragma warning restore CS8618 //
      Key = StorageExtensions.NoKey;
      Name = original.Name;
      DefaultValueProperty = original.DefaultValueProperty;
      onCloned(this);
    }
    partial void onCloned(DefaultValuePropertyClass clone);


    /// <summary>
    /// Constructor for DefaultValuePropertyClass read from CSV file
    /// </summary>
    private DefaultValuePropertyClass(int key, CsvReader csvReader){
      Key = key;
      Name = csvReader.ReadString();
      DefaultValueProperty = csvReader.ReadString();
      onCsvConstruct();
    }
    partial void onCsvConstruct();


    /// <summary>
    /// New DefaultValuePropertyClass read from CSV file
    /// </summary>
    internal static DefaultValuePropertyClass Create(int key, CsvReader csvReader) {
      return new DefaultValuePropertyClass(key, csvReader);
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Adds DefaultValuePropertyClass to DC.Data.DefaultValuePropertyClasss.<br/>
    /// Throws an Exception when DefaultValuePropertyClass is already stored.
    /// </summary>
    public void Store() {
      if (Key>=0) {
        throw new Exception($"DefaultValuePropertyClass cannot be stored again in DC.Data, key {Key} is greater equal 0." + Environment.NewLine + ToString());
      }

      var isCancelled = false;
      onStoring(ref isCancelled);
      if (isCancelled) return;

      DC.Data._DefaultValuePropertyClasss.Add(this);
      onStored();
    }
    partial void onStoring(ref bool isCancelled);
    partial void onStored();


    /// <summary>
    /// Estimated number of UTF8 characters needed to write DefaultValuePropertyClass to CSV file
    /// </summary>
    public const int EstimatedLineLength = 300;


    /// <summary>
    /// Write DefaultValuePropertyClass to CSV file
    /// </summary>
    internal static void Write(DefaultValuePropertyClass defaultValuePropertyClass, CsvWriter csvWriter) {
      defaultValuePropertyClass.onCsvWrite();
      csvWriter.Write(defaultValuePropertyClass.Name);
      csvWriter.Write(defaultValuePropertyClass.DefaultValueProperty);
    }
    partial void onCsvWrite();


    /// <summary>
    /// Updates DefaultValuePropertyClass with the provided values
    /// </summary>
    public void Update(string name, string defaultValueProperty) {
      var clone = new DefaultValuePropertyClass(this);
      var isCancelled = false;
      onUpdating(name, defaultValueProperty, ref isCancelled);
      if (isCancelled) return;


      //update properties and detect if any value has changed
      var isChangeDetected = false;
      if (Name!=name) {
        Name = name;
        isChangeDetected = true;
      }
      if (DefaultValueProperty!=defaultValueProperty) {
        DefaultValueProperty = defaultValueProperty;
        isChangeDetected = true;
      }
      if (isChangeDetected) {
        onUpdated(clone);
        if (Key>=0) {
          DC.Data._DefaultValuePropertyClasss.ItemHasChanged(clone, this);
        } else if (DC.Data.IsTransaction) {
          DC.Data.AddTransaction(new TransactionItem(7, TransactionActivityEnum.Update, Key, this, oldItem: clone));
        }
        HasChanged?.Invoke(clone, this);
      }
    }
    partial void onUpdating(string name, string defaultValueProperty, ref bool isCancelled);
    partial void onUpdated(DefaultValuePropertyClass old);


    /// <summary>
    /// Updates this DefaultValuePropertyClass with values from CSV file
    /// </summary>
    internal static void Update(DefaultValuePropertyClass defaultValuePropertyClass, CsvReader csvReader){
      defaultValuePropertyClass.Name = csvReader.ReadString();
      defaultValuePropertyClass.DefaultValueProperty = csvReader.ReadString();
      defaultValuePropertyClass.onCsvUpdate();
    }
    partial void onCsvUpdate();


    /// <summary>
    /// Removes DefaultValuePropertyClass from DC.Data.DefaultValuePropertyClasss.
    /// </summary>
    public void Release() {
      if (Key<0) {
        throw new Exception($"DefaultValuePropertyClass.Release(): DefaultValuePropertyClass '{this}' is not stored in DC.Data, key is {Key}.");
      }
      onReleased();
      DC.Data._DefaultValuePropertyClasss.Remove(Key);
    }
    partial void onReleased();


    /// <summary>
    /// Undoes the new() statement as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemNew(IStorageItem item) {
      var defaultValuePropertyClass = (DefaultValuePropertyClass) item;
      defaultValuePropertyClass.onRollbackItemNew();
    }
    partial void onRollbackItemNew();


    /// <summary>
    /// Releases DefaultValuePropertyClass from DC.Data.DefaultValuePropertyClasss as part of a transaction rollback of Store().
    /// </summary>
    internal static void RollbackItemStore(IStorageItem item) {
      var defaultValuePropertyClass = (DefaultValuePropertyClass) item;
      defaultValuePropertyClass.onRollbackItemStored();
    }
    partial void onRollbackItemStored();


    /// <summary>
    /// Restores the DefaultValuePropertyClass item data as it was before the last update as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemUpdate(IStorageItem oldStorageItem, IStorageItem newStorageItem) {
      var oldItem = (DefaultValuePropertyClass) oldStorageItem;//an item clone with the values before item was updated
      var item = (DefaultValuePropertyClass) newStorageItem;//is the instance whose values should be restored

      // updated item: restore old values
      item.Name = oldItem.Name;
      item.DefaultValueProperty = oldItem.DefaultValueProperty;
      item.onRollbackItemUpdated(oldItem);
    }
    partial void onRollbackItemUpdated(DefaultValuePropertyClass oldDefaultValuePropertyClass);


    /// <summary>
    /// Adds DefaultValuePropertyClass to DC.Data.DefaultValuePropertyClasss as part of a transaction rollback of Release().
    /// </summary>
    internal static void RollbackItemRelease(IStorageItem item) {
      var defaultValuePropertyClass = (DefaultValuePropertyClass) item;
      defaultValuePropertyClass.onRollbackItemRelease();
    }
    partial void onRollbackItemRelease();


    /// <summary>
    /// Returns property values for tracing. Parents are shown with their key instead their content.
    /// </summary>
    public string ToTraceString() {
      var returnString =
        $"{this.GetKeyOrHash()}|" +
        $" {Name}|" +
        $" {DefaultValueProperty}";
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
        $" {DefaultValueProperty}";
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
        $" DefaultValueProperty: {DefaultValueProperty};";
      onToString(ref returnString);
      return returnString;
    }
    partial void onToString(ref string returnString);
    #endregion
  }
}
