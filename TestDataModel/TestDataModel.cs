/**************************************************************************************

TestDataModel
=============

Shows how Data Model classes can be defined for storage compiler

Written in 2020 by Jürgpeter Huber 
Contact: PeterCode at Peterbox dot com

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/


#region Documentation
//      -------------

// Structure of this file
// ----------------------
//
// The code in this file is used to auto generated classes, properties and their parent child relationships.
//
// Simple comments '//' will not be included in the generated files. They can be used to comment the model only.
// XML comments '///' will be included in the generated files as comments for the classes or properties they comment.
// #region will not be included in the generated files. They are used to navigation among the different Storage models
//
// using and namespace will be included in the generated files
// #pragma are used only to prevent compiler warnings in this file
// [StorageClass] and [StorageProperty] attributes are used for generating code

// How to setup your own project
// -----------------------------
//
// Create a .NET Core Console application project for your model and a .dll project for your generated code.
// Change Program.cs to something like this:
/*
using System;
using System.IO;
namespace YourNameSpace {

  class Program {
    public static void Main(string[] _) {
      var solutionDirectory = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent.Parent;
      var sourceDirectoryPath = solutionDirectory.FullName + @"\Model";
      var targetDirectoryPath = solutionDirectory.FullName + @"\DataContext";
      new StorageClassGenerator(
        sourceDirectoryString: sourceDirectoryPath, //directory from where the .cs files get read.
        targetDirectoryString: targetDirectoryPath, //directory where the new .cs files get written.
        context: "DC"); //>Name of Data Context class, which gives static access to all data stored.
    }
  }
}
*/
// It defines the model project and the generated code projects and then calls the StorageClassGenerator.
// Run the console application each time you have made a change to the model

// Add a .CS file following the structure of the file you read presently. A simple model could look like this:
/*
using System;
using System.Collections.Generic;
using Storage;

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
namespace YourNameSpace {

  /// <summary>
  /// Some comment for Parent.
  /// </summary>
  [StorageClass(areInstancesUpdatable:false, areInstancesReleasable: false)]
  public class Parent {

    /// <summary>
    /// Some Text comment
    /// </summary>
    public string Text;

    /// <summary>
    /// List representing parent child relationship
    /// </summary>
    public List<Child> Children;
  }

  /// <summary>
  /// Some comment for Child
  /// </summary>
  [StorageClass(pluralName: "Children")]
  public class Child {
    /// <summary>
    /// Some Text comment
    /// </summary>
    public string Text;

    /// <summary>
    /// Some Parent comment
    /// </summary>
    public Parent Parent;
  }
}*/

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

#endregion


#pragma warning disable CS8618 // Non-nullable field is uninitialized. 
using System;
using System.Collections.Generic;
using StorageLib;


//The name of this namespace will be used as namespace of the generated classes.
namespace TestContext {


  #region LookupParent: LookupChild, parent has no maintained relationship to child
  //      -------------------------------------------------------------------------

  //An example for lookup, only the child linking to parent but the parent having no link to the child.
  //This can be useful for example if parent holds exchange rates for every day. The child links to
  //one particular exchange rate, but the exchange rate does not know which child links to it. If a parent
  //is used as a lookup, that class is not allowed to support deletion. Because when the parent should get
  //deleted, it does not know if children still point to it.

  [StorageClass(areInstancesUpdatable: true, areInstancesReleasable: false)]
  public class LookupParent {
    public string Text;
  }


  [StorageClass(areInstancesUpdatable: true, areInstancesReleasable: false)]
  public class LookupParentN {
    public string Text;
  }


  [StorageClass(areInstancesUpdatable: true, areInstancesReleasable: false)]
  public class LookupParentR {
    public string Text;
  }


  [StorageClass(areInstancesUpdatable: true, areInstancesReleasable: false)]
  public class LookupParentNR {
    public string Text;
  }


  /// <summary>
  /// Example of a child with a none nullable and a nullable lookup parent. The child maintains links
  /// to its parents, but the parents don't have children collections.
  /// </summary>
  [StorageClass(areInstancesUpdatable: true, areInstancesReleasable: true, pluralName: "LookupChildren")]
  public class LookupChild {
    /// <summary>
    /// Some info
    /// </summary>
    public string Text;

