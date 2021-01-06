using System;
using System.Collections.Generic;
using System.Text;

namespace StorageLib {

  /// <summary>
  /// Data types supported by Storage compiler
  /// </summary>
  public enum MemberTypeEnum {
    Undefined = 0,
    Date,
    Time,
    DateMinutes,
    DateSeconds,
    DateTimeTicks,
    TimeSpanTicks,
    Decimal,
    Decimal2,
    Decimal4,
    Decimal5,
    Bool,
    Int,
    Long,
    Char,
    String,
    ToLower, //lower case copy of another string property
    Enum,
    //---- add new simple types before List ----
    LinkToParent, //member of child linking to parent, but parent not linking to child
    ParentOneChild, // parent might have 1 child
    ParentMultipleChildrenList, //property of parent being a List<TValue> of children
    ParentMultipleChildrenDictionary, //property of parent being a Dictionary<TKey, TValue> of children
    ParentMultipleChildrenSortedList, //property of parent being a SortedList<TKey, TValue> of children
    Lenght
  }


  /// <summary>
  /// Collection types supported by Storage compiler
  /// </summary>
  public enum ParentCollectionTypeEnum{
    Undefined = 0,
    List,
    Dictionary,
    SortedList,
    Lenght
  }


  ////extension methods for MemberTypeEnum
  //public static class MemberTypeExtensions {


  //  static readonly string?[] baseTypeStrings = {
  //  null, //Undefined
  //  "DateTime", //Date,
  //  "DateTime", //Time,
  //  "DateTime", //DateMinutes,
  //  "DateTime", //DateSeconds,
  //  "DateTime", //DateTime,
  //  "Decimal", //Decimal,
  //  "Decimal", //Decimal2,
  //  "Decimal", //Decimal4,
  //  "Decimal", //Decimal5,
  //  "bool", //Bool,
  //  "int", //Int,
  //  null, //List,
  //  null, //Dictionary,
  //  null, //Parent,
  //  "string", //String,
  //  null, //Enum,
  //  };


  //  /// <summary>
  //  /// Converts a specialised CSV type to a .NET type, like Decimal2 to Decimal or DateMinutes to DateTime
  //  /// </summary>
  //  public static string? ToBaseType(this MemberTypeEnum memberType) {
  //    return baseTypeStrings[(int)memberType];
  //  }
  //}
}
