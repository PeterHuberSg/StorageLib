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
[**Design principals**](#design-principals)  
[**- Supported relationships**](#supported-relationships)  
[**- Parents first**](#parents-first)  
[**- Children define the relationship**](#children-define-the-relationship)  
[**- Relationship notation**](#relationship-notation)  
[**- Nullability indicates conditionality**](#nullability-indicates-conditionality)  
[**- Relationship examples**](#relationship-examples)  
[**- Data Model**](#Data-Model)  
[**- Generated Data Model Classes**](#generated-data-model-classes)  
[**- DC Data Context**](#DC-Data-Context)  
[**- Transactions**](#transactions)  
[**- Automatic data files compaction**](#automatic-data-files-compaction)  
  
[**Further Documentation**](#further-documentation)  


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

## Parents first
The parent objects must be stored before the child or the children linking to it. *StorageLib*
stores all instances (objects) of one class in one file. On application startup, the file gets read 
completely and all its objects created immediately, before the next file with the next 
class gets read. It must be possible to create any parent without any children, meaning
a `Child` property in a parent is always nullable (=conditional) and `Children` is always 
a collection, which can be empty (=mc).

## Children define the relationship
The child defines to which parent(s) it belongs. For each relationship to a parent it has one 
property. When a child property linking to a parent gets stored, the child gets added to the 
`Child` or `Children` propert of its parent. When a child changes (`Update()`) the value of 
its `Parent` property from `Parent1` to `Parent2`, *StorageLib* 
removes child from `Parent1.Children` and adds child to `Parent2.Children`. 

The parents get written to a file without any children information. When the parent gets 
read first during startup, the parents have no children. The children get added to the parent 
once the children files gets read.

*StorageLib* ensures that when a child has a link to a parent, that child is also added to the 
children collection of the parent. *"Children define the child parent relationship"* is only 
relevant in the sense that adding or removing a child from its
parent can only be achieved by changing the value of the child property linking to the 
parent. There is no method supporting removing the child directly form the children 
collection in the parent.

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
A child can have an unlimitted number of child parent relationships
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
object gets stored, at which time the key gets a unique value.

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
`Dictionary<string, searchableClass>` to the Data Context. This Dicionary can be used like an 
index in a database. *StorageLib* updates the Dictionary when objects get stored, updated or
released.

At startup, the DC reads all files and fills the collections with instances. When the
program runs, data changes get automatically written to the files, although with a
small time delay to write many changes at the same time, which improves performance. At
application shutdown, the DC guarantees that all data is written (flushed) to the files.

DC has a static property *Data*, which holds all the collections. The application can acces
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


# Further Documentation
* [Readme.md](Readme.md) describes main features of *StorageLib* and gives a high level overview how *StorageLib* works.
* [Setup.md](Setup.md) describes how to install a local copy of *StorageLib* on your PC and how to setup VS for your own application using *StorageLib*.
* [DataModel.md](DataModel.md) explains how to write your Data Model code, which defines the classes *StorageLib* will create for you.
