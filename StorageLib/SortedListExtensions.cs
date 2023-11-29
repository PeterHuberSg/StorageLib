/**************************************************************************************

StorageLib.SortedListExtensions
===============================

Enumeration of the state of data

Written in 2020 by Jürgpeter Huber 
Contact: https://github.com/PeterHuberSg/StorageLib

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
#pragma warning disable IDE0046 // Convert to conditional expression


namespace StorageLib {

  /// <summary>
  /// SortedList Extensions returning TValue of next higher TKey, if TKey does not exist in SortedList
  /// </summary>
  public static class SortedListExtensions {


    /// <summary>
    /// Returns first key in IReadOnlyDictionary which must be a SortedList and not empty if TValue is a struct.
    /// </summary>
    [return: MaybeNull]
    public static TKey GetFirstKey<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> iReadOnlyDictionary)
      where TKey : notnull where TValue : class 
    {
      var sortedList = (SortedList<TKey, TValue>) iReadOnlyDictionary;
      return sortedList.GetFirstKey();
    }


    /// <summary>
    /// Returns first key in sortedList. It must not be empty if TValue is a struct.
    /// </summary>
    [return: MaybeNull]
    public static TKey GetFirstKey<TKey, TValue>(this SortedList<TKey, TValue> sortedList)
      where TKey : notnull 
    {
      if (sortedList.Count==0) {
        if (default(TValue) is null) {
          return default;
        } else {
          throw new Exception("SortedList.GetFirstKey() works only when SortedList has item(s).");
        }
      }
      return sortedList.Keys[0];
    }


    /// <summary>
    /// Returns last key in IReadOnlyDictionary which must be a SortedList and not empty if TValue is a struct.
    /// </summary>
    [return: MaybeNull]
    public static TKey GetLastKey<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> iReadOnlyDictionary)
      where TKey : notnull where TValue : class 
    {
      var sortedList = (SortedList<TKey, TValue>) iReadOnlyDictionary;
      return sortedList.GetLastKey();
    }


    /// <summary>
    /// Returns last key in sortedList. It must not be empty if TValue is a struct.
    /// </summary>
    [return: MaybeNull]
    public static TKey GetLastKey<TKey, TValue>(this SortedList<TKey, TValue> sortedList)
      where TKey : notnull 
    {
      if (sortedList.Count==0) {
        if (default(TValue) is null) {
          return default;
        } else {
          throw new Exception("SortedList.GetLastKey() works only when SortedList has item(s).");
        }
      }
      return sortedList.Keys[sortedList.Count-1];
    }


    /// <summary>
    /// Returns first key in IReadOnlyDictionary which must be a SortedList and not empty if TValue is a struct.
    /// </summary>
    [return: MaybeNull]
    public static TValue GetFirstItem<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> iReadOnlyDictionary)
      where TKey : notnull where TValue : class 
    {
      var sortedList = (SortedList<TKey, TValue>) iReadOnlyDictionary;
      return sortedList[sortedList.GetFirstKey()!];
    }


    /// <summary>
    /// Returns first key in sortedList. It must not be empty if TValue is a struct.
    /// </summary>
    [return: MaybeNull]
    public static TValue GetFirstItem<TKey, TValue>(this SortedList<TKey, TValue> sortedList)
      where TKey : notnull 
    {
      return sortedList[sortedList.GetFirstKey()!];
    }


    /// <summary>
    /// Returns last key in IReadOnlyDictionary which must be a SortedList and not empty if TValue is a struct.
    /// </summary>
    [return: MaybeNull]
    public static TValue GetLastItem<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> iReadOnlyDictionary)
      where TKey : notnull where TValue : class 
    {
      var sortedList = (SortedList<TKey, TValue>) iReadOnlyDictionary;
      return sortedList[sortedList.GetLastKey()!];
    }


    /// <summary>
    /// Returns last key in sortedList. It must not be empty if TValue is a struct.
    /// </summary>
    [return: MaybeNull]
    public static TValue GetLastItem<TKey, TValue>(this SortedList<TKey, TValue> sortedList)
      where TKey : notnull 
    {
      return sortedList[sortedList.GetLastKey()!];
    }


    /// <summary>
    /// Returns TValue? for IReadOnlyDictionary[key]. If there is no entry for TKey==key, TValue of the next bigger 
    /// TKey gets returned. The IReadOnlyDictionary must be a SortedList. If key is smaller than any TKey in the 
    /// SortedList, TValue of the smallest TKey in the SortedList gets returned. If key is greater than any TKey in 
    /// SortedList, TValue of the highest TKey in the SortedList gets returned. If SortedList is empty and TValue a 
    /// class, null gets returned. If TValue is a struct, an Exception gets thrown.
    /// </summary>
    [return: MaybeNull]
    public static TValue GetEqualGreater<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> iReadOnlyDictionary, TKey key)
      where TKey : notnull 
    {
      var sortedList = (SortedList<TKey, TValue>)iReadOnlyDictionary;
      return sortedList.GetEqualGreater(key);
    }


    /// <summary>
    /// Returns TValue? for SortedList. If there is no entry for TKey==key, TValue of the next bigger TKey gets 
    /// returned. If key is smaller than any TKey in the SortedList, TValue of the smallest TKey in the SortedList 
    /// gets returned. If key is greater than any TKey in SortedList, TValue of the highest TKey in the SortedList 
    /// gets returned. If SortedList is empty and TValue a class, null gets returned. If TValue is a struct, an 
    /// Exception gets thrown.
    /// </summary>
    [return: MaybeNull]
    public static TValue GetEqualGreater<TKey, TValue>(this SortedList<TKey, TValue> sortedList, TKey key)
      where TKey : notnull 
    {
      var comparer = sortedList.Comparer;

      if (sortedList.Count==0) {
        if (default(TValue) is null) {
          return default;
        } else {
          throw new Exception("SortedList.GetEqualGreater() works only when SortedList has item(s).");
        }
      }
      var firstKey = sortedList.Keys[0];
      if (comparer.Compare(firstKey, key)==1) return sortedList[firstKey];// item is missing, key is too small

      var lastKey = sortedList.Keys[sortedList.Count-1];
      if (comparer.Compare(lastKey, key)==-1) return sortedList[lastKey];// item is missing, key is too big

      //search
      int min = 0;
      int max = sortedList.Count-1;
      while (min<=max) {
        int mid = (min + max) / 2;
        var midKey = sortedList.Keys[mid];
        int compareResult = comparer.Compare(midKey, key);

        if (compareResult==0) {
          return sortedList[midKey];
        } else if (compareResult==1) {
          max = mid - 1;
        } else {
          min = mid + 1;
        }
      }
      return sortedList[sortedList.Keys[min]];
    }


    /// <summary>
    /// Returns KeyValuePair<TKey, TValue>? for IReadOnlyDictionary[key]. If there is no entry for TKey==key, the 
    /// KeyValuePair<TKey, TValue>? of the next bigger TKey gets returned. The IReadOnlyDictionary must be a 
    /// SortedList. If key is smaller than any TKey in the SortedList, the KeyValuePair<TKey, TValue>? of the smallest 
    /// TKey in the SortedList gets returned. If key is greater than any TKey in SortedList, the 
    /// KeyValuePair<TKey, TValue>? of the highest TKey in the SortedList gets returned. If SortedList is empty and 
    /// TValue a class, null gets returned. If TValue is a struct, an Exception gets thrown.
    /// </summary>
    [return: MaybeNull]
    public static KeyValuePair<TKey, TValue>? GetEqualGreaterKVP<TKey, TValue>(
      this IReadOnlyDictionary<TKey, TValue> iReadOnlyDictionary, TKey key)
      where TKey : notnull 
    {
      var sortedList = (SortedList<TKey, TValue>)iReadOnlyDictionary;
      return sortedList.GetEqualGreaterKVP(key);
    }


    /// <summary>
    /// Returns KeyValuePair<TKey, TValue>? for SortedList[key]. If there is no entry for TKey==key, the 
    /// KeyValuePair<TKey, TValue>? of the next bigger TKey gets returned. If key is smaller than any TKey in the 
    /// SortedList, the KeyValuePair<TKey, TValue>? of the smallest TKey in the SortedList gets returned. If key is 
    /// greater than any TKey in SortedList, the KeyValuePair<TKey, TValue>? of the highest TKey in the SortedList 
    /// gets returned. If SortedList is empty and TValue a class, null gets returned. If TValue is a struct, an 
    /// Exception gets thrown.
    /// </summary>
    public static KeyValuePair<TKey, TValue>? GetEqualGreaterKVP<TKey, TValue>(this SortedList<TKey, TValue> sortedList, TKey key)
      where TKey : notnull 
    {
      var comparer = sortedList.Comparer;

      if (sortedList.Count==0) {
        if (default(TValue) is null) {
          return default;
        } else {
          throw new Exception("SortedList.GetEqualGreater() works only when SortedList has item(s).");
        }
      }
      var firstKey = sortedList.Keys[0];
      if (comparer.Compare(firstKey, key)==1) return new KeyValuePair<TKey, TValue>(firstKey, sortedList[firstKey]);// item is missing, key is too small

      var lastKey = sortedList.Keys[sortedList.Count-1];
      if (comparer.Compare(lastKey, key)==-1) return new KeyValuePair<TKey, TValue>(lastKey, sortedList[lastKey]);// item is missing, key is too big

      //search
      int min = 0;
      int max = sortedList.Count-1;
      while (min<=max) {
        int mid = (min + max) / 2;
        var midKey = sortedList.Keys[mid];
        int compareResult = comparer.Compare(midKey, key);

        if (compareResult==0) {
          return new KeyValuePair<TKey, TValue>(midKey, sortedList[midKey]);
        } else if (compareResult==1) {
          max = mid - 1;
        } else {
          min = mid + 1;
        }
      }
      var minDate = sortedList.Keys[min];
      return new KeyValuePair<TKey, TValue>(minDate, sortedList[minDate]);
    }


    /// <summary>
    /// Returns TValue? for IReadOnlyDictionary[key]. If there is no entry for TKey==key, TValue of the next smaller 
    /// TKey gets returned. The IReadOnlyDictionary must be a SortedList. If key is smaller than any TKey in the 
    /// SortedList, TValue of the smallest TKey in the SortedList gets returned. If key is greater than any TKey in 
    /// SortedList, TValue of the highest TKey in the SortedList gets returned. If SortedList is empty and TValue a 
    /// class, null gets returned. If TValue is a struct, an Exception gets thrown.
    /// </summary>
    [return: MaybeNull]
    public static TValue GetEqualSmaller<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> iReadOnlyDictionary, TKey key)
      where TKey : notnull 
    {
      var sortedList = (SortedList<TKey, TValue>)iReadOnlyDictionary;
      return sortedList.GetEqualSmaller(key);
    }


    /// <summary>
    /// Returns TValue? for SortedList. If there is no entry for TKey==key, TValue of the next smaller TKey gets 
    /// returned. If key is smaller than any TKey in the SortedList, TValue of the smallest TKey in the SortedList 
    /// gets returned. If key is greater than any TKey in SortedList, TValue of the highest TKey in the SortedList 
    /// gets returned. If SortedList is empty and TValue a class, null gets returned. If TValue is a struct, an 
    /// Exception gets thrown.
    /// </summary>
    [return: MaybeNull]
    public static TValue GetEqualSmaller<TKey, TValue>(this SortedList<TKey, TValue> sortedList, TKey key)
      where TKey : notnull 
    {
      var comparer = sortedList.Comparer;

      if (sortedList.Count==0) {
        if (default(TValue) is null) {
          return default;
        } else {
          throw new Exception("SortedList.GetEqualSmaller() works only when SortedList has item(s).");
        }
      }
      var firstKey = sortedList.Keys[0];
      if (comparer.Compare(firstKey, key)==1) return sortedList[firstKey];// item is missing, key is too small

      var lastKey = sortedList.Keys[sortedList.Count-1];
      if (comparer.Compare(lastKey, key)==-1) return sortedList[lastKey];// item is missing, key is too big

      //search
      int min = 0;
      int max = sortedList.Count-1;
      while (min<=max) {
        int mid = (min + max) / 2;
        var midKey = sortedList.Keys[mid];
        int compareResult = comparer.Compare(midKey, key);

        if (compareResult==0) {
          return sortedList[midKey];
        } else if (compareResult==1) {
          max = mid - 1;
        } else {
          min = mid + 1;
        }
      }
      return sortedList[sortedList.Keys[max]];
    }


    /// <summary>
    /// Returns KeyValuePair<TKey, TValue>? for IReadOnlyDictionary[key]. If there is no entry for TKey==key, the 
    /// KeyValuePair<TKey, TValue>? of the next smaller TKey gets returned. The IReadOnlyDictionary must be a 
    /// SortedList. If key is smaller than any TKey in the SortedList, the KeyValuePair<TKey, TValue>? of the smallest 
    /// TKey in the SortedList gets returned. If key is greater than any TKey in SortedList, the 
    /// KeyValuePair<TKey, TValue>? of the highest TKey in the SortedList gets returned. If SortedList is empty and 
    /// TValue a class, null gets returned. If TValue is a struct, an Exception gets thrown.
    /// </summary>
    [return: MaybeNull]
    public static KeyValuePair<TKey, TValue>? GetEqualSmallerKVP<TKey, TValue>(
      this IReadOnlyDictionary<TKey, TValue> iReadOnlyDictionary, TKey key)
      where TKey : notnull 
    {
      var sortedList = (SortedList<TKey, TValue>)iReadOnlyDictionary;
      return sortedList.GetEqualSmallerKVP(key);
    }


    /// <summary>
    /// Returns KeyValuePair<TKey, TValue>? for SortedList[key]. If there is no entry for TKey==key, the 
    /// KeyValuePair<TKey, TValue>? of the next smaller TKey gets returned. If key is smaller than any TKey in the 
    /// SortedList, the KeyValuePair<TKey, TValue>? of the smallest TKey in the SortedList gets returned. If key is 
    /// greater than any TKey in SortedList, the KeyValuePair<TKey, TValue>? of the highest TKey in the SortedList 
    /// gets returned. If SortedList is empty and TValue a class, null gets returned. If TValue is a struct, an 
    /// Exception gets thrown.
    /// </summary>
    public static KeyValuePair<TKey, TValue>? GetEqualSmallerKVP<TKey, TValue>(this SortedList<TKey, TValue> sortedList, TKey key)
      where TKey : notnull 
    {
      var comparer = sortedList.Comparer;

      if (sortedList.Count==0) {
        if (default(TValue) is null) {
          return default;
        } else {
          throw new Exception("SortedList.GetEqualSmaller() works only when SortedList has item(s).");
        }
      }
      var firstKey = sortedList.Keys[0];
      if (comparer.Compare(firstKey, key)==1) return new KeyValuePair<TKey, TValue>(firstKey, sortedList[firstKey]);// item is missing, key is too small

      var lastKey = sortedList.Keys[sortedList.Count-1];
      if (comparer.Compare(lastKey, key)==-1) return new KeyValuePair<TKey, TValue>(lastKey, sortedList[lastKey]);// item is missing, key is too big

      //search
      int min = 0;
      int max = sortedList.Count-1;
      while (min<=max) {
        int mid = (min + max) / 2;
        var midKey = sortedList.Keys[mid];
        int compareResult = comparer.Compare(midKey, key);

        if (compareResult==0) {
          return new KeyValuePair<TKey, TValue>(midKey, sortedList[midKey]);
        } else if (compareResult==1) {
          max = mid - 1;
        } else {
          min = mid + 1;
        }
      }
      var minDate = sortedList.Keys[max];
      return new KeyValuePair<TKey, TValue>(minDate, sortedList[minDate]);
    }
  }
}
