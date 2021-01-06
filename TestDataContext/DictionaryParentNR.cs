using System;
using System.Collections.Generic;
using StorageLib;


namespace TestContext  {


  public partial class DictionaryParentNR: IStorageItemGeneric<DictionaryParentNR> {


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
    partial void onCloned(DictionaryParentNR clone) {
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
    /// Called after DictionaryParentNR.Store() is executed
    /// </summary>
    partial void onStored() {
    }


    /// <summary>
    /// Called before DictionaryParentNR gets written to a CSV file
    /// </summary>
    partial void onCsvWrite() {
    }


    /// <summary>
    /// Called before any property of DictionaryParentNR is updated and before the HasChanged event gets raised
    /// </summary>
    partial void onUpdating(string text, ref bool isCancelled){
   }


    /// <summary>
    /// Called after all properties of DictionaryParentNR are updated, but before the HasChanged event gets raised
    /// </summary>
    partial void onUpdated(DictionaryParentNR old) {
    }


    /// <summary>
    /// Called after an update for DictionaryParentNR is read from a CSV file
    /// </summary>
    partial void onCsvUpdate() {
    }


    /// <summary>
    /// Called after DictionaryParentNR.Release() got executed
    /// </summary>
    partial void onReleased() {
    }


    /// <summary>
    /// Called after 'new DictionaryParentNR()' transaction is rolled back
    /// </summary>
    partial void onRollbackItemNew() {
    }


    /// <summary>
    /// Called after DictionaryParentNR.Store() transaction is rolled back
    /// </summary>
    partial void onRollbackItemStored() {
    }


    /// <summary>
    /// Called after DictionaryParentNR.Update() transaction is rolled back
    /// </summary>
    partial void onRollbackItemUpdated(DictionaryParentNR oldDictionaryParentNR) {
    }


    /// <summary>
    /// Called after DictionaryParentNR.Release() transaction is rolled back
    /// </summary>
    partial void onRollbackItemRelease() {
    }


    /// <summary>
    /// Called after a dictionaryChild gets added to DictionaryChidren.
    /// </summary>
    partial void onAddedToDictionaryChidren(DictionaryChild dictionaryChild){
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
