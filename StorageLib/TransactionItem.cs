using System;
using System.Collections.Generic;
using System.Text;

namespace StorageLib {

  /// <summary>
  /// Type of TransactionItem activity
  /// </summary>
  public enum TransactionActivityEnum {
    None,
    New,
    Store,
    Update,
    Release
  }


  /// <summary>
  /// Information about one transaction activities (Add, Update or Remove). One Transaction can have several TransactionItems.
  /// </summary>
  public readonly struct TransactionItem {

    #region Properties
    //      ----------

    /// <summary>
    /// Identifies DataStore involved in the transaction
    /// </summary>
    public readonly int StoreKey;


    /// <summary>
    /// Type of TransactionItem activity
    /// </summary>
    public readonly TransactionActivityEnum TransactionActivity;


    /// <summary>
    /// Old key of transaction item. This is needed to undelete an item.
    /// </summary>
    public readonly int Key;


    /// <summary>
    /// Item involved in this part of the transaction
    /// </summary>
    public readonly IStorageItem Item;


    /// <summary>
    /// Index into items[], is needed to rollback deletion
    /// </summary>
    public readonly int Index;


    /// <summary>
    /// Value of item before update happened. This info is needed to undo an update.
    /// </summary>
    public readonly IStorageItem? OldItem;
    #endregion


    #region Constructor
    //      -----------

    /// <summary>
    /// Constructor
    /// </summary>
    public TransactionItem(
      int storeKey,
      TransactionActivityEnum transactionActivity,
      int key,
      IStorageItem item,
      int index = int.MinValue,
      IStorageItem? oldItem = null) 
    {
      StoreKey = storeKey;
      TransactionActivity = transactionActivity;
      Key = key;
      Item = item;
      Index = index;
      OldItem = oldItem;
    }
    #endregion

    #region Methods
    //      -------

    /// <summary>
    /// Writes some important about the TransactionItem into a string.
    /// </summary>
    public override string ToString() {
      return $"StoreKey: {StoreKey}; TransactionActivity: {TransactionActivity}; Key: {Key}; " +
        $"Item: {Item.ToShortString()}; OldItem: {OldItem?.ToShortString()};";
    }
    #endregion
  }
}