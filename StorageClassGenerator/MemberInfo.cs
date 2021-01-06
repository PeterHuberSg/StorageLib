/**************************************************************************************

StorageLib.MemberInfo
=====================

Some info for each class defined in data model

Written in 2020 by Jürgpeter Huber 
Contact: PeterCode at Peterbox dot com

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/
using System;
using System.IO;


//todo: MemberInfo: Add support for dateTime?

namespace StorageLib {


  /// <summary>
  /// Some info for each class defined in data model
  /// </summary>
  public class MemberInfo {
    public readonly string MemberText;
    public readonly string MemberName;
    public readonly string LowerMemberName;
    public MemberTypeEnum MemberType;
    public readonly ClassInfo ClassInfo;
    public string? CsvTypeString;
    public string TypeString;
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
    public readonly string? ChildTypeName; //used by List, Dictionary and SortedList
    public readonly string? LowerChildTypeName; //used by List, Dictionary and SortedList
    public string? ChildKeyPropertyName; //used by Dictionary and SortedList
    public readonly string? ChildKeyTypeString; //used by Dictionary and SortedList
    public readonly string? ParentTypeString;//is only different from TypeString when IsNullable
    public readonly string? LowerParentType;
    public readonly string? CsvReaderRead;
    public readonly string? CsvWriterWrite;
    public readonly string? NoValue; //used to fill NoClass with a obviously bad value
    public string? ToStringFunc;

    public ClassInfo? ChildClassInfo;
    public MemberInfo? ChildMemberInfo;//is null for lists, because several child members can point to different parent lists
    public bool IsChildReadOnly;
    public ClassInfo? ParentClassInfo; //not really used
    public MemberInfo? ParentMemberInfo;
    public EnumInfo? EnumInfo;
    public int ChildCount = 0;


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
      LowerMemberName = name[0..1].ToLowerInvariant() + name[1..];
      CsvTypeString = csvTypeString;
      MemberType = memberType;
      ClassInfo = classInfo;
      SetIsNullable(isNullable);
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
        throw new Exception("Dictionary and SortedList uses its own constructor.");
      default:
        throw new NotSupportedException();
      }

      if (isNullable) {
        TypeString += '?';
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
      LowerMemberName = name[0..1].ToLowerInvariant() + name[1..];
      CsvTypeString = "string";
      MemberType = MemberTypeEnum.ToLower;
      ClassInfo = classInfo;
      PropertyForToLower = toLower;
      SetIsNullable(isNullable);
      IsReadOnly = false;
      Comment = comment;
      DefaultValue = null;
      NeedsDictionary = needsDictionary;
      if (needsDictionary && !IsReadOnly) ClassInfo.HasNotReadOnlyNeedDirectories = true;
      TypeString = "string";

      MaxStorageSize = 150;//reasonable limit, but could be much longer. CsvWriter checks if it writes longer strings and corrects this number for CsvReader
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
      string? comment) 
    {
      MemberText = memberText;
      MemberType = MemberTypeEnum.ParentOneChild;
      MaxStorageSize = 0;//a reference is only stored in the child, not the parent
      MemberName = name;
      LowerMemberName = name[0..1].ToLowerInvariant() + name[1..];
      ClassInfo = classInfo;
      ChildTypeName = childType;
      LowerChildTypeName = childType[0..1].ToLowerInvariant() + childType[1..];
      SetIsNullable(true);
      IsReadOnly = false; 
      TypeString = childType + '?';

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
      string? comment) 
    {
      MemberText = memberText;
      MemberType = MemberTypeEnum.ParentMultipleChildrenList;
      MaxStorageSize = 0;//a reference is only stored in the child, not the parent
      MemberName = name;
      LowerMemberName = name[0..1].ToLowerInvariant() + name[1..];
      ClassInfo = classInfo;
      ChildTypeName = childType;
      LowerChildTypeName = childType[0..1].ToLowerInvariant() + childType[1..];
      SetIsNullable(false);
      IsReadOnly = false; //List properties are IReadOnlyList, but no need to mark them with ReadOnly
      TypeString = listType;
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
      string? childKeyPropertyName, 
      string keyTypeString, 
      string? comment) 
    {
      MemberText = memberText;
      MemberType = memberType;
      MaxStorageSize = 0;//a reference is only stored in the child, not the parent
      MemberName = name;
      LowerMemberName = name[0..1].ToLowerInvariant() + name[1..];
      ClassInfo = classInfo;
      ChildTypeName = childType;
      LowerChildTypeName = childType[0..1].ToLowerInvariant() + childType[1..];
      ChildKeyPropertyName = childKeyPropertyName;
      ChildKeyTypeString = keyTypeString;
      SetIsNullable(false);
      IsReadOnly = false; //Collection properties are IReadOnlyXXX, but not need to mark them with ReadOnly
      TypeString = memberTypeString; //will be overwritten in Compiler.AnalyzeDependencies()
      CsvReaderRead = null;
      CsvWriterWrite = null;
      Comment = comment;
      DefaultValue = null;
    }


    /// <summary>
    /// constructor for a parent property in child class
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
      LowerMemberName = name[0..1].ToLowerInvariant() + name[1..];
      ClassInfo = classInfo;
      SetIsNullable(isNullable);
      IsReadOnly = isReadOnly;
      if (isReadOnly) ClassInfo.HasReadOnlies = true;
      ParentTypeString = memberTypeString;
      LowerParentType = memberTypeString[0..1].ToLowerInvariant() + memberTypeString[1..];
      CsvWriterWrite = "Write";
      if (isNullable) {
        TypeString = memberTypeString + '?';
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
          MemberType==MemberTypeEnum.ParentMultipleChildrenSortedList) 
      {
        return $"{TypeString} {MemberName}";
      } else {
        return $"{MemberType}{QMark} {MemberName}";
      }
    }


    internal void WriteProperty(StreamWriter streamWriter, bool isRaw = false) {
      if (isRaw && MemberType>=MemberTypeEnum.ParentOneChild) return;

      streamWriter.WriteLine();
      streamWriter.WriteLine();
      bool hasWrittenComment = false;
      if (Comment!=null) {
        var linesArray = Comment.Split(Environment.NewLine);
        foreach (var line in linesArray) {
          if (!string.IsNullOrWhiteSpace(line)) {
            if (PrecissionComment!=null && line.Contains("/// </summary>")) {
              hasWrittenComment = true;
              streamWriter.WriteLine($"    /// {PrecissionComment}");
            }
            streamWriter.WriteLine($"    {line}");
          }
        }
      }
      if (PrecissionComment!=null && !hasWrittenComment) {
        streamWriter.WriteLine("    /// <summary>");
        streamWriter.WriteLine($"    /// {PrecissionComment}");
        streamWriter.WriteLine("    ///  </summary>");
      }
      if (MemberType==MemberTypeEnum.ParentMultipleChildrenList) {
        if (ChildCount<1) {
          throw new Exception();
        } else if (ChildCount==1) {
          streamWriter.WriteLine($"    public IReadOnly{TypeString} {MemberName} => {LowerMemberName};");
          streamWriter.WriteLine($"    readonly List<{ChildTypeName}> {LowerMemberName};");
        } else {
          streamWriter.WriteLine($"    public ICollection<{ChildTypeName}> {MemberName} => {LowerMemberName};");
          streamWriter.WriteLine($"    readonly HashSet<{ChildTypeName}> {LowerMemberName};");
        }
      } else if (MemberType==MemberTypeEnum.ParentMultipleChildrenDictionary ||
        MemberType==MemberTypeEnum.ParentMultipleChildrenSortedList) 
      {
        //streamWriter.WriteLine($"    public IReadOnly{TypeString} {MemberName} => {LowerMemberName};");
        streamWriter.WriteLine($"    public {ReadOnlyTypeString} {MemberName} => {LowerMemberName};");
        streamWriter.WriteLine($"    readonly {TypeString} {LowerMemberName};");

      } else {
        if (isRaw) {
          if (MemberType==MemberTypeEnum.LinkToParent) {
            streamWriter.WriteLine($"    public int{QMark} {MemberName}Key {{ get; set; }}");
          } else {
            if (TypeString=="string") {
              streamWriter.WriteLine($"    public string {MemberName} {{ get; set; }} =\"\";");
            } else {
              streamWriter.WriteLine($"    public {TypeString} {MemberName} {{ get; set; }}");
            }
          }
        } else {
          if (IsReadOnly) {
            streamWriter.WriteLine($"    public {TypeString} {MemberName} {{ get; }}");
          } else {
            streamWriter.WriteLine($"    public {TypeString} {MemberName} {{ get; private set; }}");
          }
        }
      }
    }
  }
}
