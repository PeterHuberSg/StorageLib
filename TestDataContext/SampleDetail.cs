using System;
using System.Collections.Generic;
using StorageLib;


namespace StorageDataContext  {


    /// <summary>
    /// Some comment for SampleDetail
    /// </summary>
  public partial class SampleDetail: IStorageItemGeneric<SampleDetail> {


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
    partial void onCloned(SampleDetail clone) {
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
    /// Called after SampleDetail.Store() is executed
    /// </summary>
    partial void onStored() {
    }


    /// <summary>
    /// Called before SampleDetail gets written to a CSV file
    /// </summary>
    partial void onCsvWrite() {
    }


    /// <summary>
    /// Called before any property of SampleDetail is updated and before the HasChanged event gets raised
    /// </summary>
    partial void onUpdating(string text, Sample sample, ref bool isCancelled){
   }


    /// <summary>
    /// Called after all properties of SampleDetail are updated, but before the HasChanged event gets raised
    /// </summary>
    partial void onUpdated(SampleDetail old) {
    }


    /// <summary>
    /// Called after an update for SampleDetail is read from a CSV file
    /// </summary>
    partial void onCsvUpdate() {
    }


    /// <summary>
    /// Called after SampleDetail.Release() got executed
    /// </summary>
    partial void onReleased() {
    }


    /// <summary>
    /// Called after 'new SampleDetail()' transaction is rolled back
    /// </summary>
    partial void onRollbackItemNew() {
    }


    /// <summary>
    /// Called after SampleDetail.Store() transaction is rolled back
    /// </summary>
    partial void onRollbackItemStored() {
    }


    /// <summary>
    /// Called after SampleDetail.Update() transaction is rolled back
    /// </summary>
    partial void onRollbackItemUpdated(SampleDetail oldSampleDetail) {
    }


    /// <summary>
    /// Called after SampleDetail.Release() transaction is rolled back
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
