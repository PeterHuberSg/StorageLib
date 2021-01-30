using System;
using System.Collections.Generic;
using StorageLib;


namespace DataModelSamples  {


  public partial class List_C_MC_Parent: IStorageItem<List_C_MC_Parent> {


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
    //partial void onCloned(List_C_MC_Parent clone) {
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
    /// Called after List_C_MC_Parent.Store() is executed
    /// </summary>
    //partial void onStored() {
    //}


    /// <summary>
    /// Called before List_C_MC_Parent gets written to a CSV file
    /// </summary>
    //partial void onCsvWrite() {
    //}


    /// <summary>
    /// Called before any property of List_C_MC_Parent is updated and before the HasChanged event gets raised
    /// </summary>
    //partial void onUpdating(string name, ref bool isCancelled){
   //}


    /// <summary>
    /// Called after all properties of List_C_MC_Parent are updated, but before the HasChanged event gets raised
    /// </summary>
    //partial void onUpdated(List_C_MC_Parent old) {
    //}


    /// <summary>
    /// Called after an update for List_C_MC_Parent is read from a CSV file
    /// </summary>
    //partial void onCsvUpdate() {
    //}


    /// <summary>
    /// Called after List_C_MC_Parent.Release() got executed
    /// </summary>
    //partial void onReleased() {
    //}


    /// <summary>
    /// Called after 'new List_C_MC_Parent()' transaction is rolled back
    /// </summary>
    //partial void onRollbackItemNew() {
    //}


    /// <summary>
    /// Called after List_C_MC_Parent.Store() transaction is rolled back
    /// </summary>
    //partial void onRollbackItemStored() {
    //}


    /// <summary>
    /// Called after List_C_MC_Parent.Update() transaction is rolled back
    /// </summary>
    //partial void onRollbackItemUpdated(List_C_MC_Parent oldList_C_MC_Parent) {
    //}


    /// <summary>
    /// Called after List_C_MC_Parent.Release() transaction is rolled back
    /// </summary>
    //partial void onRollbackItemRelease() {
    //}


    /// <summary>
    /// Called after a list_C_MC_Child gets added to Children.
    /// </summary>
    //partial void onAddedToChildren(List_C_MC_Child list_C_MC_Child){
    //}


    /// <summary>
    /// Called after a list_C_MC_Child gets removed from Children.
    /// </summary>
    //partial void onRemovedFromChildren(List_C_MC_Child list_C_MC_Child){
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
