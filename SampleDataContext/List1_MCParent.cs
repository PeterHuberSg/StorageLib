using System;
using System.Collections.Generic;
using StorageLib;


namespace DataModelSamples  {


  public partial class List1_MCParent: IStorageItemGeneric<List1_MCParent> {


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
    //partial void onCloned(List1_MCParent clone) {
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
    /// Called after List1_MCParent.Store() is executed
    /// </summary>
    //partial void onStored() {
    //}


    /// <summary>
    /// Called before List1_MCParent gets written to a CSV file
    /// </summary>
    //partial void onCsvWrite() {
    //}


    /// <summary>
    /// Called before any property of List1_MCParent is updated and before the HasChanged event gets raised
    /// </summary>
    //partial void onUpdating(string name, ref bool isCancelled){
   //}


    /// <summary>
    /// Called after all properties of List1_MCParent are updated, but before the HasChanged event gets raised
    /// </summary>
    //partial void onUpdated(List1_MCParent old) {
    //}


    /// <summary>
    /// Called after an update for List1_MCParent is read from a CSV file
    /// </summary>
    //partial void onCsvUpdate() {
    //}


    /// <summary>
    /// Called after List1_MCParent.Release() got executed
    /// </summary>
    //partial void onReleased() {
    //}


    /// <summary>
    /// Called after 'new List1_MCParent()' transaction is rolled back
    /// </summary>
    //partial void onRollbackItemNew() {
    //}


    /// <summary>
    /// Called after List1_MCParent.Store() transaction is rolled back
    /// </summary>
    //partial void onRollbackItemStored() {
    //}


    /// <summary>
    /// Called after List1_MCParent.Update() transaction is rolled back
    /// </summary>
    //partial void onRollbackItemUpdated(List1_MCParent oldList1_MCParent) {
    //}


    /// <summary>
    /// Called after List1_MCParent.Release() transaction is rolled back
    /// </summary>
    //partial void onRollbackItemRelease() {
    //}


    /// <summary>
    /// Called after a list1_MCChild gets added to Children.
    /// </summary>
    //partial void onAddedToChildren(List1_MCChild list1_MCChild){
    //}


    /// <summary>
    /// Called after a list1_MCChild gets removed from Children.
    /// </summary>
    //partial void onRemovedFromChildren(List1_MCChild list1_MCChild){
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
