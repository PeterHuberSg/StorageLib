using System;
using System.Collections.Generic;
using StorageLib;


namespace TestContext  {


  public partial class ListChild: IStorageItem<ListChild> {


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
    partial void onCloned(ListChild clone) {
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
    /// Called after ListChild.Store() is executed
    /// </summary>
    partial void onStored() {
    }


    /// <summary>
    /// Called before ListChild gets written to a CSV file
    /// </summary>
    partial void onCsvWrite() {
    }


    /// <summary>
    /// Called before any property of ListChild is updated and before the HasChanged event gets raised
    /// </summary>
    partial void onUpdating(string text, ListParent parent, ListParentN? parentN, ref bool isCancelled){
   }


    /// <summary>
    /// Called after all properties of ListChild are updated, but before the HasChanged event gets raised
    /// </summary>
    partial void onUpdated(ListChild old) {
    }


    /// <summary>
    /// Called after an update for ListChild is read from a CSV file
    /// </summary>
    partial void onCsvUpdate() {
    }


    /// <summary>
    /// Called before ListChild.Release() gets executed
    /// </summary>
    partial void onReleasing() {
    }


    /// <summary>
    /// Called after ListChild.Release() got executed
    /// </summary>
    partial void onReleased() {
    }


    /// <summary>
    /// Called after 'new ListChild()' transaction is rolled back
    /// </summary>
    partial void onRollbackItemNew() {
    }


    /// <summary>
    /// Called after ListChild.Store() transaction is rolled back
    /// </summary>
    partial void onRollbackItemStored() {
    }


    /// <summary>
    /// Called after ListChild.Update() transaction is rolled back
    /// </summary>
    partial void onRollbackItemUpdated(ListChild oldListChild) {
    }


    /// <summary>
    /// Called after ListChild.Release() transaction is rolled back
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
