using System;
using System.Collections.Generic;
using StorageLib;


namespace DataModelSamples  {


  public partial class SortedListWithPropertyNameChild: IStorageItem<SortedListWithPropertyNameChild> {


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
    //partial void onCloned(SortedListWithPropertyNameChild clone) {
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
    /// Called after SortedListWithPropertyNameChild.Store() is executed
    /// </summary>
    //partial void onStored() {
    //}


    /// <summary>
    /// Called before SortedListWithPropertyNameChild gets written to a CSV file
    /// </summary>
    //partial void onCsvWrite() {
    //}


    /// <summary>
    /// Called before any property of SortedListWithPropertyNameChild is updated and before the HasChanged event gets raised
    /// </summary>
    //partial void onUpdating(string name, string address, SortedListWithPropertyNameParent parent, ref bool isCancelled){
   //}


    /// <summary>
    /// Called after all properties of SortedListWithPropertyNameChild are updated, but before the HasChanged event gets raised
    /// </summary>
    //partial void onUpdated(SortedListWithPropertyNameChild old) {
    //}


    /// <summary>
    /// Called after an update for SortedListWithPropertyNameChild is read from a CSV file
    /// </summary>
    //partial void onCsvUpdate() {
    //}


    /// <summary>
    /// Called after SortedListWithPropertyNameChild.Release() got executed
    /// </summary>
    //partial void onReleased() {
    //}


    /// <summary>
    /// Called after 'new SortedListWithPropertyNameChild()' transaction is rolled back
    /// </summary>
    //partial void onRollbackItemNew() {
    //}


    /// <summary>
    /// Called after SortedListWithPropertyNameChild.Store() transaction is rolled back
    /// </summary>
    //partial void onRollbackItemStored() {
    //}


    /// <summary>
    /// Called after SortedListWithPropertyNameChild.Update() transaction is rolled back
    /// </summary>
    //partial void onRollbackItemUpdated(SortedListWithPropertyNameChild oldSortedListWithPropertyNameChild) {
    //}


    /// <summary>
    /// Called after SortedListWithPropertyNameChild.Release() transaction is rolled back
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