    [StorageProperty(isLookupOnly: true)]
    public LookupParent Parent;

    [StorageProperty(isLookupOnly: true)]
    public LookupParentN? ParentN;

    [StorageProperty(isLookupOnly: true)]
    public readonly LookupParentR ParentR;

    [StorageProperty(isLookupOnly: true)]
    public readonly LookupParentNR? ParentNR;
  }
  #endregion



  #region Parent with at most one child
  //      -----------------------------

  // The parent might or might not have a child.
  //
  // The parent class has a nullable property with the child class type, while the child class has a property
  // with the parent class type.
  //
  //Todo: Should only enforce during runtime that a parent cannot get released while a child is not released yet
  // Since the parent class is deletable, it cannot have a child which is not deletable, because the deletion of
  // the parent forces also the deletion of the child.

  public class SingleChildParent {
    public string Text;
    [StorageProperty(isParentOneChild: true)]
    public SingleChildChild? Child;
  }

  public class SingleChildParentN {
    public string Text;
    [StorageProperty(isParentOneChild: true)]
    public SingleChildChild? Child;
  }

  public class SingleChildParentR {
    public string Text;
    [StorageProperty(isParentOneChild: true)]
    public SingleChildChild? Child;
  }

  public class SingleChildParentNR {
    public string Text;
    [StorageProperty(isParentOneChild: true)]
    public SingleChildChild? Child;
  }

  [StorageClass(pluralName: "SingleChildChildren")]
  public class SingleChildChild {
    public string Text;
    public SingleChildParent Parent;
    public SingleChildParentN? ParentN;
    public readonly SingleChildParentR ParentR;
    public readonly SingleChildParentNR? ParentNR;
  }
  #endregion


  #region Parent with children list
  //      -------------------------

  //Example where the parent uses a List for its children. 
  //If the child is not deletable, the parent must not be not deletable too. It's not possible to delete a parent and
  //leave the child with a link to that deleted parent.
  //The child.Parent property can be nullable (conditional parent) or not nullable (parent required)
  //The child.Parent property can be readonly (parent child relationship cannot be changed after child is created)
  //[StorageClass(isGenerateReaderWriter: true)] creates ClassXyzReader and ClassXyzWriter, which allow to read and write 
  //the CSV file without using a data context nor DataStore. This is useful for administrative tasks, like deleting
  //of data which is not deletable within the data context.

  public class ListParent {
    public string Text;
    public List<ListChild> Children;
  }

  public class ListParentN {
    public string Text;
    public List<ListChild> Children;
  }

  public class ListParentR {
    public string Text;
    public List<ListChild> Children;
  }

  public class ListParentNR {
    public string Text;
    public List<ListChild> Children;
  }

  [StorageClass(pluralName: "ListChidren")]
  public class ListChild {
    public string Text;
    public ListParent Parent;
    public ListParentN? ParentN;
    public readonly ListParentR ParentR;
    public readonly ListParentNR? ParentNR;
  }
  #endregion


  #region Parent with children Dictionary
  //      -------------------------------

  //Example where parent has a Dictionary property instead a List for its children. The child needs an additional field which
  //can be used as Key for the Dictionary. 

  public class DictionaryParent {
    public string Text;
    public Dictionary<string, DictionaryChild> DictionaryChildren;
  }

  public class DictionaryParentN {
    public string Text;
    public Dictionary<string, DictionaryChild> DictionaryChildren;
  }

  public class DictionaryParentR {
    public string Text;
    public Dictionary<string, DictionaryChild> DictionaryChildren;
  }

  public class DictionaryParentNR {
    public string Text;
    public Dictionary<string, DictionaryChild> DictionaryChildren;
  }

  [StorageClass(pluralName: "DictionaryChildren")]
  public class DictionaryChild {
    public string Text;
    public DictionaryParent Parent;
    public DictionaryParentN? ParentN;
    public readonly DictionaryParentR ParentR;
    public readonly DictionaryParentNR? ParentNR;
  }
  #endregion


