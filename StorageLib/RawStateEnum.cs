using System;
using System.Collections.Generic;
using System.Text;

namespace StorageLib {


  /// <summary>
  /// Indicates if a class instance read from a CSV file is marked Read, Updated or Deleted. New means it was not
  /// read from a CSV file.
  /// </summary>
  public enum RawStateEnum {
    New,
    Read,
    Updated,
    Deleted
  }
}
