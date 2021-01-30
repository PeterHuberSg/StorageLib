using System;
using System.Collections.Generic;
using StorageLib;


namespace TestContext  {


    /// <summary>
    /// Example of a child with a none nullable and a nullable lookup parent. The child maintains links
    /// to its parents, but the parents don't have children collections.
    /// </summary>
  public partial class LookupChild: IStorageItem<LookupChild> {


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
    partial void onCloned(LookupChild clone) {
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
    /// Called after LookupChild.Store() is executed
    /// </summary>
    partial void onStored() {
    }


    /// <summary>
    /// Called before LookupChild gets written to a CSV file
    /// </summary>
    partial void onCsvWrite() {
    }


    /// <summary>
    /// Called before any property of LookupChild is updated and before the HasChanged event gets raised
    /// </summary>
    partial void onUpdating(string text, LookupParent parent, LookupParentN? parentN, ref bool isCancelled){
   }


    /// <summary>
    /// Called after all properties of LookupChild are updated, but before the HasChanged event gets raised
    /// </summary>
    partial void onUpdated(LookupChild old) {
    }


    /// <summary>
    /// Called after an update for LookupChild is read from a CSV file
    /// </summary>
    partial void onCsvUpdate() {
    }


    /// <summary>
    /// Called after LookupChild.Release() got executed
    /// </summary>
    partial void onReleased() {
    }


    /// <summary>
    /// Called after 'new LookupChild()' transaction is rolled back
    /// </summary>
    partial void onRollbackItemNew() {
    }


    /// <summary>
    /// Called after LookupChild.Store() transaction is rolled back
    /// </summary>
    partial void onRollbackItemStored() {
    }


    /// <summary>
    /// Called after LookupChild.Update() transaction is rolled back
    /// </summary>
    partial void onRollbackItemUpdated(LookupChild oldLookupChild) {
    }


    /// <summary>
    /// Called after LookupChild.Release() transaction is rolled back
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
