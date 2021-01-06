//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by StorageClassGenerator
//
//     Do not change code in this file, it will get lost when the file gets 
//     auto generated again. Write your code into ParentWithChildDictionary.cs.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using StorageLib;


namespace DataModelSamples  {


  public partial class ParentWithChildDictionary: IStorageItemGeneric<ParentWithChildDictionary> {

    #region Properties
    //      ----------

    /// <summary>
    /// Unique identifier for ParentWithChildDictionary. Gets set once ParentWithChildDictionary gets added to DC.Data.
    /// </summary>
    public int Key { get; private set; }
    internal static void SetKey(IStorageItem parentWithChildDictionary, int key, bool _) {
      ((ParentWithChildDictionary)parentWithChildDictionary).Key = key;
    }


    public IReadOnlyDictionary<DateTime, ChildOfParentWithDictionary> Children => children;
    readonly Dictionary<DateTime, ChildOfParentWithDictionary> children;


    /// <summary>
    /// Headers written to first line in CSV file
    /// </summary>
    internal static readonly string[] Headers = {"Key"};


    /// <summary>
    /// None existing ParentWithChildDictionary
    /// </summary>
    internal static ParentWithChildDictionary NoParentWithChildDictionary = new ParentWithChildDictionary(isStoring: false);
    #endregion


    #region Events
    //      ------

    /// <summary>
    /// Content of ParentWithChildDictionary has changed. Gets only raised for changes occurring after loading DC.Data with previously stored data.
    /// </summary>
    public event Action</*old*/ParentWithChildDictionary, /*new*/ParentWithChildDictionary>? HasChanged;
    #endregion


    #region Constructors
    //      ------------

    /// <summary>
    /// ParentWithChildDictionary Constructor. If isStoring is true, adds ParentWithChildDictionary to DC.Data.ParentWithChildDictionarys.
    /// </summary>
    public ParentWithChildDictionary(bool isStoring = true) {
      Key = StorageExtensions.NoKey;
      children = new Dictionary<DateTime, ChildOfParentWithDictionary>();
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
    public ParentWithChildDictionary(ParentWithChildDictionary original) {
    #pragma warning restore CS8618 //
      Key = StorageExtensions.NoKey;
      onCloned(this);
    }
    partial void onCloned(ParentWithChildDictionary clone);


    /// <summary>
    /// Constructor for ParentWithChildDictionary read from CSV file
    /// </summary>
    private ParentWithChildDictionary(int key, CsvReader csvReader){
      Key = key;
      children = new Dictionary<DateTime, ChildOfParentWithDictionary>();
      onCsvConstruct();
    }
    partial void onCsvConstruct();


    /// <summary>
    /// New ParentWithChildDictionary read from CSV file
    /// </summary>
    internal static ParentWithChildDictionary Create(int key, CsvReader csvReader) {
      return new ParentWithChildDictionary(key, csvReader);
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Adds ParentWithChildDictionary to DC.Data.ParentWithChildDictionarys.<br/>
    /// Throws an Exception when ParentWithChildDictionary is already stored.
    /// </summary>
    public void Store() {
      if (Key>=0) {
        throw new Exception($"ParentWithChildDictionary cannot be stored again in DC.Data, key {Key} is greater equal 0." + Environment.NewLine + ToString());
      }

      var isCancelled = false;
      onStoring(ref isCancelled);
      if (isCancelled) return;

      DC.Data._ParentWithChildDictionarys.Add(this);
      onStored();
    }
    partial void onStoring(ref bool isCancelled);
    partial void onStored();


    /// <summary>
    /// Estimated number of UTF8 characters needed to write ParentWithChildDictionary to CSV file
    /// </summary>
    public const int EstimatedLineLength = 13;


    /// <summary>
    /// Write ParentWithChildDictionary to CSV file
    /// </summary>
    internal static void Write(ParentWithChildDictionary parentWithChildDictionary, CsvWriter csvWriter) {
      parentWithChildDictionary.onCsvWrite();
    }
    partial void onCsvWrite();


    /// <summary>
    /// Updates ParentWithChildDictionary with the provided values
    /// </summary>
    public void Update(