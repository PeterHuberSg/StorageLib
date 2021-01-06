using System;
using System.Collections.Generic;
using StorageLib;


namespace TestContext  {


  public partial class SingleChildParentNR: IStorageItemGeneric<SingleChildParentNR> {


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
    partial void onCloned(SingleChildParentNR clone) {
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
    /// Called after SingleChildParentNR.Store() is executed
    /// </summary>
    partial void onStored() {
    }


    /// <summary>
    /// Called before SingleChildParentNR gets written to a CSV file
    /// </summary>
    partial void onCsvWrite() {
    }


    /// <summary>
    /// Called before any property of SingleChildParentNR is updated and before the HasChanged event gets raised
    /// </summary>
    partial void onUpdating(string text, ref bool isCancelled){
   }


    /// <summary>
    /// Called after all properties of SingleChildParentNR are updated, but before the HasChanged event gets raised
    /// </summary>
    partial void onUpdated(SingleChildParentNR old) {
    }


    /// <summary>
    /// Called after an update for SingleChildParentNR is read from a CSV file
    /// </summary>
    partial void onCsvUpdate() {
    }


    /// <summary>
    /// Called after SingleChildParentNR.Release() got executed
    /// </summary>
    partial void onReleased() {
    }


    /// <summary>
    /// Called after 'new SingleChildParentNR()' transaction is rolled back
    /// </summary>
    partial void onRollbackItemNew() {
    }


    /// <summary>
    /// Called after SingleChildParentNR.Store() transaction is rolled back
    /// </summary>
    partial void onRollbackItemStored() {
    }


    /// <summary>
    /// Called after SingleChildParentNR.Update() transaction is rolled back
    /// </summary>
    partial void onRollbackItemUpdated(SingleChildParentNR oldSingleChildParentNR) {
    }


    /// <summary>
    /// Called after SingleChildParentNR.Release() transaction is rolled back
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
