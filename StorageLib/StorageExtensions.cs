/**************************************************************************************

StorageLib.StorageExtensions
============================

Extension used to display key as string.

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
  /// Extension used to display key as string.
  /// </summary>
  public static class StorageExtensions {

    public const int NoKey = -1;


    /// <summary>
    /// Display key as string
    /// </summary>
    public static string ToKeyString(this int key) {
      return key==NoKey ? "noKey" : key.ToString();
    }


    /// <summary>
    /// If Key is >=0 returns "@Key", otherwise "#GetHashCode()"
    /// </summary>
    public static string GetKeyOrHash(this IStorageItem storageItem) {
      return storageItem.Key>=0 ? '@'+storageItem.Key.ToString() : '#'+storageItem.GetHashCode().ToString();
    }


    public static string ToPureString(this char c) => c switch { 
      '\t'=> "\\t",
      '\r'=> "\\r",
      '\n'=> "\\n",
      _=> c.ToString(),
    };
  }
}
