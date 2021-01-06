using System;
using System.Collections.Generic;
using StorageLib;


namespace DataModelSamples  {


  public partial class SingleChild_1_C_Parent: IStorageItemGeneric<SingleChild_1_C_Parent> {


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
    //partial void onCloned(SingleChild_1_C_Parent clone) {
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
    /// Called after SingleChild_1_C_Parent.Store() is executed
    /// </summary>
    //partial void onStored() {
    //}


    /// <summary>
    /// Called before SingleChild_1_C_Parent gets written to a CSV file
    /// </summary>
    //partial void onCsvWrite() {
    //}


    /// <summary>
    /// Called before any property of SingleChild_1_C_Parent is updated and before the HasChanged event gets raised
    /// </summary>
    //partial void onUpdating(string name, ref bool isCancelled){
   //}


    /// <summary>
    /// Called after all properties of SingleChild_1_C_Parent are updated, but before the HasChanged event gets raised
    /// </summary>
    //partial void onUpdated(SingleChild_1_C_Parent old) {
    //}


    /// <summary>
    /// Called after an update for SingleChild_1_C_Parent is read from a CSV file
    /// </summary>
    //partial void onCsvUpdate() {
    //}


    /// <summary>
    /// Called after SingleChild_1_C_Parent.Release() got executed
    /// </summary>
    //partial void onReleased() {
    //}


    /// <summary>
    /// Called after 'new SingleChild_1_C_Parent()' transaction is rolled back
    /// </summary>
    //partial void onRollbackItemNew() {
    //}


    /// <summary>
    /// Called after SingleChild_1_C_Parent.Store() transaction is rolled back
    /// </summary>
    //partial void onRollbackItemStored() {
    //}


    /// <summary>
    /// Called after SingleChild_1_C_Parent.Update() transaction is rolled back
    /// </summary>
    //partial void onRollbackItemUpdated(SingleChild_1_C_Parent oldSingleChild_1_C_Parent) {
    //}


    /// <summary>
    /// Called after SingleChild_1_C_Parent.Release() transaction is rolled back
    /// </summary>
    //partial void onRollbackItemRelease() {
    //}


    /// <summary>
    /// Called after a singleChild_1_C_Child gets added to Child.
    /// </summary>
    //partial void onAddedToChild(SingleChild_1_C_Child singleChild_1_C_Child){
    //}


    /// <summary>
    /// Called after a singleChild_1_C_Child gets removed from Child.
    /// </summary>
    //partial void onRemovedFromChild(SingleChild_1_C_Child singleChild_1_C_Child){
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
