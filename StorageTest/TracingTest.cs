//Todo: rewrite tracing test with simple parent child relationship

//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.IO;
//using System.Text;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using StorageLib;
//using StorageDataContext;


//namespace StorageTest {


//  [TestClass]
//  public class TracingTest {


//    [TestMethod]
//    public void TestTracing() {
//      var directoryInfo = new DirectoryInfo("TestCsv");
//      if (directoryInfo.Exists) {
//        directoryInfo.Delete(recursive: true);
//        directoryInfo.Refresh();
//      }

//      directoryInfo.Create();

//      ////force static NoXxxClass variables to get initialised, preventing further tracing
//      //var pNo = new ChildrenList_Parent("pNo", isStoring: false);
//      //var pcNo = new ChildrenList_CreateOnlyParent("pcNo", isStoring: false);
//      //var cNo = new ChildrenList_Child("cNo", pNo, null, pcNo, null, isStoring: false);

//      DC.Trace = dcTraceAssert;
//      //DC.Trace = dcTraceCollect;

//      var csvConfig = new CsvConfig(directoryInfo.FullName, reportException: reportException);
//      eTM.Enqueue("Context DC initialising");
//      eTM.Enqueue("Context DC initialised");
//      trace("using (new DC(csvConfig)) {");
//      using (new DC(csvConfig)) {

//        //without storing
//        //---------------
//        traceHeader("=== Without storing ===");
//        traceHeader("--- Construct parents 2 ---");
//        eTM.Enqueue("new ChildrenList_Parent: #| P2");
//        trace("var p2 = new ChildrenList_Parent(\"P2\", isStoring: false);");
//        var p2 = new ChildrenList_Parent("P2", isStoring: false);
//        eTM.Enqueue("new ChildrenList_ParentReadonly: #| P2r");
//        trace("var p2r = new ChildrenList_ParentReadonly(\"P2r\", isStoring: false);");
//        var p2r = new ChildrenList_ParentReadonly("P2r", isStoring: false);

//        eTM.Enqueue("new ChildrenList_ParentNullable: #| PN2");
//        trace("var pn2 = new ChildrenList_ParentNullable(\"PN2\", isStoring: false);");
//        var pn2 = new ChildrenList_ParentNullable("PN2", isStoring: false);
//        eTM.Enqueue("new ChildrenList_ParentNullableReadonly: #| PN2r");
//        trace("var pn2r = new ChildrenList_ParentNullableReadonly(\"PN2r\", isStoring: false);");
//        var pn2r = new ChildrenList_ParentNullableReadonly("PN2r", isStoring: false);

//        eTM.Enqueue("new ChildrenList_CreateOnlyParent: #| PC2");
//        trace("var pc2 = new ChildrenList_CreateOnlyParent(\"PC2\", isStoring: false);");
//        var pc2 = new ChildrenList_CreateOnlyParent("PC2", isStoring: false);
//        eTM.Enqueue("new ChildrenList_CreateOnlyParentReadonly: #| PC2r");
//        trace("var pc2r = new ChildrenList_CreateOnlyParentReadonly(\"PC2r\", isStoring: false);");
//        var pc2r = new ChildrenList_CreateOnlyParentReadonly("PC2r", isStoring: false);

//        eTM.Enqueue("new ChildrenList_CreateOnlyParentNullable: #| PCN2");
//        trace("var pcn2 = new ChildrenList_CreateOnlyParentNullable(\"PCN2\", isStoring: false);");
//        var pcn2 = new ChildrenList_CreateOnlyParentNullable("PCN2", isStoring: false);
//        eTM.Enqueue("new ChildrenList_CreateOnlyParentNullableReadonly: #| PCN2r");
//        trace("var pcn2r = new ChildrenList_CreateOnlyParentNullableReadonly(\"PCN2r\", isStoring: false);");
//        var pcn2r = new ChildrenList_CreateOnlyParentNullableReadonly("PCN2r", isStoring: false);


