# Introduction
*StorageLib* is a single user C# only library for lightening fast object oriented data storage in RAM and 
long term storage on a local hard disk. No database required.

**Main features**
* object oriented data storage in RAM using collections like List, Dictionary and SortedList 
for parent child relationships
* flat file storage per class in CSV file for permanent storage (optional)
* automatically created *create*, *store*, *update* and *release* (CRUD) operations
* developer does not need to write any boiler plate code
* finely grated extensibility of generated code by using partial classes
* support for readonly and nullable properties
* transactions (start, rollback and commit transaction)
* fast backup at application start if requested
* fast compacting of files at application end
* detailed tracing for debugging
* maximum data size depends on available RAM
* high speed: 30'000 transactions per second including permanent storage
* ideal for single user applications
* data files can be read and manipulated with any Editor

Using a database for single user applications just to store data permanently is overkill, adds 
a lot of unnecessary complexity and slows down the program execution.

Nowadays PC have lots of RAM. For many single user applications it is possible to keep all the 
data in RAM and to do queries using Linq, which leads to much faster programs than interfacing 
with a "slow" database. Also the mismatch between data types in DotNet and databases
can be avoided.

To store the data permanently, it's enough to write them into local files. This can be done using 
UTF8 instead of binary, which makes it easy to inspect and edit these files with any Editor, but 
the storage requirement is not much bigger. Storing '1234567' as string takes the same space 
(7 bytes plus delimiter) like storing it as 8 byte binary.

This library contains high performance *Readers* and *Writers* for CSV ('comma' separated values) 
files and a code generator for the object related data model in RAM, using .Net 5.


# Using StorageLib
*StorageLib* comes with a code generator. It reads the definition of data classes (=*Data 
Model*) from a *source directory* and writes new versions of the data classes with the 
abilities to write to and to read from CSV files into the *target directory*, together with 
a *data context* class, which gives access to all stored data.

![](Generator.jpg)


## Structure of StorageLib Solution

The *StorageLib* solution has 4 parts:

### Storage Libraries
*StorageLib* consists of 2 .dll, which need to get added to the user's application:

**StorageClassGenerator:** is used to translate the *Data Model* into data classes and a *Data Context*.  
**StorageLib:** contains the run time code which provides the data storage functionality. 

### Get Started Example Projects for first time user
The projects with 'GetStarted' in its name show a new user how to create his own *Data Model* 
and application consuming the data.

**GetStartedDataModel:** A very simple *Data Model* explaining how to get started  
**GetStartedContext:** Contains the data classes created based on *GetStartedDataModel*  
**GetStartedConsole:** Console application using *GetStartedContext*, showing how to create, 
update and delete data. 

To save storage space, git ignores all autocreated files. Once you run 
'GetStartedDataModel\Program.cs' to create these classes, you can then build and run 
*GetStartedConsole*.

### Samples for all Data Model functionality
The projects with 'Sample' in its name provides a sample code for every *Data Model* 
functionality supported by *StorageLib*.

**SampleDataModel:** Contains one class for each *Data Model* feature  
**SampleDataContext:** Contains the data classes created based on *SampleDataModel*  
**SampleDataConsole:** Console application using SampleDataContext, not really used 

To save storage space, git ignores all autocreated files. Once you run 
'SampleDataModel\Program.cs' to create these classes, you can see how the generated 
code looks like.

### Storage Libraries Tests
The *StorageLib* testing consist of 3 projects:

**TestDataModel:** defines the data used for testing  
**TestDataContext:** contains the test data classes produced by *StorageClassGenerator*  
**StorageTest:**  contains the actual tests

To save storage space, git ignores all autocreated files. Once you run 
'TestDataModel\Program.cs' to create these classes, build and run the tests in
*StorageTest*.


# Performance
I was curious if StorageLib is faster than a solution using a database, so I 
wrote a performance test, comparing StorageLib to SQLite. It seems that 
StorageLib is 30 to 40 times faster querying data than SQLite. This is not 
surprising, because StorageLib stores all data in RAM as C# collections, while 
SQLite might need to access the disk if some data is missing in RAM.

Compared to SQLite, StorageLib is 2 to 5 times faster inserting, updating and 
deleting data.

I put the performance test in its own github repository, because I didn't want 
any SQLite dependencies in StorageLib:

[github.com/PeterHuberSg/StorageLibBenchmark](https://github.com/PeterHuberSg/StorageLibBenchmark)


# Getting started
For your own project, you just need to include the *StorageClassGenerator* and 
*StorageLib* projects. For more details, see [Setup.md](Setup.md).

If you are new to StorageLib, it might be helpful to have a look at the *GetStarted* 
and *Sample* projects described above.


# Further Documentation
* [Setup.md](Setup.md) describes how to install a local copy of *StorageLib* on your PC and how to setup VS for your own application using *StorageLib*.
* [Design.md](Design.md) gives a high level introduction into the data design principals of *StorageLib* .
* [DataModel.md](DataModel.md) explains how to write your Data Model code, which defines the classes *StorageClassGenerator* will create for you.


# Project Status
Coding completed and tested. However new functionality will be added over time.
 
The current version is in use in 3 applications.

 

