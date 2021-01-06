using System;
using System.Collections.Generic;
using StorageLib;


namespace StorageDataContext  {


    /// <summary>
    /// Example with private constructor.
    /// </summary>
  public partial class PrivateConstructor: IStorageItemGeneric<PrivateConstructor> {


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
    partial void onCloned(PrivateConstructor clone) {
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
    /// Called after PrivateConstructor.Store() is executed
    /// </summary>
    partial void onStored() {
    }


    /// <summary>
    /// Called before PrivateConstructor gets written to a CSV file
    /// </summary>
    partial void onCsvWrite() {
    }


    /// <summary>
    /// Called before any property of PrivateConstructor is updated and before the HasChanged event gets raised
    /// </summary>
    partial void onUpdating(string text, ref bool isCancelled){
   }


    /// <summary>
    /// Called after all properties of PrivateConstructor are updated, but before the HasChanged event gets raised
    /// </summary>
    partial void onUpdated(PrivateConstructor old) {
    }


    /// <summary>
    /// Called after an update for PrivateConstructor is read from a CSV file
    /// </summary>
    partial void onCsvUpdate() {
    }


    /// <summary>
    /// Called after PrivateConstructor.Release() got executed
    /// </summary>
    partial void onReleased() {
    }


    /// <summary>
    /// Called after 'new PrivateConstructor()' transaction is rolled back
    /// </summary>
    partial void onRollbackItemNew() {
    }


    /// <summary>
    /// Called after PrivateConstructor.Store() transaction is rolled back
    /// </summary>
    partial void onRollbackItemStored() {
    }


    /// <summary>
    /// Called after PrivateConstructor.Update() transaction is rolled back
    /// </summary>
    partial void onRollbackItemUpdated(PrivateConstructor oldPrivateConstructor) {
    }


    /// <summary>
    /// Called after PrivateConstructor.Release() transaction is rolled back
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