//        traceHeader("--- Construct child 1 ---");
//        eTM.Enqueue("new ChildrenList_Child: #| C1| Parent #| ParentNullable #| CreateOnlyParent #| CreateOnlyParentNullable #");
//        eTM.Enqueue("Add ChildrenList_Child # to # ChildrenList_Parent.ChildrenList_Children");
//        eTM.Enqueue("Add ChildrenList_Child # to # ChildrenList_ParentNullable.ChildrenList_Children");
//        eTM.Enqueue("Add ChildrenList_Child # to # ChildrenList_CreateOnlyParent.ChildrenList_Children");
//        eTM.Enqueue("Add ChildrenList_Child # to # ChildrenList_CreateOnlyParentNullable.ChildrenList_Children");
//        trace("var c1 = new ChildrenList_Child(\"C1\", p2, pn2, pc2, pcn2, isStoring: false);");
//        var c1 = new ChildrenList_Child("C1", p2, p2r, pn2, pn2r, pc2, pc2r, pcn2, pcn2r, isStoring: false);


//        traceHeader("--- Construct parents 3 ---");
//        eTM.Enqueue("new ChildrenList_Parent: #| P3");
//        trace("var p3 = new ChildrenList_Parent(\"P3\", isStoring: false);");
//        var p3 = new ChildrenList_Parent("P3", isStoring: false);
//        //eTM.Enqueue("new ChildrenList_Parent: #| P3r");
//        //trace("var p3r = new ChildrenList_Parent(\"P3r\", isStoring: false);");
//        //var p3r = new ChildrenList_Parent("P3r", isStoring: false);

//        eTM.Enqueue("new ChildrenList_CreateOnlyParent: #| PC3");
//        trace("var pc3 = new ChildrenList_CreateOnlyParent(\"PC3\", isStoring: false);");
//        var pc3 = new ChildrenList_CreateOnlyParent("PC3", isStoring: false);
//        //eTM.Enqueue("new ChildrenList_CreateOnlyParent: #| PC3r");
//        //trace("var pc3r = new ChildrenList_CreateOnlyParent(\"PC3r\", isStoring: false);");
//        //var pc3r = new ChildrenList_CreateOnlyParent("PC3r", isStoring: false);


//        traceHeader("--- Update child 1 ---");
//        eTM.Enqueue("Updating ChildrenList_Child: #| C1| Parent #| ParentNullable #| CreateOnlyParent #| CreateOnlyParentNullable #");
//        eTM.Enqueue("Remove ChildrenList_Child # from # ChildrenList_Parent.ChildrenList_Children");
//        eTM.Enqueue("Add ChildrenList_Child # to # ChildrenList_Parent.ChildrenList_Children");
//        eTM.Enqueue("Remove ChildrenList_Child # from # ChildrenList_ParentNullable.ChildrenList_Children");
//        eTM.Enqueue("Remove ChildrenList_Child # from # ChildrenList_CreateOnlyParent.ChildrenList_Children");
//        eTM.Enqueue("Add ChildrenList_Child # to # ChildrenList_CreateOnlyParent.ChildrenList_Children");
//        eTM.Enqueue("Remove ChildrenList_Child # from # ChildrenList_CreateOnlyParentNullable.ChildrenList_Children");
//        eTM.Enqueue("Updated ChildrenList_Child: #| C1| Parent #| ParentNullable | CreateOnlyParent #| CreateOnlyParentNullable ");
//        trace("c1.Update(c1.Text, p3, null, pc3, null);");
//        c1.Update(c1.Text, p3, null, pc3, null);


//        traceHeader("--- Construct parents 3 nullable---");
//        eTM.Enqueue("new ChildrenList_ParentNullable: #| PN3");
//        trace("var pn3 = new ChildrenList_ParentNullable(\"PN3\", isStoring: false);");
//        var pn3 = new ChildrenList_ParentNullable("PN3", isStoring: false);
//        eTM.Enqueue("new ChildrenList_CreateOnlyParentNullable: #| PCN3");
//        trace("var pcn3 = new ChildrenList_CreateOnlyParentNullable(\"PCN3\", isStoring: false);");
//        var pcn3 = new ChildrenList_CreateOnlyParentNullable("PCN3", isStoring: false);

