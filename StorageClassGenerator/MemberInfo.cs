/**************************************************************************************

StorageLib.MemberInfo
=====================

Some info for each property of a class defined in data model

Written in 2020-21 by Jürgpeter Huber 
Contact: https://github.com/PeterHuberSg/StorageLib

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;


//todo: MemberInfo: Add support for dateTime?

namespace StorageLib {


  /// <summary>
  /// Some info for each property of a class defined in data model
  /// </summary>
  public class MemberInfo {
    public readonly string MemberText;
    public readonly string MemberName;
    public readonly string LowerMemberName;
    public MemberTypeEnum MemberType;
    public readonly ClassInfo ClassInfo;
    public string? CsvTypeString;//uses types like Date instead of DateTime, which is needed when writing data to CSV files
    public string TypeString;
    public string TypeStringNotNullable;//is only different from TypeString when IsNullable
    public string? PropertyForToLower; // used by ToLower to indicate from which other property in this class a lower 
                                       // case version should be made.
    public MemberInfo? ToLowerTarget; // ToLower source member can use ToLowerTarget to find target
    public string? ReadOnlyTypeString; //used by Dictionary and SortedList: IReadOnlyDictionary<DateTime, DictionaryChild>
    public bool IsNullable; //is also set in Compiler.AnalyzeDependencies() for ToLower properties
    public string QMark = ""; //mi.IsNullable ? "?" : "";
    public bool IsReadOnly; //property is marked 'readonly' or class is not updatable 
    public readonly string? Comment;
    public readonly string? PrecissionComment;
    public readonly string? Rounding;
    public readonly string? DefaultValue;
    public readonly bool IsLookupOnly = false;
    public readonly bool NeedsDictionary = false;
    public readonly int MaxStorageSize;
    public readonly string? ChildTypeName; //used by List, Dictionary, SortedList and SortedBucketCollection
    public readonly string? LowerChildTypeName; //used by List, Dictionary, SortedList and SortedBucketCollection
    public string? ChildPropertyName; //used by Dictionary, SortedList and SortedBucketCollection
    public string? LowerChildPropertyName;
    public string? ChildKeyPropertyName; //used by Dictionary, SortedList and SortedBucketCollection
    public string? LowerChildKeyPropertyName;
    public string? ChildKeyTypeString; //used by Dictionary, SortedList and SortedBucketCollection, gets corrected in AnalyzeDependencies()
    public string? ChildKey2PropertyName; //used by SortedBucketCollection
    public string? LowerChildKey2PropertyName;
    public string? ChildKey2TypeString; //used by SortedBucketCollection, might get corrected in AnalyzeDependencies(), i.e Date to DateTime
    public readonly string? LowerParentType;
    public readonly string? CsvReaderRead;
    public readonly string? CsvWriterWrite;
    public readonly string? NoValue; //used to fill NoClass with a obviously bad value
    public string? ToStringFunc;

    public ClassInfo? ChildClassInfo;
    public List<MemberInfo>? MultipleChildrenMIs;//a List<Child> can be referenced by several properties in the child
                                                 //class, in which case the List<Child> gets replaced by a HashSet<Child>
    public MemberInfo? SingleChildMI;//all none List<> collections can have only 1 single property in the child class
                                     //referencing the parent
    public bool IsChildReadOnly;
    public ClassInfo? ParentClassInfo; //not really used
    public MemberInfo? ParentMemberInfo;
    public EnumInfo? EnumInfo;

    ////todo: delete next 2 lines
    //public MemberInfo? ChildMemberInfo;//is null for lists, because several child members can point to different parent lists
    //public int ChildCount = 0;


    /// <summary>
    /// constructor for simple data type, not enum
    /// </summary>
    public MemberInfo(
      string memberText,
      string name, 
      string csvTypeString, 
      MemberTypeEnum memberType, 
      ClassInfo classInfo, 
      bool isNullable,
      bool isReadOnly,
      string? comment, 
      string? defaultValue,
      bool needsDictionary) 
    {
      MemberText = memberText;
      MemberName = name;
      LowerMemberName = name.ToCamelCase();
      CsvTypeString = csvTypeString;
      MemberType = memberType;
      ClassInfo = classInfo;
      IsReadOnly = isReadOnly;
      if (isReadOnly) ClassInfo.HasReadOnlies = true;
      Comment = comment;
      DefaultValue = defaultValue;
      NeedsDictionary = needsDictionary;
      if (needsDictionary && !IsReadOnly) ClassInfo.HasNotReadOnlyNeedDirectories = true;

      switch (memberType) {
      case MemberTypeEnum.Date:
        TypeString = "DateTime";
        MaxStorageSize = "12.12.1234\t".Length;
        CsvWriterWrite = "WriteDate";
        if (isNullable) {
          CsvReaderRead = "ReadDateNull()";
          NoValue = "null";
          ToStringFunc = "?.ToShortDateString()";
        } else {
          CsvReaderRead = "ReadDate()";
          NoValue = "DateTime.MinValue.Date";
          ToStringFunc = ".ToShortDateString()";
        }
        PrecissionComment = "Stores only dates but no times.";
        Rounding = ".Floor(Rounding.Days)";
        break;

      case MemberTypeEnum.Time:
        TypeString = "TimeSpan";
        MaxStorageSize = "23:59:59\t".Length;
        CsvWriterWrite = "WriteTime";
        if (isNullable) {
          CsvReaderRead = "ReadTimeNull()";
          NoValue = "null";
        } else {
          CsvReaderRead = "ReadTime()";
          NoValue = "TimeSpan.MinValue";
        }
        ToStringFunc = "";
        PrecissionComment = "Stores less than 24 hours with second precision.";
        Rounding = ".Round(Rounding.Seconds)";
        break;

      case MemberTypeEnum.DateMinutes:
        TypeString = "DateTime";
        MaxStorageSize = "12.12.1234 23:59\t".Length;
        CsvWriterWrite = "WriteDateMinutes";
        if (isNullable) {
          CsvReaderRead = "ReadDateSecondsNull()";//can also be used for minutes
          NoValue = "null";
        } else {
          CsvReaderRead = "ReadDateSeconds()";//can also be used for minutes
          NoValue = "DateTime.MinValue";
        }
        ToStringFunc = "";
        PrecissionComment = "Stores date and time with minute preclusion.";
        Rounding = ".Round(Rounding.Minutes)";
        break;

      case MemberTypeEnum.DateSeconds:
        TypeString = "DateTime";
        MaxStorageSize = "12.12.1234 23:59:59\t".Length;
        CsvWriterWrite = "WriteDateSeconds";
        if (isNullable) {
          CsvReaderRead = "ReadDateSecondsNull()";
          NoValue = "null";
        } else {
          CsvReaderRead = "ReadDateSeconds()";
          NoValue = "DateTime.MinValue";
        }
        ToStringFunc = "";
        PrecissionComment = "Stores date and time with seconds precision.";
        Rounding = ".Round(Rounding.Seconds)";
        break;

      case MemberTypeEnum.DateTimeTicks: 
        TypeString = "DateTime";
        MaxStorageSize = (long.MaxValue.ToString()+"\t").Length;
        CsvWriterWrite = "WriteDateTimeTicks";
        if (isNullable) {
          CsvReaderRead = "ReadDateTimeTicksNull()";
          NoValue = "null";
        } else {
          CsvReaderRead = "ReadDateTimeTicks()";
          NoValue = "DateTime.MinValue";
        }
        ToStringFunc = "";
        PrecissionComment = "Stores date and time with tick precision.";
        break;

      case MemberTypeEnum.TimeSpanTicks:
        TypeString = "TimeSpan";
        MaxStorageSize = (long.MaxValue.ToString()+"\t").Length;
        CsvWriterWrite = "WriteTimeSpanTicks";
        if (isNullable) {
          CsvReaderRead = "ReadTimeSpanTicksNull()";
          NoValue = "null";
        } else {
          CsvReaderRead = "ReadTimeSpanTicks()";
          NoValue = "TimeSpan.MinValue";
        }
        ToStringFunc = "";
        PrecissionComment = "Stores time duration with tick precision.";
        break;

      case MemberTypeEnum.Decimal: 
        TypeString = "decimal";
        MaxStorageSize = (decimal.MinValue.ToString()+"\t").Length;
        if (isNullable) {
          CsvReaderRead = "ReadDecimalNull()";
          NoValue = "null";
        } else {
          CsvReaderRead = "ReadDecimal()";
          NoValue = "Decimal.MinValue";
        }
        CsvWriterWrite = "Write";
        ToStringFunc = "";
        PrecissionComment = "Stores date and time with maximum precision.";
        break;

      case MemberTypeEnum.Decimal2:
        TypeString = "decimal";
        MaxStorageSize = "-1234567.89\t".Length;//reasonable limit, but could be as long as decimal.MinValue
        if (isNullable) {
          CsvReaderRead = "ReadDecimalNull()";
          NoValue = "null";
        } else {
          CsvReaderRead = "ReadDecimal()";
          NoValue = "Decimal.MinValue";
        }
        CsvWriterWrite = "WriteDecimal2";
        ToStringFunc = "";
        PrecissionComment = "Stores decimal with 2 digits after comma.";
        Rounding = ".Round(2)";
        break;

      case MemberTypeEnum.Decimal4:
        TypeString = "decimal";
        MaxStorageSize = "-12345.6789\t".Length;//reasonable limit, but could be as long as decimal.MinValue
        if (isNullable) {
          CsvReaderRead = "ReadDecimalNull()";
          NoValue = "null";
        } else {
          CsvReaderRead = "ReadDecimal()";
          NoValue = "Decimal.MinValue";
        }
        CsvWriterWrite = "WriteDecimal4";
        ToStringFunc = "";
        PrecissionComment = "Stores decimal with 4 digits after comma.";
        Rounding = ".Round(4)";
        break;

      case MemberTypeEnum.Decimal5:
        TypeString = "decimal";
        MaxStorageSize = "-1234.56789\t".Length;//reasonable limit, but could be as long as decimal.MinValue
        if (isNullable) {
          CsvReaderRead = "ReadDecimalNull()";
          NoValue = "null";
        } else {
          CsvReaderRead = "ReadDecimal()";
          NoValue = "Decimal.MinValue";
        }
        CsvWriterWrite = "WriteDecimal5";
        ToStringFunc = "";
        PrecissionComment = "Stores decimal with 5 digits after comma.";
        Rounding = ".Round(5)";
        break;

      case MemberTypeEnum.Bool:
        TypeString = "bool";
        MaxStorageSize = "1\t".Length;
        if (isNullable) {
          CsvReaderRead = "ReadBoolNull()";
          NoValue = "null";
        } else {
          CsvReaderRead = "ReadBool()";
          NoValue = "false";
        }
        CsvWriterWrite = "Write";
        ToStringFunc = "";
        break;

      case MemberTypeEnum.Int:
        TypeString = "int";
        MaxStorageSize = "-123456789\t".Length;//reasonable limit, but could be int.MinValue
        if (isNullable) {
          CsvReaderRead = "ReadIntNull()";
          NoValue = "null";
        } else {
          CsvReaderRead = "ReadInt()";
          NoValue = "int.MinValue";
        }
        CsvWriterWrite = "Write";
        ToStringFunc = "";
        break;

      case MemberTypeEnum.Long:
        TypeString = "long";
        MaxStorageSize = "-123456789123456789\t".Length;//reasonable limit, but could be long.MinValue
        if (isNullable) {
          CsvReaderRead = "ReadLongNull()";
          NoValue = "null";
        } else {
          CsvReaderRead = "ReadLong()";
          NoValue = "long.MinValue";
        }
        CsvWriterWrite = "Write";
        ToStringFunc = "";
        break;

      case MemberTypeEnum.Char:
        TypeString = "char";
        MaxStorageSize = 1;
        if (isNullable) {
          CsvReaderRead = "ReadCharNull()";
          NoValue = "null";
        } else {
          CsvReaderRead = "ReadChar()";
          NoValue = "char.MaxValue";
        }
        CsvWriterWrite = "Write";
        ToStringFunc = "";
        break;

      case MemberTypeEnum.String: 
        TypeString = "string";
        MaxStorageSize = 150;//reasonable limit, but could be much longer. CsvWriter checks if it writes longer strings and corrects this number for CsvReader
        if (isNullable) {
          CsvReaderRead = "ReadStringNull()";
          NoValue = "null";
        } else {
          CsvReaderRead = "ReadString()";
          NoValue = $"\"No{name}\"";
        }
        CsvWriterWrite = "Write";
        ToStringFunc = "";
        break;

      case MemberTypeEnum.Enum:
        throw new Exception("Enum needs to get constructed as Parent first and only later memberType gets changed to MemberTypeEnum.Enum.");
      case MemberTypeEnum.LinkToParent:
        throw new Exception("LinkToParent uses its own constructor.");
      case MemberTypeEnum.ParentOneChild:
        throw new Exception("ParentOneChild uses its own constructor.");
      case MemberTypeEnum.ParentMultipleChildrenList:
        throw new Exception("List uses its own constructor.");
      case MemberTypeEnum.ParentMultipleChildrenDictionary:
      case MemberTypeEnum.ParentMultipleChildrenSortedList:
        throw new Exception("Dictionary and SortedList use their own constructor.");
      case MemberTypeEnum.ParentMultipleChildrenSortedBucket:
        throw new Exception("SortedBucketCollection uses its own constructor.");
      default:
        throw new NotSupportedException();
      }

      TypeStringNotNullable = TypeString;
      IsNullable = isNullable;
      if (isNullable) {
        TypeString += '?';
        QMark = "?";
      }
    }


    /// <summary>
    /// constructor for toLower (copy of another property in lower case letters)
    /// </summary>
    public MemberInfo(
      string memberText,
      string name,
      ClassInfo classInfo,
      string toLower,
      bool isNullable,
      string? comment,
      bool needsDictionary) 
    {
      MemberText = memberText;
      MemberName = name;
      LowerMemberName = name.ToCamelCase();
      CsvTypeString = "string";
      MemberType = MemberTypeEnum.ToLower;
      ClassInfo = classInfo;
      PropertyForToLower = toLower;
      TypeString = "string";
      TypeStringNotNullable = TypeString;
      SetIsNullable(isNullable);
      IsReadOnly = false;
      Comment = comment;
      DefaultValue = null;
      NeedsDictionary = needsDictionary;
      if (needsDictionary && !IsReadOnly) ClassInfo.HasNotReadOnlyNeedDirectories = true;

      MaxStorageSize = 0;//toLower properties are not stored in CSV files, they live only in the RAM
      CsvWriterWrite = null;
      ToStringFunc = null;
      NoValue = null;
      //isNullable and IsReadOnly will be set like for the "other" property, the original

      //if (isNullable) {
      //  NoValue = "null";
      //} else {
      //  CsvReaderRead = "ReadString()";
      //  NoValue = $"\"No{name}\"";
      //}
    }


    /// <summary>
    /// constructor for ParentOneChild
    /// </summary>
    public MemberInfo(
      string memberText,
      string name,
      ClassInfo classInfo,
      string childType,
      string? childPropertyName,
      string? comment) 
    {
      MemberText = memberText;
      MemberType = MemberTypeEnum.ParentOneChild;
      MaxStorageSize = 0;//a reference is only stored in the child, not the parent
      MemberName = name;
      LowerMemberName = name.ToCamelCase();
      ClassInfo = classInfo;
      ChildTypeName = childType;
      LowerChildTypeName = childType.ToCamelCase();
      ChildPropertyName = childPropertyName;
      if (childPropertyName is not null) {
        LowerChildPropertyName = childPropertyName.ToCamelCase();
      }
      IsReadOnly = false;
      IsNullable = true;
      TypeStringNotNullable = childType;
      TypeString = childType + '?';
      QMark = "?";

      CsvReaderRead = null;
      CsvWriterWrite = null;
      Comment = comment;
      DefaultValue = null;
    }


    /// <summary>
    /// constructor for List
    /// </summary>
    public MemberInfo(
      int _,
      string memberText,
      string name, 
      ClassInfo classInfo, 
      string listType, 
      string childType, 
      string? childPropertyName,
      string? comment) 
    {
      MemberText = memberText;
      MemberType = MemberTypeEnum.ParentMultipleChildrenList;
      MaxStorageSize = 0;//a reference is only stored in the child, not the parent
      MemberName = name;
      LowerMemberName = name.ToCamelCase();
      ClassInfo = classInfo;
      ChildTypeName = childType;
      LowerChildTypeName = childType.ToCamelCase();
      ChildPropertyName = childPropertyName;
      if (childPropertyName is not null) {
        LowerChildPropertyName = childPropertyName.ToCamelCase();
      }
      SetIsNullable(false);
      IsReadOnly = false; //List properties are IReadOnlyList, but no need to mark them with ReadOnly
      TypeString = listType;
      TypeStringNotNullable = TypeString;
      CsvReaderRead = null;
      CsvWriterWrite = null;
      Comment = comment;
      DefaultValue = null;
    }


    /// <summary>
    /// constructor for CollectionKeyValue, i.e. Dictionary&lt;TKey, TValue> or SortedList&lt;TKey, TValue>
    /// </summary>
    public MemberInfo(
      string memberText,
      string name, 
      ClassInfo classInfo, 
      string memberTypeString,
      MemberTypeEnum memberType,
      string childType,
      string? childPropertyName, 
      string? childKeyPropertyName, 
      string keyTypeString, 
      string? comment) 
    {
      MemberText = memberText;
      MemberType = memberType;
      MaxStorageSize = 0;//a reference is only stored in the child, not the parent
      MemberName = name;
      LowerMemberName = name.ToCamelCase();
      ClassInfo = classInfo;
      ChildTypeName = childType;
      LowerChildTypeName = childType.ToCamelCase();
      ChildPropertyName = childPropertyName;
      if (childPropertyName is not null) {
        LowerChildPropertyName = childPropertyName.ToCamelCase();
      }
      ChildKeyPropertyName = childKeyPropertyName;
      if (childKeyPropertyName is not null) {
        LowerChildKeyPropertyName = childKeyPropertyName.ToCamelCase();
      }
      ChildKeyTypeString = keyTypeString;
      SetIsNullable(false);
      IsReadOnly = false; //Collection properties are IReadOnlyXXX, but not need to mark them with ReadOnly
      TypeString = memberTypeString; //will be overwritten in Compiler.AnalyzeDependencies()
      TypeStringNotNullable = TypeString;
      CsvReaderRead = null;
      CsvWriterWrite = null;
      Comment = comment;
      DefaultValue = null;
    }


    /// <summary>
    /// constructor for CollectionKeyKeyValue, i.e. SortedBucketCollection&lt;TKey1, TKey2, TValue>
    /// </summary>
    public MemberInfo(
      string memberText,
      string name,
      ClassInfo classInfo,
      string memberTypeString,
      MemberTypeEnum memberType,
      string childType,
      string? childPropertyName,
      string? childKeyPropertyName,
      string? childKey2PropertyName,
      string key1TypeString,
      string key2TypeString,
      string? comment) 
    {
      MemberText = memberText;
      MemberType = memberType;
      MaxStorageSize = 0;//a reference is only stored in the child, not the parent
      MemberName = name;
      LowerMemberName = name.ToCamelCase();
      ClassInfo = classInfo;
      ChildTypeName = childType;
      LowerChildTypeName = childType.ToCamelCase();
      ChildPropertyName = childPropertyName;
      if (childPropertyName is not null) {
        LowerChildPropertyName = childPropertyName.ToCamelCase();
      }
      ChildKeyPropertyName = childKeyPropertyName;
      if (childKeyPropertyName is not null) {
        LowerChildKeyPropertyName = childKeyPropertyName.ToCamelCase();
      }
      ChildKey2PropertyName = childKey2PropertyName;
      if (childKey2PropertyName is not null) {
        LowerChildKey2PropertyName = childKey2PropertyName.ToCamelCase();
      }
      ChildKeyTypeString = key1TypeString;
      ChildKey2TypeString = key2TypeString;
      SetIsNullable(false);
      IsReadOnly = false; //Collection properties are IReadOnlyXXX, but no need to mark them with ReadOnly
      TypeString = memberTypeString; //will be overwritten in Compiler.AnalyzeDependencies()
      TypeStringNotNullable = TypeString;
      CsvReaderRead = null;
      CsvWriterWrite = null;
      Comment = comment;
      DefaultValue = null;
    }


    /// <summary>
    /// constructor for a parent property in child class, which can be a collection, lookup or enum
    /// </summary>
    public MemberInfo(
      string memberText,
      string name, 
      ClassInfo classInfo, 
      string memberTypeString, 
      bool isNullable,
      bool isReadOnly,
      string? comment, 
      bool isLookupOnly) 
    {
      MemberText = memberText;
      MemberType = MemberTypeEnum.LinkToParent;
      MemberName = name;
      LowerMemberName = name.ToCamelCase();
      ClassInfo = classInfo;
      IsReadOnly = isReadOnly;
      if (isReadOnly) ClassInfo.HasReadOnlies = true;
      LowerParentType = memberTypeString.ToCamelCase();
      CsvWriterWrite = "Write";
      TypeStringNotNullable = memberTypeString;
      IsNullable = isNullable;
      if (isNullable) {
        TypeString = memberTypeString + '?';
        QMark = "?";
        CsvReaderRead = "ReadIntNull()";
        NoValue = "null";
        ToStringFunc = "?.ToShortString()";
      } else {
        TypeString = memberTypeString;
        CsvReaderRead = "ReadInt()";
        NoValue = $"{TypeString}.No{TypeString}";
        ToStringFunc = ".ToShortString()";
      }
      Comment = comment;
      DefaultValue = null;
      IsLookupOnly = isLookupOnly;
    }


    internal void SetIsNullable(bool isNullable) {
      IsNullable = isNullable;
      if (isNullable) {
        TypeString += '?';
        QMark = "?";
      }
    }


    public override string ToString() {
      if (MemberType==MemberTypeEnum.ParentMultipleChildrenList) {
        return $"List<{ChildTypeName}> {MemberName}";
      }else if (MemberType==MemberTypeEnum.ParentMultipleChildrenDictionary || 
          MemberType==MemberTypeEnum.ParentMultipleChildrenSortedList ||
          MemberType==MemberTypeEnum.ParentMultipleChildrenSortedBucket) 
      {
        return $"{TypeString} {MemberName}";
      } else {
        return $"{MemberType}{QMark} {MemberName}";
      }
    }


    internal void WriteProperty(StreamWriter sw, bool isRaw = false) {
      if (isRaw && MemberType>=MemberTypeEnum.ParentOneChild) return;

      sw.WriteLine();
      sw.WriteLine();
      bool hasWrittenComment = false;
      if (Comment!=null) {
        var linesArray = Comment.Split(Environment.NewLine);
        foreach (var line in linesArray) {
          if (!string.IsNullOrWhiteSpace(line)) {
            if (PrecissionComment!=null && line.Contains("/// </summary>")) {
              hasWrittenComment = true;
              sw.WriteLine($"    /// {PrecissionComment}");
            }
            sw.WriteLine($"    {line}");
          }
        }
      }
      if (PrecissionComment!=null && !hasWrittenComment) {
        sw.WriteLine("    /// <summary>");
        sw.WriteLine($"    /// {PrecissionComment}");
        sw.WriteLine("    ///  </summary>");
      }
      if (MemberType==MemberTypeEnum.ParentMultipleChildrenList) {
        //if (ChildCount<1) {
        //  throw new Exception();
        //} else if (ChildCount==1) {
        //  if (ChildClassInfo!.AreInstancesReleasable) {
        //    sw.WriteLine($"    public IStorageReadOnlyList<{ChildTypeName}> {MemberName} => {LowerMemberName};");
        //    sw.WriteLine($"    readonly StorageList<{ChildTypeName}> {LowerMemberName};");
        //  } else {
        //    sw.WriteLine($"    public IReadOnly{TypeString} {MemberName} => {LowerMemberName};");
        //    sw.WriteLine($"    readonly List<{ChildTypeName}> {LowerMemberName};");
        //  }
        //} else {
        //  sw.WriteLine($"    public ICollection<{ChildTypeName}> {MemberName} => {LowerMemberName};");
        //  sw.WriteLine($"    readonly HashSet<{ChildTypeName}> {LowerMemberName};");
        //}
        if (SingleChildMI is not null) {
          if (ChildClassInfo!.AreInstancesReleasable) {
            sw.WriteLine($"    public IStorageReadOnlyList<{ChildTypeName}> {MemberName} => {LowerMemberName};");
            sw.WriteLine($"    readonly StorageList<{ChildTypeName}> {LowerMemberName};");
          } else {
            sw.WriteLine($"    public IReadOnly{TypeString} {MemberName} => {LowerMemberName};");
            sw.WriteLine($"    readonly List<{ChildTypeName}> {LowerMemberName};");
          }
        } else if (MultipleChildrenMIs is not null) {
          //A parent List<> referenced by 2 properties in the child class
          sw.WriteLine($"    public ICollection<{ChildTypeName}> {MemberName} => {LowerMemberName};");
          sw.WriteLine($"    readonly HashSet<{ChildTypeName}> {LowerMemberName};");
        } else {
          throw new Exception();
        }
      } else if (MemberType==MemberTypeEnum.ParentMultipleChildrenDictionary ||
        MemberType==MemberTypeEnum.ParentMultipleChildrenSortedList ||
        MemberType==MemberTypeEnum.ParentMultipleChildrenSortedBucket) 
      {
        sw.WriteLine($"    public {ReadOnlyTypeString} {MemberName} => {LowerMemberName};");
        sw.WriteLine($"    readonly {TypeString} {LowerMemberName};");

      } else {
        if (isRaw) {
          if (MemberType==MemberTypeEnum.LinkToParent) {
            sw.WriteLine($"    public int{QMark} {MemberName}Key {{ get; set; }}");
          } else {
            if (TypeString=="string") {
              sw.WriteLine($"    public string {MemberName} {{ get; set; }} =\"\";");
            } else {
              sw.WriteLine($"    public {TypeString} {MemberName} {{ get; set; }}");
            }
          }
        } else {
          if (IsReadOnly) {
            sw.WriteLine($"    public {TypeString} {MemberName} {{ get; }}");
          } else {
            sw.WriteLine($"    public {TypeString} {MemberName} {{ get; private set; }}");
          }
        }
      }
    }
  }


  public static class ToCamelCaseExtension {
    public static string ToCamelCase(this string str) {
      return str[0..1].ToLowerInvariant() + str[1..];
    }
  }
}
