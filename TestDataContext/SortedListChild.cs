using System;
using System.Collections.Generic;
using StorageLib;


namespace TestContext  {


  public partial class SortedListChild: IStorageItem<SortedListChild> {


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
    partial void onCloned(SortedListChild clone) {
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
    /// Called after SortedListChild.Store() is executed
    /// </summary>
    partial void onStored() {
    }


    /// <summary>
    /// Called before SortedListChild gets written to a CSV file
    /// </summary>
    partial void onCsvWrite() {
    }


    /// <summary>
    /// Called before any property of SortedListChild is updated and before the HasChanged event gets raised
    /// </summary>
    partial void onUpdating(string text, SortedListParent parent, SortedListParentN? parentN, ref bool isCancelled){
   }


    /// <summary>
    /// Called after all properties of SortedListChild are updated, but before the HasChanged event gets raised
    /// </summary>
    partial void onUpdated(SortedListChild old) {
    }


    /// <summary>
    /// Called after an update for SortedListChild is read from a CSV file
    /// </summary>
    partial void onCsvUpdate() {
    }


    /// <summary>
    /// Called before SortedListChild.Release() gets executed
    /// </summary>
    partial void onReleasing() {
    }


    /// <summary>
    /// Called after SortedListChild.Release() got executed
    /// </summary>
    partial void onReleased() {
    }


    /// <summary>
    /// Called after 'new SortedListChild()' transaction is rolled back
    /// </summary>
    partial void onRollbackItemNew() {
    }


    /// <summary>
    /// Called after SortedListChild.Store() transaction is rolled back
    /// </summary>
    partial void onRollbackItemStored() {
    }


    /// <summary>
    /// Called after SortedListChild.Update() transaction is rolled back
    /// </summary>
    partial void onRollbackItemUpdated(SortedListChild oldSortedListChild) {
    }


    /// <summary>
    /// Called after SortedListChild.Release() transaction is rolled back
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