  #region Parent with children SortedList
  //      -------------------------------

  //Example where parent has a SortedList instead a List for its children. The child needs an additional field which
  //can be used as Key for the SortedList.

  //It might be more convenient to use a SortedList than a SortedDictionary, because in a SortedList, an item can be accessed
  //by its place in the SortedList like the last item:
  //key = sortedList.Keys[sortedList.Lenght];
  //item = sortedList[key];

  public class SortedListParent {
    public string Text;
    public SortedList<string, SortedListChild> SortedListChidren;
  }

  public class SortedListParentN {
    public string Text;
    public SortedList<string, SortedListChild> SortedListChidren;
  }

  public class SortedListParentR {
    public string Text;
    public SortedList<string, SortedListChild> SortedListChidren;
  }

  public class SortedListParentNR {
    public string Text;
    public SortedList<string, SortedListChild> SortedListChidren;
  }

  [StorageClass(pluralName: "SortedListChidren")]
  public class SortedListChild {
    public string Text;
    public SortedListParent Parent;
    public SortedListParentN? ParentN;
    public readonly SortedListParentR ParentR;
    public readonly SortedListParentNR? ParentNR;
  }
  #endregion


  #region Class using all supported data types 
  //      ------------------------------------

  // check this class to find all available data types.

  // The goal is to use as little storage space in the CSV file as possible. It is better to use the data type Date
  // then DateTime, if the time portion is not use. It is better to use Decimal2, which stores maximally 2 digits
  // after the decimal point than decimal, which gets stored with full precision.

  // In general, it is better to use none nullable value types, they give the garbage collector less work to do.


  /// <summary>
  /// Class having every possible data type used for a property
  /// </summary>
  public class DataTypeSample {
    //A DateTime with only Date, but no Time
    public Date ADate;
    public Date? ANullableDate;

    //A TimeSpan covering only positive 23 hours, 59 minutes and 59 seconds 
    public Time ATime;
    public Time? ANullableTime;

    //A DateTime with a precision of minutes
    public DateMinutes ADateMinutes;
    public DateMinutes? ANullableDateMinutes;

    //A DateTime with a precision of seconds
    public DateSeconds ADateSeconds;
    public DateSeconds? ANullableDateSeconds;

    //A DateTime with a precision of ticks
    public DateTimeTicks ADateTime;
    public DateTimeTicks? ANullableDateTime;

    //A TimeSpan with a precision of ticks
    public TimeSpanTicks ATimeSpan;
    public TimeSpanTicks? ANullableTimeSpan;

    //A decimal with full precision. If possible, use DecimalX, which uses less CSV file storage space
    public decimal ADecimal;
    public decimal? ANullableDecimal;

    //A decimal with up to 2 digits after decimal point
    public Decimal2 ADecimal2;
    public Decimal2? ANullableDecimal2;

    //A decimal with up to 4 digits after decimal point
    public Decimal4 ADecimal4;
    public Decimal4? ANullableDecimal4;

    //A decimal with up to 5 digits after decimal point
    public Decimal5 ADecimal5;
    public Decimal5? ANullableDecimal5;

    //A boolean
    public bool ABool;
    public bool? ANullableBool;

    //An integer
    public int AInt;
    public int? ANullableInt;

    //A long
    public long ALong;
    public long? ANullableLong;

    //A character
    public char AChar;
    public char? ANullableChar;

    //A string, full Unicode supported, but ASCII only strings get faster processed
    public string AString;
    public string? ANullableString;

    //any enum defined in this file
    public SampleStateEnum AEnum;
    public SampleStateEnum? ANullableEnum;
  }
  #endregion


  #region Private Constructor, public constructor will be in the other partial file
  //      -------------------------------------------------------------------------

  // Example where constructor is private instead of public. This is convenient when another public constructor
  // is defined in Xxx.cs, additional to the private constructor in Xxx.base.cs, which is now hidden.

  /// <summary>
  /// Example with private constructor.
  /// </summary>
  [StorageClass(isConstructorPrivate: true)]
  public class PrivateConstructor {

