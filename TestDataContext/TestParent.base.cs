//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by StorageClassGenerator
//
//     Do not change code in this file, it will get lost when the file gets 
//     auto generated again. Write your code into TestParent.cs.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using StorageLib;


namespace TestContext  {


  public partial class TestParent: IStorageItem<TestParent> {

    #region Properties
    //      ----------

    /// <summary>
    /// Unique identifier for TestParent. Gets set once TestParent gets added to DC.Data.
    /// </summary>
    public int Key { get; private set; }
    internal static void SetKey(IStorageItem testParent, int key, bool isRollback) {
#if DEBUG
      if (isRollback) {
        if (key==StorageExtensions.NoKey) {
          DC.Trace?.Invoke($"Release TestParent key @{testParent.Key} #{testParent.GetHashCode()}");
        } else {
          DC.Trace?.Invoke($"Store TestParent key @{key} #{testParent.GetHashCode()}");
        }
      }
#endif
      ((TestParent)testParent).Key = key;
    }


    public string Text { get; private set; }


    public IStorageReadOnlyList<TestChild> Children => children;
    readonly StorageList<TestChild> children;


    /// <summary>
    /// Headers written to first line in CSV file
    /// </summary>
    internal static readonly string[] Headers = {"Key", "Text"};


    /// <summary>
    /// None existing TestParent, used as a temporary place holder when reading a CSV file
    /// which was not compacted. It might create first a later deleted item linking to a 
    /// deleted parent. In this case, the parent property gets set to NoTestParent. Once the CSV
    /// file is completely read, that child will actually be deleted (released) and Verify()
    /// ensures that there are no stored children with links to NoTestParent.
    /// </summary>
    internal static TestParent NoTestParent = new TestParent("NoText", isStoring: false);
    #endregion


    #region Events
    //      ------

    /// <summary>
    /// Content of TestParent has changed. Gets only raised for changes occurring after loading DC.Data with previously stored data.
    /// </summary>
    public event Action</*old*/TestParent, /*new*/TestParent>? HasChanged;
    #endregion


    #region Constructors
    //      ------------

    /// <summary>
    /// TestParent Constructor. If isStoring is true, adds TestParent to DC.Data.TestParents.
    /// </summary>
    public TestParent(string text, bool isStoring = true) {
      Key = StorageExtensions.NoKey;
      Text = text;
      children = new StorageList<TestChild>();
#if DEBUG
      DC.Trace?.Invoke($"new TestParent: {ToTraceString()}");
#endif
      onConstruct();
      if (DC.Data.IsTransaction) {
        DC.Data.AddTransaction(new TransactionItem(33,TransactionActivityEnum.New, Key, this));
      }

      if (isStoring) {
        Store();
      }
    }
    partial void onConstruct();


    /// <summary>
    /// Cloning constructor. It will copy all data from original except any collection (children).
    /// </summary>
    #pragma warning disable CS8618 // Children collections are uninitialized.
    public TestParent(TestParent original) {
    #pragma warning restore CS8618 //
      Key = StorageExtensions.NoKey;
      Text = original.Text;
      onCloned(this);
    }
    partial void onCloned(TestParent clone);


    /// <summary>
    /// Constructor for TestParent read from CSV file
    /// </summary>
    private TestParent(int key, CsvReader csvReader){
      Key = key;
      Text = csvReader.ReadString();
      children = new StorageList<TestChild>();
      onCsvConstruct();
    }
    partial void onCsvConstruct();


