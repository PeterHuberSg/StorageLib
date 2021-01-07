/**************************************************************************************

GetStarted Console Program
==========================

This console program shows a new user how the classes in a data context can be used in
his applicatio.

Written in 2021 by Jürgpeter Huber 
Contact: PeterCode at Peterbox dot com

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/
using System;
using System.IO;
using System.Text;
using StorageLib;
using YourNameSpace; //The name of this namespace is defined in your Data Model


namespace GetStartedConsole {
  class Program {
    static void Main(string[] args) {
      Console.ForegroundColor = ConsoleColor.White;
      Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
      Console.WriteLine("StorageLib Get Started Application");
      Console.WriteLine("==================================");
      Console.WriteLine();
      var directoryInfo = new DirectoryInfo("GetSetartedData");
      if (directoryInfo.Exists) {
        Console.WriteLine("Found existing directory for data: " + directoryInfo.FullName);
      } else { 
        directoryInfo.Create();
        //it is enough just to start with an empty directory. The data context will create
        //the necessary files if they are missing.
        Console.WriteLine("Created new directory for data: " + directoryInfo.FullName);
      }

      try {
        var dc = new DC(new CsvConfig(directoryInfo.FullName, reportException: reportException));
        Parent parent0;
        Parent parent1;
        Child child0;
        if (dc.Parents.Count==0) {
          //new data context, we come here only the very first time Program runs or when the .csv gets deleted
          parent0 = new Parent("Parent0"); //per default, new stores instance in Data Context
          parent1 = new Parent("Parent1", isStoring: false); //example where new instance is not stored yet
          parent1.Store(); //this can be done much later, but before application shuts down
          child0 = new Child("Child0", parent0);//this adds child0 automatically to parent0.Children
          consoleWriteLine("Newly created data", parent0, parent1, child0);

        } else {
          //access already existing data 
          parent0 = DC.Data.Parents[0]; //access without local DC reference, just through static variable DC.Data
          parent1 = dc.Parents[1];
          child0 = dc.Children[0];
          consoleWriteLine("Exisiting data", parent0, parent1, child0);
        }

        //update without transaction
        child0.Update(child0.Text + " updated", parent1);
        consoleWriteLine("After simple update", parent0, parent1, child0);

        //a normal transaction
        dc.StartTransaction();
        try {
          child0.Update("Child0", parent0);
          //could have many more statements
          dc.CommitTransaction();
        } catch (Exception exception) {
          dc.RollbackTransaction();
          reportException(exception);
        }
        consoleWriteLine("After update with transaction", parent0, parent1, child0);

        //showing that transaction rollback really works
        dc.StartTransaction();
        child0.Update(child0.Text + " updated before rollback", parent1);
        consoleWriteLine("After update, before rollback", parent0, parent1, child0);
        dc.RollbackTransaction(); //normally, a dc.CommitTransaction() would be here
        consoleWriteLine("After transaction rollback", parent0, parent1, child0);

        child0.Release(); // removing child0 from dc.Children, but it is still in parent0.Children
        consoleWriteLine("After child0.Release", parent0, parent1, child0);

        child0.Store();
        consoleWriteLine("After child0.Store", parent0, parent1, child0);
      } catch (Exception exception) {
        Console.WriteLine("Exception occured: " + exception);
      } finally {
        //It is important that your application calls DC.DisposeData() when it shuts down. This
        //flushes any not yet written data to that CSV files.
        DC.DisposeData();
      }
    }


    /* How a transaction normally looks like (althoug transactions are facultative)
    
    */

    private static void reportException(Exception exception) {
      Console.WriteLine("Exception occured: " + exception);
    }


    static string previousParent0 = "";
    static string previousParent1 = "";
    static string previousChild0 = "";


    private static void consoleWriteLine(string comment, Parent parent0, Parent parent1, Child child0) {
      Console.WriteLine();
      Console.WriteLine(comment);
      consoleWriteLine("parent0: " , parent0.ToString(), ref previousParent0);
      consoleWriteLine("parent1: ", parent1.ToString(), ref previousParent1);
      consoleWriteLine("child0: ", child0.ToString(), ref previousChild0);
    }


    private static void consoleWriteLine(string name, string newText, ref string oldText) {
      Console.Write(name);
      if (oldText.Length==0) {
        Console.WriteLine(newText);
      } else {
        var oldIndex = 0;
        var newIndex = 0;
        do {
          var c = newText[newIndex];
          if (c==oldText[oldIndex]) {
            Console.Write(c);
            oldIndex++;
          } else {
            c = oldText[oldIndex];
            while (c!=',' && c!=';') {
              c = oldText[++oldIndex];
            }
            var startIndex = newIndex;
            c = newText[newIndex];
            while (c!=',' && c!=';') {
              c = newText[++newIndex];
            }
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(newText[startIndex..newIndex]);
            Console.ForegroundColor = oldColor;
            newIndex--;
          }

        } while (++newIndex<newText.Length);
        Console.WriteLine();
      }
      oldText = newText;
    }
  }
}
