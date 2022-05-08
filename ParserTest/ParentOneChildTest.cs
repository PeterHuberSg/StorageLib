using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace ParserTest {


  [TestClass]
  public class ParentOneChildTest {


    [TestMethod]
    public void TestParentOneChild() {

      //simple ParentOneChild
      //=====================
      var compiler = Generator.Analyze(@"
        public class SingleChildParent {
          [StorageProperty(isParentOneChild: true)]
          public SingleChildChild? Child;
        }
        public class SingleChildChild {
          public SingleChildParent Parent;
        }
      ");
      Assert.AreEqual("Parent", compiler!.Classes["SingleChildParent"].Members["Child"].SingleChildMI!.MemberName);
      Assert.AreEqual("Child", compiler!.Classes["SingleChildChild"].Members["Parent"].ParentMemberInfo!.MemberName);

      //Child property is not nullable
      //------------------------------
      Generator.Analyze(@"  
        public class SingleChildParent {
          [StorageProperty(isParentOneChild: true)]
          public SingleChildChild Child;
        }
        public class SingleChildChild {
          public SingleChildParent Parent;
        }
      ",
      @"Class: SingleChildParent
        Property: [StorageProperty(isParentOneChild: true)]
                  public SingleChildChild Child;
        Child is a parent for at most 1 child. It must be nullable. Reason: It must be possible to create the " +
        "parent when the child does not exist yet.");

      //unknown child class
      //-------------------
      Generator.Analyze(@"  
        public class SingleChildParent {
          [StorageProperty(isParentOneChild: true)]
          public ChildXxx? Child;
        }
        public class SingleChildChild {
          public SingleChildParent Parent;
        }
      ",
      @"Class: SingleChildParent
        Property: [StorageProperty(isParentOneChild: true)]
                  public ChildXxx? Child;
        Cannot find class ChildXxx.");

      //missing parent property child class
      //-----------------------------------
      Generator.Analyze(@"  
        public class SingleChildParent {
          [StorageProperty(isParentOneChild: true)]
          public SingleChildChild? Child;
        }
        public class SingleChildChild {
        }
      ",
      @"Class: SingleChildParent
        Property: [StorageProperty(isParentOneChild: true)]
                  public SingleChildChild? Child;
        Child references the class SingleChildChild. A corresponding property with type SingleChildParent is missing in SingleChildChild.");


      //Two properties in child class with parent's type and 2 collections in parent
      //============================================================================
      compiler = Generator.Analyze(@"  
        public class SingleChildParent {
          [StorageProperty(isParentOneChild: true, childPropertyName: ""Parent1"")]
          public SingleChildChild? Child1;
          [StorageProperty(isParentOneChild: true, childPropertyName: ""Parent2"")]
          public SingleChildChild? Child2;
        }
        public class SingleChildChild {
          public SingleChildParent Parent1;
          public SingleChildParent Parent2;
        }
        ");
      Assert.AreEqual("Parent1", compiler!.Classes["SingleChildParent"].Members["Child1"].SingleChildMI!.MemberName);
      Assert.AreEqual("Child1", compiler!.Classes["SingleChildChild"].Members["Parent1"].ParentMemberInfo!.MemberName);
      Assert.AreEqual("Parent2", compiler!.Classes["SingleChildParent"].Members["Child2"].SingleChildMI!.MemberName);
      Assert.AreEqual("Child2", compiler!.Classes["SingleChildChild"].Members["Parent2"].ParentMemberInfo!.MemberName);

      //Two properties in child class with parent's type
      //------------------------------------------------
      Generator.Analyze(@"  
        public class SingleChildParent {
          [StorageProperty(isParentOneChild: true)]
          public SingleChildChild? Child1;
          [StorageProperty(isParentOneChild: true)]
          public SingleChildChild? Child2;
        }
        public class SingleChildChild {
          public SingleChildParent Parent1;
          public SingleChildParent Parent2;
        }
      ",
      @"Class: SingleChildParent
        Property: [StorageProperty(isParentOneChild: true)]
                  public SingleChildChild? Child1;
        The child class SingleChildChild has 2 properties linking to parent class SingleChildParent:
        public SingleChildParent Parent1;
        public SingleChildParent Parent2;
        Only List<SingleChildChild> allows multiple properties in the child class SingleChildChild to link to the " +
        "parent class SingleChildParent, but Child1 is not a List<> type.");

      //Two properties in child class with parent's type and only 1 StorageProperty.childPropertyName
      //----------------------------------------------------------------------------------------------
      compiler = Generator.Analyze(@"  
        public class SingleChildParent {
          [StorageProperty(isParentOneChild: true, childPropertyName: ""Parent1"")]
          public SingleChildChild? Child1;
        }
        public class SingleChildChild {
          public SingleChildParent Parent1;
          public SingleChildParent Parent2;
        }
        ",
        @"Class: SingleChildChild
        Property: public SingleChildParent Parent2;
        The parent class SingleChildParent is not linking back to Parent2 property. This can happen if 2 or more " +
        "properties of SingleChildChild link to SingleChildParent class. In this case, several collections or " +
        "properties for singel child are needed in the SingleChildParent class and they need to use " +
        "StoragePropertyAttribute.ChildPropertyName to indicate which of their property links to which " +
        "SingleChildChild property.");

      //Two properties in child class with parent's type and wrong StorageProperty.childPropertyName
      //---------------------------------------------------------------------------------------------
      Generator.Analyze(@"  
         public class SingleChildParent {
          [StorageProperty(isParentOneChild: true, childPropertyName: ""ParentXXX"")]
          public SingleChildChild? Child1;
        }
        public class SingleChildChild {
          public SingleChildParent Parent1;
          public SingleChildParent Parent2;
        }

      ",
      @"Class: SingleChildParent
        Property: [StorageProperty(isParentOneChild: true, childPropertyName: ""ParentXXX"")]
                  public SingleChildChild? Child1;
        Can not find property ParentXXX in child class SingleChildChild.");
    }
  }
}
