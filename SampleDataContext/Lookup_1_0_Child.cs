using System;
using System.Collections.Generic;
using StorageLib;


namespace DataModelSamples  {


  public partial class Lookup_1_0_Child: IStorageItemGeneric<Lookup_1_0_Child> {


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
    //partial void onCloned(Lookup_1_0_Child clone) {
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
    /// Called after Lookup_1_0_Child.Store() is executed
    /// </summary>
    //partial void onStored() {
    //}


    /// <summary>
    /// Called before Lookup_1_0_Child gets written to a CSV file
    /// </summary>
    //partial void onCsvWrite() {
    //}


    /// <summary>
    /// Called before any property of Lookup_1_0_Child is updated and before the HasChanged event gets raised
    /// </summary>
    //partial void onUpdating(string name, Lookup_1_0_Parent parent, ref bool isCancelled){
   //}


    /// <summary>
    /// Called after all properties of Lookup_1_0_Child are updated, but before the HasChanged event gets raised
    /// </summary>
    //partial void onUpdated(Lookup_1_0_Child old) {
    //}


    /// <summary>
    /// Called after an update for Lookup_1_0_Child is read from a CSV file
    /// </summary>
    //partial void onCsvUpdate() {
    //}


    /// <summary>
    /// Called after Lookup_1_0_Child.Release() got executed
    /// </summary>
    //partial void onReleased() {
    //}


    /// <summary>
    /// Called after 'new Lookup_1_0_Child()' transaction is rolled back
    /// </summary>
    //partial void onRollbackItemNew() {
    //}


    /// <summary>
    /// Called after Lookup_1_0_Child.Store() transaction is rolled back
    /// </summary>
    //partial void onRollbackItemStored() {
    //}


    /// <summary>
    /// Called after Lookup_1_0_Child.Update() transaction is rolled back
    /// </summary>
    //partial void onRollbackItemUpdated(Lookup_1_0_Child oldLookup_1_0_Child) {
    //}


    /// <summary>
    /// Called after Lookup_1_0_Child.Release() transaction is rolled back
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
