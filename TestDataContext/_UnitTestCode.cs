using StorageLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TestContext {

  //partial class SimpleParent: ITestSimpleParent<SimpleChild> { }
  //partial class SimpleParentN: ITestSimpleParent<SimpleChild> { }
  //partial class SimpleParentR: ITestSimpleParent<SimpleChild> { }
  //partial class SimpleParentNR: ITestSimpleParent<SimpleChild> { }


  partial class LookupParent: ILookupParent { }
  partial class LookupParentN: ILookupParent { }
  partial class LookupParentR: ILookupParent { }
  partial class LookupParentNR: ILookupParent { }
  partial class LookupChild: ITestChild<LookupParent, LookupParentN, LookupParentR, LookupParentNR> { }


  partial class SingleChildParent: ISingleParent<SingleChildParent, SingleChildParentN, SingleChildParentR, SingleChildParentNR, SingleChildChild> { }
  partial class SingleChildParentN: ISingleParent<SingleChildParent, SingleChildParentN, SingleChildParentR, SingleChildParentNR, SingleChildChild> { }
  partial class SingleChildParentR: ISingleParent<SingleChildParent, SingleChildParentN, SingleChildParentR, SingleChildParentNR, SingleChildChild> { }
  partial class SingleChildParentNR: ISingleParent<SingleChildParent, SingleChildParentN, SingleChildParentR, SingleChildParentNR, SingleChildChild> { }
  partial class SingleChildChild: ITestChild<SingleChildParent, SingleChildParentN, SingleChildParentR, SingleChildParentNR> { }


  partial class ListParent: ICollectionParent<ListParent, ListParentN, ListParentR, ListParentNR, ListChild> {
    public int CountAllChildren => children.Count;
    public IEnumerable<ListChild> GetAllChildren => children;
    public int CountStoredChildren => children.CountStoredItems;
    public IEnumerable<ListChild> GetStoredChildren => children.GetStoredItems();
    public ListChild? AllChildrenFirst => children.FirstOrDefault();
  }
  partial class ListParentN: ICollectionParent<ListParent, ListParentN, ListParentR, ListParentNR, ListChild> {
    public int CountAllChildren => children.Count;
    public IEnumerable<ListChild> GetAllChildren => children;
    public int CountStoredChildren => children.CountStoredItems;
    public IEnumerable<ListChild> GetStoredChildren => children.GetStoredItems();
    public ListChild? AllChildrenFirst => children.FirstOrDefault();
  }
  partial class ListParentR: ICollectionParent<ListParent, ListParentN, ListParentR, ListParentNR, ListChild> {
    public int CountAllChildren => children.Count;
    public IEnumerable<ListChild> GetAllChildren => children;
    public int CountStoredChildren => children.CountStoredItems;
    public IEnumerable<ListChild> GetStoredChildren => children.GetStoredItems();
    public ListChild? AllChildrenFirst => children.FirstOrDefault();
  }
  partial class ListParentNR: ICollectionParent<ListParent, ListParentN, ListParentR, ListParentNR, ListChild> {
    public int CountAllChildren => children.Count;
    public IEnumerable<ListChild> GetAllChildren => children;
    public int CountStoredChildren => children.CountStoredItems;
    public IEnumerable<ListChild> GetStoredChildren => children.GetStoredItems();
    public ListChild? AllChildrenFirst => children.FirstOrDefault();
  }
  partial class ListChild: ITestChild<ListParent, ListParentN, ListParentR, ListParentNR> { }


  partial class DictionaryParent: ICollectionParent<DictionaryParent, DictionaryParentN, DictionaryParentR, DictionaryParentNR, DictionaryChild> {
    public int CountAllChildren => dictionaryChildren.Count;
    public IEnumerable<DictionaryChild> GetAllChildren => dictionaryChildren.GetAllItems();
    public int CountStoredChildren => dictionaryChildren.CountStoredItems;
    public IEnumerable<DictionaryChild> GetStoredChildren => dictionaryChildren.GetStoredItems();
    public DictionaryChild? AllChildrenFirst => dictionaryChildren.FirstOrDefault().Value;
  }
  partial class DictionaryParentN: ICollectionParent<DictionaryParent, DictionaryParentN, DictionaryParentR, DictionaryParentNR, DictionaryChild> {
    public int CountAllChildren => dictionaryChildren.Count;
    public IEnumerable<DictionaryChild> GetAllChildren => dictionaryChildren.GetAllItems();
    public int CountStoredChildren => dictionaryChildren.CountStoredItems;
    public IEnumerable<DictionaryChild> GetStoredChildren => dictionaryChildren.GetStoredItems();
    public DictionaryChild? AllChildrenFirst => dictionaryChildren.FirstOrDefault().Value;
  }
  partial class DictionaryParentR: ICollectionParent<DictionaryParent, DictionaryParentN, DictionaryParentR, DictionaryParentNR, DictionaryChild> {
    public int CountAllChildren => dictionaryChildren.Count;
    public IEnumerable<DictionaryChild> GetAllChildren => dictionaryChildren.GetAllItems();
    public int CountStoredChildren => dictionaryChildren.CountStoredItems;
    public IEnumerable<DictionaryChild> GetStoredChildren => dictionaryChildren.GetStoredItems();
    public DictionaryChild? AllChildrenFirst => dictionaryChildren.FirstOrDefault().Value;
  }
  partial class DictionaryParentNR: ICollectionParent<DictionaryParent, DictionaryParentN, DictionaryParentR, DictionaryParentNR, DictionaryChild> {
    public int CountAllChildren => dictionaryChildren.Count;
    public IEnumerable<DictionaryChild> GetAllChildren => dictionaryChildren.GetAllItems();
    public int CountStoredChildren => dictionaryChildren.CountStoredItems;
    public IEnumerable<DictionaryChild> GetStoredChildren => dictionaryChildren.GetStoredItems();
    public DictionaryChild? AllChildrenFirst => dictionaryChildren.FirstOrDefault().Value;
  }
  partial class DictionaryChild: ITestChild<DictionaryParent, DictionaryParentN, DictionaryParentR, DictionaryParentNR> { }


  partial class SortedListParent: ICollectionParent<SortedListParent, SortedListParentN, SortedListParentR, SortedListParentNR, SortedListChild> {
    public int CountAllChildren => sortedListChildren.Count;
    public IEnumerable<SortedListChild> GetAllChildren => sortedListChildren.GetAllItems();
    public int CountStoredChildren => sortedListChildren.CountStoredItems;
    public IEnumerable<SortedListChild> GetStoredChildren => sortedListChildren.GetStoredItems();
    public SortedListChild? AllChildrenFirst => sortedListChildren.FirstOrDefault().Value;
  }
  partial class SortedListParentN: ICollectionParent<SortedListParent, SortedListParentN, SortedListParentR, SortedListParentNR, SortedListChild> {
    public int CountAllChildren => sortedListChildren.Count;
    public IEnumerable<SortedListChild> GetAllChildren => sortedListChildren.GetAllItems();
    public int CountStoredChildren => sortedListChildren.CountStoredItems;
    public IEnumerable<SortedListChild> GetStoredChildren => sortedListChildren.GetStoredItems();
    public SortedListChild? AllChildrenFirst => sortedListChildren.FirstOrDefault().Value;
  }
  partial class SortedListParentR: ICollectionParent<SortedListParent, SortedListParentN, SortedListParentR, SortedListParentNR, SortedListChild> {
    public int CountAllChildren => sortedListChildren.Count;
    public IEnumerable<SortedListChild> GetAllChildren => sortedListChildren.GetAllItems();
    public int CountStoredChildren => sortedListChildren.CountStoredItems;
    public IEnumerable<SortedListChild> GetStoredChildren => sortedListChildren.GetStoredItems();
    public SortedListChild? AllChildrenFirst => sortedListChildren.FirstOrDefault().Value;
  }
  partial class SortedListParentNR: ICollectionParent<SortedListParent, SortedListParentN, SortedListParentR, SortedListParentNR, SortedListChild> {
    public int CountAllChildren => sortedListChildren.Count;
    public IEnumerable<SortedListChild> GetAllChildren => sortedListChildren.GetAllItems();
    public int CountStoredChildren => sortedListChildren.CountStoredItems;
    public IEnumerable<SortedListChild> GetStoredChildren => sortedListChildren.GetStoredItems();
    public SortedListChild? AllChildrenFirst => sortedListChildren.FirstOrDefault().Value;
  }
  partial class SortedListChild: ITestChild<SortedListParent, SortedListParentN, SortedListParentR, SortedListParentNR> { }


}
