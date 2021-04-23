using System;
using System.Collections.Generic;
using StorageLib;


namespace TestContext  {


  public partial class SingleChildChild: IStorageItem<SingleChildChild> {


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
    partial void onCloned(SingleChildChild clone) {
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
    /// Called after SingleChildChild.Store() is executed
    /// </summary>
    partial void onStored() {
    }


    /// <summary>
    /// Called before SingleChildChild gets written to a CSV file
    /// </summary>
    partial void onCsvWrite() {
    }


    /// <summary>
    /// Called before any property of SingleChildChild is updated and before the HasChanged event gets raised
    /// </summary>
    partial void onUpdating(string text, SingleChildParent parent, SingleChildParentN? parentN, ref bool isCancelled){
   }


    /// <summary>
    /// Called after all properties of SingleChildChild are updated, but before the HasChanged event gets raised
    /// </summary>
    partial void onUpdated(SingleChildChild old) {
    }


    /// <summary>
    /// Called after an update for SingleChildChild is read from a CSV file
    /// </summary>
    partial void onCsvUpdate() {
    }


    /// <summary>
    /// Called before SingleChildChild.Release() gets executed
    /// </summary>
    partial void onReleasing() {
    }


    /// <summary>
    /// Called after SingleChildChild.Release() got executed
    /// </summary>
    partial void onReleased() {
    }


    /// <summary>
    /// Called after 'new SingleChildChild()' transaction is rolled back
    /// </summary>
    partial void onRollbackItemNew() {
    }


    /// <summary>
    /// Called after SingleChildChild.Store() transaction is rolled back
    /// </summary>
    partial void onRollbackItemStored() {
    }


    /// <summary>
    /// Called after SingleChildChild.Update() transaction is rolled back
    /// </summary>
    partial void onRollbackItemUpdated(SingleChildChild oldSingleChildChild) {
    }


    /// <summary>
    /// Called after SingleChildChild.Release() transaction is rolled back
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
