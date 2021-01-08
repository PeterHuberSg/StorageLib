//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by StorageClassGenerator
//
//     Do not change code in this file, it will get lost when the file gets 
//     auto generated again. Write your code into ListChild.cs.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using StorageLib;


namespace TestContext  {


  public partial class ListChild: IStorageItemGeneric<ListChild> {

    #region Properties
    //      ----------

    /// <summary>
    /// Unique identifier for ListChild. Gets set once ListChild gets added to DC.Data.
    /// </summary>
    public int Key { get; private set; }
    internal static void SetKey(IStorageItem listChild, int key, bool isRollback) {
#if DEBUG
      if (isRollback) {
        if (key==StorageExtensions.NoKey) {
          DC.Trace?.Invoke($"Release ListChild key @{listChild.Key} #{listChild.GetHashCode()}");
        } else {
          DC.Trace?.Invoke($"Store ListChild key @{key} #{listChild.GetHashCode()}");
        }
      }
#endif
      ((ListChild)listChild).Key = key;
    }


    public string Text { get; private set; }


    public ListParent Parent { get; private set; }


    public ListParentN? ParentN { get; private set; }


    public ListParentR ParentR { get; }


    public ListParentNR? ParentNR { get; }


    /// <summary>
    /// Headers written to first line in CSV file
    /// </summary>
    internal static readonly string[] Headers = {
      "Key", 
      "Text", 
      "Parent", 
      "ParentN", 
      "ParentR", 
      "ParentNR"
    };


    /// <summary>
    /// None existing ListChild, used as a temporary place holder when reading a CSV file
    /// which was not compacted. It might create first a later deleted item linking to a 
    /// deleted parent. In this case, the parent property gets set to NoListChild. Once the CSV
    /// file is completely read, that child will actually be deleted (released) and Verify()
    /// ensures that there are no stored children with links to NoListChild.
    /// </summary>
    internal static ListChild NoListChild = new ListChild("NoText", ListParent.NoListParent, null, ListParentR.NoListParentR, null, isStoring: false);
    #endregion


    #region Events
    //      ------

    /// <summary>
    /// Content of ListChild has changed. Gets only raised for changes occurring after loading DC.Data with previously stored data.
    /// </summary>
    public event Action</*old*/ListChild, /*new*/ListChild>? HasChanged;
    #endregion


    #region Constructors
    //      ------------

