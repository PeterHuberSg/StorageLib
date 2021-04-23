using System;
using System.Collections.Generic;
using StorageLib;


namespace TestContext  {


  public partial class SingleChildParentR: IStorageItem<SingleChildParentR> {


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
    partial void onCloned(SingleChildParentR clone) {
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
    /// Called after SingleChildParentR.Store() is executed
    /// </summary>
    partial void onStored() {
    }


    /// <summary>
    /// Called before SingleChildParentR gets written to a CSV file
    /// </summary>
    partial void onCsvWrite() {
    }


    /// <summary>
    /// Called before any property of SingleChildParentR is updated and before the HasChanged event gets raised
    /// </summary>
    partial void onUpdating(string text, ref bool isCancelled){
   }


    /// <summary>
    /// Called after all properties of SingleChildParentR are updated, but before the HasChanged event gets raised
    /// </summary>
    partial void onUpdated(SingleChildParentR old) {
    }


    /// <summary>
    /// Called after an update for SingleChildParentR is read from a CSV file
    /// </summary>
    partial void onCsvUpdate() {
    }


    /// <summary>
    /// Called before SingleChildParentR.Release() gets executed
    /// </summary>
    partial void onReleasing() {
    }


    /// <summary>
    /// Called after SingleChildParentR.Release() got executed
    /// </summary>
    partial void onReleased() {
    }


    /// <summary>
    /// Called after 'new SingleChildParentR()' transaction is rolled back
    /// </summary>
    partial void onRollbackItemNew() {
    }


    /// <summary>
    /// Called after SingleChildParentR.Store() transaction is rolled back
    /// </summary>
    partial void onRollbackItemStored() {
    }


    /// <summary>
    /// Called after SingleChildParentR.Update() transaction is rolled back
    /// </summary>
    partial void onRollbackItemUpdated(SingleChildParentR oldSingleChildParentR) {
    }


    /// <summary>
    /// Called after SingleChildParentR.Release() transaction is rolled back
    /// </summary>
    partial void onRollbackItemRelease() {
    }


    /// <summary>
    /// Called after a singleChildChild gets added to Child.
    /// </summary>
    partial void onAddedToChild(SingleChildChild singleChildChild){
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
