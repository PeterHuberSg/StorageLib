using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageLib;
using TestContext;


namespace StorageTest {


  [TestClass]
  public class GetNearestExtensionsTest {

    class itemClass {
      public DateTime Key;
      public int Value;
    }

    readonly SortedList<DateTime, itemClass> sortedListClass = new SortedList<DateTime, itemClass>();
    readonly SortedList<DateTime, int> sortedListStruct = new SortedList<DateTime, int>();


    [TestMethod]
    public void TestGetNearestExtensions() {
      var now = DateTime.Now.Date;
      var now1 = DateTime.Now.Date.AddDays(1);
      var now2 = DateTime.Now.Date.AddDays(2);
      var now3 = DateTime.Now.Date.AddDays(3);
      var now4 = DateTime.Now.Date.AddDays(4);
      var now5 = DateTime.Now.Date.AddDays(5);
      var now_1 = DateTime.Now.Date.AddDays(-1);
      var now_2 = DateTime.Now.Date.AddDays(-2);
      var now_3 = DateTime.Now.Date.AddDays(-3);
      var now_4 = DateTime.Now.Date.AddDays(-4);
      var now_5 = DateTime.Now.Date.AddDays(-5);
      var now_6 = DateTime.Now.Date.AddDays(-6);
      var now_7 = DateTime.Now.Date.AddDays(-7);

      Assert.IsNull(sortedListClass.GetEqualGreater(now));
      Assert.IsNull(sortedListClass.GetEqualGreaterKVP(now));
      Assert.ThrowsException<Exception>(() => sortedListStruct.GetEqualGreater(now));
      Assert.ThrowsException<Exception>(() => sortedListStruct.GetEqualGreaterKVP(now));
      Assert.IsNull(sortedListClass.GetEqualSmaller(now));
      Assert.IsNull(sortedListClass.GetEqualSmallerKVP(now));
      Assert.ThrowsException<Exception>(() => sortedListStruct.GetEqualSmaller(now));
      Assert.ThrowsException<Exception>(() => sortedListStruct.GetEqualSmallerKVP(now));

      add(now, 0);
      assertGetEqualGreater(now, 0);
      assertGetEqualGreater(now1, 0);
      assertGetEqualGreater(now_1, 0);
      assertGetEqualSmaller(now, 0);
      assertGetEqualSmaller(now1, 0);
      assertGetEqualSmaller(now_1, 0);
      assertFirstLast(now, 0, now, 0);

      add(now1, 1);
      assertGetEqualGreater(now, 0);
      assertGetEqualGreater(now1, 1);
      assertGetEqualGreater(now2, 1);
      assertGetEqualGreater(now_1, 0);
      assertGetEqualSmaller(now, 0);
      assertGetEqualSmaller(now1, 1);
      assertGetEqualSmaller(now2, 1);
      assertGetEqualSmaller(now_1, 0);
      assertFirstLast(now, 0, now1, 1);

      add(now_2, -2);
      assertGetEqualGreater(now, 0);
      assertGetEqualGreater(now1, 1);
      assertGetEqualGreater(now2, 1);
      assertGetEqualGreater(now_1, 0);
      assertGetEqualGreater(now_2, -2);
      assertGetEqualGreater(now_3, -2);
      assertGetEqualSmaller(now, 0);
      assertGetEqualSmaller(now1, 1);
      assertGetEqualSmaller(now2, 1);
      assertGetEqualSmaller(now_1, -2);
      assertGetEqualSmaller(now_2, -2);
      assertGetEqualSmaller(now_3, -2);
      assertFirstLast(now_2, -2, now1, 1);

      add(now4, 4);
      assertGetEqualGreater(now, 0);
      assertGetEqualGreater(now1, 1);
      assertGetEqualGreater(now2, 4);
      assertGetEqualGreater(now3, 4);
      assertGetEqualGreater(now4, 4);
      assertGetEqualGreater(now5, 4);
      assertGetEqualGreater(now_1, 0);
      assertGetEqualGreater(now_2, -2);
      assertGetEqualGreater(now_3, -2);
      assertGetEqualSmaller(now, 0);
      assertGetEqualSmaller(now1, 1);
      assertGetEqualSmaller(now2, 1);
      assertGetEqualSmaller(now3, 1);
      assertGetEqualSmaller(now4, 4);
      assertGetEqualSmaller(now5, 4);
      assertGetEqualSmaller(now_1, -2);
      assertGetEqualSmaller(now_2, -2);
      assertGetEqualSmaller(now_3, -2);
      assertFirstLast(now_2, -2, now4, 4);

      add(now_6, -6);
      assertGetEqualGreater(now, 0);
      assertGetEqualGreater(now1, 1);
      assertGetEqualGreater(now2, 4);
      assertGetEqualGreater(now3, 4);
      assertGetEqualGreater(now4, 4);
      assertGetEqualGreater(now5, 4);
      assertGetEqualGreater(now_1, 0);
      assertGetEqualGreater(now_2, -2);
      assertGetEqualGreater(now_3, -2);
      assertGetEqualGreater(now_4, -2);
      assertGetEqualGreater(now_5, -2);
      assertGetEqualGreater(now_6, -6);
      assertGetEqualGreater(now_7, -6);
      assertGetEqualSmaller(now, 0);
      assertGetEqualSmaller(now1, 1);
      assertGetEqualSmaller(now2, 1);
      assertGetEqualSmaller(now3, 1);
      assertGetEqualSmaller(now4, 4);
      assertGetEqualSmaller(now5, 4);
      assertGetEqualSmaller(now_1, -2);
      assertGetEqualSmaller(now_2, -2);
      assertGetEqualSmaller(now_3, -6);
      assertGetEqualSmaller(now_4, -6);
      assertGetEqualSmaller(now_5, -6);
      assertGetEqualSmaller(now_6, -6);
      assertGetEqualSmaller(now_7, -6);
      assertFirstLast(now_6, -6, now4, 4);
    }


