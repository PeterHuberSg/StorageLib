using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StorageLib;


namespace TestContext {

  public interface ITestSimpleParent<TChild> where TChild: IStorageItem{
    string Text { get; }
    IReadOnlyList<TChild> Children { get; }
  }

   public interface ILookupParent: IStorageItem{
    string Text { get; }

    void Update(string text);
  }


  public interface ISingleParent<TParent, TParentN, TParentR, TParentNR, TChild>: IStorageItem
    where TParent : IStorageItem
    where TParentN : IStorageItem
    where TParentR : IStorageItem
    where TParentNR : IStorageItem
    where TChild : ITestChild<TParent, TParentN, TParentR, TParentNR> {
    string Text { get; }
    TChild Child { get; }

    void Update(string text);
  }


  public interface ICollectionParent<TParent, TParentN, TParentR, TParentNR, TChild>: IStorageItem
    where TParent : IStorageItem
    where TParentN : IStorageItem
    where TParentR : IStorageItem
    where TParentNR : IStorageItem
    where TChild : ITestChild<TParent, TParentN, TParentR, TParentNR> 
  {
    string Text { get; }
    int CountAllChildren { get; }
    IEnumerable<TChild> GetAllChildren { get; }
    TChild? AllChildrenFirst { get; }
    void Update(string text);

    void Release();
  }


  public interface ITestChild<TParent, TParentN, TParentR, TParentNR>: IStorageItem
    where TParent: IStorageItem
    where TParentN: IStorageItem
    where TParentR: IStorageItem
    where TParentNR: IStorageItem
  {
    string Text { get; }
    TParent Parent { get; }
    TParentN? ParentN { get; }
    TParentR ParentR { get; }
    TParentNR? ParentNR { get; }

    void Update(string text, TParent parent, TParentN? parentN);

    void Release();
  }
}
