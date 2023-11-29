using Microsoft.VisualStudio.TestTools.UnitTesting;
#pragma warning disable IDE0059 // Unnecessary assignment of a value


namespace ParserTest {


  [TestClass]
  public class SortedBucketTest {


    [TestMethod]
    public void TestSortedBucket() {

      //simple SortedBucket
      //===================
      var compiler = Generator.Analyze(@"  
        public class SortedBucketParent {
          public SortedBucketCollection<Date, string, SortedBucketChild> Children;
        }
        public class SortedBucketChild {
          public string Name;
          public Date Date;
          public SortedBucketParent Parent;
        }
      ");
      var children = compiler!.Classes["SortedBucketParent"].Members["Children"];
      Assert.AreEqual("Date", children.ChildKeyPropertyName);
      Assert.AreEqual("DateTime", children.ChildKeyTypeString);
      Assert.AreEqual("Name", children.ChildKey2PropertyName);
      Assert.AreEqual("string", children.ChildKey2TypeString);
      Assert.AreEqual("SortedBucketChild", children.ChildTypeName);
      Assert.AreEqual("StorageSortedBucketCollection<DateTime, string, SortedBucketChild>", children.TypeString);
      Assert.AreEqual("Parent", children.SingleChildMI!.MemberName);

      Assert.AreEqual("Children", compiler!.Classes["SortedBucketChild"].Members["Parent"].ParentMemberInfo!.MemberName);

      //unknown child class
      //-------------------
      Generator.Analyze(@"  
        public class SortedBucketParent {
          public SortedBucketCollection<Date, string, SortedBucketChildXXX> Children;
        }
        public class SortedBucketChild {
          public string Name;
          public Date Date;
          public SortedBucketParent Parent;
        }
      ",
      @"Class: SortedBucketParent
        Property: public SortedBucketCollection<Date, string, SortedBucketChildXXX> Children;
        Cannot find class SortedBucketChildXXX.");

      //missing parent property in child class
      //--------------------------------------
      Generator.Analyze(@"  
        public class SortedBucketParent {
          public SortedBucketCollection<Date, string, SortedBucketChild> Children;
        }
        public class SortedBucketChild {
          public string Name;
          public Date Date;
        }
      ",
      @"Class: SortedBucketParent
        Property: public SortedBucketCollection<Date, string, SortedBucketChild> Children;
        Children references the class SortedBucketChild. A corresponding property with type SortedBucketParent is " +
        "missing in SortedBucketChild.");

      //missing key1 property in child class
      //------------------------------------
      Generator.Analyze(@"  
        public class SortedBucketParent {
          public SortedBucketCollection<Date, string, SortedBucketChild> Children;
        }
        public class SortedBucketChild {
          public string Name;
          public SortedBucketParent Parent;
        }
      ",
      @"Class: SortedBucketParent
        Property: public SortedBucketCollection<Date, string, SortedBucketChild> Children;
        Children references the class SortedBucketChild. A corresponding property with type Date used as key " +
        "into Children is missing in SortedBucketChild.");

      //missing key2 property in child class
      //------------------------------------
      Generator.Analyze(@"  
        public class SortedBucketParent {
          public SortedBucketCollection<Date, string, SortedBucketChild> Children;
        }
        public class SortedBucketChild {
          public Date Date;
          public SortedBucketParent Parent;
        }
      ",
      @"Class: SortedBucketParent
        Property: public SortedBucketCollection<Date, string, SortedBucketChild> Children;
        Children references the class SortedBucketChild. A corresponding property with type string used as second " +
        "key into Children is missing in SortedBucketChild.");

      //Two properties in child class with parent's type
      //------------------------------------------------
      Generator.Analyze(@"  
        public class SortedBucketParent {
          public SortedBucketCollection<Date, string, SortedBucketChild> Children;
        }
        public class SortedBucketChild {
          public string Name;
          public Date Date;
          public SortedBucketParent Parent1;
          public SortedBucketParent Parent2;
        }
      ",
      @"Class: SortedBucketParent
        Property: public SortedBucketCollection<Date, string, SortedBucketChild> Children;
        The child class SortedBucketChild has 2 properties linking to parent class SortedBucketParent:
        public SortedBucketParent Parent1;
        public SortedBucketParent Parent2;
        Use HashSet<SortedBucketChild> if more than one child property links to Children or add to SortedBucketParent " +
        "one List<SortedBucketChild> for each child property with the type SortedBucketParent and specify with " +
        "attribute StorageProperty.ChildPropertyName which child property links to which List<> in Parent.");

      //Two properties in child class matching the first key type defined in the parent
      //-------------------------------------------------------------------------------
      Generator.Analyze(@"  
        public class SortedBucketParent {
          public SortedBucketCollection<Date, string, SortedBucketChild> Children;
        }
        public class SortedBucketChild {
          public string Name;
          public Date Date1;
          public Date Date2;
          public SortedBucketParent Parent1;
        }
      ",
      @"Class: SortedBucketParent
        Property: public SortedBucketCollection<Date, string, SortedBucketChild> Children;
        The collection Children in parent class SortedBucketParent needs a key with the type Date. There are 2 " +
        "properties in SortedBucketChild with the type Date. Use StoragePropertyAttribute.ChildKey2PropertyName to " +
        "indicate which property should be used.");

      //Two properties in child class matching the second key type defined in the parent
      //--------------------------------------------------------------------------------
      Generator.Analyze(@"  
        public class SortedBucketParent {
          public SortedBucketCollection<Date, string, SortedBucketChild> Children;
        }
        public class SortedBucketChild {
          public string Name1;
          public string Name2;
          public Date Date;
          public SortedBucketParent Parent1;
        }
      ",
      @"Class: SortedBucketParent
        Property: public SortedBucketCollection<Date, string, SortedBucketChild> Children;
        The collection Children in parent class SortedBucketParent needs a second key with the type string. There " +
        "are 2 properties in SortedBucketChild with the type string. Use StoragePropertyAttribute.ChildKeyPropertyName " +
        "to indicate which property should be used.");

      //Two collections in parent and only 1 parent property in child
      //-------------------------------------------------------------
      compiler = Generator.Analyze(@"  
        public class SortedBucketParent {
          public SortedBucketCollection<Date, string, SortedBucketChild> Children1;
          public SortedBucketCollection<Date, string, SortedBucketChild> Children2;
        }
        public class SortedBucketChild {
          public string Name;
          public Date Date;
          public SortedBucketParent Parent;
        }
      ",
      @"Class: SortedBucketParent
        Property: public SortedBucketCollection<Date, string, SortedBucketChild> Children2;
        Property Children2 in parent class SortedBucketParent references child class SortedBucketChild, which has " +
        "the property Parent with the proper type SortedBucketParent, but it is used already by " +
        "SortedBucketParent.Children1. If 2 or more properties in the parent class SortedBucketParent references " +
        "2 or more properties in the child class SortedBucketChild, use StoragePropertyAttribute.childPropertyName " +
        "to define which parent properties reference which child properties.");


      //simple SortedBucket using StorageProperty
      //=========================================
      compiler = Generator.Analyze(@"
        public class SortedBucketParent {
          [StorageProperty(childPropertyName: ""Parent"", childKeyPropertyName:""Date"", childKey2PropertyName:""Name"")]
          public SortedBucketCollection<Date, string, SortedBucketChild> Children;
        }
        public class SortedBucketChild {
          public string AnotherName;
          public Date AnotherDate;
          public string Name;
          public Date Date;
          public SortedBucketParent Parent;
        }
      ");
      children = compiler!.Classes["SortedBucketParent"].Members["Children"];
      Assert.AreEqual("Date", children.ChildKeyPropertyName);
      Assert.AreEqual("DateTime", children.ChildKeyTypeString);
      Assert.AreEqual("Name", children.ChildKey2PropertyName);
      Assert.AreEqual("string", children.ChildKey2TypeString);
      Assert.AreEqual("SortedBucketChild", children.ChildTypeName);
      Assert.AreEqual("StorageSortedBucketCollection<DateTime, string, SortedBucketChild>", children.TypeString);
      Assert.AreEqual("Parent", children.SingleChildMI!.MemberName);

      Assert.AreEqual("Children", compiler!.Classes["SortedBucketChild"].Members["Parent"].ParentMemberInfo!.MemberName);

      //parent property in child class has wrong type
      //---------------------------------------------
      Generator.Analyze(@"  
        public class SortedBucketParent {
          [StorageProperty(childPropertyName: ""Parent"", childKeyPropertyName:""Date"", childKey2PropertyName:""Name"")]
          public SortedBucketCollection<Date, string, SortedBucketChild> Children;
        }
        public class SortedBucketChild {
          public string AnotherName;
          public Date AnotherDate;
          public int Name;
          public Date Date;
          public string Parent;
        }
      ",
      @"Class: SortedBucketParent
        Property: [StorageProperty(childPropertyName: ""Parent"", childKeyPropertyName:""Date"", childKey2PropertyName:""Name"")]
                  public SortedBucketCollection<Date, string, SortedBucketChild> Children;
        Property Parent in child class SortedBucketChild should have the type SortedBucketParent, but its type is string:
        Class: SortedBucketChild
        Property: public string Parent;");

      //Key1 property in child class has wrong type
      //-------------------------------------------
      Generator.Analyze(@"  
        public class SortedBucketParent {
          [StorageProperty(childPropertyName: ""Parent"", childKeyPropertyName:""Date"", childKey2PropertyName:""Name"")]
          public SortedBucketCollection<Date, string, SortedBucketChild> Children;
        }
        public class SortedBucketChild {
          public string AnotherName;
          public Date AnotherDate;
          public int Name;
          public Date Date;
          public SortedBucketParent Parent;
        }
      ",
      @"Class: SortedBucketParent
        Property: [StorageProperty(childPropertyName: ""Parent"", childKeyPropertyName:""Date"", childKey2PropertyName:""Name"")]
                  public SortedBucketCollection<Date, string, SortedBucketChild> Children;
        Property Name in child class SortedBucketChild should have the type string, but its type is int:
        Class: SortedBucketChild
        Property: public int Name;");

      //Key2 property in child class has wrong type
      //-------------------------------------------
      Generator.Analyze(@"  
        public class SortedBucketParent {
          [StorageProperty(childPropertyName: ""Parent"", childKeyPropertyName:""Date"", childKey2PropertyName:""Name"")]
          public SortedBucketCollection<Date, string, SortedBucketChild> Children;
        }
        public class SortedBucketChild {
          public string AnotherName;
          public Date AnotherDate;
          public string Name;
          public int Date;
          public SortedBucketParent Parent;
        }
      ",
      @"Class: SortedBucketParent
        Property: [StorageProperty(childPropertyName: ""Parent"", childKeyPropertyName:""Date"", childKey2PropertyName:""Name"")]
                  public SortedBucketCollection<Date, string, SortedBucketChild> Children;
        Property Date in child class SortedBucketChild should have the type Date, but its type is int:
        Class: SortedBucketChild
        Property: public int Date;");


      //Two properties in child class with parent's type and 2 collections in parent
      //============================================================================
      compiler = Generator.Analyze(@"  
        public class SortedBucketParent {
          [StorageProperty(childPropertyName: ""Parent1"", childKeyPropertyName:""Date1"", childKey2PropertyName:""Name1"")]
          public SortedBucketCollection<Date, string, SortedBucketChild> Children1;
          [StorageProperty(childPropertyName: ""Parent2"", childKeyPropertyName:""Date2"", childKey2PropertyName:""Name2"")]
          public SortedBucketCollection<Date, string, SortedBucketChild> Children2;
        }
        public class SortedBucketChild {
          public string Name1;
          public Date Date1;
          public SortedBucketParent Parent1;
          public string Name2;
          public Date Date2;
          public SortedBucketParent Parent2;
        }
      ");
      children = compiler!.Classes["SortedBucketParent"].Members["Children1"];
      Assert.AreEqual("Date1", children.ChildKeyPropertyName);
      Assert.AreEqual("DateTime", children.ChildKeyTypeString);
      Assert.AreEqual("Name1", children.ChildKey2PropertyName);
      Assert.AreEqual("string", children.ChildKey2TypeString);
      Assert.AreEqual("SortedBucketChild", children.ChildTypeName);
      Assert.AreEqual("StorageSortedBucketCollection<DateTime, string, SortedBucketChild>", children.TypeString);
      Assert.AreEqual("Parent1", children.SingleChildMI!.MemberName);
      Assert.AreEqual("Children1", compiler!.Classes["SortedBucketChild"].Members["Parent1"].ParentMemberInfo!.MemberName);

      children = compiler!.Classes["SortedBucketParent"].Members["Children2"];
      Assert.AreEqual("Date2", children.ChildKeyPropertyName);
      Assert.AreEqual("DateTime", children.ChildKeyTypeString);
      Assert.AreEqual("Name2", children.ChildKey2PropertyName);
      Assert.AreEqual("string", children.ChildKey2TypeString);
      Assert.AreEqual("SortedBucketChild", children.ChildTypeName);
      Assert.AreEqual("StorageSortedBucketCollection<DateTime, string, SortedBucketChild>", children.TypeString);
      Assert.AreEqual("Parent2", children.SingleChildMI!.MemberName);
      Assert.AreEqual("Children2", compiler!.Classes["SortedBucketChild"].Members["Parent2"].ParentMemberInfo!.MemberName);

      //Two properties in child class with parent's type and only 1 StorageProperty.childPropertyName
      //----------------------------------------------------------------------------------------------
      compiler = Generator.Analyze(@"  
        public class SortedBucketParent {
          [StorageProperty(childPropertyName: ""Parent1"", childKeyPropertyName:""Date1"", childKey2PropertyName:""Name1"")]
          public SortedBucketCollection<Date, string, SortedBucketChild> Children1;
        }
        public class SortedBucketChild {
          public string Name1;
          public Date Date1;
          public SortedBucketParent Parent1;
          public string Name2;
          public Date Date2;
          public SortedBucketParent Parent2;
        }
      ",
      @"Class: SortedBucketChild
        Property: public SortedBucketParent Parent2;
        The parent class SortedBucketParent is not linking back to Parent2 property. This can happen if 2 or more " +
        "properties of SortedBucketChild link to SortedBucketParent class. In this case, several collections or " +
        "properties for single child are needed in the SortedBucketParent class and they need to use " +
        "StoragePropertyAttribute.ChildPropertyName to indicate which of their property links to which " +
        "SortedBucketChild property.");

      //Two properties in child class with parent's type and wrong StorageProperty.childPropertyName
      //---------------------------------------------------------------------------------------------
      Generator.Analyze(@"  
        public class SortedBucketParent {
          [StorageProperty(childPropertyName: ""ParentXXX"", childKeyPropertyName:""Date1"", childKey2PropertyName:""Name1"")]
          public SortedBucketCollection<Date, string, SortedBucketChild> Children1;
        }
        public class SortedBucketChild {
          public string Name1;
          public Date Date1;
          public SortedBucketParent Parent1;
          public string Name2;
          public Date Date2;
          public SortedBucketParent Parent2;
        }
      ",
      @"Class: SortedBucketParent
        Property: [StorageProperty(childPropertyName: ""ParentXXX"", childKeyPropertyName:""Date1"", childKey2PropertyName:""Name1"")]
                  public SortedBucketCollection<Date, string, SortedBucketChild> Children1;
        Can not find property ParentXXX in child class SortedBucketChild.");
    }
  }
}
