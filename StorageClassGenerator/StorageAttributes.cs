/**************************************************************************************

StorageLib.StorageAttributes
============================

Classes defining attributes for for data model

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
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace StorageLib {


#pragma warning disable IDE0060 // Remove unused parameter
  /// <summary>
  /// Provides additional information about storing the class in a CSV file
  /// </summary>
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
  public class StorageClassAttribute: Attribute {
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="pluralName">used if class name has an irregular plural. Example: Activity => Activities</param>
    /// <param name="areInstancesUpdatable">Can the properties of the class change ?</param>
    /// <param name="areInstancesReleasable">Can stored class instance be removed from data context ?</param>
    /// <param name="isConstructorPrivate">Should constructor be private instead of public ?</param>
    /// <param name="isGenerateReaderWriter">Should code get generated to read instances of that class from
    /// CSV files without using a data context ? This is mostly used for data administration use cases.</param>
    public StorageClassAttribute(
      string? pluralName = null,
      bool areInstancesUpdatable = true,
      bool areInstancesReleasable = true,
      bool isConstructorPrivate = false,
      bool isGenerateReaderWriter = false) { }
  }


  /// <summary>
  /// Provides additional information about a property of a class which can be written to a CSV file.
  /// </summary>
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
  public class StoragePropertyAttribute: Attribute {
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="defaultValue">Provides a default value for this property in the class constructor.</param>
    /// <param name="isLookupOnly">Normally, a parent has a child collection for every child type referencing it. If the
    /// child just wants to link to the parent without the parent having a collection for that child, set isLookupOnly = true.</param>
    /// <param name="needsDictionary">A dictionary gets created in the data context for quick access to an instance using
    /// the value of this property.</param>
    /// <param name="toLower">Copies the content of a string property into a second property of the same class using 
    /// lower casing only. Can be helpful when creating case insensitive dictionaries in the data context (together with 
    /// needsDictionary argument)</param>
    /// <param name="childKeyPropertyName">Tells a dictionary or SortedList in a parent which property in the child should be used as key.</param>
    public StoragePropertyAttribute(
      string? defaultValue = null,
      bool isLookupOnly = false,
      bool isParentOneChild = false,
      bool needsDictionary = false,
      string? toLower = null,
      string? childKeyPropertyName = null) { }
  }
#pragma warning restore IDE0060 // Remove unused parameter
}
