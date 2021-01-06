using System;
using System.Collections.Generic;
using StorageLib;


namespace DataModelSamples  {


  public partial class ListC_MCChild: IStorageItemGeneric<ListC_MCChild> {


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
    //partial void onCloned(ListC_MCChild clone) {
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
    /// Called after ListC_MCChild.Store() is executed
    /// </summary>
    //partial void onStored() {
    //}


    /// <summary>
    /// Called before ListC_MCChild gets written to a CSV file
    /// </summary>
    //partial void onCsvWrite() {
    //}


    /// <summary>
    /// Called before any property of ListC_MCChild is updated and before the HasChanged event gets raised
    /// </summary>
    //partial void onUpdating(DateTime date, ListC_MCParent? parent, ref bool isCancelled){
   //}


    /// <summary>
    /// Called after all properties of ListC_MCChild are updated, but before the HasChanged event gets raised
    /// </summary>
    //partial void onUpdated(ListC_MCChild old) {
    //}


    /// <summary>
    /// Called after an update for ListC_MCChild is read from a CSV file
    /// </summary>
    //partial void onCsvUpdate() {
    //}


    /// <summary>
    /// Called after ListC_MCChild.Release() got executed
    /// </summary>
    //partial void onReleased() {
    //}


    /// <summary>
    /// Called after 'new ListC_MCChild()' transaction is rolled back
    /// </summary>
    //partial void onRollbackItemNew() {
    //}


    /// <summary>
    /// Called after ListC_MCChild.Store() transaction is rolled back
    /// </summary>
    //partial void onRollbackItemStored() {
    //}


    /// <summary>
    /// Called after ListC_MCChild.Update() transaction is rolled back
    /// </summary>
    //partial void onRollbackItemUpdated(ListC_MCChild oldListC_MCChild) {
    //}


    /// <summary>
    /// Called after ListC_MCChild.Release() transaction is rolled back
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
