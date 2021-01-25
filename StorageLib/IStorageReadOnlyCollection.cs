/**************************************************************************************

StorageLib.IStorageReadOnlyCollection
=====================================

IStorageReadOnlyCollection is a replacement for IReadOnlyList used by parent classes with releasable children, used by
StorageList, a replacement of 

Written in 2021 by Jürgpeter Huber 
Contact: PeterCode at Peterbox dot com

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace StorageLib {


  /// <summary>
  /// Like IReadOnlyCollection, but for StorageList, StorageDictionary, etc., which show 'normally' only stored children if 
  /// the parent is stored. 
  /// </summary>
  public interface IStorageReadOnlyCollection<T>: IReadOnlyCollection<T> {
    /// <summary>
    /// Itereate over all children. 'IEnumerator<TChild> GetEnumerator()' returns only stored children if parent is stored.
    /// </summary>
    public IEnumerable<T> GetAll();

    /// <summary>
    /// Counting all children. 'Count' counts only stored children if parent is stored.
    /// </summary>
    public int CountAll { get; }
  }


  /// <summary>
  /// Like IReadOnlyList, but for StorageList, which shows 'normally' only stored children if 
  /// the parent is stored. 
  /// </summary>
  public interface IStorageReadOnlyList<T>: IStorageReadOnlyCollection<T>, IReadOnlyList<T> { }


  /// <summary>
  /// Like IReadOnlyDictionary, but for StorageDictionary or StorageSortedList, which show 'normally' only stored children if 
  /// the parent is stored. 
  /// </summary>
  public interface IStorageReadOnlyDictionary<TKey, TValue>:
    IStorageReadOnlyCollection<KeyValuePair<TKey, TValue>>, IReadOnlyDictionary<TKey, TValue> { }
}
