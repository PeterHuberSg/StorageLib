/**************************************************************************************

StorageLib.StorageClassGenerator
================================

Reads classes and enumerations in a Data Model and compiles it into Storage classes
in a Data Context.

Written in 2020 by Jürgpeter Huber 
Contact: https://github.com/PeterHuberSg/StorageLib

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.IO;

/*
https://github.com/dotnet/roslyn/wiki/Getting-Started-C%23-Syntax-Analysis
https://github.com/dotnet/roslyn/wiki/Roslyn%20Overview
https://docs.microsoft.com/en-us/archive/msdn-magazine/2014/special-issue/csharp-and-visual-basic-use-roslyn-to-write-a-live-code-analyzer-for-your-api

*/

namespace StorageLib {


  /// <summary>
  /// Should tracing code be generated ?
  /// </summary>
  public enum TracingEnum {
    noTracing,
    debugOnlyTracing,
    alwaysTracing
  }


  public class StorageClassGenerator {


    /// <summary>
    /// Reads all .cs files in sourceDirectoryString. The .cs file can contain one or several classes. If any class contains
    /// a method, it gets skipped, otherwise StorageClassGenerator generates for each class a new .base.cs file in targetDirectoryString, 
    /// adding all code needed for object oriented data storage. A corresponding.cs file gets created, if it doesn't exist yet, where
    /// more code can get added manually. The generator will not overwrite these changes.<para/>
    /// 
    /// The following arguments can be used in the class attribute StorageClass to configure its behavior:<para/>
    /// 
    /// pluralName: used if class name has an irregular plural. Example: Activity => Activities<para/>
    /// areInstancesUpdatable: Can the properties of the class change ?<para/>
    /// areInstancesReleasable: Can class instance be removed from StorageDirectory ?<para/>
    /// 
    /// </summary>
    /// <param name="sourceDirectoryString">Source directory from where the .cs files get read.</param>
    /// <param name="targetDirectoryString">Target directory where the new .cs files get written.</param>
    /// <param name="context">Name of Context class, which gives static access to all data stored.</param>
    /// <param name="isTracing">defines if tracing instructions should get added to the code.</param> 
    /// <param name="isFullyCommented">If true (default), the created .cs files (not .base.cs files) have all code lines 
    /// commented out.</param>
    public StorageClassGenerator(
      string sourceDirectoryString, 
      string targetDirectoryString, 
      string context,
      TracingEnum isTracing = TracingEnum.noTracing,
      bool isFullyCommented = true) 
    {
      try {
        Console.WriteLine("Storage Class Generator");
        Console.WriteLine("**********************");
        Console.WriteLine();

        DirectoryInfo sourceDirectory = findDirectory(sourceDirectoryString, "source directory");
        DirectoryInfo targetDirectory= findDirectory(targetDirectoryString, "target directory");
        Console.WriteLine();

        var compiler = new Compiler(isTracing, isFullyCommented);
        var hasModelFile = false;
        foreach (var file in sourceDirectory.GetFiles("*.cs")) {
          if (isModelFile(file, out NamespaceDeclarationSyntax? namespaceDeclaration)) {
            hasModelFile = true;
            Console.WriteLine($"Parse {file.Name}");
            compiler.Parse(namespaceDeclaration!, file.Name);
          }
        }

        if (hasModelFile) {
          Console.WriteLine("analyze dependencies");
          compiler.AnalyzeDependencies();
          Console.WriteLine();

          Console.WriteLine("write class files");
          compiler.WriteClassFiles(targetDirectory, context);
          Console.WriteLine("");

          if (compiler.Enums.Count>0) {
            Console.WriteLine("write Enums.base.cs");
            compiler.WriteEnumsFile(targetDirectory);
            Console.WriteLine("");
          }

          Console.WriteLine($"write {context}.cs");
          compiler.WriteContextFile(targetDirectory, context);
          Console.WriteLine("");
       
        } else {
          var oldForegroundColor = Console.ForegroundColor;
          Console.ForegroundColor = ConsoleColor.Yellow;
          Console.WriteLine();
          Console.WriteLine($"Error: There is no .cs file in {sourceDirectory.FullName} containing a valid data model.");
          Console.WriteLine();
          Console.ForegroundColor = oldForegroundColor;

        }
      } catch (GeneratorException gex) {
        var oldForegroundColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine();
        Console.WriteLine("Error: ");
        Console.WriteLine(gex.Message);
        Console.WriteLine();
        Console.ForegroundColor = oldForegroundColor;

      } catch (Exception ex) {
        var oldForegroundColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine();
        Console.WriteLine("Exception");
        Console.WriteLine(ex.ToString());
        Console.WriteLine();
        Console.ForegroundColor = oldForegroundColor;
      }
    }


    private static DirectoryInfo findDirectory(string directoryString, string directoryName) {
      DirectoryInfo directory;
      try {
        directory = new DirectoryInfo(directoryString);
      } catch (Exception) {
        throw new GeneratorException($"Cannot find {directoryName} {directoryString}.");
      }
      if (!directory.Exists) {
        throw new GeneratorException($"Cannot find {directoryName} {directoryString}.");
      }
      Console.WriteLine($"{directoryName}: {directory.FullName}");
      return directory;
    }


    private static bool isModelFile(FileInfo file, out NamespaceDeclarationSyntax? namespaceDeclaration) {
      var tree = CSharpSyntaxTree.ParseText(file.OpenText().ReadToEnd());
      if (tree.GetRoot() is not CompilationUnitSyntax root) {
        Console.WriteLine($"{file.Name} skipped, it cannot be converted to a compilation unit.");
        namespaceDeclaration = null;
        return false;
      }
      if (root.Members.Count!=1) {
        Console.WriteLine($"{file.Name} skipped, it has 0 or more than 1 namespace declaration in the compilation unit.");
        namespaceDeclaration = null;
        return false;
      }
      namespaceDeclaration = root.Members[0] as NamespaceDeclarationSyntax;
      if (namespaceDeclaration is null) {
        Console.WriteLine($"{file.Name} skipped, compilation unit does not contain just 1 namespace declaration.");
        return false;

      }
      foreach (var member in namespaceDeclaration.Members) {
        if (member is not ClassDeclarationSyntax classDeclaration) {
          if (member is not EnumDeclarationSyntax) {
            Console.WriteLine($"{file.Name} skipped, namespace does not contain just class and enum declarations.");
            return false;
          }
        } else {
          foreach (var classMember in classDeclaration.Members) {
            if (classMember is MethodDeclarationSyntax methodDeclaration) {
              Console.WriteLine($"{file.Name} skipped, it has a method {methodDeclaration.Identifier.Text}.");
              return false;
            }
          }
        }
      }
      return true;
    }
  }
}
