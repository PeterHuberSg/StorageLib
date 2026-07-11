using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageLib;
using System;
using System.Collections.Generic;
using System.Linq;


namespace StorageTest {


  [TestClass]
  public class SortedBucket1KeyTest {

    //Id is not a key. The single key collection uses only Date as key1. Id just makes each item unique and
    //identifiable for the test bookkeeping.
    public record Item(DateTime Date, int Id, string Description);

    readonly SortedBucketCollection<DateTime, Item> items = new(i => i.Date);
    const int daysCount = 7;
    //oracle: each day holds its items in insertion order, so a List is used, not a SortedList
    readonly List<Item>[] expectedItems = new List<Item>[daysCount];
    static readonly DateTime day0 = DateTime.Now.Date;
    static readonly DateTime day1 = day0.AddDays(1);
    static readonly DateTime day2 = day1.AddDays(1);
    static readonly DateTime day3 = day2.AddDays(1);
    static readonly DateTime day4 = day3.AddDays(1);
    static readonly DateTime day5 = day4.AddDays(1);


    [TestMethod]
    public void TestSortedBucket1Key() {
      for (int itemIndex = 0; itemIndex<expectedItems.Length; itemIndex++) {
        expectedItems[itemIndex] = new List<Item>();
      }

      assertItems();

      addItem(day2, 10);
      assertItems();

      //same day, added 28 before 26: insertion order must stay 28, 26 (not sorted by Id)
      addItem(day5, 28);
      addItem(day5, 26);
      assertItems();

      //same day, descending Id: insertion order must stay 38, 36, 35
      addItem(day4, 38);
      addItem(day4, 36);
      addItem(day4, 35);
      assertItems();

      //remove the middle item, the remaining items keep their insertion order
      removeItem(day4, 36);
      assertItems();

      removeItem(day4, 35);
      assertItems();

      removeItem(day4, 38);
      assertItems();

      items.Clear();
      for (int itemIndex = 0; itemIndex<expectedItems.Length; itemIndex++) {
        expectedItems[itemIndex].Clear();
      }
      assertItems();

      for (int i = 0; i<100; i++) {
        addItem(day0.AddDays(i % daysCount), i);
        assertItems();
      }
    }


    private void addItem(DateTime day, int id) {
      var item = new Item(day, id, $"{day:dd.MM.yyyy} {id}");
      items.Add(item);
      expectedItems[(day-day0).Days].Add(item);
    }


    private void removeItem(DateTime day, int id) {
      var dayItems = expectedItems[(day-day0).Days];
      var item = dayItems.Single(i => i.Id==id);
      dayItems.Remove(item);
      Assert.IsTrue(items.Remove(item));
    }


    private void assertItems() {
      var dateKeysList = items.Keys.ToList();
      var dateKeysListIndex = 0;
      var allItemsList = items.ToList();//this tests also CopyTo(), ToList() depends on it
      var allItemsListIndex = 0;
      var expectedItemsCount = 0;
      for (int dayIndex = 0; dayIndex<expectedItems.Length; dayIndex++) {
        var day = day0.AddDays(dayIndex);
        var dayList = expectedItems[dayIndex];
        if (dayList.Count==0) {
          Assert.IsFalse(items.Contains(day));

        } else {
          Assert.AreEqual(dateKeysList[dateKeysListIndex++], day);
          //an item which was never added is not contained, even though its Date exists
          Assert.IsFalse(items.Contains(new Item(day, -1, "notAdded")));
          var dayItems = items[day].ToList();
          Assert.AreEqual(dayList.Count, dayItems.Count);
          var dayListIndex = 0;
          foreach (var expectedItem in dayList) {
            expectedItemsCount++;
            Assert.IsTrue(items.Contains(expectedItem.Date));
            Assert.IsTrue(items.Contains(expectedItem));
            Assert.AreEqual(expectedItem, dayItems[dayListIndex++]);
            Assert.AreEqual(expectedItem, allItemsList[allItemsListIndex++]);
          }
        }
      }

      Assert.AreEqual(dateKeysListIndex, dateKeysList.Count);
      Assert.AreEqual(dateKeysListIndex, items.Key1Count);
      Assert.AreEqual(expectedItemsCount, items.Count);

      //test the key1 range indexer for every day range, including empty ranges where day1Index>day2Index
      for (int day1Index = 0; day1Index<expectedItems.Length; day1Index++) {
        for (int day2Index = 0; day2Index<expectedItems.Length; day2Index++) {
          var dayLower = day0.AddDays(day1Index);
          var dayHigher = day0.AddDays(day2Index);
          var expectedRange = new List<Item>();
          for (int dayRangeIndex = day1Index; dayRangeIndex<=day2Index; dayRangeIndex++) {
            expectedRange.AddRange(expectedItems[dayRangeIndex]);
          }
          CollectionAssert.AreEqual(expectedRange, items[dayLower, dayHigher].ToList());
        }
      }
    }
  }
}