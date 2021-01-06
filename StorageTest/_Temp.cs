using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageTest {

  public interface IParent<TChild> {
    public string Name { get; }
    public IReadOnlyList<TChild> Children { get; }
  }

  public interface IChild<TParent> {
    public string Name { get; }
    public TParent Parent { get; }
  }

  public class ParentA: IParent<ChildA> {
    public string Name { get; }
    public IReadOnlyList<ChildA> Children => children;
    private List<ChildA> children;
    public ParentA(DataContext dc, string name) {
      Name = name; 
      children = new List<ChildA>();
      dc.ParentsA.Add(this);
    }
    public void AddChild(ChildA child) { children.Add(child); }
  }

  public class ChildA: IChild<ParentA> {
    public string Name { get; }
    public ParentA Parent { get; private set; }
    public ChildA(DataContext dc, string name, ParentA parent) {
      Name = name;
      Parent = parent;
      parent.AddChild(this);
      dc.ChildrenA.Add(this);
    }
  }

  public class ParentB: IParent<ChildB> {
    public string Name { get; }
    public IReadOnlyList<ChildB> Children => children;
    private List<ChildB> children;
    public ParentB(DataContext dc, string name) { 
      Name = name; 
      children = new List<ChildB>();
      dc.ParentsB.Add(this);
    }
    public void AddChild(ChildB child) { children.Add(child); }
  }

  public class ChildB: IChild<ParentB> {
    public string Name { get; }
    public ParentB Parent { get; private set; }
    public ChildB(DataContext dc, string name, ParentB parent) {
      Name = name;
      Parent = parent;
      parent.AddChild(this);
      dc.ChildrenB.Add(this);
    }
  }

  public class DataContext {
    public IReadOnlyList<IParent<ChildA>> ParentsATest => ParentsA;
    public List<ParentA> ParentsA = new List<ParentA>();
    public IReadOnlyList<IChild<ParentA>> ChildrenATest => ChildrenA;
    public List<ChildA> ChildrenA = new List<ChildA>();
    public IReadOnlyList<IParent<ChildB>> ParentsBTest => ParentsB;
    public List<ParentB> ParentsB = new List<ParentB>();
    public IReadOnlyList<IChild<ParentB>> ChildrenBTest => ChildrenB;
    public List<ChildB> ChildrenB = new List<ChildB>();
  }

  public class Testing {
    public Testing() {
      var dc = new DataContext();
      _=new TestBase<ChildA, ParentA>(dc);
      _=new TestBase<ChildB, ParentB>(dc);
    }
  }

  public class TestBase<TChild, TParent> where TChild: IChild<TParent> where TParent: IParent<TChild> {
    public TestBase(DataContext dc) {
      var parent = createParent(dc, "Parent0");
      var child = createChild(dc, "Parent0", parent);
      if (!parent.Children[0]!.Parent.Equals(parent)) throw new Exception();
      if (parent.Children[0]!.Parent.Name!=parent.Name) throw new Exception();
      //foreach (var parent in dc.ParentsA) {
      //  if (!parent.Children[0]!.Parent.Equals(parent)) throw new Exception();
      //  if (parent.Children[0]!.Parent.Name!=parent.Name) throw new Exception();
      //}
      if (typeof(TParent)==typeof(ParentA)) {
        testDcParents<ParentA, ChildA>((IReadOnlyList<IParent<ChildA>>)dc.ParentsA);
      } else if (typeof(TParent)==typeof(ParentB)) {
        testDcParents<ParentB, ChildB>((IReadOnlyList<IParent<ChildB>>)dc.ParentsB);
      }
    }

    private static IParent<TChild> createParent(DataContext dc, string name) {
      if (typeof(TParent)==typeof(ParentA)) {
        return (IParent<TChild>)new ParentA(dc, name);
      } else if (typeof(TParent)==typeof(ParentB)) {
        return (IParent<TChild>)new ParentB(dc, name);
      }
      throw new NotSupportedException();
    }

    private IChild<TParent> createChild(DataContext dc, string name, IParent<TChild> parent) {
      if (typeof(TChild)==typeof(ChildA)) {
        return (IChild<TParent>)new ChildA(dc, name, (ParentA)parent);
      } else if(typeof(TChild)==typeof(ChildB)) {
        return (IChild<TParent>)new ChildB(dc, name, (ParentB)parent);
      }
      throw new NotSupportedException();
    }


    private void testDcParents<tParent1, tChild1>(IReadOnlyList<IParent<tChild1>> parents) where tChild1 : IChild<tParent1> {
      foreach (var parent in parents) {
        foreach (var child in parent.Children) {
          if (child.Parent!.Equals(parent)) throw new Exception();
        }
      }
    }
  }











  public static class GenericFactory {
    public interface IGeneric<TId> {
      void ProcessEntity(TId id);
    }


    public class ClientEntity: IGeneric<int> // Record with Id that is an int
{
      public void ProcessEntity(int id) {
        Console.WriteLine(id);
        // call 3rd party API with int Id
      }
    }

    public class InvoiceEntity: IGeneric<string> // Record with Id that is a string (guid)
    {
      public void ProcessEntity(string id) {
        Console.WriteLine(id);
        // call 3rd party API with string Id
      }
    }


    public static IGeneric<T> CreateGeneric<T>() {
      if (typeof(T) == typeof(string)) {
        return (IGeneric<T>)new ClientEntity();
      }

      if (typeof(T) == typeof(int)) {
        return (IGeneric<T>)new InvoiceEntity();
      }

      throw new InvalidOperationException();
    }
  }


}