    private void add(DateTime key, int value) {
      sortedListClass.Add(key, new itemClass { Key = key, Value = value });
      sortedListStruct.Add(key, value);
    }


    private void assertGetEqualGreater(DateTime key, int value) {
      Assert.AreEqual(value, sortedListClass.GetEqualGreater(key)!.Value);
      Assert.AreEqual(value, sortedListStruct.GetEqualGreater(key));
      Assert.AreEqual(value, sortedListClass.GetEqualGreaterKVP(key)!.Value.Value.Value);
      Assert.AreEqual(value, sortedListStruct.GetEqualGreaterKVP(key)!.Value.Value);
    }


    private void assertGetEqualSmaller(DateTime key, int value) {
      Assert.AreEqual(value, sortedListClass.GetEqualSmaller(key)!.Value);
      Assert.AreEqual(value, sortedListStruct.GetEqualSmaller(key));
      //Assert.AreEqual(value, sortedListClass.GetEqualSmallerKVP(key)!.Value.Value.Value);
      //Assert.AreEqual(value, sortedListStruct.GetEqualSmallerKVP(key)!.Value.Value);
    }


    private void assertFirstLast(DateTime dateFirst, int valueFirst, DateTime dateLast, int valueLast) {
      Assert.AreEqual(dateFirst, sortedListClass.GetFirstKey());
      Assert.AreEqual(valueFirst, sortedListClass.GetFirstItem()!.Value);
      Assert.AreEqual(dateLast, sortedListClass.GetLastKey());
      Assert.AreEqual(valueLast, sortedListClass.GetLastItem()!.Value);
      Assert.AreEqual(dateFirst, sortedListStruct.GetFirstKey());
      Assert.AreEqual(valueFirst, sortedListStruct.GetFirstItem());
      Assert.AreEqual(dateLast, sortedListStruct.GetLastKey());
      Assert.AreEqual(valueLast, sortedListStruct.GetLastItem());
    }
  }
}
