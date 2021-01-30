using System;
using System.Collections.Generic;
using StorageLib;


namespace DataModelSamples  {


  public partial class DictionaryWithPropertyNameParent: IStorageItem<DictionaryWithPropertyNameParent> {


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
    //partial void onCloned(DictionaryWithPropertyNameParent clone) {
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
    /// Called after DictionaryWithPropertyNameParent.Store() is executed
    /// </summary>
    //partial void onStored() {
    //}


    /// <summary>
    /// Called before DictionaryWithPropertyNameParent gets written to a CSV file
    /// </summary>
    //partial void onCsvWrite() {
    //}


    /// <summary>
    /// Called before any property of DictionaryWithPropertyNameParent is updated and before the HasChanged event gets raised
    /// </summary>
    //partial void onUpdating(string name, ref bool isCancelled){
   //}


    /// <summary>
    /// Called after all properties of DictionaryWithPropertyNameParent are updated, but before the HasChanged event gets raised
    /// </summary>
    //partial void onUpdated(DictionaryWithPropertyNameParent old) {
    //}


    /// <summary>
    /// Called after an update for DictionaryWithPropertyNameParent is read from a CSV file
    /// </summary>
    //partial void onCsvUpdate() {
    //}


    /// <summary>
    /// Called after DictionaryWithPropertyNameParent.Release() got executed
    /// </summary>
    //partial void onReleased() {
    //}


    /// <summary>
    /// Called after 'new DictionaryWithPropertyNameParent()' transaction is rolled back
    /// </summary>
    //partial void onRollbackItemNew() {
    //}


    /// <summary>
    /// Called after DictionaryWithPropertyNameParent.Store() transaction is rolled back
    /// </summary>
    //partial void onRollbackItemStored() {
    //}


    /// <summary>
    /// Called after DictionaryWithPropertyNameParent.Update() transaction is rolled back
    /// </summary>
    //partial void onRollbackItemUpdated(DictionaryWithPropertyNameParent oldDictionaryWithPropertyNameParent) {
    //}


    /// <summary>
    /// Called after DictionaryWithPropertyNameParent.Release() transaction is rolled back
    /// </summary>
    //partial void onRollbackItemRelease() {
    //}


    /// <summary>
    /// Called after a dictionaryWithPropertyNameChild gets added to Children.
    /// </summary>
    //partial void onAddedToChildren(DictionaryWithPropertyNameChild dictionaryWithPropertyNameChild){
    //}


    /// <summary>
    /// Called after a dictionaryWithPropertyNameChild gets removed from Children.
    /// </summary>
    //partial void onRemovedFromChildren(DictionaryWithPropertyNameChild dictionaryWithPropertyNameChild){
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
