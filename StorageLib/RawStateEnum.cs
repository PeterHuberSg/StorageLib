/**************************************************************************************

StorageLib.RawStateEnum
=======================

Enumeration of the state of data

Written in 2020 by Jürgpeter Huber 
Contact: https://github.com/PeterHuberSg/StorageLib

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/
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
