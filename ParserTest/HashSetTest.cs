using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace ParserTest {

  [TestClass]
  public class HashSetTest {

    [TestMethod]
    public void TestHashSet() {

      //2 child properties linke to the same parent list
      //================================================
      var compiler = Generator.Analyze(@"  
        public class Parent {
          public List<Child> Children;
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

      //Parent refereces explicitely 1 of the 2 properties in the child
      //---------------------------------------------------------------
      Generator.Analyze(@"  
        public class Parent {
        [StorageProperty(childPropertyName: ""Parent1"")]
        public List<Child> Children;
        }

        public class Child {
          public Parent Parent1;
          public Parent Parent2;
        }
      ",
      @"Class: Child
        Property: public Parent Parent2;
        Parent class: Parent
        Parent property: [StorageProperty(childPropertyName: ""Parent1"")]
                public List<Child> Children;
        Property Parent2 in child class Child references property Children in parent class Parent, which links " +
        "explicitely to Parent1 in class Child. Remove StoragePropertyAttribute.childPropertyName from Children " +
        "if more than 1 property in child class Child should reference Parent.Children, in which case the List<> " +
        "will get replaced by a HashSet<> in the generated code.");


      //3 child properties linke to the same parent list
      //================================================
          compiler = Generator.Analyze(@"  
        public class Parent {
          public List<Child> Children;
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
