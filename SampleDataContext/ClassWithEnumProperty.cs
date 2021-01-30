using System;
using System.Collections.Generic;
using StorageLib;


namespace DataModelSamples  {


  public partial class ClassWithEnumProperty: IStorageItem<ClassWithEnumProperty> {


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
    //partial void onCloned(ClassWithEnumProperty clone) {
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
    /// Called after ClassWithEnumProperty.Store() is executed
    /// </summary>
    //partial void onStored() {
    //}


    /// <summary>
    /// Called before ClassWithEnumProperty gets written to a CSV file
    /// </summary>
    //partial void onCsvWrite() {
    //}


    /// <summary>
    /// Called before any property of ClassWithEnumProperty is updated and before the HasChanged event gets raised
    /// </summary>
    //partial void onUpdating(Weekdays weekday, Weekdays? conditionalWeekDay, ref bool isCancelled){
   //}


    /// <summary>
    /// Called after all properties of ClassWithEnumProperty are updated, but before the HasChanged event gets raised
    /// </summary>
    //partial void onUpdated(ClassWithEnumProperty old) {
    //}


    /// <summary>
    /// Called after an update for ClassWithEnumProperty is read from a CSV file
    /// </summary>
    //partial void onCsvUpdate() {
    //}


    /// <summary>
    /// Called after ClassWithEnumProperty.Release() got executed
    /// </summary>
    //partial void onReleased() {
    //}


    /// <summary>
    /// Called after 'new ClassWithEnumProperty()' transaction is rolled back
    /// </summary>
    //partial void onRollbackItemNew() {
    //}


    /// <summary>
    /// Called after ClassWithEnumProperty.Store() transaction is rolled back
    /// </summary>
    //partial void onRollbackItemStored() {
    //}


    /// <summary>
    /// Called after ClassWithEnumProperty.Update() transaction is rolled back
    /// </summary>
    //partial void onRollbackItemUpdated(ClassWithEnumProperty oldClassWithEnumProperty) {
    //}


    /// <summary>
    /// Called after ClassWithEnumProperty.Release() transaction is rolled back
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
