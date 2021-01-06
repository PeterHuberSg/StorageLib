/**************************************************************************************

StorageLib.DataContextBase
==========================

The DataContext holds all DataStores and is used by the application code to access them. The DataContext also 
provides transaction support. StorageClassGenerator generates a class XxxDataContext (name can be configured) which
inherits from DataContextBase.

Written in 2020 by Jürgpeter Huber 
Contact: PeterCode at Peterbox dot com

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;


namespace StorageLib {


  /// <summary>
  /// The DataContext holds all DataStores and is used by the application code to access them. The DataContext also 
  /// provides transaction support. StorageClassGenerator generates a class XxxDataContext (name can be configured) which
  /// inherits from DataContextBase.
  /// </summary>
  public abstract class DataContextBase: IDisposable {

    #region Properties
    //      ----------

    /// <summary>
    /// StartTransaction() sets it true, CommitTransaction() and RollbackTransaction() set it false.
    /// </summary>
    public bool IsTransaction { get; protected set; }


    /// <summary>
    /// Gives access to any DataStore through its index.
    /// </summary>
    public DataStore[] DataStores { get; protected set; }


    /// <summary>
    /// Holds any item that was stored, updated or removed during the current transaction. It also holds the data needed
    /// to restore the old values of the item.
    /// </summary>
    internal List<TransactionItem> TransactionItems;


    /// <summary>
    /// Adds transactionItem to TransactionItems
    /// </summary>
    virtual protected void AddTransaction(TransactionItem transactionItem) {
      TransactionItems.Add(transactionItem);
    }

    /// <summary>
    /// Marks any DataStore that was changed during the current transaction
    /// </summary>
    internal bool[] TransactionStoreFlags;
    #endregion


    #region Constructor
    //      -----------

    /// <summary>
    /// Default DataContextBase constructor initialises few variables which are none generic.
    /// </summary>
    public DataContextBase(int DataStoresCount) {
      if (!IsStaticDisposed) {
        throw new Exception("Dispose old DC before creating a new one.");
      }
      isStaticDisposed = 0;

      DataStores = new DataStore[DataStoresCount];
      TransactionItems = new List<TransactionItem>();
      TransactionStoreFlags = new bool[DataStoresCount];
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Trace messages used by DataContextBase
    /// </summary>
    public enum TraceMessageEnum {
      none,
      StartTransaction,
      CommitTransaction,
      RollingbackTransaction,
      RolledbackTransaction
    }


    /// <summary>
    /// Inheritor can override TraceFromBase() if trace messages from DataContextBase should be processed.
    /// </summary>
    protected virtual void TraceFromBase(TraceMessageEnum traceMessageEnum) { 
    }


    /// <summary>
    /// Starts a transaction. All DataStores changes will be traced until a commit or rollback.
    /// </summary>
    public void StartTransaction() {
      if (IsTransaction) {
        throw new Exception("Commit or roll back the currently running transaction before starting a new one.");
      }
      IsTransaction = true;
      startTransactio();
    }


    /// <summary>
    /// If a transaction is already running, returns false, otherwise starts a transaction and returns true. 
    /// All DataStores changes will be traced until a commit or rollback.
    /// </summary>
    public bool StartOrContinueTransaction() {
      if (IsTransaction) return false;
      
      IsTransaction = true;
      startTransactio();
      return true;
    }


    private void startTransactio() {
      TraceFromBase(TraceMessageEnum.StartTransaction);
#if DEBUG
      if (TransactionItems.Count>0) throw new Exception();
#endif
    }

    /// <summary>
    /// Ends a transaction. All changes to DataStores traced during the current transaction get deleted.
    /// </summary>
    public void CommitTransaction() {
      if (!IsTransaction) {
        throw new Exception("It is not possible to commit a transaction, there is no transaction active.");
      }
      IsTransaction = false;
      TraceFromBase(TraceMessageEnum.CommitTransaction);
      for (int storeIndex = 0; storeIndex < TransactionStoreFlags.Length; storeIndex++) {
        if (TransactionStoreFlags[storeIndex]) {
          TransactionStoreFlags[storeIndex] = false;
          DataStores[storeIndex].CommitTransaction();
        }
      }
      /*+
      for (int transactionItemsIndex = 0; transactionItemsIndex < TransactionItems.Count; transactionItemsIndex++) {
        var transactionItem = TransactionItems[transactionItemsIndex];
        DataStores[(int)transactionItem.StoreKey].CommitItem(transactionItem);
      }
      +*/
      TransactionItems.Clear();
    }


    /// <summary>
    /// Reverses all DataStores changes made during the current transaction.
    /// </summary>
    public void RollbackTransaction() {
      if (!IsTransaction) {
        throw new Exception("It is not possible to roll back a transaction, there is no transaction active.");
      }
      IsTransaction = false;
      TraceFromBase(TraceMessageEnum.RollingbackTransaction);
      //rollback the changes since transaction started in each DataStore which was changed during the transaction.
      for (int storeIndex = 0; storeIndex < (int)TransactionStoreFlags.Length; storeIndex++) {
        if (TransactionStoreFlags[storeIndex]) {
          TransactionStoreFlags[storeIndex] = false;
          DataStores[storeIndex].RollbackTransaction();
        }
      }
      //rollback the action done on each item since transaction started like updating parent child relationships
      for (int transactionItemsIndex = TransactionItems.Count-1; transactionItemsIndex>=0; transactionItemsIndex--) {
        var transactionItem = TransactionItems[transactionItemsIndex];
        DataStores[(int)transactionItem.StoreKey].RollbackItem(transactionItem);
      }
      TransactionItems.Clear();
      TraceFromBase(TraceMessageEnum.RolledbackTransaction);
    }
    #endregion


    #region IDisposable Support
    //      -------------------

    /// <summary>
    /// Releases all resource used by the DataContext, like files used to store data permanently
    /// </summary>
    public void Dispose() {
      var wasDisposed = Interlocked.Exchange(ref isStaticDisposed, 1);//prevents that 2 threads dispose simultaneously
      if (wasDisposed==1) return; // already disposed

      Dispose(true);

      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Is DataContextBase already disposed ?
    /// </summary>
    public bool IsStaticDisposed {
      get { return isStaticDisposed==1; }
    }
    static int isStaticDisposed =1;


    protected virtual void Dispose(bool disposing) {
      if (disposing) {
        //release big properties. This helps in performance measurements, when several DataContextBases are created
        //sequentially and the garbage collector is run in between.
        DataStores = null!;
        TransactionItems = null!;
      }
    }
    #endregion

  }
}
