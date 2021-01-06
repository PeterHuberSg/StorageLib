using System;
using System.Collections.Generic;
using StorageLib;


namespace DataModelSamples  {


  public partial class ListC_MCParent: IStorageItemGeneric<ListC_MCParent> {


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
    //partial void onCloned(ListC_MCParent clone) {
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
    /// Called after ListC_MCParent.Store() is executed
    /// </summary>
    //partial void onStored() {
    //}


    /// <summary>
    /// Called before ListC_MCParent gets written to a CSV file
    /// </summary>
    //partial void onCsvWrite() {
    //}


    /// <summary>
    /// Called before any property of ListC_MCParent is updated and before the HasChanged event gets raised
    /// </summary>
    //partial void onUpdating(string name, ref bool isCancelled){
   //}


    /// <summary>
    /// Called after all properties of ListC_MCParent are updated, but before the HasChanged event gets raised
    /// </summary>
    //partial void onUpdated(ListC_MCParent old) {
    //}


    /// <summary>
    /// Called after an update for ListC_MCParent is read from a CSV file
    /// </summary>
    //partial void onCsvUpdate() {
    //}


    /// <summary>
    /// Called after ListC_MCParent.Release() got executed
    /// </summary>
    //partial void onReleased() {
    //}


    /// <summary>
    /// Called after 'new ListC_MCParent()' transaction is rolled back
    /// </summary>
    //partial void onRollbackItemNew() {
    //}


    /// <summary>
    /// Called after ListC_MCParent.Store() transaction is rolled back
    /// </summary>
    //partial void onRollbackItemStored() {
    //}


    /// <summary>
    /// Called after ListC_MCParent.Update() transaction is rolled back
    /// </summary>
    //partial void onRollbackItemUpdated(ListC_MCParent oldListC_MCParent) {
    //}


    /// <summary>
    /// Called after ListC_MCParent.Release() transaction is rolled back
    /// </summary>
    //partial void onRollbackItemRelease() {
    //}


    /// <summary>
    /// Called after a listC_MCChild gets added to Children.
    /// </summary>
    //partial void onAddedToChildren(ListC_MCChild listC_MCChild){
    //}


    /// <summary>
    /// Called after a listC_MCChild gets removed from Children.
    /// </summary>
    //partial void onRemovedFromChildren(ListC_MCChild listC_MCChild){
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
