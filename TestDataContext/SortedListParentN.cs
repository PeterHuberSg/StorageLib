using System;
using System.Collections.Generic;
using StorageLib;


namespace TestContext  {


  public partial class SortedListParentN: IStorageItem<SortedListParentN> {


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
    partial void onCloned(SortedListParentN clone) {
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
    /// Called after SortedListParentN.Store() is executed
    /// </summary>
    partial void onStored() {
    }


    /// <summary>
    /// Called before SortedListParentN gets written to a CSV file
    /// </summary>
    partial void onCsvWrite() {
    }


    /// <summary>
    /// Called before any property of SortedListParentN is updated and before the HasChanged event gets raised
    /// </summary>
    partial void onUpdating(string text, ref bool isCancelled){
   }


    /// <summary>
    /// Called after all properties of SortedListParentN are updated, but before the HasChanged event gets raised
    /// </summary>
    partial void onUpdated(SortedListParentN old) {
    }


    /// <summary>
    /// Called after an update for SortedListParentN is read from a CSV file
    /// </summary>
    partial void onCsvUpdate() {
    }


    /// <summary>
    /// Called after SortedListParentN.Release() got executed
    /// </summary>
    partial void onReleased() {
    }


    /// <summary>
    /// Called after 'new SortedListParentN()' transaction is rolled back
    /// </summary>
    partial void onRollbackItemNew() {
    }


    /// <summary>
    /// Called after SortedListParentN.Store() transaction is rolled back
    /// </summary>
    partial void onRollbackItemStored() {
    }


    /// <summary>
    /// Called after SortedListParentN.Update() transaction is rolled back
    /// </summary>
    partial void onRollbackItemUpdated(SortedListParentN oldSortedListParentN) {
    }


    /// <summary>
    /// Called after SortedListParentN.Release() transaction is rolled back
    /// </summary>
    partial void onRollbackItemRelease() {
    }


    /// <summary>
    /// Called after a sortedListChild gets added to SortedListChildren.
    /// </summary>
    partial void onAddedToSortedListChildren(SortedListChild sortedListChild){
    }


    /// <summary>
    /// Called after a sortedListChild gets removed from SortedListChildren.
    /// </summary>
    partial void onRemovedFromSortedListChildren(SortedListChild sortedListChild){
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
