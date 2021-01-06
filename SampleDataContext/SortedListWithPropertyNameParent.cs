using System;
using System.Collections.Generic;
using StorageLib;


namespace DataModelSamples  {


  public partial class SortedListWithPropertyNameParent: IStorageItemGeneric<SortedListWithPropertyNameParent> {


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
    //partial void onCloned(SortedListWithPropertyNameParent clone) {
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
    /// Called after SortedListWithPropertyNameParent.Store() is executed
    /// </summary>
    //partial void onStored() {
    //}


    /// <summary>
    /// Called before SortedListWithPropertyNameParent gets written to a CSV file
    /// </summary>
    //partial void onCsvWrite() {
    //}


    /// <summary>
    /// Called before any property of SortedListWithPropertyNameParent is updated and before the HasChanged event gets raised
    /// </summary>
    //partial void onUpdating(string name, ref bool isCancelled){
   //}


    /// <summary>
    /// Called after all properties of SortedListWithPropertyNameParent are updated, but before the HasChanged event gets raised
    /// </summary>
    //partial void onUpdated(SortedListWithPropertyNameParent old) {
    //}


    /// <summary>
    /// Called after an update for SortedListWithPropertyNameParent is read from a CSV file
    /// </summary>
    //partial void onCsvUpdate() {
    //}


    /// <summary>
    /// Called after SortedListWithPropertyNameParent.Release() got executed
    /// </summary>
    //partial void onReleased() {
    //}


    /// <summary>
    /// Called after 'new SortedListWithPropertyNameParent()' transaction is rolled back
    /// </summary>
    //partial void onRollbackItemNew() {
    //}


    /// <summary>
    /// Called after SortedListWithPropertyNameParent.Store() transaction is rolled back
    /// </summary>
    //partial void onRollbackItemStored() {
    //}


    /// <summary>
    /// Called after SortedListWithPropertyNameParent.Update() transaction is rolled back
    /// </summary>
    //partial void onRollbackItemUpdated(SortedListWithPropertyNameParent oldSortedListWithPropertyNameParent) {
    //}


    /// <summary>
    /// Called after SortedListWithPropertyNameParent.Release() transaction is rolled back
    /// </summary>
    //partial void onRollbackItemRelease() {
    //}


    /// <summary>
    /// Called after a sortedListWithPropertyNameChild gets added to Children.
    /// </summary>
    //partial void onAddedToChildren(SortedListWithPropertyNameChild sortedListWithPropertyNameChild){
    //}


    /// <summary>
    /// Called after a sortedListWithPropertyNameChild gets removed from Children.
    /// </summary>
    //partial void onRemovedFromChildren(SortedListWithPropertyNameChild sortedListWithPropertyNameChild){
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
