using System;
using System.Collections.Generic;
using StorageLib;


namespace DataModelSamples  {


  public partial class DefaultValuePropertyClass: IStorageItem<DefaultValuePropertyClass> {


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
    //partial void onCloned(DefaultValuePropertyClass clone) {
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
    /// Called after DefaultValuePropertyClass.Store() is executed
    /// </summary>
    //partial void onStored() {
    //}


    /// <summary>
    /// Called before DefaultValuePropertyClass gets written to a CSV file
    /// </summary>
    //partial void onCsvWrite() {
    //}


    /// <summary>
    /// Called before any property of DefaultValuePropertyClass is updated and before the HasChanged event gets raised
    /// </summary>
    //partial void onUpdating(string name, string defaultValueProperty, ref bool isCancelled){
   //}


    /// <summary>
    /// Called after all properties of DefaultValuePropertyClass are updated, but before the HasChanged event gets raised
    /// </summary>
    //partial void onUpdated(DefaultValuePropertyClass old) {
    //}


    /// <summary>
    /// Called after an update for DefaultValuePropertyClass is read from a CSV file
    /// </summary>
    //partial void onCsvUpdate() {
    //}


    /// <summary>
    /// Called after DefaultValuePropertyClass.Release() got executed
    /// </summary>
    //partial void onReleased() {
    //}


    /// <summary>
    /// Called after 'new DefaultValuePropertyClass()' transaction is rolled back
    /// </summary>
    //partial void onRollbackItemNew() {
    //}


    /// <summary>
    /// Called after DefaultValuePropertyClass.Store() transaction is rolled back
    /// </summary>
    //partial void onRollbackItemStored() {
    //}


    /// <summary>
    /// Called after DefaultValuePropertyClass.Update() transaction is rolled back
    /// </summary>
    //partial void onRollbackItemUpdated(DefaultValuePropertyClass oldDefaultValuePropertyClass) {
    //}


    /// <summary>
    /// Called after DefaultValuePropertyClass.Release() transaction is rolled back
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
