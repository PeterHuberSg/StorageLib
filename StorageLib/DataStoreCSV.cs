﻿/**************************************************************************************

StorageLib.DataStoreCSV
=======================

Stores items in a DataStore and permanently in a CSV (comma separated value) file. An item can be accessed by its 
key. When starting, the file content gets read into the DataStoreCSV, if a file exists, otherwise an empty file with 
only the header definition gets written. Items added to or deleted from DataStoreCSV and items with changed content get 
continuously written to the file. 

Written in 2020 by Jürgpeter Huber 
Contact: https://github.com/PeterHuberSg/StorageLib

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Timers;


namespace StorageLib {


  /// <summary>
  /// Stores items in a DataStore and permanently in a CSV (comma separated value) file. When starting, the file 
  /// content gets read into the DataStoreCSV, if a file exists, otherwise an empty file with only the header definition 
  /// gets written. Items added to or deleted from DataStoreCSV and items with changed content get continuously written 
  /// to the file. If there is no write activity, a flush timer ensures that the writes are committed to the hard disk.
  /// Disposing the DataStoreCSV ensures that all data is flushed to the file if only new files were added or the complete 
  /// DataStoreCSV is rewritten, which eliminates all the updated and deleted lines.
  /// </summary>
  public class DataStoreCSV<TItemCSV>: DataStore<TItemCSV> 
    where TItemCSV : class, IStorageItem<TItemCSV>
  {

    #region Properties
    //      ----------

    /// <summary>
    /// Configuration data of CSV files
    /// </summary>
    public readonly CsvConfig CsvConfig;


    /// <summary>
    /// Estimated length of TItemCSV when stored as string. Could be too long, but not too short. 10*EstimatedLineLength should be shorter
    /// than CsvConfig.BufferSize
    /// </summary>
    public int EstimatedLineLength { get; private set; }


    /// <summary>
    /// Path and file name 
    /// </summary>
    public readonly string PathFileName;


    /// <summary>
    /// Headers of the item, separated by delimiter 
    /// </summary>
    public readonly string CsvHeaderString;


    /// <summary>
    /// Delay in millisecond before flush gets executed after the last write
    /// </summary>
    public readonly int FlushDelay;


    /// <summary>
    /// While DataStoreCSV gets disposed, the present .csv file, which might contain update and deletion instructions
    /// gets renamed as .bak file and a new, compacter .csv file gets written which contains only create instructions.
    /// If dispose is not executed properly, DataStoreCSV reads the .csv file with update and deletion instructions and
    /// sets IsReadFromBakFile.
    /// </summary>
    public bool IsReadFromBakFile { get; private set; }
    #endregion

    #region Constructor
    //     ------------

    Func<int, CsvReader, DataStoreCSV<TItemCSV>, TItemCSV?> create;
    Func<TItemCSV, bool>? verify;
    Action<TItemCSV, CsvReader, DataStoreCSV<TItemCSV>>? update;
    Action<TItemCSV, CsvWriter>? write;
    Action<TItemCSV>? disconnect;

    readonly bool isInitialReading;
    FileStream? fileStream;
    CsvWriter? csvWriter;

    //Timer? flushTimer;


    /// <summary>
    /// Constructs a DataStoreCSV. If a CSV file exists already, its content gets read at startup. If no CSV file 
    /// exists, an empty one gets created with a header line.
    /// </summary>
    /// <param name="dataContext">DataContext creating this DataStore</param>
    /// <param name="storeKey">Unique number to identify DataStore</param>
    /// <param name="csvConfig">File name and other parameters for CSV file</param>
    /// <param name="estimatedLineLength">Estimated number of bytes needed to write one line</param>
    /// <param name="headers">Name for each item property</param>
    /// <param name="setKey">Called when an item gets added to set its Key</param>
    /// <param name="create">Creates a new item with one line read from the CSV file</param>
    /// <param name="verify">Verifies item, for example it parent(s) exist</param>
    /// <param name="update">Updates an item if an line with updates is read</param>
    /// <param name="write">Writes item to CSV file</param>
    /// <param name="disconnect">Disconnects item from parents, data context dictionaries, etc.</param>
    /// <param name="rollbackItemNew">Undo of data change in item during transaction due to item constructor()</param>
    /// <param name="rollbackItemStore">Undo of data change in item during transaction due to item.Store()</param>
    /// <param name="rollbackItemUpdate">Undo of data change in item during transaction due to item.Update()</param>
    /// <param name="rollbackItemRemove">Undo of data change in item during transaction due to item.Remove()</param>
    /// <param name="areInstancesUpdatable">Can the property of an item change ?</param>
    /// <param name="areInstancesReleasable">Can an item be removed from DataStoreCSV</param>
    /// <param name="capacity">How many items should DataStoreCSV by able to hold initially ?</param>
    /// <param name="flushDelay">When the items in DataStoreCSV are not changed for flushDelay milliseconds, the internal
    /// buffer gets written to the CSV file.</param>

    public DataStoreCSV(
      DataContextBase? dataContext,
      int storeKey,
      CsvConfig csvConfig,
      int estimatedLineLength,
      string[] headers,
      Action<IStorageItem, int, /*isRollback*/bool> setKey,
      Func<int, CsvReader, DataStoreCSV<TItemCSV>, TItemCSV> create,
      Func<TItemCSV, bool>? verify,
      Action<TItemCSV, CsvReader, DataStoreCSV<TItemCSV>>? update,
      Action<TItemCSV, CsvWriter> write,
      Action<TItemCSV>? disconnect,
      Action<IStorageItem> rollbackItemNew,
      Action<IStorageItem> rollbackItemStore,
      Action</*old*/IStorageItem, /*new*/IStorageItem>? rollbackItemUpdate,
      Action<IStorageItem>? rollbackItemRemove,
      bool areInstancesUpdatable = false,
      bool areInstancesReleasable = false,
      int capacity = 0,
      int flushDelay = 200) : base(dataContext, storeKey, setKey, rollbackItemNew, rollbackItemStore, 
        rollbackItemUpdate, rollbackItemRemove, areInstancesUpdatable, areInstancesReleasable, capacity) 
    {
      CsvConfig = csvConfig;
      EstimatedLineLength = estimatedLineLength;
      CsvHeaderString = Csv.ToCsvHeaderString(headers, csvConfig.Delimiter);
      this.create = create;
      this.verify = verify;
      this.update= update;
      this.write = write;
      this.disconnect = disconnect;

      PathFileName = Csv.ToPathFileName(csvConfig, typeof(TItemCSV).Name);
      fileStream = new FileStream(PathFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, csvConfig.BufferSize, FileOptions.SequentialScan);
      if (fileStream.Length>0) {
        IsNew = false;
        var csvReader = new CsvReader(null, CsvConfig, estimatedLineLength, fileStream);
        isInitialReading = true;
        try {
          readFromCsvFile(csvReader);
        } catch {
          fileStream.Dispose();
          throw;
        }
        isInitialReading = false;
        fileStream.Position = fileStream.Length;
        csvWriter = new CsvWriter("", csvConfig, estimatedLineLength, fileStream, flushDelay: flushDelay);
      } else {
        //there is no file yet. Write an empty file with just the CSV header
        csvWriter = new CsvWriter("", csvConfig, estimatedLineLength, fileStream, flushDelay: flushDelay);
        WriteToCsvFile(csvWriter);
      }
      //flushTimer = new Timer(flushTimerMethod, null, Timeout.Infinite, Timeout.Infinite);
      FlushDelay = flushDelay;
    }

    //todo: Add constructor with capacity, store number of records per DataStore in additional file, size datastore properly at startup
    #endregion


    #region IDispose interface
    //      ------------------

    readonly object disposingLock = new object();


    protected override void Dispose(bool disposing) {
      lock (disposingLock) {
        var wasCsvWriter = Interlocked.Exchange(ref csvWriter, null);
        if (wasCsvWriter!=null) {
          try {
            lock (wasCsvWriter) {
              wasCsvWriter.Dispose();

              fileStream?.Dispose();
              fileStream = null;
            }
          } catch (Exception ex) {
            CsvConfig.ReportException?.Invoke(ex);
          }
        }

        try {
          var backupFileName = PathFileName[..^3] + "bak";
          if (File.Exists(backupFileName)) {
            File.Delete(backupFileName);
          }
          if (AreItemsUpdated || AreItemsDeleted) {
            //backup file and create a new one with only the latest items, but no deletions nor updates
            File.Move(PathFileName, backupFileName);
            using var fileStream = new FileStream(PathFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, CsvConfig.BufferSize, FileOptions.SequentialScan);
            using var csvWriter = new CsvWriter("", CsvConfig, EstimatedLineLength, fileStream);
            WriteToCsvFile(csvWriter);
          }
        } catch (Exception ex) {
          CsvConfig.ReportException?.Invoke(ex);
        }

        create = null!;
        verify = null;
        update = null;
        write = null;
        disconnect = null;
      }

      base.Dispose(disposing);
    }
    #endregion


    #region Methods
    //      -------

    /// <summary>
    /// Initiates that all data presently in RAM write buffers are written immediately to a file. Usually buffers get 
    /// written when they are full, the CSVWriter.FlushTimer runs or the DataStore gets disposed. For normal operation
    /// it should not be necessary to call Flush(), it is mainly used for time measurement.
    /// </summary>
    public override void Flush() {
      csvWriter?.Flush();
    }


    private void readFromCsvFile(CsvReader csvReader) {
      //verify headers line
      var headerLine = csvReader.ReadLine();
      if (CsvHeaderString!=headerLine) throw new Exception($"Error reading file {csvReader.FileName}{Environment.NewLine}'" + 
        headerLine + "' should be '" + CsvHeaderString + "'.");

      //read data lines
      var errorStringBuilder = new StringBuilder();
      var isFirstAdd = true;
      int lastKey = 0;
      while (!csvReader.IsEndOfFileReached()) {
        if (IsReadOnly) {
          addItem(LastItemIndex+1, csvReader, errorStringBuilder);
 
        } else {
          var firstLineChar = csvReader.ReadFirstLineChar();
          if (firstLineChar==CsvConfig.LineCharAdd) {
            var newKey = csvReader.ReadInt();
            if (AreKeysContinuous) {
              if (isFirstAdd) {
                isFirstAdd = false;
              } else {
                AreKeysContinuous = newKey == lastKey + 1;
                if (!AreInstancesReleasable && !AreKeysContinuous) {
                  throw new IndexOutOfRangeException($"Reading {typeof(TItemCSV).Name}: Key {newKey} is out of sequence, it should be {lastKey + 1}.");
                }
              }
              lastKey = newKey;
            }
            addItem(newKey, csvReader, errorStringBuilder);

          } else if (firstLineChar==CsvConfig.LineCharDelete) {
            //delete
            IsReadFromBakFile = true;
            int key = csvReader.ReadInt();
            var item = this[key];
            csvReader.SkipToEndOfLine();
            if (!Remove(key)) {
              errorStringBuilder.AppendLine($"Deletion Line with key '{key}' did not exist in StorageDictionary.");
            }
            //Todo: I guess Disconnect() should no longer be called, since delete, i.e. release no longer disconnects child from parent
            disconnect!(item);

          } else if (firstLineChar==CsvConfig.LineCharUpdate) {
            //update
            IsReadFromBakFile = true;
            var key = csvReader.ReadInt();
            var item = this[key];
            update!(item, csvReader, this);
            csvReader.ReadEndOfLine();

          } else {
            throw new Exception($"Error reading file {csvReader.FileName}{Environment.NewLine}" +
              $"First character should be '{CsvConfig.LineCharAdd}', '{CsvConfig.LineCharDelete}'or " +
              $"'{CsvConfig.LineCharUpdate}', but was '{firstLineChar}'.{Environment.NewLine}" + csvReader.GetPresentContent());
          }
        }
      }

      if (verify!=null) {
        foreach (var item in this) {
          if (!verify(item)) {
            errorStringBuilder.AppendLine($"DataStoreCSV<{typeof(TItemCSV).Name}>: item Key: {item.Key} Content: '{item}' could not be validated in {PathFileName}.");
          }
        }
      }

      if (errorStringBuilder.Length>0) {
        throw new Exception($"Errors reading file {csvReader.FileName}, problems on following lines:" + Environment.NewLine +
          errorStringBuilder.ToString());
      }
      //UpdateAreKeysContinuous();
    }


    private void addItem(int newKey, CsvReader csvReader, StringBuilder errorStringBuilder) {
      TItemCSV? item;
      try {
        item = create(newKey, csvReader, this);
      } catch (Exception ex) {
        if (errorStringBuilder.Length>0) {
          errorStringBuilder.AppendLine();
        }
        errorStringBuilder.AppendLine($"Error when reading from file '{csvReader.FileName}'.");
        errorStringBuilder.AppendLine($"At '{csvReader.PresentContent}'");
        throw new Exception(errorStringBuilder.ToString(), ex);
      }
      if (errorStringBuilder.Length==0) {
        AddProtected(item!);
      }
      csvReader.ReadEndOfLine();
    }


    /// <summary>
    /// Writes all items in DataStoreCSV to a CSV file
    /// </summary>
    public void WriteToCsvFile(CsvWriter csvWriter) {
      csvWriter.WriteLine(CsvHeaderString);
      foreach (TItemCSV item in this) {
        if (item!=null) {
          if (IsReadOnly) {
            csvWriter.StartNewLine();
          } else {
            csvWriter.WriteFirstLineChar(CsvConfig.LineCharAdd);
            csvWriter.Write(item.Key);
          }
          write!(item, csvWriter);
          csvWriter.WriteEndOfLine();
        }
      }
    }


    #region Overrides
    //      ---------

    protected override void OnItemAdded(TItemCSV item) {
      if (isInitialReading) return;

      try {
        lock (csvWriter!) {
          if (IsReadOnly) {
            csvWriter.StartNewLine();
          } else {
            csvWriter.WriteFirstLineChar(CsvConfig.LineCharAdd);
            csvWriter.Write(item.Key);
          }
          write!(item, csvWriter);
          csvWriter.WriteEndOfLine();
        }
        //kickFlushTimer();
      } catch (Exception ex) {
        csvWriter!.CleanupAfterException();
        CsvConfig.ReportException?.Invoke(ex);
      }
    }


    protected override void OnItemHasChanged(TItemCSV oldItem, TItemCSV newItem) {
      if (isInitialReading) return;

      try {
        lock (csvWriter!) {
          csvWriter.WriteFirstLineChar(CsvConfig.LineCharUpdate);
          csvWriter.Write(newItem.Key);
          write!(newItem, csvWriter);
          csvWriter.WriteEndOfLine();
        }
        //kickFlushTimer();
      } catch (Exception ex) {
        CsvConfig.ReportException?.Invoke(ex);
      }
    }


    protected override void OnItemRemoved(TItemCSV item) {
      if (isInitialReading) return;

      try {
        lock (csvWriter!) {
          csvWriter.WriteFirstLineChar(CsvConfig.LineCharDelete);
          csvWriter.Write(item.Key);
          //Todo: cannot write content of deleted item, because it might link to an already deleted parent
          //write!(item, csvWriter);
          csvWriter.WriteEndOfLine();
        }
        //kickFlushTimer();
      } catch (Exception ex) {
        CsvConfig.ReportException?.Invoke(ex);
      }
    }


    /// <summary>
    /// Reset any transaction related data. 
    /// </summary>
    protected override void OnStartTransaction() {
      csvWriter!.StartTransaction();
    }


    /// <summary>
    /// Reset any transaction related data. 
    /// </summary>
    protected override void OnCommitTransaction() {
      lock (csvWriter!) {
        csvWriter!.CommitTransaction();
      }
    }


    /// <summary>
    /// Undo any change on the hard disk that happened since a new transaction started. 
    /// </summary>
    protected override void OnRollbackTransaction() {
      lock (csvWriter!) {
        csvWriter!.RollbackTransaction();
      }
    }


    public override string ToString() {
      return base.ToString() + $"; MaxLine: {this.EstimatedLineLength};";
    }
    #endregion
    #endregion
  }
}
