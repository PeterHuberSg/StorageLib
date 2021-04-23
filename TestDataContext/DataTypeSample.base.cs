//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by StorageClassGenerator
//
//     Do not change code in this file, it will get lost when the file gets 
//     auto generated again. Write your code into DataTypeSample.cs.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using StorageLib;


namespace TestContext  {


    /// <summary>
    /// Class having every possible data type used for a property
    /// </summary>
  public partial class DataTypeSample: IStorageItem<DataTypeSample> {

    #region Properties
    //      ----------

    /// <summary>
    /// Unique identifier for DataTypeSample. Gets set once DataTypeSample gets added to DC.Data.
    /// </summary>
    public int Key { get; private set; }
    internal static void SetKey(IStorageItem dataTypeSample, int key, bool isRollback) {
#if DEBUG
      if (isRollback) {
        if (key==StorageExtensions.NoKey) {
          DC.Trace?.Invoke($"Release DataTypeSample key @{dataTypeSample.Key} #{dataTypeSample.GetHashCode()}");
        } else {
          DC.Trace?.Invoke($"Store DataTypeSample key @{key} #{dataTypeSample.GetHashCode()}");
        }
      }
#endif
      ((DataTypeSample)dataTypeSample).Key = key;
    }


    /// <summary>
    /// Stores only dates but no times.
    ///  </summary>
    public DateTime ADate { get; private set; }


    /// <summary>
    /// Stores only dates but no times.
    ///  </summary>
    public DateTime? ANullableDate { get; private set; }


    /// <summary>
    /// Stores less than 24 hours with second precision.
    ///  </summary>
    public TimeSpan ATime { get; private set; }


    /// <summary>
    /// Stores less than 24 hours with second precision.
    ///  </summary>
    public TimeSpan? ANullableTime { get; private set; }


    /// <summary>
    /// Stores date and time with minute preclusion.
    ///  </summary>
    public DateTime ADateMinutes { get; private set; }


    /// <summary>
    /// Stores date and time with minute preclusion.
    ///  </summary>
    public DateTime? ANullableDateMinutes { get; private set; }


    /// <summary>
    /// Stores date and time with seconds precision.
    ///  </summary>
    public DateTime ADateSeconds { get; private set; }


    /// <summary>
    /// Stores date and time with seconds precision.
    ///  </summary>
    public DateTime? ANullableDateSeconds { get; private set; }


    /// <summary>
    /// Stores date and time with tick precision.
    ///  </summary>
    public DateTime ADateTime { get; private set; }


    /// <summary>
    /// Stores date and time with tick precision.
    ///  </summary>
    public DateTime? ANullableDateTime { get; private set; }


    /// <summary>
    /// Stores time duration with tick precision.
    ///  </summary>
    public TimeSpan ATimeSpan { get; private set; }


    /// <summary>
    /// Stores time duration with tick precision.
    ///  </summary>
    public TimeSpan? ANullableTimeSpan { get; private set; }


    /// <summary>
    /// Stores date and time with maximum precision.
    ///  </summary>
    public decimal ADecimal { get; private set; }


    /// <summary>
    /// Stores date and time with maximum precision.
    ///  </summary>
    public decimal? ANullableDecimal { get; private set; }


    /// <summary>
    /// Stores decimal with 2 digits after comma.
    ///  </summary>
    public decimal ADecimal2 { get; private set; }


    /// <summary>
    /// Stores decimal with 2 digits after comma.
    ///  </summary>
    public decimal? ANullableDecimal2 { get; private set; }


    /// <summary>
    /// Stores decimal with 4 digits after comma.
    ///  </summary>
    public decimal ADecimal4 { get; private set; }


    /// <summary>
    /// Stores decimal with 4 digits after comma.
    ///  </summary>
    public decimal? ANullableDecimal4 { get; private set; }


    /// <summary>
    /// Stores decimal with 5 digits after comma.
    ///  </summary>
    public decimal ADecimal5 { get; private set; }


    /// <summary>
    /// Stores decimal with 5 digits after comma.
    ///  </summary>
    public decimal? ANullableDecimal5 { get; private set; }


    public bool ABool { get; private set; }


    public bool? ANullableBool { get; private set; }


    public int AInt { get; private set; }


    public int? ANullableInt { get; private set; }


    public long ALong { get; private set; }


    public long? ANullableLong { get; private set; }


    public char AChar { get; private set; }


    public char? ANullableChar { get; private set; }


    public string AString { get; private set; }


    public string? ANullableString { get; private set; }


    public SampleStateEnum AEnum { get; private set; }


    public SampleStateEnum? ANullableEnum { get; private set; }


