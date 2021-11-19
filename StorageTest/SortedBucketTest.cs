using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;


namespace StorageTest {

  [TestClass]
  public class SortedBucketTest {

    public record Item(DateTime Date, int Key2, string Description);

    readonly SortedBucketCollection<DateTime, int, Item> items = new(i => i.Date, i => i.Key2);
    const int daysCount = 7;
    readonly SortedList<int, Item>[]expectedItems = new SortedList<int, Item>[daysCount];
    static readonly DateTime day0 = DateTime.Now.Date;
    static readonly DateTime day1 = day0.AddDays(1);
    static readonly DateTime day2 = day1.AddDays(1);
    static readonly DateTime day3 = day2.AddDays(1);
    static readonly DateTime day4 = day3.AddDays(1);
    static readonly DateTime day5 = day4.AddDays(1);
    static readonly DateTime day6 = day5.AddDays(1);


    [TestMethod]
    public void TestSortedBucket() {
      for (int itemIndex = 0; itemIndex < expectedItems.Length; itemIndex++) {
        expectedItems[itemIndex] = new SortedList<int, Item>();
      }

      assertItems();

      addItem(day2, 10);
      assertItems();

      addItem(day5, 28);
      addItem(day5, 26);
      assertItems();

      addItem(day4, 38);
      addItem(day4, 36);
      addItem(day4, 35);
      assertItems();

      removeItem(day4, 36);
      assertItems();
      
      removeItem(day4, 35);
      assertItems();
      
      removeItem(day4, 38);
      assertItems();

      items.Clear();
      for (int itemIndex = 0; itemIndex < expectedItems.Length; itemIndex++) {
        expectedItems[itemIndex].Clear();
      }
      assertItems();

      for (int i = 0; i < 100; i++) {
        addItem(day0.AddDays(i % daysCount), i);
        assertItems();
      }
    }


    private void addItem(DateTime day, int uniqueKey) {
      var item = new Item(day, uniqueKey, $"{day:dd.MM.yyyy} {uniqueKey}");
      items.Add(item);
      expectedItems[(day-day0).Days].Add(uniqueKey, item);
    }


    private void removeItem(DateTime day, int uniqueKey) {
      var dayIndex = (day - day0).Days;
      var dayItems = expectedItems[dayIndex];
      var item = dayItems[uniqueKey];
      dayItems.Remove(uniqueKey);
      items.Remove(item);
    }


    List<Item>[] allItems = initialiseAllItems();


    private static List<Item>[] initialiseAllItems() {
      var items = new List<Item>[daysCount];
      for (int itemsIndex = 0; itemsIndex < items.Length; itemsIndex++) {
        items[itemsIndex] = new();
      }
      return items;
    }


    private void assertItems() {
      var dateKeysList = items.Keys.ToList();
      var dateKeysListIndex = 0;
      var allItemsList = items.ToList();//this tests also CopyTo(), ToList() depends on it
      var allItemsListIndex = 0;
      var expectedItemsCount = 0;
      for (int dayIndex = 0; dayIndex < expectedItems.Length; dayIndex++) {
        var day = day0.AddDays(dayIndex);
        var dayList = expectedItems[dayIndex];
        if (dayList.Count==0) {
          Assert.IsFalse(items.Contains(day));
          Assert.IsFalse(items.Contains(day, 12345678));
          Assert.IsFalse(items.Contains(day));
          Assert.IsFalse(items.TryGetValue(day, 87654321, out var itemFound));
          Assert.IsNull(itemFound);

        } else {
          Assert.AreEqual(dateKeysList[dateKeysListIndex++], day);
          var dayItems = items[day].ToList();
          var dayListIndex = 0;
          foreach (var KeyValuePairItem in dayList) {
            var item = KeyValuePairItem.Value;
            expectedItemsCount++;
            Assert.IsTrue(items.Contains(item.Date));
            Assert.IsTrue(items.Contains(item.Date, item.Key2));
            Assert.IsTrue(items.Contains(item));
            Assert.IsTrue(items.TryGetValue(item.Date, item.Key2, out var itemFound));
            Assert.AreEqual(item, itemFound);
            Assert.AreEqual(item, items[item.Date, item.Key2]);
            Assert.AreEqual(item, dayItems[dayListIndex++]);
            Assert.AreEqual(item, allItemsList[allItemsListIndex++]);
          }
        }
      }

      Assert.AreEqual(dateKeysListIndex, dateKeysList.Count);
      Assert.AreEqual(dateKeysListIndex, items.Key1Count);
      Assert.AreEqual(expectedItemsCount, items.Count);

      for (int day1Index = 0; day1Index<expectedItems.Length; day1Index++) {
        for (int day2Index = 0; day2Index<expectedItems.Length; day2Index++) {
          var day1 = day0.AddDays(day1Index);
          var day2 = day0.AddDays(day2Index);
          var dayRangeItems = items[day1, day2].ToArray();
          for (int dayRangeIndex = day1Index; dayRangeIndex<=day2Index; dayRangeIndex++) {
            var dayList = expectedItems[dayRangeIndex];
            if (dayList.Count>0) {
              foreach (var KeyValuePairItem in dayList) {
                var item = KeyValuePairItem.Value;
                Assert.AreEqual(item, items[item.Date, item.Key2]);
              }
            }
          }
        }
      }
    }
  }
}
