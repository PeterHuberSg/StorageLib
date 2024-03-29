﻿/**************************************************************************************

StorageLib.IStorageReadOnlyCollection
=====================================

IStorageReadOnlyCollection is a replacement for IReadOnlyList used by parent classes with releasable children, used by
StorageList, a replacement of 

Written in 2021 by Jürgpeter Huber 
Contact: https://github.com/PeterHuberSg/StorageLib

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/
using System.Collections.Generic;


namespace StorageLib {


  /// <summary>
  /// Like IReadOnlyCollection, but for StorageList, StorageDictionary, etc., which provide CountStoredItems and 
  /// GetStoredItems().
  /// </summary>
  public interface IStorageReadOnly<TItem>{

    /// <summary>
    /// Enumerate over all stored items.
    /// </summary>
    public IEnumerable<TItem> GetStoredItems();

    /// <summary>
    /// Counting all stored items.
    /// </summary>
    public int CountStoredItems { get; }
  }


  /// <summary>
  /// Like IReadOnlyList, but for StorageList, which provides CountStoredItems and GetStoredItems().
  /// </summary>
  public interface IStorageReadOnlyList<TItem>: IStorageReadOnly<TItem>, IReadOnlyList<TItem>{ }


  /// <summary>
  /// Like IReadOnlySet, but for StorageHashSet, which provides CountStoredItems and GetStoredItems().
  /// </summary>
  public interface IStorageReadOnlySet<TItem>: IStorageReadOnly<TItem>, IReadOnlySet<TItem> { }


  /// <summary>
  /// Like IReadOnlyDictionary, but for StorageDictionary or StorageSortedList, which provide CountStoredItems and 
  /// GetStoredItems(). 
  /// </summary>
  public interface IStorageReadOnlyDictionary<TKey, TValue>:
    IStorageReadOnly<TValue>, IReadOnlyDictionary<TKey, TValue> { }
}
