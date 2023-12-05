# Introduction  
This document describes the design of the *StorageLib* software and how to use it.

*StorageLib* can be used as a replacement of a database for a **single user application**. All
data is stored in RAM and any data change copied to CSV (comma separated value) files. The programmer
only needs to define the data classes and their properties, *StorageLib* creates the code needed 
for creation (`new()`), update and delete (`Release()`). *StorageLib* maintains parent child 
relationships, i.e. when `child.Parent` gets updated from `Parent1` to `Parent2`, *StorageLib* 
removes `child` from `Parent1.Children` and adds `child` to `Parent2.Children`. *StorageLib* supports 
transactions and data backup.

# Table of Contents  
[**Introduction StorageLib usage**](#introduction=storageLib-usage)  
[**- Data Model**](#data-model)  
[**- Generated Code**](#generated-code)  
[**-- Data Class**](#data-class)  
[**-- Data Context**](#data-context)  
[**- Application Code**](#Application-code)  

[**Design principals**](#design-principals)  
[**- Supported relationships**](#supported-relationships)  
[**- Relationship notation**](#relationship-notation)  
[**- Nullability indicates conditionality**](#nullability-indicates-conditionality)  
[**- Relationship examples**](#relationship-examples)  
[**- Children define its end of a relationship by having a property with the parent's type**](#Children-define-its-end-of-a-relationship-by-having-a-property-with-the-parents-type)  
[**- Parents define the details of a relationship**](#Parents-define-the-details-of-a-relationship)  
[**- Parents first**](#parents-first)  
[**- Children control the relationship**](#children-control-the-relationship)  
[**- Keys**](#Keys)  
[**- Data Model**](#Data-Model)
[**- Generated Data Model Classes**](#generated-data-model-classes)  
[**- DC Data Context**](#DC-Data-Context)  
[**- Transactions**](#transactions)  
[**- Automatic data files compaction**](#automatic-data-files-compaction)

[**Challenges**](#challenges)  
[**- What should happen when a child gets "deleted"**](#What-should-happen-when-a-child-gets-deleted)  
 
[**Further Documentation**](#further-documentation)  


# Introduction StorageLib usage

## Data Model

In a first step, the developer makes a *Data Model*. Here is the `GetStartedDataModel`:
```csharp
namespace YourNamespace {
  public class Parent {
    public string Name;
    public List<Child> Children;
  }
  [StorageClass(pluralName: "Children")]
  public class Child {
    public string Name;
    public Parent Parent;
  }
}
```

It shows a `Parent` class which can have many children, and a `Child` class which must have 
exactly one parent. Of course, it would be possible to make the `Parent` conditional (nullable) 
to cater for orphans, but the goal here is to use a very simple *Data Model*.


## Generated Code
Based on the *Data Model*, the *StorageClassGenerator* creates some code like this (extremely 
simplified):

### Data Class

```csharp
namespace YourNamespace {
  public partial class Parent: IStorageItem<Parent> {
    public int Key { get; private set; }
    public string Name { get; private set; }
    public IReadOnlyList<Child> Children => children;
    readonly List<Child> children;
  
    public Parent(string name, bool isStoring = true) {
      Key = StorageExtensions.NoKey;
      Name = name;
      children = new List<Child>();
      if (isStoring) {
        Store();
      }
    }

    public void Store() {DC.Data._Parents.Add(this);}

    public void Update(string name) {
      var isChangeDetected = false;
      if (Name!=name) {
        Name = name;
        isChangeDetected = true;
      }
      if (isChangeDetected) {
        if (Key>=0) {
          DC.Data._Parents.ItemHasChanged(clone, this);
        }
      }
    }

    internal void AddToChildren(Child child) {children.Add(child);}

    internal void RemoveFromChildren(Child child) {children.Remove(child);}

    public void Release() {DC.Data._Parents.Remove(Key);}

    public override string ToString() {
      var returnString =
        $"Key: {Key.ToKeyString()}," +
        $" Name: {Name}," +
        $" Children: {Children.Count};";
      return returnString;
    }
  }
}
```

In reality, the `Parent.base.cs` class has over 300 lines of code to support reading the
property values from and writing to a CSV file, transactions and more. 

`StorageClassGenerator.cs` creates for each *data class* (= a  class defined in a *Data 
Model*) a `Key` property (gets later updated by the *Data Store*), a constructor and the following methods:
* `Store()`: Adds the instance to its *Data Store* in the *Data Context*
* `Update()`: Allows to change the property values of an instance
* `Release()`: Removes the instance from its *Data Store* in the *Data Context*
* `ToString()`: Shows the property values of the instance

If the class is a parent of another class, the following gets added:
* a `Child` collection (`List`, `Dictionary` or `SortedList`) with the name `Children` (can have any name)
* `AddToChildren()`: Adds a `Child` to `Children`
* `RemoveFromToChildren()`: Removes a `Child` from `Children`

**Note:** `AddToChildren()` and `RemoveFromToChildren()` are `internal`. They get only called 
from the `Child` class.

**Note:** `Parent.base.cs` is a `partial class`. You can add your own code to `Parent.cs`, which
will not be overwritten by `StorageClassGenerator`. `Parent.base.cs` calls many `partial` methods 
so that you can add additional functionality. They are not shown in the code samples here.

`Child.base.cs` code:

```csharp
namespace YourNamespace  {
  public partial class Child: IStorageItem<Child> {
    public int Key { get; private set; }
    public string Name { get; private set; }
    public Parent Parent { get; private set; }

    public Child(string name, Parent parent, bool isStoring = true) {
      Key = StorageExtensions.NoKey;
      Name = name;
      Parent = parent;
      Parent.AddToChildren(this);
      if (isStoring) {
        Store();
      }
    }

    public void Store() {DC.Data._Children.Add(this)}

    public void Update(string name, Parent parent) {
      var hasParentChanged = Parent!=parent;
      if (hasParentChanged) {
        Parent.RemoveFromChildren(this);
      }
      var isChangeDetected = false;
      if (Name!=name) {
        Name = name;
        isChangeDetected = true;
      }
      if (Parent!=parent) {
        Parent = parent;
        isChangeDetected = true;
      }
      if (hasParentChanged) {
        Parent.AddToChildren(this);
      }
      if (isChangeDetected) {
        if (Key>=0) {
          DC.Data._Children.ItemHasChanged(clone, this);
        }
      }
    }

    public void Release() {DC.Data._Children.Remove(Key);}


    public override string ToString() {
      var returnString =
        $"Key: {Key.ToKeyString()}," +
        $" Name: {Name}," +
        $" Parent: {Parent.ToShortString()};";
      onToString(ref returnString);
      return returnString;
    }
    partial void onToString(ref string returnString);
    #endregion
  }
}
```

The `Child` code looks similar to the `Parent` code, although `Update()` is more complicated. The
additional lines keep the *parent child relationship* updated.


### Data Context
Finally, the `StorageClassGenerator.cs` creates a *Data Context*, a file called `DC.base.cs`. It 
gives `static` access to all stored *data classes* instances:

```csharp
namespace YourNamespace  {
  public partial class DC: DataContextBase {

    public static DC? Data {get;}

    public static void DisposeData() {dataLocal?.Dispose();}

    public CsvConfig? CsvConfig { get; }
    public bool IsInitialised { get; private set; }
    public IReadonlyDataStore<Child> Children => _Children;
    internal DataStore<Child> _Children { get; private set; }
    public IReadonlyDataStore<Parent> Parents => _Parents;
    internal DataStore<Parent> _Parents { get; private set; }

    public DC(CsvConfig? csvConfig): base(DataStoresCount: 2) {
      Data = this;
      CsvConfig = csvConfig;

      if (csvConfig==null) {
        _Parents = new DataStore<Parent>();
        DataStores[0] = _Parents;
        _Children = new DataStore<Child>();
        DataStores[1] = _Children;
      } else {
        _Parents = new DataStoreCSV<Parent>();
        DataStores[0] = _Parents;
        _Children = new DataStoreCSV<Child>();
        DataStores[1] = _Children;
      }
      IsInitialised = true;
    }

    protected override void Dispose(bool disposing) {
      if (disposing) {
        _Children?.Dispose();
        _Children = null!;
        _Parents?.Dispose();
        _Parents = null!;
        data = null;
      }
      base.Dispose(disposing);
    }
  }
}
```

The *Data Context* has for every *Data Class* a `DataStore` (data only in RAM) or `DataStoreCSV` (data stored in a CSV 
file), depending on the `CsvConfig` settings given in the constructor of `DC`. A `DataStore` is 
like a dictionary, it gives access to a stored instance through the instance's `Key` property.

**Note:** In C#, a child uses a reference to point to its parent. When stored in a CSV file, the 
child uses the parent's `Key` to link to it. During application startup, `DC` reads all CSV files. 
When it finds a `Child` with a key value in the CSV file for a parent, it searches `DC.Parents` 
for the parent with that key and sets `Child.Parent` to that `Parent`. `DC` then adds the `Child` 
to `Parent.Children`. The application code usually doesn't need to read `Key`, except to determine 
if the instance is stored (`Key>=0`) or not (`Key<0`). 

## Application Code
After the `StorageClassGenerator.cs` has written all this code, the developer writes now his 
application code:
```csharp
using StorageLib;
using System;
using System.IO;
using YourNamespace; 
namespace GetStartedConsole {
  class Program {
    static void Main(string[] args) {
      try {
        _ = new DC(new CsvConfig(...)); // 1)
        var parent0 = new Parent("Parent0"); // 2)
        var child0 = new Child("Child0", parent0); // 3)
        var parent1 = new Parent("Parent1");
        DC.Data.StartTransaction(); // 4)
        try {
          child0.Update(child0.Name + " updated", parent1); // 5)
          parent0.Release(); // 6)
          DC.Data.CommitTransaction(); // 7)
        } catch (Exception exception) {
          DC.Data.RollbackTransaction(); // 8)
          reportException(exception);
        }
      } catch (Exception exception) {
          reportException(exception);
      } finally {
        DC.DisposeData(); // 9)
      }
    }
  }
}
```

Some steps in this very simple example:

1) creates a new *Data Context*. `CsvConfig` provides configuration information like if CSV files should be used and where they are stored.
2) A parent gets created and stored in the *Data Context*, i.e. in `DC.Data.Parents` and in the CSV file.
3) A child gets created, stored and added to its parent's `Children` collection.
4) It is not mandatory to use transactions, but often helpful.
5) The child's `Name` gets changed and `parent1` becomes the new parent. This automatically removes `child0` from `parent0.Children` and adds it to `parent1.Children`. In the child's CSV file, a new line gets added with that update information.
6) `parent0` gets removed from the *Data Context* (=deleted). In the parent's CSV file, a new line gets added indicating that `parent0` is removed.
7) During `CommitTransaction()` not much needs to be done, since the data is already written to the CSV files.
8) Upon a `RollbackTransaction()`, all lines written to the CSV files since `StartTransaction()` get deleted and the instances in the RAM set back to the values they had before `StartTransaction()`.
9) For performance reason, CSV file changes get written first into a RAM buffer and only written to the CSV file once the buffer is full or a short time has passed. `DC.DisposeData()` guarantees that all buffers are written to the CSV files when the application shuts down.  

When the application starts again and the transaction was successful, the *Data Context* just 
contains `child0` linked to `parent1`.

# Design principals

## Supported relationships

*StorageLib* supports the following relationships between 2 objects:  
**"one to conditional"** (1:c)  
**"conditional to conditional"** (c:c)  
**"one to many conditional"** (1:mc)  
**"conditional to many conditional"** (c:mc)

**"one to one"** (1:1) should not be needed, because in praxis this means they form together only
1 object.

**"one to many"** (1:m) is not possible, because in a 1:m relationship, both objects must be 
created precisely at the same time, one cannot exist without the other. But *StorageLib* allows only 
the creation (more precisely: storage, retrieval) of one object at a time. When an instance gets 
stored, all its properties must have legal values, i.e. cannot be null. Nullable indicates a 'c' 
relationship. Similarly, when at startup the CSV files are written back into RAM, only one file 
can be read at a time and only 1 object of that relationship created. A 1:m does not allow one 
object to exist without the other object of the relationship.

**"many x to many x"** (m:m), (m:mc), (mc:mc): Like with relational data tables, 
many to many is not supported. Instead a third child class must be designed 
additionally to the two parents, which need the mc:mc relationship:

```
Parent1 <── mc : 1 ──┐
   mc                │
   ↕                 ╞══> Child 
   mc                │
Parent2 <── mc : 1 ──┘

``` 

## Relationship notation
*Child:Parent*  
1:c  
c:c  
1:mc  
c:mc

Child can only be 1 or c (in this document always first)  
Parent can only be c or mc (in this document always second)

1: single  
c: conditional  
m: multiple

## Nullability indicates conditionality
*StorageLib* supports nullable reference types for children. A child property for a conditional 
child parent relationship (c:c or c:mc) is marked as nullable in the child. 

If a parent can have only one child (1:c or c:c), it has
a child property with the child's type. This property must be nullable, because at 
application start the parent is read before the child and for a short period that property 
is always null. 

If the parent can have many children (1:mc or c:mc), it has a children collection. The 
collection itself cannot be nullable, but empty (= no child).

## Relationship examples

Note that the property names can be anything allowed by C#, but In these examples they are just 
Parent, Child and Children to highlight their role.

1:c
```c#
public class Parent {
  public Child? Child;
}

public class Child {
  public Parent Parent;
}
```
c:c
```c#
public class Parent {
  public Child? Child;
}

public class Child {
  public Parent? Parent;
}
```
1:mc
```c#
public class Parent {
  public List<Child> Children;
}

public class Child {
  public Parent Parent;
}
```
c:mc
```c#
public class Parent {
  public List<Child> Children;
}

public class Child {
  public Parent? Parent;
}
```
A child can have an unlimited number of child parent relationships
```c#
public class Parent1 {
  public Child Child;
}

public class Parent2 {
  public List<Child> Children;
}

public class Child {
  public Parent1 Parent1;
  public Parent2? Parent2;
}
```

## Children define its end of a relationship by having a property with the parent's type
Setting up a child parent relationship from a child is easy. Just add a property to the child
class which has the parents property. In rare cases, the child needs to define some relationship
details:

```csharp
[StorageClass(areInstancesReleasable: false)]
public class Item {
  public string Name;
}

public class OrderDetail {
  public Order Order;
  public int Quantity;
  [StorageProperty(isLookupOnly: true)]
  public Item Item;
}
```

In this example there is an *Item* class describing items that can be ordered and a class 
*OrderDetail* which tracks the item belonging to a particular order. In this case, there 
might be no need for the parent class *Item* to maitain a list with all orders which have
ordered this item. When a child property has the type of a parent, *StorageClassGeneretor* 
will show an error if there is not corresponding property in the parent class, unless 
`[StorageProperty(isLookupOnly: true)]` specifies that the parent will not link back 
to the child.

However, this is a rare example when further definition is needed with the child property.

## Parents define the details of a relationship
Setting up a child parent relationship from the parent's end needs more information, for 
example if the children collection should be:
- List<Child>
- HashSet<Child>
- Dictionary<Key, Child>
- SortedList<Key, Child>
- SortedBucketCollection<Key1, Key2, Child>

Most often just a *List* is used. A *Dictionary* or *SortedList* allows to find a child quickly 
based on it's key. Example of storing company shares and their prices:

```csharp
public class Share {
  public string CompanyName;
  public SortedList<Date, Activity> Quotes;
}

public class Quote {
  public Share Share;
  public Date Date;
  public Decimal4 Amount;
}
```

It is the parent property which defines that for each date there can be only one quote. It usese a 
*SortedList* and not a *Dictionary* to define that. *SortedList* is much faster than *Dictionary* 
when new data gets always written at the end.

However, addional information might be needed, for example when the *Child* class has 2 properties 
with the type `Date`. 

```csharp
public class Share {
  public string CompanyName;
  [StorageProperty(childKeyPropertyName: "Date")]
  public SortedList<Date, Activity> Quotes;
}

public class Quote {
  public Share Share;
  public Date Date;
  public Date ReportedDate;
  public Decimal2 Amount;
}
```

The additional information `[StorageProperty(childKeyPropertyName: "Date")]` tells 
*StorageClassGeneretor* which Date in the child should be used for the *Quote* *Share* relationship.

Sometimes it is helpful to access children by `Date`, but several children might have the same
`Date`. Example of storing of share sales:

```csharp
public class Share {
  public string CompanyName;
  public SortedBucketCollection<Date, int, Sale> Sales;
}

public class Sale {
  public Date Date;
  public Decimal4 Amount;
}
```

`SortedBucketCollection<Key1, Key2, Child>` sorts all children first by *Key1*. If several children 
have the same value for *Key1*, they get sorted by *Key2*. This collection was specifically written for
*StorageLib*.

When 2 properties in a child link to a parent, the parent's children collection 
should contain the child only once, regardless if one or more than one child 
property link to that parent. For this reason, *StorageClassGenerator* replaces 
List<Child> with a HashSet<Child> when 2 or more child properties have
the same parent class type.

## Parents first
The parent objects must be stored before the child or the children linking to it. *StorageLib*
stores all instances (objects) of one class in one file. On application startup, the file gets read 
completely and all its objects created immediately, before the next file with the next 
class gets read. It must be possible to create any parent without any children, meaning
a `Child` property in a parent is always nullable (=conditional) and `Children` is always 
a collection, which can be empty (=mc).

## Children control the relationship
The child controls to which parent(s) it belongs. For each relationship to a parent it has one 
property. When a child property linking to a parent gets stored, the child gets added to the 
`Child` or `Children` property of its parent. When a child changes (`Update()`) the value of 
its `Parent` property from `Parent1` to `Parent2`, *StorageLib* 
removes child from `Parent1.Children` and adds child to `Parent2.Children`. 

The parents get written to a file without any children information. When the parent gets 
read first during startup, the parents have no children. The children get added to the parent 
once the children files gets read.

*StorageLib* ensures that when a child has a link to a parent, that child is also added to the 
children collection of the parent. *"Children control the child parent relationship"* is only 
relevant in the sense that adding or removing a child from its
parent can only be achieved by changing the value of the child property linking to the 
parent. There is no method supporting removing the child directly form the children 
collection in the parent.

## Keys
Each item has a key with a unique value, which is used by *DataStore* to retrieve, update or 
release (remove the item from the datstore = delete) an item. Keys are also used to establish 
child parent relationships (i.e. the child stores the parent's key). The key value gets 
assigned by *DataStore* and should not have 
any meaning for the rest of the software (except that a key of -1 shows that the item is not 
stored yet). A new key is always 1 higher than the highest 
already existing key value. This guarantees that items are always stored sorted in asscending 
key values, which allows for a binary search when retrieving an item in the *DataStore*.

There are 2 formats how a .csv file can store items:

1) If the items are updatable or releasable, the first character of a .csv line indicates if it is an Add, Update or Relase record, followed by the key value.
2) I the items are neiter updatable nor releasable, it contains only Add records and therefore the first character does not indicate the record type and even the key value is not written to the file. The first item has always the key 0 and all following items get their keys incremented by one. 

**Note:** If keys are continuous, i.e if there is no key missing, a search is no longer 
needed to retrieve an item, *DataStore* can just use the item key to find the item. This 
works also when the key of the first item is not 0. 

*DataStore* decides if keys are continuous like this:

```csharp
AreKeysContinuous = 
  !AreInstancesReleasable || 
  Count<2 || 
  lastKeyValue - fistKeyValue + 1 == Count;
``` 

## Data Model

The Data Model defines all classes and their relationships among them as shown in 
*Relationship examples*. The StorageClassGenerator reads the Data Model and creates a) a
Data Context class and b) for each class in the Data Model a new class supporting the creation, 
storing, updating and deleting (removal) of instances.

## Generated Data Model Classes
As with any other C# class, the constructor **new()** is called to create a new instance (object) 
of that class. The
constructor has a *isStoring* parameter, which indicates if the object should get written
immediately to a file. Otherwise, the instance can get written later to the file by calling its 
**Store()** method. A child can only get stored if all its parents are stored already, otherwise
`new(isStoring: true)` an `Store()` throw an exception.

*StorageLib* adds to each class a **Key** property. Its value is smaller 0 until the
object gets stored, at which time the key gets a unique value, which will be biggest key used 
so far + 1.

None of the properties can be changed directly, they must be changed by calling the object's
**Update()** method. This allows changing and writing the new content to the file of several 
properties at the same time. For performance reason, it is important that the writing 
does not get executed for every single property that changes. 

If an object is no longer needed, **Release()** can be called. Release removes the 
object from the data context and adds an new entry to the file marking the object as deleted.
 

## DC Data Context
The DC gives access to all stored data. For each class in the Data Model, a `DataStore` 
collection gets 
added to the DC. A particular instance can get retrieved from the class' `DataStore` through 
the object's  Key value. `DataStore` implements *IReadOnlyCollection* for accessing 
any data object. A class can indicate in the Data Model that it should be searchable through one
of its property values, like maybe Name. In that case, the *StorageLib* adds a 
`Dictionary<string, searchableClass>` to the Data Context. This Dictionary can be used like an 
index in a database. *StorageLib* updates the Dictionary when objects get stored, updated or
released.

At startup, the DC reads all files and fills the collections with instances. When the
program runs, data changes get automatically written to the files, although with a
small time delay to write many changes at the same time, which improves performance. At
application shutdown, the DC guarantees that all data is written (flushed) to the files.

DC has a static property *Data*, which holds all the collections. The application can access 
any data through *DC.Data.CollectionName*.

## Transactions

```c#
DC.Data.StartTransaction();
var parent = new Parent(..., isStoring: true);
var child = new Child(Parent, ..., isStoring: false);
child.Store();
child.Update(...);
if (ok){
  DC.Data.CommitTransaction();
}else{
  DC.Data.RollbackTransaction();
}
```

Any data change caused by `new()`, `Store()`, `Update()` or `Remove()` gets directly written to a
file, even before `DC.Data.CommitTransaction()` is called. `RollbackTransaction()` removes 
whatever was written since `StartTransaction()` from the files and restores the data in the RAM
to the values it had before the transaction started.

## Automatic data files compaction
An `Update()` or `Remove()` changes already stored data. For performance reasons, the stored
data in the middle of the file does not get changed, but a new line gets added at the end
of the file with the update or deletion data. In RAM is always the latest data, while the 
file contains a change history

A lot of disk space can be saved by writing a new data file containing only the actual 
data. The file with the change history changes its extension from .csv to .bak and a new
.csv file gets written with the actual content as stored in RAM.

If the application does not shut down properly because of an exception, no compacted file
might get written and the .csv file contains the history instead. When the application 
then starts again, the history will be executed, with the result that the data in 
RAM is the same as when the application stopped suddenly.


# Challenges
## What should happen when a child gets "deleted"

One challenge is that an instance cannot get truly deleted in .NET. It's only possible to remove 
all references to an instance.

Another challenge is that in a 1:c relationship, the child is not nullable and must always have 
a parent. If a child has a parent, then *StorageLib* ensures that that parent has a link (reference) 
back to that child. So if the child gets released, i.e. removed from the `Children` `DataStore` 
in the *Data Context*, the parent has still a reference to the child.

In theory, this problem could be solved by updating the `Parent` property in the child 
to null, which would remove the child from the `Children` `List` in the parent. That's not "nice"
from a design point of view, because releasing an item from the *Data Context* should not change the 
property values of that instance. Also, in an 1:c relationship, it is not possible to set the 
`Parent` to null.

Even worse is a *readonly* 1:c relationship, where the `Parent` property in the child is readonly. 
When the child gets created, immediately a parent child relationship gets established. Once the
child should get "deleted", C# doesn't allow the reference to its parent to be changed, meaning 
the parent will also keep a link to the child and the garbage collector can never remove that child.

As a consequence, even a deleted (released) child is still part of its parent's `Children` collection.

It makes actually sense if also not yet stored children and parents can 
establish relationships. For example the user creates some complicated objects 
and only once everything is set up correctly he decides if the objects should 
be stored or discarded.



# Further Documentation
* [Readme.md](Readme.md) describes main features of *StorageLib* and gives a high level overview how *StorageLib* works.
* [Setup.md](Setup.md) describes how to install a local copy of *StorageLib* on your PC and how to setup VS for your own application using *StorageLib*.
* [DataModel.md](DataModel.md) explains how to write your Data Model code, which defines the classes *StorageClassGenerator* will create for you.
