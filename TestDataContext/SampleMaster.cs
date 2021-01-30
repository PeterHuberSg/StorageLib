using System;
using System.Collections.Generic;
using StorageLib;


namespace TestContext  {


    /// <summary>
    /// Some comment for SampleMaster.
    /// With an additional line.
    /// </summary>
  public partial class SampleMaster: IStorageItem<SampleMaster> {


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
    partial void onCloned(SampleMaster clone) {
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
    /// Called after SampleMaster.Store() is executed
    /// </summary>
    partial void onStored() {
    }


    /// <summary>
    /// Called before SampleMaster gets written to a CSV file
    /// </summary>
    partial void onCsvWrite() {
    }


    /// <summary>
    /// Called before any property of SampleMaster is updated and before the HasChanged event gets raised
    /// </summary>
    partial void onUpdating(string text, int numberWithDefault, ref bool isCancelled){
   }


    /// <summary>
    /// Called after all properties of SampleMaster are updated, but before the HasChanged event gets raised
    /// </summary>
    partial void onUpdated(SampleMaster old) {
    }


    /// <summary>
    /// Called after an update for SampleMaster is read from a CSV file
    /// </summary>
    partial void onCsvUpdate() {
    }


    /// <summary>
    /// Called after 'new SampleMaster()' transaction is rolled back
    /// </summary>
    partial void onRollbackItemNew() {
    }


    /// <summary>
    /// Called after SampleMaster.Store() transaction is rolled back
    /// </summary>
    partial void onRollbackItemStored() {
    }


    /// <summary>
    /// Called after SampleMaster.Update() transaction is rolled back
    /// </summary>
    partial void onRollbackItemUpdated(SampleMaster oldSampleMaster) {
    }


    /// <summary>
    /// Called after a sample gets added to SampleX.
    /// </summary>
    partial void onAddedToSampleX(Sample sample){
    }


    /// <summary>
    /// Called after a sample gets removed from SampleX.
    /// </summary>
    partial void onRemovedFromSampleX(Sample sample){
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
