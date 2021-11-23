using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;


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
    /// Returns TValue for iReadOnlyDictionary[TKey]. If TKey is not in iReadOnlyDictionary, the next higher TKey is 
    /// searched and its TValue returned. If TKey is greater than any TKey in iReadOnlyDictionary, TValue for the 
    /// highest TKey in iReadOnlyDictionary gets returned. An Exception is thrown if iReadOnlyDictionary is not a 
    /// SortedList. If sortedList is empty and TValue a class, null gets returned. If TValue is a struct, an
    /// Exception gets thrown.
    /// </summary>
    [return: MaybeNull]
    public static TValue GetEqualGreater<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> iReadOnlyDictionary, TKey key)
      where TKey : notnull
    {
      var sortedList = (SortedList<TKey, TValue>) iReadOnlyDictionary;
      return sortedList.GetEqualGreater(key);
    }
    

    /// <summary>
    /// Returns TValue for sortedList[TKey]. If TKey is not in SortedList, the next higher TKey is searched and
    /// its TValue returned. If TKey is greater than any TKey in SortedList, TValue for the highest TKey in SortedList
    /// gets returned. If sortedList is empty and TValue a class, null gets returned. If TValue is a struct, an
    /// Exception gets thrown.
    /// </summary>
    [return: MaybeNull]
    public static TValue GetEqualGreater<TKey, TValue>(this SortedList<TKey, TValue> sortedList, TKey key)
      where TKey: notnull 
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
    /// Returns KeyValuePair<TKey, TValue>? for sortedList[TKey]. If TKey is not in SortedList, the next higher TKey 
    /// is searched and its KeyValuePair<TKey, TValue> returned. If TKey is greater than any TKey in SortedList, 
    /// KeyValuePair<TKey, TValue> for the highest TKey in SortedList gets returned. If sortedList is empty and TValue 
    /// a class, null gets returned. If TValue is a struct, an Exception gets thrown.
    /// </summary>
    public static KeyValuePair<TKey, TValue>? GetEqualGreaterKVP<TKey, TValue>(this SortedList<TKey, TValue> sortedList, TKey key)
      where TKey : notnull {
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
  }
}
