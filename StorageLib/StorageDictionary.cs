/**************************************************************************************

StorageLib.StorageDictionary
============================

StorageDictionary is a replacement for Dictionary used by parent classes with releasable children. 

Written in 2021 by Jürgpeter Huber 
Contact: PeterCode at Peterbox dot com

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace StorageLib {


  /// <summary>
  /// StorageDictionary is a replacement for Dictionary used by parent classes with releasable children. A stored parent might
  /// have stored and not stored children. Enumerating StorageDictionary shows all children as KeyValuePair<TKey, TValue>. 
  /// GetAllItems() enumerates all children and GetStoredItems() enumerates all stored children. Count() counts all children, 
  /// while CountStoredItems() counts all stored children. 
  /// </summary>
  public class StorageDictionary<TKey, TValue>: Dictionary<TKey, TValue>, IStorageReadOnlyDictionary<TKey, TValue>
   where TKey : notnull
   where TValue : class, IStorageItem<TValue> {


    /// <summary>
    /// Count how many items from StorageDictionary are stored in the Data Context
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
    /// <returns></returns>
    public IEnumerable<TValue> GetAllItems() {
      foreach (var item in this) {
          yield return item.Value;
      }
    }


    /// <summary>
    /// Enumerate all items from StorageDictionary being stored in the Data Context
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
