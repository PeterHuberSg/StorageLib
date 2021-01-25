/**************************************************************************************

StorageLib.StorageSortedList
============================

StorageSortedList is a replacement for SortedList used by parent classes with releasable children. 

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
  /// StorageSortedList is a replacement for StoredList used by parent classes with releasable children. A stored parent might
  /// have stored and not stored children. Enumerating StorageSortedList shows only stored children. GetAll() supports
  /// enumerating all children. Count() counts only stored children, while CountAll() counts all. 
  /// </summary>
  public class StorageSortedList<TParent, TKey, TChild>: IDictionary<TKey, TChild>, IStorageReadOnlyDictionary<TKey, TChild>
    where TParent : class, IStorageItemGeneric<TParent>
    where TKey : notnull
    where TChild : class, IStorageItemGeneric<TChild> {

    #region Properties
    //      ----------

    /// <summary>
    /// Owner of StorageSortedList
    /// </summary>
    public TParent Parent { get; }

    /// <summary>
    /// Counting only stored children for a stored parent, but all children for a not stored parent, which are all not stored.
    /// </summary>
    public int Count {
      get {
        if (Parent.Key>=0) {
          //parent is stored => count only stored children
          var count = 0;
          foreach (var item in sortedList) {
            if (item.Value.Key>=0) count++;
          }
          return count;
        } else {
          return sortedList.Count;
        }
      }
    }


    /// <summary>
    /// Counting alll children.
    /// </summary>
    public int CountAll => sortedList.Count;


    /// <summary>
    /// Required by ICollection
    /// </summary>
    public bool IsReadOnly => false;

    public ICollection<TKey> Keys => sortedList.Keys;

    public ICollection<TChild> Values => sortedList.Values;

    IEnumerable<TKey> IReadOnlyDictionary<TKey, TChild>.Keys => sortedList.Keys;

    IEnumerable<TChild> IReadOnlyDictionary<TKey, TChild>.Values => sortedList.Values;

    TChild IReadOnlyDictionary<TKey, TChild>.this[TKey key] => sortedList[key];

    public TChild this[TKey key] { get => sortedList[key]; set => sortedList[key]=value; }
    #endregion


    #region Constructor
    //     ------------
    readonly SortedList<TKey, TChild> sortedList;


    public StorageSortedList(TParent parent) {
      Parent = parent;
      sortedList = new();

    }
    #endregion


    #region Methods
    //      -------

    public void Add(TKey key, TChild value) {
      sortedList.Add(key, value);
    }

    public bool ContainsKey(TKey key) {
      return sortedList.ContainsKey(key);
    }

    public bool Remove(TKey key) {
      return sortedList.Remove(key);
    }

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TChild value) {
      return sortedList.TryGetValue(key, out value);
    }

    public void Add(KeyValuePair<TKey, TChild> item) {
      sortedList.Add(item.Key, item.Value);
    }

    public void Clear() {
      sortedList.Clear();
    }

    public bool Contains(KeyValuePair<TKey, TChild> item) {
      return ((ICollection<KeyValuePair<TKey, TChild>>)sortedList).Contains(item);
    }

    public void CopyTo(KeyValuePair<TKey, TChild>[] array, int arrayIndex) {
      ((ICollection<KeyValuePair<TKey, TChild>>)sortedList).CopyTo(array, arrayIndex);
    }

    public bool Remove(KeyValuePair<TKey, TChild> item) {
      return sortedList.Remove(item.Key);
    }
    //public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TChild value) {
    //  throw new System.NotImplementedException();
    //}

    #endregion


    #region Iterators
    //     ----------

    /// <summary>
    /// None generic iterator, showing only stored children for a stored parent, but all children for a not stored parent
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }


    /// <summary>
    /// Generic iterator, showing only stored children for a stored parent, but all children for a not stored parent
    /// </summary>
    public IEnumerator<KeyValuePair<TKey, TChild>> GetEnumerator() {
      if (Parent.Key>=0) {
        //parent is stored => return only stored children
        foreach (var item in sortedList) {
          if (item.Value.Key>=0) {
            yield return item;
          }
        }
      } else {
        //parent is not stored => none of the children is stored
        foreach (var item in sortedList) {
          yield return item;
        }
      }
    }


    /// <summary>
    /// Iterator showing all children
    /// </summary>
    public IEnumerable<KeyValuePair<TKey, TChild>> GetAll() {
      foreach (var item in sortedList) {
        yield return item;
      }
    }

    IEnumerable<KeyValuePair<TKey, TChild>> IStorageReadOnlyCollection<KeyValuePair<TKey, TChild>>.GetAll() {
      return (IEnumerable<KeyValuePair<TKey, TChild>>)GetAll();
    }

    //IEnumerator<KeyValuePair<TKey, TChild>> IEnumerable<KeyValuePair<TKey, TChild>>.GetEnumerator() {
    //  throw new System.NotImplementedException();
    //}
    #endregion
  }
}
