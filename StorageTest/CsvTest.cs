using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageLib;


namespace StorageTest {


  [TestClass]
  public class CsvTest {


    [TestMethod]
    public void TestCsv() {
      assertInt(int.MinValue);
      assertInt(-1);
      assertInt(0);
      assertInt(1);
      assertInt(int.MaxValue);
      assertIntFailed("");
      assertIntFailed(int.MinValue.ToString()+"1");
      assertIntFailed(int.MaxValue.ToString()+"1");

      assertIntNull(null);
      assertIntNull(int.MinValue);
      assertIntNull(-1);
      assertIntNull(0);
      assertIntNull(1);
      assertIntNull(int.MaxValue);
      assertIntNullFailed(int.MinValue.ToString()+"1");
      assertIntNullFailed(int.MaxValue.ToString()+"1");

      assertDecimal(Decimal.MinValue);
      assertDecimal(-1);
      assertDecimal(0);
      assertDecimal(1);
      assertDecimal(Decimal.MaxValue);
      assertDecimalFailed("");
      assertDecimalFailed(Decimal.MinValue.ToString()+"1");
      assertDecimalFailed(Decimal.MaxValue.ToString()+"1");

      assertDecimalNull(null);
      assertDecimalNull(Decimal.MinValue);
      assertDecimalNull(-1);
      assertDecimalNull(0);
      assertDecimalNull(1);
      assertDecimalNull(Decimal.MaxValue);
      assertDecimalNullFailed(Decimal.MinValue.ToString()+"1");
      assertDecimalNullFailed(Decimal.MaxValue.ToString()+"1");

      assertDate(DateTime.MinValue);
      assertDate(DateTime.Now.Date);
      assertDate(DateTime.MaxValue.Date.AddHours(23));
      assertDateFailed("");
      assertDateFailed(DateTime.MinValue.ToString()+"1");
      assertDateFailed(DateTime.MaxValue.ToString()+"1");

      assertDateNull(null);
      assertDateNull(DateTime.MinValue);
      assertDateNull(DateTime.Now.Date);
      assertDateNull(DateTime.MaxValue.Date.AddHours(23));
      assertDateNullFailed(DateTime.MinValue.ToString()+"1");
      assertDateNullFailed(DateTime.MaxValue.ToString()+"1");
    }


    private void assertInt(int i) {
      var field = i.ToString();
      var errorStringBuilder = new StringBuilder();
      var i2 = Csv.ParseInt("Test", field, field, errorStringBuilder);
      Assert.AreEqual(i, i2);
      Assert.AreEqual(0, errorStringBuilder.Length);
    }


    private void assertIntFailed(string field) {
      var errorStringBuilder = new StringBuilder();
      Csv.ParseInt("Test", field, field, errorStringBuilder);
      Assert.AreNotEqual(0, errorStringBuilder.Length);
    }


    private void assertIntNull(int? i) {
      string field = i.ToString()??"";
      var errorStringBuilder = new StringBuilder();
      var i2 = Csv.ParseIntNull("Test", field, field, errorStringBuilder);
      Assert.AreEqual(i, i2);
      Assert.AreEqual(0, errorStringBuilder.Length);
    }


    private void assertIntNullFailed(string field) {
      var errorStringBuilder = new StringBuilder();
      Csv.ParseIntNull("Test", field, field, errorStringBuilder);
      Assert.AreNotEqual(0, errorStringBuilder.Length);
    }


    private void assertDecimal(Decimal i) {
      var field = i.ToString();
      var errorStringBuilder = new StringBuilder();
      var i2 = Csv.ParseDecimal("Test", field, field, errorStringBuilder);
      Assert.AreEqual(i, i2);
      Assert.AreEqual(0, errorStringBuilder.Length);
    }


    private void assertDecimalFailed(string field) {
      var errorStringBuilder = new StringBuilder();
      Csv.ParseDecimal("Test", field, field, errorStringBuilder);
      Assert.AreNotEqual(0, errorStringBuilder.Length);
    }


    private void assertDecimalNull(Decimal? i) {
      var field = i.ToString()??"";
      var errorStringBuilder = new StringBuilder();
      var i2 = Csv.ParseDecimalNull("Test", field, field, errorStringBuilder);
      Assert.AreEqual(i, i2);
      Assert.AreEqual(0, errorStringBuilder.Length);
    }


    private void assertDecimalNullFailed(string field) {
      var errorStringBuilder = new StringBuilder();
      Csv.ParseDecimalNull("Test", field, field, errorStringBuilder);
      Assert.AreNotEqual(0, errorStringBuilder.Length);
    }


    private void assertDate(DateTime i) {
      var field = i.ToCompactDateString();
      var errorStringBuilder = new StringBuilder();
      var i2 = Csv.ParseDateTime("Test", field, field, errorStringBuilder);
      Assert.AreEqual(i.Date, i2);
      Assert.AreEqual(0, errorStringBuilder.Length);
    }


    private void assertDateFailed(string field) {
      var errorStringBuilder = new StringBuilder();
      Csv.ParseDateTime("Test", field, field, errorStringBuilder);
      Assert.AreNotEqual(0, errorStringBuilder.Length);
    }


    private void assertDateNull(DateTime? i) {
      var field = i.ToCompactDateString();
      var errorStringBuilder = new StringBuilder();
      var i2 = Csv.ParseDateTimeNull("Test", field, field, errorStringBuilder);
      Assert.AreEqual(0, errorStringBuilder.Length);
      Assert.AreEqual(i.HasValue, i2.HasValue);
      if (!i.HasValue) return;

      Assert.AreEqual(i.Value.Date, i2);
    }


    private void assertDateNullFailed(string field) {
      var errorStringBuilder = new StringBuilder();
      Csv.ParseDateTimeNull("Test", field, field, errorStringBuilder);
      Assert.AreNotEqual(0, errorStringBuilder.Length);
    }

    #region Test Equality
    //      -------------

    [TestMethod]
    public void TestCsvEquality() {
      Assert.IsTrue(Csv.AreEqual(null, null));
      Assert.IsTrue(Csv.AreEqual(null, ""));
      Assert.IsTrue(Csv.AreEqual("", null));
      Assert.IsTrue(Csv.AreEqual("", ""));
      Assert.IsTrue(Csv.AreEqual("a", "a"));
      Assert.IsFalse(Csv.AreEqual(null, " "));
      Assert.IsFalse(Csv.AreEqual(" ", null));
      Assert.IsFalse(Csv.AreEqual(" ", ""));
      Assert.IsFalse(Csv.AreEqual("a", "A"));
      Assert.IsFalse(Csv.AreEqual(null, "ABC"));
    }
    #endregion

  }
}
