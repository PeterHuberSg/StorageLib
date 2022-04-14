/**************************************************************************************

StorageLib.StorageSortedBucketCollection
========================================

StorageSortedBucketCollection is a replacement for SortedBucketCollection used by parent classes with releasable children. 

Written in 2021 by Jürgpeter Huber 
Contact: https://github.com/PeterHuberSg/StorageLib

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/
using System;
using System.Collections.Generic;


namespace StorageLib {


  /// <summary>
  /// Like IReadOnlySortedBucketCollection, but for StorageSortedBucketCollection, which provides CountStoredItems and 
  /// GetStoredItems().
  /// </summary>
  public interface IStorageReadOnlySortedBucketCollection<TKey1, TKey2, TItem>: 
    IStorageReadOnly<TItem>, IReadOnlySortedBucketCollection<TKey1, TKey2, TItem> { }


  /// <summary>
  /// StorageSortedBucketCollection is a replacement for SortedBucketCollection used by parent classes with releasable 
  /// children. A stored parent might have stored and not stored children. Enumerating StorageSortedBucketCollection shows 
  /// all children, while GetStoredItems() enumerates all stored children. Count() counts all children,  while 
  /// CountStoredItems() counts all stored children. 
  /// </summary>
  public class StorageSortedBucketCollection<TKey1, TKey2, TValue>: 
    SortedBucketCollection<TKey1, TKey2, TValue>,
    IStorageReadOnlySortedBucketCollection<TKey1, TKey2, TValue>
    where TKey1 : notnull, IComparable<TKey1>
    where TKey2 : notnull, IComparable<TKey2>
    where TValue : class, IStorageItem<TValue> {

    #region Properties
    //      ----------

    /// <summary>
    /// Count how many items from StorageSortedList are stored in the Data Context
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
    #endregion


    #region Constructor
    //      -----------

    public StorageSortedBucketCollection(Func<TValue, TKey1> getKey1, Func<TValue, TKey2> getKey2): base(getKey1, getKey2) {}
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Enumerate all items from StorageSortedList being stored in the Data Context
    /// </summary>
    /// <returns></returns>
    public IEnumerable<TValue> GetStoredItems() {
      foreach (var item in this) {
        if (item.Key>=0) {
          yield return item;
        }
      }
    }
    #endregion
  }
}
