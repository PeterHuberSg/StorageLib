/**************************************************************************************

StorageLib.ClassInfo
====================

Some infos about a class to be generated

Written in 2020 by Jürgpeter Huber 
Contact: https://github.com/PeterHuberSg/StorageLib

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Microsoft.CodeAnalysis.CSharp.Syntax;


namespace StorageLib {

  /// <summary>
  /// Some infos about a class to be generated
  /// </summary>
  public class ClassInfo {
    public readonly string ClassName;
    public readonly string LowerClassName;
    public readonly string RawName;
    public readonly string LowerRawName;
    public readonly string ReaderName;
    public readonly string WriterName;
    public readonly string? ClassComment;
    public readonly string PluralName;
    public readonly string LowerPluralName;
    public readonly bool AreInstancesUpdatable;
    public readonly bool AreInstancesReleasable;
    public readonly bool IsConstructorPrivate;
    public readonly bool IsGenerateReaderWriter;
    //public bool IsPerformReleaseNeeded = true;
    public readonly Dictionary<string, MemberInfo> Members;
    public bool HasParents;
    public bool HasNotReadOnlyAndNotLookupParents;
    public bool HasNotReadOnlyNeedDirectories;
    public bool HasReadOnlies;
    public readonly HashSet<ClassInfo> ParentsAll;
    public readonly HashSet<ClassInfo> Children;

    public int EstimatedMaxByteSize;
    public int HeaderLength;
    public bool IsAddedToParentChildTree;
    public int StoreKey;
    private object sdw;

    public ClassInfo(
      string name, 
      string? classComment, 
      string pluralName, 
      bool areInstancesUpdatable, 
      bool areInstancesReleasable,
      bool isConstructorPrivate,
      bool isGenerateReaderWriter)
    {
      ClassName = name;
      LowerClassName = name[0..1].ToLowerInvariant() + name[1..];
      RawName = name + "Raw";
      LowerRawName = LowerClassName + "Raw";
      ReaderName = name + "Reader";
      WriterName = name + "Writer";
      ClassComment = classComment;
      PluralName = pluralName;
      LowerPluralName = pluralName.ToCamelCase();
      AreInstancesUpdatable = areInstancesUpdatable;
      AreInstancesReleasable = areInstancesReleasable;
      IsConstructorPrivate = isConstructorPrivate;
      IsGenerateReaderWriter = isGenerateReaderWriter;
      Members = new Dictionary<string, MemberInfo>();
      ParentsAll = new HashSet<ClassInfo>();
      //ParentsWithList = new HashSet<ClassInfo>();
      Children = new HashSet<ClassInfo>();
      EstimatedMaxByteSize = 0;
      if (AreInstancesReleasable || AreInstancesUpdatable) {
        HeaderLength = "Key".Length + 1;
      } else {
        HeaderLength = 0;
      }
      IsAddedToParentChildTree = false;
    }


    public void AddMember(
      string memberText,
      string name, 
      string csvTypeString, 
      string? propertyComment, 
      string ? defaultValue, 
      bool? isLookupOnly,
      bool isParentOneChild,
      string? toLower,
      bool needsDictionary,
      string? childPropertyName,
      string? childKeyPropertyName,
      string? childKey2PropertyName,
      bool isReadOnly) 
    {
      var isLookUp = false;
      var isNullable = csvTypeString[^1]=='?';
      if (isNullable) {
        csvTypeString = csvTypeString[..^1];
      }
      MemberInfo member;
      var memberType = (csvTypeString.ToLowerInvariant()) switch
      {
        "date" => MemberTypeEnum.Date,
        "time" => MemberTypeEnum.Time,
        "dateminutes" => MemberTypeEnum.DateMinutes,
        "dateseconds" => MemberTypeEnum.DateSeconds,
        "datetimeticks" => MemberTypeEnum.DateTimeTicks,
        "timespanticks" => MemberTypeEnum.TimeSpanTicks,
        "decimal" => MemberTypeEnum.Decimal,
        "decimal2" => MemberTypeEnum.Decimal2,
        "decimal4" => MemberTypeEnum.Decimal4,
        "decimal5" => MemberTypeEnum.Decimal5,
        "bool" => MemberTypeEnum.Bool,
        "int" => MemberTypeEnum.Int,
        "long" => MemberTypeEnum.Long,
        "char" => MemberTypeEnum.Char,
        "string" => MemberTypeEnum.String,
        _ => MemberTypeEnum.Undefined,
      };

      if (toLower!=null) {
        //copy of another property in lower case letters
        //----------------------------------------------
        if (memberType!=MemberTypeEnum.String) {
          throw new GeneratorException($"{ClassName}.{name} is of type {csvTypeString}. The StoragePropertyAttribute parameter " +
            $"toLower can only be used with string type:" +
            Environment.NewLine + memberText);
        }
        if (isLookupOnly.HasValue || isParentOneChild) {
          throw new GeneratorException($"{ClassName}.{name} is of type string. It cannot have the StoragePropertyAttribute " +
            "parameters 'isLookupOnly' or 'isParentOneChild', which are only available for links to other classes:" + 
            Environment.NewLine + memberText);
        }
        if (defaultValue!=null) {
          throw new GeneratorException($"{ClassName}.{name} is a lower case copy of property {toLower}. It cannot have it's " +
            "own default value:" + Environment.NewLine + memberText);
        }
        if (isReadOnly) {
          throw new GeneratorException($"{ClassName}.{name} is a lower case copy of property {toLower}. It cannot be " +
            "marked readonly:" + Environment.NewLine + memberText);
        }
        if (childPropertyName is not null) {
          throw new GeneratorException($"{ClassName}.{name} is a lower case copy of property {toLower}. It cannot have a " +
            "StoragePropertyAttribute with a childPropertyName argument, which is only for ParentOneChild, Lists, Directories, SortedLists and SortedBucketCollections:" + Environment.NewLine + memberText);
        }
        if (childKeyPropertyName is not null) {
          throw new GeneratorException($"{ClassName}.{name} is a lower case copy of property {toLower}. It cannot have a " +
            "StoragePropertyAttribute with a childKeyPropertyName argument, which is only for Directories, SortedLists and SortedBucketCollections:" + Environment.NewLine + memberText);
        }
        if (childKey2PropertyName is not null) {
          throw new GeneratorException($"{ClassName}.{name} is a lower case copy of property {toLower}. It cannot have a " +
            "StoragePropertyAttribute with a childKey2PropertyName argument, which is only for SortedBucketCollections:" + Environment.NewLine + memberText);
        }
        member = new MemberInfo(memberText, name, this, toLower, isNullable, propertyComment, needsDictionary);

      } else if (memberType!=MemberTypeEnum.Undefined) {
        //simple data type
        //----------------
        if (isLookupOnly.HasValue || isParentOneChild) {
          throw new GeneratorException($"{ClassName}.{name} is of type {csvTypeString}. It " +
            "cannot have the attribute 'isLookupOnly' or 'isParentOneChild', which are only available for links to other classes:" +
            Environment.NewLine + memberText);
        }
        //illegal toLower combinations are handled already
        if (childPropertyName is not null) {
          throw new GeneratorException($"{ClassName}.{name} is of type {csvTypeString}. It cannot have a " +
            "StoragePropertyAttribute with a childPropertyName argument, which is only for ParentOneChild, Lists, Directories, SortedLists and SortedBucketCollections:" + Environment.NewLine + memberText);
        }
        if (childKeyPropertyName is not null) {
          throw new GeneratorException($"{ClassName}.{name} is of type {csvTypeString}. It cannot have a " +
            "StoragePropertyAttribute with a childKeyPropertyName argument, which is only for Directories, SortedLists and SortedBucketCollections:" + Environment.NewLine + memberText);
        }
        if (childKey2PropertyName is not null) {
          throw new GeneratorException($"{ClassName}.{name} is of type {csvTypeString}. It cannot have a " +
            "StoragePropertyAttribute with a childKey2PropertyName argument, which is only for SortedBucketCollections:" + Environment.NewLine + memberText);
        }
        member = new MemberInfo(memberText, name, csvTypeString, memberType, this, isNullable, isReadOnly, propertyComment,
          defaultValue, needsDictionary);

      } else {
        //links to other classes
        if (needsDictionary) {
          throw new GeneratorException($"{ClassName}.{name} is of type {csvTypeString}. It " +
            "cannot have the StoragePropertyAttribute parameter 'needsDictionary', which is only available for simple types:" +
            Environment.NewLine + memberText);
        }
        //illegal toLower combinations are handled already
        if (csvTypeString.Contains("<")) {
          //a parent having a collection for its children
          //=============================================

          if (isLookUp || isParentOneChild) {
            throw new GeneratorException($"{ClassName}.{name} is a collection of type {csvTypeString}. One to many relationships " +
              "using collections cannot have the StoragePropertyAttribute parameters 'isLookUp' or 'isParentOneChild':" +
              Environment.NewLine + memberText);
          }
          if (defaultValue!=null) {
            throw new GeneratorException($"{ClassName}.{name} is parent linking to children of type {csvTypeString}. It " +
              "cannot have the StoragePropertyAttribute parameter 'defaultValue':" +
              Environment.NewLine + memberText);
          }
          //illegal toLower, needsDictionary combinations are handled already
          if (csvTypeString.StartsWith("List<") && csvTypeString.EndsWith(">")) {
            //List
            //----
            if (isNullable) throw new GeneratorException($"Class '{ClassName}'.Property '{name}': {csvTypeString} cannot be nullable:" +
              Environment.NewLine + memberText);

            if (childKeyPropertyName is not null) {
              throw new GeneratorException($"{ClassName}.{name} is of type {csvTypeString}. It cannot have a " +
                "StoragePropertyAttribute with a childKeyPropertyName argument, which is only for Directories, SortedLists and SortedBucketCollections:" + Environment.NewLine + memberText);
            }

            if (childKey2PropertyName is not null) {
              throw new GeneratorException($"{ClassName}.{name} is of type {csvTypeString}. It cannot have a " +
                "StoragePropertyAttribute with a childKey2PropertyName argument, which is only for SortedBucketCollections:" + Environment.NewLine + memberText);
            }

            member = new MemberInfo(isList: true, memberText, name, this, csvTypeString, csvTypeString[5..^1], childPropertyName, propertyComment);

          }else if (csvTypeString.StartsWith("HashSet<") && csvTypeString.EndsWith(">")) {
            //HashSet
            //-------
            if (isNullable) throw new GeneratorException($"Class '{ClassName}'.Property '{name}': {csvTypeString} cannot be nullable:" +
              Environment.NewLine + memberText);

            if (childKeyPropertyName is not null) {
              throw new GeneratorException($"{ClassName}.{name} is of type {csvTypeString}. It cannot have a " +
                "StoragePropertyAttribute with a childKeyPropertyName argument, which is only for Directories, SortedLists and SortedBucketCollections:" + Environment.NewLine + memberText);
            }

            if (childKey2PropertyName is not null) {
              throw new GeneratorException($"{ClassName}.{name} is of type {csvTypeString}. It cannot have a " +
                "StoragePropertyAttribute with a childKey2PropertyName argument, which is only for SortedBucketCollections:" + Environment.NewLine + memberText);
            }

            member = new MemberInfo(isList: false, memberText, name, this, csvTypeString, csvTypeString[8..^1], childPropertyName, propertyComment);

          } else if ((csvTypeString.StartsWith("Dictionary<") || csvTypeString.StartsWith("SortedList<")) &&
            csvTypeString.EndsWith(">")) 
          {
            //Dictionary or SortedList
            //------------------------
            // csvTypeString: SortedList<DateTime, Sample>
            // csvTypeString: Dictionary<DateTime, Sample>
            if (isNullable) throw new GeneratorException(ClassName, memberText, $"{csvTypeString} cannot be nullable.");

            // csvTypeString: Dictionary<DateTime, Sample>
            var openBracketPos = csvTypeString.IndexOf('<');
            if (openBracketPos<0) throw new GeneratorException(ClassName, memberText, $"In '{csvTypeString}' " +
              "opening '<' is missing.");
            var closingBracketPos = csvTypeString.IndexOf('>');
            if (closingBracketPos<0) throw new GeneratorException(ClassName, memberText, $"In '{csvTypeString}' " +
              "closing '>' is missing:");
            var comaPos = csvTypeString.IndexOf(',');
            if (comaPos<0) throw new GeneratorException(ClassName, memberText, $"In '{csvTypeString}' a ',' is " +
              "missing.");
            if (comaPos<openBracketPos) throw new GeneratorException(ClassName, memberText, $"In '{ csvTypeString}' " +
              "coma ',' should come after '<'.");
            if (closingBracketPos<comaPos) throw new GeneratorException(ClassName, memberText, $"In '{csvTypeString}' " +
              "coma ',' should come before '>'.");
            var keyTypeString = csvTypeString[(openBracketPos+1)..(comaPos)].Trim();
            var itemTypeName = csvTypeString[(comaPos+1)..(closingBracketPos)].Trim();

            if (csvTypeString.StartsWith("SortedList<")) {
              memberType = MemberTypeEnum.ParentMultipleChildrenSortedList;
            } else {
              memberType = MemberTypeEnum.ParentMultipleChildrenDictionary;
            }
            member = new MemberInfo(
              memberText,
              name,
              this,
              csvTypeString,
              memberType,
              itemTypeName,
              childPropertyName,
              childKeyPropertyName,
              keyTypeString,
              propertyComment);

          } else if (csvTypeString.StartsWith("SortedBucketCollection<") && csvTypeString.EndsWith(">")) {
            //SortedBucketCollection
            //----------------------
            // csvTypeString: SortedBucketCollection<DateTime, int, Sample>
            if (isNullable) throw new GeneratorException(ClassName, memberText, $"'{csvTypeString}' cannot be " +
              "nullable.");

            var openBracketPos = csvTypeString.IndexOf('<');
            if (openBracketPos<0) throw new GeneratorException(ClassName, memberText, $"In '{csvTypeString}' " +
              "Opening '<' is missing.");
            var closingBracketPos = csvTypeString.IndexOf('>');
            if (closingBracketPos<0) throw new GeneratorException(ClassName, memberText, $"In '{csvTypeString}' " +
              "Closing '>' is missing.");
            var coma1Pos = csvTypeString.IndexOf(',', openBracketPos+1);
            if (coma1Pos<0) throw new GeneratorException(ClassName, memberText, $"In '{csvTypeString}' " +
              "',' is missing.");
            var coma2Pos = csvTypeString.IndexOf(',',  coma1Pos+1);
            if (coma2Pos<0) throw new GeneratorException(ClassName, memberText, $"In '{csvTypeString}' " +
              "second',' is missing.");
            if (closingBracketPos<coma2Pos) throw new GeneratorException(ClassName, memberText, $"In " +
              "'{csvTypeString}' 2 comas ',' should come before '>'.");
            var key1TypeString = csvTypeString[(openBracketPos+1)..coma1Pos].Trim();
            var key2TypeString = csvTypeString[(coma1Pos+1)..coma2Pos].Trim();
            var itemTypeName = csvTypeString[(coma2Pos+1)..(closingBracketPos)].Trim();

            memberType = MemberTypeEnum.ParentMultipleChildrenSortedBucket;
            member = new MemberInfo(
              memberText,
              name,
              this,
              csvTypeString,
              memberType,
              itemTypeName,
              childPropertyName,
              childKeyPropertyName,
              childKey2PropertyName,
              key1TypeString,
              key2TypeString,
              propertyComment);
          } else {
            throw new GeneratorException(ClassName, memberText, $"'{csvTypeString}' is not supported. Should " +
              "it be a List<>, SortedList<,>, Dictionary<,> or SortedBucketCollection<,,> ?" +
              Environment.NewLine + memberText);
          }
        } else if (isParentOneChild) {

          //a parent having at most one child
          //=================================
          if (isLookupOnly.HasValue) {
            throw new GeneratorException(ClassName, memberText, $"{name} is a parent for at most 1 child. The " +
              "StoragePropertyAttribute parameters 'isLookupOnly' cannot be used here.");
          }
          if (!isNullable) {
            throw new GeneratorException(ClassName, memberText, $"{name} is a parent for at most 1 child. It must " +
              "be nullable. Reason: It must be possible to create the parent when the child does not exist yet.");
          }
          if (defaultValue!=null) {
            throw new GeneratorException($"{ClassName}.{name} is a parent linking to a {csvTypeString} child. It " +
              "cannot have the StoragePropertyAttribute parameter 'defaultValue':" +
              Environment.NewLine + memberText);
          }
          //illegal toLower, needsDictionary combinations are handled already
          if (childKeyPropertyName is not null) {
            throw new GeneratorException($"{ClassName}.{name} is a parent linking to a {csvTypeString} child. It cannot have a " +
              "StoragePropertyAttribute with a childKeyPropertyName argument, which is only for Directories, SortedLists and SortedBucketCollections:" + Environment.NewLine + memberText);
          }
          if (childKey2PropertyName is not null) {
            throw new GeneratorException($"{ClassName}.{name} is a parent linking to a {csvTypeString} child. It cannot have a " +
              "StoragePropertyAttribute with a childKey2PropertyName argument, which is only for SortedBucketCollections:" + Environment.NewLine + memberText);
          }
          member = new MemberInfo(memberText, name, this, csvTypeString, childPropertyName, propertyComment);
        } else {

          //a child linking to its parent, which can be a collection, lookup or enum
          //========================================================================
          //Todo: it should be possible to have default null or enum.XXX
          if (defaultValue!=null) {
            throw new GeneratorException($"{ClassName}.{name} is a child linking to a {csvTypeString} parent. It " +
              "cannot have the StoragePropertyAttribute parameter 'defaultValue':" +
              Environment.NewLine + memberText);
          }
          //illegal toLower, needsDictionary, isParentOneChild combinations are handled already
          if (childPropertyName is not null) {
            throw new GeneratorException($"{ClassName}.{name} is a child linking to a {csvTypeString} parent. It cannot have a " +
              "StoragePropertyAttribute with a childPropertyName argument, which is only for ParentOneChild, Lists, Directories, SortedLists and SortedBucketCollections:" + Environment.NewLine + memberText);
          }
          if (childKeyPropertyName is not null) {
            throw new GeneratorException($"{ClassName}.{name} is a child linking to a {csvTypeString} parent. It cannot have a " +
              "StoragePropertyAttribute with a childKeyPropertyName argument, which is only for Directories, SortedLists and SortedBucketCollections:" + Environment.NewLine + memberText);
          }
          if (childKey2PropertyName is not null) {
            throw new GeneratorException($"{ClassName}.{name} is a child linking to a {csvTypeString} parent. It cannot have a " +
              "StoragePropertyAttribute with a childKey2PropertyName argument, which is only for SortedBucketCollections:" + Environment.NewLine + memberText);
          }
          member = new MemberInfo(memberText, name, this, csvTypeString, isNullable, isReadOnly, propertyComment, isLookupOnly??false);
          isLookUp = true;
        }
      }

      if (!isLookUp && isLookupOnly.HasValue) {
        throw new GeneratorException($"Class '{ClassName}.{name}': Remove [StorageProperty(isLookupOnly: {isLookupOnly.Value})], " +
            "it can only be applied when referencing a parent:" +
            Environment.NewLine + memberText);
      }
      try {
        Members.Add(member.MemberName, member);
      } catch (Exception ex) {
        if (ex is ArgumentException && ex.Message.StartsWith("An item with the same key")) {
          throw new GeneratorException(ClassName, memberText, $"Class {ClassName} has 2 properties with the same " +
            $"name {member.MemberName}.");
        } else { 
          throw; 
        }
        throw;
      }
      EstimatedMaxByteSize +=member.MaxStorageSize;
      HeaderLength += member.MemberName.Length + 1;
    }


    public override string ToString() {
      return $"Class {ClassName}";
    }


    internal void WriteClassFile(StreamWriter sw, string nameSpace, bool isFullyCommented) {
      var cs /*CommentString*/= isFullyCommented ? "//" : "";
      if (!isFullyCommented) {
        sw.WriteLine("#pragma warning disable CA1822 // Mark members as static");
        sw.WriteLine("#pragma warning disable IDE0060 // Remove unused parameter");
      }
      sw.WriteLine("using System;");
      sw.WriteLine("using System.Collections.Generic;");
      sw.WriteLine("using StorageLib;");
      sw.WriteLine();
      sw.WriteLine();
      sw.WriteLine($"namespace {nameSpace} {{");
      sw.WriteLine();
      sw.WriteLine();
      if (ClassComment!=null) {
        var linesArray = ClassComment.Split(Environment.NewLine);
        foreach (var line in linesArray) {
          if (!string.IsNullOrWhiteSpace(line)) {
            sw.WriteLine($"  {line}");
          }
        }
      }
      sw.WriteLine($"  public partial class {ClassName}: IStorageItem<{ClassName}> {{");
      sw.WriteLine();
      sw.WriteLine();
      sw.WriteLine("    #region Properties");
      sw.WriteLine("    //      ----------");
      sw.WriteLine();
      sw.WriteLine("    #endregion");
      sw.WriteLine();
      sw.WriteLine();
      sw.WriteLine("    #region Events");
      sw.WriteLine("    //      ------");
      sw.WriteLine();
      sw.WriteLine("    #endregion");
      sw.WriteLine();
      sw.WriteLine();
      sw.WriteLine("    #region Constructors");
      sw.WriteLine("    //      ------------");
      sw.WriteLine();
      sw.WriteLine("    /// <summary>");
      sw.WriteLine("    /// Called once the constructor has filled all the properties");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine($"    {cs}partial void onConstruct() {{");
      sw.WriteLine($"    {cs}}}");
      sw.WriteLine();
      sw.WriteLine();
      sw.WriteLine("    /// <summary>");
      sw.WriteLine("    /// Called once the cloning constructor has filled all the properties. Clones have no children data.");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine($"    {cs}partial void onCloned({ClassName} clone) {{");
      sw.WriteLine($"    {cs}}}");
      sw.WriteLine();
      sw.WriteLine();
      sw.WriteLine("    /// <summary>");
      sw.WriteLine("    /// Called once the CSV-constructor who reads the data from a CSV file has filled all the properties");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine($"    {cs}partial void onCsvConstruct() {{");
      sw.WriteLine($"    {cs}}}");
      sw.WriteLine("    #endregion");
      sw.WriteLine();
      sw.WriteLine();
      sw.WriteLine("    #region Methods");
      sw.WriteLine("    //      -------");
      sw.WriteLine();
      sw.WriteLine("    /// <summary>");
      sw.WriteLine("    /// Called before {ClassName}.Store() gets executed");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine($"    {cs}partial void onStoring(ref bool isCancelled) {{");
      sw.WriteLine($"    {cs}}}");
      sw.WriteLine();
      sw.WriteLine();
      sw.WriteLine("    /// <summary>");
      sw.WriteLine($"    /// Called after {ClassName}.Store() is executed");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine($"    {cs}partial void onStored() {{");
      sw.WriteLine($"    {cs}}}");
      sw.WriteLine();
      sw.WriteLine();
      sw.WriteLine("    /// <summary>");
      sw.WriteLine($"    /// Called before {ClassName} gets written to a CSV file");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine($"    {cs}partial void onCsvWrite() {{");
      sw.WriteLine($"    {cs}}}");
      sw.WriteLine();
      sw.WriteLine();
      if (AreInstancesUpdatable) {
        sw.WriteLine("    /// <summary>");
        sw.WriteLine($"    /// Called before any property of {ClassName} is updated and before the HasChanged event gets raised");
        sw.WriteLine("    /// </summary>");
        sw.Write($"    {cs}partial void onUpdating(");
        if (!writeOnUpdateParameters(sw, updateTypeEnum.Implementation, cs)) {
          throw new GeneratorException($"Method '{ClassName}.onUpdating()': has no parameters. Are all properties readonly ?" +
            " Then add attribute [StorageClass(areInstancesUpdatable: false)] and remove readonly from the properties.");
        }
        sw.WriteLine();
        sw.WriteLine();
        sw.WriteLine("    /// <summary>");
        sw.WriteLine($"    /// Called after all properties of {ClassName} are updated, but before the HasChanged event gets raised");
        sw.WriteLine("    /// </summary>");
        sw.WriteLine($"    {cs}partial void onUpdated({ClassName} old) {{");
        sw.WriteLine($"    {cs}}}");
        sw.WriteLine();
        sw.WriteLine();
        sw.WriteLine("    /// <summary>");
        sw.WriteLine($"    /// Called after an update for {ClassName} is read from a CSV file");
        sw.WriteLine("    /// </summary>");
        sw.WriteLine($"    {cs}partial void onCsvUpdate() {{");
        sw.WriteLine($"    {cs}}}");
        sw.WriteLine();
        sw.WriteLine();
      }
      if (AreInstancesReleasable) {
        sw.WriteLine("    /// <summary>");
        sw.WriteLine($"    /// Called before {ClassName}.Release() gets executed");
        sw.WriteLine("    /// </summary>");
        sw.WriteLine($"    {cs}partial void onReleasing() {{");
        sw.WriteLine($"    {cs}}}");
        sw.WriteLine();
        sw.WriteLine();
        sw.WriteLine("    /// <summary>");
        sw.WriteLine($"    /// Called after {ClassName}.Release() got executed");
        sw.WriteLine("    /// </summary>");
        sw.WriteLine($"    {cs}partial void onReleased() {{");
        sw.WriteLine($"    {cs}}}");
        sw.WriteLine();
        sw.WriteLine();
        sw.WriteLine("    /// <summary>");
        sw.WriteLine($"    /// Called after {ClassName}.Disconnect() got executed");
        sw.WriteLine("    /// </summary>");
        sw.WriteLine($"    {cs}partial void onDisconnected() {{");
        sw.WriteLine($"    {cs}}}");
        sw.WriteLine();
        sw.WriteLine();
      }
      sw.WriteLine("    /// <summary>");
      sw.WriteLine($"    /// Called after 'new {ClassName}()' transaction is rolled back");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine($"    {cs}partial void onRollbackItemNew() {{");
      sw.WriteLine($"    {cs}}}");
      sw.WriteLine();
      sw.WriteLine();
      sw.WriteLine("    /// <summary>");
      sw.WriteLine($"    /// Called after {ClassName}.Store() transaction is rolled back");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine($"    {cs}partial void onRollbackItemStored() {{");
      sw.WriteLine($"    {cs}}}");
      sw.WriteLine();
      sw.WriteLine();
      if (AreInstancesUpdatable) {
        sw.WriteLine("    /// <summary>");
        sw.WriteLine($"    /// Called after {ClassName}.Update() transaction is rolled back");
        sw.WriteLine("    /// </summary>");
        sw.WriteLine($"    {cs}partial void onRollbackItemUpdated({ClassName} old{ClassName}) {{");
        sw.WriteLine($"    {cs}}}");
        sw.WriteLine();
        sw.WriteLine();
      }
      if (AreInstancesReleasable) {
        sw.WriteLine("    /// <summary>");
        sw.WriteLine($"    /// Called after {ClassName}.Release() transaction is rolled back");
        sw.WriteLine("    /// </summary>");
        sw.WriteLine($"    {cs}partial void onRollbackItemRelease() {{");
        sw.WriteLine($"    {cs}}}");
        sw.WriteLine();
        sw.WriteLine();
      }
      foreach (var mi in Members.Values) {
        if (mi.MemberType==MemberTypeEnum.ParentOneChild) {
          sw.WriteLine("    /// <summary>");
          sw.WriteLine($"    /// Called after a {mi.LowerChildTypeName} gets added to {mi.MemberName}.");
          sw.WriteLine("    /// </summary>");
          sw.WriteLine($"    {cs}partial void onAddedTo{mi.MemberName}({mi.ChildTypeName} {mi.LowerChildTypeName}){{");
          sw.WriteLine($"    {cs}}}");
          sw.WriteLine();
          sw.WriteLine();
          if (!mi.IsChildReadOnly) {
            sw.WriteLine("    /// <summary>");
            sw.WriteLine($"    /// Called after a {mi.LowerChildTypeName} gets removed from {mi.MemberName}.");
            sw.WriteLine("    /// </summary>");
            sw.WriteLine($"    {cs}partial void onRemovedFrom{mi.MemberName}({mi.ChildTypeName} {mi.LowerChildTypeName}){{");
            sw.WriteLine($"    {cs}}}");
            sw.WriteLine();
            sw.WriteLine();
          }
        } else  if (mi.MemberType>MemberTypeEnum.ParentOneChild) { //ParentMultipleChildrenXxx
          sw.WriteLine("    /// <summary>");
          sw.WriteLine($"    /// Called after a {mi.LowerChildTypeName} gets added to {mi.MemberName}.");
          sw.WriteLine("    /// </summary>");
          sw.WriteLine($"    {cs}partial void onAddedTo{mi.MemberName}({mi.ChildTypeName} {mi.LowerChildTypeName}){{");
          sw.WriteLine($"    {cs}}}");
          sw.WriteLine();
          sw.WriteLine();
          if (!mi.IsChildReadOnly) {
            sw.WriteLine("    /// <summary>");
            sw.WriteLine($"    /// Called after a {mi.LowerChildTypeName} gets removed from {mi.MemberName}.");
            sw.WriteLine("    /// </summary>");
            sw.WriteLine($"    {cs}partial void onRemovedFrom{mi.MemberName}({mi.ChildTypeName} {mi.LowerChildTypeName}){{");
            sw.WriteLine($"    {cs}}}");
            sw.WriteLine();
            sw.WriteLine();
          }
          if (mi.MemberType==MemberTypeEnum.ParentMultipleChildrenHashSet) {
            sw.WriteLine("    /// <summary>");
            sw.WriteLine($"    /// Called after rollback of AddTo{mi.MemberName}({mi.ChildTypeName} {mi.LowerChildTypeName}), removing {mi.LowerChildTypeName} from {mi.MemberName}");
            sw.WriteLine($"    /// without testing {mi.LowerChildTypeName} properties.");
            sw.WriteLine("    /// </summary>");
            sw.WriteLine($"    {cs}partial void onRollbackAddTo{mi.MemberName}({mi.ChildTypeName} {mi.LowerChildTypeName}){{");
            sw.WriteLine($"    {cs}}}");
            sw.WriteLine();
            sw.WriteLine();
          }
        }
      }
      sw.WriteLine("    /// <summary>");
      sw.WriteLine("    /// Updates returnString with additional info for a short description.");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine($"    {cs}partial void onToShortString(ref string returnString) {{");
      sw.WriteLine($"    {cs}}}");
      sw.WriteLine();
      sw.WriteLine();
      sw.WriteLine("    /// <summary>");
      sw.WriteLine("    /// Updates returnString with additional info for a short description.");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine($"    {cs}partial void onToString(ref string returnString) {{");
      sw.WriteLine($"    {cs}}}");
      sw.WriteLine("    #endregion");
      sw.WriteLine("  }");
      sw.WriteLine("}");
    }


    public void WriteBaseClassFile(StreamWriter sw, string nameSpace, string context, TracingEnum isTracing) {
      sw.WriteLine("//------------------------------------------------------------------------------");
      sw.WriteLine("// <auto-generated>");
      sw.WriteLine("//     This code was generated by StorageClassGenerator");
      sw.WriteLine("//");
      sw.WriteLine("//     Do not change code in this file, it will get lost when the file gets ");
      sw.WriteLine($"//     auto generated again. Write your code into {ClassName}.cs.");
      sw.WriteLine("// </auto-generated>");
      sw.WriteLine("//------------------------------------------------------------------------------");
      sw.WriteLine("#nullable enable");
      sw.WriteLine("using System;");
      sw.WriteLine("using System.Collections.Generic;");
      if (IsGenerateReaderWriter) {
        sw.WriteLine("using System.Diagnostics.CodeAnalysis;");
      }
      sw.WriteLine("using System.Threading;");
      sw.WriteLine("using StorageLib;");
      sw.WriteLine();
      sw.WriteLine();
      sw.WriteLine($"namespace {nameSpace} {{");
      sw.WriteLine();
      sw.WriteLine();
      if (ClassComment!=null) {
        var linesArray = ClassComment.Split(Environment.NewLine);
        foreach (var line in linesArray) {
          if (!string.IsNullOrWhiteSpace(line)) {
            sw.WriteLine($"  {line}");
          }
        }
      }
      sw.WriteLine($"  public partial class {ClassName}: IStorageItem<{ClassName}> {{");
      sw.WriteLine();

      var lines = new List<string>();
      writeProperties(sw, context, lines, isTracing);
      writeEvents(sw, context);
      writeConstructor(sw, context, lines, isTracing);
      writeMethods(sw, context, lines, isTracing);
    }


    #region Properties Code
    //      ---------------

    private void writeProperties(StreamWriter sw, string context, List<string> lines, TracingEnum isTracing) {
      sw.WriteLine("    #region Properties");
      sw.WriteLine("    //      ----------");
      sw.WriteLine();
      sw.WriteLine("    /// <summary>");
      sw.WriteLine($"    /// Unique identifier for {ClassName}. Gets set once {ClassName} gets added to {context}.Data.");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine("    public int Key { get; private set; }");
      if (isTracing==TracingEnum.noTracing) {
        sw.WriteLine($"    internal static void SetKey(IStorageItem {LowerClassName}, int key, bool _) {{");
      } else {
        sw.WriteLine($"    internal static void SetKey(IStorageItem {LowerClassName}, int key, bool isRollback) {{");
      }
      Compiler.WriteLinesTracing(sw, isTracing,
                   $"      if (isRollback) {{",
                   $"        if (key==StorageExtensions.NoKey) {{",
                   $"          {context}.Trace?.Invoke($\"Release {ClassName} key @{{{LowerClassName}.Key}} #{{{LowerClassName}.GetHashCode()}}\");",
                   $"        }} else {{",
                   $"          {context}.Trace?.Invoke($\"Store {ClassName} key @{{key}} #{{{LowerClassName}.GetHashCode()}}\");",
                   $"        }}",
                   $"      }}");
      sw.WriteLine($"      (({ClassName}){LowerClassName}).Key = key;");
      sw.WriteLine("    }");
      foreach (var mi in Members.Values) {
        mi.WriteProperty(sw);
      }
      sw.WriteLine();
      sw.WriteLine();

      writeHeaders(sw, lines);
      writeNoClassSingleton(sw);
      sw.WriteLine("    #endregion");
      sw.WriteLine();
      sw.WriteLine();
    }


    private void writeHeaders(StreamWriter sw, List<string> lines) {
      sw.WriteLine("    /// <summary>");
      sw.WriteLine("    /// Headers written to first line in CSV file");
      sw.WriteLine("    /// </summary>");
      sw.Write("    internal static readonly string[] Headers = {");
      lines.Clear();
      if (AreInstancesReleasable || AreInstancesUpdatable) {
        lines.Add("\"Key\"");
      }
      foreach (var mi in Members.Values) {
        if (mi.MemberType<MemberTypeEnum.ParentOneChild && //not ParentMultipleChildrenXxx
          mi.MemberType!=MemberTypeEnum.ToLower) 
        {
          lines.Add($"\"{mi.MemberName}\"");
        }
      }
      for (int lineIndex = 0; lineIndex < lines.Count; lineIndex++) {
        var line = lines[lineIndex];
        if (lines.Count>5) {
          sw.WriteLine();
          sw.Write("      ");
        }
        sw.Write(line);
        if (lineIndex+1<lines.Count) {
          sw.Write(", ");
        }
      }
      if (lines.Count>5) {
        sw.WriteLine();
        sw.WriteLine("    };");
      } else {
        sw.WriteLine("};");
      }
      sw.WriteLine();
      sw.WriteLine();
    }


    private void writeNoClassSingleton(StreamWriter sw) {
      sw.WriteLine($"    /// <summary>");
      sw.WriteLine($"    /// None existing {ClassName}, used as a temporary place holder when reading a CSV file");
      sw.WriteLine($"    /// which was not compacted. It might create first a later deleted item linking to a ");
      sw.WriteLine($"    /// deleted parent. In this case, the parent property gets set to No{ClassName}. Once the CSV");
      sw.WriteLine($"    /// file is completely read, that child will actually be deleted (released) and Verify()");
      sw.WriteLine($"    /// ensures that there are no stored children with links to No{ClassName}.");
      sw.WriteLine($"    /// </summary>");
      sw.Write($"    internal static {ClassName} No{ClassName} = new {ClassName}(");
      foreach (var mi in Members.Values) {
        if (mi.MemberType==MemberTypeEnum.Enum) {
          if (mi.IsNullable) {
            sw.Write("null, ");
          } else {
            sw.Write(0 + ", ");
          }
        } else if (mi.MemberType<MemberTypeEnum.ToLower || mi.MemberType==MemberTypeEnum.LinkToParent) {
          //simple data type, enum, link to parent
          sw.Write(mi.NoValue + ", ");
        }
      }
      sw.WriteLine("isStoring: false);");
    }
    #endregion


    #region Events Code
    //      -----------

    private void writeEvents(StreamWriter sw, string context) {
      sw.WriteLine("    #region Events");
      sw.WriteLine("    //      ------");
      sw.WriteLine();
      sw.WriteLine("    /// <summary>");
      if (AreInstancesUpdatable) {
        sw.WriteLine($"    /// Content of {ClassName} has changed. Gets only raised for changes occurring after loading {context}.Data with previously stored data.");
      } else {
        sw.WriteLine($"    /// This event will never be raised, but is needed to comply with IStorage.");
      }
      sw.WriteLine("    /// </summary>");
      if (!AreInstancesUpdatable) {
        sw.WriteLine("#pragma warning disable 67");
      }
      sw.WriteLine($"    public event Action</*old*/{ClassName}, /*new*/{ClassName}>? HasChanged;");
      if (!AreInstancesUpdatable) {
        sw.WriteLine("#pragma warning restore 67");
      }
      sw.WriteLine("    #endregion");
      sw.WriteLine();
      sw.WriteLine();
    }
    #endregion


    #region Constructor Code
    //      ----------------

    private void writeConstructor(StreamWriter sw, string context, List<string> lines, TracingEnum isTracing) {
      sw.WriteLine("    #region Constructors");
      sw.WriteLine("    //      ------------");
      sw.WriteLine();
      writePublicConstructor(sw, context, lines, isTracing);
      writeCloningConstructor(sw);
      writeCsvConstructor(sw, context);
      writeVerify(sw, lines);
      sw.WriteLine("    #endregion");
      sw.WriteLine();
      sw.WriteLine();
    }


    private void writePublicConstructor(StreamWriter sw, string context, List<string> lines, TracingEnum isTracing) {
      sw.WriteLine("    /// <summary>");
      sw.WriteLine($"    /// {ClassName} Constructor. If isStoring is true, adds {ClassName} to {context}.Data.{PluralName}.");
      sw.WriteLine("    /// </summary>");

      if (IsConstructorPrivate) {
        sw.Write($"    private {ClassName}(");
      } else {
        sw.Write($"    public {ClassName}(");
      }
      writeParameters(sw, lines, isConstructor: true);
      sw.WriteLine("      Key = StorageExtensions.NoKey;");

      foreach (var mi in Members.Values) {
        if (mi.MemberType==MemberTypeEnum.ToLower || mi.MemberType==MemberTypeEnum.ParentOneChild) continue;

        if (mi.MemberType==MemberTypeEnum.ParentMultipleChildrenList) {
          if (mi.ChildClassInfo!.AreInstancesReleasable) {
            sw.WriteLine($"      {mi.LowerMemberName} = new StorageList<{mi.ChildTypeName}>();");
          } else {
            sw.WriteLine($"      {mi.LowerMemberName} = new List<{mi.ChildTypeName}>();");
          }
        } else if (mi.MemberType is
          MemberTypeEnum.ParentMultipleChildrenHashSet or
          MemberTypeEnum.ParentMultipleChildrenDictionary or
          MemberTypeEnum.ParentMultipleChildrenSortedList)
        {
          sw.WriteLine($"      {mi.LowerMemberName} = new {mi.TypeString}();");
        } else if (mi.MemberType==MemberTypeEnum.ParentMultipleChildrenSortedBucket) {
          sw.WriteLine($"      {mi.LowerMemberName} = new {mi.TypeString}(item => item.{mi.ChildKeyPropertyName}, item => " +
            $"item.{mi.ChildKey2PropertyName});");
        } else {
          //LinkToParent, simple type or enum
          sw.WriteLine($"      {mi.MemberName} = {mi.LowerMemberName}{mi.Rounding};");
          writeToLowerCopyStatement(sw, mi, needsDictEnum.Contructor); //toLower must be executed here once the source
                                                                       //property has its value and not when
                                                                       //mi.MemberType==MemberTypeEnum.ToLower
        }
      }
      Compiler.WriteLinesTracing(sw, isTracing,
                   $"      {context}.Trace?.Invoke($\"new {ClassName}: {{ToTraceString()}}\");");
      foreach (var mi in Members.Values) {
        if (mi.MemberType==MemberTypeEnum.LinkToParent && !mi.IsLookupOnly) {
          if (mi.IsNullable) {
            sw.WriteLine($"      if ({mi.MemberName}!=null) {{");
            sw.WriteLine($"        {mi.MemberName}.AddTo{mi.ParentMemberInfo!.MemberName}(this);");
            sw.WriteLine("      }");
          } else {
            sw.WriteLine($"      {mi.MemberName}.AddTo{mi.ParentMemberInfo!.MemberName}(this);");
          }
        }
      }
      sw.WriteLine("      onConstruct();");
      sw.WriteLine("      if (DC.Data?.IsTransaction??false) {");
      sw.WriteLine($"        DC.Data.AddTransaction(new TransactionItem({StoreKey},"+
                             "TransactionActivityEnum.New, Key, this));");
      sw.WriteLine("      }");
      sw.WriteLine();
      sw.WriteLine("      if (isStoring) {");
      sw.WriteLine("        Store();");
      sw.WriteLine("      }");
      sw.WriteLine("    }");
      sw.WriteLine("    partial void onConstruct();");
      sw.WriteLine();
      sw.WriteLine();
    }


    private void writeCloningConstructor(StreamWriter sw) {
      sw.WriteLine("    /// <summary>");
      sw.WriteLine("    /// Cloning constructor. It will copy all data from original except any collection (children).");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine("    #pragma warning disable CS8618 // Children collections are uninitialized.");
      sw.WriteLine($"    public {ClassName}({ClassName} original) {{");
      sw.WriteLine("    #pragma warning restore CS8618 //");
      sw.WriteLine("      Key = StorageExtensions.NoKey;");
      foreach (var mi in Members.Values) {
        if (mi.MemberType>MemberTypeEnum.LinkToParent) continue;

        //linkToParent, enum, ToLower or simple type
        sw.WriteLine($"      {mi.MemberName} = original.{mi.MemberName};");
      }
      sw.WriteLine("      onCloned(this);");
      sw.WriteLine("    }");
      sw.WriteLine($"    partial void onCloned({ClassName} clone);");
      sw.WriteLine();
      sw.WriteLine();
    }


    private void writeCsvConstructor(StreamWriter sw, string context) {
      sw.WriteLine("    /// <summary>");
      sw.WriteLine($"    /// Constructor for {ClassName} read from CSV file");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine($"    private {ClassName}(int key, CsvReader csvReader, DataStoreCSV<{ClassName}> dataStore){{");
      sw.WriteLine("      Key = key;");
      //var isVarNeeded = true;
      //var isVarNullableNeeded = true;
      foreach (var mi in Members.Values) {
        if (mi.MemberType==MemberTypeEnum.ParentOneChild || mi.MemberType==MemberTypeEnum.ToLower) continue;

        if (mi.MemberType==MemberTypeEnum.ParentMultipleChildrenList) {
          if (mi.ChildClassInfo!.AreInstancesReleasable) {
            sw.WriteLine($"      {mi.LowerMemberName} = new StorageList<{mi.ChildTypeName}>();");
          } else {
            sw.WriteLine($"      {mi.LowerMemberName} = new List<{mi.ChildTypeName}>();");
          }
        } else if (mi.MemberType is
          MemberTypeEnum.ParentMultipleChildrenHashSet or
          MemberTypeEnum.ParentMultipleChildrenDictionary or
          MemberTypeEnum.ParentMultipleChildrenSortedList) 
        {
          sw.WriteLine($"      {mi.LowerMemberName} = new {mi.TypeString}();");
        } else if (mi.MemberType==MemberTypeEnum.ParentMultipleChildrenSortedBucket) {
          sw.WriteLine($"      {mi.LowerMemberName} = new {mi.TypeString}(item => item.{mi.ChildKeyPropertyName}, item => " +
            $"item.{mi.ChildKey2PropertyName});");
        } else if (mi.MemberType==MemberTypeEnum.LinkToParent) {
          if (mi.IsNullable) {
            sw.WriteLine($"      var {mi.LowerMemberName}Key = csvReader.ReadIntNull();");
            sw.WriteLine($"      if ({mi.LowerMemberName}Key.HasValue) {{");
            if (mi.IsSelfReferencing) {
              sw.WriteLine($"        {mi.MemberName} = dataStore.GetItem({mi.LowerMemberName}Key.Value)?? {mi.TypeStringNotNullable}.No{mi.TypeStringNotNullable};");
            } else {
              sw.WriteLine($"        {mi.MemberName} = {context}.Data._{mi.ParentClassInfo!.PluralName}.GetItem({mi.LowerMemberName}Key.Value)?? {mi.TypeStringNotNullable}.No{mi.TypeStringNotNullable};");
            }
            sw.WriteLine("      }");
          } else {
            sw.WriteLine($"      var {mi.LowerMemberName}Key = csvReader.ReadInt();");
            if (mi.IsSelfReferencing) {
              sw.WriteLine($"      {mi.MemberName} = dataStore.GetItem({mi.LowerMemberName}Key)?? {mi.TypeStringNotNullable}.No{mi.TypeStringNotNullable};");
            } else {
              sw.WriteLine($"      {mi.MemberName} = {context}.Data._{mi.ParentClassInfo!.PluralName}.GetItem({mi.LowerMemberName}Key)?? {mi.TypeStringNotNullable}.No{mi.TypeStringNotNullable};");
            }
          }
        } else {
          //enum, simple data type
          if (mi.MemberType==MemberTypeEnum.Enum) {
            //sw.WriteLine($"      {mi.MemberName} = ({mi.EnumInfo!.Name})csvReader.{mi.CsvReaderRead};");
            sw.WriteLine($"      {mi.MemberName} = ({mi.TypeString})csvReader.{mi.CsvReaderRead};");
          } else {
            sw.WriteLine($"      {mi.MemberName} = csvReader.{mi.CsvReaderRead};");
            writeToLowerCopyStatement(sw, mi, needsDictEnum.Contructor);
          }
          writeNeedsDictionaryAddStatement(sw, mi, context, needsDictEnum.Contructor);
        }
      }
      //if the parent is a Dictionary or SortedList, the key property must be assigned first (i.e. in the for loop 
      //above), before the child gets added to the parent
      foreach (var mi in Members.Values) {
        if (mi.MemberType==MemberTypeEnum.LinkToParent && !mi.IsLookupOnly) {
          if (mi.IsNullable) {
            sw.WriteLine($"      if ({mi.LowerMemberName}Key.HasValue && {mi.MemberName}!={mi.TypeStringNotNullable}.No{mi.TypeStringNotNullable}) {{");
            sw.WriteLine($"        {mi.MemberName}!.AddTo{mi.ParentMemberInfo!.MemberName}(this);");
            sw.WriteLine("      }");
          } else {
            sw.WriteLine($"      if ({mi.MemberName}!={mi.TypeStringNotNullable}.No{mi.TypeStringNotNullable}) {{");
            sw.WriteLine($"        {mi.MemberName}.AddTo{mi.ParentMemberInfo!.MemberName}(this);");
            sw.WriteLine("      }");
          }
        }
      }
      sw.WriteLine("      onCsvConstruct();");
      sw.WriteLine("    }");
      sw.WriteLine($"    partial void onCsvConstruct();");
      sw.WriteLine();
      sw.WriteLine();
      sw.WriteLine("    /// <summary>");
      sw.WriteLine($"    /// New {ClassName} read from CSV file");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine($"    internal static {ClassName} Create(int key, CsvReader csvReader, DataStoreCSV<{ClassName}> dataStore) {{");
      sw.WriteLine($"      return new {ClassName}(key, csvReader, dataStore);");
      sw.WriteLine("    }");
    }


    private void writeVerify(StreamWriter sw, List<string> lines) {
      var commentLines = new List<string>();
      lines.Clear();
      foreach (var mi in Members.Values) {
        if (mi.MemberType==MemberTypeEnum.LinkToParent) {
          commentLines.Add($"    /// Verify that {LowerClassName}.{mi.MemberName} exists.");
          lines.Add($"      if ({LowerClassName}.{mi.MemberName}=={mi.TypeStringNotNullable}.No{mi.TypeStringNotNullable}) return false;");
        }
      }
      if (commentLines.Count>0) {
        sw.WriteLine();
        sw.WriteLine();
        sw.WriteLine("    /// <summary>");
        foreach (var commentLine in commentLines) {
          sw.WriteLine(commentLine);
        }
        sw.WriteLine("    /// </summary>");
        sw.WriteLine($"    internal static bool Verify({ClassName} {LowerClassName}) {{");
        foreach (var codeLine in lines) {
          sw.WriteLine(codeLine);
        }
        sw.WriteLine($"      return true;");
        sw.WriteLine("    }");

      }
    }
    #endregion


    #region Methods Code
    //      ------------

    private void writeMethods(StreamWriter sw, string context, List<string> lines, TracingEnum isTracing) {
      sw.WriteLine("    #region Methods");
      sw.WriteLine("    //      -------");
      sw.WriteLine();
      writeStore(sw, context, isTracing);
      writeWriteCsv(sw, context);

      if (AreInstancesUpdatable) {
        writeUpdate(sw, context, lines, isTracing);
        writeUpdateCsv(sw, context);
      }

      writeAddToRemoveFrom(sw, context, isTracing);
      if (AreInstancesReleasable) {
        writeRelease(sw, context, isTracing);
        writeDisconnect(sw, context, isTracing);
      } else {
        sw.WriteLine("    /// <summary>");
        sw.WriteLine($"    /// Releasing {ClassName} from {context}.Data.{PluralName} is not supported.");
        sw.WriteLine("    /// </summary>");
        sw.WriteLine("    public void Release() {");
        sw.WriteLine("      throw new NotSupportedException(\"Release() is not supported, StorageClass attribute AreInstancesReleasable is false.\");");
        sw.WriteLine("    }");
        sw.WriteLine();
        sw.WriteLine();
      }

      writeTransactionSupport(sw, context, isTracing);
      writeToString(sw, lines);
      sw.WriteLine("    #endregion");
      sw.WriteLine("  }");

      if (IsGenerateReaderWriter) {
        writeRawClass(sw, context, lines);
        writeReaderClass(sw);
        writeWriterClass(sw, context, lines);
      }
      sw.WriteLine("}");
    }


    private void writeStore(StreamWriter sw, string context, TracingEnum isTracing) {
      //sw.WriteLine("    bool isStoreExecuting;");
      //sw.WriteLine();
      //sw.WriteLine();
      sw.WriteLine("    /// <summary>");
      sw.WriteLine($"    /// Adds {ClassName} to {context}.Data.{PluralName}.<br/>");
      sw.WriteLine($"    /// Throws an Exception when {ClassName} is already stored.<br/>");
      sw.WriteLine($"    /// Returns true unless onStoring() cancels storing.");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine("    public bool Store() {");
      //sw.WriteLine("      if (isStoreExecuting) return true; // Store() called parent.Store() which calls Store() again");
      //sw.WriteLine();
      sw.WriteLine("      if (Key>=0) {");
      sw.WriteLine($"        throw new Exception($\"{ClassName} cannot be stored again in {context}.Data, " +
        $"key {{Key}} is greater equal 0.\" + Environment.NewLine + ToString());");
      sw.WriteLine("      }");
      sw.WriteLine();
      //sw.WriteLine("      try {");
      //sw.WriteLine("        isStoreExecuting = true;");
      sw.WriteLine("      var isCancelled = false;");
      sw.WriteLine("      onStoring(ref isCancelled);");
      sw.WriteLine("      if (isCancelled) return false;");
      sw.WriteLine();
      //Compiler.WriteLinesTracing(sw, isTracing,
      //             $"      {context}.Trace?.Invoke($\"storing {ClassName} #{{GetHashCode()}}\");");
      foreach (var mi in Members.Values) {
        if (mi.MemberType==MemberTypeEnum.LinkToParent) {
          sw.WriteLine($"      if ({mi.MemberName}{mi.QMark}.Key<0) {{");
          sw.WriteLine($"        throw new Exception($\"Cannot store child {ClassName} '{{this}}'.{mi.MemberName} to " +
            $"{mi.ParentClassInfo!.ClassName} '{{{mi.MemberName}}}' because parent is not stored yet.\");");
          //store all parents if not done yet before storing Add(this).
          //sw.WriteLine($"          {mi.MemberName}.Store(isStoreAll);");
          sw.WriteLine("      }");
        }
      }

      //add first the item to dictinaries, if needed and only when this is successful, write it to the dataStore. The
      //chance of an exception is higher with the dictionary, because the same entry might exist already. If the item
      //is added to the dictionary after storing, that item causes a Dictionary exception again after restart.
      foreach (var mi in Members.Values) {
        if (mi.MemberType<=MemberTypeEnum.String || mi.MemberType==MemberTypeEnum.Enum) {
          //enum or simple data type
          //toLower should already be copied here from source property by constructor or update
          writeNeedsDictionaryAddStatement(sw, mi, context, needsDictEnum.Store);
        }
      }
      sw.WriteLine($"      {context}.Data._{PluralName}.Add(this);");

      sw.WriteLine("      onStored();");
      Compiler.WriteLinesTracing(sw, isTracing,
                   $"      {context}.Trace?.Invoke($\"Stored {ClassName} #{{GetHashCode()}} @{{Key}}\");");
      //sw.WriteLine("      } finally {");
      //sw.WriteLine("        isStoreExecuting = false;");
      //sw.WriteLine("      }");
      sw.WriteLine("      return true;");
      sw.WriteLine("    }");
      sw.WriteLine("    partial void onStoring(ref bool isCancelled);");
      sw.WriteLine("    partial void onStored();");
      sw.WriteLine();
      sw.WriteLine();
      sw.WriteLine("    /// <summary>");
      sw.WriteLine($"    /// Estimated number of UTF8 characters needed to write {ClassName} to CSV file");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine($"    public const int EstimatedLineLength = {EstimatedMaxByteSize};");
      sw.WriteLine();
      sw.WriteLine();
    }


    private void writeWriteCsv(StreamWriter sw, string context) {
      sw.WriteLine("    /// <summary>");
      sw.WriteLine($"    /// Write {ClassName} to CSV file");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine($"    internal static void Write({ClassName} {LowerClassName}, CsvWriter csvWriter) {{");
      sw.WriteLine($"      {LowerClassName}.onCsvWrite();");
      foreach (var mi in Members.Values) {
        if (mi.MemberType==MemberTypeEnum.ToLower) continue;

        if (mi.MemberType==MemberTypeEnum.LinkToParent) {
          if (mi.IsNullable) {
            sw.WriteLine($"      if ({LowerClassName}.{mi.MemberName} is null) {{");
            sw.WriteLine("        csvWriter.WriteNull();");
            sw.WriteLine("      } else {");
            sw.WriteLine($"        if ({LowerClassName}.{mi.MemberName}.Key<0) throw new Exception($\"Cannot write" +
              $" {LowerClassName} '{{{LowerClassName}}}' to CSV File, because {mi.MemberName} is not stored in {context}.Data.{mi.ParentClassInfo!.PluralName}.\");");
            sw.WriteLine();
            sw.WriteLine($"        csvWriter.Write({LowerClassName}.{mi.MemberName}.Key.ToString());");
            sw.WriteLine("      }");
          } else {
            sw.WriteLine($"      if ({LowerClassName}.{mi.MemberName}.Key<0) throw new Exception($\"Cannot write {LowerClassName} '{{{LowerClassName}}}'" +
              $" to CSV File, because {mi.MemberName} is not stored in {context}.Data.{mi.ParentClassInfo!.PluralName}.\");");
            sw.WriteLine();
            sw.WriteLine($"      csvWriter.Write({LowerClassName}.{mi.MemberName}.Key.ToString());");
          }
        } else if (mi.MemberType==MemberTypeEnum.Enum) {
          sw.WriteLine($"      csvWriter.{mi.CsvWriterWrite}((int{mi.QMark}){LowerClassName}.{mi.MemberName});");
        } else if (mi.MemberType<MemberTypeEnum.ToLower) {
          //simple data type
          sw.WriteLine($"      csvWriter.{mi.CsvWriterWrite}({LowerClassName}.{mi.MemberName});");
        }
      }
      sw.WriteLine("    }");
      sw.WriteLine("    partial void onCsvWrite();");
      sw.WriteLine();
      sw.WriteLine();
    }


    private void writeUpdate(StreamWriter sw, string context, List<string> lines, TracingEnum isTracing) {
      sw.WriteLine("    /// <summary>");
      sw.WriteLine($"    /// Updates {ClassName} with the provided values.<br/>");
      sw.WriteLine($"    /// Returns true unless onUpdating() cancels updating.");
      sw.WriteLine("    /// </summary>");
      sw.Write("    public bool Update(");
      if (!writeParameters(sw, lines, isConstructor: false)) {
        throw new GeneratorException($"Method '{ClassName}.Update()': has no parameters. Are all properties readonly ?" +
          " Then add attribute [StorageClass(areInstancesUpdatable: false)] and remove readonly from the properties.");
      }

      //generated code needs to throw an exception when stored child would link after update to a not stored parent
      lines.Clear();
      foreach (var mi in Members.Values) {
        if (mi.MemberType==MemberTypeEnum.LinkToParent && !mi.IsReadOnly) {
          lines.Add($"        if ({mi.LowerMemberName}{mi.QMark}.Key<0) {{");
          lines.Add($"          throw new Exception($\"{ClassName}.Update(): It is illegal to add stored {ClassName} '{{this}}'\" " +
            "+ Environment.NewLine + ");
          lines.Add($"            $\"to {mi.MemberName} '{{{mi.LowerMemberName}}}', which is not stored.\");");
          lines.Add($"        }}");
        }
      }
      if (lines.Count>0) {
        sw.WriteLine("      if (Key>=0){");
        foreach (var line in lines) {
          sw.WriteLine(line);
        }
        sw.WriteLine("      }");
      }
      sw.WriteLine($"      var clone = new {ClassName}(this);");

      //call onUpdating()
      sw.WriteLine("      var isCancelled = false;");
      sw.Write("      onUpdating(");
      if (!writeOnUpdateParameters(sw, updateTypeEnum.Call)) {
        throw new GeneratorException($"Method '{ClassName}.onUpdating()': has no parameters. Are all properties readonly ?" +
          " Then add attribute [StorageClass(areInstancesUpdatable: false)] and remove readonly from the properties.");
      }
      sw.WriteLine("      if (isCancelled) return false;");
      sw.WriteLine();

      Compiler.WriteLinesTracing(sw, isTracing,
                   $"      {context}.Trace?.Invoke($\"Updating {ClassName}: {{ToTraceString()}}\");");

      var isFirst = true;
      if (HasNotReadOnlyAndNotLookupParents) {
        foreach (var mi in Members.Values) {
          if (mi.IsReadOnly || 
            mi.MemberType!=MemberTypeEnum.LinkToParent || 
            mi.IsLookupOnly ||
            (mi.ParentMemberInfo?.MemberType==MemberTypeEnum.ParentMultipleChildrenHashSet)) continue; 
            //child needs to perform updating property linking to parent's HashSet and removing child from HashSet at
            //the same time.

          if (isFirst) {
            isFirst = false;
            sw.WriteLine();
            sw.WriteLine($"      //remove item with old values from parents which will be overewritten by update");
          }

          //collections with keys: the child needs to be removed from the parent's collection before the key get updated.
          if (mi.ParentMemberInfo!.MemberType==MemberTypeEnum.ParentMultipleChildrenDictionary ||
            mi.ParentMemberInfo.MemberType==MemberTypeEnum.ParentMultipleChildrenSortedList) 
          {
            sw.WriteLine($"      var has{mi.MemberName}Changed = {mi.MemberName}!={mi.LowerMemberName} || " +
              $"{mi.ParentMemberInfo.ChildKeyPropertyName}!={mi.ParentMemberInfo.LowerChildKeyPropertyName};");
          } else if(mi.ParentMemberInfo.MemberType==MemberTypeEnum.ParentMultipleChildrenSortedBucket) {
            if (mi.ParentMemberInfo.ChildKey2PropertyName=="Key") {
              //Key property will never be changed in Update(), only 2 compparisions needed
              sw.WriteLine($"      var has{mi.MemberName}Changed = {mi.MemberName}!={mi.LowerMemberName} || " +
                $"{mi.ParentMemberInfo.ChildKeyPropertyName}!={mi.ParentMemberInfo.LowerChildKeyPropertyName};");
            } else {
              sw.WriteLine($"      var has{mi.MemberName}Changed = {mi.MemberName}!={mi.LowerMemberName} || " +
                $"{mi.ParentMemberInfo.ChildKeyPropertyName}!={mi.ParentMemberInfo.LowerChildKeyPropertyName} || " +
                $"{mi.ParentMemberInfo.ChildKey2PropertyName}!={mi.ParentMemberInfo.LowerChildKey2PropertyName};");
            }
          } else {
            //parent has single child or list of children and child class has only one property with parent's type.
            sw.WriteLine($"      var has{mi.MemberName}Changed = {mi.MemberName}!={mi.LowerMemberName};");
          }
          if (mi.IsNullable) {
            sw.WriteLine($"      if ({mi.MemberName} is not null && has{mi.MemberName}Changed) {{");
          } else {
            sw.WriteLine($"      if (has{mi.MemberName}Changed) {{");
          }
          sw.WriteLine($"        {mi.MemberName}.RemoveFrom{mi.ParentMemberInfo!.MemberName}(this);");
          sw.WriteLine($"      }}");
        }
      }

      //update the item properties before updating the collections in the item's parents
      sw.WriteLine();
      sw.WriteLine("      //update properties and detect if any value has changed");
      sw.WriteLine("      var isChangeDetected = false;");
      foreach (var mi in Members.Values) {
        if (mi.IsReadOnly || 
          mi.MemberType>MemberTypeEnum.LinkToParent || 
          mi.MemberType==MemberTypeEnum.ToLower) //ToLower properties get their value assigned to when their source
                                                 //property changes its value
          continue;

        if (mi.Rounding!=null) {
          sw.WriteLine($"      {mi.LowerMemberName} = {mi.LowerMemberName}{mi.Rounding};");
        }
        if (mi.ParentMemberInfo?.MemberType==MemberTypeEnum.ParentMultipleChildrenHashSet) {
          //parent uses a HashSet instead a List for children, because more than one property in the child has
          //the parent's type. The child should get added to parent's Children only once, regardless how many
          //of the child's properties link to that parent. When removing a child parent link during updating such
          //a child property, the parent will only remove the child if no other child property links to it. If 2
          //child property each remove a particulat parent, the first property must be properly updated before
          //the second property's update get processed. The second update can only remove the child from 
          //parent.Children if the first property points already to a different parent or is null.
          sw.WriteLine($"      var has{mi.MemberName}Changed = {mi.MemberName}!={mi.LowerMemberName};");
          sw.WriteLine($"      if (has{mi.MemberName}Changed){{");
          if (mi.IsNullable) {
            sw.WriteLine($"        if ({mi.MemberName} is not null) {{");
            sw.WriteLine($"          {mi.MemberName}.RemoveFrom{mi.ParentMemberInfo!.MemberName}(this);");
            sw.WriteLine($"        }}");
          } else {
            sw.WriteLine($"          {mi.MemberName}.RemoveFrom{mi.ParentMemberInfo!.MemberName}(this);");
          }
          sw.WriteLine($"        {mi.MemberName} = {mi.LowerMemberName};");
          sw.WriteLine($"        isChangeDetected = true;");
          sw.WriteLine($"      }}");

        } else {
          //all property types except links to parents using a HashSet for children
          sw.WriteLine($"      if ({mi.MemberName}!={mi.LowerMemberName}) {{");
          writeNeedsDictionaryRemoveStatement(sw, mi, context, needsDictEnum.Update);
          sw.WriteLine($"        {mi.MemberName} = {mi.LowerMemberName};");
          writeToLowerCopyStatement(sw, mi, needsDictEnum.Update);
          writeNeedsDictionaryAddStatement(sw, mi, context, needsDictEnum.Update);
          sw.WriteLine("        isChangeDetected = true;");
          sw.WriteLine("      }");
        }
      }

      if (HasNotReadOnlyAndNotLookupParents) {
        sw.WriteLine();
        sw.WriteLine($"      //add updated item to parents which have been newly added during update");
        foreach (var mi in Members.Values) {
          if (mi.IsReadOnly || mi.MemberType!=MemberTypeEnum.LinkToParent || mi.IsLookupOnly) continue;

          if (mi.IsNullable) {
            sw.WriteLine($"      if ({mi.MemberName} is not null && has{mi.MemberName}Changed) {{");
          } else {
            sw.WriteLine($"      if (has{mi.MemberName}Changed) {{");
          }
          sw.WriteLine($"        {mi.MemberName}.AddTo{mi.ParentMemberInfo!.MemberName}(this);");
          sw.WriteLine($"      }}");
        }
      }

      //call updated()
      sw.WriteLine("      if (isChangeDetected) {");
      sw.WriteLine("        onUpdated(clone);");
      sw.WriteLine("        if (Key>=0) {");
      sw.WriteLine($"          {context}.Data._{PluralName}.ItemHasChanged(clone, this);");
      sw.WriteLine("        } else if (DC.Data.IsTransaction) {");
      sw.WriteLine($"          DC.Data.AddTransaction(new TransactionItem({StoreKey}, " +
                                 "TransactionActivityEnum.Update, Key, this, oldItem: clone));");
      sw.WriteLine("        }");
      sw.WriteLine("        HasChanged?.Invoke(clone, this);");
      sw.WriteLine("      }");
      Compiler.WriteLinesTracing(sw, isTracing,
                   $"      {context}.Trace?.Invoke($\"Updated {ClassName}: {{ToTraceString()}}\");");
      sw.WriteLine("    return true;");
      sw.WriteLine("    }");
      sw.Write("    partial void onUpdating(");
      if (!writeOnUpdateParameters(sw, updateTypeEnum.Definition)) {
        throw new GeneratorException($"Method '{ClassName}.onUpdating()': has no parameters. Are all properties readonly ?" +
          " Then add attribute [StorageClass(areInstancesUpdatable: false] and remove readonly from the properties.");
      }
      sw.WriteLine($"    partial void onUpdated({ClassName} old);");
      sw.WriteLine();
      sw.WriteLine();
    }


    private void writeUpdateCsv(StreamWriter sw, string context) {
      sw.WriteLine("    /// <summary>");
      sw.WriteLine($"    /// Updates this {ClassName} with values from CSV file");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine($"    internal static void Update({ClassName} {LowerClassName}, CsvReader csvReader, DataStoreCSV<{ClassName}> dataStore){{");

      sw.WriteLine("      //read first all property values into local variables");
      var isFirst = true;
      foreach (var mi in Members.Values) {
        if (mi.MemberType==MemberTypeEnum.ToLower) continue;

        if (isFirst) {
          isFirst = false;
        } else { 
          sw.WriteLine();
        }
        if (mi.IsReadOnly) {
          //LinkToParent, enum or simple type
          //the parents collections or a parent linking to one child cannot be readonly
          //ensure that the values stored in the CSV file are the same as the instance itself
          if (mi.MemberType==MemberTypeEnum.Enum) {
            sw.WriteLine($"      var {mi.LowerMemberName} = ({mi.TypeString})csvReader.{mi.CsvReaderRead};");

          } else if (mi.MemberType==MemberTypeEnum.LinkToParent) {
            if (mi.IsNullable) {
              sw.WriteLine($"      var {mi.LowerMemberName}Key = csvReader.ReadIntNull();");
              sw.WriteLine($"      {mi.TypeStringNotNullable}? {mi.LowerMemberName};");
              sw.WriteLine($"      if ({mi.LowerMemberName}Key is null) {{");
              sw.WriteLine($"        {mi.LowerMemberName} = null;");
              sw.WriteLine("      } else {");
              if (mi.IsSelfReferencing) {
                sw.WriteLine($"        {mi.LowerMemberName} = dataStore.GetItem({mi.LowerMemberName}Key.Value)??{mi.TypeStringNotNullable}.No{mi.TypeStringNotNullable};");
              } else {
                sw.WriteLine($"        {mi.LowerMemberName} = {context}.Data._{mi.ParentClassInfo!.PluralName}.GetItem({mi.LowerMemberName}Key.Value)??{mi.TypeStringNotNullable}.No{mi.TypeStringNotNullable};");
              }
              sw.WriteLine("      }");
 
            } else {
              if (mi.IsSelfReferencing) {
                sw.WriteLine($"      var {mi.LowerMemberName} = dataStore.GetItem(csvReader.ReadInt())??{mi.TypeStringNotNullable}.No{mi.TypeStringNotNullable};");
              } else {
                sw.WriteLine($"      var {mi.LowerMemberName} = {context}.Data._{mi.ParentClassInfo!.PluralName}.GetItem(csvReader.ReadInt())??{mi.TypeStringNotNullable}.No{mi.TypeStringNotNullable};");
              }
            }

          } else {
            sw.WriteLine($"      var {mi.LowerMemberName} = csvReader.{mi.CsvReaderRead};");
          }
          sw.WriteLine($"      if ({LowerClassName}.{mi.MemberName}!={mi.LowerMemberName}) {{");
          sw.WriteLine($"        throw new Exception($\"{ClassName}.Update(): Property {mi.MemberName}" +
            $" '{{{LowerClassName}.{mi.MemberName}}}' is \" +");
          sw.WriteLine($"          $\"readonly, {mi.LowerMemberName} '{{{mi.LowerMemberName}}}' read from the CSV file should be the" +
            " same.\" + Environment.NewLine + ");
          sw.WriteLine($"          {LowerClassName}.ToString());");
          sw.WriteLine("      }");

        } else {
          //not readonly
          if (mi.MemberType==MemberTypeEnum.LinkToParent) {
            if (mi.IsNullable) {
              sw.WriteLine($"      var {mi.LowerMemberName}Key = csvReader.ReadIntNull();");
              sw.WriteLine($"      {mi.TypeStringNotNullable}? {mi.LowerMemberName};");
              sw.WriteLine($"      if ({mi.LowerMemberName}Key is null) {{");
              sw.WriteLine($"        {mi.LowerMemberName} = null;");
              sw.WriteLine( "      } else {");
              if (mi.IsSelfReferencing) {
                sw.WriteLine($"        {mi.LowerMemberName} = dataStore.GetItem({mi.LowerMemberName}Key.Value)??{mi.TypeStringNotNullable}.No{mi.TypeStringNotNullable};");
              } else {
                sw.WriteLine($"        {mi.LowerMemberName} = {context}.Data._{mi.ParentClassInfo!.PluralName}.GetItem({mi.LowerMemberName}Key.Value)??{mi.TypeStringNotNullable}.No{mi.TypeStringNotNullable};");
              }
              sw.WriteLine( "      }");
            } else {
              if (mi.IsSelfReferencing) {
                sw.WriteLine($"      var {mi.LowerMemberName} = dataStore.GetItem(csvReader.ReadInt())??{mi.TypeStringNotNullable}.No{mi.TypeStringNotNullable};");
              } else {
                sw.WriteLine($"      var {mi.LowerMemberName} = {context}.Data._{mi.ParentClassInfo!.PluralName}.GetItem(csvReader.ReadInt())??{mi.TypeStringNotNullable}.No{mi.TypeStringNotNullable};");
              }
            }

          } else if (mi.MemberType<MemberTypeEnum.LinkToParent) {
            //enum, simple type
            if (mi.MemberType==MemberTypeEnum.Enum) {
              sw.WriteLine($"      var {mi.LowerMemberName} = ({mi.TypeString})csvReader.{mi.CsvReaderRead};");
            } else {
              sw.WriteLine($"      var {mi.LowerMemberName} = csvReader.{mi.CsvReaderRead};");
            }
          }
        }
      }

      //remove class from parent if link to parent or the child's key into the parent's children collection has changed
      if (HasNotReadOnlyAndNotLookupParents) {
        isFirst = true;
        foreach (var mi in Members.Values) {
          if (
            mi.IsReadOnly || 
            mi.MemberType!=MemberTypeEnum.LinkToParent || 
            mi.IsLookupOnly||
            (mi.ParentMemberInfo?.MemberType==MemberTypeEnum.ParentMultipleChildrenHashSet)) continue; 
            //child needs to perform updating property linking to parent's HashSet and removing child from HashSet
            //at the same time.

          if (isFirst) {
            isFirst = false;
            sw.WriteLine();
            sw.WriteLine($"      //remove item with old values from parents which will be overewritten by update");
          }

          if (mi.ParentMemberInfo!.MemberType==MemberTypeEnum.ParentMultipleChildrenDictionary ||
            mi.ParentMemberInfo.MemberType==MemberTypeEnum.ParentMultipleChildrenSortedList) 
          {
            sw.WriteLine($"      var has{mi.MemberName}Changed = {LowerClassName}.{mi.MemberName}!={mi.LowerMemberName} || " +
              $"{LowerClassName}.{mi.ParentMemberInfo.ChildKeyPropertyName}!={mi.ParentMemberInfo.LowerChildKeyPropertyName};");
          } else if(mi.ParentMemberInfo!.MemberType==MemberTypeEnum.ParentMultipleChildrenSortedBucket) {
            if (mi.ParentMemberInfo.ChildKey2PropertyName=="Key") {
              //Key property will never be changed in Update(), only 2 compparisions needed
              sw.WriteLine($"      var has{mi.MemberName}Changed = {LowerClassName}.{mi.MemberName}!={mi.LowerMemberName} || " +
                $"{LowerClassName}.{mi.ParentMemberInfo.ChildKeyPropertyName}!={mi.ParentMemberInfo.LowerChildKeyPropertyName};");
            } else {
              sw.WriteLine($"      var has{mi.MemberName}Changed = {LowerClassName}.{mi.MemberName}!={mi.LowerMemberName} || " +
                $"{LowerClassName}.{mi.ParentMemberInfo.ChildKeyPropertyName}!={mi.ParentMemberInfo.LowerChildKeyPropertyName} || " +
                $"{LowerClassName}.{mi.ParentMemberInfo.ChildKey2PropertyName}!={mi.ParentMemberInfo.LowerChildKey2PropertyName};");
            }
          } else {
            //parent has single child or list of children
            sw.WriteLine($"      var has{mi.MemberName}Changed = {LowerClassName}.{mi.MemberName}!={mi.LowerMemberName};");
          }
          if (mi.IsNullable) {
            sw.WriteLine($"      if ({LowerClassName}.{mi.MemberName} is not null && has{mi.MemberName}Changed && {LowerClassName}.{mi.MemberName}!={mi.TypeStringNotNullable}.No{mi.TypeStringNotNullable}) {{");
          } else {
            sw.WriteLine($"      if (has{mi.MemberName}Changed && {LowerClassName}.{mi.MemberName}!={mi.TypeStringNotNullable}.No{mi.TypeStringNotNullable}) {{");
          }
          sw.WriteLine($"        {LowerClassName}.{mi.MemberName}.RemoveFrom{mi.ParentMemberInfo!.MemberName}({LowerClassName});");
          sw.WriteLine($"      }}");
        }
      }


      //update the item properties before updating the collections in the item's parents
      sw.WriteLine();
      sw.WriteLine("      //update not readonly properties");
      foreach (var mi in Members.Values) {
        if (mi.IsReadOnly || mi.MemberType==MemberTypeEnum.ToLower || mi.MemberType>MemberTypeEnum.LinkToParent) continue;

        if (mi.ParentMemberInfo?.MemberType==MemberTypeEnum.ParentMultipleChildrenHashSet) {
          //parent uses a HashSet instead a List for children, because more than one property in the child has
          //the parent's type. The child should get added to parent's Children only once, regardless how many
          //of the child's properties link to that parent. When removing a child parent link during updating such
          //a child property, the parent will only remove the child if no other child property links to it. If 2
          //child property each remove a particulat parent, the first property must be properly updated before
          //the second property's update get processed. The second update can only remove the child from 
          //parent.Children if the first property points already to a different parent or is null.
          sw.WriteLine($"      var has{mi.MemberName}Changed = {LowerClassName}.{mi.MemberName}!={mi.LowerMemberName};");
          sw.WriteLine($"      if (has{mi.MemberName}Changed){{");
          if (mi.IsNullable) {
            sw.WriteLine($"        if ({LowerClassName}.{mi.MemberName} is not null) {{");
            sw.WriteLine($"          {LowerClassName}.{mi.MemberName}.RemoveFrom{mi.ParentMemberInfo!.MemberName}({LowerClassName});");
            sw.WriteLine($"        }}");
          } else {
            sw.WriteLine($"          {LowerClassName}.{mi.MemberName}.RemoveFrom{mi.ParentMemberInfo!.MemberName}({LowerClassName});");
          }
          sw.WriteLine($"        {LowerClassName}.{mi.MemberName} = {mi.LowerMemberName};");
          sw.WriteLine($"      }}");

        } else {
          if (mi.Rounding!=null) {
            sw.WriteLine($"      {mi.LowerMemberName} = {mi.LowerMemberName}{mi.Rounding};");
          }
          if (mi.NeedsDictionary || (mi.ToLowerTarget!=null && mi.ToLowerTarget.NeedsDictionary)) {
            //all property types except links to parents using a HashSet for children
            sw.WriteLine($"      if ({LowerClassName}.{mi.MemberName}!={mi.LowerMemberName}) {{");
            writeNeedsDictionaryRemoveStatement(sw, mi, context, needsDictEnum.UpdateCsv);
            sw.WriteLine($"        {LowerClassName}.{mi.MemberName} = {mi.LowerMemberName};");
            writeToLowerCopyStatement(sw, mi, needsDictEnum.UpdateCsv);
            writeNeedsDictionaryAddStatement(sw, mi, context, needsDictEnum.UpdateCsv);
            sw.WriteLine($"      }}");
          } else {
            sw.WriteLine($"      {LowerClassName}.{mi.MemberName} = {mi.LowerMemberName};");
          }
        }
      }

      if (HasNotReadOnlyAndNotLookupParents) {
        sw.WriteLine();
        sw.WriteLine($"      //add updated item to parents which have been newly added during update");
        foreach (var mi in Members.Values) {
          if (mi.IsReadOnly || mi.MemberType!=MemberTypeEnum.LinkToParent || mi.IsLookupOnly) continue;

          if (mi.IsNullable) {
            sw.WriteLine($"      if ({LowerClassName}.{mi.MemberName} is not null && has{mi.MemberName}Changed && {LowerClassName}.{mi.MemberName}!={mi.TypeStringNotNullable}.No{mi.TypeStringNotNullable}) {{");
          } else {
            sw.WriteLine($"      if (has{mi.MemberName}Changed && {LowerClassName}.{mi.MemberName}!={mi.TypeStringNotNullable}.No{mi.TypeStringNotNullable}) {{");
          }
          sw.WriteLine($"        {LowerClassName}.{mi.MemberName}.AddTo{mi.ParentMemberInfo!.MemberName}({LowerClassName});");
          sw.WriteLine($"      }}");
        }
      }
      sw.WriteLine($"      {LowerClassName}.onCsvUpdate();");
      sw.WriteLine("    }");
      sw.WriteLine("    partial void onCsvUpdate();");
      sw.WriteLine();
      sw.WriteLine();
    }


    private void writeAddToRemoveFrom(StreamWriter sw, string context, TracingEnum isTracing) {
      foreach (var mi in Members.Values) {
        if (mi.MemberType>=MemberTypeEnum.ParentOneChild) {
          //ParentOneChild or ParentMultipleChildren

          if (mi.MemberType==MemberTypeEnum.ParentOneChild) {
            sw.WriteLine("    /// <summary>");
            sw.WriteLine($"    /// Add {mi.LowerChildTypeName} to {mi.MemberName}.");
            sw.WriteLine("    /// </summary>");
            sw.WriteLine($"    internal void AddTo{mi.MemberName}({mi.ChildTypeName} {mi.LowerChildTypeName}) {{");
            sw.WriteLine($"#if DEBUG");
            sw.WriteLine($"      if ({mi.LowerChildTypeName}=={mi.ChildTypeName}.No{mi.ChildTypeName}) throw new Exception();");
            sw.WriteLine($"      if (({mi.LowerChildTypeName}.Key>=0)&&(Key<0)) throw new Exception();");
            sw.WriteLine($"      if({mi.MemberName}=={mi.LowerChildTypeName}) throw new Exception();");
            sw.WriteLine($"#endif");
            sw.WriteLine($"      if ({mi.MemberName}!=null) {{");
            sw.WriteLine($"        throw new Exception($\"{mi.ClassInfo.ClassName}.AddTo{mi.MemberName}(): " +
              $"'{{{mi.MemberName}}}' is already assigned to {mi.MemberName}, it is not possible to assign now '{{{mi.LowerChildTypeName}}}'.\");");
            sw.WriteLine($"      }}");
            sw.WriteLine($"      {mi.MemberName} = {mi.LowerChildTypeName};");
            sw.WriteLine($"      onAddedTo{mi.MemberName}({mi.LowerChildTypeName});");
            Compiler.WriteLinesTracing(sw, isTracing,
                         $"      {context}.Trace?.Invoke($\"Add {mi.ChildTypeName} {{{mi.LowerChildTypeName}.GetKeyOrHash()}} to \" +",
                         $"        $\"{{this.GetKeyOrHash()}} {ClassName}.{mi.MemberName}\");");
            sw.WriteLine("    }");
            sw.WriteLine($"    partial void onAddedTo{mi.MemberName}({mi.ChildTypeName} {mi.LowerChildTypeName});");
            sw.WriteLine();
            sw.WriteLine();

          } else if (mi.MemberType>MemberTypeEnum.ParentOneChild) {
            //ParentMultipleChildrenXxx
            sw.WriteLine("    /// <summary>");
            sw.WriteLine($"    /// Add {mi.LowerChildTypeName} to {mi.MemberName}.");
            sw.WriteLine("    /// </summary>");
            sw.WriteLine($"    internal void AddTo{mi.MemberName}({mi.ChildTypeName} {mi.LowerChildTypeName}) {{");
            sw.WriteLine($"#if DEBUG");
            sw.WriteLine($"      if ({mi.LowerChildTypeName}=={mi.ChildTypeName}.No{mi.ChildTypeName}) throw new Exception();");
            sw.WriteLine($"      if (({mi.LowerChildTypeName}.Key>=0)&&(Key<0)) throw new Exception();");
            if (mi.MemberType==MemberTypeEnum.ParentMultipleChildrenList) {
              sw.WriteLine($"      if ({mi.LowerMemberName}.Contains({mi.LowerChildTypeName})) throw new Exception();");
            }else if (mi.MemberType==MemberTypeEnum.ParentMultipleChildrenHashSet) {
              //no test needed
            } else if(mi.MemberType==MemberTypeEnum.ParentMultipleChildrenSortedBucket) {
              sw.WriteLine($"      if ({mi.LowerMemberName}.Contains({mi.LowerChildTypeName}.{mi.ChildKeyPropertyName}, " +
                $"{mi.LowerChildTypeName}.{mi.ChildKey2PropertyName})) throw new Exception();");
            } else {//Dictionary or SortedList
              sw.WriteLine($"      if ({mi.LowerMemberName}.ContainsKey({mi.LowerChildTypeName}.{mi.ChildKeyPropertyName})) throw new Exception();");
            }
            sw.WriteLine($"#endif");
            if (mi.MemberType is MemberTypeEnum.ParentMultipleChildrenList or MemberTypeEnum.ParentMultipleChildrenHashSet) {
              sw.WriteLine($"      {mi.LowerMemberName}.Add({mi.LowerChildTypeName});");
            } else if(mi.MemberType==MemberTypeEnum.ParentMultipleChildrenSortedBucket) {
              sw.WriteLine($"      {mi.LowerMemberName}.Add({mi.LowerChildTypeName});");
            } else { //Dictionary or SortedList
              sw.WriteLine($"      {mi.LowerMemberName}.Add({mi.LowerChildTypeName}.{mi.ChildKeyPropertyName}, " +
                $"{mi.LowerChildTypeName});");
            }
            sw.WriteLine($"      onAddedTo{mi.MemberName}({mi.LowerChildTypeName});");
            Compiler.WriteLinesTracing(sw, isTracing,
                         $"      {context}.Trace?.Invoke($\"Add {mi.ChildTypeName} {{{mi.LowerChildTypeName}.GetKeyOrHash()}} to \" +",
                         $"        $\"{{this.GetKeyOrHash()}} {ClassName}.{mi.MemberName}\");");
            sw.WriteLine("    }");
            sw.WriteLine($"    partial void onAddedTo{mi.MemberName}({mi.ChildTypeName} {mi.LowerChildTypeName});");
            sw.WriteLine();
            sw.WriteLine();
          }

          //RemoveFrom
          //----------
          sw.WriteLine("    /// <summary>");
          sw.WriteLine($"    /// Removes {mi.LowerChildTypeName} from {mi.ClassInfo!.ClassName}.");
          sw.WriteLine("    /// </summary>");

          if (mi.MemberType==MemberTypeEnum.ParentOneChild) {
            sw.WriteLine($"    internal void RemoveFrom{mi.MemberName}({mi.ChildTypeName} {mi.LowerChildTypeName}) {{");
            sw.WriteLine($"#if DEBUG");
            sw.WriteLine($"      if ({mi.MemberName}!={mi.LowerChildTypeName}) {{");
            sw.WriteLine($"        throw new Exception($\"{mi.ClassInfo!.ClassName}.RemoveFrom{mi.MemberName}(): {mi.MemberName} does not link to {mi.LowerChildTypeName} '{{{mi.LowerChildTypeName}}}' but '{{{mi.MemberName}}}'.\");");
            sw.WriteLine($"      }}");
            sw.WriteLine($"#endif");
            sw.WriteLine($"      {mi.MemberName} = null;");
            sw.WriteLine($"      onRemovedFrom{mi.MemberName}({mi.LowerChildTypeName});");
            Compiler.WriteLinesTracing(sw, isTracing,
                        $"      {context}.Trace?.Invoke($\"Remove {mi.ChildTypeName} {{{mi.LowerChildTypeName}.GetKeyOrHash()}} from \" +",
                        $"        $\"{{this.GetKeyOrHash()}} {ClassName}.{mi.MemberName}\");");
            sw.WriteLine($"    }}");
            sw.WriteLine($"    partial void onRemovedFrom{mi.MemberName}({mi.ChildTypeName} {mi.LowerChildTypeName});");

          } else {
            //ParentMultipleChildrenXxx
            sw.WriteLine($"    internal void RemoveFrom{mi.MemberName}({mi.ChildTypeName} {mi.LowerChildTypeName}) {{");
            if (mi.MemberType==MemberTypeEnum.ParentMultipleChildrenList) {
              sw.WriteLine("#if DEBUG");
              sw.WriteLine($"      if (!{mi.LowerMemberName}.Remove({mi.LowerChildTypeName})) throw new Exception();");
              sw.WriteLine("#else");
              sw.WriteLine($"        {mi.LowerMemberName}.Remove({mi.LowerChildTypeName});");
              sw.WriteLine("#endif");
            } else if (mi.MemberType==MemberTypeEnum.ParentMultipleChildrenHashSet) {
              sw.WriteLine("      //Execute Remove() only when exactly one property in the child still links to this parent. If");
              sw.WriteLine("      //no property links here (Count=0), the child should not be in the children collection. If");
              sw.WriteLine("      //more than 1 child property links here, it cannot yet be removed from the children collection.");
              sw.WriteLine("      var countLinks = 0;");
              foreach (var childMI in mi.ChildClassInfo!.Members.Values) {
                if (childMI.MemberType==MemberTypeEnum.LinkToParent && (childMI.TypeStringNotNullable)==ClassName) {
                  sw.WriteLine($"      if ({childMI.ClassInfo.LowerClassName}.{childMI.MemberName}==this ) countLinks++;");
                }
              }
              sw.WriteLine("      if (countLinks>1) return;");
              sw.WriteLine("#if DEBUG");
              sw.WriteLine("      if (countLinks==0) throw new Exception();");
              sw.WriteLine($"      if (!{mi.LowerMemberName}.Remove({mi.LowerChildTypeName})) throw new Exception();");
              sw.WriteLine("#else");
              sw.WriteLine($"        {mi.LowerMemberName}.Remove({mi.LowerChildTypeName});");
              sw.WriteLine("#endif");

            } else if(mi.MemberType==MemberTypeEnum.ParentMultipleChildrenSortedBucket) {
              sw.WriteLine("#if DEBUG");
              sw.WriteLine($"      if (!{mi.LowerMemberName}.Remove({mi.LowerChildTypeName})) throw new Exception();");
              sw.WriteLine("#else");
              sw.WriteLine($"        {mi.LowerMemberName}.Remove({mi.LowerChildTypeName});");
              sw.WriteLine("#endif");
            } else { //Dictionary or SortedList
              sw.WriteLine("#if DEBUG");
              sw.WriteLine($"      if (!{mi.LowerMemberName}.Remove({mi.LowerChildTypeName}.{mi.ChildKeyPropertyName})) throw new Exception();");
              sw.WriteLine("#else");
              sw.WriteLine($"        {mi.LowerMemberName}.Remove({mi.LowerChildTypeName}.{mi.ChildKeyPropertyName});");
              sw.WriteLine("#endif");
            }

            sw.WriteLine($"      onRemovedFrom{mi.MemberName}({mi.LowerChildTypeName});");
            Compiler.WriteLinesTracing(sw, isTracing,
                         $"      {context}.Trace?.Invoke($\"Remove {mi.ChildTypeName} {{{mi.LowerChildTypeName}.GetKeyOrHash()}} from \" +",
                         $"        $\"{{this.GetKeyOrHash()}} {ClassName}.{mi.MemberName}\");");
            sw.WriteLine("    }");
            sw.WriteLine($"    partial void onRemovedFrom{mi.MemberName}({mi.ChildTypeName} {mi.LowerChildTypeName});");
          }
          sw.WriteLine();
          sw.WriteLine();
        }

        if (mi.MemberType==MemberTypeEnum.ParentMultipleChildrenHashSet) {
          //RollbackAddTo
          //-------------
          sw.WriteLine("    /// <summary>");
          sw.WriteLine($"    /// Removes {mi.LowerChildTypeName} from {mi.ClassInfo!.ClassName} without testing {mi.LowerChildTypeName} properties. Called from ");
          sw.WriteLine($"    /// {mi.ChildTypeName}.RollbackItemNew().");
          sw.WriteLine("    /// </summary>");
          sw.WriteLine($"    internal void RollbackAddTo{mi.MemberName}({mi.ChildTypeName} {mi.LowerChildTypeName}) {{");
          sw.WriteLine($"      {mi.LowerMemberName}.Remove({mi.LowerChildTypeName});");
          sw.WriteLine($"      onRollbackAddTo{mi.MemberName}({mi.LowerChildTypeName});");
          Compiler.WriteLinesTracing(sw, isTracing,
                       $"      {context}.Trace?.Invoke($\"RollbackAddTo{mi.MemberName}: Remove {mi.ChildTypeName} {{{mi.LowerChildTypeName}.GetKeyOrHash()}} from \" +",
                       $"        $\"{{this.GetKeyOrHash()}} {ClassName}.{mi.MemberName}\");");
          sw.WriteLine("    }");
          sw.WriteLine($"    partial void onRollbackAddTo{mi.MemberName}({mi.ChildTypeName} {mi.LowerChildTypeName});");
          sw.WriteLine();
          sw.WriteLine();
        }
      }
    }


    private void writeRelease(StreamWriter sw, string context, TracingEnum isTracing) {
      sw.WriteLine("    /// <summary>");
      sw.WriteLine($"    /// Removes {ClassName} from {context}.Data.{PluralName}.");
      //IsPerformReleaseNeeded = writeRemoveComment(sw, context, isCapitaliseFirstLetter: false);
      //sw.WriteLine(".");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine("    public void Release() {");
      sw.WriteLine("      if (Key<0) {");
      sw.WriteLine($"        throw new Exception($\"{ClassName}.Release(): {ClassName} '{{this}}' is not stored in {context}.Data, key is {{Key}}.\");");
      sw.WriteLine("      }");
      sw.WriteLine("      onReleasing();");
      foreach (var mi in Members.Values) {
        if (mi.MemberType==MemberTypeEnum.ParentOneChild) {
          sw.WriteLine($"      if ({mi.MemberName}{mi.QMark}.Key>=0) {{");
          sw.WriteLine($"        throw new Exception($\"Cannot release {ClassName} '{{this}}' \" + Environment.NewLine + ");
          sw.WriteLine($"          $\"because '{{{mi.MemberName}}}' in {ClassName}.{mi.MemberName} is still stored.\");");
          sw.WriteLine("      }");

        } else if (mi.MemberType>MemberTypeEnum.ParentOneChild) { //ParentMultipleChildren
          if (mi.MemberType is
            MemberTypeEnum.ParentMultipleChildrenList or
            MemberTypeEnum.ParentMultipleChildrenHashSet or
            MemberTypeEnum.ParentMultipleChildrenSortedBucket) 
          {
            sw.WriteLine($"      foreach (var {mi.LowerChildTypeName} in {mi.MemberName}) {{");
          } else {
            sw.WriteLine($"      foreach (var {mi.LowerChildTypeName} in {mi.MemberName}.Values) {{");
          }
          sw.WriteLine($"        if ({mi.LowerChildTypeName}?.Key>=0) {{");
          sw.WriteLine($"          throw new Exception($\"Cannot release {ClassName} '{{this}}' \" + Environment.NewLine + ");
          sw.WriteLine($"            $\"because '{{{mi.LowerChildTypeName}}}' in {ClassName}.{mi.MemberName} is still stored.\");");
          sw.WriteLine("        }");
          sw.WriteLine("      }");
        }
      }
      foreach (var mi in Members.Values) {
        if (mi.MemberType<MemberTypeEnum.ToLower || mi.MemberType==MemberTypeEnum.Enum) {
          writeNeedsDictionaryRemoveStatement(sw, mi, context, needsDictEnum.Release);
        }
      }
      //sw.WriteLine("      //the testing if this instance can be released gets executed in PerformRelease(), which gets");
      //sw.WriteLine("      //called during the execution of the following line.");
      sw.WriteLine($"      {context}.Data._{PluralName}.Remove(Key);");
      sw.WriteLine("      onReleased();");
      Compiler.WriteLinesTracing(sw, isTracing,
                   $"      {context}.Trace?.Invoke($\"Released {ClassName} @{{Key}} #{{GetHashCode()}}\");");
      sw.WriteLine("    }");
      sw.WriteLine("    partial void onReleasing();");
      sw.WriteLine("    partial void onReleased();");
      sw.WriteLine();
      sw.WriteLine();
    }


    private void writeDisconnect(StreamWriter sw, string context, TracingEnum isTracing) {
      sw.WriteLine("    /// <summary>");
      sw.WriteLine($"    /// Disconnects {ClassName} from parents and possibly from dictionaries in {context}.Data.");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine($"    internal static void Disconnect({ClassName} {LowerClassName}){{");

      foreach (var mi in Members.Values) {
        if (mi.MemberType==MemberTypeEnum.LinkToParent && !mi.IsLookupOnly) {
          if (mi.IsNullable) {
            sw.WriteLine($"      if ({LowerClassName}.{mi.MemberName}!=null && {LowerClassName}.{mi.MemberName}!={mi.TypeStringNotNullable}.No{mi.TypeStringNotNullable}) {{");
          } else {
            sw.WriteLine($"      if ({LowerClassName}.{mi.MemberName}!={mi.TypeStringNotNullable}.No{mi.TypeStringNotNullable}) {{");
          }
          if (mi.ParentMemberInfo!.MemberType==MemberTypeEnum.ParentMultipleChildrenHashSet) {
            sw.WriteLine($"        {LowerClassName}.{mi.MemberName}.RollbackAddTo{mi.ParentMemberInfo!.MemberName}({LowerClassName});");
          } else {
            sw.WriteLine($"        {LowerClassName}.{mi.MemberName}.RemoveFrom{mi.ParentMemberInfo!.MemberName}({LowerClassName});");
          }
          sw.WriteLine("      }");
        }
        if (mi.MemberType<MemberTypeEnum.ToLower || mi.MemberType==MemberTypeEnum.Enum) {
          writeNeedsDictionaryRemoveStatement(sw, mi, context, needsDictEnum.Disconnect);
        }
      }
      sw.WriteLine($"      {LowerClassName}.onDisconnected();");
      Compiler.WriteLinesTracing(sw, isTracing,
                   $"      {context}.Trace?.Invoke($\"Disconnected {ClassName} @{{{LowerClassName}.Key}} #{{{LowerClassName}.GetHashCode()}} from parents.\");");
      sw.WriteLine("    }");
      sw.WriteLine("    partial void onDisconnected();");
      sw.WriteLine();
      sw.WriteLine();
    }


    private void writeTransactionSupport(StreamWriter sw, string context, TracingEnum isTracing) {
      //RollbackItemNew
      //---------------
      sw.WriteLine("    /// <summary>");
      if (HasParents) {
        sw.WriteLine($"    /// Removes {ClassName} from parents as part of a transaction rollback of the new() statement.");
      } else {
        sw.WriteLine($"    /// Undoes the new() statement as part of a transaction rollback.");
      }
      sw.WriteLine("    /// </summary>");
      sw.WriteLine("    internal static void RollbackItemNew(IStorageItem item) {");
      sw.WriteLine($"      var {LowerClassName} = ({ClassName}) item;");
      Compiler.WriteLinesTracing(sw, isTracing,
                   $"      {context}.Trace?.Invoke($\"Rollback new {ClassName}(): {{{LowerClassName}.ToTraceString()}}\");");
      foreach (var mi in Members.Values) {
        if (mi.MemberType==MemberTypeEnum.LinkToParent) {
          if (!mi.IsLookupOnly) {
            if (mi.IsNullable) {
              sw.WriteLine($"      if ({LowerClassName}.{mi.MemberName}!=null && {LowerClassName}.{mi.MemberName}!={mi.TypeStringNotNullable}.No{mi.TypeStringNotNullable}) {{");
            } else {
              sw.WriteLine($"      if ({LowerClassName}.{mi.MemberName}!={mi.TypeStringNotNullable}.No{mi.TypeStringNotNullable}) {{");
            }
            if (mi.ParentMemberInfo!.MemberType==MemberTypeEnum.ParentMultipleChildrenHashSet) {
              sw.WriteLine($"        {LowerClassName}.{mi.MemberName}.RollbackAddTo{mi.ParentMemberInfo!.MemberName}({LowerClassName});");
            } else {
              sw.WriteLine($"        {LowerClassName}.{mi.MemberName}.RemoveFrom{mi.ParentMemberInfo!.MemberName}({LowerClassName});");
            }
            sw.WriteLine("      }");
          }
        }
      }
      sw.WriteLine($"      {LowerClassName}.onRollbackItemNew();");
      sw.WriteLine("    }");
      sw.WriteLine("    partial void onRollbackItemNew();");
      sw.WriteLine();
      sw.WriteLine();

      //RollbackItemStore
      //-----------------
      sw.WriteLine("    /// <summary>");
      sw.WriteLine($"    /// Releases {ClassName} from {context}.Data.{PluralName} as part of a transaction rollback of Store().");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine("    internal static void RollbackItemStore(IStorageItem item) {");
      sw.WriteLine($"      var {LowerClassName} = ({ClassName}) item;");
      Compiler.WriteLinesTracing(sw, isTracing,
                   $"      {context}.Trace?.Invoke($\"Rollback {ClassName}.Store(): {{{LowerClassName}.ToTraceString()}}\");");
      foreach (var mi in Members.Values) {
        if (mi.MemberType<MemberTypeEnum.ToLower || mi.MemberType==MemberTypeEnum.Enum) {
          writeNeedsDictionaryRemoveStatement(sw, mi, context, needsDictEnum.RollbackStore);
        }
      }
      sw.WriteLine($"      {LowerClassName}.onRollbackItemStored();");
      sw.WriteLine("    }");
      sw.WriteLine("    partial void onRollbackItemStored();");
      sw.WriteLine();
      sw.WriteLine();

      //RollbackItemUpdate
      //------------------
      sw.WriteLine("    /// <summary>");
      sw.WriteLine($"    /// Restores the {ClassName} item data as it was before the last update as part of a transaction rollback.");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine("    internal static void RollbackItemUpdate(IStorageItem oldStorageItem, IStorageItem newStorageItem) {");
      sw.WriteLine($"      var oldItem = ({ClassName}) oldStorageItem;//an item clone with the values before item was updated");
      sw.WriteLine($"      var item = ({ClassName}) newStorageItem;//is the instance whose values should be restored");
      Compiler.WriteLinesTracing(sw, isTracing,
                  $"      {context}.Trace?.Invoke($\"Rolling back {ClassName}.Update(): {{item.ToTraceString()}}\");");

      if (HasReadOnlies) {
        sw.WriteLine();
        sw.WriteLine($"      // if possible, throw exceptions before changing anything");
        foreach (var mi in Members.Values) {
          if (mi.MemberType==MemberTypeEnum.ToLower) continue;

          if (mi.IsReadOnly) {
            sw.WriteLine($"      if (item.{mi.MemberName}!=oldItem.{mi.MemberName}) {{");
            sw.WriteLine($"        throw new Exception($\"{ClassName}.Update(): Property {mi.MemberName}" +
              $" '{{item.{mi.MemberName}}}' is \" +");
            sw.WriteLine($"          $\"readonly, {mi.MemberName} '{{oldItem.{mi.MemberName}}}' should be the same.\" + Environment.NewLine + ");
            sw.WriteLine($"          item.ToString());");
            sw.WriteLine("      }");
          }
        }
      }

      if (HasNotReadOnlyAndNotLookupParents || HasNotReadOnlyNeedDirectories) {
        sw.WriteLine();
        if (!HasNotReadOnlyAndNotLookupParents) {
          sw.WriteLine($"      // remove updated item from dictionaries");
        } else if (!HasNotReadOnlyNeedDirectories) {
          sw.WriteLine($"      // remove updated item from parents");
        } else {
          sw.WriteLine($"      // remove updated item from parents and dictionaries");
        }
        foreach (var mi in Members.Values) {
          if (mi.MemberType==MemberTypeEnum.ToLower || mi.IsLookupOnly || mi.IsReadOnly) continue;

          if (mi.MemberType==MemberTypeEnum.LinkToParent) {
            var parentMI = mi.ParentMemberInfo!;
            if (parentMI.MemberType==MemberTypeEnum.ParentMultipleChildrenDictionary || 
              parentMI.MemberType==MemberTypeEnum.ParentMultipleChildrenSortedList) 
            {
              sw.WriteLine($"      var has{mi.MemberName}Changed = oldItem.{mi.MemberName}!=item.{mi.MemberName} ||" +
                $" oldItem.{parentMI.ChildKeyPropertyName}!=item.{parentMI.ChildKeyPropertyName};");
            } else if(parentMI.MemberType==MemberTypeEnum.ParentMultipleChildrenSortedBucket) {
              sw.WriteLine($"      var has{mi.MemberName}Changed = oldItem.{mi.MemberName}!=item.{mi.MemberName} ||" +
                $" oldItem.{parentMI.ChildKeyPropertyName}!=item.{parentMI.ChildKeyPropertyName} ||" +
                $" oldItem.{parentMI.ChildKey2PropertyName}!=item.{parentMI.ChildKey2PropertyName};");
            } else {
              //List, HashSet or parent with 1 child only
              sw.WriteLine($"      var has{mi.MemberName}Changed = oldItem.{mi.MemberName}!=item.{mi.MemberName};");
            }
            //if (mi.IsNullable) {
            //  sw.WriteLine($"      if (has{mi.MemberName}Changed && item.{mi.MemberName} is not null) {{");
            //} else {
            //  sw.WriteLine($"      if (has{mi.MemberName}Changed) {{");
            //}
            //sw.WriteLine($"        item.{mi.MemberName}.RemoveFrom{mi.ParentMemberInfo!.MemberName}(item);");
            //if (parentMI.MemberType==MemberTypeEnum.ParentMultipleChildrenHashSet) {
            //  //property needs to be updated already here, otherwise RemoveFrom() will not work if 2 properties link
            //  //to the same parent.
            //  sw.WriteLine($"        item.{mi.MemberName} = oldItem.{mi.MemberName};");
            //}
            //sw.WriteLine($"      }}");

            if (mi.IsNullable && parentMI.MemberType==MemberTypeEnum.ParentMultipleChildrenHashSet) {
              sw.WriteLine($"      if (has{mi.MemberName}Changed) {{");
              sw.WriteLine($"        if (item.{mi.MemberName} is not null) {{");
              sw.WriteLine($"          item.{mi.MemberName}.RemoveFrom{mi.ParentMemberInfo!.MemberName}(item);");
              sw.WriteLine($"        }}");
              sw.WriteLine($"        item.{mi.MemberName} = oldItem.{mi.MemberName};");
            } else {
              if (mi.IsNullable) {
                sw.WriteLine($"      if (has{mi.MemberName}Changed && item.{mi.MemberName} is not null) {{");
              } else {
                sw.WriteLine($"      if (has{mi.MemberName}Changed) {{");
              }
              sw.WriteLine($"        item.{mi.MemberName}.RemoveFrom{mi.ParentMemberInfo!.MemberName}(item);");
              if (parentMI.MemberType==MemberTypeEnum.ParentMultipleChildrenHashSet) {
                //property needs to be updated already here, otherwise RemoveFrom() will not work if 2 properties link
                //to the same parent.
                sw.WriteLine($"        item.{mi.MemberName} = oldItem.{mi.MemberName};");
              }
            }
            sw.WriteLine($"      }}");
          } else {
            writeNeedsDictionaryRemoveStatement(sw, mi, context, needsDictEnum.RollbackUpdate);
          }
        }
      }

      sw.WriteLine();
      sw.WriteLine($"      // updated item: restore old values");
      foreach (var mi in Members.Values) {
        if (
          mi.MemberType==MemberTypeEnum.ToLower || 
          mi.MemberType>=MemberTypeEnum.ParentOneChild ||
          mi.ParentMemberInfo?.MemberType==MemberTypeEnum.ParentMultipleChildrenHashSet || //value is already updated
          mi.IsReadOnly) continue;

        sw.WriteLine($"      item.{mi.MemberName} = oldItem.{mi.MemberName};");
        writeToLowerCopyStatement(sw, mi, needsDictEnum.RollbackUpdate);
      }

      if (HasNotReadOnlyAndNotLookupParents || HasNotReadOnlyNeedDirectories) {
        sw.WriteLine();
        if (!HasNotReadOnlyAndNotLookupParents) {
          sw.WriteLine($"      // add item with previous values to dictionaries");
        } else if (!HasNotReadOnlyNeedDirectories) {
          sw.WriteLine($"      // add item with previous values to parents");
        } else {
          sw.WriteLine($"      // add item with previous values to parents and dictionaries");
        }
        foreach (var mi in Members.Values) {
          if (mi.MemberType==MemberTypeEnum.ToLower || mi.IsLookupOnly || mi.IsReadOnly) continue;

          if (mi.MemberType==MemberTypeEnum.LinkToParent) {
            if (mi.IsNullable) {
              sw.WriteLine($"      if (has{mi.MemberName}Changed && item.{mi.MemberName} is not null) {{");
            } else {
              sw.WriteLine($"      if (has{mi.MemberName}Changed) {{");
            }
            sw.WriteLine($"        item.{mi.MemberName}.AddTo{mi.ParentMemberInfo!.MemberName}(item);");
            sw.WriteLine($"      }}");
          } else {
            writeNeedsDictionaryAddStatement(sw, mi, context, needsDictEnum.RollbackUpdate);
          }
        }
      }

      sw.WriteLine($"      item.onRollbackItemUpdated(oldItem);");
      Compiler.WriteLinesTracing(sw, isTracing,
                   $"      {context}.Trace?.Invoke($\"Rolled back {ClassName}.Update(): {{item.ToTraceString()}}\");");
      sw.WriteLine("    }");
      sw.WriteLine($"    partial void onRollbackItemUpdated({ClassName} old{ClassName});");
      sw.WriteLine();
      sw.WriteLine();

      //RollbackItemRelease
      //-------------------
      sw.WriteLine("    /// <summary>");
      sw.WriteLine($"    /// Adds {ClassName} to {context}.Data.{PluralName} as part of a transaction rollback of Release().");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine("    internal static void RollbackItemRelease(IStorageItem item) {");
      sw.WriteLine($"      var {LowerClassName} = ({ClassName}) item;");
      Compiler.WriteLinesTracing(sw, isTracing,
                  $"      {context}.Trace?.Invoke($\"Rollback {ClassName}.Release(): {{{LowerClassName}.ToTraceString()}}\");");
      foreach (var mi in Members.Values) {
        if (mi.MemberType==MemberTypeEnum.Enum || mi.MemberType<=MemberTypeEnum.String) {
          //enum, simple data type
          writeNeedsDictionaryAddStatement(sw, mi, context, needsDictEnum.RollbackRelease);
        }
      }
      sw.WriteLine($"      {LowerClassName}.onRollbackItemRelease();");
      sw.WriteLine("    }");
      sw.WriteLine("    partial void onRollbackItemRelease();");
      sw.WriteLine();
      sw.WriteLine();
    }


    private void writeToString(StreamWriter sw, List<string> lines) {
      sw.WriteLine("    /// <summary>");
      sw.WriteLine("    /// Returns property values for tracing. Parents are shown with their key instead their content.");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine("    public string ToTraceString() {");
      sw.WriteLine("      var returnString =");
      lines.Clear();
      lines.Add("        $\"{this.GetKeyOrHash()}");
      foreach (var mi in Members.Values) {
        if (mi.MemberType==MemberTypeEnum.LinkToParent) {
          lines.Add($"        $\" {mi.MemberName} {{{mi.MemberName}{mi.QMark}.GetKeyOrHash()}}");
        } else if (mi.MemberType<=MemberTypeEnum.Enum) {
          //simple data type or enum
          lines.Add($"        $\" {{{mi.MemberName}{mi.ToStringFunc}}}");
        }
      }
      for (int linesIndex = 0; linesIndex < lines.Count; linesIndex++) {
        var line = lines[linesIndex];
        sw.Write(line);
        if (linesIndex+1<lines.Count) {
          sw.WriteLine("|\" +");
        } else {
          sw.WriteLine("\";");
        }
      }
      sw.WriteLine("      onToTraceString(ref returnString);");
      sw.WriteLine("      return returnString;");
      sw.WriteLine("    }");
      sw.WriteLine("    partial void onToTraceString(ref string returnString);");
      sw.WriteLine();
      sw.WriteLine();
      sw.WriteLine("    /// <summary>");
      sw.WriteLine("    /// Returns property values");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine("    public string ToShortString() {");
      sw.WriteLine("      var returnString =");
      lines.Clear();
      lines.Add("        $\"{Key.ToKeyString()}");
      foreach (var mi in Members.Values) {
        if (mi.MemberType<=MemberTypeEnum.LinkToParent) {
          //simple data type, enum or LinkToParent
          lines.Add($"        $\" {{{mi.MemberName}{mi.ToStringFunc}}}");
        }
      }
      for (int linesIndex = 0; linesIndex < lines.Count; linesIndex++) {
        var line = lines[linesIndex];
        sw.Write(line);
        if (linesIndex+1<lines.Count) {
          sw.WriteLine(",\" +");
        } else {
          sw.WriteLine("\";");
        }
      }
      sw.WriteLine("      onToShortString(ref returnString);");
      sw.WriteLine("      return returnString;");
      sw.WriteLine("    }");
      sw.WriteLine("    partial void onToShortString(ref string returnString);");
      sw.WriteLine();
      sw.WriteLine();
      sw.WriteLine("    /// <summary>");
      sw.WriteLine("    /// Returns all property names and values");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine("    public override string ToString() {");
      sw.WriteLine("      var returnString =");
      lines.Clear();
      lines.Add("        $\"Key: {Key.ToKeyString()}");
      foreach (var mi in Members.Values) {
        if (mi.MemberType==MemberTypeEnum.ParentOneChild) {
          lines.Add($"        $\" {mi.MemberName}: {{{mi.MemberName}?.ToShortString()}}");
        } else if (mi.MemberType>MemberTypeEnum.ParentOneChild) {
          //List, Directory, SortedList or SortedBucketCollection
          lines.Add($"        $\" {mi.MemberName}: {{{mi.MemberName}.Count}}");
          if (mi.ChildClassInfo!.AreInstancesReleasable && mi.SingleChildMI is not null) {
            //not a parent List<> referenced by 2 properties in the child class
            lines.Add($"        $\" {mi.MemberName}Stored: {{{mi.MemberName}.CountStoredItems}}");
          }
        } else {
          //simple data type and enum
          lines.Add($"        $\" {mi.MemberName}: {{{mi.MemberName}{mi.ToStringFunc}}}");
        }
      }
      for (int linesIndex = 0; linesIndex < lines.Count; linesIndex++) {
        var line = lines[linesIndex];
        sw.Write(line);
        if (linesIndex+1<lines.Count) {
          sw.WriteLine(",\" +");
        } else {
          sw.WriteLine(";\";");
        }
      }
      sw.WriteLine("      onToString(ref returnString);");
      sw.WriteLine("      return returnString;");
      sw.WriteLine("    }");
      sw.WriteLine("    partial void onToString(ref string returnString);");
    }


    private void writeRawClass(StreamWriter sw, string context, List<string> lines) {
      sw.WriteLine();
      sw.WriteLine();
      sw.WriteLine($"  #region {RawName}");
      sw.WriteLine("  //      " + new string('-', RawName.Length));
      sw.WriteLine();
      sw.WriteLine("  /// <summary>");
      sw.WriteLine($"  /// {RawName} is used instead {ClassName} and {context}.Data to read an instance from a CSV file with ");
      sw.WriteLine($"  /// {ReaderName} or write with {WriterName}.");
      sw.WriteLine("  /// </summary>");
      sw.WriteLine($"  public class {RawName} {{");
      sw.WriteLine();
      sw.WriteLine("    /// <summary>");
      sw.WriteLine($"    /// Unique identifier for {RawName}.");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine("    public int Key { get; set; }");
      foreach (var mi in Members.Values) {
        mi.WriteProperty(sw, isRaw: true);
      }
      if (AreInstancesUpdatable || AreInstancesReleasable) {
        sw.WriteLine();
        sw.WriteLine();
        sw.WriteLine("    /// <summary>");
        sw.WriteLine($"    /// How was {RawName} marked in CSV file (read, update, delete) ? If not read from CSV file, RawState is new.");
        sw.WriteLine("    /// </summary>");
        sw.WriteLine("    public RawStateEnum RawState { get; set; }");
      }

      sw.WriteLine();
      sw.WriteLine();
      sw.WriteLine("    /// <summary>");
      sw.WriteLine("    /// Default Constructor.");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine($"    public {RawName}() {{");
      sw.WriteLine("    }");

      sw.WriteLine();
      sw.WriteLine();
      sw.WriteLine("    /// <summary>");
      sw.WriteLine("    /// Constructor, will replace links to parents with the parents' key.");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine($"    public {RawName}({ClassName} {LowerClassName}) {{");
      sw.WriteLine($"      Key = {LowerClassName}.Key;");
      foreach (var mi in Members.Values) {
        if (mi.MemberType<MemberTypeEnum.LinkToParent) {
          sw.WriteLine($"      {mi.MemberName} = {LowerClassName}.{mi.MemberName};");
        } else if (mi.MemberType==MemberTypeEnum.LinkToParent) {
          sw.WriteLine($"      {mi.MemberName}Key = {LowerClassName}.{mi.MemberName}{mi.QMark}.Key;");
        }
      }
      sw.WriteLine("    }");

      sw.WriteLine();
      sw.WriteLine();
      sw.WriteLine("    /// <summary>");
      sw.WriteLine("    /// Returns all property names and values");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine("    public override string ToString() {");
      sw.WriteLine("      var returnString =");
      lines.Clear();
      lines.Add("        $\"Key: {Key}");
      foreach (var mi in Members.Values) {
        if (mi.MemberType==MemberTypeEnum.LinkToParent) {
          lines.Add($"        $\" {mi.MemberName}Key: {{{mi.MemberName}Key}}");
        } else if (mi.MemberType<MemberTypeEnum.LinkToParent) {
          //simple data type and enum
          lines.Add($"        $\" {mi.MemberName}: {{{mi.MemberName}{mi.ToStringFunc}}}");
        }
      }
      for (int linesIndex = 0; linesIndex < lines.Count; linesIndex++) {
        var line = lines[linesIndex];
        sw.Write(line);
        if (linesIndex+1<lines.Count) {
          sw.WriteLine(",\" +");
        } else {
          sw.WriteLine(";\";");
        }
      }
      sw.WriteLine("      return returnString;");
      sw.WriteLine("    }");
      sw.WriteLine("  }");
      sw.WriteLine("  #endregion");
      sw.WriteLine();
      sw.WriteLine();
    }


    private void writeReaderClass(StreamWriter sw) {
      sw.WriteLine($"  #region {ReaderName}");
      sw.WriteLine("  //      " + new string('-', ReaderName.Length));
      sw.WriteLine();
      sw.WriteLine("  /// <summary>");
      sw.WriteLine($"  /// Reads from a CSV file containing {ClassName} instances. Note that the keys of linked objects will be returned ");
      sw.WriteLine("  /// and not the linked objects themselves, since the data context will not be involved.");
      sw.WriteLine("  /// </summary>");
      sw.WriteLine($"  public class {ReaderName}: IDisposable {{");
      sw.WriteLine();
      sw.WriteLine("    readonly CsvConfig csvConfig;");
      sw.WriteLine("    readonly CsvReader csvReader;");
      if (!AreInstancesReleasable && !AreInstancesUpdatable) {
        sw.WriteLine("    int nextKey = 0;");
      }
      sw.WriteLine();
      sw.WriteLine();
      sw.WriteLine("    /// <summary>");
      sw.WriteLine($"    /// Constructor, will read and verify the {ClassName} header line. You need to dispose {ReaderName} once");
      sw.WriteLine($"    /// you are done, except when you have read the whole file, then {ReaderName}.ReadLine() disposes automatically.");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine($"    public {ReaderName}(string fileNamePath, CsvConfig csvConfig) {{");
      sw.WriteLine("      this.csvConfig = csvConfig;");
      sw.WriteLine($"      csvReader = new CsvReader(fileNamePath, csvConfig, {ClassName}.EstimatedLineLength);");
      sw.WriteLine($"      var csvHeaderString = Csv.ToCsvHeaderString({ClassName}.Headers, csvConfig.Delimiter);");
      sw.WriteLine("      var headerLine = csvReader.ReadLine();");
      sw.WriteLine("      if (csvHeaderString!=headerLine) throw new Exception($\"Error reading file {csvReader.FileName}{Environment.NewLine}'\" +");
      sw.WriteLine("        headerLine + \"' should be '\" + csvHeaderString + \"'.\");");
      sw.WriteLine("    }");
      sw.WriteLine();
      sw.WriteLine();
      sw.WriteLine("    /// <summary>");
      sw.WriteLine($"    /// Reads the details of one {ClassName} from the CSV file. Returns false when all lines are");
      sw.WriteLine($"    /// read and disposes the reader.");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine($"    public bool ReadLine([NotNullWhen(true)] out {RawName}? {LowerRawName}){{");
      sw.WriteLine("      if (csvReader.IsEndOfFileReached()) {");
      sw.WriteLine("        csvReader.Dispose();");
      sw.WriteLine($"        {LowerRawName} = null;");
      sw.WriteLine("        return false;");
      sw.WriteLine("      }");
      sw.WriteLine($"      {LowerRawName} = new {RawName}();");
      if (AreInstancesReleasable || AreInstancesUpdatable) {
        sw.WriteLine($"      var firstLineChar = csvReader.ReadFirstLineChar();");
        sw.WriteLine($"      if (firstLineChar==csvConfig.LineCharAdd) {{");
        sw.WriteLine($"        {LowerRawName}.RawState = RawStateEnum.Read;");
        sw.WriteLine($"      }} else if (firstLineChar==csvConfig.LineCharUpdate) {{");
        sw.WriteLine($"        {LowerRawName}.RawState = RawStateEnum.Updated;");
        sw.WriteLine($"      }} else if (firstLineChar==csvConfig.LineCharDelete) {{");
        sw.WriteLine($"        {LowerRawName}.RawState = RawStateEnum.Deleted;");
        sw.WriteLine($"      }} else {{");
        sw.WriteLine($"        throw new NotSupportedException($\"Illegal first line character '{{firstLineChar}}' found in '{{csvReader.GetPresentContent()}}'.\");");
        sw.WriteLine($"      }}");
        sw.WriteLine($"      {LowerRawName}.Key = csvReader.ReadInt();");
      } else {
        sw.WriteLine($"      {LowerRawName}.Key = nextKey++;");
      }
      foreach (var mi in Members.Values) {
        if (mi.MemberType==MemberTypeEnum.LinkToParent) {
          if (mi.IsNullable) {
            sw.WriteLine($"      {LowerRawName}.{mi.MemberName}Key = csvReader.ReadIntNull();");
          } else {
            sw.WriteLine($"      {LowerRawName}.{mi.MemberName}Key = csvReader.ReadInt();");
          }
        } else if (mi.MemberType==MemberTypeEnum.Enum) {
          sw.WriteLine($"      {LowerRawName}.{mi.MemberName} = ({mi.TypeString})csvReader.{mi.CsvReaderRead};");
        } else if (mi.MemberType<=MemberTypeEnum.LinkToParent) {//simple data type or LinkToParent
          sw.WriteLine($"      {LowerRawName}.{mi.MemberName} = csvReader.{mi.CsvReaderRead};");
        }
      }
      sw.WriteLine("      csvReader.ReadEndOfLine();");
      sw.WriteLine($"      return true;");
      sw.WriteLine("    }");
      sw.WriteLine();
      sw.WriteLine();
      sw.WriteLine("    #region IDisposable Support");
      sw.WriteLine("    //      -------------------");
      sw.WriteLine();
      sw.WriteLine("    /// <summary>");
      sw.WriteLine($"    /// Executes disposal of {ReaderName} exactly once.");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine("    public void Dispose() {");
      sw.WriteLine("      Dispose(true);");
      sw.WriteLine();
      sw.WriteLine("      GC.SuppressFinalize(this);");
      sw.WriteLine("    }");
      sw.WriteLine();
      sw.WriteLine();
      sw.WriteLine("    /// <summary>");
      sw.WriteLine($"    /// Is {ReaderName} already exposed ?");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine("    protected bool IsDisposed {");
      sw.WriteLine("      get { return isDisposed==1; }");
      sw.WriteLine("    }");
      sw.WriteLine();
      sw.WriteLine();
      sw.WriteLine("    int isDisposed = 0;");
      sw.WriteLine();
      sw.WriteLine();
      sw.WriteLine("    /// <summary>");
      sw.WriteLine("    /// Inheritors should call Dispose(false) from their destructor");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine("    protected void Dispose(bool disposing) {");
      sw.WriteLine("      var wasDisposed = Interlocked.Exchange(ref isDisposed, 1);//prevents that 2 threads dispose simultaneously");
      sw.WriteLine("      if (wasDisposed==1) return; // already disposed");
      sw.WriteLine();
      sw.WriteLine("      csvReader.Dispose();");
      sw.WriteLine("    }");
      sw.WriteLine("    #endregion");
      sw.WriteLine("  }");
      sw.WriteLine("  #endregion");
      sw.WriteLine();
      sw.WriteLine();
    }


    private void writeWriterClass(StreamWriter sw, string context, List<string> lines) {
      sw.WriteLine($"  #region {WriterName}");
      sw.WriteLine("  //      " + new string('-', WriterName.Length));
      sw.WriteLine();
      sw.WriteLine("  /// <summary>");
      sw.WriteLine($"  /// Writes a CSV file containing records which can be read back as {ClassName}. Note that the keys of linked objects");
      sw.WriteLine("  /// need to be provided in Write(), since the data context will not be involved.");
      sw.WriteLine("  /// </summary>");
      sw.WriteLine($"  public class {WriterName}: IDisposable {{");
      sw.WriteLine();
      sw.WriteLine("    readonly CsvConfig csvConfig;");
      sw.WriteLine("    readonly CsvWriter csvWriter;");
      if (AreInstancesReleasable || AreInstancesUpdatable) {
        sw.WriteLine("    int lastKey = int.MinValue;");
      } else {
        sw.WriteLine("    int nextKey = 0;");
      }
      sw.WriteLine();
      sw.WriteLine();
      sw.WriteLine("    /// <summary>");
      sw.WriteLine($"    /// Constructor, will write the {ClassName} header line into the CSV file. Dispose {WriterName} once done.");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine($"    public {WriterName}(string fileNamePath, CsvConfig csvConfig){{");
      sw.WriteLine("      this.csvConfig = csvConfig;");
      sw.WriteLine($"      csvWriter = new CsvWriter(fileNamePath, csvConfig, {ClassName}.EstimatedLineLength, null, 0);");
      sw.WriteLine($"      var csvHeaderString = Csv.ToCsvHeaderString({ClassName}.Headers, csvConfig.Delimiter);");
      sw.WriteLine("      csvWriter.WriteLine(csvHeaderString);");
      sw.WriteLine("    }");

      sw.WriteLine();
      sw.WriteLine();
      sw.WriteLine("    /// <summary>");
      sw.WriteLine($"    /// Writes the details of one {RawName} to the CSV file");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine($"    public void Write({RawName} {LowerRawName}){{");
      if (AreInstancesReleasable || AreInstancesUpdatable) {
        sw.WriteLine($"      if ({LowerRawName}.Key<0) {{");
        sw.WriteLine($"        throw new Exception($\"{RawName}'s key {{{LowerRawName}.Key}} needs to be greater equal 0.\");");
        sw.WriteLine("      }");
        sw.WriteLine($"      if ({LowerRawName}.Key<=lastKey) {{");
        sw.WriteLine($"        throw new Exception($\"{RawName}'s key {{{LowerRawName}.Key}} must be greater than the last written {ClassName}'s key {{lastKey}}.\");");
        sw.WriteLine("      }");
        sw.WriteLine($"      lastKey = {LowerRawName}.Key;");
        sw.WriteLine("      csvWriter.WriteFirstLineChar(csvConfig.LineCharAdd);");
        sw.WriteLine($"      csvWriter.Write({LowerRawName}.Key);");
      } else {
        sw.WriteLine($"      if ({LowerRawName}.Key!=nextKey) {{");
        sw.WriteLine($"        throw new Exception($\"{RawName}'s key {{{LowerRawName}.Key}} should be {{nextKey}}.\");");
        sw.WriteLine("      }");
        sw.WriteLine("      nextKey++;");
        sw.WriteLine("      csvWriter.StartNewLine();");
      }
      foreach (var mi in Members.Values) {
        if (mi.MemberType==MemberTypeEnum.LinkToParent) {
          if (mi.IsNullable) {
            sw.WriteLine($"      if ({LowerRawName}.{mi.MemberName}Key is null) {{");
            sw.WriteLine("        csvWriter.WriteNull();");
            sw.WriteLine("      } else {");
            sw.WriteLine($"        if ({LowerRawName}.{mi.MemberName}Key<0) throw new Exception($\"Cannot write" +
              $" {LowerClassName} to CSV File, because {mi.MemberName} is not stored in {context}.Data.{mi.ParentClassInfo!.PluralName}.\");");
            sw.WriteLine();
            sw.WriteLine($"        csvWriter.Write({LowerRawName}.{mi.MemberName}Key.ToString());");
            sw.WriteLine("      }");
          } else {
            sw.WriteLine($"      if ({LowerRawName}.{mi.MemberName}Key<0) throw new Exception($\"Cannot write {LowerClassName}" +
              $" to CSV File, because {mi.MemberName} is not stored in {context}.Data.{mi.ParentClassInfo!.PluralName}.\");");
            sw.WriteLine();
            sw.WriteLine($"      csvWriter.Write({LowerRawName}.{mi.MemberName}Key.ToString());");
          }
        } else if (mi.MemberType==MemberTypeEnum.Enum) {
          sw.WriteLine($"      csvWriter.{mi.CsvWriterWrite}((int){LowerRawName}.{mi.MemberName});");
        } else if (mi.MemberType<MemberTypeEnum.Enum) {//simple data
          sw.WriteLine($"      csvWriter.{mi.CsvWriterWrite}({LowerRawName}.{mi.MemberName});");
        }
      }
      sw.WriteLine("      csvWriter.WriteEndOfLine();");
      sw.WriteLine("    }");

      sw.WriteLine();
      sw.WriteLine();
      sw.WriteLine("    /// <summary>");
      sw.WriteLine($"    /// Writes the details of one {ClassName} to the CSV file");
      sw.WriteLine("    /// </summary>");
      sw.Write("    public void Write(");
      writeParameters(sw, lines, isConstructor: true, isWriterWrite: true);
      if (AreInstancesReleasable || AreInstancesUpdatable) {
        sw.WriteLine("      if (key<0) {");
        sw.WriteLine($"        throw new Exception($\"{ClassName}'s key {{key}} needs to be greater equal 0.\");");
        sw.WriteLine("      }");
        sw.WriteLine("      if (key<=lastKey) {");
        sw.WriteLine($"        throw new Exception($\"{ClassName}'s key {{key}} must be greater than the last written {ClassName}'s key {{lastKey}}.\");");
        sw.WriteLine("      }");
        sw.WriteLine("      lastKey = key;");
        sw.WriteLine("      csvWriter.WriteFirstLineChar(csvConfig.LineCharAdd);");
        sw.WriteLine("      csvWriter.Write(key);");
      } else {
        sw.WriteLine("      if (key!=nextKey) {");
        sw.WriteLine($"        throw new Exception($\"{ClassName}'s key {{key}} should be {{nextKey}}.\");");
        sw.WriteLine("      }");
        sw.WriteLine("      nextKey++;");
        sw.WriteLine("      csvWriter.StartNewLine();");
      }
      foreach (var mi in Members.Values) {
        if (mi.MemberType==MemberTypeEnum.LinkToParent) {
          if (mi.IsNullable) {
            sw.WriteLine($"      if ({mi.LowerMemberName}Key is null) {{");
            sw.WriteLine("        csvWriter.WriteNull();");
            sw.WriteLine("      } else {");
            sw.WriteLine($"        if ({mi.LowerMemberName}Key<0) throw new Exception($\"Cannot write" +
              $" {LowerClassName} to CSV File, because {mi.MemberName} is not stored in {context}.Data.{mi.ParentClassInfo!.PluralName}.\");");
            sw.WriteLine();
            sw.WriteLine($"        csvWriter.Write({mi.LowerMemberName}Key.ToString());");
            sw.WriteLine("      }");
          } else {
            sw.WriteLine($"      if ({mi.LowerMemberName}Key<0) throw new Exception($\"Cannot write {LowerClassName}" +
              $" to CSV File, because {mi.MemberName} is not stored in {context}.Data.{mi.ParentClassInfo!.PluralName}.\");");
            sw.WriteLine();
            sw.WriteLine($"      csvWriter.Write({mi.LowerMemberName}Key.ToString());");
          }
        } else if (mi.MemberType==MemberTypeEnum.Enum) {
          sw.WriteLine($"      csvWriter.{mi.CsvWriterWrite}((int){mi.LowerMemberName});");
        } else if (mi.MemberType<MemberTypeEnum.Enum) {//simple data
          sw.WriteLine($"      csvWriter.{mi.CsvWriterWrite}({mi.LowerMemberName});");
        }
      }
      sw.WriteLine("      csvWriter.WriteEndOfLine();");
      sw.WriteLine("    }");
      sw.WriteLine();
      sw.WriteLine();
      sw.WriteLine("    #region IDisposable Support");
      sw.WriteLine("    //      -------------------");
      sw.WriteLine();
      sw.WriteLine("    /// <summary>");
      sw.WriteLine($"    /// Executes disposal of {WriterName} exactly once.");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine("    public void Dispose() {");
      sw.WriteLine("      Dispose(true);");
      sw.WriteLine();
      sw.WriteLine("      GC.SuppressFinalize(this);");
      sw.WriteLine("    }");
      sw.WriteLine();
      sw.WriteLine();
      sw.WriteLine("    /// <summary>");
      sw.WriteLine($"    /// Is {WriterName} already exposed ?");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine("    protected bool IsDisposed {");
      sw.WriteLine("      get { return isDisposed==1; }");
      sw.WriteLine("    }");
      sw.WriteLine();
      sw.WriteLine();
      sw.WriteLine("    int isDisposed = 0;");
      sw.WriteLine();
      sw.WriteLine();
      sw.WriteLine("    /// <summary>");
      sw.WriteLine("    /// Inheritors should call Dispose(false) from their destructor");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine("    protected void Dispose(bool disposing) {");
      sw.WriteLine("      var wasDisposed = Interlocked.Exchange(ref isDisposed, 1);//prevents that 2 threads dispose simultaneously");
      sw.WriteLine("      if (wasDisposed==1) return; // already disposed");
      sw.WriteLine();
      sw.WriteLine("      csvWriter.Dispose();");
      sw.WriteLine("    }");
      sw.WriteLine("    #endregion");
      sw.WriteLine("  }");
      sw.WriteLine("  #endregion");
    }
    #endregion


    enum needsDictEnum {
      //add only
      Contructor,
      Store, //addIndent: ""
      RollbackRelease, //isCSV: true

      //both
      Update, //isUpdate: true
      UpdateCsv, //isCSV: true
      RollbackUpdate, //isCSV: true, isRollbackUpdate: true

      //remove only
      Release, //
      Disconnect, //
      RollbackStore, //isCSV: true
    }


    private void writeNeedsDictionaryAddStatement(
      StreamWriter sw, 
      MemberInfo memberInfo, 
      string context,
      needsDictEnum needsDict) 
    {
      //The code here covers 2 situation of NeedsDictionary:
      //1) A member needs directory and is not involved with ToLower
      //2) A member is the source for ToLower and the ToLower target needs a directory
      //Compiler prevents that ToLower source and target each can require their own directory at the same time.
      MemberInfo? mi = null;
      if (memberInfo.NeedsDictionary) {
        mi = memberInfo;
      } else if (memberInfo.ToLowerTarget!=null && memberInfo.ToLowerTarget.NeedsDictionary) {
        mi = memberInfo.ToLowerTarget;
      }

      if (mi!=null) {
        var className = "";
        var classNameDot = "";
        if (needsDict==needsDictEnum.RollbackUpdate) {
          className = "item";
          classNameDot = className + ".";
        } else if (needsDict==needsDictEnum.RollbackRelease || needsDict==needsDictEnum.UpdateCsv) {
          className = memberInfo.ClassInfo.LowerClassName;
          classNameDot = className + ".";
        }
        var indent = needsDict==needsDictEnum.UpdateCsv ? "        " : "      ";
        if (needsDict==needsDictEnum.Update) {
          sw.WriteLine($"        if ({classNameDot}Key>=0) {{");
          indent += "   ";
        }
        if (mi.IsNullable) {
          sw.WriteLine($"{indent}if ({classNameDot}{mi.MemberName}!=null) {{");
          if (
            needsDict==needsDictEnum.RollbackRelease ||
            needsDict==needsDictEnum.UpdateCsv ||
            needsDict==needsDictEnum.RollbackUpdate) 
          {
            sw.WriteLine($"{indent}  {context}.Data._{mi.ClassInfo.PluralName}By{mi.MemberName}.Add({classNameDot}"+
              $"{mi.MemberName}, {className});");
          } else {
            sw.WriteLine($"{indent}  {context}.Data._{mi.ClassInfo.PluralName}By{mi.MemberName}.Add({mi.MemberName}, this);");
          }
          sw.WriteLine($"{indent}}}");
        } else {
          if (
            needsDict==needsDictEnum.RollbackRelease ||
            needsDict==needsDictEnum.UpdateCsv ||
            needsDict==needsDictEnum.RollbackUpdate) 
          {
            sw.WriteLine($"{indent}{context}.Data._{mi.ClassInfo.PluralName}By{mi.MemberName}.Add({classNameDot}" +
              $"{mi.MemberName}, {className});");
          } else {
            sw.WriteLine($"{indent}{context}.Data._{mi.ClassInfo.PluralName}By{mi.MemberName}.Add({mi.MemberName}, this);");
          }
        }
        if (needsDict==needsDictEnum.Update) {
          sw.WriteLine("        }");
        }
      }
    }


    //enum needsDictEnum {
    //  Update, //isUpdate: true
    //  UpdateCsv, //isCSV: true
    //  Release, //
    //  Diconnect, //
    //  RollbackStore, //isCSV: true
    //  RollbackUpdate, //isCSV: true, isRollbackUpdate: true
    //}


    private void writeNeedsDictionaryRemoveStatement(
      StreamWriter sw, 
      MemberInfo memberInfo, 
      string context,
      needsDictEnum needsDict) 
    {
      //The code here covers 2 situation of NeedsDictionary:
      //1) A member needs directory and is not involved with ToLower
      //2) A member is the source for ToLower and the ToLower target needs a directory
      //Compiler prevents that ToLower source and target each can require their own directory at the same time.
      MemberInfo? mi = null;
      if (memberInfo.NeedsDictionary) {
        mi = memberInfo;
      } else if (memberInfo.ToLowerTarget!=null && memberInfo.ToLowerTarget.NeedsDictionary) {
        mi = memberInfo.ToLowerTarget;
      }

      if (mi!=null) {
        var classNameDot = "";
        if (needsDict==needsDictEnum.RollbackUpdate) {
          classNameDot = "item.";
        } else if(
          needsDict==needsDictEnum.UpdateCsv || 
          needsDict==needsDictEnum.RollbackStore || 
          needsDict==needsDictEnum.Disconnect) 
        {
          classNameDot = memberInfo.ClassInfo.LowerClassName + ".";
        }
        var indent = needsDict==needsDictEnum.UpdateCsv ? "        " : "      ";
        if (needsDict==needsDictEnum.Update) {
          sw.WriteLine($"        if ({classNameDot}Key>=0) {{");
          indent = "            ";
        }
        if (mi.IsNullable) {
          sw.WriteLine($"{indent}if ({classNameDot}{mi.MemberName}!=null) {{");
          sw.WriteLine($"{indent}  {context}.Data._{mi.ClassInfo.PluralName}By{mi.MemberName}.Remove({classNameDot}"+
            $"{mi.MemberName});");
          sw.WriteLine($"{indent}}}");
        } else {
          sw.WriteLine($"{indent}{context}.Data._{mi.ClassInfo.PluralName}By{mi.MemberName}.Remove({classNameDot}" +
            $"{mi.MemberName});");
        }
        if (needsDict==needsDictEnum.Update) {
          sw.WriteLine("        }");
        }
      }
    }


    private void writeToLowerCopyStatement(
      StreamWriter sw, 
      MemberInfo memberInfo,
      needsDictEnum needsDict) 
    {
      if (memberInfo.ToLowerTarget is null) return;
      
      var mi = memberInfo.ToLowerTarget;
      var classNameDot = "";
      if (needsDict==needsDictEnum.RollbackUpdate) {
        classNameDot = "item.";
      } else if (needsDict==needsDictEnum.UpdateCsv) {
        classNameDot = memberInfo.ClassInfo.LowerClassName + ".";
      }
      var indent = needsDict==needsDictEnum.Update || needsDict==needsDictEnum.UpdateCsv ? "  " : "";
      sw.WriteLine($"      {indent}{classNameDot}{mi.MemberName} = {classNameDot}{mi.PropertyForToLower}{mi.QMark}.ToLowerInvariant();");
    }


    private bool writeParameters(StreamWriter streamWriter, List<string> lines, bool isConstructor, bool isWriterWrite = false) {
      if (!isConstructor && isWriterWrite) throw new Exception("Not supported combination.");

      lines.Clear();
      if (isWriterWrite) {
        lines.Add("int key");
      }
      foreach (var mi in Members.Values) {
        if (mi.MemberType==MemberTypeEnum.ToLower) continue;

        if (mi.MemberType<=MemberTypeEnum.LinkToParent && //simple data type and LinkToParent
          (isConstructor || !mi.IsReadOnly))
        {
          string part;
          if (isWriterWrite && mi.ParentClassInfo!=null) {
            part = $"int{mi.QMark} {mi.LowerMemberName}Key";
          } else {
            part = $"{mi.TypeString} {mi.LowerMemberName}";
            if (isConstructor && mi.DefaultValue!=null) {
              if (mi.MemberType==MemberTypeEnum.String) {
                part += $" = \"{mi.DefaultValue}\"";
              } else {
                part += $" = {mi.DefaultValue}";
              }
            }
          }
          lines.Add(part);
        }
      }
      if (isConstructor && !isWriterWrite) {
        lines.Add("bool isStoring = true");
      }
      if (lines.Count==0) return false;//update should only be created if it has parameters.

      writeCode(streamWriter, lines);
      return true;
    }


    private void writeCode(StreamWriter streamWriter, List<string> parts, bool isStatement = false) {
      var isNewLines = parts.Count>4;
      for (int partsIndex = 0; partsIndex < parts.Count; partsIndex++) {
        if (isNewLines) {
          streamWriter.WriteLine();
          streamWriter.Write("      ");
        }
        streamWriter.Write(parts[partsIndex]);
        if (partsIndex+1!=parts.Count) {
          streamWriter.Write(", ");
        }
      }
      if (isStatement) {
        streamWriter.Write(");");
      } else {
        streamWriter.Write(")");
        if (isNewLines) {
          streamWriter.WriteLine();
          streamWriter.Write("   ");
        }
        streamWriter.WriteLine(" {");
      }
    }


    enum updateTypeEnum {
      Call,
      Definition,
      Implementation
    }


    private bool writeOnUpdateParameters(StreamWriter streamWriter, updateTypeEnum updateType, string? comment = null) {
      var parts = new List<string>();
      foreach (var mi in Members.Values) {
        if (mi.MemberType==MemberTypeEnum.ToLower) continue;

        if (mi.MemberType<=MemberTypeEnum.LinkToParent && //simple data type and LinkToParent
          (!mi.IsReadOnly)) 
        {
          var part = "";
          if (updateType==updateTypeEnum.Definition || updateType==updateTypeEnum.Implementation) {
            part = $"{mi.TypeString} ";
          }
          part += $"{mi.LowerMemberName}";
          parts.Add(part);
        }
      }
      if (parts.Count==0) return false;//update should only be created if it has parameters.

      var lastPart = "ref ";
      if (updateType==updateTypeEnum.Definition || updateType==updateTypeEnum.Implementation) {
        lastPart += "bool ";
      }
      lastPart += "isCancelled";
      parts.Add(lastPart);

      bool isNewLines;
      if (updateType==updateTypeEnum.Call) {
        isNewLines = parts.Count>7;
      } else {
        isNewLines = parts.Count>4;
      }
      for (int partsIndex = 0; partsIndex < parts.Count; partsIndex++) {
        if (isNewLines) {
          streamWriter.WriteLine();
          if (updateType==updateTypeEnum.Call) {
            streamWriter.Write("        ");
          } else {
            streamWriter.Write("      ");
          }
          if (comment!=null) {
            streamWriter.Write(comment);
          }
        }
        streamWriter.Write(parts[partsIndex]);
        if (partsIndex+1!=parts.Count) {
          streamWriter.Write(", ");
        }
      }
      switch (updateType) {
      case updateTypeEnum.Call:
      case updateTypeEnum.Definition:
        streamWriter.WriteLine(");");
        break;
      case updateTypeEnum.Implementation:
        streamWriter.Write(")");
        if (isNewLines) {
          streamWriter.WriteLine();
          streamWriter.Write("   ");
          if (comment!=null) {
            streamWriter.Write(comment);
          }
        }
        streamWriter.WriteLine("{");
        streamWriter.WriteLine($"   {comment}}}");
        break;
      default:
        throw new NotSupportedException();
      }
      return true;
    }


    private bool writeRemoveComment(StreamWriter streamWriter, string context, bool isCapitaliseFirstLetter) {
      var lineParts = new List<string>();
      foreach (var mi in Members.Values) {
        if  (mi.MemberType==MemberTypeEnum.LinkToParent) {
          lineParts.Add($"disconnects {ClassName} from {mi.TypeStringNotNullable} because of {mi.MemberName}");
        } else if (mi.NeedsDictionary) {
          lineParts.Add($"removes {ClassName} from {context}.Data.{mi.ClassInfo.PluralName}By{mi.MemberName}");
        }
      }

      if (lineParts.Count>0) {
        //var partsCount = 0;
        if (isCapitaliseFirstLetter) {
          lineParts[0] = lineParts[0][0..1].ToUpperInvariant() + lineParts[0][1..];
        }
        for (int linePartIndex = 0; linePartIndex < lineParts.Count; linePartIndex++) {
          var linePart = lineParts[linePartIndex];
          if (linePartIndex + 1 == lineParts.Count) {
            if (!isCapitaliseFirstLetter || linePartIndex!=0) {
              streamWriter.Write(" and ");
            }
          } else {
            if (!isCapitaliseFirstLetter || linePartIndex!=0) {
              streamWriter.Write(", ");
            }
          }
          if (linePartIndex>0 || !isCapitaliseFirstLetter) {
            streamWriter.WriteLine();
            streamWriter.Write("    /// ");
          }
          streamWriter.Write(linePart);
        }
        return true;
      } else {
        return false;
      }
    }
  }
}
