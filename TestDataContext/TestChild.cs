using System;
using System.Collections.Generic;
using StorageLib;


namespace TestContext  {


  public partial class TestChild: IStorageItem<TestChild> {


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
    partial void onCloned(TestChild clone) {
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
    /// Called after TestChild.Store() is executed
    /// </summary>
    partial void onStored() {
    }


    /// <summary>
    /// Called before TestChild gets written to a CSV file
    /// </summary>
    partial void onCsvWrite() {
    }


    /// <summary>
    /// Called before any property of TestChild is updated and before the HasChanged event gets raised
    /// </summary>
    partial void onUpdating(string text, TestParent parent, ref bool isCancelled){
   }


    /// <summary>
    /// Called after all properties of TestChild are updated, but before the HasChanged event gets raised
    /// </summary>
    partial void onUpdated(TestChild old) {
    }


    /// <summary>
    /// Called after an update for TestChild is read from a CSV file
    /// </summary>
    partial void onCsvUpdate() {
    }


    /// <summary>
    /// Called before TestChild.Release() gets executed
    /// </summary>
    partial void onReleasing() {
    }


    /// <summary>
    /// Called after TestChild.Release() got executed
    /// </summary>
    partial void onReleased() {
    }


    /// <summary>
    /// Called after 'new TestChild()' transaction is rolled back
    /// </summary>
    partial void onRollbackItemNew() {
    }


    /// <summary>
    /// Called after TestChild.Store() transaction is rolled back
    /// </summary>
    partial void onRollbackItemStored() {
    }


    /// <summary>
    /// Called after TestChild.Update() transaction is rolled back
    /// </summary>
    partial void onRollbackItemUpdated(TestChild oldTestChild) {
    }


    /// <summary>
    /// Called after TestChild.Release() transaction is rolled back
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
