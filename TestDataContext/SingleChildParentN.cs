using System;
using System.Collections.Generic;
using StorageLib;


namespace StorageDataContext  {


  public partial class SingleChildParentN: IStorageItemGeneric<SingleChildParentN> {


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
    partial void onCloned(SingleChildParentN clone) {
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
    /// Called after SingleChildParentN.Store() is executed
    /// </summary>
    partial void onStored() {
    }


    /// <summary>
    /// Called before SingleChildParentN gets written to a CSV file
    /// </summary>
    partial void onCsvWrite() {
    }


    /// <summary>
    /// Called before any property of SingleChildParentN is updated and before the HasChanged event gets raised
    /// </summary>
    partial void onUpdating(string text, ref bool isCancelled){
   }


    /// <summary>
    /// Called after all properties of SingleChildParentN are updated, but before the HasChanged event gets raised
    /// </summary>
    partial void onUpdated(SingleChildParentN old) {
    }


    /// <summary>
    /// Called after an update for SingleChildParentN is read from a CSV file
    /// </summary>
    partial void onCsvUpdate() {
    }


    /// <summary>
    /// Called after SingleChildParentN.Release() got executed
    /// </summary>
    partial void onReleased() {
    }


    /// <summary>
    /// Called after 'new SingleChildParentN()' transaction is rolled back
    /// </summary>
    partial void onRollbackItemNew() {
    }


    /// <summary>
    /// Called after SingleChildParentN.Store() transaction is rolled back
    /// </summary>
    partial void onRollbackItemStored() {
    }


    /// <summary>
    /// Called after SingleChildParentN.Update() transaction is rolled back
    /// </summary>
    partial void onRollbackItemUpdated(SingleChildParentN oldSingleChildParentN) {
    }


    /// <summary>
    /// Called after SingleChildParentN.Release() transaction is rolled back
    /// </summary>
    partial void onRollbackItemRelease() {
    }


    /// <summary>
    /// Called after a singleChildChild gets added to Child.
    /// </summary>
    partial void onAddedToChild(SingleChildChild singleChildChild){
    }


    /// <summary>
    /// Called after a singleChildChild gets removed from Child.
    /// </summary>
    partial void onRemovedFromChild(SingleChildChild singleChildChild){
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
