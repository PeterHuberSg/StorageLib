/**************************************************************************************

GetStarted DataModel
====================

Shows how Data Model classes can be defined using parent and child as example

Written in 2021 by Jürgpeter Huber 
Contact: https://github.com/PeterHuberSg/StorageLib

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/

// Structure of this file
// ----------------------
//
// The code in this file is used to auto generated classes, properties and their parent child relationships.
//
// Simple comments '//' will not be included in the generated files. They can be used to comment the model only.
// XML comments '///' will be included in the generated files as comments for the classes or properties they comment.
// #region will not be included in the generated files. They are used to navigation among the different data classes.
//
// using and namespace will be included in the generated files
// #pragma are used only to prevent compiler warnings in this file
// [StorageClass] and [StorageProperty] attributes are used for generating code

using StorageLib;
using System.Collections.Generic;

#pragma warning disable CS8618 // Non-nullable field is uninitialized.
namespace YourNamespace {

  /// <summary>
  /// Some comment for Parent.
  /// </summary>
  public class Parent {

    /// <summary>
    /// Some Name comment
    /// </summary>
    public string Name;

    /// <summary>
    /// Any child created will automatically get added here.
    /// </summary>
    public List<Child> Children;
  }

  [StorageClass(pluralName: "Children")]
  public class Child {
    public string Name;

    /// <summary>
    /// The child will get added to its parent Children collection.
    /// </summary>
    public Parent Parent;
  }
}

// Generated Code
// --------------
//
// 2 files will be created for every class Xxx: Xxx.base.cs containing the generated code as partial class and Xxx.cs
// where you can add your own functionality. The code generator only makes a Xxx.cs when none exist. Xxx.base.cs gets
// overwritten each time the generator runs.

// One additional file gets generated as data context. It's name is defined in new StorageClassGenerator() call of the
// Console program. Create in your application a new data context, which gives access to all classes in the model and
// reads the persisted data from the .CSV files. Dispose it before closing your application, which ensures that all
// changes are written to the files.
