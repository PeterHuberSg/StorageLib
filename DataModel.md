# Introduction  
This document specifies the content of a *Data Model* .cs file. The developer defines in the *Data Model* 
the classes needed for his project, their properties and how they are related to the other classes.

Code samples for the functionality described here can be found in the file *DataModelSamples.cs* in the *SampleDataModel* project.


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
[**Property with default value**](#property-with-default-value)  
[**Automatic lower case copy of string property**](#automatic-lower-case-copy-of-string-property)  
[**Marking a property to find an instance quickly (indexing)**](#Marking-a-property-to-find-an-instance-quickly-indexing)  
[**Enumeration Properties**](#enumeration-properties)  
[**Parent without maintained relationship to child, 1:0 or c:0**](#parent-without-maintained-relationship-to-child-10-or-c0)  
[**Parent with single child, 1:c or c:c**](#parent-with-single-child-1c-or-cc)  
[**One to one relationships cannot be implemented, 1:1**](#One-to-one-relationships-cannot-be-implemented-11)  
[**Parent with multiple children, 1:mc or c:mc**](#parent-with-multiple-children-1mc-or-cmc)  
[**-Parent with multiple children using List<>**](#parent-with-multiple-children-using-list)  
[**-Parent with multiple children using Dictionary<> or SortedList<>**](#parent-with-multiple-children-using-dictionary-or-sortedList)  
[**-Parent with multiple children using SortedBucketCollection**](#parent-with-multiple-children-using-sortedbucketcollection)
[**Generated class with private constructor**](#Generated-class-with-private-constructor)  
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
It contains a `Parent` class and a `Child` class. The data of the `Parent` might get stored in a CSV file 
called 'Parent.csv'. `List<Child> Children` indicates that the `Parent` is involved in a relationship with `Child` and
that it can have many children.

The `Child` class has also a `Name` property to hold its own data and a `Parent` property. The content 
of both properties gets possibly stored in a CSV file. When translating `Child` into a new class for the *Data Context*, 
*StorageClassGenerator* verifies that `Parent` has a property with the type `Child` or `List<Child>`. 
If the property is just a `Child` type, the `Parent` can have at most 1 child.

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

If requested by future projects, also other simple types could be added. Double and float were not 
added so far, because Decimal is often better suited if correct decimal places are needed.

`char` and `string` are stored as UTF8 UNICODE characters.  

In order to reduce the storage space in the CSV file, *StorageLib* specifies some variants of the above listed 
types, which indicates the precision (i.e. how many digits after decimal point) should be stored. C# data types 
try to provide the biggest possible data range, which is often not needed. For example, DateTime is more precise 
than milliseconds, while often only a precision of days would be needed.

Special data types can be used in the *Data Model*, which will be translated into ordinary c# data 
types by the *StorageClassGenerator*.

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
change the data in the middle of the CSV file, which would be very slow. Instead, it just adds a 
new record at the end of the file, indicating what had happened. If a class is not editable nor 
deletable, these activities need not be performed.

Surprisingly, there are quite many kinds of data which only get created but not changed afterwards, like:
* financial data
* measurement results
* data log
* chat history

Sometimes, it is better to create a new instance instead updating an existing one, like the address 
of a customer. Maybe he had placed orders in the past and an existing order was linked to the old 
address.

Of course, from time to time, like once a year, also this kind of data might need to get deleted, but this can be done 
as part of maintenance  of the CSV files. *StorageLib* provides also functionality to manage the CSV 
file content without using a *Data Context*.

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

**Note:** Normally, the 4 basic CRUD operations are called *Create*, *Read*, *Update* and *Delete*. 
In the *StorageLib*, there is no specific read operation, the access to the properties is normal C# 
code. *Update* is the only way to change the value of a property, it is not possible to assign a new 
value to a property directly. There is an additional method called *Store*. A *data class* can be 
created just in RAM and *Store* writes it into a CSV file. There is an operation similar to *Delete*, 
but it is called *Release*, because it is the opposite of *Store*, i.e. it just removes the object 
from the CSV file, but it is still available in RAM. C# does not allow to delete a class instance. 
The only thing a developer can do to remove all references to the instance and hope the garbage 
collector will one day actually delete that instance.

If a *data class* is *releasable*, it gets a `Release()` method which will remove it from the 
*DataStore*. The instance still exists in RAM, could even get added to the *DataStore* again, 
using the method `Store()`. Released data is only truly gone once the application has no longer a 
reference to it or shuts down.

If a *data class* is *updateable*, it gets an `Update()` method added. Property values of a 
*data class* can only get changed through `Update()`. The *DataStore* would get too slow if each 
property could be changed on its own and each change would be written to a file.


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

A nullable property is also used in *parent child* relationships to indicate that the child can have 
a parent, while a not nullable property requires that the must be a parent.


# Property with default value
In c#, it is possible to give a method argument a default value. When assigning a default value to
a data class property, the constructor of that class will use that default value.
```csharp
public class DefaultValuePropertyClass {
  public string Name;
  [StorageProperty(defaultValue: "NoName")]
  public string DefaultValueProperty;
}
```
The constructor will look like this:
```csharp
public DefaultValuePropertyClass(string name, string defaultValueProperty = "NoName", bool isStoring = true) {
```
Note:
The properties arguments in the constructor are in the same sequence like the properties in the data class. Once
an argument has a default value, all following arguments must have a default value too. The same applies for
the property definitions in the data class: once one property has a default value, all following properties must
have a default value too.


# Automatic lower case copy of string property
Sometimes it is nice to have a properly cased version of a string and a lower cased version. The first 
is used for displaying it to the user, the second for searching the *data class* instance containing it 
or as key into a dictionary of the *data class*. `StorageProperty(toLower: "Xyz")` provides a lower case 
version of Xyz, which gets automatically updated each time Xyz gets updated.
```csharp
public class ToLowerCasePropertyClass {
  public string Name;
  [StorageProperty(toLower: "Name")]
  public string NameLower; //will always contain the lower case version of Name
}
```
The code produced for `ToLowerCasePropertyClass` constructor:
```csharp
public ToLowerCasePropertyClass(string name, bool isStoring = true) {
  ...
  Name = name;
  NameLower = Name.ToLowerInvariant();
  ...
}
```


# Marking a property to find an instance quickly (indexing)
One can always use LINQ to search a particular item in a collection. Usually, this is fast, 
since *StorageLib* holds all data in RAM. But if there are thousands of items and this 
search need to be performed often, it might be helpful to use a Dictionary to find the item.

```csharp
public class User {
  public string Name;
  [StorageProperty(needsDictionary: true)]
  public string LoginName;

  public string Email;
  [StorageProperty(toLower: "Email", needsDictionary: true)]
  public string EmailLower;
}
```

Each user must have a login name and the same login name cannot be used for 2 different users. 
If these 2 conditions are met by a property, *StorageLib* can maintain a Dictionary which uses 
the *LoginName* as key and returns the user with that key, similarly like an index in a 
database. When the user's *LoginName* changes, the old name gets automatically removed from the 
directory and the new name gets added. If the user gets released, it's entry in the dictionary 
gets removed too.

**Note:** It is not possible to create a Dictionary for a nullable property nor when 2 items 
can have the same value, like 2 people using the same phone number.

*Email* shows an example where the Dictionary uses actually the lower case spelling of the 
property. This can be helpful when searching for an email address, but the upper and lower 
casing doesn't matter.

*StorageClassGenerator* will add the following lines to the *Data Context*:

```csharp
    public IReadonlyDataStore<User> Users;
    public IReadOnlyDictionary<string, User> UsersByLoginName;
    public IReadOnlyDictionary<string, User> UsersByEmailLower;
```
For each *data class* defined in the *Data Model*, a *DataStore* gets created, which works like 
a Dictionary, using the automatically created *Key* property as key. A *DataStore* is faster 
than a Dictionary, because it assigns the value to the *Key* property, an continously 
incremented integer. If no items get released, the access time is extremely fast and independent 
of how many items are stored. If some items got released, the retrieval speed is similar but 
still faster than a Dictionary.

*UsersByLoginName* is the Dictionary automatically maintained by *StorageLib* and allows the 
retrieval of a user by *LoginName*.


# Enumeration Properties
Sometimes it is better to hard code configuration data than creating *data classes* for it. 
Enumerations are perfect for this purpose:
```csharp
public enum Weekdays { Mo, Tu, We, Th, Fr}

public class ClassWithEnumProperty {
  public Weekdays Weekday;
  public Weekdays? ConditionalWeekDay;
  readonly public Weekdays ReadonlyWeekday;
}
```
 *StorageClassGenerator* will create a file *Enums.base.cs*, which will contain all enumerations 
defined in the *Data Model*.


# Parent without maintained relationship to child, 1:0 or c:0
```
1:0 = child must have one parent, parent does not store information about children
c:0 = child might have parent, parent does not store information about children
```
Sometimes, one class (parent) is used by one or many children classes. The children have a link to
the parent, but the parent doesn't maintain any relationship to its children.

**Example:**  
Parent: ExchangeRates, has for each day an exchange rate  
Children: Prices, Orders, Invoices, Payments, etc.

It might not make sense for one exchange rate to know which Price, Order, Invoice, Payment, etc,
links to it. However, if an ExchangeRate later gets deleted, it cannot check easily, if any child
still links to it. Therefore, ExchangeRate should be undeletable (`areInstancesReleasable: false`). 

**Example:**  
Item: From a catalog of items to sell  
OrderItem: A line item of a customer order.

```csharp
[StorageClass(areInstancesReleasable: false)]
public class Item {
  public string Name;
}

public class OrderItem {
  public Order Order;
  public int Quantity;
  [StorageProperty(isLookupOnly: true)]
  public Item Item;
}
```
Note: Order is a *data class* defined somewhere else in the *Data Model*


# Parent with single child, 1:c or c:c
```
1:c = child must have one parent, parent might have one child
c:c = child might have one parent, parent might have one child
```
Sometimes, a parent can have only 1 single child as opposed to many children. An example could be 
department which can have only 1 manager and every manager must be assigned to a department:
```csharp
  public class Department {
    public string Name;
    [StorageProperty(isParentOneChild: true)]
    public Manager? Manager;
  }
  public class Manager {
    public string Name;
    public Department Department;
  }
```
Note that the `Manager` property in the `Department` must be *nullable*.


# One to one relationships cannot be implemented, 1:1

Example:  
A country has exactly one capital, a capital belongs exactly to one country. This would actually be 
a 1:1 relationship. A 1:1 relationship cannot be created, because one item cannot
exist without the other item. However, *StorageLib* can read and create only one instance at a time.

Usually, a 1:1 relationship can be designed as one single object. If 2 different classes are 
required, the 1:1 relationship gets replaced by 1:c.
```csharp
  public class Country {
    public string Name;
    public Capital? City;
  }
  public class City {
    public string Name;
    [StorageProperty(isParentOneChild: true)]
    public Country? CountryCapital;
  }
```
Note: It is difficult to find a good 1:1 relationship example. In actual fact, each City would 
belong to one country and each country would have one capital. However, *StorageLib* does not 
support 2 classes being a child of each other. Instead, defining a `Capital` class would be a 
better idea. 

To keep things simple, in this example a city doesn't know to which country it belongs, except
when it is its capital. It might surprise that the `Country` becomes a *child* of the `City`. 
In *storageLib*, the child defines membership in a relationship and it makes sense 
that the information which `City` is the capital is defined in the `Country` class.

Question: If some properties would be needed for the capital, where would they go ? Of 
course to the `Country` class.


# Parent with multiple children, 1:mc or c:mc
```
1:mc = child must have one parent, parent might have zero, one or many children
c:mc = child might have one parent, parent might have zero, one or many children
```

*c:mc* is probably the most common relationship. One might think that *1:mc* is more common, i.e. there
cannot be a child without parent. But there is very often a good reason why the design should allow a
child to exist with no parent, for example when the proper parent is not available yet. 

In *StorageLib*, the parent cannot remove a child form its children collection. The child only 
gets removed when the child updates its `Parent` property. If `Child.Parent` is not conditional (not 
`nullable`), `Parent` cannot be set to null and therefore the child not be 
removed from its parent.

If `Child.Parent`  is updatable (not `readonly`), the `Parent` value can be changed from 
`Parent0` to `Parent1`, which will remove the child from `Parent0.Children` and add it to 
`Parent1.Children`.

Note: As there are no *1:1* relationships, there are also no *1:m* nor *c:m* relationships. The reason
is the same. It must be possible to read the parent form the permanent storage (CSV file) before 
the child. Each parent must be able to exist without children.

The `Children` are stored in RAM in a collection. When the parent gets read during startup, that
collection is empty. *StorageLib* supports 3 type of collections:

## Parent with multiple children using List<>

```charp
public class Parent {
  public string Text;
  public List<Child> Children;
}

public class Child {
  public string Text;
  public Parent Parent;
}
```

After reading the Parent class declaration, *StorageClassGenerator* searches for a *data class* 
`Child` in the DataModel and within `Child` for a property with type `Parent`. Once *StorageClassGenerator*
can match the 2 participating properties of a relationship, it creates code which will keep the
data on both ends synchronised. The idea is that if the child links to a parent, it must be 
in the children collection of the parent. The programmer does not need to do anything to maintain 
this relationship.

```csharp
var parent0 = new Parent("Parent0"); 
var child0 = new Child("Child0", parent0);
```

These 2 lines create and store `parent0` and `child0`. `child0.Parent` links to `parent0` and 
`parent0.Children` contains `child0`;

```csharp
parent1 = new Parent("Parent1");
child0.Update(child0.Name, parent1);
```

Here a new parent `parent1` gets created and `child0.Parent` links to `parent1`. 
`parent0.Children` is now empty again.

When a child is not releasable, the *StorageClassGenerator* uses a normal `List<Child>` for the 
parent's children collection, but when a child is releasable, it uses a `StorageList<Child>`. 
`StorageList<>` behaves like a `List<>`. When enumerating its items, **all** children are 
shown, stored and not stored ones. `StorageList.GetStoredItems()` enumerates over all stored 
children, but skips over not stored children. `StorageList.CountStoredItems` returns only 
the number of stored children.

**Note:** One can find out if an instance is stored by checking if it's *Key* is greater equal 0.

A challenge with released children is that they still link to their parents, especially when the 
`Parent` property in the child is not nullable or `readonly`. One approach would be to remove 
the children from its parent when it gets released (deleted), but that might have all kinds of 
strange consequences, like the readonly parent property of a released child still referencing its 
parent, but the child is not longer in the parent's children collection. *StorageLib* goes through 
great efforts to guarantee that if a child links to a parent, that child is also in the parent's 
children collection.

**Note:** *StorageLib* guarantees that a not stored parent will never have any stored children. 
For this reason all children must be released before the parent can be released.

## Parent with multiple children using Dictionary<> or SortedList<>

Sometimes it is necessary to find a child in the parent's children collection based on a child's 
property value (=dictionary key). Example: Exchange rates (=children) stored in a currency (=parent) should 
be accessible by the date this exchange rate is valid for:

```csharp
public class Currency {
  public string Name;
  public SortedList<Date, ExchangeRate> Rates;
}

public class ExchangeRate {
  public Currency Currency;
  public Date Date;
  public Decimal4 Rate;
}
```

After reading the Currency class declaration and the property `Rates`, *StorageClassGenerator* 
searches for a *data class* `ExchangeRate` in the *DataModel* and within `ExchangeRate` for a 
property with type `Currency`, which defines the relationship, and a property with type 
`Date` which will be used as dictionary key into `Rates`.

If the child has several properties with the dictionary key's type, the attribute 
`[StorageProperty(childKeyPropertyName: "Xxx")]` is used to define the property in the
child to be used as dictionary key:

```csharp
public class Task {
  public string Name;
  [StorageProperty(childKeyPropertyName: "Priority")]
  public Dictionary<int, Activity> ActivitiesByPriority;
}

[StorageClass(pluralName: "Activities")]
public class Activity {
  public Task Task;
  public string Name;
  public int Prioritiy;
  public int Mandays;
}
```

Note that in the example above, the use of a Dictionary guarantees that each activity has a 
different priority.

`SortedList` is functionally equivalent to `Dictionary`, the main difference is how they store 
their items. `SortedList` is like a `List`. If a new item needs to be inserted into a `SortedList`, 
all items after the insertion point need to be moved to one slot higher, meaning insertion and 
deletion can be rather expensive. On the other hand, appending is cheap, it doesn't involve moving 
any other items around. Like with exchange rates, there are many cases where new data gets always 
appended at the end. For these cases, `SortedList` is more efficient than 'Dictionary'. 

The functionality described in `List` applies also for `SortedList` and `Dictionary`. Maintaining 
the relationship is a bit more challenging in the second case, because if the  dictionary key property in the 
child (in the example: `Date`) changes, the child needs to get removed from the parent's children 
and added again so that the child will be found with the new dictionary key value and not the old one.

## Parent with multiple children using SortedBucketCollection

Sometimes it is convenient when a parent can store its children in a collection using 2 keys, 
as opposed to a Dictionary, which uses only 1 key. The first key groups children together, 
like all items which were created on the same day. In a Dictionary, no 2 items can have the
same key value. In SortedBucketCollection, only the combination of first and second key must
be unique.

Example: A simplicistic financial application might have accounts, like Cash or Expenses and 
AccountsItem, stating on which date for which account a certain amount needs to be stored.

```csharp
public class Account {
  public string Name;
  public SortedBucketCollection<Date, int, AccountItem> Items;
}

public class AccountItem {
  public Date Date;
  public string Text;
  public Decimal2 Amount;
  public Account Account;
}
```

Note: If StorageClassGenerator cannot find any int property in the child class of the data model, it will 
take the autogenerated Key property. In this example, the AccountItems get first sorted
by Date. If there are two AccountItems with the same Date value, Key is a usefull property to uniquly identify them. 

SortedBucketCollection allows to query a date range like this:

```
var expensesAccount = DC.Data.Accounts.Where(a => a.Name=="Expenses").First();
foreach (var accountItem in expensesAccount.Items[new DateTime(2020, 1, 1), new DateTime(2020, 1, 31)]) {
  Console.WriteLine(accountItem);
}
```

The foreach loop will efficiently enumerate each Expenses AccountItem of month January 
2020. This comes handy in finance, where some numbers have to be reported daily, 
weekly, monthly or yearly.

SortedBucketCollection was written specifically for StorageLib to replace Dictionary when 
it is possible that 2 items share the same key.






# Generated class with private constructor

Based on the *data class* defined in the *data model*, *StorageClassGenerator* generates a 
Xxx.base.cs file. The values of these properties in this files get stored in a CVS file. The values 
of some properties can be calculated based on other properties and these values don't need to 
be stored. The developer can add them and methods to the  Xxx.cs file. Both files contain a 
`partial class Xxx` declaration. 

For example, an `OrderDetail' has a *number of items* property and a *unit price* property and 
both get stored in CSV file. A total *amount property* does not need to be stored, since it can be 
calculated based on the first 2 properties. Therefore, it should be defined in the Xxx.cs file, 
not Xxx.base.cs. The developer can even add a public constructor to that file, so that he can 
write code to ensure that all properties have a proper value. In this case, he might want to 
make to constructor in the autogenerated Xxx.base.cd private. This can be done in the 
*Storage Model* like this:

```csharp
[StorageClass(isConstructorPrivate: true)]
public class Xxx {
  public string Text;
}
```


# Further Documentation
* [Readme.md](Readme.md) describes main features of *StorageLib* and gives a high level overview how *StorageLib* works.
* [Setup.md](Setup.md) describes how to install a local copy of *StorageLib* on your PC and how to setup VS for your own application using *StorageLib*.
* [Design.md](Design.md) gives a high level introduction into the data design principals of *StorageLib* .




