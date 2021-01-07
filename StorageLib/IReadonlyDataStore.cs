/**************************************************************************************

StorageLib.IReadonlyDataStore
=============================

Readonly interfaces for DataStore

Written in 2020 by Jürgpeter Huber 
Contact: PeterCode at Peterbox dot com

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace StorageLib {

  /// <summary>
  /// Gives read only access to DataStore, like indexer TItem this[int key], Count or IEnumerable<TItem>
  /// </summary>
  public interface IReadonlyDataStore<out TItem>: IReadOnlyCollection<TItem> where TItem : class, IStorageItem {

    /// <summary>
    /// Unique number of DataStore
    /// </summary>
    public int StoreKey { get; }


    /// <summary>
    /// Gets the capacity of DataStore. The capacity is the size of the internal array used to hold items, which grows 
    /// automatically over time as needed.
    /// </summary>
    public int Capacity { get; }


    /// <summary>
    /// Number of items that can get added before DataStore increases its size automatically.<br/>
    /// Capacity - Count - UnusedSpace = number of deleted items
    /// </summary>
    public int UnusedSpace { get; }


    /// <summary>
    /// Can content of an items be changed ? If yes, a change item gets written to the CVS file
    /// </summary>
    public bool AreInstancesUpdatable { get; }


    /// <summary>
    /// Can stored items be removed ? If yes, a delete item gets written to the CVS file
    /// </summary>
    public bool AreInstancesReleasable { get; }


    /// <summary>
    /// Returns true if items are not updatable nor deletable
    /// </summary>
    public bool IsReadOnly { get; }


    /// <summary>
    /// Returns true if no permanent stored data was found
    /// </summary>
    public bool IsNew { get; }


    /// <summary>
    /// AddProtected(), ItemHasChanged() and Remove() set this flag when they discover that a new transaction has started. When that happens,
    /// DataStoreCsv.OnStartTransaction gets called. 
    /// </summary>
    public bool IsTransactionRunning { get; }


    /// <summary>
    /// Are all Keys just incremented by 1 from the previous Key ? 
    /// </summary>
    public bool AreKeysContinuous { get; }


    /// <summary>
    /// The content of some items has changed and change items have been written to the CVS file. During
    /// Dispose() a new file is written containing only the latest version of the changed items.
    /// </summary>
    public bool AreItemsUpdated { get; }


    /// <summary>
    /// Some items have been deleted and delete items have been written to the CVS file. During
    /// Dispose() a new file is written containing only the undeleted items.
    /// </summary>
    public bool AreItemsDeleted { get; }


    /// <summary>
    /// Checks if item exists in StorageDirectionary and ensures that it is not marked as deleted. 
    /// </summary>
    public bool ContainsKey(int key);


    /// <summary>
    /// If item with key is found, returns item, otherwise null.
    /// </summary>
    public TItem? GetItem(int key);


    /// <summary>
    /// Gives access to item in DataStore using item.Key as index
    /// </summary>
    TItem this[int key] {
      get;
    }


    /// <summary>
    /// Returns all keys from the DataStore in an array.
    /// </summary>
    public IReadOnlyList<int> Keys { get; }


    /// <summary>
    /// Returns all items from the DataStore in an array
    /// </summary>
    public IReadOnlyList<TItem> Values { get; }//needs to be an array to allow IReadonlyDataStore being covariant, ICollection is not covariant
                                  // and IReadonlyCollection does not include Contains()
  }

}
