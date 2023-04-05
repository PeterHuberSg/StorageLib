/**************************************************************************************

StorageLib.IStorage
===================

Interfaces for Storage

Written in 2020 by Jürgpeter Huber 
Contact: https://github.com/PeterHuberSg/StorageLib

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/
using System;


/*
Storage uses 2 Interfaces:
--------------------------

IStorageItem<TItem>: IStorageItem
Implemented by classes who want to store their data in DataStore

IStorageItem
None generic version of IStorageItem<TItem>, used for transaction processing mostly
*/

namespace StorageLib {


  #region StorageItem Interfaces
  //      ----------------------

  /// <summary>
  /// Gives none generic access to IStorageItem<TItem>, used only for transaction processing
  /// </summary>
  public interface IStorageItem {

    #region Properties
    //      ----------

    /// <summary>
    /// Unique key, used to identify item
    /// </summary>
    public int Key { get; }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Returns a string for tracing, parents are shown only with their Key number
    /// </summary>
    public string ToTraceString();


    /// <summary>
    /// Returns a shorter string than ToString()
    /// </summary>
    public string ToShortString();
    #endregion
  }
  #endregion


  #region Generic StorageItem Interfaces
  //      ------------------------------

  /// <summary>
  /// Inheriting classes can be written to and read from a CSV file,
  /// </summary>
  public interface IStorageItem<TItem>: IStorageItem where TItem : class {

    #region Events
    //      ------

    /// <summary>
    /// Called when some properties of the class have been changed, which usually requires that the
    /// class gets written to the CSV file.
    /// </summary>
    public event Action</*old*/TItem, /*new*/TItem>? HasChanged;
    #endregion
  }
  #endregion
}