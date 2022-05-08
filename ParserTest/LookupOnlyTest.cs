using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace ParserTest {


  [TestClass]
  public class LookupOnlyTest {


    [TestMethod]
    public void TestLookupOnly() {

      //simple lookup
      //=============
      var compiler = Generator.Analyze(@"
        [StorageClass(areInstancesReleasable: false)]
        public class LookupParent {
          public string Name;
        }
        public class LookupChild {
          public string Name;
          [StorageProperty(isLookupOnly: true)]
          public LookupParent Parent;
        }
      ");
      Assert.AreEqual("LookupParent", compiler!.Classes["LookupChild"].Members["Parent"].ParentClassInfo!.ClassName);
      Assert.IsTrue(compiler!.Classes["LookupParent"].Children.Contains(compiler!.Classes["LookupChild"]));

      //lookup none existing parent
      //---------------------------
      compiler = Generator.Analyze(@"  
        public class LookupChild {
          public string Name;
          [StorageProperty(isLookupOnly: true)]
          public LookupParent Parent;
        }
      ",
      @"Class: LookupChild
        Property: [StorageProperty(isLookupOnly: true)]
                  public LookupParent Parent;
        Parent links to the class LookupParent, which is missing in the data model.");

      //lookup to deletable parent
      //--------------------------
      compiler = Generator.Analyze(@"  
        public class LookupParent {
          public string Name;
        }
        public class LookupChild {
          public string Name;
          [StorageProperty(isLookupOnly: true)]
          public LookupParent Parent;
        }
      ",
      @"Class: LookupChild
        Property: [StorageProperty(isLookupOnly: true)]
                  public LookupParent Parent;
        Cannot use the deletable instances of class LookupParent as lookup.");

      //Todo: Activate test
      ////lookup with childPropertyName
      ////------------------------------
      //compiler = Generator.Analyze(@"  
      //  public class LookupParent {
      //    public string Name;
      //  }
      //  public class LookupChild {
      //    public string Name;
      //   [StorageProperty(isLookupOnly: true, childPropertyName: ""Parent"")]
      //    public LookupParent Parent;
      //  }
      //",
      //@"Class: LookupChild
      //  Property: [StorageProperty(isLookupOnly: true)]
      //            public LookupParent Parent;
      //  Cannot use the deletable instances of class LookupParent as lookup.");


    }
  }
}
