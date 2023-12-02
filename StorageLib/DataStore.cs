/**************************************************************************************

StorageLib.DataStore
====================

Stores instances (=items) of classes inheriting from IStorage in an array. An item can be accessed by its key. It behaves
like a dictionary, but is much faster and supports IEnumerable.
The data is only stored in RAM and gets lost once the application ends. Use DataStoreCSV for permanent data 
storage.

Written in 2020 by Jürgpeter Huber 
Contact: https://github.com/PeterHuberSg/StorageLib

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;


namespace StorageLib {

  #region None generic base class DataStore
  //      ---------------------------------

  /// <summary>
  /// None generic base class DataStore can be used to make arrays of all DataStores, StoreKey can be used as index into
  /// that array. DataStore contains shared, none generic methods, which is needed for transaction support.
  /// </summary>
  public abstract class DataStore: IDisposable {

    #region Properties
    //      ----------

    /// <summary>
    /// DataContext which created this DataStore
    /// </summary>
    public DataContextBase? DataContextBase { get; private set; }


    /// <summary>
    /// Unique number of DataStore
    /// </summary>
    public int StoreKey { get; }


    /// <summary>
    /// Can content of an items be changed ? If yes, a "change item" gets written to the CVS file
    /// </summary>
    public bool AreInstancesUpdatable { get; }


    /// <summary>
    /// Can stored items be removed ? If yes, a "delete item" gets written to the CVS file
    /// </summary>
    public bool AreInstancesReleasable { get; }


    /// <summary>
    /// Returns true if items are not updatable nor deletable
    /// </summary>
    public bool IsReadOnly { get; }


    /// <summary>
    /// Returns true if no permanent stored data was found
    /// </summary>
    public bool IsNew { get; protected set; }


    /// <summary>
    /// AddProtected(), ItemHasChanged() and Remove() set this flag when they discover that a new transaction has started. When that happens,
    /// DataStoreCsv.OnStartTransaction gets called. 
    /// </summary>
    public bool IsTransactionRunning { get; protected set; }


    /// <summary>
    /// Number of items in DataStore
    /// </summary>
    public int Count { get; protected set; }


    /// <summary>
    /// Are all Keys just incremented by 1 from the previous Key ? 
    /// </summary>
    public bool AreKeysContinuous { get; protected set; }

    protected void UpdateAreKeysContinuous() {
      AreKeysContinuous = !AreInstancesReleasable || Count<2 || KeysArray[LastItemIndex] - KeysArray[FirstItemIndex] + 1 == Count;
    }


    /// <summary>
    /// The content of some items has changed and change items have been written to the CVS file. During
    /// Dispose() a new file is written containing only the latest version of the changed items.
    /// </summary>
    public bool AreItemsUpdated => ItemsUpdatedCount>0;
    protected int ItemsUpdatedCount;


    /// <summary>
    /// Some items have been deleted and delete items have been written to the CVS file. During
    /// Dispose() a new file is written containing only the undeleted items.
    /// </summary>
    public bool AreItemsDeleted => ItemsDeletedCount>0;
    protected int ItemsDeletedCount;


    /// <summary>
    /// Index of first item, usually 0, not really used by inheritors
    /// </summary>
    protected int FirstItemIndex { get; set; }


    /// <summary>
    /// Index of last item stored. Used by readonly DataStoreCSV to create key for new item.
    /// </summary>
    protected int LastItemIndex { get; set; }
    #endregion


    #region Constructor
    //      -----------

    protected int[] KeysArray; //keys don't get deleted when an item gets removed, because the key of the removed item is still needed for binary search


    /// <summary>
    /// Constructor
    /// </summary>
    #pragma warning disable CS8618 // Non-nullable KeysArray is uninitialized. The inheriting constructor will initialise it.
    public DataStore(
    #pragma warning restore CS8618
      DataContextBase? dataContext,
      int storeKey,
      bool areInstancesUpdatable = false,
      bool areInstancesReleasable = false) 
    {
      DataContextBase = dataContext;
      StoreKey = storeKey;
      AreInstancesUpdatable = areInstancesUpdatable;
      AreInstancesReleasable = areInstancesReleasable;
      IsReadOnly = !AreInstancesUpdatable && !AreInstancesReleasable;
      IsNew = true;
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Only used with DataStoreCSV.
    /// Initiates that all data presently in RAM write buffers are written immediately to a file. Usually buffers get 
    /// written when they are full, the CSVWriter.FlushTimer runs or the DataStore gets disposed. For normal operation
    /// it should not be necessary to call Flush(), it is mainly used for time measurement.
    /// </summary>
    public virtual void Flush() {
      throw new NotSupportedException("Use DataStoreCSV if data should get written to a file and DataStore flushed.");
    }


    /// <summary>
    /// Called by AddProtected(), ItemHasChanged() and Remove(), when they detect a new transaction.
    /// Is used by DataStoreCSV to mark where in the CSV File the transaction started. After CommitTransaction() this
    /// mark gets deleted. After RollbackTransaction(), the data written during the transaction to the CSV file gets
    /// deleted
    /// </summary>
    protected virtual void OnStartTransaction() {
    }


    /// <summary>
    /// Called by DataContext to signal the DataStore the normal end of the current transaction.
    /// </summary>
    internal void CommitTransaction() {
      IsTransactionRunning = false;
      OnCommitTransaction();
    }


    /// <summary>
    /// Used by DataStoreCSV telling the DataStore to clear the transaction supporting data
    /// </summary>
    protected virtual void OnCommitTransaction() {
    }


    /*+
    /// <summary>
    /// Performs for one item at a time activities which can be performed only one transaction is committed,
    /// like raising events like Added, Updated and Removed. 
    /// </summary>
    internal abstract void CommitItem(in TransactionItem transactionItem);
    +*/


    /// <summary>
    /// Called by DataContext to signal that the changes done by the current transaction should be undone.
    /// Rollback happens in 2 phases. First, RollbackTransaction gets called, which is used by DataStoreCSV to
    /// undo all data written to the CSV file since the transaction started. In the second part, RollbackItem() gets 
    /// called for every changed instance to undo the changes in memory.
    /// </summary>
    internal void RollbackTransaction() {
      IsTransactionRunning = false;
      OnRollbackTransaction();
    }


    protected virtual void OnRollbackTransaction() {
    }


    /// <summary>
    /// Undoes the changes done during the current transaction for one item
    /// </summary>
    internal abstract void RollbackItem(in TransactionItem transactionItem);
    #endregion


    #region IDisposable Support
    //      -------------------

    /// <summary>
    /// Releases all resource used by the DataStore, like files used to store data permanently.
    /// </summary>
    public void Dispose() {
      var wasDisposed = Interlocked.Exchange(ref isDisposed, 1);//prevents that 2 threads dispose simultaneously
      if (wasDisposed==1) return; // already disposed

      Dispose(true);

      GC.SuppressFinalize(this);
    }


    /// <summary>
    /// Is DataStore already disposed ?
    /// </summary>
    public bool IsDisposed {
      get { return isDisposed==1; }
    }
    int isDisposed;


    protected virtual void Dispose(bool disposing) {
      //release big properties. This helps in performance measurements, when several DataStores are created
      //sequentially and the garbage collector is run in between.
      DataContextBase = null!;
    }
    #endregion
  }
  #endregion


  #region Generic class DataStore
  //      -----------------------

  /// <summary>
  /// A fast collection of items which implement IStorage. Each item can get added to a DataStore only once. The DataStore
  /// sets the Key property of that item. DataStore does not publicly support DataStore.Add(item). Use item.Store() instead.
  /// </summary>
  public class DataStore<TItem>: DataStore, IReadonlyDataStore<TItem> 
    where TItem : class, IStorageItem<TItem>
  {

    #region Properties
    //      ----------

    /// <summary>
    /// Gets the capacity of DataStore. The capacity is the size of the internal array used to hold items, which grows 
    /// automatically over time as needed.
    /// </summary>
    public int Capacity {
      get {
        return items.Length;
      }
    }


    /// <summary>
    /// Number of items that can get added before DataStore increases its size automatically.<br/>
    /// Capacity - Count - UnusedSpace = number of deleted items
    /// </summary>
    public int UnusedSpace { get; }


    /// <summary>
    /// Indexer, returns item based on key
    /// </summary>
    public TItem this[int key] { 
      get {
        int arrayIndex = binarySearch(key);
        return arrayIndex<0
            ? throw new ArgumentOutOfRangeException($"Cannot find key {key}.")
            : items[arrayIndex]?? throw new ArgumentException($"There is no item for key '{key}'.");
      }
    }


    /// <summary>
    /// Returns all keys from the DataStore in an array.
    /// </summary>
    public IReadOnlyList<int> Keys {
      get {
        if (Count==0) return Array.Empty<int>();

        int[] keysCopy = new int[Count];
        var keyIndex = 0;
        for (int itemsIndex = FirstItemIndex; itemsIndex<=LastItemIndex; itemsIndex++) {
          var item = items[itemsIndex];
          if (item!=null) {
            keysCopy[keyIndex++] = item.Key;
          }
        }
        return keysCopy;
      }
    }


    /// <summary>
    /// Returns all items from the DataStore in an array
    /// </summary>
    public IReadOnlyList<TItem> Values {  
      get {
        if (Count==0) return Array.Empty<TItem>();

        TItem[] values = new TItem[Count];
        var keyIndex = 0;
        for (int itemsIndex = FirstItemIndex; itemsIndex<=LastItemIndex; itemsIndex++) {
          var item = items[itemsIndex];
          if (item!=null) {
            values[keyIndex++] = item;
          }
        }
        return values;
      }
    }
    #endregion


    #region Constructor
    //     ------------

    const uint defaultCapacity = 4;

    /*
    Unfortunately, interfaces cannot be internal. Some methods like setting an item's key should not be exposed for public
    consumption. Therefore, interfaces cannot be used for these internal methods. Instead, they are passed in the Constructor
    of DataStore.
    */
    Action<IStorageItem, int, /*isRollback*/bool> setKey;
    Action<IStorageItem> rollbackItemNew;
    Action<IStorageItem> rollbackItemStore;
    Action</*old*/IStorageItem, /*new*/IStorageItem>? rollbackItemUpdate;
    Action<IStorageItem>? rollbackItemRelease;

    TItem?[] items;
    //int[] keys; //keys don't get deleted when an item gets removed, because the key of the removed item is still needed for binary search
    int version;


    /// <summary>
    /// Constructs DataStore with a given initial capacity. It is initially empty, but will have room for the given 
    /// number of items. When too many items get added, the capacity gets increased.
    /// </summary>
    /// <param name="dataContext">DataContext creating this DataStore</param>
    /// <param name="storeKey">Unique number to identify DataStore</param>
    /// <param name="setKey">Called when an item gets added to set its Key</param>
    /// <param name="rollbackItemNew">Undo of data change in item during transaction due to item constructor</param>
    /// <param name="rollbackItemStore">Undo of data change in item during transaction due to item.Store()</param>
    /// <param name="rollbackItemUpdate">Undo of data change in item during transaction due to item.Update()</param>
    /// <param name="rollbackItemRelease">Undo of data change in item during transaction due to item.Release()</param>
    /// <param name="areInstancesUpdatable">Can the property of an item change ?</param>
    /// <param name="areInstancesReleasable">Can an item be removed from DataStore</param>
    /// <param name="capacity">How many items should DataStore by able to hold initially ?</param>
    public DataStore(
      DataContextBase? dataContext,
      int storeKey,
      Action<IStorageItem, int, /*isRollback*/bool> setKey,
      Action<IStorageItem> rollbackItemNew,
      Action<IStorageItem> rollbackItemStore,
      Action</*old*/IStorageItem, /*new*/IStorageItem>? rollbackItemUpdate,
      Action<IStorageItem>? rollbackItemRelease,
      //Action<TItem>? disconnect,
      bool areInstancesUpdatable = false,
      bool areInstancesReleasable = false,
      int capacity = 0) 
    :base(dataContext, storeKey, areInstancesUpdatable, areInstancesReleasable)
    {
      this.setKey = setKey;
      this.rollbackItemNew = rollbackItemNew;
      this.rollbackItemStore = rollbackItemStore;
      this.rollbackItemUpdate = rollbackItemUpdate;
      this.rollbackItemRelease = rollbackItemRelease;
      //this.disconnect = disconnect;
      if (capacity < 0) throw new ArgumentOutOfRangeException("Capacity must be equal or grater , but was '" + capacity + "'.");

      if (capacity==0) {
        items = Array.Empty<TItem>();
        KeysArray = Array.Empty<int>();
      } else { 
        items = new TItem[capacity];
        KeysArray = new int[capacity];
      }

      ItemsUpdatedCount = 0;
      ItemsDeletedCount = 0;
      FirstItemIndex = -1;
      LastItemIndex = -1;
      AreKeysContinuous = true; // is true for empty collection
      Count = 0;
    }
    #endregion


    /// <summary>
    /// Checks if item exists in DataStore and ensures that it is not marked as deleted. 
    /// </summary>
    public bool ContainsKey(int key) {
      return binarySearch(key)>=0;
    }


    /// <summary>
    /// Returns an enumerator over all undeleted TItems in DataStore
    /// </summary>
    public IEnumerator<TItem> GetEnumerator() {
      return new EnumeratorItems(this);
    }


    /// <summary>
    /// Returns an object enumerator over all undeleted TItems in DataStore. Better use the strongly 
    /// typed IEnumerator<TItem> GetEnumerator(). 
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator() {
      return new EnumeratorItems(this);
    }


    /// <summary>
    /// Removes item with key. Returns false if item cannot be found.
    /// </summary>
    public bool Remove(int key) {
#if DEBUG
      if (!AreInstancesReleasable) throw new NotSupportedException($"DataStore for {typeof(TItem).Name} does not allow key '{key}' to be deleted.");
#endif

      int index;
      TItem? item;
      lock (items) {
        index = binarySearch(key);
        if (index<0) return false;

        item = items[index];
        if (item==null) return true; //item was already deleted

        items[index] = null;
        //item.HasChanged -= item_HasChanged;
        version++;
        Count--;
#if DEBUG
        if (Count<0) throw new Exception(); //count should never become negative
#endif
        ItemsDeletedCount++;

        if (Count<=0) {
          FirstItemIndex = -1;
          LastItemIndex = -1;
          AreKeysContinuous = true;

        } else if (Count==1) {
          if (index==FirstItemIndex) {
            FirstItemIndex = LastItemIndex;
#if DEBUG
          } else if (index!=LastItemIndex) {
            //we should never arrive here.
            throw new Exception();
#endif
          } else {
            LastItemIndex = FirstItemIndex;
          }
          AreKeysContinuous = true;

        } else {
          if (FirstItemIndex==index) {
            do {
              FirstItemIndex++;
            } while (items[FirstItemIndex]==null);//since there are at least 2 items left, firstItemKey will always be smaller than lastItemKey
            UpdateAreKeysContinuous();
          } else if (LastItemIndex==index) {
            do {
              LastItemIndex--;
            } while (items[LastItemIndex]==null);//since there are at least 2 items left, lastItemKey will always be bigger than firstItemKey
            UpdateAreKeysContinuous();
          } else {
            //item was removed from the middle of items
            AreKeysContinuous = false;
          }
        }

        if (DataContextBase?.IsTransaction??false) {
          if (!IsTransactionRunning) {
            IsTransactionRunning = true;
            OnStartTransaction();
          }
          DataContextBase.TransactionItems.Add(new TransactionItem(StoreKey,
            TransactionActivityEnum.Release, item.Key, item, index));
          DataContextBase.TransactionStoreFlags[(int)StoreKey] = true;
        }
        OnItemRemoved(item);
      }
      //disconnect!(item);
      setKey(item, StorageExtensions.NoKey, /*isRollback*/false);//remove key only once disconnect() and OnItemRemoved() have executed.
      return true;
    }


    /// <summary>
    /// Called after item has been marked as deleted in DataStore
    /// </summary>
    protected virtual void OnItemRemoved(TItem item) {
    }


    /// <summary>
    /// Removes item from this StorageDirectory and any children from their StorageDirectories. Removed event gets fired.
    /// If item was removed already, still true gets returned. No Removed event gets fired.
    /// </summary>
    public bool Remove(TItem item) {
      return item.Key<0 ? throw new Exception($"DataStore can not remove item '{item}' with no key (-1).") : Remove(item.Key);
    }


    /// <summary>
    /// If item with key is found, returns item, otherwise false.
    /// </summary>
    public TItem? GetItem(int key) {
      var index = binarySearch(key);
      return index<0 ? null : (items?[index]);
    }


    #region Disposable Interface
    //     ---------------------

    //Todo: DataStore.Dispose(); Rollback transaction if dispose. Probably better if DC.Dispose() would detect and handle uncommitted transaction.
    protected override void Dispose(bool disposing) {
      setKey = null!;
      rollbackItemNew = null!;
      rollbackItemStore = null!;
      rollbackItemUpdate = null;
      rollbackItemRelease = null;
      //disconnect = null;
      items = null!;

      base.Dispose(disposing);
    }
    #endregion


    #region Public Methods
    //     ---------------

    // From Microsoft imposes limits on maximum array length (defined as internal constant in Array class)
    const uint maxArrayLength = 0X7FEFFFFF;


    /// <summary>
    /// Adds the given item to the end of DataStore and sets the item.Key. The Count is increased by one. If required, 
    /// the capacity of DataStore is doubled before adding the new item.
    /// </summary>
    public void Add(TItem item) {
      if (item.Key>=0) throw new Exception($"Cannot add {typeof(TItem).Name} '{item}' to DataStore, because it is already added (Key is 0 or bigger).");

      AddProtected(item);
    }


    protected void AddProtected(TItem item) {
      if (IsDisposed) throw new ObjectDisposedException($"DataStore<{typeof(TItem).Name}>");

      lock (items) {
        var lastItemKey = LastItemIndex==-1 ? -1 : items[LastItemIndex]!.Key;//throws exception if indexed item is null
        if (item.Key==StorageExtensions.NoKey) {
          setKey(item, ++lastItemKey, /*isRollback*/false);
        } else {
          if (item.Key<=lastItemKey) throw new Exception($"Cannot add {typeof(TItem).Name} '{item}' to DataStore, because its key should be greater than lastItemKey {lastItemKey}.");
        }
        LastItemIndex++;
        if (Count==0) {
          FirstItemIndex = LastItemIndex;
        }

        //ensure there is enough space
        if (LastItemIndex>=items.Length) {
          uint itemsLength = (uint)items.Length;
          uint newCapacityUInt = itemsLength == 0 ? defaultCapacity : itemsLength * 2;
          if (newCapacityUInt > maxArrayLength) newCapacityUInt = maxArrayLength;

          int newCapacity = (int)newCapacityUInt;

          var newItems = new TItem[newCapacity];
          var newKeys = new int[newCapacity];
          if (items!=Array.Empty<TItem>()) {
            Array.Copy(items, 0, newItems, 0, LastItemIndex);
            Array.Copy(KeysArray, 0, newKeys, 0, LastItemIndex);
          }
          items = newItems;
          KeysArray = newKeys;
        }

        //item.HasChanged += item_HasChanged;
        items[LastItemIndex] = item;
        KeysArray[LastItemIndex] = item.Key;
        Count++;
        version++;

        if (DataContextBase?.IsTransaction??false) {
          if (!IsTransactionRunning) {
            IsTransactionRunning = true;
            OnStartTransaction();
          }
          DataContextBase.TransactionItems.Add(new TransactionItem(StoreKey, TransactionActivityEnum.Store, item.Key, item, LastItemIndex));
          DataContextBase.TransactionStoreFlags[(int)StoreKey] = true;
        }
        OnItemAdded(item);
      }
    }


    /// <summary>
    /// Called when new item was added to DataStore. 
    /// </summary>
    protected virtual void OnItemAdded(TItem item) {
    }


    public void ItemHasChanged(TItem oldItem, TItem newItem) {
#if DEBUG
      if (!AreInstancesUpdatable) throw new NotSupportedException($"DataStore for {typeof(TItem).Name} does not " +
        $"allow item '{oldItem}' to be updated to {newItem}.");
      if (newItem.Key<0) throw new Exception();
#endif

      ItemsUpdatedCount++;
      version++;
      if (DataContextBase?.IsTransaction??false) {
        if (!IsTransactionRunning) {
          IsTransactionRunning = true;
          OnStartTransaction();
        }
        DataContextBase.TransactionItems.Add(
          new TransactionItem(StoreKey, TransactionActivityEnum.Update, newItem.Key, newItem, index: int.MinValue, oldItem));
        DataContextBase.TransactionStoreFlags[(int)StoreKey] = true;
      }

      OnItemHasChanged(oldItem, newItem);
    }



    /// <summary>
    /// Called when the content of an item has changed. There is no change (add, remove) in DataStore itself. 
    /// </summary>
    protected virtual void OnItemHasChanged(TItem oldItem, TItem newItem) {
    }
    #endregion

    #region Override Methods
    //      ----------------

    /*+
    override internal void CommitItem(in TransactionItem transactionItem) {
      var item = (TItem)transactionItem.Item;
      switch (transactionItem.TransactionActivity) {
      case TransactionActivityEnum.Add:
        Added?.Invoke(item);
        break;
      case TransactionActivityEnum.Update:
        var oldItem = (TItem)transactionItem.OldItem!;
        Updated?.Invoke(oldItem, item);
        break;
      case TransactionActivityEnum.Remove:
        Removed?.Invoke(item);
        break;
      default: throw new NotSupportedException();
      }
    }
    +*/

    override internal void RollbackItem(in TransactionItem transactionItem) {
      switch (transactionItem.TransactionActivity) {
      case TransactionActivityEnum.New:
        rollbackItemNew?.Invoke(transactionItem.Item);
        break;

      case TransactionActivityEnum.Store:
        rollbackItemStore(transactionItem.Item);//execute rollbackItemStore as long item.Key is still defined, helps tracing
        rollbackStoreAdd(transactionItem.Index, transactionItem.Item);
        break;

      case TransactionActivityEnum.Update:
#if DEBUG
        if (!AreInstancesUpdatable) throw new Exception();
#endif
        if (transactionItem.Item.Key>=0) {
          rollbackStoreUpdate();
        }
        rollbackItemUpdate?.Invoke(transactionItem.OldItem!, transactionItem.Item);
        break;

      case TransactionActivityEnum.Release:
#if DEBUG
        if (!AreInstancesReleasable) throw new Exception();
#endif
        rollbackStoreRemove(transactionItem.Index, transactionItem.Key, transactionItem.Item);
        rollbackItemRelease?.Invoke(transactionItem.Item);
        break;
      default: throw new NotSupportedException();
      }
    }


    private void rollbackStoreAdd(int index, IStorageItem item) {
#if DEBUG
      //LastItemIndex always points to the last added item. RollbackStoreAdd() must always be about the last added item.
      if (Count<=0 || index!=LastItemIndex|| KeysArray[index]!=item.Key || items[index]!=item) 
        throw new Exception($"DataStore<typeof(TItem).Name>.RollbackStoreAdd(item: {item}); count: {Count}; " +
          $"LastItemIndex: {LastItemIndex}; keys[index]: {KeysArray[index]}; items[index]: {items[index]};");
#endif
      items[index] = null;
      setKey(item, StorageExtensions.NoKey, /*isRollback*/true);
      //((TItem)item).HasChanged -= item_HasChanged;
      //no need to change keys[]. The next item added will get the same number and in the meantime keys[index] does 
      //not give a problem to binarySearch()
      Count--;
      version--;
      if (Count==0) {
        FirstItemIndex = LastItemIndex = -1;
        AreKeysContinuous = true;
      } else if (Count==1) {
        LastItemIndex = FirstItemIndex;
        AreKeysContinuous = true;
      } else {
        for (int i = LastItemIndex; i >= 0; i--) {
          if (items[i]!=null) {
            LastItemIndex = i;
            break;
          }
        }
        UpdateAreKeysContinuous();
      }
    }


    private void rollbackStoreUpdate() {
      ItemsUpdatedCount--;
#if DEBUG
      if (ItemsUpdatedCount<0) throw new Exception();
#endif
    }


    private void rollbackStoreRemove(int index, int key, IStorageItem item) {
#if DEBUG
      if (!AreInstancesReleasable) throw new Exception();
      if (item.Key!=StorageExtensions.NoKey || index<0 || items[index]!=null || KeysArray[index]!=key)
        throw new Exception($"DataStore<{typeof(TItem).Name}>.RollbackStoreRemove(key: {key}, item: {item}); index: {index};");
#endif

      var tItem = (TItem)item;
      items[index] = tItem;
      //keys has still the correct key for this item
      //AreItemsDeleted stays true. Easier than figuring out if it is still true.
      setKey(tItem, key, /*isRollback*/true);
      //((TItem)item).HasChanged += item_HasChanged;
      Count++;
      version--;
      ItemsDeletedCount--;
#if DEBUG
      if (ItemsDeletedCount<0) throw new Exception();
#endif
      if (Count==1) {
        LastItemIndex = FirstItemIndex = index;
        AreKeysContinuous = true;
      } else if (LastItemIndex<index) {
        LastItemIndex = index;
        UpdateAreKeysContinuous();
      } else if (FirstItemIndex>index) {
        FirstItemIndex = index;
        UpdateAreKeysContinuous();
      } else {
        //index is between FirstItemIndex and LastItemIndex, no need to update them
        UpdateAreKeysContinuous();
      }
    } 


    /// <summary>
    /// Write some important data about this DataStore into a string.
    /// </summary>
    public override string ToString() {
      return
        $"Count: {Count};{(AreInstancesUpdatable ? " Upd" : "")}{(AreInstancesReleasable ? " Del" : "")}{(IsReadOnly ? " ReadOnly" : "")}" +
        $"{(AreKeysContinuous ? " Cont" : "")}";
    }
    #endregion


    #region Private Methods
    //      ---------------
    int binarySearch(int key) {
      if (Count==0) return -1;// DataStore is empty

      var firstItemKey = items[FirstItemIndex]!.Key;//throws exception if firstItemIndex invalid or indexed item is null
      if (firstItemKey>key) return -1;// item is missing, key is too small

      var lastItemKey = items[LastItemIndex]!.Key;
      if (lastItemKey<key) return -1;// item is missing, key is too big

      return AreKeysContinuous ? FirstItemIndex + key - firstItemKey : binarySearch(key, FirstItemIndex, LastItemIndex);
    }


    int binarySearch(int key, int min, int max) {
      while (min<=max) {
        int mid = (min + max) / 2;
        int compareResult = KeysArray[mid].CompareTo(key);
        if (compareResult == 0) {
          return items[mid]==null ? -1 : mid;

        } else if (compareResult == 1) {
          max = mid - 1;
        } else {
          min = mid + 1;
        }
      }
      return -1;
    }
    #endregion


    #region DataStore Enumerator
    //      --------------------

    /// <summary>
    /// Item Enumerator for DataStore
    /// </summary>
    [Serializable]
    public struct EnumeratorItems: IEnumerator<TItem>, IEnumerator {
      public readonly TItem Current => current ?? throw new InvalidOperationException();

      readonly object System.Collections.IEnumerator.Current => current ?? throw new InvalidOperationException();


      readonly DataStore<TItem> dataStore;
      readonly int version;
      int index;
      readonly int maxIndex;
      TItem? current;


      /// <summary>
      /// Constructor
      /// </summary>
      internal EnumeratorItems(DataStore<TItem> dataStore) {
        this.dataStore = dataStore;
        version = dataStore.version;
        if (dataStore.Count==0) {
          index = -1;
          maxIndex = -1;
        } else {
          index = dataStore.FirstItemIndex - 1;
          maxIndex = dataStore.LastItemIndex;
        }
        current = null;
      }


      /// <summary>
      /// Doesn't do anything
      /// </summary>
      public readonly void Dispose() {
      }


      public bool MoveNext() {
        if (version!=dataStore.version) {
          throw new InvalidOperationException("DataStore content has changed during enumeration.");
        }
        while (true) {
          index++;
          if (index>maxIndex) {
            break;
          }
          var item = dataStore.items[index];
          if (item!=null) {
            current = item;
            return true;
          }
        }

        //end reached
        index = maxIndex+1;
        current = null;
        return false;
      }


      void System.Collections.IEnumerator.Reset() {
        index = -1;
        current = default;
      }
    }
#endregion
  }
  #endregion
}
