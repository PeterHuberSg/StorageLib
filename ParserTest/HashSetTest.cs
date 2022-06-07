using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace ParserTest {

  [TestClass]
  public class HashSetTest {

    [TestMethod]
    public void TestHashSet() {

      //2 child properties link to the same parent HashSet
      //==================================================
      var compiler = Generator.Analyze(@"  
        public class Parent {
          public HashSet<Child> Children;
        }

        public class Child {
          public Parent Parent1;
          public Parent Parent2;
        }
      ");
      var parentChildren = compiler!.Classes["Parent"].Members["Children"];
      Assert.IsNull(parentChildren.SingleChildMI);
      Assert.IsTrue(parentChildren.MultipleChildrenMIs!.Contains(compiler!.Classes["Child"].Members["Parent1"]));
      Assert.IsTrue(parentChildren.MultipleChildrenMIs!.Contains(compiler!.Classes["Child"].Members["Parent2"]));
      Assert.AreEqual("Children", compiler!.Classes["Child"].Members["Parent1"].ParentMemberInfo!.MemberName);
      Assert.AreEqual("Children", compiler!.Classes["Child"].Members["Parent2"].ParentMemberInfo!.MemberName);

      //unknown child class
      //-------------------
      Generator.Analyze(@"  
        public class Parent {
          public HashSet<Child1> Children;
        }

        public class Child {
          public Parent Parent1;
          public Parent Parent2;
        }
      ",
      @"Class: Parent
        Property: public HashSet<Child1> Children;
        Cannot find class Child1.");

      //missing parent property in child class
      //--------------------------------------
      Generator.Analyze(@"  
        public class Parent {
          public HashSet<Child> Children;
        }

        public class Child {
        }
      ",
      @"Class: Parent
        Property: public HashSet<Child> Children;
        Children references the class Child. A corresponding property with type Parent is missing in Child.");

      //Child has only 1 property referencing HashSet in Parent
      //-------------------------------------------------------
      Generator.Analyze(@"  
        public class Parent {
          public HashSet<Child> Children;
        }

        public class Child {
          public Parent Parent1;
        }
      ",
      @"Class: Parent
        Property: public HashSet<Child> Children;
        Children is of type HashSet, which is used when the child class hass several properties linking to the parent " +
        "class. But the child class Child has only the property Parent1 with they type Parent. With only one child " +
        "class property linking to parent class use List<> instead of HashSet<>.");

      //Parent HashSet references explicitely a property in the child
      //-------------------------------------------------------------
      Generator.Analyze(@"  
        public class Parent {
        [StorageProperty(childPropertyName: ""Parent1"")]
          public HashSet<Child> Children;
        }

        public class Child {
          public Parent Parent1;
          public Parent Parent2;
        }
      ",
      @"Class: Parent
        Property: [StorageProperty(childPropertyName: ""Parent1"")]
                  public HashSet<Child> Children;
        Remove StorageProperty attribute from Parent.Children which is a HashSet. A HashSet links to every property " +
        "which has the type Parent in child class Child.");


      //3 child properties linke to the same parent HashSet
      //===================================================
      compiler = Generator.Analyze(@"  
        public class Parent {
          public HashSet<Child> Children;
        }

        public class Child {
          public Parent Parent1;
          public Parent Parent2;
          public Parent Parent3;
        }
      ");
      parentChildren = compiler!.Classes["Parent"].Members["Children"];
      Assert.IsNull(parentChildren.SingleChildMI);
      Assert.IsTrue(parentChildren.MultipleChildrenMIs!.Contains(compiler!.Classes["Child"].Members["Parent1"]));
      Assert.IsTrue(parentChildren.MultipleChildrenMIs!.Contains(compiler!.Classes["Child"].Members["Parent2"]));
      Assert.IsTrue(parentChildren.MultipleChildrenMIs!.Contains(compiler!.Classes["Child"].Members["Parent3"]));
      Assert.AreEqual("Children", compiler!.Classes["Child"].Members["Parent1"].ParentMemberInfo!.MemberName);
      Assert.AreEqual("Children", compiler!.Classes["Child"].Members["Parent2"].ParentMemberInfo!.MemberName);
      Assert.AreEqual("Children", compiler!.Classes["Child"].Members["Parent3"].ParentMemberInfo!.MemberName);
    }
  }
}
