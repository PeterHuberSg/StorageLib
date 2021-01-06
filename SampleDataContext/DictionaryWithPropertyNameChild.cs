using System;
using System.Collections.Generic;
using StorageLib;


namespace DataModelSamples  {


  public partial class DictionaryWithPropertyNameChild: IStorageItemGeneric<DictionaryWithPropertyNameChild> {


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
    //partial void onCloned(DictionaryWithPropertyNameChild clone) {
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
    /// Called after DictionaryWithPropertyNameChild.Store() is executed
    /// </summary>
    //partial void onStored() {
    //}


    /// <summary>
    /// Called before DictionaryWithPropertyNameChild gets written to a CSV file
    /// </summary>
    //partial void onCsvWrite() {
    //}


    /// <summary>
    /// Called before any property of DictionaryWithPropertyNameChild is updated and before the HasChanged event gets raised
    /// </summary>
    //partial void onUpdating(DateTime date1, DateTime date2, DictionaryWithPropertyNameParent parent, ref bool isCancelled){
   //}


    /// <summary>
    /// Called after all properties of DictionaryWithPropertyNameChild are updated, but before the HasChanged event gets raised
    /// </summary>
    //partial void onUpdated(DictionaryWithPropertyNameChild old) {
    //}


    /// <summary>
    /// Called after an update for DictionaryWithPropertyNameChild is read from a CSV file
    /// </summary>
    //partial void onCsvUpdate() {
    //}


    /// <summary>
    /// Called after DictionaryWithPropertyNameChild.Release() got executed
    /// </summary>
    //partial void onReleased() {
    //}


    /// <summary>
    /// Called after 'new DictionaryWithPropertyNameChild()' transaction is rolled back
    /// </summary>
    //partial void onRollbackItemNew() {
    //}


    /// <summary>
    /// Called after DictionaryWithPropertyNameChild.Store() transaction is rolled back
    /// </summary>
    //partial void onRollbackItemStored() {
    //}


    /// <summary>
    /// Called after DictionaryWithPropertyNameChild.Update() transaction is rolled back
    /// </summary>
    //partial void onRollbackItemUpdated(DictionaryWithPropertyNameChild oldDictionaryWithPropertyNameChild) {
    //}


    /// <summary>
    /// Called after DictionaryWithPropertyNameChild.Release() transaction is rolled back
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