    /// <summary>
    /// New TestParent read from CSV file
    /// </summary>
    internal static TestParent Create(int key, CsvReader csvReader) {
      return new TestParent(key, csvReader);
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Adds TestParent to DC.Data.TestParents.<br/>
    /// Throws an Exception when TestParent is already stored.
    /// </summary>
    public void Store() {
      if (Key>=0) {
        throw new Exception($"TestParent cannot be stored again in DC.Data, key {Key} is greater equal 0." + Environment.NewLine + ToString());
      }

      var isCancelled = false;
      onStoring(ref isCancelled);
      if (isCancelled) return;

      DC.Data._TestParents.Add(this);
      onStored();
#if DEBUG
      DC.Trace?.Invoke($"Stored TestParent #{GetHashCode()} @{Key}");
#endif
    }
    partial void onStoring(ref bool isCancelled);
    partial void onStored();


    /// <summary>
    /// Estimated number of UTF8 characters needed to write TestParent to CSV file
    /// </summary>
    public const int EstimatedLineLength = 150;


    /// <summary>
    /// Write TestParent to CSV file
    /// </summary>
    internal static void Write(TestParent testParent, CsvWriter csvWriter) {
      testParent.onCsvWrite();
      csvWriter.Write(testParent.Text);
    }
    partial void onCsvWrite();


    /// <summary>
    /// Updates TestParent with the provided values
    /// </summary>
    public void Update(string text) {
      var clone = new TestParent(this);
      var isCancelled = false;
      onUpdating(text, ref isCancelled);
      if (isCancelled) return;

#if DEBUG
      DC.Trace?.Invoke($"Updating TestParent: {ToTraceString()}");
#endif

      //update properties and detect if any value has changed
      var isChangeDetected = false;
      if (Text!=text) {
        Text = text;
        isChangeDetected = true;
      }
      if (isChangeDetected) {
        onUpdated(clone);
        if (Key>=0) {
          DC.Data._TestParents.ItemHasChanged(clone, this);
        } else if (DC.Data.IsTransaction) {
          DC.Data.AddTransaction(new TransactionItem(33, TransactionActivityEnum.Update, Key, this, oldItem: clone));
        }
        HasChanged?.Invoke(clone, this);
      }
#if DEBUG
      DC.Trace?.Invoke($"Updated TestParent: {ToTraceString()}");
#endif
    }
    partial void onUpdating(string text, ref bool isCancelled);
    partial void onUpdated(TestParent old);


    /// <summary>
    /// Updates this TestParent with values from CSV file
    /// </summary>
    internal static void Update(TestParent testParent, CsvReader csvReader){
      testParent.Text = csvReader.ReadString();
      testParent.onCsvUpdate();
    }
    partial void onCsvUpdate();


    /// <summary>
    /// Add testChild to Children.
    /// </summary>
    internal void AddToChildren(TestChild testChild) {
#if DEBUG
      if (testChild==TestChild.NoTestChild) throw new Exception();
      if ((testChild.Key>=0)&&(Key<0)) throw new Exception();
      if (children.Contains(testChild)) throw new Exception();
#endif
      children.Add(testChild);
      onAddedToChildren(testChild);
#if DEBUG
      DC.Trace?.Invoke($"Add TestChild {testChild.GetKeyOrHash()} to " +
        $"{this.GetKeyOrHash()} TestParent.Children");
#endif
    }
    partial void onAddedToChildren(TestChild testChild);


    /// <summary>
    /// Removes testChild from TestParent.
    /// </summary>
    internal void RemoveFromChildren(TestChild testChild) {
#if DEBUG
      if (!children.Remove(testChild)) throw new Exception();
#else
        children.Remove(testChild);
#endif
      onRemovedFromChildren(testChild);
#if DEBUG
      DC.Trace?.Invoke($"Remove TestChild {testChild.GetKeyOrHash()} from " +
        $"{this.GetKeyOrHash()} TestParent.Children");
#endif
    }
    partial void onRemovedFromChildren(TestChild testChild);


    /// <summary>
    /// Removes TestParent from DC.Data.TestParents.
    /// </summary>
    public void Release() {
      if (Key<0) {
        throw new Exception($"TestParent.Release(): TestParent '{this}' is not stored in DC.Data, key is {Key}.");
      }
      foreach (var testChild in Children) {
        if (testChild?.Key>=0) {
          throw new Exception($"Cannot release TestParent '{this}' " + Environment.NewLine + 
            $"because '{testChild}' in TestParent.Children is still stored.");
        }
      }
      DC.Data._TestParents.Remove(Key);
      onReleased();
#if DEBUG
      DC.Trace?.Invoke($"Released TestParent @{Key} #{GetHashCode()}");
#endif
    }
    partial void onReleased();


    /// <summary>
    /// Undoes the new() statement as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemNew(IStorageItem item) {
      var testParent = (TestParent) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback new TestParent(): {testParent.ToTraceString()}");
#endif
      testParent.onRollbackItemNew();
    }
    partial void onRollbackItemNew();


    /// <summary>
    /// Releases TestParent from DC.Data.TestParents as part of a transaction rollback of Store().
    /// </summary>
    internal static void RollbackItemStore(IStorageItem item) {
      var testParent = (TestParent) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback TestParent.Store(): {testParent.ToTraceString()}");
#endif
      testParent.onRollbackItemStored();
    }
    partial void onRollbackItemStored();


    /// <summary>
    /// Restores the TestParent item data as it was before the last update as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemUpdate(IStorageItem oldStorageItem, IStorageItem newStorageItem) {
      var oldItem = (TestParent) oldStorageItem;//an item clone with the values before item was updated
      var item = (TestParent) newStorageItem;//is the instance whose values should be restored
#if DEBUG
      DC.Trace?.Invoke($"Rolling back TestParent.Update(): {item.ToTraceString()}");
#endif

      // updated item: restore old values
      item.Text = oldItem.Text;
      item.onRollbackItemUpdated(oldItem);
#if DEBUG
      DC.Trace?.Invoke($"Rolled back TestParent.Update(): {item.ToTraceString()}");
#endif
    }
    partial void onRollbackItemUpdated(TestParent oldTestParent);


    /// <summary>
    /// Adds TestParent to DC.Data.TestParents as part of a transaction rollback of Release().
    /// </summary>
    internal static void RollbackItemRelease(IStorageItem item) {
      var testParent = (TestParent) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback TestParent.Release(): {testParent.ToTraceString()}");
#endif
      testParent.onRollbackItemRelease();
    }
    partial void onRollbackItemRelease();


    /// <summary>
    /// Returns property values for tracing. Parents are shown with their key instead their content.
    /// </summary>
    public string ToTraceString() {
      var returnString =
        $"{this.GetKeyOrHash()}|" +
        $" {Text}";
      onToTraceString(ref returnString);
      return returnString;
    }
    partial void onToTraceString(ref string returnString);


    /// <summary>
    /// Returns property values
    /// </summary>
    public string ToShortString() {
      var returnString =
        $"{Key.ToKeyString()}," +
        $" {Text}";
      onToShortString(ref returnString);
      return returnString;
    }
    partial void onToShortString(ref string returnString);


    /// <summary>
    /// Returns all property names and values
    /// </summary>
    public override string ToString() {
      var returnString =
        $"Key: {Key.ToKeyString()}," +
        $" Text: {Text}," +
        $" Children: {Children.Count}," +
        $" ChildrenStored: {Children.CountStoredItems};";
      onToString(ref returnString);
      return returnString;
    }
    partial void onToString(ref string returnString);
    #endregion
  }
}
