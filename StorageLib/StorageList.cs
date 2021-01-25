/**************************************************************************************

StorageLib.StorageList
======================

StorageList is a replacement for List used by parent classes with releasable children. 

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


namespace StorageLib {
  //Visual Studio keeps crashing when inheriting from List<T> and creating a 'new' Count property. Because of this, 
  //StorageList was made, which does not inherit from List<T> but has a List<T> and all the important class members
  //of List<T>.


  /// <summary>
  /// StorageList is a replacement for List used by parent classes with releasable children. A stored parent might
  /// have stored and not stored children. For a stored parent, Enumerating StorageList shows only stored chidren. 
  /// GetAll() supports enumerating all children. Count() counts only stored children, while CountAll() counts all.
  /// </summary>
  public class StorageList<TParent, TChild>: ICollection<TChild>, IStorageReadOnlyList<TChild>
    where TParent : class, IStorageItemGeneric<TParent>
    where TChild : class, IStorageItemGeneric<TChild> 
  {
    // only ICollection is supported, not IList, because overwriting a child (using and indes) is not meaningful,
    // only Add() and Remove() is required.
    #region Properties
    //      ----------

    /// <summary>
    /// Owner of StorageList
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
          foreach (var item in list) {
            if (item.Key>=0) count++;
          }
          return count;
        } else {
          return list.Count;
        }
      }
    }


    /// <summary>
    /// Counting alll children.
    /// </summary>
    public int CountAll => list.Count;


    /// <summary>
    /// Gives access to all stored and not stored items
    /// </summary>
    public TChild this[int index] => list[index];


    /// <summary>
    /// Required by ICollection
    /// </summary>
    public bool IsReadOnly => false;
    #endregion


    #region Constructor
    //     ------------
    readonly List<TChild> list;

    /// <summary>
    /// constructor
    /// </summary>
    public StorageList(TParent parent) {
      Parent = parent;
      list = new();
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Adds an item to the end of StorageList
    /// </summary>
    public void Add(TChild item) {
      list.Add(item);
    }


    /// <summary>
    /// Removes all items from StorageList
    /// </summary>
    public void Clear() {
      list.Clear();
    }


    /// <summary>
    /// Determines whether item is in StorageList
    /// </summary>
    public bool Contains(TChild item) {
      return list.Contains(item);
    }


    /// <summary>
    /// Copies the entire StorageList to a compatible one-dimensional array, starting at the specified index of the target array.
    /// </summary>
    public void CopyTo(TChild[] array, int arrayIndex) {
      list.CopyTo(array, arrayIndex);
    }

    /// <summary>
    /// Removes an item at the end of StorageList
    /// </summary>
    public bool Remove(TChild item) {
      return list.Remove(item);
    }
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
    public IEnumerator<TChild> GetEnumerator() {
      if (Parent.Key>=0) {
        //parent is stored => return only stored children
        foreach (var item in list) {
          if (item.Key>=0) {
            yield return item;
          }
        }
      } else {
        //parent is not stored => none of the children is stored
        foreach (var item in list) {
          yield return item;
        }
      }
    }


    /// <summary>
    /// Iterator showing all children
    /// </summary>
    /// <returns></returns>
    public IEnumerable<TChild> GetAll() {
      foreach (var item in list) {
        yield return item;
      }
    }

    #endregion
  }

}