    /// <summary>
    /// Headers written to first line in CSV file
    /// </summary>
    internal static readonly string[] Headers = {
      "Key", 
      "ADate", 
      "ANullableDate", 
      "ATime", 
      "ANullableTime", 
      "ADateMinutes", 
      "ANullableDateMinutes", 
      "ADateSeconds", 
      "ANullableDateSeconds", 
      "ADateTime", 
      "ANullableDateTime", 
      "ATimeSpan", 
      "ANullableTimeSpan", 
      "ADecimal", 
      "ANullableDecimal", 
      "ADecimal2", 
      "ANullableDecimal2", 
      "ADecimal4", 
      "ANullableDecimal4", 
      "ADecimal5", 
      "ANullableDecimal5", 
      "ABool", 
      "ANullableBool", 
      "AInt", 
      "ANullableInt", 
      "ALong", 
      "ANullableLong", 
      "AChar", 
      "ANullableChar", 
      "AString", 
      "ANullableString", 
      "AEnum", 
      "ANullableEnum"
    };


    /// <summary>
    /// None existing DataTypeSample, used as a temporary place holder when reading a CSV file
    /// which was not compacted. It might create first a later deleted item linking to a 
    /// deleted parent. In this case, the parent property gets set to NoDataTypeSample. Once the CSV
    /// file is completely read, that child will actually be deleted (released) and Verify()
    /// ensures that there are no stored children with links to NoDataTypeSample.
    /// </summary>
    internal static DataTypeSample NoDataTypeSample = new DataTypeSample(DateTime.MinValue.Date, null, TimeSpan.MinValue, null, DateTime.MinValue, null, DateTime.MinValue, null, DateTime.MinValue, null, TimeSpan.MinValue, null, Decimal.MinValue, null, Decimal.MinValue, null, Decimal.MinValue, null, Decimal.MinValue, null, false, null, int.MinValue, null, long.MinValue, null, char.MaxValue, null, "NoAString", null, 0, null, isStoring: false);
    #endregion


    #region Events
    //      ------

    /// <summary>
    /// Content of DataTypeSample has changed. Gets only raised for changes occurring after loading DC.Data with previously stored data.
    /// </summary>
    public event Action</*old*/DataTypeSample, /*new*/DataTypeSample>? HasChanged;
    #endregion


    #region Constructors
    //      ------------

