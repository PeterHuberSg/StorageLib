using System;
using System.Collections.Generic;
using StorageLib;


namespace DataModelSamples  {


  public partial class SingleChild_1_C_Child: IStorageItemGeneric<SingleChild_1_C_Child> {


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
    //partial void onCloned(SingleChild_1_C_Child clone) {
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
    /// Called after SingleChild_1_C_Child.Store() is executed
    /// </summary>
    //partial void onStored() {
    //}


    /// <summary>
    /// Called before SingleChild_1_C_Child gets written to a CSV file
    /// </summary>
    //partial void onCsvWrite() {
    //}


    /// <summary>
    /// Called before any property of SingleChild_1_C_Child is updated and before the HasChanged event gets raised
    /// </summary>
    //partial void onUpdating(string name, SingleChild_1_C_Parent parent, ref bool isCancelled){
   //}


    /// <summary>
    /// Called after all properties of SingleChild_1_C_Child are updated, but before the HasChanged event gets raised
    /// </summary>
    //partial void onUpdated(SingleChild_1_C_Child old) {
    //}


    /// <summary>
    /// Called after an update for SingleChild_1_C_Child is read from a CSV file
    /// </summary>
    //partial void onCsvUpdate() {
    //}


    /// <summary>
    /// Called after SingleChild_1_C_Child.Release() got executed
    /// </summary>
    //partial void onReleased() {
    //}


    /// <summary>
    /// Called after 'new SingleChild_1_C_Child()' transaction is rolled back
    /// </summary>
    //partial void onRollbackItemNew() {
    //}


    /// <summary>
    /// Called after SingleChild_1_C_Child.Store() transaction is rolled back
    /// </summary>
    //partial void onRollbackItemStored() {
    //}


    /// <summary>
    /// Called after SingleChild_1_C_Child.Update() transaction is rolled back
    /// </summary>
    //partial void onRollbackItemUpdated(SingleChild_1_C_Child oldSingleChild_1_C_Child) {
    //}


    /// <summary>
    /// Called after SingleChild_1_C_Child.Release() transaction is rolled back
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
