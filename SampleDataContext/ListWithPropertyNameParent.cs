using System;
using System.Collections.Generic;
using StorageLib;


namespace DataModelSamples  {


  public partial class ListWithPropertyNameParent: IStorageItemGeneric<ListWithPropertyNameParent> {


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
    //partial void onCloned(ListWithPropertyNameParent clone) {
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
    /// Called after ListWithPropertyNameParent.Store() is executed
    /// </summary>
    //partial void onStored() {
    //}


    /// <summary>
    /// Called before ListWithPropertyNameParent gets written to a CSV file
    /// </summary>
    //partial void onCsvWrite() {
    //}


    /// <summary>
    /// Called before any property of ListWithPropertyNameParent is updated and before the HasChanged event gets raised
    /// </summary>
    //partial void onUpdating(string name, ref bool isCancelled){
   //}


    /// <summary>
    /// Called after all properties of ListWithPropertyNameParent are updated, but before the HasChanged event gets raised
    /// </summary>
    //partial void onUpdated(ListWithPropertyNameParent old) {
    //}


    /// <summary>
    /// Called after an update for ListWithPropertyNameParent is read from a CSV file
    /// </summary>
    //partial void onCsvUpdate() {
    //}


    /// <summary>
    /// Called after ListWithPropertyNameParent.Release() got executed
    /// </summary>
    //partial void onReleased() {
    //}


    /// <summary>
    /// Called after 'new ListWithPropertyNameParent()' transaction is rolled back
    /// </summary>
    //partial void onRollbackItemNew() {
    //}


    /// <summary>
    /// Called after ListWithPropertyNameParent.Store() transaction is rolled back
    /// </summary>
    //partial void onRollbackItemStored() {
    //}


    /// <summary>
    /// Called after ListWithPropertyNameParent.Update() transaction is rolled back
    /// </summary>
    //partial void onRollbackItemUpdated(ListWithPropertyNameParent oldListWithPropertyNameParent) {
    //}


    /// <summary>
    /// Called after ListWithPropertyNameParent.Release() transaction is rolled back
    /// </summary>
    //partial void onRollbackItemRelease() {
    //}


    /// <summary>
    /// Called after a listWithPropertyNameChild gets added to Children.
    /// </summary>
    //partial void onAddedToChildren(ListWithPropertyNameChild listWithPropertyNameChild){
    //}


    /// <summary>
    /// Called after a listWithPropertyNameChild gets removed from Children.
    /// </summary>
    //partial void onRemovedFromChildren(ListWithPropertyNameChild listWithPropertyNameChild){
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
