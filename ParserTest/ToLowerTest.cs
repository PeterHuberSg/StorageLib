using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace ParserTest {


  [TestClass]
  public class ToLowerTest {


    [TestMethod]
    public void TestToLower() {

      //simple ToLower
      //==============
      var compiler = Generator.Analyze(@"  
        public class ToLowerClass {
          public string Name;
          [StorageProperty(toLower: ""Name"")]
          public string NameLower;
        }
      ");
      Assert.IsFalse(compiler!.Classes["ToLowerClass"].HasNotReadOnlyNeedDirectories);
      Assert.AreEqual("NameLower", compiler!.Classes["ToLowerClass"].Members["Name"].ToLowerTarget!.MemberName);
      Assert.AreEqual("Name", compiler!.Classes["ToLowerClass"].Members["NameLower"].PropertyForToLower);

      //ToLower with not existing source property
      //-------------------
      Generator.Analyze(@"  
        public class ToLowerClass {
          [StorageProperty(toLower: ""Name"")]
          public string NameLower;
        }
      ",
      @"Class: ToLowerClass
        Property: [StorageProperty(toLower: ""Name"")]
                  public string NameLower;
        NameLower is supposed to be a lower case copy of a Name property in the same class, which cannot be found.");


      //ToLower with source directory
      //=============================
      compiler = Generator.Analyze(@"  
        public class ToLowerDictionaryClass {
          [StorageProperty(needsDictionary: true)]
          public string Name;
          [StorageProperty(toLower: ""Name"")]
          public string NameLower;//the lower case version of Name will be used as key into the dictionary.
        }
      ");
      Assert.AreEqual("NameLower", compiler!.Classes["ToLowerDictionaryClass"].Members["Name"].ToLowerTarget!.MemberName);
      Assert.IsTrue(compiler!.Classes["ToLowerDictionaryClass"].Members["Name"].NeedsDictionary);
      Assert.IsFalse(compiler!.Classes["ToLowerDictionaryClass"].Members["NameLower"].NeedsDictionary);
      Assert.IsTrue(compiler!.Classes["ToLowerDictionaryClass"].HasNotReadOnlyNeedDirectories);


      //ToLower with lower case directory
      //=================================
      compiler = Generator.Analyze(@"  
        public class ToLowerDictionaryClass {
          public string Name;
          [StorageProperty(toLower: ""Name"", needsDictionary: true)]
          public string NameLower;//the lower case version of Name will be used as key into the dictionary.
        }
      ");
      Assert.AreEqual("NameLower", compiler!.Classes["ToLowerDictionaryClass"].Members["Name"].ToLowerTarget!.MemberName);
      Assert.IsTrue(compiler!.Classes["ToLowerDictionaryClass"].Members["NameLower"].NeedsDictionary);
      Assert.IsTrue(compiler!.Classes["ToLowerDictionaryClass"].HasNotReadOnlyNeedDirectories);

      //ToLower with source and lower case directory
      //-------------------
      Generator.Analyze(@"  
        public class ToLowerDictionaryClass {
          [StorageProperty(needsDictionary: true)]
          public string Name;
          [StorageProperty(toLower: ""Name"", needsDictionary: true)]
          public string NameLower;//the lower case version of Name will be used as key into the dictionary.
        }
      ",
      @"Class: ToLowerDictionaryClass
        Property: [StorageProperty(toLower: ""Name"", needsDictionary: true)]
                  public string NameLower;
        NameLower is a lower case copy of Name. Both have the attribute parameter NeedsDictionary, but only one of them at a time can have a dictionary. StorageLib could be extended to allow both having a Dictionary.");
    }
  }
}