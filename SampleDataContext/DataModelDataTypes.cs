using System;
using System.Collections.Generic;
using StorageLib;


namespace DataModelSamples  {


  public partial class DataModelDataTypes: IStorageItem<DataModelDataTypes> {


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
    //partial void onConstruct() {
    //}


    /// <summary>
    /// Called once the cloning constructor has filled all the properties. Clones have no children data.
    /// </summary>
    //partial void onCloned(DataModelDataTypes clone) {
    //}


    /// <summary>
    /// Called once the CSV-constructor who reads the data from a CSV file has filled all the properties
    /// </summary>
    //partial void onCsvConstruct() {
    //}


    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Called before {ClassName}.Store() gets executed
    /// </summary>
    //partial void onStoring(ref bool isCancelled) {
    //}


    /// <summary>
    /// Called after DataModelDataTypes.Store() is executed
    /// </summary>
    //partial void onStored() {
    //}


    /// <summary>
    /// Called before DataModelDataTypes gets written to a CSV file
    /// </summary>
    //partial void onCsvWrite() {
    //}


    /// <summary>
    /// Called before any property of DataModelDataTypes is updated and before the HasChanged event gets raised
    /// </summary>
    //partial void onUpdating(
      //DateTime date, 
      //TimeSpan time, 
      //DateTime dateMinutes, 
      //DateTime dateSeconds, 
      //DateTime dateTimeTicks, 
      //TimeSpan timeSpanTicks, 
      //decimal decimal_, 
      //decimal decimal2, 
      //decimal decimal4, 
      //decimal decimal5, 
      //bool bool_, 
      //int int_, 
      //long long_, 
      //char char_, 
      //string string_, 
      //ref bool isCancelled)
   //{
   //}


    /// <summary>
    /// Called after all properties of DataModelDataTypes are updated, but before the HasChanged event gets raised
    /// </summary>
    //partial void onUpdated(DataModelDataTypes old) {
    //}


    /// <summary>
    /// Called after an update for DataModelDataTypes is read from a CSV file
    /// </summary>
    //partial void onCsvUpdate() {
    //}


    /// <summary>
    /// Called after DataModelDataTypes.Release() got executed
    /// </summary>
    //partial void onReleased() {
    //}


    /// <summary>
    /// Called after 'new DataModelDataTypes()' transaction is rolled back
    /// </summary>
    //partial void onRollbackItemNew() {
    //}


    /// <summary>
    /// Called after DataModelDataTypes.Store() transaction is rolled back
    /// </summary>
    //partial void onRollbackItemStored() {
    //}


    /// <summary>
    /// Called after DataModelDataTypes.Update() transaction is rolled back
    /// </summary>
    //partial void onRollbackItemUpdated(DataModelDataTypes oldDataModelDataTypes) {
    //}


    /// <summary>
    /// Called after DataModelDataTypes.Release() transaction is rolled back
    /// </summary>
    //partial void onRollbackItemRelease() {
    //}


    /// <summary>
    /// Updates returnString with additional info for a short description.
    /// </summary>
    //partial void onToShortString(ref string returnString) {
    //}


    /// <summary>
    /// Updates returnString with additional info for a short description.
    /// </summary>
    //partial void onToString(ref string returnString) {
    //}
    #endregion
  }
}