//        traceHeader("--- Update child 1 ---");
//        eTM.Enqueue("Updating ChildrenList_Child: #| C1| Parent #| ParentNullable | CreateOnlyParent #| CreateOnlyParentNullable ");
//        eTM.Enqueue("Add ChildrenList_Child # to # ChildrenList_ParentNullable.ChildrenList_Children");
//        eTM.Enqueue("Add ChildrenList_Child # to # ChildrenList_CreateOnlyParentNullable.ChildrenList_Children");
//        eTM.Enqueue("Updated ChildrenList_Child: #| C1| Parent #| ParentNullable #| CreateOnlyParent #| CreateOnlyParentNullable #");
//        trace("c1.Update(c1.Text, p3, pn3, pc3, pcn3);");
//        c1.Update(c1.Text, p3, pn3, pc3, pcn3);

//        //with storing
//        //------------
//        traceHeader("=== With storing ===");
//        traceHeader("--- Construct and store parents 0 ---");
//        eTM.Enqueue("new ChildrenList_Parent: #| P0");
//        eTM.Enqueue("Stored ChildrenList_Parent # @0");
//        trace("var p0 = new ChildrenList_Parent(\"P0\");");
//        var p0 = new ChildrenList_Parent("P0");
//        eTM.Enqueue("new ChildrenList_ParentReadonly: #| P0r");
//        eTM.Enqueue("Stored ChildrenList_ParentReadonly # @0");
//        trace("var p0r = new ChildrenList_ParentReadonly(\"P0r\");");
//        var p0r = new ChildrenList_ParentReadonly("P0r");

//        eTM.Enqueue("new ChildrenList_ParentNullable: #| PN0");
//        eTM.Enqueue("Stored ChildrenList_ParentNullable # @0");
//        trace("var pn0 = new ChildrenList_ParentNullable(\"PN0\");");
//        var pn0 = new ChildrenList_ParentNullable("PN0");
//        eTM.Enqueue("new ChildrenList_ParentNullableReadonly: #| PN0r");
//        eTM.Enqueue("Stored ChildrenList_ParentNullableReadonly # @0");
//        trace("var pn0r = new ChildrenList_ParentNullableReadonly(\"PN0r\");");
//        var pn0r = new ChildrenList_ParentNullableReadonly("PN0r");

//        eTM.Enqueue("new ChildrenList_CreateOnlyParent: #| PC0");
//        eTM.Enqueue("Stored ChildrenList_CreateOnlyParent # @0");
//        trace("var pc0 = new ChildrenList_CreateOnlyParent(\"PC0\");");
//        var pc0 = new ChildrenList_CreateOnlyParent("PC0");
//        eTM.Enqueue("new ChildrenList_CreateOnlyParentReadonly: #| PC0r");
//        eTM.Enqueue("Stored ChildrenList_CreateOnlyParentReadonly # @0");
//        trace("var pc0r = new ChildrenList_CreateOnlyParentReadonly(\"PC0r\");");
//        var pc0r = new ChildrenList_CreateOnlyParentReadonly("PC0r");

//        eTM.Enqueue("new ChildrenList_CreateOnlyParentNullable: #| PCN0");
//        eTM.Enqueue("Stored ChildrenList_CreateOnlyParentNullable # @0");
//        trace("var pcn0 = new ChildrenList_CreateOnlyParentNullable(\"PCN0\");");
//        var pcn0 = new ChildrenList_CreateOnlyParentNullable("PCN0");
//        eTM.Enqueue("new ChildrenList_CreateOnlyParentNullableReadonly: #| PCN0r");
//        eTM.Enqueue("Stored ChildrenList_CreateOnlyParentNullableReadonly # @0");
//        trace("var pcn0r = new ChildrenList_CreateOnlyParentNullableReadonly(\"PCN0r\");");
//        var pcn0r = new ChildrenList_CreateOnlyParentNullableReadonly("PCN0r");

