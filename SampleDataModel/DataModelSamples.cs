/**************************************************************************************

DataModelSamples
================

Shows examples of how the content of a Data Model can look like.

Written in 2021 by Jürgpeter Huber 
Contact: PeterCode at Peterbox dot com

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/

using System;
using System.Collections.Generic;
using StorageLib;

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
//The name of this namespace will be used as namespace of the generated classes.
namespace DataModelSamples {

  #region Classes which are updatable and releasable or not
  //      -------------------------------------------------

  /*
  If data gets only created and stored, but never deleted, data retrieval becomes extremly faster. Any instance
  can be found by its key immediately, while when deletion is allowed and some keys are missing, a search
  needs to be performed which has a log(number of keys) delay. It is quite often, that data does not
  need to get deleted, or even updated, like with measurement results, accounting, logs, etc.

  StorageClassAttribut supports for this purpose 2 arguments:
  areInstancesUpdatable (default true): the class has some properties which can be changed calling Update().
  areInstancesReleasable (default true): stored instances of that class can get removed from Data Contect (= deletion).

  If a child class is updatable and changes its parent, StorageLib automatically removes the child from its
  old parent and adds it to its new parent. If data gets stored in a CSV file, an update recorde gets added and
  when the application shuts down, the file gets rewritten with updated data only.

  If Release() is called on an instance (object) of a releasable class, that object gets removed from the Data 
  Context and it's key set to smaller 0. If data gets stored in a CSV file, a delete record gets added and
  when the application shuts down, the file gets rewritten with undeleted data only.
  */

  [StorageClass()]
  public class UpdateableReleasableClass {
    public string Name;
  }

  [StorageClass(areInstancesReleasable: false)]
  public class UpdateableNoneReleasableClass {
    public string Name;
  }

  [StorageClass(areInstancesUpdatable: false)]
  public class NoneUpdateableReleasableClass {
    public string Name;
  }

  [StorageClass(areInstancesUpdatable: false, areInstancesReleasable: false)]
  public class NoneUpdateableNoneReleasableClass {
    public string Name;
  }
  #endregion

  #region Class with none standard plural name
  //      ------------------------------------

  //If a data class gets stored in a collection, StorageLib gives that collection the plural name of
  //the data class. The plural building standard is just to add an 's' to the class name. This can
  //be overwritten using the 

  [StorageClass(pluralName: "PluralNameNoneStandardClasses")]
  public class PluralNameNoneStandardClass {
    public string Name;
  }
  #endregion

  #region Readonly property
  //      -----------------

  //If a property is marked as readonly, it will not be included in Update().
  //If all properties are readonly, do not mark each property, but use 
  //[StorageClass(areInstancesUpdatable: false)] instead.

  public class ReadonlyPropertyClass {
    public readonly string Name; //cannot be changed with Update()
    public string Address; //can be changed with Update()
  }
  #endregion

  #region Nullable (conditional) property
  //      -------------------------------

  //If a property is marked as nullable, it might have a value or not.
  //If a property used in a relationship is conditional, it is marked as nullable. 
  //If a property used in a relationship is one (1), it is marked as not nullable. 

  public class ConditionalUnconditionalPropertyClass {
    public string? ConditionalString; //could be used for conditional relationship c:
    public string UnconditionalString; //could be used for not conditional relationship 1:
  }
  #endregion

  #region Property with default value
  //      ---------------------------

  //In c#, it is possible to give a method argument a default value. When assigning a default value to
  //a data class property, the constructor of that class will use that default value.

  public class DefaultValuePropertyClass {
    public string Name;
    [StorageProperty(defaultValue: "NoName")]
    public string DefaultValueProperty;
  }

  /*
  produced code:
  public DefaultValuePropertyClass(string name, string defaultValueProperty = "NoName", bool isStoring = true) {
  */

  //Note:
  //The properties arguments in the constructor are in the same sequence like the properties in the data class. Once
  //an argument has a default value, all following arguments must have a default value too. The same applies for
  //the property definitions in the data class: once one property has a default value, all following properties must
  //have a default value too.
  #endregion

  #region Automatic lower case copy of string property
  //      --------------------------------------------

  //For searching, it is often useful when a string is in lower case, but when displayed to the user, it must
  //retain its casing. StorageProperty(toLower: "Xyz") provides a lower case version of Xyz.

  public class ToLowerCasePropertyClass {
    public string Name;
    [StorageProperty(toLower: "Name")]
    public string NameLower; //will always contain the lower case version of Name
  }
  #endregion

  #region Data Model data types
  //      ---------------------

  public class DataModelDataTypes {
    public Date Date; // becomes DateTime, stored as dd.mm.yyyy
    public Time Time; // becomes TimeSpan, stored as hh:mm:ss, max: 23.59:59
    public DateMinutes DateMinutes; // becomes DateTime, stored as dd.mm.yyyy hh:mm
    public DateSeconds DateSeconds; // becomes DateTime, stored as dd.mm.yyyy hh:mm:ss
    public DateTimeTicks DateTimeTicks; // becomes DateTime stored with full DateTime precission as Ticks
    public TimeSpanTicks TimeSpanTicks; // becomes TimeSpan stored with full TimeSpan precission, stored as Ticks
    public decimal Decimal_; // stays decimal, stored with full decimal precission
    public Decimal2 Decimal2; // becomes decimal stored with 2 digits after decimal point
    public Decimal4 Decimal4; // becomes decimal stored with 4 digits after decimal point
    public Decimal5 Decimal5; // becomes decimal stored with 5 digits after decimal point
    public bool Bool_; // stored as 
    public int Int_;
    public long Long_;
    public char Char_;
    public string String_;
  }
  #endregion

  #region Enumeration Properties
  //      ----------------------

  public enum Weekdays { Mo, Tu, We, Th, Fr}

  public class ClassWithEnumProperty {
    public Weekdays Weekday;
    public Weekdays? ConditionalWeekDay;
    readonly public Weekdays ReadonlyWeekdays;
  }
  #endregion

  #region Parent without maintained relationsip to child, 1:0 or c:0
  //      ----------------------------------------------------------

  /*
  Sometimes, one class (parent) is used by one or many children classes. The children have a link to
  the parent, but the parent doesn't maintain any relationship to its children. Example:
  Parent: ExchangeRates, has for each day an exchange rate
  Children: Prices, Orders, Invoices, Payments, etc.

  It might not make sense for one exchange rate to know which Price, Order, Invoice, Payment, etc,
  links to it. However, if an ExchangeRate later gets deleted, it cannot check easily, if any child
  still links to it. Therefore, ExchangeRate should be undeletable. 
   */
  //1:0 child requires parent, but parent does not link back to child 
  [StorageClass(areInstancesReleasable: false)]
  public class Lookup_1_0_Parent {
    public string Name;
  }
  public class Lookup_1_0_Child {
    public string Name;
    [StorageProperty(isLookupOnly: true)]
    public Lookup_1_0_Parent Parent;
  }

  //c:0 child might have a parent, but parent does not link back to child
  [StorageClass(areInstancesReleasable: false)]
  public class Lookup_C_0_Parent {
    public string Name;
  }
  public class Lookup_C_0_Child {
    public string Name;
    [StorageProperty(isLookupOnly: true)]
    public Lookup_1_0_Parent Parent;
  }
  #endregion

  #region Parent with single child, 1:c or c:c
  //      ------------------------------------

  //1:c Parent with at most 1 child, child requires Parent 
  public class SingleChild_1_C_Parent {
    public string Name;
    [StorageProperty(isParentOneChild: true)]
    public SingleChild_1_C_Child? Child;
  }
  public class SingleChild_1_C_Child {
    public string Name;
    public SingleChild_1_C_Parent Parent;
  }

  //c:c Parent with at most 1 child, child with conditional Parent
  public class SingleChild_C_C_Parent {
    public string Name;
    [StorageProperty(isParentOneChild: true)]
    public SingleChild_C_C_Child? Child;
  }
  public class SingleChild_C_C_Child {
    public string Name;
    public SingleChild_C_C_Parent? Parent; //nullable parent indicates c:mc relation
  }
  #endregion

  #region Parent with Children List, 1:mc or c:mc
  //      ---------------------------------------

  //child 1 : mc parent relationship using List
  public class List_1_MC_Parent {
    public string Name;
    public List<List_1_MC_Child> Children;
  }
  public class List_1_MC_Child {
    public Date Date;
    public List_1_MC_Parent Parent;
  }

  //child c : mc parent relationship using List
  public class List_C_MC_Parent {
    public string Name;
    public List<List_C_MC_Child> Children;
  }
  public class List_C_MC_Child {
    public Date Date;
    public List_C_MC_Parent? Parent; //nullable parent indicates c:mc relation
  }

  public class ListWithPropertyNameParent {
    public string Name;
    public List<ListWithPropertyNameChild> Children;
  }
  public class ListWithPropertyNameChild {
    public Date Date1;
    public Date Date2;
    public ListWithPropertyNameParent Parent;
  }
  #endregion

  #region Parent with Children Dictionary, 1:mc or c:mc
  //      ---------------------------------------------

  //child 1 : mc parent relationship using Dictionary
  public class Dictionary_1_MC_Parent {
    public string Name;
    //DictionaryChild has only one property of type Date. No need for [StorageProperty(childKeyPropertyName: "Date")]
    public Dictionary<Date, Dictionary_1_MC_Child> Children; 
  }
  public class Dictionary_1_MC_Child {
    public Date Date;
    public Dictionary_1_MC_Parent Parent;
  }

  //child c : mc parent relationship using Dictionary
  public class Dictionary_C_MC_Parent {
    public string Name;
    public Dictionary<Date, Dictionary_C_MC_Child> Children;
  }
  public class Dictionary_C_MC_Child {
    public Date Date;
    public Dictionary_C_MC_Parent? Parent; //nullable parent indicates c:mc relation
  }

  public class DictionaryWithPropertyNameParent {
    public string Name;
    [StorageProperty(childKeyPropertyName: "Date1")] //Dictionary needs to know which Date property
    public Dictionary<Date, DictionaryWithPropertyNameChild> Children;
  }
  public class DictionaryWithPropertyNameChild {
    public Date Date1;
    public Date Date2;
    public DictionaryWithPropertyNameParent Parent;
  }
  #endregion

  #region Parent with Children SortedList, 1:mc or c:mc
  //      ---------------------------------------------

  //child 1 : mc parent relationship using SortedList
  public class SortedList_1_MC_Parent {
    public string Name;
    //SortedListChild has only one property of type strig. No need for [StorageProperty(childKeyPropertyName: "Name")]
    public SortedList<string, SortedList_1_MC_Child> Children;
  }
  public class SortedList_1_MC_Child {
    public string Name;
    public SortedList_1_MC_Parent Parent;
  }

  //child c : mc parent relationship using SortedList
  public class SortedList_C_MC_Parent {
    public string Name;
    public SortedList<string, SortedList_C_MC_Child> Children;
  }
  public class SortedList_C_MC_Child {
    public string Name;
    public SortedList_C_MC_Parent? Parent; //nullable parent indicates c:mc relation
  }

  public class SortedListWithPropertyNameParent {
    public string Name;
    [StorageProperty(childKeyPropertyName: "Name")] //SortedList needs to know which string property
    public SortedList<string, SortedListWithPropertyNameChild> Children;
  }
  public class SortedListWithPropertyNameChild {
    public string Name;
    public string Address;
    public SortedListWithPropertyNameParent Parent;
  }
  #endregion

  #region Class retrievable by one of its properties (like db Index)
  //      ----------------------------------------------------------

  //StorageLib creates for each data class a DataStore, which is like a Dictionary, using the data class'
  //Key property as key into the DataStore. If there are many instances of a data class, it might become
  //to time consuming to serach through all of them to find one with a particular value. For this case,
  //StorageLib can create an additional Dictionary in the Data Context, using the vaue of on property as
  //key.

  public class NeedsDictionaryClass {
    [StorageProperty(needsDictionary: true)]
    public string Name;//this property will be used as key into the dictionary. It cannot be nullable.
    public string Address;
  }

  /*
  The automatically generated Dictionary will look like this in the Data Context:
  /// <summary>
  /// Directory of all NeedsDictionaryClasss by Name
  /// </summary>
  public IReadOnlyDictionary<string, NeedsDictionaryClass> NeedsDictionaryClasssByName => _NeedsDictionaryClasssByName;
  internal Dictionary<string, NeedsDictionaryClass> _NeedsDictionaryClasssByName { get; private set; }
  
       [StorageProperty(toLower: "Name")]
    public string NameLower; //will always contain the lower case version of Name

   */

  //sometimes, it is convenient if the Directoy key is the lower case version of the actual string. This can be achieved
  //like this:

  public class NeedsDictionaryLowerCaseClass {
    public string Name;
    [StorageProperty(toLower: "Name", needsDictionary: true)]
    public string NameLower;//the lower case version of Name will be used as key into the dictionary.
    public string Address;
  }
  #endregion
}
