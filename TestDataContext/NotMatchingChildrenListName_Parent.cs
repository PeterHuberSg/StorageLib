using System;
using System.Collections.Generic;
using StorageLib;


namespace TestContext  {


    /// <summary>
    /// Example where the parent's List for it's children is not the plural of the child type type. 
    /// </summary>
  public partial class NotMatchingChildrenListName_Parent: IStorageItem<NotMatchingChildrenListName_Parent> {


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
    partial void onCloned(NotMatchingChildrenListName_Parent clone) {
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
    /// Called after NotMatchingChildrenListName_Parent.Store() is executed
    /// </summary>
    partial void onStored() {
    }


    /// <summary>
    /// Called before NotMatchingChildrenListName_Parent gets written to a CSV file
    /// </summary>
    partial void onCsvWrite() {
    }


    /// <summary>
    /// Called before any property of NotMatchingChildrenListName_Parent is updated and before the HasChanged event gets raised
    /// </summary>
    partial void onUpdating(string text, ref bool isCancelled){
   }


    /// <summary>
    /// Called after all properties of NotMatchingChildrenListName_Parent are updated, but before the HasChanged event gets raised
    /// </summary>
    partial void onUpdated(NotMatchingChildrenListName_Parent old) {
    }


    /// <summary>
    /// Called after an update for NotMatchingChildrenListName_Parent is read from a CSV file
    /// </summary>
    partial void onCsvUpdate() {
    }


    /// <summary>
    /// Called after NotMatchingChildrenListName_Parent.Release() got executed
    /// </summary>
    partial void onReleased() {
    }


    /// <summary>
    /// Called after 'new NotMatchingChildrenListName_Parent()' transaction is rolled back
    /// </summary>
    partial void onRollbackItemNew() {
    }


    /// <summary>
    /// Called after NotMatchingChildrenListName_Parent.Store() transaction is rolled back
    /// </summary>
    partial void onRollbackItemStored() {
    }


    /// <summary>
    /// Called after NotMatchingChildrenListName_Parent.Update() transaction is rolled back
    /// </summary>
    partial void onRollbackItemUpdated(NotMatchingChildrenListName_Parent oldNotMatchingChildrenListName_Parent) {
    }


    /// <summary>
    /// Called after NotMatchingChildrenListName_Parent.Release() transaction is rolled back
    /// </summary>
    partial void onRollbackItemRelease() {
    }


    /// <summary>
    /// Called after a notMatchingChildrenListName_Child gets added to Children.
    /// </summary>
    partial void onAddedToChildren(NotMatchingChildrenListName_Child notMatchingChildrenListName_Child){
    }


    /// <summary>
    /// Called after a notMatchingChildrenListName_Child gets removed from Children.
    /// </summary>
    partial void onRemovedFromChildren(NotMatchingChildrenListName_Child notMatchingChildrenListName_Child){
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
