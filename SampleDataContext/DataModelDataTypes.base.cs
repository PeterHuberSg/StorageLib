//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by StorageClassGenerator
//
//     Do not change code in this file, it will get lost when the file gets 
//     auto generated again. Write your code into DataModelDataTypes.cs.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using StorageLib;


namespace DataModelSamples  {


  public partial class DataModelDataTypes: IStorageItemGeneric<DataModelDataTypes> {

    #region Properties
    //      ----------

    /// <summary>
    /// Unique identifier for DataModelDataTypes. Gets set once DataModelDataTypes gets added to DC.Data.
    /// </summary>
    public int Key { get; private set; }
    internal static void SetKey(IStorageItem dataModelDataTypes, int key, bool _) {
      ((DataModelDataTypes)dataModelDataTypes).Key = key;
    }


    /// <summary>
    /// Stores only dates but no times.
    ///  </summary>
    public DateTime Date { get; private set; }


    /// <summary>
    /// Stores less than 24 hours with second precision.
    ///  </summary>
    public TimeSpan Time { get; private set; }


    /// <summary>
    /// Stores date and time with minute preclusion.
    ///  </summary>
    public DateTime DateMinutes { get; private set; }


    /// <summary>
    /// Stores date and time with seconds precision.
    ///  </summary>
    public DateTime DateSeconds { get; private set; }


    /// <summary>
    /// Stores date and time with tick precision.
    ///  </summary>
    public DateTime DateTimeTicks { get; private set; }


    /// <summary>
    /// Stores time duration with tick precision.
    ///  </summary>
    public TimeSpan TimeSpanTicks { get; private set; }


    /// <summary>
    /// Stores date and time with maximum precision.
    ///  </summary>
    public decimal Decimal_ { get; private set; }


    /// <summary>
    /// Stores decimal with 2 digits after comma.
    ///  </summary>
    public decimal Decimal2 { get; private set; }


    /// <summary>
    /// Stores decimal with 4 digits after comma.
    ///  </summary>
    public decimal Decimal4 { get; private set; }


    /// <summary>
    /// Stores decimal with 5 digits after comma.
    ///  </summary>
    public decimal Decimal5 { get; private set; }


    public bool Bool_ { get; private set; }


    public int Int_ { get; private set; }


    public long Long_ { get; private set; }


    public char Char_ { get; private set; }


    public string String_ { get; private set; }


    /// <summary>
    /// Headers written to first line in CSV file
    /// </summary>
    internal static readonly string[] Headers = {
      "Key", 
      "Date", 
      "Time", 
      "DateMinutes", 
      "DateSeconds", 
      "DateTimeTicks", 
      "TimeSpanTicks", 
      "Decimal_", 
      "Decimal2", 
      "Decimal4", 
      "Decimal5", 
      "Bool_", 
      "Int_", 
      "Long_", 
      "Char_", 
      "String_"
    };


    /// <summary>
    /// None existing DataModelDataTypes, used as a temporary place holder when reading a CSV file
    /// which was not compacted. It might create first a later deleted item linking to a 
    /// deleted parent. In this case, the parent property gets set to NoDataModelDataTypes. Once the CSV
    /// file is completely read, that child will actually be deleted (released) and Verify()
    /// ensures that there are no stored children with links to NoDataModelDataTypes.
    /// </summary>
    internal static DataModelDataTypes NoDataModelDataTypes = new DataModelDataTypes(DateTime.MinValue.Date, TimeSpan.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, TimeSpan.MinValue, Decimal.MinValue, Decimal.MinValue, Decimal.MinValue, Decimal.MinValue, false, int.MinValue, long.MinValue, char.MaxValue, "NoString_", isStoring: false);
    #endregion


    #region Events
    //      ------

    /// <summary>
    /// Content of DataModelDataTypes has changed. Gets only raised for changes occurring after loading DC.Data with previously stored data.
    /// </summary>
    public event Action</*old*/DataModelDataTypes, /*new*/DataModelDataTypes>? HasChanged;
    #endregion


    #region Constructors
    //      ------------

