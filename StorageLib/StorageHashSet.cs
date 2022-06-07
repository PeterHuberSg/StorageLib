/**************************************************************************************

StorageLib.StorageHashSet
=========================

StorageHashSet is a replacement for HashSet used by parent classes with releasable children. 

Written in 2022 by Jürgpeter Huber 
Contact: https://github.com/PeterHuberSg/StorageLib

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/
using System.Collections;
using System.Collections.Generic;


namespace StorageLib {


  /// <summary>
  /// StorageHashSet is a replacement for HashSet used by parent classes with releasable children. A stored parent might
  /// have stored and not stored children. Enumerating StorageHashSet shows all children. GetStoredItems() enumerates all 
  /// stored children. Count() counts all children, while CountStoredItems() counts all stored children. 
  /// </summary>
  public class StorageHashSet<TItem>: HashSet<TItem>, IStorageReadOnlySet<TItem>
    where TItem : class, IStorageItem<TItem> {
    /// <summary>
    /// Count how many items from StorageList are stored in the Data Context
    /// </summary>
    public int CountStoredItems {
      get {
        var count = 0;
        foreach (var item in this) {
          if (item.Key>=0) {
            count++;
          }
        }
        return count;
      }
    }


    /// <summary>
    /// Enumerate all items from StorageList being stored in the Data Context
    /// </summary>
    /// <returns></returns>
    public IEnumerable<TItem> GetStoredItems() {
      foreach (var item in this) {
        if (item.Key>=0) {
          yield return item;
        }
      }
    }
  }
}