    /// <summary>
    /// Some Text
    /// </summary>
    public string Text;
  }
  #endregion


  #region Class where the value of one property is used to build a dictionary for that class
  //      ----------------------------------------------------------------------------------

  // Often, a class has one property which can be used to identify one particular instance. 
  // [StorageProperty(needsDictionary: true)] adds a Dictionary to the data context, which gets updated whenever
  // an instance get added, that property updated or the instance deleted.

  /// <summary>
  /// Some comment for PropertyNeedsDictionaryClass
  /// </summary>
  [StorageClass(pluralName: "PropertyNeedsDictionaryClasses")]
  public class PropertyNeedsDictionaryClass {

    /// <summary>
    /// Used as key into dictionary PropertyNeedsDictionaryClassesByIdInt
    /// </summary>
    [StorageProperty(needsDictionary: true)]
    public int IdInt;

    /// <summary>
    /// Used as key into dictionary PropertyNeedsDictionaryClassesByIdString
    /// </summary>
    [StorageProperty(needsDictionary: true)]
    public string? IdString;

    /// <summary>
    /// Some Text comment
    /// </summary>
    public string Text;

    /// <summary>
    /// Lower case version of Text
    /// </summary>
    [StorageProperty(toLower: "Text", needsDictionary: true)]
    public string TextLower;

    /// <summary>
    /// Some Text comment which can be null
    /// </summary>
    public string? TextNullable;

    /// <summary>
    /// Lower case version of TextNullable
    /// </summary>
    [StorageProperty(toLower: "TextNullable", needsDictionary: true)]
    public string? TextNullableLower;

    /// <summary>
    /// Some Text comment
    /// </summary>
    public readonly string TextReadonly;

    /// <summary>
    /// Lower case version of TextReadonly
    /// </summary>
    [StorageProperty(toLower: "TextReadonly", needsDictionary: true)]
    public string TextReadonlyLower;
  }
  #endregion


  #region SampleMaster -> Sample -> SampleDetail, grand parent, parent, child, using List for children
  //      ---------------------------------------------------------------

  //Sample.SampleMaster is nullable
  //SampleDetail.Sample is NOT nullable, it is not possible to store a SampleDetail without a parent Sample
  //shows in Sample also most data types supported

  /// <summary>
  /// Some SampleStateEnum comment
  /// </summary>
  public enum SampleStateEnum {
    /// <summary>
    /// Recommendation while creating your own enums: use value 0 as undefined
    /// </summary>
    None,
    Some
  }


  /// <summary>
  /// Some comment for SampleMaster.
  /// With an additional line.
  /// </summary>
  [StorageClass(areInstancesReleasable: false)]
  public class SampleMaster {

    /// <summary>
    /// Some Text comment
    /// </summary>
    public string Text;

    /// <summary>
    /// List representing parent child relationship
    /// </summary>
    public List<Sample> SampleX;


    /// <summary>
    /// Integer property with int.MinValue as default
    /// </summary>
    [StorageProperty(defaultValue: "int.MinValue")]
    public int NumberWithDefault;
  }


  /// <summary>
  /// Some comment for Sample
  /// </summary>
  [StorageClass(pluralName: "SampleX")]
  public class Sample {
    /// <summary>
    /// Some Text comment
    /// </summary>
    public string Text;

    /// <summary>
    /// Some Flag comment
    /// </summary>
    public bool Flag;

    /// <summary>
    /// Some Amount comment
    /// </summary>
    public int Number;

    /// <summary>
    /// Amount with 2 digits after comma comment
    /// </summary>
    public Decimal2 Amount;

    /// <summary>
    /// Amount with 4 digits after comma comment
    /// </summary>
    public Decimal4 Amount4;

    /// <summary>
    /// Nullable amount with 5 digits after comma comment
    /// </summary>
    public Decimal5? Amount5;

    /// <summary>
    /// PreciseDecimal with about 20 digits precision, takes more storage space then the other Decimalx types.
    /// </summary>
    public decimal PreciseDecimal;

    /// <summary>
    /// Some SampleState comment
    /// </summary>
    public SampleStateEnum SampleState;

