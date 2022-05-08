using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace ParserTest {


  [TestClass]
  public class EnumTest {
    [TestMethod]
    public void TestList() {

      //simple enum
      //-----------
      var compiler = Generator.Analyze(@"  
        public enum Weekdays { Mo, Tu, We, Th, Fr}

        public class ClassWithEnumProperty {
          public Weekdays Weekday;
        }
      ");
      Assert.AreEqual("Weekdays", compiler!.Classes["ClassWithEnumProperty"].Members["Weekday"].TypeString);

      //Todo: Activate test
      ////enum with default value
      ////-----------------------
      //Generator.Analyze(@"  
      //  public enum Weekdays { Mo, Tu, We, Th, Fr}

      //  public class ClassWithEnumProperty {
      //    [StorageProperty(defaultValue: ""Weekdays.Mo"")]
      //    public Weekdays Weekday;
      //  }
      //");

      ////enum with illegal default value
      ////-------------------------------
      //Generator.Analyze(@"  
      //  public enum Weekdays { Mo, Tu, We, Th, Fr}

      //  public class ClassWithEnumProperty {
      //    [StorageProperty(defaultValue: ""Weekdays.Sa"")]
      //    public Weekdays Weekday;
      //  }
      //",
      //@"Class: ListParent
      //  Property: public List<ListChild1> Children;
      //  Cannot find class ListChild1.");
    }
  }
}
