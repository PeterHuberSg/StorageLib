using System;
using System.Collections.Generic;
using StorageLib;


namespace DataModelSamples  {


    /// <summary>
    /// Directory of all NeedsDictionaryClasss by Name
    /// </summary>
  public partial class NeedsDictionaryLowerCaseClass: IStorageItemGeneric<NeedsDictionaryLowerCaseClass> {


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
    //partial void onCloned(NeedsDictionaryLowerCaseClass clone) {
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
    /// Called after NeedsDictionaryLowerCaseClass.Store() is executed
    /// </summary>
    //partial void onStored() {
    //}


    /// <summary>
    /// Called before NeedsDictionaryLowerCaseClass gets written to a CSV file
    /// </summary>
    //partial void onCsvWrite() {
    //}


    /// <summary>
    /// Called before any property of NeedsDictionaryLowerCaseClass is updated and before the HasChanged event gets raised
    /// </summary>
    //partial void onUpdating(string name, string address, ref bool isCancelled){
   //}


    /// <summary>
    /// Called after all properties of NeedsDictionaryLowerCaseClass are updated, but before the HasChanged event gets raised
    /// </summary>
    //partial void onUpdated(NeedsDictionaryLowerCaseClass old) {
    //}


    /// <summary>
    /// Called after an update for NeedsDictionaryLowerCaseClass is read from a CSV file
    /// </summary>
    //partial void onCsvUpdate() {
    //}


    /// <summary>
    /// Called after NeedsDictionaryLowerCaseClass.Release() got executed
    /// </summary>
    //partial void onReleased() {
    //}


    /// <summary>
    /// Called after 'new NeedsDictionaryLowerCaseClass()' transaction is rolled back
    /// </summary>
    //partial void onRollbackItemNew() {
    //}


    /// <summary>
    /// Called after NeedsDictionaryLowerCaseClass.Store() transaction is rolled back
    /// </summary>
    //partial void onRollbackItemStored() {
    //}


    /// <summary>
    /// Called after NeedsDictionaryLowerCaseClass.Update() transaction is rolled back
    /// </summary>
    //partial void onRollbackItemUpdated(NeedsDictionaryLowerCaseClass oldNeedsDictionaryLowerCaseClass) {
    //}


    /// <summary>
    /// Called after NeedsDictionaryLowerCaseClass.Release() transaction is rolled back
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
