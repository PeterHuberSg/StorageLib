using System;
using System.Collections.Generic;
using StorageLib;


namespace TestContext  {


  public partial class ListParentN: IStorageItemGeneric<ListParentN> {


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
    partial void onCloned(ListParentN clone) {
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
    /// Called after ListParentN.Store() is executed
    /// </summary>
    partial void onStored() {
    }


    /// <summary>
    /// Called before ListParentN gets written to a CSV file
    /// </summary>
    partial void onCsvWrite() {
    }


    /// <summary>
    /// Called before any property of ListParentN is updated and before the HasChanged event gets raised
    /// </summary>
    partial void onUpdating(string text, ref bool isCancelled){
   }


    /// <summary>
    /// Called after all properties of ListParentN are updated, but before the HasChanged event gets raised
    /// </summary>
    partial void onUpdated(ListParentN old) {
    }


    /// <summary>
    /// Called after an update for ListParentN is read from a CSV file
    /// </summary>
    partial void onCsvUpdate() {
    }


    /// <summary>
    /// Called after ListParentN.Release() got executed
    /// </summary>
    partial void onReleased() {
    }


    /// <summary>
    /// Called after 'new ListParentN()' transaction is rolled back
    /// </summary>
    partial void onRollbackItemNew() {
    }


    /// <summary>
    /// Called after ListParentN.Store() transaction is rolled back
    /// </summary>
    partial void onRollbackItemStored() {
    }


    /// <summary>
    /// Called after ListParentN.Update() transaction is rolled back
    /// </summary>
    partial void onRollbackItemUpdated(ListParentN oldListParentN) {
    }


    /// <summary>
    /// Called after ListParentN.Release() transaction is rolled back
    /// </summary>
    partial void onRollbackItemRelease() {
    }


    /// <summary>
    /// Called after a listChild gets added to Children.
    /// </summary>
    partial void onAddedToChildren(ListChild listChild){
    }


    /// <summary>
    /// Called after a listChild gets removed from Children.
    /// </summary>
    partial void onRemovedFromChildren(ListChild listChild){
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
