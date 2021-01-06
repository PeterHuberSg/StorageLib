using System;
using System.Collections.Generic;
using StorageLib;


namespace TestContext  {


    /// <summary>
    /// Class having every possible data type used for a property
    /// </summary>
  public partial class DataTypeSample: IStorageItemGeneric<DataTypeSample> {


    #region Properties
    //      ----------

    #endregion


    #region Events
    //      ------

    #endregion


    #region Constructors
    //      ------------

    /// <summary>
    /// Called once the constructor has filled all the properties
    /// </summary>
    partial void onConstruct() {
    }


    /// <summary>
    /// Called once the cloning constructor has filled all the properties. Clones have no children data.
    /// </summary>
    partial void onCloned(DataTypeSample clone) {
    }


    /// <summary>
    /// Called once the CSV-constructor who reads the data from a CSV file has filled all the properties
    /// </summary>
    partial void onCsvConstruct() {
    }


    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Called before {ClassName}.Store() gets executed
    /// </summary>
    partial void onStoring(ref bool isCancelled) {
    }


    /// <summary>
    /// Called after DataTypeSample.Store() is executed
    /// </summary>
    partial void onStored() {
    }


    /// <summary>
    /// Called before DataTypeSample gets written to a CSV file
    /// </summary>
    partial void onCsvWrite() {
    }


    /// <summary>
    /// Called before any property of DataTypeSample is updated and before the HasChanged event gets raised
    /// </summary>
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
      ref bool isCancelled)
   {
   }


    /// <summary>
    /// Called after all properties of DataTypeSample are updated, but before the HasChanged event gets raised
    /// </summary>
    partial void onUpdated(DataTypeSample old) {
    }


    /// <summary>
    /// Called after an update for DataTypeSample is read from a CSV file
    /// </summary>
    partial void onCsvUpdate() {
    }


    /// <summary>
    /// Called after DataTypeSample.Release() got executed
    /// </summary>
    partial void onReleased() {
    }


    /// <summary>
    /// Called after 'new DataTypeSample()' transaction is rolled back
    /// </summary>
    partial void onRollbackItemNew() {
    }


    /// <summary>
    /// Called after DataTypeSample.Store() transaction is rolled back
    /// </summary>
    partial void onRollbackItemStored() {
    }


    /// <summary>
    /// Called after DataTypeSample.Update() transaction is rolled back
    /// </summary>
    partial void onRollbackItemUpdated(DataTypeSample oldDataTypeSample) {
    }


    /// <summary>
    /// Called after DataTypeSample.Release() transaction is rolled back
    /// </summary>
    partial void onRollbackItemRelease() {
    }


    /// <summary>
    /// Updates returnString with additional info for a short description.
    /// </summary>
    partial void onToShortString(ref string returnString) {
    }


    /// <summary>
    /// Updates returnString with additional info for a short description.
    /// </summary>
    partial void onToString(ref string returnString) {
    }
    #endregion
  }
}
