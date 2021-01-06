/**************************************************************************************

StorageLib.EnumInfo
===================

Some infos about enums created by storage compiler

Written in 2020 by Jürgpeter Huber 
Contact: PeterCode at Peterbox dot com

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/


namespace StorageLib {


  /// <summary>
  /// Some infos about enums created by storage compiler
  /// </summary>
  public class EnumInfo {
    public readonly string Name;
    public readonly string CodeLines;


    public EnumInfo(string name, string codeLines) {
      Name = name;
      CodeLines = codeLines;
    }
  }
}
