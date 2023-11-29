/**************************************************************************************

StorageLib.MemberTypeEnum
=========================

Data types supported by StorageLib compiler

Written in 2020-21 by Jürgpeter Huber 
Contact: https://github.com/PeterHuberSg/StorageLib

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/
namespace StorageLib {

  /// <summary>
  /// Data types supported by StorageLib compiler
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
    ParentMultipleChildrenHashSet, //property of parent being a HashSet<TValue> of children
    ParentMultipleChildrenDictionary, //property of parent being a Dictionary<TKey, TValue> of children
    ParentMultipleChildrenSortedList, //property of parent being a SortedList<TKey, TValue> of children
    ParentMultipleChildrenSortedBucket, //property of parent being a SortedBucketCollection<TKey1, TKey2, TValue> of children

    Lenght
  }
}