//        traceHeader("--- Construct and store child 0 ---");
//        eTM.Enqueue("Start transaction");
//        DC.Data.StartTransaction();
//        eTM.Enqueue("new ChildrenList_Child: #| C0| Parent @0| ParentNullable @0| CreateOnlyParent @0| CreateOnlyParentNullable @0");
//        eTM.Enqueue("Add ChildrenList_Child # to @0 ChildrenList_Parent.ChildrenList_Children");
//        eTM.Enqueue("Add ChildrenList_Child # to @0 ChildrenList_ParentNullable.ChildrenList_Children");
//        eTM.Enqueue("Add ChildrenList_Child # to @0 ChildrenList_CreateOnlyParent.ChildrenList_Children");
//        eTM.Enqueue("Add ChildrenList_Child # to @0 ChildrenList_CreateOnlyParentNullable.ChildrenList_Children");
//        eTM.Enqueue("Stored ChildrenList_Child # @0");
//        trace("var c0 = new ChildrenList_Child(\"C0\", p0, pn0, pc0, pcn0);");
//        var c0 = new ChildrenList_Child("C0", p0, p0r, pn0, pn0r, pc0, pc0r, pcn0, pcn0r);
//        eTM.Enqueue("Rolling back transaction");
//        eTM.Enqueue("Rollback ChildrenList_Child.Store(): @0| C0| Parent @0| ParentNullable @0| CreateOnlyParent @0| CreateOnlyParentNullable @0");
//        eTM.Enqueue("Release ChildrenList_Child key @0 #");
//        eTM.Enqueue("Rollback new ChildrenList_Child(): #| C0| Parent @0| ParentNullable @0| CreateOnlyParent @0| CreateOnlyParentNullable @0");
//        eTM.Enqueue("Remove ChildrenList_Child # from @0 ChildrenList_Parent.ChildrenList_Children");
//        eTM.Enqueue("Remove ChildrenList_Child # from @0 ChildrenList_ParentNullable.ChildrenList_Children");
//        eTM.Enqueue("Remove ChildrenList_Child # from @0 ChildrenList_CreateOnlyParent.ChildrenList_Children");
//        eTM.Enqueue("Remove ChildrenList_Child # from @0 ChildrenList_CreateOnlyParentNullable.ChildrenList_Children");
//        eTM.Enqueue("Rolled back transaction");
//        DC.Data.RollbackTransaction();
//        eTM.Enqueue("Start transaction");
//        DC.Data.StartTransaction();
//        eTM.Enqueue("new ChildrenList_Child: #| C0| Parent @0| ParentNullable @0| CreateOnlyParent @0| CreateOnlyParentNullable @0");
//        eTM.Enqueue("Add ChildrenList_Child # to @0 ChildrenList_Parent.ChildrenList_Children");
//        eTM.Enqueue("Add ChildrenList_Child # to @0 ChildrenList_ParentNullable.ChildrenList_Children");
//        eTM.Enqueue("Add ChildrenList_Child # to @0 ChildrenList_CreateOnlyParent.ChildrenList_Children");
//        eTM.Enqueue("Add ChildrenList_Child # to @0 ChildrenList_CreateOnlyParentNullable.ChildrenList_Children");
//        eTM.Enqueue("Stored ChildrenList_Child # @0");
//        trace("var c0 = new ChildrenList_Child(\"C0\", p0, pn0, pc0, pcn0);");
//        c0 = new ChildrenList_Child("C0", p0, p0r, pn0, pn0r, pc0, pc0r, pcn0, pcn0r);
//        eTM.Enqueue("Commit transaction");
//        DC.Data.CommitTransaction();

//        traceHeader("--- Construct and store parents 1 ---");
//        eTM.Enqueue("new ChildrenList_Parent: #| P1");
//        eTM.Enqueue("Stored ChildrenList_Parent # @1");
//        trace("var p1 = new ChildrenList_Parent(\"P1\");");
//        var p1 = new ChildrenList_Parent("P1");
//        eTM.Enqueue("new ChildrenList_CreateOnlyParent: #| PC1");
//        eTM.Enqueue("Stored ChildrenList_CreateOnlyParent # @1");
//        trace("var pc1 = new ChildrenList_CreateOnlyParent(\"PC1\");");
//        var pc1 = new ChildrenList_CreateOnlyParent("PC1");

