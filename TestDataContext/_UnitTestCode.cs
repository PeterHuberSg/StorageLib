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
    public int CountAllChildren => children.CountAll;
    public IEnumerable<ListChild> GetAllChildren => children.GetAll();
    public ListChild? AllChildrenFirst => children.FirstOrDefault();
  }
  partial class ListParentN: ICollectionParent<ListParent, ListParentN, ListParentR, ListParentNR, ListChild> {
    public int CountAllChildren => children.CountAll;
    public IEnumerable<ListChild> GetAllChildren => children.GetAll();
    public ListChild? AllChildrenFirst => children.FirstOrDefault();
  }
  partial class ListParentR: ICollectionParent<ListParent, ListParentN, ListParentR, ListParentNR, ListChild> {
    public int CountAllChildren => children.CountAll;
    public IEnumerable<ListChild> GetAllChildren => children.GetAll();
    public ListChild? AllChildrenFirst => children.FirstOrDefault();
  }
  partial class ListParentNR: ICollectionParent<ListParent, ListParentN, ListParentR, ListParentNR, ListChild> {
    public int CountAllChildren => children.CountAll;
    public IEnumerable<ListChild> GetAllChildren => children.GetAll();
    public ListChild? AllChildrenFirst => children.FirstOrDefault();
  }
  partial class ListChild: ITestChild<ListParent, ListParentN, ListParentR, ListParentNR> { }


  partial class DictionaryParent: ICollectionParent<DictionaryParent, DictionaryParentN, DictionaryParentR, DictionaryParentNR, DictionaryChild> {
    public int CountAllChildren => dictionaryChildren.CountAll;
    public IEnumerable<DictionaryChild> GetAllChildren {
      get{
        foreach (var item in dictionaryChildren.GetAll()) {
          yield return item.Value;
        }
      }
    }
    public DictionaryChild? AllChildrenFirst => dictionaryChildren.FirstOrDefault().Value;
  }
  partial class DictionaryParentN: ICollectionParent<DictionaryParent, DictionaryParentN, DictionaryParentR, DictionaryParentNR, DictionaryChild> {
    public int CountAllChildren => dictionaryChildren.CountAll;
    public IEnumerable<DictionaryChild> GetAllChildren {
      get {
        foreach (var item in dictionaryChildren.GetAll()) {
          yield return item.Value;
        }
      }
    }
    public DictionaryChild? AllChildrenFirst => dictionaryChildren.FirstOrDefault().Value;
  }
  partial class DictionaryParentR: ICollectionParent<DictionaryParent, DictionaryParentN, DictionaryParentR, DictionaryParentNR, DictionaryChild> {
    public int CountAllChildren => dictionaryChildren.CountAll;
    public IEnumerable<DictionaryChild> GetAllChildren {
      get {
        foreach (var item in dictionaryChildren.GetAll()) {
          yield return item.Value;
        }
      }
    }
    public DictionaryChild? AllChildrenFirst => dictionaryChildren.FirstOrDefault().Value;
  }
  partial class DictionaryParentNR: ICollectionParent<DictionaryParent, DictionaryParentN, DictionaryParentR, DictionaryParentNR, DictionaryChild> {
    public int CountAllChildren => dictionaryChildren.CountAll;
    public IEnumerable<DictionaryChild> GetAllChildren {
      get {
        foreach (var item in dictionaryChildren.GetAll()) {
          yield return item.Value;
        }
      }
    }
    public DictionaryChild? AllChildrenFirst => dictionaryChildren.FirstOrDefault().Value;
  }
  partial class DictionaryChild: ITestChild<DictionaryParent, DictionaryParentN, DictionaryParentR, DictionaryParentNR> { }


  partial class SortedListParent: ICollectionParent<SortedListParent, SortedListParentN, SortedListParentR, SortedListParentNR, SortedListChild> {
    public int CountAllChildren => sortedListChildren.CountAll;
    public IEnumerable<SortedListChild> GetAllChildren {
      get {
        foreach (var item in sortedListChildren.GetAll()) {
          yield return item.Value;
        }
      }
    }
    public SortedListChild? AllChildrenFirst => sortedListChildren.FirstOrDefault().Value;
  }
  partial class SortedListParentN: ICollectionParent<SortedListParent, SortedListParentN, SortedListParentR, SortedListParentNR, SortedListChild> {
    public int CountAllChildren => sortedListChildren.CountAll;
    public IEnumerable<SortedListChild> GetAllChildren {
      get {
        foreach (var item in sortedListChildren.GetAll()) {
          yield return item.Value;
        }
      }
    }
    public SortedListChild? AllChildrenFirst => sortedListChildren.FirstOrDefault().Value;
  }
  partial class SortedListParentR: ICollectionParent<SortedListParent, SortedListParentN, SortedListParentR, SortedListParentNR, SortedListChild> {
    public int CountAllChildren => sortedListChildren.CountAll;
    public IEnumerable<SortedListChild> GetAllChildren {
      get {
        foreach (var item in sortedListChildren.GetAll()) {
          yield return item.Value;
        }
      }
    }
    public SortedListChild? AllChildrenFirst => sortedListChildren.FirstOrDefault().Value;
  }
  partial class SortedListParentNR: ICollectionParent<SortedListParent, SortedListParentN, SortedListParentR, SortedListParentNR, SortedListChild> {
    public int CountAllChildren => sortedListChildren.CountAll;
    public IEnumerable<SortedListChild> GetAllChildren {
      get {
        foreach (var item in sortedListChildren.GetAll()) {
          yield return item.Value;
        }
      }
    }
    public SortedListChild? AllChildrenFirst => sortedListChildren.FirstOrDefault().Value;
  }
  partial class SortedListChild: ITestChild<SortedListParent, SortedListParentN, SortedListParentR, SortedListParentNR> { }


}