    /// <summary>
    /// ListChild Constructor. If isStoring is true, adds ListChild to DC.Data.ListChidren.
    /// </summary>
    public ListChild(
      string text, 
      ListParent parent, 
      ListParentN? parentN, 
      ListParentR parentR, 
      ListParentNR? parentNR, 
      bool isStoring = true)
    {
      Key = StorageExtensions.NoKey;
      Text = text;
      Parent = parent;
      ParentN = parentN;
      ParentR = parentR;
      ParentNR = parentNR;
#if DEBUG
      DC.Trace?.Invoke($"new ListChild: {ToTraceString()}");
#endif
      Parent.AddToChildren(this);
      if (ParentN!=null) {
        ParentN.AddToChildren(this);
      }
      ParentR.AddToChildren(this);
      if (ParentNR!=null) {
        ParentNR.AddToChildren(this);
      }
      onConstruct();
      if (DC.Data.IsTransaction) {
        DC.Data.AddTransaction(new TransactionItem(14,TransactionActivityEnum.New, Key, this));
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
    public ListChild(ListChild original) {
    #pragma warning restore CS8618 //
      Key = StorageExtensions.NoKey;
      Text = original.Text;
      Parent = original.Parent;
      ParentN = original.ParentN;
      ParentR = original.ParentR;
      ParentNR = original.ParentNR;
      onCloned(this);
    }
    partial void onCloned(ListChild clone);


    /// <summary>
    /// Constructor for ListChild read from CSV file
    /// </summary>
    private ListChild(int key, CsvReader csvReader){
      Key = key;
      Text = csvReader.ReadString();
      var listParentKey = csvReader.ReadInt();
      Parent = DC.Data._ListParents.GetItem(listParentKey)?? ListParent.NoListParent;
      var parentNKey = csvReader.ReadIntNull();
      if (parentNKey.HasValue) {
        ParentN = DC.Data._ListParentNs.GetItem(parentNKey.Value)?? ListParentN.NoListParentN;
      }
      var listParentRKey = csvReader.ReadInt();
      ParentR = DC.Data._ListParentRs.GetItem(listParentRKey)?? ListParentR.NoListParentR;
      var parentNRKey = csvReader.ReadIntNull();
      if (parentNRKey.HasValue) {
        ParentNR = DC.Data._ListParentNRs.GetItem(parentNRKey.Value)?? ListParentNR.NoListParentNR;
      }
      if (Parent!=ListParent.NoListParent) {
        Parent.AddToChildren(this);
      }
      if (parentNKey.HasValue && ParentN!=ListParentN.NoListParentN) {
        ParentN!.AddToChildren(this);
      }
      if (ParentR!=ListParentR.NoListParentR) {
        ParentR.AddToChildren(this);
      }
      if (parentNRKey.HasValue && ParentNR!=ListParentNR.NoListParentNR) {
        ParentNR!.AddToChildren(this);
      }
      onCsvConstruct();
    }
    partial void onCsvConstruct();


    /// <summary>
    /// New ListChild read from CSV file
    /// </summary>
    internal static ListChild Create(int key, CsvReader csvReader) {
      return new ListChild(key, csvReader);
    }


    /// <summary>
    /// Verify that listChild.Parent exists.
    /// Verify that listChild.ParentN exists.
    /// Verify that listChild.ParentR exists.
    /// Verify that listChild.ParentNR exists.
    /// </summary>
    internal static bool Verify(ListChild listChild) {
      if (listChild.Parent==ListParent.NoListParent) return false;
      if (listChild.ParentN==ListParentN.NoListParentN) return false;
      if (listChild.ParentR==ListParentR.NoListParentR) return false;
      if (listChild.ParentNR==ListParentNR.NoListParentNR) return false;
      return true;
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Adds ListChild to DC.Data.ListChidren.<br/>
    /// Throws an Exception when ListChild is already stored.
    /// </summary>
    public void Store() {
      if (Key>=0) {
        throw new Exception($"ListChild cannot be stored again in DC.Data, key {Key} is greater equal 0." + Environment.NewLine + ToString());
      }

      var isCancelled = false;
      onStoring(ref isCancelled);
      if (isCancelled) return;

      if (Parent.Key<0) {
        throw new Exception($"Cannot store child ListChild '{this}'.Parent to ListParent '{Parent}' because parent is not stored yet.");
      }
      if (ParentN?.Key<0) {
        throw new Exception($"Cannot store child ListChild '{this}'.ParentN to ListParentN '{ParentN}' because parent is not stored yet.");
      }
      if (ParentR.Key<0) {
        throw new Exception($"Cannot store child ListChild '{this}'.ParentR to ListParentR '{ParentR}' because parent is not stored yet.");
      }
      if (ParentNR?.Key<0) {
        throw new Exception($"Cannot store child ListChild '{this}'.ParentNR to ListParentNR '{ParentNR}' because parent is not stored yet.");
      }
      DC.Data._ListChidren.Add(this);
      onStored();
#if DEBUG
      DC.Trace?.Invoke($"Stored ListChild #{GetHashCode()} @{Key}");
#endif
    }
    partial void onStoring(ref bool isCancelled);
    partial void onStored();


    /// <summary>
    /// Estimated number of UTF8 characters needed to write ListChild to CSV file
    /// </summary>
    public const int EstimatedLineLength = 150;


    /// <summary>
    /// Write ListChild to CSV file
    /// </summary>
    internal static void Write(ListChild listChild, CsvWriter csvWriter) {
      listChild.onCsvWrite();
      csvWriter.Write(listChild.Text);
      if (listChild.Parent.Key<0) throw new Exception($"Cannot write listChild '{listChild}' to CSV File, because Parent is not stored in DC.Data.ListParents.");

      csvWriter.Write(listChild.Parent.Key.ToString());
      if (listChild.ParentN is null) {
        csvWriter.WriteNull();
      } else {
        if (listChild.ParentN.Key<0) throw new Exception($"Cannot write listChild '{listChild}' to CSV File, because ParentN is not stored in DC.Data.ListParentNs.");

        csvWriter.Write(listChild.ParentN.Key.ToString());
      }
      if (listChild.ParentR.Key<0) throw new Exception($"Cannot write listChild '{listChild}' to CSV File, because ParentR is not stored in DC.Data.ListParentRs.");

      csvWriter.Write(listChild.ParentR.Key.ToString());
      if (listChild.ParentNR is null) {
        csvWriter.WriteNull();
      } else {
        if (listChild.ParentNR.Key<0) throw new Exception($"Cannot write listChild '{listChild}' to CSV File, because ParentNR is not stored in DC.Data.ListParentNRs.");

        csvWriter.Write(listChild.ParentNR.Key.ToString());
      }
    }
    partial void onCsvWrite();


    /// <summary>
    /// Updates ListChild with the provided values
    /// </summary>
    public void Update(string text, ListParent parent, ListParentN? parentN) {
      if (Key>=0){
        if (parent.Key<0) {
          throw new Exception($"ListChild.Update(): It is illegal to add stored ListChild '{this}'" + Environment.NewLine + 
            $"to Parent '{parent}', which is not stored.");
        }
        if (parentN?.Key<0) {
          throw new Exception($"ListChild.Update(): It is illegal to add stored ListChild '{this}'" + Environment.NewLine + 
            $"to ParentN '{parentN}', which is not stored.");
        }
      }
      var clone = new ListChild(this);
      var isCancelled = false;
      onUpdating(text, parent, parentN, ref isCancelled);
      if (isCancelled) return;

#if DEBUG
      DC.Trace?.Invoke($"Updating ListChild: {ToTraceString()}");
#endif

      //remove not yet updated item from parents which will be removed by update
      var hasParentChanged = Parent!=parent;
      if (hasParentChanged) {
        Parent.RemoveFromChildren(this);
      }
      var hasParentNChanged = ParentN!=parentN;
      if (ParentN is not null && hasParentNChanged) {
        ParentN.RemoveFromChildren(this);
      }

      //update properties and detect if any value has changed
      var isChangeDetected = false;
      if (Text!=text) {
        Text = text;
        isChangeDetected = true;
      }
      if (Parent!=parent) {
        Parent = parent;
        isChangeDetected = true;
      }
      if (ParentN!=parentN) {
        ParentN = parentN;
        isChangeDetected = true;
      }

      //add updated item to parents which have been newly added during update
      if (hasParentChanged) {
        Parent.AddToChildren(this);
      }
      if (ParentN is not null && hasParentNChanged) {
        ParentN.AddToChildren(this);
      }
      if (isChangeDetected) {
        onUpdated(clone);
        if (Key>=0) {
          DC.Data._ListChidren.ItemHasChanged(clone, this);
        } else if (DC.Data.IsTransaction) {
          DC.Data.AddTransaction(new TransactionItem(14, TransactionActivityEnum.Update, Key, this, oldItem: clone));
        }
        HasChanged?.Invoke(clone, this);
      }
#if DEBUG
      DC.Trace?.Invoke($"Updated ListChild: {ToTraceString()}");
#endif
    }
    partial void onUpdating(string text, ListParent parent, ListParentN? parentN, ref bool isCancelled);
    partial void onUpdated(ListChild old);


    /// <summary>
    /// Updates this ListChild with values from CSV file
    /// </summary>
    internal static void Update(ListChild listChild, CsvReader csvReader){
      listChild.Text = csvReader.ReadString();
        var parent = DC.Data._ListParents.GetItem(csvReader.ReadInt())??
          ListParent.NoListParent;
      if (listChild.Parent!=parent) {
        if (listChild.Parent!=ListParent.NoListParent) {
          listChild.Parent.RemoveFromChildren(listChild);
        }
        listChild.Parent = parent;
        listChild.Parent.AddToChildren(listChild);
      }
      var parentNKey = csvReader.ReadIntNull();
      ListParentN? parentN;
      if (parentNKey is null) {
        parentN = null;
      } else {
        parentN = DC.Data._ListParentNs.GetItem(parentNKey.Value)??
          ListParentN.NoListParentN;
      }
      if (listChild.ParentN is null) {
        if (parentN is null) {
          //nothing to do
        } else {
          listChild.ParentN = parentN;
          listChild.ParentN.AddToChildren(listChild);
        }
      } else {
        if (parentN is null) {
          if (listChild.ParentN!=ListParentN.NoListParentN) {
            listChild.ParentN.RemoveFromChildren(listChild);
          }
          listChild.ParentN = null;
        } else {
          if (listChild.ParentN!=ListParentN.NoListParentN) {
            listChild.ParentN.RemoveFromChildren(listChild);
          }
          listChild.ParentN = parentN;
          listChild.ParentN.AddToChildren(listChild);
        }
      }
        var parentR = DC.Data._ListParentRs.GetItem(csvReader.ReadInt())??
          ListParentR.NoListParentR;
      if (listChild.ParentR!=parentR) {
        throw new Exception($"ListChild.Update(): Property ParentR '{listChild.ParentR}' is " +
          $"readonly, parentR '{parentR}' read from the CSV file should be the same." + Environment.NewLine + 
          listChild.ToString());
      }
      var parentNRKey = csvReader.ReadIntNull();
      ListParentNR? parentNR;
      if (parentNRKey is null) {
        parentNR = null;
      } else {
        parentNR = DC.Data._ListParentNRs.GetItem(parentNRKey.Value)??
          ListParentNR.NoListParentNR;
      }
      if (listChild.ParentNR!=parentNR) {
        throw new Exception($"ListChild.Update(): Property ParentNR '{listChild.ParentNR}' is " +
          $"readonly, parentNR '{parentNR}' read from the CSV file should be the same." + Environment.NewLine + 
          listChild.ToString());
      }
      listChild.onCsvUpdate();
    }
    partial void onCsvUpdate();


    /// <summary>
    /// Removes ListChild from DC.Data.ListChidren.
    /// </summary>
    public void Release() {
      if (Key<0) {
        throw new Exception($"ListChild.Release(): ListChild '{this}' is not stored in DC.Data, key is {Key}.");
      }
      onReleased();
      DC.Data._ListChidren.Remove(Key);
#if DEBUG
      DC.Trace?.Invoke($"Released ListChild @{Key} #{GetHashCode()}");
#endif
    }
    partial void onReleased();


    /// <summary>
    /// Removes ListChild from parents as part of a transaction rollback of the new() statement.
    /// </summary>
    internal static void RollbackItemNew(IStorageItem item) {
      var listChild = (ListChild) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback new ListChild(): {listChild.ToTraceString()}");
#endif
      if (listChild.Parent!=ListParent.NoListParent) {
        listChild.Parent.RemoveFromChildren(listChild);
      }
      if (listChild.ParentN!=null && listChild.ParentN!=ListParentN.NoListParentN) {
        listChild.ParentN.RemoveFromChildren(listChild);
      }
      if (listChild.ParentR!=ListParentR.NoListParentR) {
        listChild.ParentR.RemoveFromChildren(listChild);
      }
      if (listChild.ParentNR!=null && listChild.ParentNR!=ListParentNR.NoListParentNR) {
        listChild.ParentNR.RemoveFromChildren(listChild);
      }
      listChild.onRollbackItemNew();
    }
    partial void onRollbackItemNew();


    /// <summary>
    /// Releases ListChild from DC.Data.ListChidren as part of a transaction rollback of Store().
    /// </summary>
    internal static void RollbackItemStore(IStorageItem item) {
      var listChild = (ListChild) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback ListChild.Store(): {listChild.ToTraceString()}");
#endif
      listChild.onRollbackItemStored();
    }
    partial void onRollbackItemStored();


    /// <summary>
    /// Restores the ListChild item data as it was before the last update as part of a transaction rollback.
    /// </summary>
    internal static void RollbackItemUpdate(IStorageItem oldStorageItem, IStorageItem newStorageItem) {
      var oldItem = (ListChild) oldStorageItem;//an item clone with the values before item was updated
      var item = (ListChild) newStorageItem;//is the instance whose values should be restored
#if DEBUG
      DC.Trace?.Invoke($"Rolling back ListChild.Update(): {item.ToTraceString()}");
#endif

      // if possible, throw exceptions before changing anything
      if (item.ParentR!=oldItem.ParentR) {
        throw new Exception($"ListChild.Update(): Property ParentR '{item.ParentR}' is " +
          $"readonly, ParentR '{oldItem.ParentR}' should be the same." + Environment.NewLine + 
          item.ToString());
      }
      if (item.ParentNR!=oldItem.ParentNR) {
        throw new Exception($"ListChild.Update(): Property ParentNR '{item.ParentNR}' is " +
          $"readonly, ParentNR '{oldItem.ParentNR}' should be the same." + Environment.NewLine + 
          item.ToString());
      }

      // remove updated item from parents
      var hasParentChanged = oldItem.Parent!=item.Parent;
      if (hasParentChanged) {
        item.Parent.RemoveFromChildren(item);
      }
      var hasParentNChanged = oldItem.ParentN!=item.ParentN;
      if (hasParentNChanged && item.ParentN is not null) {
        item.ParentN.RemoveFromChildren(item);
      }

      // updated item: restore old values
      item.Text = oldItem.Text;
      item.Parent = oldItem.Parent;
      item.ParentN = oldItem.ParentN;

      // add item with previous values to parents
      if (hasParentChanged) {
        item.Parent.AddToChildren(item);
      }
      if (hasParentNChanged && item.ParentN is not null) {
        item.ParentN.AddToChildren(item);
      }
      item.onRollbackItemUpdated(oldItem);
#if DEBUG
      DC.Trace?.Invoke($"Rolled back ListChild.Update(): {item.ToTraceString()}");
#endif
    }
    partial void onRollbackItemUpdated(ListChild oldListChild);


    /// <summary>
    /// Adds ListChild to DC.Data.ListChidren as part of a transaction rollback of Release().
    /// </summary>
    internal static void RollbackItemRelease(IStorageItem item) {
      var listChild = (ListChild) item;
#if DEBUG
      DC.Trace?.Invoke($"Rollback ListChild.Release(): {listChild.ToTraceString()}");
#endif
      listChild.onRollbackItemRelease();
    }
    partial void onRollbackItemRelease();


    /// <summary>
    /// Returns property values for tracing. Parents are shown with their key instead their content.
    /// </summary>
    public string ToTraceString() {
      var returnString =
        $"{this.GetKeyOrHash()}|" +
        $" {Text}|" +
        $" Parent {Parent.GetKeyOrHash()}|" +
        $" ParentN {ParentN?.GetKeyOrHash()}|" +
        $" ParentR {ParentR.GetKeyOrHash()}|" +
        $" ParentNR {ParentNR?.GetKeyOrHash()}";
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
        $" {Text}," +
        $" {Parent.ToShortString()}," +
        $" {ParentN?.ToShortString()}," +
        $" {ParentR.ToShortString()}," +
        $" {ParentNR?.ToShortString()}";
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
        $" Parent: {Parent.ToShortString()}," +
        $" ParentN: {ParentN?.ToShortString()}," +
        $" ParentR: {ParentR.ToShortString()}," +
        $" ParentNR: {ParentNR?.ToShortString()};";
      onToString(ref returnString);
      return returnString;
    }
    partial void onToString(ref string returnString);
    #endregion
  }
}