//        traceHeader("--- Update child 0 ---");
//        eTM.Enqueue("Start transaction");
//        DC.Data.StartTransaction();
//        eTM.Enqueue("Updating ChildrenList_Child: @0| C0| Parent @0| ParentNullable @0| CreateOnlyParent @0| CreateOnlyParentNullable @0");
//        eTM.Enqueue("Remove ChildrenList_Child @0 from @0 ChildrenList_Parent.ChildrenList_Children");
//        eTM.Enqueue("Add ChildrenList_Child @0 to @1 ChildrenList_Parent.ChildrenList_Children");
//        eTM.Enqueue("Remove ChildrenList_Child @0 from @0 ChildrenList_ParentNullable.ChildrenList_Children");
//        eTM.Enqueue("Remove ChildrenList_Child @0 from @0 ChildrenList_CreateOnlyParent.ChildrenList_Children");
//        eTM.Enqueue("Add ChildrenList_Child @0 to @1 ChildrenList_CreateOnlyParent.ChildrenList_Children");
//        eTM.Enqueue("Remove ChildrenList_Child @0 from @0 ChildrenList_CreateOnlyParentNullable.ChildrenList_Children");
//        eTM.Enqueue("Updated ChildrenList_Child: @0| C0| Parent @1| ParentNullable | CreateOnlyParent @1| CreateOnlyParentNullable ");
//        trace("c0.Update(c0.Text, p1, null, pc1, null);");
//        c0.Update(c0.Text, p1, null, pc1, null);
//        eTM.Enqueue("Rolling back transaction");
//        eTM.Enqueue("Rolling back ChildrenList_Child.Update(): @0| C0| Parent @1| ParentNullable | CreateOnlyParent @1| CreateOnlyParentNullable ");
//        eTM.Enqueue("Remove ChildrenList_Child @0 from @1 ChildrenList_Parent.ChildrenList_Children");
//        eTM.Enqueue("Add ChildrenList_Child @0 to @0 ChildrenList_Parent.ChildrenList_Children");
//        eTM.Enqueue("Add ChildrenList_Child @0 to @0 ChildrenList_ParentNullable.ChildrenList_Children");
//        eTM.Enqueue("Remove ChildrenList_Child @0 from @1 ChildrenList_CreateOnlyParent.ChildrenList_Children");
//        eTM.Enqueue("Add ChildrenList_Child @0 to @0 ChildrenList_CreateOnlyParent.ChildrenList_Children");
//        eTM.Enqueue("Add ChildrenList_Child @0 to @0 ChildrenList_CreateOnlyParentNullable.ChildrenList_Children");
//        eTM.Enqueue("Rolled back ChildrenList_Child.Update(): @0| C0| Parent @0| ParentNullable @0| CreateOnlyParent @0| CreateOnlyParentNullable @0");
//        eTM.Enqueue("Rolled back transaction");
//        DC.Data.RollbackTransaction();
//        eTM.Enqueue("Start transaction");
//        DC.Data.StartTransaction();
//        eTM.Enqueue("Updating ChildrenList_Child: @0| C0| Parent @0| ParentNullable @0| CreateOnlyParent @0| CreateOnlyParentNullable @0");
//        eTM.Enqueue("Remove ChildrenList_Child @0 from @0 ChildrenList_Parent.ChildrenList_Children");
//        eTM.Enqueue("Add ChildrenList_Child @0 to @1 ChildrenList_Parent.ChildrenList_Children");
//        eTM.Enqueue("Remove ChildrenList_Child @0 from @0 ChildrenList_ParentNullable.ChildrenList_Children");
//        eTM.Enqueue("Remove ChildrenList_Child @0 from @0 ChildrenList_CreateOnlyParent.ChildrenList_Children");
//        eTM.Enqueue("Add ChildrenList_Child @0 to @1 ChildrenList_CreateOnlyParent.ChildrenList_Children");
//        eTM.Enqueue("Remove ChildrenList_Child @0 from @0 ChildrenList_CreateOnlyParentNullable.ChildrenList_Children");
//        eTM.Enqueue("Updated ChildrenList_Child: @0| C0| Parent @1| ParentNullable | CreateOnlyParent @1| CreateOnlyParentNullable ");
//        trace("c0.Update(c0.Text, p1, null, pc1, null);");
//        c0.Update(c0.Text, p1, null, pc1, null);
//        eTM.Enqueue("Commit transaction");
//        DC.Data.CommitTransaction();

