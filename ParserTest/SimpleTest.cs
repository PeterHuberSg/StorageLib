using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace ParserTest {


  [TestClass]
  public class SimpleTest {


    [TestMethod]
    public void TestSimple() {

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

      //A class having 2 properties with the same name
      //----------------------------------------------
      Generator.Analyze(@"  
        public class AClass {
          public int Property;
          public int Property;
        }
      ",
      @"Class: AClass
        Property: public int Property;
        Class AClass has 2 properties with the same name Property.");
    }
  }
}