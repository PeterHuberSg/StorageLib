using System;
using System.Collections.Generic;
using System.Text;
using StorageLib;


namespace StorageTest {


  public class TestItemCsv: IStorageItem<TestItemCsv> {

    public int Key { get; private set; }
    public static void SetKey(IStorageItem testItem, int key, bool /*isRollback*/_) { ((TestItemCsv)testItem).Key = key; }

    public string Text { get; private set; }


    public static readonly string[] Headers = { "Key", "Text" };

    public const int MaxLineLength = 30;

    public event Action<TestItemCsv, TestItemCsv>? HasChanged;


    public TestItemCsv(string text) {
      Key = StorageExtensions.NoKey;
      Text = text;
    }


    public TestItemCsv(int key, CsvReader csvReader) {
      Key = key;
      Text = csvReader.ReadString()!;
    }


    public void Store() {
      throw new NotSupportedException();
    }


    public static TestItemCsv Create(int key, CsvReader csvReader) {
      return new TestItemCsv(key, csvReader);
    }


    internal static void Write(TestItemCsv testItemCsv, CsvWriter csvWriter) {
      csvWriter.Write(testItemCsv.Text);
    }


    internal static void Update(TestItemCsv testItemCsv, CsvReader csvReader) 
    {
      testItemCsv.Text = csvReader.ReadString()!;
    }


    public void Update(string text, DataStore<TestItemCsv> dataStore) {
      if (Text!=text) {
        var old = new TestItemCsv(Text);
        Text = text;
        dataStore.ItemHasChanged(old, this);
        HasChanged?.Invoke(old, this);
      }
    }


    public void Release() {
      throw new NotImplementedException();
    }


    public void Remove(DataStore<TestItemCsv> dataStore) {
      if (Key<0) {
        throw new Exception($"TestItemCsv.Remove(): TestItemCsv is not in storageDictionary, key is {Key}.");
      }
      dataStore.Remove(Key);
    }


    internal static void Disconnect(TestItemCsv _) {
      //nothing to do
    }


    internal static void RollbackItemStore(IStorageItem _) {
      throw new NotSupportedException();
    }


    #pragma warning disable IDE0060 // Remove unused parameter oldItem, newItem
    internal static void RollbackItemUpdate(IStorageItem oldItem, IStorageItem newItem) {
    #pragma warning restore IDE0060
      throw new NotSupportedException();
    }


    internal static void RollbackItemRelease(IStorageItem _) {
      throw new NotSupportedException();
    }


    public string ToTraceString() {
      return $"{Key.ToKeyString()}, {Text};";
    }


    public string ToShortString() {
      return $"{Key.ToKeyString()}, {Text};";
    }


    public override string ToString() {
      return $"Key: {Key.ToKeyString()}; Text: {Text};";
    }
  }
}
