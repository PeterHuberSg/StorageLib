//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by StorageClassGenerator
//
//     Do not change code in this file, it will get lost when the file gets 
//     auto generated again. Write your code into Lookup_1_0_Parent.cs.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using StorageLib;


namespace DataModelSamples  {


  public partial class Lookup_1_0_Parent: IStorageItemGeneric<Lookup_1_0_Parent> {

    #region Properties
    //      ----------

    /// <summary>
    /// Unique identifier for Lookup_1_0_Parent. Gets set once Lookup_1_0_Parent gets added to DC.Data.
    /// </summary>
    public int Key { get; private set; }
    internal static void SetKey(IStorageItem lookup_1_0_Parent, int key, bool _) {
      ((Lookup_1_0_Parent)lookup_1_0_Parent).Key = key;
    }


    public string Name { get; private set; }


    /// <summary>
    /// Headers written to first line in CSV file
    /// </summary>
    internal static readonly string[] Headers = {"Key", "Name"};


    /// <summary>
    /// None existing Lookup_1_0_Parent
    /// </summary>
    internal static Lookup_1_0_Parent NoLookup_1_0_Parent = new Lookup_1_0_Parent("NoName", isStoring: false);
    #endregion


    #region Events
    //      ------

    /// <summary>
    /// Content of Lookup_1_0_Parent has changed. Gets only raised for changes occurring after loading DC.Data with previously stored data.
    /// </summary>
    public event Action</*old*/Lookup_1_0_Parent, /*new*/Lookup_1_0_Parent>? HasChanged;
    #endregion


    #region Constructors
    //      ------------

    /// <summary>
    /// Lookup_1_0_Parent Constructor. If isStoring is true, adds Lookup_1_0_Parent to DC.Data.Lookup_1_0_Parents.
    /// </summary>
    public Lookup_1_0_Parent(string name, bool isStoring = true) {
      Key = StorageExtensions.NoKey;
      Name = name;
      onConstruct();
      if (DC.Data.IsTransaction) {
        DC.Data.AddTransaction(new TransactionItem(11,TransactionActivityEnum.New, Key, this));
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
    public Lookup_1_0_Parent(Lookup_1_0_Parent original) {
    #pragma warning restore CS8618 //
      Key = StorageExtensions.NoKey;
      Name = original.Name;
      onCloned(this);
    }
    partial void onCloned(Lookup_1_0_Parent clone);


    /// <summary>
    /// Constructor for Lookup_1_0_Parent read from CSV file
    /// </summary>
    private Lookup_1_0_Parent(int key, CsvReader csvReader){
      Key = key;
      Name = csvReader.ReadString();
      onCsvConstruct();
    }
    partial void onCsvConstruct();


    /// <summary>
    /// New Lookup_1_0_Parent read from CSV file
    /// </summary>
    internal static Lookup_1_0_Parent Create(int key, CsvReader csvReader) {
      return new Lookup_1_0_Parent(key, csvReader);
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Adds Lookup_1_0_Parent to DC.Data.Lookup_1_0_Parents.<br/>
    /// Throws an Exception when Lookup_1_0_Parent is already stored.
    /// </summary>
    public void Store() {
      if (Key>=0) {
        throw new Exception($"Lookup_1_0_Parent cannot be stored again in DC.Data, key {Key} is greater equal 0." + Environment.NewLine + ToString());
      }

      var isCancelled = false;
      onStoring(ref isCancelled);
      if (isCancelled) return;

      DC.Data._Lookup_1_0_Parents.Add(this);
      onStored();
    }
    partial void onStoring(ref bool isCancelled);
    partial void onStored();


    /// <summary>
    /// Estimated number of UTF8 characters needed to write Lookup_1_0_Parent to CSV file
    /// </summary>
    public const int EstimatedLineLength = 150;


    /// <summary>
    /// Write Lookup_1_0_Parent to CSV file
    /// </summary>
    internal static void Write(Lookup_1_0_Parent lookup_1_0_Parent, CsvWriter csvWriter) {
      lookup_1_0_Parent.onCsvWrite();
      csvWriter.Write(lookup_1_0_Parent.Name);
    }
    partial void onCsvWrite();


    /// <summary>
    /// Updates Lookup_1_0_Parent with the provided values
    /// </summary>
    public void Update(string name) {
      var clone = new Lookup_1_0_Parent(this);
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
          DC.Data._Lookup_1_0_Parents.ItemHasChanged(clone, this);
        } else if (DC.Data.IsTransaction) {
          DC.Data.AddTransaction(new TransactionItem(11, TransactionActivityEnum.Update, Key, this, oldItem: clone));
        }
        HasChanged?.Invoke(clone, this);
      }
    }
    partial void onUpdating(string name, ref bool isCancelled);
    partial void onUpdated(Lookup_1_0_Parent old);


    /// <summary>
    /// Updates this Lookup_1_0_Parent with values from CSV file
    /// </summary>
    internal static void Update(Lookup_1_0_Parent lookup_1_0_Parent, CsvReader csvReader){
      lookup_1_0_Parent.Name = csvReader.ReadString();
      lookup_1_0_Parent.onCsvUpdate();
    }
    partial void onCsvUpdate();


    /// <summary>
    /// Releasing Lookup_1_0_Parent from DC.Data.Lookup_1_0_Parents is not supported.
    /// </summary>
    public void Release() {
      throw new NotSupportedException("Release() is not supported, StorageClass attribute AreInstancesReleasable is false.");
    }


    /// <summary>
    /// Undoes the new() statement as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemNew(IStorageItem item) {
      var lookup_1_0_Parent = (Lookup_1_0_Parent) item;
      lookup_1_0_Parent.onRollbackItemNew();
    }
    partial void onRollbackItemNew();


    /// <summary>
    /// Releases Lookup_1_0_Parent from DC.Data.Lookup_1_0_Parents as part of a transaction rollback of Store().
    /// </summary>
    internal static void RollbackItemStore(IStorageItem item) {
      var lookup_1_0_Parent = (Lookup_1_0_Parent) item;
      lookup_1_0_Parent.onRollbackItemStored();
    }
    partial void onRollbackItemStored();


    /// <summary>
    /// Restores the Lookup_1_0_Parent item data as it was before the last update as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemUpdate(IStorageItem oldStorageItem, IStorageItem newStorageItem) {
      var oldItem = (Lookup_1_0_Parent) oldStorageItem;//an item clone with the values before item was updated
      var item = (Lookup_1_0_Parent) newStorageItem;//is the instance whose values should be restored

      // updated item: restore old values
      item.Name = oldItem.Name;
      item.onRollbackItemUpdated(oldItem);
    }
    partial void onRollbackItemUpdated(Lookup_1_0_Parent oldLookup_1_0_Parent);


    /// <summary>
    /// Adds Lookup_1_0_Parent to DC.Data.Lookup_1_0_Parents as part of a transaction rollback of Release().
    /// </summary>
    internal static void RollbackItemRelease(IStorageItem item) {
      var lookup_1_0_Parent = (Lookup_1_0_Parent) item;
      lookup_1_0_Parent.onRollbackItemRelease();
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
