using System;
using System.Collections.Generic;
using StorageLib;


namespace TestContext  {


  public partial class DictionaryParentN: IStorageItem<DictionaryParentN> {


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
    partial void onCloned(DictionaryParentN clone) {
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
    /// Called after DictionaryParentN.Store() is executed
    /// </summary>
    partial void onStored() {
    }


    /// <summary>
    /// Called before DictionaryParentN gets written to a CSV file
    /// </summary>
    partial void onCsvWrite() {
    }


    /// <summary>
    /// Called before any property of DictionaryParentN is updated and before the HasChanged event gets raised
    /// </summary>
    partial void onUpdating(string text, ref bool isCancelled){
   }


    /// <summary>
    /// Called after all properties of DictionaryParentN are updated, but before the HasChanged event gets raised
    /// </summary>
    partial void onUpdated(DictionaryParentN old) {
    }


    /// <summary>
    /// Called after an update for DictionaryParentN is read from a CSV file
    /// </summary>
    partial void onCsvUpdate() {
    }


    /// <summary>
    /// Called after DictionaryParentN.Release() got executed
    /// </summary>
    partial void onReleased() {
    }


    /// <summary>
    /// Called after 'new DictionaryParentN()' transaction is rolled back
    /// </summary>
    partial void onRollbackItemNew() {
    }


    /// <summary>
    /// Called after DictionaryParentN.Store() transaction is rolled back
    /// </summary>
    partial void onRollbackItemStored() {
    }


    /// <summary>
    /// Called after DictionaryParentN.Update() transaction is rolled back
    /// </summary>
    partial void onRollbackItemUpdated(DictionaryParentN oldDictionaryParentN) {
    }


    /// <summary>
    /// Called after DictionaryParentN.Release() transaction is rolled back
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