    /// <summary>
    /// Stores dates but not times
    /// </summary>
    public Date DateOnly;

    /// <summary>
    /// Stores times (24 hour timespan) but not date.
    /// </summary>
    public Time TimeOnly;

    /// <summary>
    /// Stores date and time precisely to a tick.
    /// </summary>
    public DateTimeTicks DateTimeTicks;

    /// <summary>
    /// Stores date and time precisely to a minute.
    /// </summary>
    public DateMinutes DateTimeMinute;

    /// <summary>
    /// Stores date and time precisely to a second.
    /// </summary>
    public DateSeconds DateTimeSecond;

    /// <summary>
    /// Some OneMaster comment
    /// </summary>
    public SampleMaster? OneMaster;

    /// <summary>
    /// Some OtherMaster comment
    /// </summary>
    public SampleMaster? OtherMaster;

    /// <summary>
    /// Some Optional comment
    /// </summary>
    public string? Optional;

    /// <summary>
    /// Some SampleDetails comment
    /// </summary>
    public List<SampleDetail> SampleDetails;
  }


  /// <summary>
  /// Some comment for SampleDetail
  /// </summary>
  public class SampleDetail {
    /// <summary>
    /// Some Text comment
    /// </summary>
    public string Text;

    /// <summary>
    /// Link to parent Sample
    /// </summary>
    public Sample Sample;
  }
  #endregion


  #region Parent with none standard name for children list
  //      ------------------------------------------------

  // Example where the parent's List for it's children is not the plural of the child type type. 

  /// <summary>
  /// Example where the parent's List for it's children is not the plural of the child type type. 
  /// </summary>
  public class NotMatchingChildrenListName_Parent {

    /// <summary>
    /// Some Text
    /// </summary>
    public string Text;

    /// <summary>
    /// Deletable children which must have a parent
    /// </summary>
    public List<NotMatchingChildrenListName_Child> Children;
  }


  /// <summary>
  /// Child for NotMatchingChildrenListName_Parent
  /// </summary>
  public class NotMatchingChildrenListName_Child {

    /// <summary>
    /// Some Text
    /// </summary>
    public string Text;

    /// <summary>
    /// Deletable children which must have a parent
    /// </summary>
    public NotMatchingChildrenListName_Parent Parent;
  }
  #endregion

  #region Simple test child and parent
  //      ----------------------------

  public class TestParent {
    public string Text;
    public List<TestChild> Children;
  }

  [StorageClass(pluralName: "TestChildren")]
  public class TestChild {
    public string Text;
    public TestParent Parent;
  }
  #endregion


















  //Todo: TwoListsParent needs StorageProperty to link child property to parent list
  //#region Parent with 2 lists for same child type
  ////      ---------------------------------------

  //// Example where the parent has 2 Lists for one child type. 

  ///// <summary>
  /////  Example where the parent has 2 Lists for one child type.
  ///// </summary>
  //public class TwoListsParent_Parent {

  //  /// <summary>
  //  /// Some Text
  //  /// </summary>
  //  public string Text;

  //  /// <summary>
  //  /// Deletable children which must have a parent
  //  /// </summary>
  //  public List<TwoListsParent_Child> ChildrenA;

  //  /// <summary>
  //  /// Deletable children which must have a parent
  //  /// </summary>
  //  public List<TwoListsParent_Child> ChildrenB;
  //}


  ///// <summary>
  ///// Example of deletable parent using a List for its children. It can have only deletable children. The child must have a 
  ///// parent (the child.Parent property is not nullable). The relationship cannot be updated, since child is readonly.
  ///// </summary>
  //public class TwoListsParent_Child {

  //  /// <summary>
  //  /// Some Text
  //  /// </summary>
  //  public string Text;

  //  /// <summary>
  //  /// Deletable children which must have a parent
  //  /// </summary>
  //  public TwoListsParent_Parent ParentA;

  //  /// <summary>
  //  /// Deletable children which must have a parent
  //  /// </summary>
  //  public TwoListsParent_Parent? ParentB;
  //}
  //#endregion
}
