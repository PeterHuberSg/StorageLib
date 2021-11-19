/**************************************************************************************

StorageLib.StorageSortedList
============================

StorageSortedList is a replacement for SortedList used by parent classes with releasable children. 

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
  /// StorageSortedList is a replacement for StoredList used by parent classes with releasable children. A stored parent might
  /// have stored and not stored children. Enumerating StorageStoredList shows all children as KeyValuePair<TKey, TValue>. 
  /// GetAllItems() enumerates all children and GetStoredItems() enumerates all stored children. Count() counts all children, 
  /// while CountStoredItems() counts all stored children. 
  /// </summary>
  public class StorageSortedList<TKey, TValue>: SortedList<TKey, TValue>, IStorageReadOnlyDictionary<TKey, TValue>
   where TKey : notnull
   where TValue : class, IStorageItem<TValue> {

    /// <summary>
    /// Count how many items from StorageSortedList are stored in the Data Context
    /// </summary>
    public int CountStoredItems {
      get {
        var count = 0;
        foreach (var item in this) {
          if (item.Value.Key>=0) {
            count++;
          }
        }
        return count;
      }
    }


    /// <summary>
    /// Enumerate all items from StorageSortedList, stored and not stored ones
    /// </summary>
    public IEnumerable<TValue> GetAllItems() {
      foreach (var item in this) {
        yield return item.Value;
      }
    }


    /// <summary>
    /// Enumerate all items from StorageSortedList being stored in the Data Context
    /// </summary>
    /// <returns></returns>
    public IEnumerable<TValue> GetStoredItems() {
      foreach (var item in this) {
        if (item.Value.Key>=0) {
          yield return item.Value;
        }
      }
    }
  }
}