    /// <summary>
    /// DataModelDataTypes Constructor. If isStoring is true, adds DataModelDataTypes to DC.Data.DataModelDataTypess.
    /// </summary>
    public DataModelDataTypes(
      DateTime date, 
      TimeSpan time, 
      DateTime dateMinutes, 
      DateTime dateSeconds, 
      DateTime dateTimeTicks, 
      TimeSpan timeSpanTicks, 
      decimal decimal_, 
      decimal decimal2, 
      decimal decimal4, 
      decimal decimal5, 
      bool bool_, 
      int int_, 
      long long_, 
      char char_, 
      string string_, 
      bool isStoring = true)
    {
      Key = StorageExtensions.NoKey;
      Date = date.Floor(Rounding.Days);
      Time = time.Round(Rounding.Seconds);
      DateMinutes = dateMinutes.Round(Rounding.Minutes);
      DateSeconds = dateSeconds.Round(Rounding.Seconds);
      DateTimeTicks = dateTimeTicks;
      TimeSpanTicks = timeSpanTicks;
      Decimal_ = decimal_;
      Decimal2 = decimal2.Round(2);
      Decimal4 = decimal4.Round(4);
      Decimal5 = decimal5.Round(5);
      Bool_ = bool_;
      Int_ = int_;
      Long_ = long_;
      Char_ = char_;
      String_ = string_;
      onConstruct();
      if (DC.Data.IsTransaction) {
        DC.Data.AddTransaction(new TransactionItem(9,TransactionActivityEnum.New, Key, this));
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
    public DataModelDataTypes(DataModelDataTypes original) {
    #pragma warning restore CS8618 //
      Key = StorageExtensions.NoKey;
      Date = original.Date;
      Time = original.Time;
      DateMinutes = original.DateMinutes;
      DateSeconds = original.DateSeconds;
      DateTimeTicks = original.DateTimeTicks;
      TimeSpanTicks = original.TimeSpanTicks;
      Decimal_ = original.Decimal_;
      Decimal2 = original.Decimal2;
      Decimal4 = original.Decimal4;
      Decimal5 = original.Decimal5;
      Bool_ = original.Bool_;
      Int_ = original.Int_;
      Long_ = original.Long_;
      Char_ = original.Char_;
      String_ = original.String_;
      onCloned(this);
    }
    partial void onCloned(DataModelDataTypes clone);


    /// <summary>
    /// Constructor for DataModelDataTypes read from CSV file
    /// </summary>
    private DataModelDataTypes(int key, CsvReader csvReader){
      Key = key;
      Date = csvReader.ReadDate();
      Time = csvReader.ReadTime();
      DateMinutes = csvReader.ReadDateSeconds();
      DateSeconds = csvReader.ReadDateSeconds();
      DateTimeTicks = csvReader.ReadDateTimeTicks();
      TimeSpanTicks = csvReader.ReadTimeSpanTicks();
      Decimal_ = csvReader.ReadDecimal();
      Decimal2 = csvReader.ReadDecimal();
      Decimal4 = csvReader.ReadDecimal();
      Decimal5 = csvReader.ReadDecimal();
      Bool_ = csvReader.ReadBool();
      Int_ = csvReader.ReadInt();
      Long_ = csvReader.ReadLong();
      Char_ = csvReader.ReadChar();
      String_ = csvReader.ReadString();
      onCsvConstruct();
    }
    partial void onCsvConstruct();


    /// <summary>
    /// New DataModelDataTypes read from CSV file
    /// </summary>
    internal static DataModelDataTypes Create(int key, CsvReader csvReader) {
      return new DataModelDataTypes(key, csvReader);
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Adds DataModelDataTypes to DC.Data.DataModelDataTypess.<br/>
    /// Throws an Exception when DataModelDataTypes is already stored.
    /// </summary>
    public void Store() {
      if (Key>=0) {
        throw new Exception($"DataModelDataTypes cannot be stored again in DC.Data, key {Key} is greater equal 0." + Environment.NewLine + ToString());
      }

      var isCancelled = false;
      onStoring(ref isCancelled);
      if (isCancelled) return;

      DC.Data._DataModelDataTypess.Add(this);
      onStored();
    }
    partial void onStoring(ref bool isCancelled);
    partial void onStored();


    /// <summary>
    /// Estimated number of UTF8 characters needed to write DataModelDataTypes to CSV file
    /// </summary>
    public const int EstimatedLineLength = 348;


    /// <summary>
    /// Write DataModelDataTypes to CSV file
    /// </summary>
    internal static void Write(DataModelDataTypes dataModelDataTypes, CsvWriter csvWriter) {
      dataModelDataTypes.onCsvWrite();
      csvWriter.WriteDate(dataModelDataTypes.Date);
      csvWriter.WriteTime(dataModelDataTypes.Time);
      csvWriter.WriteDateMinutes(dataModelDataTypes.DateMinutes);
      csvWriter.WriteDateSeconds(dataModelDataTypes.DateSeconds);
      csvWriter.WriteDateTimeTicks(dataModelDataTypes.DateTimeTicks);
      csvWriter.WriteTimeSpanTicks(dataModelDataTypes.TimeSpanTicks);
      csvWriter.Write(dataModelDataTypes.Decimal_);
      csvWriter.WriteDecimal2(dataModelDataTypes.Decimal2);
      csvWriter.WriteDecimal4(dataModelDataTypes.Decimal4);
      csvWriter.WriteDecimal5(dataModelDataTypes.Decimal5);
      csvWriter.Write(dataModelDataTypes.Bool_);
      csvWriter.Write(dataModelDataTypes.Int_);
      csvWriter.Write(dataModelDataTypes.Long_);
      csvWriter.Write(dataModelDataTypes.Char_);
      csvWriter.Write(dataModelDataTypes.String_);
    }
    partial void onCsvWrite();


    /// <summary>
    /// Updates DataModelDataTypes with the provided values
    /// </summary>
    public void Update(
      DateTime date, 
      TimeSpan time, 
      DateTime dateMinutes, 
      DateTime dateSeconds, 
      DateTime dateTimeTicks, 
      TimeSpan timeSpanTicks, 
      decimal decimal_, 
      decimal decimal2, 
      decimal decimal4, 
      decimal decimal5, 
      bool bool_, 
      int int_, 
      long long_, 
      char char_, 
      string string_)
    {
      var clone = new DataModelDataTypes(this);
      var isCancelled = false;
      onUpdating(
        date, 
        time, 
        dateMinutes, 
        dateSeconds, 
        dateTimeTicks, 
        timeSpanTicks, 
        decimal_, 
        decimal2, 
        decimal4, 
        decimal5, 
        bool_, 
        int_, 
        long_, 
        char_, 
        string_, 
        ref isCancelled);
      if (isCancelled) return;


      //update properties and detect if any value has changed
      var isChangeDetected = false;
      var dateRounded = date.Floor(Rounding.Days);
      if (Date!=dateRounded) {
        Date = dateRounded;
        isChangeDetected = true;
      }
      var timeRounded = time.Round(Rounding.Seconds);
      if (Time!=timeRounded) {
        Time = timeRounded;
        isChangeDetected = true;
      }
      var dateMinutesRounded = dateMinutes.Round(Rounding.Minutes);
      if (DateMinutes!=dateMinutesRounded) {
        DateMinutes = dateMinutesRounded;
        isChangeDetected = true;
      }
      var dateSecondsRounded = dateSeconds.Round(Rounding.Seconds);
      if (DateSeconds!=dateSecondsRounded) {
        DateSeconds = dateSecondsRounded;
        isChangeDetected = true;
      }
      if (DateTimeTicks!=dateTimeTicks) {
        DateTimeTicks = dateTimeTicks;
        isChangeDetected = true;
      }
      if (TimeSpanTicks!=timeSpanTicks) {
        TimeSpanTicks = timeSpanTicks;
        isChangeDetected = true;
      }
      if (Decimal_!=decimal_) {
        Decimal_ = decimal_;
        isChangeDetected = true;
      }
      var decimal2Rounded = decimal2.Round(2);
      if (Decimal2!=decimal2Rounded) {
        Decimal2 = decimal2Rounded;
        isChangeDetected = true;
      }
      var decimal4Rounded = decimal4.Round(4);
      if (Decimal4!=decimal4Rounded) {
        Decimal4 = decimal4Rounded;
        isChangeDetected = true;
      }
      var decimal5Rounded = decimal5.Round(5);
      if (Decimal5!=decimal5Rounded) {
        Decimal5 = decimal5Rounded;
        isChangeDetected = true;
      }
      if (Bool_!=bool_) {
        Bool_ = bool_;
        isChangeDetected = true;
      }
      if (Int_!=int_) {
        Int_ = int_;
        isChangeDetected = true;
      }
      if (Long_!=long_) {
        Long_ = long_;
        isChangeDetected = true;
      }
      if (Char_!=char_) {
        Char_ = char_;
        isChangeDetected = true;
      }
      if (String_!=string_) {
        String_ = string_;
        isChangeDetected = true;
      }
      if (isChangeDetected) {
        onUpdated(clone);
        if (Key>=0) {
          DC.Data._DataModelDataTypess.ItemHasChanged(clone, this);
        } else if (DC.Data.IsTransaction) {
          DC.Data.AddTransaction(new TransactionItem(9, TransactionActivityEnum.Update, Key, this, oldItem: clone));
        }
        HasChanged?.Invoke(clone, this);
      }
    }
    partial void onUpdating(
      DateTime date, 
      TimeSpan time, 
      DateTime dateMinutes, 
      DateTime dateSeconds, 
      DateTime dateTimeTicks, 
      TimeSpan timeSpanTicks, 
      decimal decimal_, 
      decimal decimal2, 
      decimal decimal4, 
      decimal decimal5, 
      bool bool_, 
      int int_, 
      long long_, 
      char char_, 
      string string_, 
      ref bool isCancelled);
    partial void onUpdated(DataModelDataTypes old);


    /// <summary>
    /// Updates this DataModelDataTypes with values from CSV file
    /// </summary>
    internal static void Update(DataModelDataTypes dataModelDataTypes, CsvReader csvReader){
      dataModelDataTypes.Date = csvReader.ReadDate();
      dataModelDataTypes.Time = csvReader.ReadTime();
      dataModelDataTypes.DateMinutes = csvReader.ReadDateSeconds();
      dataModelDataTypes.DateSeconds = csvReader.ReadDateSeconds();
      dataModelDataTypes.DateTimeTicks = csvReader.ReadDateTimeTicks();
      dataModelDataTypes.TimeSpanTicks = csvReader.ReadTimeSpanTicks();
      dataModelDataTypes.Decimal_ = csvReader.ReadDecimal();
      dataModelDataTypes.Decimal2 = csvReader.ReadDecimal();
      dataModelDataTypes.Decimal4 = csvReader.ReadDecimal();
      dataModelDataTypes.Decimal5 = csvReader.ReadDecimal();
      dataModelDataTypes.Bool_ = csvReader.ReadBool();
      dataModelDataTypes.Int_ = csvReader.ReadInt();
      dataModelDataTypes.Long_ = csvReader.ReadLong();
      dataModelDataTypes.Char_ = csvReader.ReadChar();
      dataModelDataTypes.String_ = csvReader.ReadString();
      dataModelDataTypes.onCsvUpdate();
    }
    partial void onCsvUpdate();


    /// <summary>
    /// Removes DataModelDataTypes from DC.Data.DataModelDataTypess.
    /// </summary>
    public void Release() {
      if (Key<0) {
        throw new Exception($"DataModelDataTypes.Release(): DataModelDataTypes '{this}' is not stored in DC.Data, key is {Key}.");
      }
      DC.Data._DataModelDataTypess.Remove(Key);
      onReleased();
    }
    partial void onReleased();


    /// <summary>
    /// Undoes the new() statement as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemNew(IStorageItem item) {
      var dataModelDataTypes = (DataModelDataTypes) item;
      dataModelDataTypes.onRollbackItemNew();
    }
    partial void onRollbackItemNew();


    /// <summary>
    /// Releases DataModelDataTypes from DC.Data.DataModelDataTypess as part of a transaction rollback of Store().
    /// </summary>
    internal static void RollbackItemStore(IStorageItem item) {
      var dataModelDataTypes = (DataModelDataTypes) item;
      dataModelDataTypes.onRollbackItemStored();
    }
    partial void onRollbackItemStored();


    /// <summary>
    /// Restores the DataModelDataTypes item data as it was before the last update as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemUpdate(IStorageItem oldStorageItem, IStorageItem newStorageItem) {
      var oldItem = (DataModelDataTypes) oldStorageItem;//an item clone with the values before item was updated
      var item = (DataModelDataTypes) newStorageItem;//is the instance whose values should be restored

      // updated item: restore old values
      item.Date = oldItem.Date;
      item.Time = oldItem.Time;
      item.DateMinutes = oldItem.DateMinutes;
      item.DateSeconds = oldItem.DateSeconds;
      item.DateTimeTicks = oldItem.DateTimeTicks;
      item.TimeSpanTicks = oldItem.TimeSpanTicks;
      item.Decimal_ = oldItem.Decimal_;
      item.Decimal2 = oldItem.Decimal2;
      item.Decimal4 = oldItem.Decimal4;
      item.Decimal5 = oldItem.Decimal5;
      item.Bool_ = oldItem.Bool_;
      item.Int_ = oldItem.Int_;
      item.Long_ = oldItem.Long_;
      item.Char_ = oldItem.Char_;
      item.String_ = oldItem.String_;
      item.onRollbackItemUpdated(oldItem);
    }
    partial void onRollbackItemUpdated(DataModelDataTypes oldDataModelDataTypes);


    /// <summary>
    /// Adds DataModelDataTypes to DC.Data.DataModelDataTypess as part of a transaction rollback of Release().
    /// </summary>
    internal static void RollbackItemRelease(IStorageItem item) {
      var dataModelDataTypes = (DataModelDataTypes) item;
      dataModelDataTypes.onRollbackItemRelease();
    }
    partial void onRollbackItemRelease();


    /// <summary>
    /// Returns property values for tracing. Parents are shown with their key instead their content.
    /// </summary>
    public string ToTraceString() {
      var returnString =
        $"{this.GetKeyOrHash()}|" +
        $" {Date.ToShortDateString()}|" +
        $" {Time}|" +
        $" {DateMinutes}|" +
        $" {DateSeconds}|" +
        $" {DateTimeTicks}|" +
        $" {TimeSpanTicks}|" +
        $" {Decimal_}|" +
        $" {Decimal2}|" +
        $" {Decimal4}|" +
        $" {Decimal5}|" +
        $" {Bool_}|" +
        $" {Int_}|" +
        $" {Long_}|" +
        $" {Char_}|" +
        $" {String_}";
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
        $" {Time}," +
        $" {DateMinutes}," +
        $" {DateSeconds}," +
        $" {DateTimeTicks}," +
        $" {TimeSpanTicks}," +
        $" {Decimal_}," +
        $" {Decimal2}," +
        $" {Decimal4}," +
        $" {Decimal5}," +
        $" {Bool_}," +
        $" {Int_}," +
        $" {Long_}," +
        $" {Char_}," +
        $" {String_}";
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
        $" Time: {Time}," +
        $" DateMinutes: {DateMinutes}," +
        $" DateSeconds: {DateSeconds}," +
        $" DateTimeTicks: {DateTimeTicks}," +
        $" TimeSpanTicks: {TimeSpanTicks}," +
        $" Decimal_: {Decimal_}," +
        $" Decimal2: {Decimal2}," +
        $" Decimal4: {Decimal4}," +
        $" Decimal5: {Decimal5}," +
        $" Bool_: {Bool_}," +
        $" Int_: {Int_}," +
        $" Long_: {Long_}," +
        $" Char_: {Char_}," +
        $" String_: {String_};";
      onToString(ref returnString);
      return returnString;
    }
    partial void onToString(ref string returnString);
    #endregion
  }
}
