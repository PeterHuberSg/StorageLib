/**************************************************************************************

StorageLib.GeneratorException
=============================

Exception thrown by storage compiler

Written in 2020 by Jürgpeter Huber 
Contact: https://github.com/PeterHuberSg/StorageLib

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/
using System;


namespace StorageLib {


  /// <summary>
  /// Exception thrown by storage compiler
  /// </summary>
  public class GeneratorException: Exception {

    /// <summary>
    /// Constructor for Generator specific exception taking one string as argument
    /// </summary>
    public GeneratorException(string? message): base(message) {}


    /// <summary>
    /// Constructor for Generator specific exception taking className, memberText and exceptionMessage as argument
    /// </summary>
    public GeneratorException(string className, string memberText, string exceptionMessage) :
    base($"Class: {className}" + Environment.NewLine +
       $"Property: {memberText}" + Environment.NewLine + 
      exceptionMessage) { }


    /// <summary>
    /// Constructor for Generator specific exception taking ClassInfo, MemberInfo and exceptionMessage as argument
    /// </summary>
    public GeneratorException(ClassInfo ci, MemberInfo mi, string exceptionMessage) : 
    base($"Class: {ci.ClassName}" + Environment.NewLine +
       $"Property: {mi.MemberText}" + Environment.NewLine + exceptionMessage) { }
  }
}
