using System;
using System.Collections.Generic;
using StorageLib;


namespace TestContext  {


    /// <summary>
    /// Some comment for PropertyNeedsDictionaryClass
    /// </summary>
  public partial class PropertyNeedsDictionaryClass: IStorageItemGeneric<PropertyNeedsDictionaryClass> {


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
    partial void onCloned(PropertyNeedsDictionaryClass clone) {
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
    /// Called after PropertyNeedsDictionaryClass.Store() is executed
    /// </summary>
    partial void onStored() {
    }


    /// <summary>
    /// Called before PropertyNeedsDictionaryClass gets written to a CSV file
    /// </summary>
    partial void onCsvWrite() {
    }


    /// <summary>
    /// Called before any property of PropertyNeedsDictionaryClass is updated and before the HasChanged event gets raised
    /// </summary>
    partial void onUpdating(
      int idInt, 
      string? idString, 
      string text, 
      string? textNullable, 
      ref bool isCancelled)
   {
   }


    /// <summary>
    /// Called after all properties of PropertyNeedsDictionaryClass are updated, but before the HasChanged event gets raised
    /// </summary>
    partial void onUpdated(PropertyNeedsDictionaryClass old) {
    }


    /// <summary>
    /// Called after an update for PropertyNeedsDictionaryClass is read from a CSV file
    /// </summary>
    partial void onCsvUpdate() {
    }


    /// <summary>
    /// Called after PropertyNeedsDictionaryClass.Release() got executed
    /// </summary>
    partial void onReleased() {
    }


    /// <summary>
    /// Called after 'new PropertyNeedsDictionaryClass()' transaction is rolled back
    /// </summary>
    partial void onRollbackItemNew() {
    }


    /// <summary>
    /// Called after PropertyNeedsDictionaryClass.Store() transaction is rolled back
    /// </summary>
    partial void onRollbackItemStored() {
    }


    /// <summary>
    /// Called after PropertyNeedsDictionaryClass.Update() transaction is rolled back
    /// </summary>
    partial void onRollbackItemUpdated(PropertyNeedsDictionaryClass oldPropertyNeedsDictionaryClass) {
    }


    /// <summary>
    /// Called after PropertyNeedsDictionaryClass.Release() transaction is rolled back
    /// </summary>
    partial void onRollbackItemRelease() {
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