    /// <summary>
    /// DataTypeSample Constructor. If isStoring is true, adds DataTypeSample to DC.Data.DataTypeSamples.
    /// </summary>
    public DataTypeSample(
      DateTime aDate, 
      DateTime? aNullableDate, 
      TimeSpan aTime, 
      TimeSpan? aNullableTime, 
      DateTime aDateMinutes, 
      DateTime? aNullableDateMinutes, 
      DateTime aDateSeconds, 
      DateTime? aNullableDateSeconds, 
      DateTime aDateTime, 
      DateTime? aNullableDateTime, 
      TimeSpan aTimeSpan, 
      TimeSpan? aNullableTimeSpan, 
      decimal aDecimal, 
      decimal? aNullableDecimal, 
      decimal aDecimal2, 
      decimal? aNullableDecimal2, 
      decimal aDecimal4, 
      decimal? aNullableDecimal4, 
      decimal aDecimal5, 
      decimal? aNullableDecimal5, 
      bool aBool, 
      bool? aNullableBool, 
      int aInt, 
      int? aNullableInt, 
      long aLong, 
      long? aNullableLong, 
      char aChar, 
      char? aNullableChar, 
      string aString, 
      string? aNullableString, 
      SampleStateEnum aEnum, 
      SampleStateEnum? aNullableEnum, 
      bool isStoring = true)
    {
      Key = StorageExtensions.NoKey;
      ADate = aDate.Floor(Rounding.Days);
      ANullableDate = aNullableDate.Floor(Rounding.Days);
      ATime = aTime.Round(Rounding.Seconds);
      ANullableTime = aNullableTime.Round(Rounding.Seconds);
      ADateMinutes = aDateMinutes.Round(Rounding.Minutes);
      ANullableDateMinutes = aNullableDateMinutes.Round(Rounding.Minutes);
      ADateSeconds = aDateSeconds.Round(Rounding.Seconds);
      ANullableDateSeconds = aNullableDateSeconds.Round(Rounding.Seconds);
      ADateTime = aDateTime;
      ANullableDateTime = aNullableDateTime;
      ATimeSpan = aTimeSpan;
      ANullableTimeSpan = aNullableTimeSpan;
      ADecimal = aDecimal;
      ANullableDecimal = aNullableDecimal;
      ADecimal2 = aDecimal2.Round(2);
      ANullableDecimal2 = aNullableDecimal2.Round(2);
      ADecimal4 = aDecimal4.Round(4);
      ANullableDecimal4 = aNullableDecimal4.Round(4);
      ADecimal5 = aDecimal5.Round(5);
      ANullableDecimal5 = aNullableDecimal5.Round(5);
      ABool = aBool;
      ANullableBool = aNullableBool;
      AInt = aInt;
      ANullableInt = aNullableInt;
      ALong = aLong;
      ANullableLong = aNullableLong;
      AChar = aChar;
      ANullableChar = aNullableChar;
      AString = aString;
      ANullableString = aNullableString;
      AEnum = aEnum;
      ANullableEnum = aNullableEnum;
#if DEBUG
      DC.Trace?.Invoke($"new DataTypeSample: {ToTraceString()}");
#endif
      onConstruct();
      if (DC.Data.IsTransaction) {
        DC.Data.AddTransaction(new TransactionItem(25,TransactionActivityEnum.New, Key, this));
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
    public DataTypeSample(DataTypeSample original) {
    #pragma warning restore CS8618 //
      Key = StorageExtensions.NoKey;
      ADate = original.ADate;
      ANullableDate = original.ANullableDate;
      ATime = original.ATime;
      ANullableTime = original.ANullableTime;
      ADateMinutes = original.ADateMinutes;
      ANullableDateMinutes = original.ANullableDateMinutes;
      ADateSeconds = original.ADateSeconds;
      ANullableDateSeconds = original.ANullableDateSeconds;
      ADateTime = original.ADateTime;
      ANullableDateTime = original.ANullableDateTime;
      ATimeSpan = original.ATimeSpan;
      ANullableTimeSpan = original.ANullableTimeSpan;
      ADecimal = original.ADecimal;
      ANullableDecimal = original.ANullableDecimal;
      ADecimal2 = original.ADecimal2;
      ANullableDecimal2 = original.ANullableDecimal2;
      ADecimal4 = original.ADecimal4;
      ANullableDecimal4 = original.ANullableDecimal4;
      ADecimal5 = original.ADecimal5;
      ANullableDecimal5 = original.ANullableDecimal5;
      ABool = original.ABool;
      ANullableBool = original.ANullableBool;
      AInt = original.AInt;
      ANullableInt = original.ANullableInt;
      ALong = original.ALong;
      ANullableLong = original.ANullableLong;
      AChar = original.AChar;
      ANullableChar = original.ANullableChar;
      AString = original.AString;
      ANullableString = original.ANullableString;
      AEnum = original.AEnum;
      ANullableEnum = original.ANullableEnum;
      onCloned(this);
    }
    partial void onCloned(DataTypeSample clone);


    /// <summary>
    /// Constructor for DataTypeSample read from CSV file
    /// </summary>
    private DataTypeSample(int key, CsvReader csvReader){
      Key = key;
      ADate = csvReader.ReadDate();
      ANullableDate = csvReader.ReadDateNull();
      ATime = csvReader.ReadTime();
      ANullableTime = csvReader.ReadTimeNull();
      ADateMinutes = csvReader.ReadDateSeconds();
      ANullableDateMinutes = csvReader.ReadDateSecondsNull();
      ADateSeconds = csvReader.ReadDateSeconds();
      ANullableDateSeconds = csvReader.ReadDateSecondsNull();
      ADateTime = csvReader.ReadDateTimeTicks();
      ANullableDateTime = csvReader.ReadDateTimeTicksNull();
      ATimeSpan = csvReader.ReadTimeSpanTicks();
      ANullableTimeSpan = csvReader.ReadTimeSpanTicksNull();
      ADecimal = csvReader.ReadDecimal();
      ANullableDecimal = csvReader.ReadDecimalNull();
      ADecimal2 = csvReader.ReadDecimal();
      ANullableDecimal2 = csvReader.ReadDecimalNull();
      ADecimal4 = csvReader.ReadDecimal();
      ANullableDecimal4 = csvReader.ReadDecimalNull();
      ADecimal5 = csvReader.ReadDecimal();
      ANullableDecimal5 = csvReader.ReadDecimalNull();
      ABool = csvReader.ReadBool();
      ANullableBool = csvReader.ReadBoolNull();
      AInt = csvReader.ReadInt();
      ANullableInt = csvReader.ReadIntNull();
      ALong = csvReader.ReadLong();
      ANullableLong = csvReader.ReadLongNull();
      AChar = csvReader.ReadChar();
      ANullableChar = csvReader.ReadCharNull();
      AString = csvReader.ReadString();
      ANullableString = csvReader.ReadStringNull();
      AEnum = (SampleStateEnum)csvReader.ReadInt();
      ANullableEnum = (SampleStateEnum?)csvReader.ReadIntNull();
      onCsvConstruct();
    }
    partial void onCsvConstruct();


    /// <summary>
    /// New DataTypeSample read from CSV file
    /// </summary>
    internal static DataTypeSample Create(int key, CsvReader csvReader) {
      return new DataTypeSample(key, csvReader);
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Adds DataTypeSample to DC.Data.DataTypeSamples.<br/>
    /// Throws an Exception when DataTypeSample is already stored.
    /// </summary>
    public void Store() {
      if (Key>=0) {
        throw new Exception($"DataTypeSample cannot be stored again in DC.Data, key {Key} is greater equal 0." + Environment.NewLine + ToString());
      }

      var isCancelled = false;
      onStoring(ref isCancelled);
      if (isCancelled) return;

      DC.Data._DataTypeSamples.Add(this);
      onStored();
#if DEBUG
      DC.Trace?.Invoke($"Stored DataTypeSample #{GetHashCode()} @{Key}");
#endif
    }
    partial void onStoring(ref bool isCancelled);
    partial void onStored();


    /// <summary>
    /// Estimated number of UTF8 characters needed to write DataTypeSample to CSV file
    /// </summary>
    public const int EstimatedLineLength = 696;


    /// <summary>
    /// Write DataTypeSample to CSV file
    /// </summary>
    internal static void Write(DataTypeSample dataTypeSample, CsvWriter csvWriter) {
      dataTypeSample.onCsvWrite();
      csvWriter.WriteDate(dataTypeSample.ADate);
      csvWriter.WriteDate(dataTypeSample.ANullableDate);
      csvWriter.WriteTime(dataTypeSample.ATime);
      csvWriter.WriteTime(dataTypeSample.ANullableTime);
      csvWriter.WriteDateMinutes(dataTypeSample.ADateMinutes);
      csvWriter.WriteDateMinutes(dataTypeSample.ANullableDateMinutes);
      csvWriter.WriteDateSeconds(dataTypeSample.ADateSeconds);
      csvWriter.WriteDateSeconds(dataTypeSample.ANullableDateSeconds);
      csvWriter.WriteDateTimeTicks(dataTypeSample.ADateTime);
      csvWriter.WriteDateTimeTicks(dataTypeSample.ANullableDateTime);
      csvWriter.WriteTimeSpanTicks(dataTypeSample.ATimeSpan);
      csvWriter.WriteTimeSpanTicks(dataTypeSample.ANullableTimeSpan);
      csvWriter.Write(dataTypeSample.ADecimal);
      csvWriter.Write(dataTypeSample.ANullableDecimal);
      csvWriter.WriteDecimal2(dataTypeSample.ADecimal2);
      csvWriter.WriteDecimal2(dataTypeSample.ANullableDecimal2);
      csvWriter.WriteDecimal4(dataTypeSample.ADecimal4);
      csvWriter.WriteDecimal4(dataTypeSample.ANullableDecimal4);
      csvWriter.WriteDecimal5(dataTypeSample.ADecimal5);
      csvWriter.WriteDecimal5(dataTypeSample.ANullableDecimal5);
      csvWriter.Write(dataTypeSample.ABool);
      csvWriter.Write(dataTypeSample.ANullableBool);
      csvWriter.Write(dataTypeSample.AInt);
      csvWriter.Write(dataTypeSample.ANullableInt);
      csvWriter.Write(dataTypeSample.ALong);
      csvWriter.Write(dataTypeSample.ANullableLong);
      csvWriter.Write(dataTypeSample.AChar);
      csvWriter.Write(dataTypeSample.ANullableChar);
      csvWriter.Write(dataTypeSample.AString);
      csvWriter.Write(dataTypeSample.ANullableString);
      csvWriter.Write((int)dataTypeSample.AEnum);
      csvWriter.Write((int?)dataTypeSample.ANullableEnum);
    }
    partial void onCsvWrite();


    /// <summary>
    /// Updates DataTypeSample with the provided values
    /// </summary>
    public void Update(
      DateTime aDate, 
      DateTime? aNullableDate, 
      TimeSpan aTime, 
      TimeSpan? aNullableTime, 
      DateTime aDateMinutes, 
      DateTime? aNullableDateMinutes, 
      DateTime aDateSeconds, 
      DateTime? aNullableDateSeconds, 
      DateTime aDateTime, 
      DateTime? aNullableDateTime, 
      TimeSpan aTimeSpan, 
      TimeSpan? aNullableTimeSpan, 
      decimal aDecimal, 
      decimal? aNullableDecimal, 
      decimal aDecimal2, 
      decimal? aNullableDecimal2, 
      decimal aDecimal4, 
      decimal? aNullableDecimal4, 
      decimal aDecimal5, 
      decimal? aNullableDecimal5, 
      bool aBool, 
      bool? aNullableBool, 
      int aInt, 
      int? aNullableInt, 
      long aLong, 
      long? aNullableLong, 
      char aChar, 
      char? aNullableChar, 
      string aString, 
      string? aNullableString, 
      SampleStateEnum aEnum, 
      SampleStateEnum? aNullableEnum)
    {
      var clone = new DataTypeSample(this);
      var isCancelled = false;
      onUpdating(
        aDate, 
        aNullableDate, 
        aTime, 
        aNullableTime, 
        aDateMinutes, 
        aNullableDateMinutes, 
        aDateSeconds, 
        aNullableDateSeconds, 
        aDateTime, 
        aNullableDateTime, 
        aTimeSpan, 
        aNullableTimeSpan, 
        aDecimal, 
        aNullableDecimal, 
        aDecimal2, 
        aNullableDecimal2, 
        aDecimal4, 
        aNullableDecimal4, 
        aDecimal5, 
        aNullableDecimal5, 
        aBool, 
        aNullableBool, 
        aInt, 
        aNullableInt, 
        aLong, 
        aNullableLong, 
        aChar, 
        aNullableChar, 
        aString, 
        aNullableString, 
        aEnum, 
        aNullableEnum, 
        ref isCancelled);
      if (isCancelled) return;

#if DEBUG
      DC.Trace?.Invoke($"Updating DataTypeSample: {ToTraceString()}");
#endif

      //update properties and detect if any value has changed
      var isChangeDetected = false;
      var aDateRounded = aDate.Floor(Rounding.Days);
      if (ADate!=aDateRounded) {
        ADate = aDateRounded;
        isChangeDetected = true;
      }
      var aNullableDateRounded = aNullableDate.Floor(Rounding.Days);
      if (ANullableDate!=aNullableDateRounded) {
        ANullableDate = aNullableDateRounded;
        isChangeDetected = true;
      }
      var aTimeRounded = aTime.Round(Rounding.Seconds);
      if (ATime!=aTimeRounded) {
        ATime = aTimeRounded;
        isChangeDetected = true;
      }
      var aNullableTimeRounded = aNullableTime.Round(Rounding.Seconds);
      if (ANullableTime!=aNullableTimeRounded) {
        ANullableTime = aNullableTimeRounded;
        isChangeDetected = true;
      }
      var aDateMinutesRounded = aDateMinutes.Round(Rounding.Minutes);
      if (ADateMinutes!=aDateMinutesRounded) {
        ADateMinutes = aDateMinutesRounded;
        isChangeDetected = true;
      }
      var aNullableDateMinutesRounded = aNullableDateMinutes.Round(Rounding.Minutes);
      if (ANullableDateMinutes!=aNullableDateMinutesRounded) {
        ANullableDateMinutes = aNullableDateMinutesRounded;
        isChangeDetected = true;
      }
      var aDateSecondsRounded = aDateSeconds.Round(Rounding.Seconds);
      if (ADateSeconds!=aDateSecondsRounded) {
        ADateSeconds = aDateSecondsRounded;
        isChangeDetected = true;
      }
      var aNullableDateSecondsRounded = aNullableDateSeconds.Round(Rounding.Seconds);
      if (ANullableDateSeconds!=aNullableDateSecondsRounded) {
        ANullableDateSeconds = aNullableDateSecondsRounded;
        isChangeDetected = true;
      }
      if (ADateTime!=aDateTime) {
        ADateTime = aDateTime;
        isChangeDetected = true;
      }
      if (ANullableDateTime!=aNullableDateTime) {
        ANullableDateTime = aNullableDateTime;
        isChangeDetected = true;
      }
      if (ATimeSpan!=aTimeSpan) {
        ATimeSpan = aTimeSpan;
        isChangeDetected = true;
      }
      if (ANullableTimeSpan!=aNullableTimeSpan) {
        ANullableTimeSpan = aNullableTimeSpan;
        isChangeDetected = true;
      }
      if (ADecimal!=aDecimal) {
        ADecimal = aDecimal;
        isChangeDetected = true;
      }
      if (ANullableDecimal!=aNullableDecimal) {
        ANullableDecimal = aNullableDecimal;
        isChangeDetected = true;
      }
      var aDecimal2Rounded = aDecimal2.Round(2);
      if (ADecimal2!=aDecimal2Rounded) {
        ADecimal2 = aDecimal2Rounded;
        isChangeDetected = true;
      }
      var aNullableDecimal2Rounded = aNullableDecimal2.Round(2);
      if (ANullableDecimal2!=aNullableDecimal2Rounded) {
        ANullableDecimal2 = aNullableDecimal2Rounded;
        isChangeDetected = true;
      }
      var aDecimal4Rounded = aDecimal4.Round(4);
      if (ADecimal4!=aDecimal4Rounded) {
        ADecimal4 = aDecimal4Rounded;
        isChangeDetected = true;
      }
      var aNullableDecimal4Rounded = aNullableDecimal4.Round(4);
      if (ANullableDecimal4!=aNullableDecimal4Rounded) {
        ANullableDecimal4 = aNullableDecimal4Rounded;
        isChangeDetected = true;
      }
      var aDecimal5Rounded = aDecimal5.Round(5);
      if (ADecimal5!=aDecimal5Rounded) {
        ADecimal5 = aDecimal5Rounded;
        isChangeDetected = true;
      }
      var aNullableDecimal5Rounded = aNullableDecimal5.Round(5);
      if (ANullableDecimal5!=aNullableDecimal5Rounded) {
        ANullableDecimal5 = aNullableDecimal5Rounded;
        isChangeDetected = true;
      }
      if (ABool!=aBool) {
        ABool = aBool;
        isChangeDetected = true;
      }
      if (ANullableBool!=aNullableBool) {
        ANullableBool = aNullableBool;
        isChangeDetected = true;
      }
      if (AInt!=aInt) {
        AInt = aInt;
        isChangeDetected = true;
      }
      if (ANullableInt!=aNullableInt) {
        ANullableInt = aNullableInt;
        isChangeDetected = true;
      }
      if (ALong!=aLong) {
        ALong = aLong;
        isChangeDetected = true;
      }
      if (ANullableLong!=aNullableLong) {
        ANullableLong = aNullableLong;
        isChangeDetected = true;
      }
      if (AChar!=aChar) {
        AChar = aChar;
        isChangeDetected = true;
      }
      if (ANullableChar!=aNullableChar) {
        ANullableChar = aNullableChar;
        isChangeDetected = true;
      }
      if (AString!=aString) {
        AString = aString;
        isChangeDetected = true;
      }
      if (ANullableString!=aNullableString) {
        ANullableString = aNullableString;
        isChangeDetected = true;
      }
      if (AEnum!=aEnum) {
        AEnum = aEnum;
        isChangeDetected = true;
      }
      if (ANullableEnum!=aNullableEnum) {
        ANullableEnum = aNullableEnum;
        isChangeDetected = true;
      }
      if (isChangeDetected) {
        onUpdated(clone);
        if (Key>=0) {
          DC.Data._DataTypeSamples.ItemHasChanged(clone, this);
        } else if (DC.Data.IsTransaction) {
          DC.Data.AddTransaction(new TransactionItem(25, TransactionActivityEnum.Update, Key, this, oldItem: clone));
        }
        HasChanged?.Invoke(clone, this);
      }
#if DEBUG
      DC.Trace?.Invoke($"Updated DataTypeSample: {ToTraceString()}");
#endif
    }
    partial void onUpdating(
      DateTime aDate, 
      DateTime? aNullableDate, 
      TimeSpan aTime, 
      TimeSpan? aNullableTime, 
      DateTime aDateMinutes, 
      DateTime? aNullableDateMinutes, 
      DateTime aDateSeconds, 
      DateTime? aNullableDateSeconds, 
      DateTime aDateTime, 
      DateTime? aNullableDateTime, 
      TimeSpan aTimeSpan, 
      TimeSpan? aNullableTimeSpan, 
      decimal aDecimal, 
      decimal? aNullableDecimal, 
      decimal aDecimal2, 
      decimal? aNullableDecimal2, 
      decimal aDecimal4, 
      decimal? aNullableDecimal4, 
      decimal aDecimal5, 
      decimal? aNullableDecimal5, 
      bool aBool, 
      bool? aNullableBool, 
      int aInt, 
      int? aNullableInt, 
      long aLong, 
      long? aNullableLong, 
      char aChar, 
      char? aNullableChar, 
      string aString, 
      string? aNullableString, 
      SampleStateEnum aEnum, 
      SampleStateEnum? aNullableEnum, 
      ref bool isCancelled);
    partial void onUpdated(DataTypeSample old);


    /// <summary>
    /// Updates this DataTypeSample with values from CSV file
    /// </summary>
    internal static void Update(DataTypeSample dataTypeSample, CsvReader csvReader){
      dataTypeSample.ADate = csvReader.ReadDate();
      dataTypeSample.ANullableDate = csvReader.ReadDateNull();
      dataTypeSample.ATime = csvReader.ReadTime();
      dataTypeSample.ANullableTime = csvReader.ReadTimeNull();
      dataTypeSample.ADateMinutes = csvReader.ReadDateSeconds();
      dataTypeSample.ANullableDateMinutes = csvReader.ReadDateSecondsNull();
      dataTypeSample.ADateSeconds = csvReader.ReadDateSeconds();
      dataTypeSample.ANullableDateSeconds = csvReader.ReadDateSecondsNull();
      dataTypeSample.ADateTime = csvReader.ReadDateTimeTicks();
      dataTypeSample.ANullableDateTime = csvReader.ReadDateTimeTicksNull();
      dataTypeSample.ATimeSpan = csvReader.ReadTimeSpanTicks();
      dataTypeSample.ANullableTimeSpan = csvReader.ReadTimeSpanTicksNull();
      dataTypeSample.ADecimal = csvReader.ReadDecimal();
      dataTypeSample.ANullableDecimal = csvReader.ReadDecimalNull();
      dataTypeSample.ADecimal2 = csvReader.ReadDecimal();
      dataTypeSample.ANullableDecimal2 = csvReader.ReadDecimalNull();
      dataTypeSample.ADecimal4 = csvReader.ReadDecimal();
      dataTypeSample.ANullableDecimal4 = csvReader.ReadDecimalNull();
      dataTypeSample.ADecimal5 = csvReader.ReadDecimal();
      dataTypeSample.ANullableDecimal5 = csvReader.ReadDecimalNull();
      dataTypeSample.ABool = csvReader.ReadBool();
      dataTypeSample.ANullableBool = csvReader.ReadBoolNull();
      dataTypeSample.AInt = csvReader.ReadInt();
      dataTypeSample.ANullableInt = csvReader.ReadIntNull();
      dataTypeSample.ALong = csvReader.ReadLong();
      dataTypeSample.ANullableLong = csvReader.ReadLongNull();
      dataTypeSample.AChar = csvReader.ReadChar();
      dataTypeSample.ANullableChar = csvReader.ReadCharNull();
      dataTypeSample.AString = csvReader.ReadString();
      dataTypeSample.ANullableString = csvReader.ReadStringNull();
      dataTypeSample.AEnum = (SampleStateEnum)csvReader.ReadInt();
      dataTypeSample.ANullableEnum = (SampleStateEnum?)csvReader.ReadIntNull();
      dataTypeSample.onCsvUpdate();
    }
    partial void onCsvUpdate();


    /// <summary>
    /// Removes DataTypeSample from DC.Data.DataTypeSamples.
    /// </summary>
    public void Release() {
      if (Key<0) {
        throw new Exception($"DataTypeSample.Release(): DataTypeSample '{this}' is not stored in DC.Data, key is {Key}.");
      }
      onReleasing();
      DC.Data._DataTypeSamples.Remove(Key);
      onReleased();
#if DEBUG
      DC.Trace?.Invoke($"Released DataTypeSample @{Key} #{GetHashCode()}");
#endif
    }
    partial void onReleasing();
    partial void onReleased();


    /// <summary>
    /// Undoes the new() statement as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemNew(IStorageItem item) {
      var dataTypeSample = (DataTypeSample) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback new DataTypeSample(): {dataTypeSample.ToTraceString()}");
#endif
      dataTypeSample.onRollbackItemNew();
    }
    partial void onRollbackItemNew();


    /// <summary>
    /// Releases DataTypeSample from DC.Data.DataTypeSamples as part of a transaction rollback of Store().
    /// </summary>
    internal static void RollbackItemStore(IStorageItem item) {
      var dataTypeSample = (DataTypeSample) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback DataTypeSample.Store(): {dataTypeSample.ToTraceString()}");
#endif
      dataTypeSample.onRollbackItemStored();
    }
    partial void onRollbackItemStored();


    /// <summary>
    /// Restores the DataTypeSample item data as it was before the last update as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemUpdate(IStorageItem oldStorageItem, IStorageItem newStorageItem) {
      var oldItem = (DataTypeSample) oldStorageItem;//an item clone with the values before item was updated
      var item = (DataTypeSample) newStorageItem;//is the instance whose values should be restored
#if DEBUG
      DC.Trace?.Invoke($"Rolling back DataTypeSample.Update(): {item.ToTraceString()}");
#endif

      // updated item: restore old values
      item.ADate = oldItem.ADate;
      item.ANullableDate = oldItem.ANullableDate;
      item.ATime = oldItem.ATime;
      item.ANullableTime = oldItem.ANullableTime;
      item.ADateMinutes = oldItem.ADateMinutes;
      item.ANullableDateMinutes = oldItem.ANullableDateMinutes;
      item.ADateSeconds = oldItem.ADateSeconds;
      item.ANullableDateSeconds = oldItem.ANullableDateSeconds;
      item.ADateTime = oldItem.ADateTime;
      item.ANullableDateTime = oldItem.ANullableDateTime;
      item.ATimeSpan = oldItem.ATimeSpan;
      item.ANullableTimeSpan = oldItem.ANullableTimeSpan;
      item.ADecimal = oldItem.ADecimal;
      item.ANullableDecimal = oldItem.ANullableDecimal;
      item.ADecimal2 = oldItem.ADecimal2;
      item.ANullableDecimal2 = oldItem.ANullableDecimal2;
      item.ADecimal4 = oldItem.ADecimal4;
      item.ANullableDecimal4 = oldItem.ANullableDecimal4;
      item.ADecimal5 = oldItem.ADecimal5;
      item.ANullableDecimal5 = oldItem.ANullableDecimal5;
      item.ABool = oldItem.ABool;
      item.ANullableBool = oldItem.ANullableBool;
      item.AInt = oldItem.AInt;
      item.ANullableInt = oldItem.ANullableInt;
      item.ALong = oldItem.ALong;
      item.ANullableLong = oldItem.ANullableLong;
      item.AChar = oldItem.AChar;
      item.ANullableChar = oldItem.ANullableChar;
      item.AString = oldItem.AString;
      item.ANullableString = oldItem.ANullableString;
      item.AEnum = oldItem.AEnum;
      item.ANullableEnum = oldItem.ANullableEnum;
      item.onRollbackItemUpdated(oldItem);
#if DEBUG
      DC.Trace?.Invoke($"Rolled back DataTypeSample.Update(): {item.ToTraceString()}");
#endif
    }
    partial void onRollbackItemUpdated(DataTypeSample oldDataTypeSample);


    /// <summary>
    /// Adds DataTypeSample to DC.Data.DataTypeSamples as part of a transaction rollback of Release().
    /// </summary>
    internal static void RollbackItemRelease(IStorageItem item) {
      var dataTypeSample = (DataTypeSample) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback DataTypeSample.Release(): {dataTypeSample.ToTraceString()}");
#endif
      dataTypeSample.onRollbackItemRelease();
    }
    partial void onRollbackItemRelease();


    /// <summary>
    /// Returns property values for tracing. Parents are shown with their key instead their content.
    /// </summary>
    public string ToTraceString() {
      var returnString =
        $"{this.GetKeyOrHash()}|" +
        $" {ADate.ToShortDateString()}|" +
        $" {ANullableDate?.ToShortDateString()}|" +
        $" {ATime}|" +
        $" {ANullableTime}|" +
        $" {ADateMinutes}|" +
        $" {ANullableDateMinutes}|" +
        $" {ADateSeconds}|" +
        $" {ANullableDateSeconds}|" +
        $" {ADateTime}|" +
        $" {ANullableDateTime}|" +
        $" {ATimeSpan}|" +
        $" {ANullableTimeSpan}|" +
        $" {ADecimal}|" +
        $" {ANullableDecimal}|" +
        $" {ADecimal2}|" +
        $" {ANullableDecimal2}|" +
        $" {ADecimal4}|" +
        $" {ANullableDecimal4}|" +
        $" {ADecimal5}|" +
        $" {ANullableDecimal5}|" +
        $" {ABool}|" +
        $" {ANullableBool}|" +
        $" {AInt}|" +
        $" {ANullableInt}|" +
        $" {ALong}|" +
        $" {ANullableLong}|" +
        $" {AChar}|" +
        $" {ANullableChar}|" +
        $" {AString}|" +
        $" {ANullableString}|" +
        $" {AEnum}|" +
        $" {ANullableEnum}";
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
        $" {ADate.ToShortDateString()}," +
        $" {ANullableDate?.ToShortDateString()}," +
        $" {ATime}," +
        $" {ANullableTime}," +
        $" {ADateMinutes}," +
        $" {ANullableDateMinutes}," +
        $" {ADateSeconds}," +
        $" {ANullableDateSeconds}," +
        $" {ADateTime}," +
        $" {ANullableDateTime}," +
        $" {ATimeSpan}," +
        $" {ANullableTimeSpan}," +
        $" {ADecimal}," +
        $" {ANullableDecimal}," +
        $" {ADecimal2}," +
        $" {ANullableDecimal2}," +
        $" {ADecimal4}," +
        $" {ANullableDecimal4}," +
        $" {ADecimal5}," +
        $" {ANullableDecimal5}," +
        $" {ABool}," +
        $" {ANullableBool}," +
        $" {AInt}," +
        $" {ANullableInt}," +
        $" {ALong}," +
        $" {ANullableLong}," +
        $" {AChar}," +
        $" {ANullableChar}," +
        $" {AString}," +
        $" {ANullableString}," +
        $" {AEnum}," +
        $" {ANullableEnum}";
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
        $" ADate: {ADate.ToShortDateString()}," +
        $" ANullableDate: {ANullableDate?.ToShortDateString()}," +
        $" ATime: {ATime}," +
        $" ANullableTime: {ANullableTime}," +
        $" ADateMinutes: {ADateMinutes}," +
        $" ANullableDateMinutes: {ANullableDateMinutes}," +
        $" ADateSeconds: {ADateSeconds}," +
        $" ANullableDateSeconds: {ANullableDateSeconds}," +
        $" ADateTime: {ADateTime}," +
        $" ANullableDateTime: {ANullableDateTime}," +
        $" ATimeSpan: {ATimeSpan}," +
        $" ANullableTimeSpan: {ANullableTimeSpan}," +
        $" ADecimal: {ADecimal}," +
        $" ANullableDecimal: {ANullableDecimal}," +
        $" ADecimal2: {ADecimal2}," +
        $" ANullableDecimal2: {ANullableDecimal2}," +
        $" ADecimal4: {ADecimal4}," +
        $" ANullableDecimal4: {ANullableDecimal4}," +
        $" ADecimal5: {ADecimal5}," +
        $" ANullableDecimal5: {ANullableDecimal5}," +
        $" ABool: {ABool}," +
        $" ANullableBool: {ANullableBool}," +
        $" AInt: {AInt}," +
        $" ANullableInt: {ANullableInt}," +
        $" ALong: {ALong}," +
        $" ANullableLong: {ANullableLong}," +
        $" AChar: {AChar}," +
        $" ANullableChar: {ANullableChar}," +
        $" AString: {AString}," +
        $" ANullableString: {ANullableString}," +
        $" AEnum: {AEnum}," +
        $" ANullableEnum: {ANullableEnum};";
      onToString(ref returnString);
      return returnString;
    }
    partial void onToString(ref string returnString);
    #endregion
  }
}
