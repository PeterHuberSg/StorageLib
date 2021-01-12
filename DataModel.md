# Introduction  
This document specifies the content of a *Data Model* .cs file. Code samples of the functionality 
can be found in the file *DataModelSamples.cs* in the *SampleDataModel* project.


# Table of Contents  
[**Data class**](#data-class)  
[**Data Model File Structure**](#data-model-file-structure)  
[**Supported Data Types in a Data Model**](#supported-data-types-in-a-data-model)  
[**-DateTime Replacements**](#datetime-and-timespan-replacements)  
[**-decimal Replacements**](#decimal-replacements)  
[**Create only, editable and deletable classes**](#create-only-editable-and-deletable-classes)  
[**Class with none standard plural name**](#class-with-none-standard-plural-name)  
[**Readonly property**](#readonly-property)  
[**Nullable (conditional) property**](#nullable-conditional-property)  
[**Further Documentation**](#further-documentation)  


# Data class
Within *StorageLib*, the expression *data class* means a class being defined in the *Data Model*. It has
some porperties defining the data hold by this class and any parent child relationship to other 
*data classes*.

*StorageClassGenerator* translates the *data classes* into new classes in the *Data Context*, adding the 
code needed for maintaining the parent child relationships and possibly storing the *data class* property 
values into one CSV file per *data class*.


# Data Model File Structure
The code for a simple *Data Model* could look like this (code is from *GetStartedDataModel.cs* in the
*GetStartedDataModel* project):
```csharp
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

```
It contains a `Parent` class and a `Child` class. The data of the `Parent` that might get stored in a CSV file 
is its name. The `Children` list indicates that the `Parent` is involved in a relationship with `Child` and
that it can have many children.

The `Child` class has also a `Name` property and a `Parent` property. Both get possibly stored in a CSV
file. When translating `Child` into a new class for the *Data Context*, *StorageClassGenerator* verifies 
that `Parent` has a property with the type `Child` or `List<Child>`. If the property has just a 
`Child` type, the `Parent` can have at most 1 child.

If the `Parent` property in the `Child` class is nullable, the `Child` might or might not have a 
`Parent`, indicating a conditional relationship. If the `Parent` property in the `Child` class is 
not nullable, the child must always have a parent.

*StorageLib* offers great flexibility in defining the *Data Model*, which is explained in the rest of this 
document.


# Supported Data Types in a Data Model
The data class properties can be simple type as described below, enumeration defined in the *Data Model* 
or other classes defined in the Data Model.

StorageLib knows how to store the following c# data types:
* `DateTime`
* `TimeSpan`
* `Decimal`
* `bool`
* `int`
* `long`
* `char`
* `string`

If needed also other simple types could be added. Double and float were not added so far, because Decimal is 
often better suited if correct decimal places are needed.

`char` and `string` are stored as UTF8 UNICODE characters.  

In order to reduce the storage space in the CSV file, *StorageLib* specifies some variants of the above listed 
types, which indicates the precision (i.e. how many digits after decimal point) should be stored. C# data types 
try to provide the biggest possible data range, which is often not needed. For example, DateTime is more precise 
than milliseconds, while often only a precision of days would be needed.

## DateTime and TimeSpan Replacements
* `Date` becomes `DateTime`, stored as dd.mm.yyyy
* `Time` becomes `TimeSpan`, stored as hh:mm:ss, max: 23.59:59
* `DateMinutes` becomes `DateTime`, stored as dd.mm.yyyy hh:mm
* `DateSeconds` becomes `DateTime`, stored as dd.mm.yyyy hh:mm:ss
* `DateTimeTicks` becomes `DateTime` stored with full `DateTime` precission as Ticks
* `TimeSpanTicks` becomes `TimeSpan` stored with full `TimeSpan` precission, stored as Ticks

Only significant parts get written to a CSV file. If `DateMinutes` contains only a date and hours, but no 
minutes, it gets written as 'dd.mm.yyyy hh', without minutes.

## decimal Replacements
* `decimal` stays `decimal`, stored with full `decimal` precission
* `Decimal2` becomes `decimal` stored with 2 digits after decimal point
* `Decimal4` becomes `decimal` stored with 4 digits after decimal point
* `Decimal5` becomes `decimal` stored with 5 digits after decimal point

Only significant digits get written to a CSV file. If `Decimal2` has a 0 as second digit after the decimal point, 
it gets written as '99999.9', without the trailing '0'.


# Create only, editable and deletable classes
Each *data class* is stored in its own `DataStore` which is similar to a `Dictionary`, where the `Key` 
property of the *data class* is the key into the `DataStore`. 

If it is only possible to create instances of a *data class*, but not to delete them, 
then `DataStore` can find any instance with the same speed like a simple arry access. When some instances 
get deleted, `DataStore` might need to make a binary search for the instance, which has a delay of
log(number of instances).

If the data of an instance get changed (=updated) or the instance deleted, the `DataStore` will not 
change the data in the middle of the CSV file, which would be very slow. Instaed, it just adds a 
new record at the end of the file, indicating what had happened. If a class is not editable nor 
deletable, these activities need not be performed.

Surprisingly, there are quite many kinds of data which only get created but not changed afterwards, like:
* financial data
* measurement results
* data log
* chat history

Sometimes, it is better to create a new instance instead updating an existing one, like the address 
of a customer. Maybe he had placed orders in the past and the order was linked to the old address.

Of course, from time to time, like once a year, also this kind of data might need to get deleted, but this can be done 
as part of maintenace of the CSV files. *StorageLib* provides also functionality to manage the CSV 
file contect without using a *Data Context*.

It is helpful for the *Data Context* to know, if a *data class*  will change its data. This 
can be indicated in the *Data Model* using the `StorageClassAttribute`:

```csharp
public class UpdateableReleasableClass {}

[StorageClass(areInstancesReleasable: false)]
public class UpdateableNoneReleasableClass {}

[StorageClass(areInstancesUpdatable: false)]
public class NoneUpdateableReleasableClass {}

[StorageClass(areInstancesUpdatable: false, areInstancesReleasable: false)]
public class NoneUpdateableNoneReleasableClass {}
```

If a *data class* is *releasable*, it gets a `Release()` method which will remove it from the 
*DataStore*. The instance still exists, could even get added to the *DataStore* again, using the method 
`Store()`. For this reason, the word *release* is used instead of *delete*. Released data is 
only truely gone once the application shuts down of has no longer a reference to it.

If a *data class* is *updateable*, it gets an `Update()` method added. Property values of a 
*data class* can only get changed through `Update()`. The *DataStore* would get too slow if each 
property could be changed on its own.


# Class with none standard plural name
The *Data Context* has a *Data Store* for each *Data Class*, which provides data access for the 
rest of the application. The name of that *Data Store* is the name of the *Data Class* with an 
appended 's'. It is possible to change that name with the `StorageClassAttribute`, `pluralName` 
attribute:
```csharp
[StorageClass(pluralName: "Children")]
public class Child {}
```


# Readonly property
Sometimes, the value of a property should not be changed. An example could be the creation date:
```csharp
public class ReadonlyPropertyClass {
  public readonly Date CreationDate;
  public string SomeOtherData;
}
```
In this example, the constructor of the  `ReadonlyPropertyClass` will have the parameters 
`CreationDate` and `SomeOtherData`, but `Update()` will have only the parameter `SomeOtherData`.


# Nullable (conditional) property
The .NET Core Framework introduced *nullable reference types*, which comes handy to mark a property
as required or optional:
```csharp
public class Name {
  public string FirstName;
  public string? MiddleName;
}
```
When creating or updating a `Name`, a `FirstName` must be provided, but there might be no 
`MiddleName`.

A nullable property is also used in *parent child* relationshipd to indicate that the child can have 
a parent, while a not nullable property requires that the must be a parent.



# Further Documentation
* [Readme.md](Readme.md) describes main features of *StorageLib* and gives a high level overview how *StorageLib* works.
* [Setup.md](Setup.md) describes how to install a local copy of *StorageLib* on your PC and how to setup VS for your own application using *StorageLib*.
* [Design.md](Design.md) gives a high level introduction into the data design principals of *StorageLib* .




