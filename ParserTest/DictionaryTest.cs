using Microsoft.VisualStudio.TestTools.UnitTesting;
#pragma warning disable IDE0059 // Unnecessary assignment of a value


namespace ParserTest {


  [TestClass]
  public class DictionaryTest {


    [TestMethod]
    public void TestDictionary() {

      //simple Dictionary
      //=================
      var compiler = Generator.Analyze(@"  
        public class DictionaryParent {
          public Dictionary<Date, DictionaryChild> Children;
        }
        public class DictionaryChild {
          public Date Date;
          public DictionaryParent Parent;
        }
      ");
      var children = compiler!.Classes["DictionaryParent"].Members["Children"];
      Assert.AreEqual("Date", children.ChildKeyPropertyName);
      Assert.AreEqual("DateTime", children.ChildKeyTypeString);
      Assert.AreEqual("DictionaryChild", children.ChildTypeName);
      Assert.AreEqual("StorageDictionary<DateTime, DictionaryChild>", children.TypeString);
      Assert.AreEqual("Parent", children.SingleChildMI!.MemberName);

      Assert.AreEqual("Children", compiler!.Classes["DictionaryChild"].Members["Parent"].ParentMemberInfo!.MemberName);

      //unknown child class
      //-------------------
      Generator.Analyze(@"  
        public class DictionaryParent {
          public Dictionary<Date, DictionaryChildXXX> Children;
        }
        public class DictionaryChild {
          public Date Date;
          public DictionaryParent Parent;
        }
      ",
      @"Class: DictionaryParent
        Property: public Dictionary<Date, DictionaryChildXXX> Children;
        Cannot find class DictionaryChildXXX.");

      //missing parent property in child class
      //--------------------------------------
      Generator.Analyze(@"  
        public class DictionaryParent {
          public Dictionary<Date, DictionaryChild> Children;
        }
        public class DictionaryChild {
          public Date Date;
        }
      ",
      @"Class: DictionaryParent
        Property: public Dictionary<Date, DictionaryChild> Children;
        Children references the class DictionaryChild. A corresponding property with type DictionaryParent is " +
        "missing in DictionaryChild.");

      //missing key property in child class
      //-----------------------------------
      Generator.Analyze(@"  
        public class DictionaryParent {
          public Dictionary<Date, DictionaryChild> Children;
        }
        public class DictionaryChild {
          public DictionaryParent Parent;
        }
      ",
      @"Class: DictionaryParent
        Property: public Dictionary<Date, DictionaryChild> Children;
        Children references the class DictionaryChild. A corresponding property with type Date used as key " +
        "into Children is missing in DictionaryChild.");

      //Two properties in child class with parent's type
      //------------------------------------------------
      Generator.Analyze(@"  
        public class DictionaryParent {
          public Dictionary<Date, DictionaryChild> Children;
        }
        public class DictionaryChild {
          public Date Date;
          public DictionaryParent Parent1;
          public DictionaryParent Parent2;
        }
      ",
      @"Class: DictionaryParent
        Property: public Dictionary<Date, DictionaryChild> Children;
        The child class DictionaryChild has 2 properties linking to parent class DictionaryParent:
        public DictionaryParent Parent1;
        public DictionaryParent Parent2;
        Use HashSet<DictionaryChild> if more than one child property links to Children or add to DictionaryParent " +
        "one List<DictionaryChild> for each child property with the type DictionaryParent and specify with attribute " +
        "StorageProperty.ChildPropertyName which child property links to which List<> in Parent.");

      //Two properties in child class matching the key type defined in the parent
      //-------------------------------------------------------------------------
      Generator.Analyze(@"  
        public class DictionaryParent {
          public Dictionary<Date, DictionaryChild> Children;
        }
        public class DictionaryChild {
          public Date Date1;
          public Date Date2;
          public DictionaryParent Parent1;
        }
      ",
      @"Class: DictionaryParent
        Property: public Dictionary<Date, DictionaryChild> Children;
        The collection Children in parent class DictionaryParent needs a key with the type Date. There are 2 " +
        "properties in DictionaryChild with the type Date. Use StoragePropertyAttribute.ChildKey2PropertyName to " +
        "indicate which property should be used.");

      //Two collections in parent and only 1 parent property in child
      //-------------------------------------------------------------
      compiler = Generator.Analyze(@"  
        public class DictionaryParent {
          public Dictionary<Date, DictionaryChild> Children1;
          public Dictionary<Date, DictionaryChild> Children2;
        }
        public class DictionaryChild {
          public Date Date;
          public DictionaryParent Parent;
        }
      ",
      @"Class: DictionaryParent
        Property: public Dictionary<Date, DictionaryChild> Children2;
        Property Children2 in parent class DictionaryParent references child class DictionaryChild, which has " +
        "the property Parent with the proper type DictionaryParent, but it is used already by " +
        "DictionaryParent.Children1. If 2 or more properties in the parent class DictionaryParent references " +
        "2 or more properties in the child class DictionaryChild, use StoragePropertyAttribute.childPropertyName " +
        "to define which parent properties reference which child properties.");


      //simple Dictionary using StorageProperty
      //=========================================
      compiler = Generator.Analyze(@"
        public class DictionaryParent {
          [StorageProperty(childPropertyName: ""Parent"", childKeyPropertyName:""Date"")]
          public Dictionary<Date, DictionaryChild> Children;
        }
        public class DictionaryChild {
          public Date AnotherDate;
          public Date Date;
          public DictionaryParent Parent;
        }
      ");
      children = compiler!.Classes["DictionaryParent"].Members["Children"];
      Assert.AreEqual("Date", children.ChildKeyPropertyName);
      Assert.AreEqual("DateTime", children.ChildKeyTypeString);
      Assert.AreEqual("DictionaryChild", children.ChildTypeName);
      Assert.AreEqual("StorageDictionary<DateTime, DictionaryChild>", children.TypeString);
      Assert.AreEqual("Parent", children.SingleChildMI!.MemberName);

      Assert.AreEqual("Children", compiler!.Classes["DictionaryChild"].Members["Parent"].ParentMemberInfo!.MemberName);

      //parent property in child class has wrong type
      //---------------------------------------------
      Generator.Analyze(@"  
        public class DictionaryParent {
          [StorageProperty(childPropertyName: ""Parent"", childKeyPropertyName:""Date"")]
          public Dictionary<Date, DictionaryChild> Children;
        }
        public class DictionaryChild {
          public Date AnotherDate;
          public Date Date;
          public string Parent;
        }
      ",
      @"Class: DictionaryParent
        Property: [StorageProperty(childPropertyName: ""Parent"", childKeyPropertyName:""Date"")]
                  public Dictionary<Date, DictionaryChild> Children;
        Property Parent in child class DictionaryChild should have the type DictionaryParent, but its type is string:
        Class: DictionaryChild
        Property: public string Parent;");

      //Key1 property in child class has wrong type
      //-------------------------------------------
      Generator.Analyze(@"  
        public class DictionaryParent {
          [StorageProperty(childPropertyName: ""Parent"", childKeyPropertyName:""Date"")]
          public Dictionary<Date, DictionaryChild> Children;
        }
        public class DictionaryChild {
          public Date AnotherDate;
          public int Date;
          public DictionaryParent Parent;
        }
      ",
      @"Class: DictionaryParent
        Property: [StorageProperty(childPropertyName: ""Parent"", childKeyPropertyName:""Date"")]
                  public Dictionary<Date, DictionaryChild> Children;
        Property Date in child class DictionaryChild should have the type Date, but its type is int:
        Class: DictionaryChild
        Property: public int Date;");


      //Two properties in child class with parent's type and 2 collections in parent
      //============================================================================
      compiler = Generator.Analyze(@"  
        public class DictionaryParent {
          [StorageProperty(childPropertyName: ""Parent1"", childKeyPropertyName:""Date1"")]
          public Dictionary<Date, DictionaryChild> Children1;
          [StorageProperty(childPropertyName: ""Parent2"", childKeyPropertyName:""Date2"")]
          public Dictionary<Date, DictionaryChild> Children2;
        }
        public class DictionaryChild {
          public Date Date1;
          public DictionaryParent Parent1;
          public Date Date2;
          public DictionaryParent Parent2;
        }
      ");
      children = compiler!.Classes["DictionaryParent"].Members["Children1"];
      Assert.AreEqual("Date1", children.ChildKeyPropertyName);
      Assert.AreEqual("DateTime", children.ChildKeyTypeString);
      Assert.AreEqual("DictionaryChild", children.ChildTypeName);
      Assert.AreEqual("StorageDictionary<DateTime, DictionaryChild>", children.TypeString);
      Assert.AreEqual("Parent1", children.SingleChildMI!.MemberName);
      Assert.AreEqual("Children1", compiler!.Classes["DictionaryChild"].Members["Parent1"].ParentMemberInfo!.MemberName);

      children = compiler!.Classes["DictionaryParent"].Members["Children2"];
      Assert.AreEqual("Date2", children.ChildKeyPropertyName);
      Assert.AreEqual("DateTime", children.ChildKeyTypeString);
      Assert.AreEqual("DictionaryChild", children.ChildTypeName);
      Assert.AreEqual("StorageDictionary<DateTime, DictionaryChild>", children.TypeString);
      Assert.AreEqual("Parent2", children.SingleChildMI!.MemberName);
      Assert.AreEqual("Children2", compiler!.Classes["DictionaryChild"].Members["Parent2"].ParentMemberInfo!.MemberName);

      //Two properties in child class with parent's type and only 1 StorageProperty.childPropertyName
      //----------------------------------------------------------------------------------------------
      compiler = Generator.Analyze(@"
        public class DictionaryParent {
          [StorageProperty(childPropertyName: ""Parent1"", childKeyPropertyName:""Date1"")]
          public Dictionary<Date, DictionaryChild> Children1;
        }
        public class DictionaryChild {
          public Date Date1;
          public DictionaryParent Parent1;
          public Date Date2;
          public DictionaryParent Parent2;
        }
      ",
      @"Class: DictionaryChild
        Property: public DictionaryParent Parent2;
        The parent class DictionaryParent is not linking back to Parent2 property. This can happen if 2 or more " +
        "properties of DictionaryChild link to DictionaryParent class. In this case, several collections or " +
        "properties for single child are needed in the DictionaryParent class and they need to use " +
        "StoragePropertyAttribute.ChildPropertyName to indicate which of their property links to which " +
        "DictionaryChild property.");

      //Two properties in child class with parent's type and wrong StorageProperty.childPropertyName
      //---------------------------------------------------------------------------------------------
      Generator.Analyze(@"
        public class DictionaryParent {
          [StorageProperty(childPropertyName: ""ParentXXX"", childKeyPropertyName:""Date1"")]
          public Dictionary<Date, DictionaryChild> Children1;
        }
        public class DictionaryChild {
          public Date Date1;
          public DictionaryParent Parent1;
          public Date Date2;
          public DictionaryParent Parent2;
        }
      ",
      @"Class: DictionaryParent
        Property: [StorageProperty(childPropertyName: ""ParentXXX"", childKeyPropertyName:""Date1"")]
                  public Dictionary<Date, DictionaryChild> Children1;
        Can not find property ParentXXX in child class DictionaryChild.");
    }
  }
}
