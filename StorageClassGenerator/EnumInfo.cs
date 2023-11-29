/**************************************************************************************

StorageLib.EnumInfo
===================

Some info about enums created by storage compiler

Written in 2020 by Jürgpeter Huber 
Contact: https://github.com/PeterHuberSg/StorageLib

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/
using System.Collections.Generic;


namespace StorageLib {


  /// <summary>
  /// Some info about enums created by storage compiler
  /// </summary>
  public class EnumInfo {
    public readonly string Name;
    public readonly string CodeLines;
    public readonly IReadOnlyList<string> EnumValues;


    public EnumInfo(string name, string codeLines, List<string> enumValues) {
      Name = name;
      CodeLines = codeLines;
      EnumValues = enumValues;
    }
  }
}
