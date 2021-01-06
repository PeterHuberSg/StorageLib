# Introduction  
This document specifies the content of a *Data Model* .cs file.


# Table of Contents  
[**Design principals**](#design-principals)  

# Data Model File Structure

# Supported Data Types in a Data Model
The data class properties can be simple type as described below or other classes defined in the Data Model.

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

## DateTime Replacements
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