//        traceHeader("--- Construct and store parents 1 nullable ---");
//        eTM.Enqueue("new ChildrenList_ParentNullable: #| PN1");
//        eTM.Enqueue("Stored ChildrenList_ParentNullable # @1");
//        trace("var pn1 = new ChildrenList_ParentNullable(\"PN1\");");
//        var pn1 = new ChildrenList_ParentNullable("PN1");
//        eTM.Enqueue("new ChildrenList_CreateOnlyParentNullable: #| PCN1");
//        eTM.Enqueue("Stored ChildrenList_CreateOnlyParentNullable # @1");
//        trace("var pcn1 = new ChildrenList_CreateOnlyParentNullable(\"PCN1\");");
//        var pcn1 = new ChildrenList_CreateOnlyParentNullable("PCN1");

//        traceHeader("--- Update child 0 ---");
//        eTM.Enqueue("Start transaction");
//        DC.Data.StartTransaction();
//        eTM.Enqueue("Updating ChildrenList_Child: @0| C0| Parent @1| ParentNullable | CreateOnlyParent @1| CreateOnlyParentNullable ");
//        eTM.Enqueue("Add ChildrenList_Child @0 to @1 ChildrenList_ParentNullable.ChildrenList_Children");
//        eTM.Enqueue("Add ChildrenList_Child @0 to @1 ChildrenList_CreateOnlyParentNullable.ChildrenList_Children");
//        eTM.Enqueue("Updated ChildrenList_Child: @0| C0| Parent @1| ParentNullable @1| CreateOnlyParent @1| CreateOnlyParentNullable @1");
//        trace("c0.Update(c0.Text, p1, pn1, pc1, pcn1);");
//        c0.Update(c0.Text, p1, pn1, pc1, pcn1);
//        eTM.Enqueue("Rolling back transaction");
//        eTM.Enqueue("Rolling back ChildrenList_Child.Update(): @0| C0| Parent @1| ParentNullable @1| CreateOnlyParent @1| CreateOnlyParentNullable @1");
//        eTM.Enqueue("Remove ChildrenList_Child @0 from @1 ChildrenList_ParentNullable.ChildrenList_Children");
//        eTM.Enqueue("Remove ChildrenList_Child @0 from @1 ChildrenList_CreateOnlyParentNullable.ChildrenList_Children");
//        eTM.Enqueue("Rolled back ChildrenList_Child.Update(): @0| C0| Parent @1| ParentNullable | CreateOnlyParent @1| CreateOnlyParentNullable ");
//        eTM.Enqueue("Rolled back transaction");
//        DC.Data.RollbackTransaction();
//        eTM.Enqueue("Start transaction");
//        DC.Data.StartTransaction();
//        eTM.Enqueue("Updating ChildrenList_Child: @0| C0| Parent @1| ParentNullable | CreateOnlyParent @1| CreateOnlyParentNullable ");
//        eTM.Enqueue("Add ChildrenList_Child @0 to @1 ChildrenList_ParentNullable.ChildrenList_Children");
//        eTM.Enqueue("Add ChildrenList_Child @0 to @1 ChildrenList_CreateOnlyParentNullable.ChildrenList_Children");
//        eTM.Enqueue("Updated ChildrenList_Child: @0| C0| Parent @1| ParentNullable @1| CreateOnlyParent @1| CreateOnlyParentNullable @1");
//        trace("c0.Update(c0.Text, p1, pn1, pc1, pcn1);");
//        c0.Update(c0.Text, p1, pn1, pc1, pcn1);
//        eTM.Enqueue("Commit transaction");
//        DC.Data.CommitTransaction();

