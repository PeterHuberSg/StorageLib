using System;
using System.Collections.Generic;
using StorageLib;


namespace TestContext  {


    /// <summary>
    /// Some comment for Sample
    /// </summary>
  public partial class Sample: IStorageItem<Sample> {


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
    partial void onCloned(Sample clone) {
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
    /// Called after Sample.Store() is executed
    /// </summary>
    partial void onStored() {
    }


    /// <summary>
    /// Called before Sample gets written to a CSV file
    /// </summary>
    partial void onCsvWrite() {
    }


    /// <summary>
    /// Called before any property of Sample is updated and before the HasChanged event gets raised
    /// </summary>
    partial void onUpdating(
      string text, 
      bool flag, 
      int number, 
      decimal amount, 
      decimal amount4, 
      decimal? amount5, 
      decimal preciseDecimal, 
      SampleStateEnum sampleState, 
      DateTime dateOnly, 
      TimeSpan timeOnly, 
      DateTime dateTimeTicks, 
      DateTime dateTimeMinute, 
      DateTime dateTimeSecond, 
      SampleMaster? oneMaster, 
      SampleMaster? otherMaster, 
      string? optional, 
      ref bool isCancelled)
   {
   }


    /// <summary>
    /// Called after all properties of Sample are updated, but before the HasChanged event gets raised
    /// </summary>
    partial void onUpdated(Sample old) {
    }


    /// <summary>
    /// Called after an update for Sample is read from a CSV file
    /// </summary>
    partial void onCsvUpdate() {
    }


    /// <summary>
    /// Called before Sample.Release() gets executed
    /// </summary>
    partial void onReleasing() {
    }


    /// <summary>
    /// Called after Sample.Release() got executed
    /// </summary>
    partial void onReleased() {
    }


    /// <summary>
    /// Called after 'new Sample()' transaction is rolled back
    /// </summary>
    partial void onRollbackItemNew() {
    }


    /// <summary>
    /// Called after Sample.Store() transaction is rolled back
    /// </summary>
    partial void onRollbackItemStored() {
    }


    /// <summary>
    /// Called after Sample.Update() transaction is rolled back
    /// </summary>
    partial void onRollbackItemUpdated(Sample oldSample) {
    }


    /// <summary>
    /// Called after Sample.Release() transaction is rolled back
    /// </summary>
    partial void onRollbackItemRelease() {
    }


    /// <summary>
    /// Called after a sampleDetail gets added to SampleDetails.
    /// </summary>
    partial void onAddedToSampleDetails(SampleDetail sampleDetail){
    }


    /// <summary>
    /// Called after a sampleDetail gets removed from SampleDetails.
    /// </summary>
    partial void onRemovedFromSampleDetails(SampleDetail sampleDetail){
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
