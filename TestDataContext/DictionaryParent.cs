using System;
using System.Collections.Generic;
using StorageLib;


namespace TestContext  {


  public partial class DictionaryParent: IStorageItem<DictionaryParent> {


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
    partial void onCloned(DictionaryParent clone) {
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
    /// Called after DictionaryParent.Store() is executed
    /// </summary>
    partial void onStored() {
    }


    /// <summary>
    /// Called before DictionaryParent gets written to a CSV file
    /// </summary>
    partial void onCsvWrite() {
    }


    /// <summary>
    /// Called before any property of DictionaryParent is updated and before the HasChanged event gets raised
    /// </summary>
    partial void onUpdating(string text, ref bool isCancelled){
   }


    /// <summary>
    /// Called after all properties of DictionaryParent are updated, but before the HasChanged event gets raised
    /// </summary>
    partial void onUpdated(DictionaryParent old) {
    }


    /// <summary>
    /// Called after an update for DictionaryParent is read from a CSV file
    /// </summary>
    partial void onCsvUpdate() {
    }


    /// <summary>
    /// Called after DictionaryParent.Release() got executed
    /// </summary>
    partial void onReleased() {
    }


    /// <summary>
    /// Called after 'new DictionaryParent()' transaction is rolled back
    /// </summary>
    partial void onRollbackItemNew() {
    }


    /// <summary>
    /// Called after DictionaryParent.Store() transaction is rolled back
    /// </summary>
    partial void onRollbackItemStored() {
    }


    /// <summary>
    /// Called after DictionaryParent.Update() transaction is rolled back
    /// </summary>
    partial void onRollbackItemUpdated(DictionaryParent oldDictionaryParent) {
    }


    /// <summary>
    /// Called after DictionaryParent.Release() transaction is rolled back
    /// </summary>
    partial void onRollbackItemRelease() {
    }


    /// <summary>
    /// Called after a dictionaryChild gets added to DictionaryChildren.
    /// </summary>
    partial void onAddedToDictionaryChildren(DictionaryChild dictionaryChild){
    }


    /// <summary>
    /// Called after a dictionaryChild gets removed from DictionaryChildren.
    /// </summary>
    partial void onRemovedFromDictionaryChildren(DictionaryChild dictionaryChild){
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
