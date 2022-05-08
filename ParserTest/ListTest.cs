using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace ParserTest {


  [TestClass]
  public class ListTest {


    [TestMethod]
    public void TestList() {

      //simple parent child
      //===================
      var compiler = Generator.Analyze(@"  
        public class ListParent {
          public List<ListChild> Children;
        }

        public class ListChild {
          public ListParent Parent;
        }
      ");
      Assert.AreEqual("Parent", compiler!.Classes["ListParent"].Members["Children"].SingleChildMI!.MemberName);
      Assert.AreEqual("Children", compiler!.Classes["ListChild"].Members["Parent"].ParentMemberInfo!.MemberName);

      //unknown child class
      //-------------------
      Generator.Analyze(@"  
        public class ListParent {
          public List<ListChild1> Children;
        }

        public class ListChild {
          public ListParent Parent;
        }
      ",
      @"Class: ListParent
        Property: public List<ListChild1> Children;
        Cannot find class ListChild1.");

      //missing parent property in child class
      //--------------------------------------
      Generator.Analyze(@"  
        public class ListParent {
          public List<ListChild> Children;
        }

        public class ListChild {
        }
      ",
      @"Class: ListParent
        Property: public List<ListChild> Children;
        Children references the class ListChild. A corresponding property with type ListParent is missing in ListChild.");

      //Todo: Replace with HashSetTest
      ////Two properties in child class with parent's type
      ////------------------------------------------------
      //Generator.Analyze(@"  
      //  public class ListParent {
      //    public List<ListChild> Children;
      //  }

      //  public class ListChild {
      //    public ListParent Parent1;
      //    public ListParent Parent2;
      //  }
      //",
      //@"Class: ListChild
      //  Property: public ListParent Parent2;
      //  The parent class ListParent is not linking back to Parent2 property. This can happen if 2 or more properties " +
      //  "of ListChild link to ListParent class. In this case, several collections or properties for singel child are " +
      //  "needed in the ListParent class and they need to use StoragePropertyAttribute.ChildPropertyName to indicate " +
      //  "which of their property links to which ListChild property.");

      //missing children property in parent class
      //-----------------------------------------
      Generator.Analyze(@"  
        public class ListParent {
        }

        public class ListChild {
             public ListParent Parent;
     }
      ",
      @"Class: ListChild
        Property: public ListParent Parent;
        Property Parent in child class ListChild references parent class ListParent, which does not have a property referencing ListChild.");


      //simple parent child using childPropertyName
      //============================================
      compiler = Generator.Analyze(@"  
        public class ListParent {
          [StorageProperty(childPropertyName: ""Parent"")]
          public List<ListChild> Children;
        }

        public class ListChild {
          public ListParent Parent;
        }
      ");
      Assert.AreEqual("Parent", compiler!.Classes["ListParent"].Members["Children"].SingleChildMI!.MemberName);
      Assert.AreEqual("Children", compiler!.Classes["ListChild"].Members["Parent"].ParentMemberInfo!.MemberName);

      //Property in child class with parent's type, but wrong StorageProperty.childPropertyName
      //----------------------------------------------------------------------------------------
      Generator.Analyze(@"  
        public class ListParent {

          [StorageProperty(childPropertyName: ""ParentXXX"")]
          public List<ListChild> Children;
        }

        public class ListChild {
          public ListParent Parent;
        }
      ",
      @"Class: ListParent
        Property: [StorageProperty(childPropertyName: ""ParentXXX"")]
                  public List<ListChild> Children;
        Can not find property ParentXXX in child class ListChild.");

      //parent property in child class has wrong type
      //---------------------------------------------
      Generator.Analyze(@"  
        public class ListParent {
          [StorageProperty(childPropertyName: ""Parent"")]
          public List<ListChild> Children;
        }

        public class ListChild {
          public string Parent;
        }
      ",
      @"Class: ListParent
        Property: [StorageProperty(childPropertyName: ""Parent"")]
                  public List<ListChild> Children;
        Property Parent in child class ListChild should have the type ListParent, but its type is string:
        Class: ListChild
        Property: public string Parent;");

      //Two properties in child class with parent's type and only 1 StorageProperty.childPropertyName
      //----------------------------------------------------------------------------------------------
      compiler = Generator.Analyze(@"  
        public class ListParent {

          [StorageProperty(childPropertyName: ""Parent1"")]
          public List<ListChild> Children;
        }

        public class ListChild {
          public ListParent Parent1;
          public ListParent Parent2;
        }
      ",
      @"Class: ListChild
        Property: public ListParent Parent2;
        Parent class: ListParent
        Parent property: [StorageProperty(childPropertyName: ""Parent1"")]
                  public List<ListChild> Children;
        Property Parent2 in child class ListChild references property Children in parent class ListParent, which links " +
        "explicitely to Parent1 in class ListChild. Remove StoragePropertyAttribute.childPropertyName from Children if " +
        "more than 1 property in child class ListChild should reference ListParent.Children, in which case the List<> " +
        "will get replaced by a HashSet<> in the generated code.");


      //Two properties in child class with parent's type and 2 collections in parent
      //============================================================================
          compiler = Generator.Analyze(@"  
        public class ListParent {

          [StorageProperty(childPropertyName: ""Parent1"")]
          public List<ListChild> Children1;
          [StorageProperty(childPropertyName: ""Parent2"")]
          public List<ListChild> Children2;
        }

        public class ListChild {
          public ListParent Parent1;
          public ListParent Parent2;
        }
        ");
      Assert.AreEqual("Parent1", compiler!.Classes["ListParent"].Members["Children1"].SingleChildMI!.MemberName);
      Assert.AreEqual("Children1", compiler!.Classes["ListChild"].Members["Parent1"].ParentMemberInfo!.MemberName);
      Assert.AreEqual("Parent2", compiler!.Classes["ListParent"].Members["Children2"].SingleChildMI!.MemberName);
      Assert.AreEqual("Children2", compiler!.Classes["ListChild"].Members["Parent2"].ParentMemberInfo!.MemberName);

      //Two lists in parent and only 1 parent property in child
      //-------------------------------------------------------
      compiler = Generator.Analyze(@"  
        public class ListParent {
          public List<ListChild> Children1;
          public List<ListChild> Children2;
        }

        public class ListChild {
          public ListParent Parent;
        }
      ",
      @"Class: ListParent
        Property: public List<ListChild> Children2;
        Property Children2 in parent class ListParent references child class ListChild, which has the property " +
        "Parent with the proper type ListParent, but it is used already by ListParent.Children1. If 2 or more " +
        "properties in the parent class ListParent references 2 or more properties in the child class ListChild, " 
        +"use StoragePropertyAttribute.childPropertyName to define which parent properties reference which child " +
        "properties.");

    }
  }
}
