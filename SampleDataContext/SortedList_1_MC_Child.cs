using System;
using System.Collections.Generic;
using StorageLib;


namespace DataModelSamples  {


  public partial class SortedList_1_MC_Child: IStorageItemGeneric<SortedList_1_MC_Child> {


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
    //partial void onCloned(SortedList_1_MC_Child clone) {
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
    /// Called after SortedList_1_MC_Child.Store() is executed
    /// </summary>
    //partial void onStored() {
    //}


    /// <summary>
    /// Called before SortedList_1_MC_Child gets written to a CSV file
    /// </summary>
    //partial void onCsvWrite() {
    //}


    /// <summary>
    /// Called before any property of SortedList_1_MC_Child is updated and before the HasChanged event gets raised
    /// </summary>
    //partial void onUpdating(string name, SortedList_1_MC_Parent parent, ref bool isCancelled){
   //}


    /// <summary>
    /// Called after all properties of SortedList_1_MC_Child are updated, but before the HasChanged event gets raised
    /// </summary>
    //partial void onUpdated(SortedList_1_MC_Child old) {
    //}


    /// <summary>
    /// Called after an update for SortedList_1_MC_Child is read from a CSV file
    /// </summary>
    //partial void onCsvUpdate() {
    //}


    /// <summary>
    /// Called after SortedList_1_MC_Child.Release() got executed
    /// </summary>
    //partial void onReleased() {
    //}


    /// <summary>
    /// Called after 'new SortedList_1_MC_Child()' transaction is rolled back
    /// </summary>
    //partial void onRollbackItemNew() {
    //}


    /// <summary>
    /// Called after SortedList_1_MC_Child.Store() transaction is rolled back
    /// </summary>
    //partial void onRollbackItemStored() {
    //}


    /// <summary>
    /// Called after SortedList_1_MC_Child.Update() transaction is rolled back
    /// </summary>
    //partial void onRollbackItemUpdated(SortedList_1_MC_Child oldSortedList_1_MC_Child) {
    //}


    /// <summary>
    /// Called after SortedList_1_MC_Child.Release() transaction is rolled back
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
