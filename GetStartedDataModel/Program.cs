﻿/**************************************************************************************

Program
=======

The Data Model is ideally defined in a 'Console App DotNet'. The Model itself can be written
into any .cs file in the same project. Program.cs, which uses StorageClassGenerator to read 
the Model and create the code for the Data Context, needs only few lines of code with some 
configuration info.

Written in 2021 by Jürgpeter Huber 
Contact: https://github.com/PeterHuberSg/StorageLib

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/
using StorageLib;
using System;
using System.IO;

namespace StorageModel {

  /// <summary>
  /// Reads all .cs files which are in the same directory like Program. These files 
  /// should contain only data model code. Program creates accordingly .cs files for 
  /// data context and data classes in the folder at targetDirectoryPath.
  /// </summary>
  class Program {


    public static void Main(string[] _) {
      //here are the .cs file(s) stored with the data model
      var sourceDirectory = new DirectoryInfo(Environment.CurrentDirectory).Parent!.Parent!.Parent!;
      //path to VS solution of your project
      var solutionDirectory = sourceDirectory.Parent!;
      //path of the VS project where the created code should be generated
      var targetDirectoryPath = solutionDirectory.FullName + @"\GetStartedDataContext"; //<== Enter here the name of your data contect project

#pragma warning disable CA1806 // Do not ignore method results
      new StorageClassGenerator(
        sourceDirectoryString: sourceDirectory.FullName,
        targetDirectoryString: targetDirectoryPath,
        context: "DC"); //<== class name of data context, which gives static access to all data stored.
    }
  }
}
