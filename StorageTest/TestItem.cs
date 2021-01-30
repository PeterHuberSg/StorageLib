using System;
using System.Collections.Generic;
using System.Text;
using StorageLib;


namespace StorageTest {


  public class TestItem: IStorageItem<TestItem> {
    public int Key { get; private set; }
    public static void SetKey(IStorageItem testItem, int key, bool /*isRollback*/_) { ((TestItem)testItem).Key = key; }

    public string Text { get; private set; }


    public event Action<TestItem, TestItem>? HasChanged;


    public TestItem(string text) {
      Key = StorageExtensions.NoKey;
      Text = text;
    }


    public void Store() {
      throw new NotSupportedException();
    }


    public void Update(string text) {
      if (Text!=text) {
        var old = new TestItem(Text);
        Text = text;
        HasChanged?.Invoke(old, this);
      }
    }


    public void Release() {
      throw new NotImplementedException();
    }


    public void Remove(DataStore<TestItem> dataStore) {
      if (Key<0) {
        throw new Exception($"TestItem.Remove(): TestItem '{this}' is not stored in storageDictionary, key is {Key}.");
      }
      dataStore.Remove(Key);
    }


    internal static void Disconnect(TestItem _) {
      //nothing to do
    }


    internal static void RollbackItemNew(IStorageItem _) {
    }


    internal static void RollbackItemStore(IStorageItem _) {
    }


    #pragma warning disable IDE0060 // Remove unused parameter
    internal static void RollbackItemUpdate(IStorageItem oldItem, IStorageItem newItem) {
    #pragma warning restore IDE0060 // Remove unused parameter
    }


    internal static void RollbackItemRelease(IStorageItem _) {
    }


    public string ToTraceString() {
      return $"{Key.ToKeyString()}, {Text};";
    }


    public string ToShortString() {
      return $"{Key.ToKeyString()}, {Text};";
    }


    public override string ToString() {
      return $"Key: {Key}; Text: {Text};";
    }
  }
}