//        traceHeader("--- Store parents 3 ---");
//        eTM.Enqueue("Stored ChildrenList_Parent # @2");
//        trace("p3.Store();");
//        p3.Store();
//        eTM.Enqueue("Stored ChildrenList_ParentNullable # @2");
//        trace("pn3.Store();");
//        pn3.Store();
//        eTM.Enqueue("Stored ChildrenList_CreateOnlyParent # @2");
//        trace("pc3.Store();");
//        pc3.Store();
//        eTM.Enqueue("Stored ChildrenList_CreateOnlyParentNullable # @2");
//        trace("pcn3.Store();");
//        pcn3.Store();

//        traceHeader("--- Store child 1 ---");
//        eTM.Enqueue("Start transaction");
//        DC.Data.StartTransaction();
//        eTM.Enqueue("Stored ChildrenList_Child # @1");
//        trace("c1.Store();");
//        c1.Store();
//        eTM.Enqueue("Rolling back transaction");
//        eTM.Enqueue("Rollback ChildrenList_Child.Store(): @1| C1| Parent @2| ParentNullable @2| CreateOnlyParent @2| CreateOnlyParentNullable @2");
//        eTM.Enqueue("Release ChildrenList_Child key @1 #10896719");
//        eTM.Enqueue("Rolled back transaction");
//        DC.Data.RollbackTransaction();
//        eTM.Enqueue("Start transaction");
//        DC.Data.StartTransaction();
//        eTM.Enqueue("Stored ChildrenList_Child # @1");
//        trace("c1.Store();");
//        c1.Store();
//        eTM.Enqueue("Commit transaction");
//        DC.Data.CommitTransaction();

//        traceHeader("--- Release child 1 ---");
//        eTM.Enqueue("Start transaction");
//        eTM.Enqueue("Released ChildrenList_Child @-1 #");
//        eTM.Enqueue("Rolling back transaction");
//        eTM.Enqueue("Store ChildrenList_Child key @1 #");
//        eTM.Enqueue("Rollback ChildrenList_Child.Release(): @1| C1| Parent @2| ParentNullable @2| CreateOnlyParent @2| CreateOnlyParentNullable @2");
//        eTM.Enqueue("Rolled back transaction");
//        eTM.Enqueue("Start transaction");
//        eTM.Enqueue("Released ChildrenList_Child @-1 #");
//        eTM.Enqueue("Commit transaction");
//        trace("c1.Release();");
//        DC.Data.StartTransaction();
//        c1.Release();
//        DC.Data.RollbackTransaction();
//        DC.Data.StartTransaction();
//        c1.Release();
//        DC.Data.CommitTransaction();
//      }
//    }


//    private void traceHeader(string line) {
//      traceStrinBuilder.AppendLine();
//      traceStrinBuilder.AppendLine(line);
//    }


//    private void trace(string line) {
//      traceStrinBuilder.AppendLine(line);
//    }


//    readonly StringBuilder traceStrinBuilder = new StringBuilder();


//    #pragma warning disable IDE0051 // Remove unused private members
//    private void dcTraceCollect(string message) {
//    #pragma warning restore IDE0051 // Remove unused private members
//      traceStrinBuilder.AppendLine(message);
//    }

//    readonly Queue<string> eTM/*expectedTraceMessages*/ = new Queue<string>();


//    private void dcTraceAssert(string message) {
//      if (eTM.Count==0) {
//        Debugger.Break();
//      }
//      var expectedMessage = eTM.Dequeue();
//      var expectedIndex = 0;
//      var isSkipDigits = false;

//      for (int actualIndex = 0; actualIndex < message.Length; actualIndex++) {
//        var actualChar = message[actualIndex];

//        if (actualChar=='#') {
//          isSkipDigits = true;
//        } else if (isSkipDigits) {
//          if (actualChar>='0' && actualChar<='9') {
//            continue;
//          }
//          isSkipDigits = false;
//        }
//        if (expectedMessage[expectedIndex++]!=actualChar) {
//          Assert.Fail($"expected: '{expectedMessage}'{Environment.NewLine}actual: '{message}'");
//        }
//      }
//    }


//    private void reportException(Exception obj) {
//      System.Diagnostics.Debug.WriteLine(obj);
//      System.Diagnostics.Debugger.Break();
//      Assert.Fail();
//    }

//  }
//}
