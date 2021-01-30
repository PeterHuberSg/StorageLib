/**************************************************************************************

StorageLib.Compiler
===================

Compiles a Model into a DataContext

Written in 2020 by Jürgpeter Huber 
Contact: PeterCode at Peterbox dot com

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;


// https://csharp-source.net/last-projects: add link to Storage on that website
// http://prevayler.org/ java oject persitence library

namespace StorageLib {


  /// <summary>
  /// Compiles a Model into a DataContext
  /// </summary>
  public class Compiler {

    //Step 1 Parse(): Find all classes and their properties in the Model
    //Step 2 AnalyzeDependencies()(): Find which class (=parents) has collections of other classes (=children). Parents
    //       must be created (read from files) before their children. Also ensure that every parent child relationship
    //       is correctly defined (no orphans).
    //Step 3 Write code:
    //       Write class.cs files is missing and class.base.cs files
    //       Write context class
    //       write file with enumerations

    #region Properties
    //      ----------

    public readonly TracingEnum IsTracing; //Should tracing code be generated ?
    public readonly bool IsFullyCommented; //if the class.cs file does not exist, one gets created. Should all
                                           //code in it be commented out. Default: true
    public readonly bool IsAddTestCode; //adds test code for unit testing of StorageLib
    #endregion


    #region Constructor
    //      -----------

    readonly Dictionary<string, ClassInfo> classes;
    readonly List<ClassInfo> parentChildTree;
    readonly Dictionary<string, EnumInfo> enums;
    public IReadOnlyDictionary<string, EnumInfo> Enums { get { return enums; } }
    bool isUsingDictionary = false; // is statement 'using System.Collections.Generic' needed in data context source file ? 


    public Compiler(TracingEnum isTracing, bool isFullyCommented = true) {
      IsTracing = isTracing;
      IsFullyCommented = isFullyCommented;
      classes = new Dictionary<string, ClassInfo>();
      parentChildTree = new List<ClassInfo>();
      enums = new Dictionary<string, EnumInfo>();
    }
    #endregion


    #region Parse
    //      -----
    string? nameSpaceString;
    const string onlyAcceptableConsts = "should only contain properties and configuration constants for " +
            "MaxLineLenght, AreInstancesUpdatable and AreInstancesReleasable, but not";


    public void Parse(NamespaceDeclarationSyntax namespaceDeclaration, string fileName) {
      var newNameSpaceString = namespaceDeclaration.Name.GetText().ToString();
      if (nameSpaceString is null) {
        nameSpaceString = newNameSpaceString;
      } else if (nameSpaceString!=newNameSpaceString) {
        throw new GeneratorException($"{fileName} defines a different namespace 'newNameSpaceString' than the already defined one 'nameSpaceString'.");
      }
      foreach (var namespaceMember in namespaceDeclaration.Members) {
        if (namespaceMember is EnumDeclarationSyntax enumDeclarationSyntax) {
          parseEnum(enumDeclarationSyntax);
          continue;
        }

        if (!(namespaceMember is ClassDeclarationSyntax classDeclaration)) {
          throw new GeneratorException($"{fileName} contains not only class and enum declarations in namespace '{nameSpaceString}'.");
        }
        var className = classDeclaration.Identifier.Text;
        string? classComment = getXmlComment(classDeclaration.GetLeadingTrivia());
        string? pluralName = className + 's';
        bool areInstancesUpdatable = true;
        bool areInstancesReleasable = true;
        bool isConstructorPrivate = false;
        bool isGenerateReaderWriter = false;
        if (classDeclaration.AttributeLists.Count==0) {
          //use the default values
        } else if (classDeclaration.AttributeLists.Count>1) {
          throw new GeneratorException($"Class {className} should contain at most 1 attribute, i.e. StorageClass attribute, but has '{classDeclaration.AttributeLists.Count}' attributes: '{classDeclaration.AttributeLists}'");

        } else {
          var attributes = classDeclaration.AttributeLists[0].Attributes;
          if (attributes.Count!=1) throw new GeneratorException($"Class {className} should contain at most 1 attribute, i.e. StorageClass attribute, but has '{classDeclaration.AttributeLists.Count}' attributes: '{attributes}'");

          var attribute = attributes[0];
          if (!(attribute.Name is IdentifierNameSyntax attributeName) || attributeName.Identifier.Text!="StorageClass") {
            throw new GeneratorException($"Class {className} should contain only a StorageClass attribute, but has: '{classDeclaration.AttributeLists}'");
          }
          if (attribute.ArgumentList is not null) {
            foreach (var argument in attribute.ArgumentList.Arguments) {
              if (argument.NameColon is null) throw new GeneratorException($"Class {className} Attribute{attribute}: the argument name is missing, like 'areInstancesUpdatable: true'.");

              var name = argument.NameColon.Name.Identifier.Text;
              var isException = false;
              try {
                var value = ((LiteralExpressionSyntax)argument.Expression).Token.Text;
                switch (name) {
                case "pluralName": pluralName = value[1..^1]; break;
                case "areInstancesUpdatable": areInstancesUpdatable = value=="true"; break;
                case "areInstancesReleasable": areInstancesReleasable = value=="true"; break;
                case "isConstructorPrivate": isConstructorPrivate = value=="true"; break;
                case "isGenerateReaderWriter": isGenerateReaderWriter = value=="true"; break;
                default: isException = true; break;
                }
              } catch (Exception ex) {
                throw new GeneratorException($"Class {className} Attribute {name}: Exception '{ex.Message}' was thrown when processing '{attribute}'.");
              }
              if (isException) {
                throw new GeneratorException($"Class {className}: Illegal attribute name '{name}' in '{attribute}'. It " +
                  "can only be: pluralName, areInstancesUpdatable, areInstancesReleasable, isConstructorPrivate or " +
                  "isGenerateReaderWriter.");
              }
            }

          }
        }
        var classInfo = new ClassInfo(className, classComment, pluralName, areInstancesUpdatable, 
          areInstancesReleasable, isConstructorPrivate, isGenerateReaderWriter);
        classes.Add(className, classInfo);
        var isPropertyWithDefaultValueFound = false;
        foreach (var classMember in classDeclaration.Members) {
          //each field has only 1 property
          if (!(classMember is FieldDeclarationSyntax field)) {
            throw new GeneratorException($"Class {className} should contain only properties and these properties should " + 
              $"not contain get and set, but has '{classMember}'.");
          }

          string? propertyComment = getComment(field.GetLeadingTrivia());

          bool isReadOnly = false;
          foreach (var modifierToken in field.Modifiers) {
            if (modifierToken.Text=="const") {
              throw new GeneratorException($"Class {className} should contain only properties, but has const '{classMember}'.");
            } else if (modifierToken.Text=="readonly") {
              isReadOnly = true;
            }
          }
          if (!classInfo.AreInstancesUpdatable) {
            isReadOnly = true; //if a class cannot be updated, all its properties are readonly
          }

          if (!(field.Declaration is VariableDeclarationSyntax variableDeclaration)) {
            throw new GeneratorException($"Class {className} {onlyAcceptableConsts} '{field.Declaration}'.");
          }
          var propertyType = variableDeclaration.Type.ToString();
          foreach (var property in variableDeclaration.Variables) {
            string? defaultValue = null;
            bool? isLookupOnly = null;
            bool isParentOneChild = false;
            bool needsDictionary = false;
            string? toLower = null;
            string? childKeyPropertyName = null;
            if (field.AttributeLists.Count==0) {
              if (isPropertyWithDefaultValueFound && !propertyType.StartsWith("List<")) {
                throw new GeneratorException($"Property {className}.{property.Identifier.Text} should have a " +
                  "StorageProperty(defaultValue: \"xxx\") attribute, because the previous one had one too. Once a " +
                  "property has a default value, all following properties need to have one too.");
              }
              //use the default values
            } else if (field.AttributeLists.Count>1) {
              throw new GeneratorException($"Property {className}.{property.Identifier.Text} should contain at most 1 attribute, i.e. StorageProperty attribute, but has '{field.AttributeLists.Count}' attributes: '{field.AttributeLists}'");

            } else {
              var attributes = field.AttributeLists[0].Attributes;
              if (attributes.Count!=1) throw new GeneratorException($"Property {className}.{property.Identifier.Text} should contain at most 1 attribute, i.e. StorageProperty attribute, but has '{field.AttributeLists.Count}' attributes: '{attributes}'");

              var attribute = attributes[0];
              if (!(attribute.Name is IdentifierNameSyntax attributeName) || attributeName.Identifier.Text!="StorageProperty") {
                throw new GeneratorException($"Property {className}.{property.Identifier.Text} should contain only a StorageProperty attribute, but has: '{classDeclaration.AttributeLists}'");
              }
              foreach (var argument in attribute.ArgumentList!.Arguments) {
                if (argument.NameColon is null) throw new GeneratorException($"Property {className}.{property.Identifier.Text} Attribute{attribute}: the argument name is missing, like 'defaultValue: null'.");

                var name = argument.NameColon.Name.Identifier.Text;
                var isUnknown = false;
                try {
                  var value = ((LiteralExpressionSyntax)argument.Expression).Token.Text;
                  switch (name) {
                  case "defaultValue": defaultValue = value[1..^1]; isPropertyWithDefaultValueFound = true; break;
                  case "isLookupOnly": isLookupOnly = bool.Parse(value); break;
                  case "isParentOneChild": isParentOneChild = bool.Parse(value); break;
                  case "toLower": toLower = value[1..^1]; break;
                  case "needsDictionary": 
                    needsDictionary = bool.Parse(value);
                    if (needsDictionary) isUsingDictionary = true;
                    break;
                  case "childKeyPropertyName": childKeyPropertyName = value[1..^1]; break;
                  default: isUnknown = true; break;
                  }
                } catch (Exception ex) {
                  throw new GeneratorException($"Property {className}.{property.Identifier.Text}: Exception '{ex.Message}' was thrown when processing attribute '{name}' in '{attribute}'.");
                }
                if (isUnknown) {
                  throw new GeneratorException($"Property {className}.{property.Identifier.Text}: Illegal attribute name '{name}' in '{attribute}'. It " +
                    "can only be: defaultValue, isLookupOnly, isParentOneChild, lowerFrom or " +
                    "needsDictionary.");
                }
              }
            }
            if ((isLookupOnly??false) && isParentOneChild) {
              throw new GeneratorException($"Property {className}.{property.Identifier.Text} cannot have " + 
                "isLookupOnly: true and isParentOneChild: true in its StorageProperty attribute.");
            }
            classInfo.AddMember(classMember.ToString(), property.Identifier.Text, propertyType, propertyComment, defaultValue, 
              isLookupOnly, isParentOneChild, toLower, needsDictionary, childKeyPropertyName, isReadOnly);
            //}
          }
        }
      }
    }


    private void parseEnum(EnumDeclarationSyntax enumDeclaration) {
      var enumLeadingComment = getXmlComment(enumDeclaration.GetLeadingTrivia());
      //var enumDeclarationWithLeadingComment = enumDeclaration.ToFullString();
      //var enumDeclarationOnly = removeRegionAndLeadingSimpleComments(enumDeclarationWithLeadingComment);
      var indentation = enumDeclaration.GetLastToken().LeadingTrivia.ToString();
      enums.Add(enumDeclaration.Identifier.Text, 
        new EnumInfo(enumDeclaration.Identifier.Text, enumLeadingComment + indentation + enumDeclaration.ToString()));
    }


    private string addLeadingSpaces(string declaration, int pos) {
      pos--;
      while (pos>0) {
        var c = declaration[pos];
        if (c!=' ') {
          break;
        }
        pos--;
      }
      pos++;
      return declaration[pos..];
    }


    private string? getXmlComment(SyntaxTriviaList syntaxTriviaList) {
      string? comment = null;
      var leadingTrivia = syntaxTriviaList.ToString();
      var lines = leadingTrivia.Split(Environment.NewLine);
      foreach (var line in lines) {
        if (line.Contains("///")) {
          comment += line + Environment.NewLine;
        }
      }
      return comment;
    }


    private string? getComment(SyntaxTriviaList syntaxTriviaList) {
      string? comment = null;
      var leadingTrivia = syntaxTriviaList.ToString();
      if (leadingTrivia.Contains("///")) {
        var triviaLines = leadingTrivia.Split(Environment.NewLine);
        foreach (var line in triviaLines) {
          if (!string.IsNullOrWhiteSpace(line)) {
            comment += line.TrimStart() + Environment.NewLine;
          }
        }
      }
      return comment;
    }
    #endregion


    #region Analyze Dependencies
    //      --------------------

    public void AnalyzeDependencies() {
      var topClasses = classes.Values.ToDictionary(c=>c.ClassName);
      foreach (var ci in classes.Values) {
        foreach (var mi in ci.Members.Values) {
          var isFound = false;
          var parentMembersCount = 0;
          switch (mi.MemberType) {

          case MemberTypeEnum.ToLower:
            //                --------
            foreach (var member in ci.Members.Values) {
              if (member.MemberName==mi.PropertyForToLower) {
                if (member.NeedsDictionary && mi.NeedsDictionary) {
                  throw new GeneratorException($"{ci.ClassName}.{mi.MemberName}: is a lower case copy of {member.MemberName}. " +
                    "Both haveOnly the attribute parameter NeedsDictionary, but only one of them at a time can have a " +
                    "dictionary. The software could be extended to allow both having a Dictionary." + Environment.NewLine + 
                    mi.MemberText);
                }
                isFound = true;
                member.ToLowerTarget = mi;
                mi.SetIsNullable(member.IsNullable);
                mi.IsReadOnly = member.IsReadOnly;
                break;
              }
            }
            if (isFound) {
              break;
            }
            throw new GeneratorException($"{ci.ClassName}.{mi.MemberName}: is supposed to be a lower case copy of a " +
              $"{mi.PropertyForToLower} property in the same class, which cannot be found:" + Environment.NewLine + mi.MemberText);

          case MemberTypeEnum.LinkToParent:
            //                -------------
            if (classes.TryGetValue(mi.ParentTypeString!, out mi.ParentClassInfo)) {
              ci.ParentsAll.Add(mi.ParentClassInfo);
              topClasses.Remove(ci.ClassName);
              mi.ParentClassInfo.Children.Add(ci);
              if (mi.IsLookupOnly) {
                if (mi.ParentClassInfo.AreInstancesReleasable) {
                  throw new GeneratorException($"{ci.ClassName}.{mi.MemberName}: cannot use the deletable instances of class " +
                    $"{mi.ParentClassInfo.ClassName} as lookup:" + Environment.NewLine + mi.MemberText);
                }
              } else {
                ci.HasParents = true;
                if (!mi.IsReadOnly && !mi.IsLookupOnly) {
                  ci.HasNotReadOnlyAndNotLookupParents = true;
                }
                foreach (var parentMember in mi.ParentClassInfo.Members.Values) {
                  if (parentMember.ChildTypeName==mi.ClassInfo.ClassName) {
                    parentMembersCount++;
                    mi.ParentMemberInfo = parentMember;
                    if (parentMember.MemberType!=MemberTypeEnum.ParentOneChild) {
                      parentMember.ChildCount++;
                    }
                  }
                }
                if (parentMembersCount==1) break;

                if (parentMembersCount<1) {
                  throw new GeneratorException(
                    $"Property {mi.MemberName} from child class {ci.ClassName} links to parent {mi.ParentClassInfo.ClassName}. " + 
                    $"But the parent does not have a property which links to the child. Add a collection (list, dictionary or sortedList) " + 
                    $"to the parent if many children are allowed or a property with the [StorageProperty(isParentOneChild: true)] attribute " + 
                    $"if only 1 child is allowed. Add [StorageProperty(isLookupOnly: true)] to the child property if the parent " +
                    "should not have a relation with the child:" + Environment.NewLine + mi.MemberText);
                } else {
                  throw new GeneratorException(
                    $"Property {mi.MemberName} from child class {ci.ClassName} links to parent {mi.ParentClassInfo.ClassName}. " +
                    $"But the parent has more than 1 property which links to the child:" + Environment.NewLine + mi.MemberText);
                }
                //if (!mi.ClassInfo.AreInstancesReleasable && mi.ParentClassInfo.AreInstancesReleasable) {
                //  //todo: Compiler.AnalyzeDependencies() Add tests if child is at least updatable, parent property not readonly and nullable
                //  throw new GeneratorException($"Child {mi.ClassInfo.ClassName} does not support deletion. Therefore, the " + 
                //    $"parent {mi.ParentClassInfo.ClassName} can neither support deletion, because it can not delete its children:" 
                //    + Environment.NewLine + mi.MemberText);
                //}
              }

            } else if (enums.TryGetValue(mi.ParentTypeString!, out mi.EnumInfo)) {
              mi.MemberType = MemberTypeEnum.Enum;
              mi.ToStringFunc = "";
            } else {
              throw new GeneratorException($"{ci.ClassName}.{mi.MemberName}: cannot find '{mi.ParentTypeString}'. Should this be a data type " +
                "defined by Storage, a user defined enum or a link to a user defined class ?" + Environment.NewLine + 
                mi.MemberText);
            }
            break;

          case MemberTypeEnum.ParentOneChild:
            //                --------------
            if (!classes.TryGetValue(mi.ChildTypeName!, out mi.ChildClassInfo))
              throw new GeneratorException($"{ci} '{mi}': cannot find class {mi.ChildTypeName}:" + Environment.NewLine + 
                mi.MemberText);

            foreach (var childMI in mi.ChildClassInfo.Members.Values) {
              if (childMI.MemberType==MemberTypeEnum.LinkToParent && childMI.ParentTypeString==ci.ClassName) {
                isFound = true;
                mi.ChildMemberInfo = childMI;
                mi.IsChildReadOnly = childMI.IsReadOnly;
                break;
              }
            }
            if (!isFound) {
              //guarantee that there is a property linking to the parent for each child class.
              throw new GeneratorException($"{ci} '{mi}' is a property which links to 0 or 1 child. A corresponding " +
                $"property with type {ci.ClassName} is missing in the class {mi.ChildTypeName}:" + Environment.NewLine + 
                mi.MemberText);
            }
            break;

          case MemberTypeEnum.ParentMultipleChildrenList:
            //                --------------------------
            if (!classes.TryGetValue(mi.ChildTypeName!, out mi.ChildClassInfo))
              throw new GeneratorException($"{ci} '{mi}': can not find class {mi.ChildTypeName}:" + Environment.NewLine + 
                mi.MemberText);

            foreach (var childMI in mi.ChildClassInfo.Members.Values) {
              if (childMI.MemberType==MemberTypeEnum.LinkToParent && childMI.ParentTypeString==ci.ClassName) {
                isFound = true;
                mi.IsChildReadOnly |= childMI.IsReadOnly;
              }
            }
            if (!isFound) {
              //guarantee that there is a property linking to the parent for each child class.
              throw new GeneratorException($"{ci} '{mi}': has a List<{mi.ChildTypeName}>. The corresponding " +
                $"property with type {ci.ClassName} is missing in the class {mi.ChildTypeName}:" + Environment.NewLine + 
                mi.MemberText);
            }
            break;

          case MemberTypeEnum.ParentMultipleChildrenDictionary:
          case MemberTypeEnum.ParentMultipleChildrenSortedList:
            //Dictionary, SortedList
            if (!classes.TryGetValue(mi.ChildTypeName!, out mi.ChildClassInfo))
              throw new GeneratorException($"{ci} '{mi}': cannot find class {mi.ChildTypeName}:" + Environment.NewLine + 
                mi.MemberText);

            //search for member in child class which has a parent linking to mi
            foreach (var childMI in mi.ChildClassInfo.Members.Values) {
              if (childMI.MemberType==MemberTypeEnum.LinkToParent && childMI.ParentTypeString==ci.ClassName) {
                //child property found pointing to parent. 
                mi.IsChildReadOnly = childMI.IsReadOnly;
                MemberInfo? childKeyMIFound = null;
                //Find another child property which is used as key into the Dictionary or SortedList
                foreach (var childKeyMI in mi.ChildClassInfo.Members.Values) {
                  if (mi.ChildKeyTypeString==childKeyMI.CsvTypeString || mi.ChildKeyTypeString==childKeyMI.TypeString) {
                    if (mi.ChildKeyPropertyName is null) {
                      //parent class does not know the name of the child property used as key. Use a property with the proper key type.
                      if (childKeyMIFound is null) {
                        //first property found with the expected key type
                        childKeyMIFound = childKeyMI;
                      } else {
                        //second property found with the expected key type, throw exception
                        throw new GeneratorException($"{ci}.{mi.MemberName} {mi.TypeString}: found " +
                          $"{childMI.ClassInfo.ClassName}.{childMI.MemberName}, but found two properties in {childMI.ClassInfo.ClassName} " +
                          $"with the type {mi.ChildKeyTypeString}: {childKeyMIFound.MemberName}, {childKeyMI.MemberName}. Use " +
                          $"[StorageProperty(childKeyPropertyName: \"Xyz\")] in the parent to indicate which property should be used." +
                          Environment.NewLine + mi.MemberText);
                      }
                    } else {
                      //parent collection class knows the name of the child property to be used as key
                      if (mi.ChildKeyPropertyName==childKeyMI.MemberName) {
                        //property found in child class which matches the expected key name
                        childKeyMIFound = childKeyMI;
                        //the c# compiler enforces that each property name of a class is unique. No need to check if there is another.
                        break;
                      }
                    }
                  } else {
                    //child property has wrong key type. Throw exception if it has the expected key name.
                    if (mi.ChildKeyPropertyName is not null && mi.ChildKeyPropertyName==childKeyMI.MemberName) {
                      throw new GeneratorException($"{ci}.{mi.MemberName} {mi.TypeString}: found " +
                        $"{childKeyMI.ClassInfo.ClassName}.{childKeyMI.MemberName}, but it has wrong type: " +
                        $"{childKeyMI.CsvTypeString}:" + Environment.NewLine + mi.MemberText);
                    }
                  }
                }

                if (childKeyMIFound is null) {
                  if (mi.ChildKeyPropertyName is null) {
                    throw new GeneratorException($"{ci}.{mi.MemberName} {mi.TypeString}: found " +
                      $"{childMI.ClassInfo.ClassName}.{childMI.MemberName}, but could not find another property in {childMI.ClassInfo.ClassName} " +
                      $"with the type {mi.ChildKeyTypeString} needed as key." + Environment.NewLine + mi.MemberText);
                  } else {
                    throw new GeneratorException($"{ci}.{mi.MemberName} {mi.TypeString}: found " +
                      $"{childMI.ClassInfo.ClassName}.{childMI.MemberName}, but could not find another property with the name" +
                      $"{mi.ChildKeyPropertyName} and type {mi.ChildKeyTypeString} needed as key." + Environment.NewLine + mi.MemberText);
                  }

                } else {
                  isFound = true;
                  mi.ChildMemberInfo = childMI;
                  if (mi.MemberType==MemberTypeEnum.ParentMultipleChildrenSortedList) {
                    //memberTypeString = $"SortedList<{keyTypeName}, {itemTypeName}>";
                    if (childMI.ClassInfo.AreInstancesReleasable) {
                      mi.TypeString = $"StorageSortedList<{childKeyMIFound.TypeString}, {childKeyMIFound.ClassInfo.ClassName}>";
                      mi.ReadOnlyTypeString = $"IStorageReadOnlyDictionary<{childKeyMIFound.TypeString}, {childKeyMIFound.ClassInfo.ClassName}>";
                    } else {
                      mi.TypeString = $"SortedList<{childKeyMIFound.TypeString}, {childKeyMIFound.ClassInfo.ClassName}>";
                      mi.ReadOnlyTypeString = $"IReadOnlyDictionary<{childKeyMIFound.TypeString}, {childKeyMIFound.ClassInfo.ClassName}>";
                    }
                  } else {
                    //Dictionary
                    //memberTypeString = $"Dictionary<{keyTypeName}, {itemTypeName}>";
                    if (childMI.ClassInfo.AreInstancesReleasable) {
                      mi.TypeString = $"StorageDictionary<{childKeyMIFound.TypeString}, {childKeyMIFound.ClassInfo.ClassName}>";
                      mi.ReadOnlyTypeString = $"IStorageReadOnlyDictionary<{childKeyMIFound.TypeString}, {childKeyMIFound.ClassInfo.ClassName}>";
                    } else {
                      mi.TypeString = $"Dictionary<{childKeyMIFound.TypeString}, {childKeyMIFound.ClassInfo.ClassName}>";
                      mi.ReadOnlyTypeString = $"IReadOnlyDictionary<{childKeyMIFound.TypeString}, {childKeyMIFound.ClassInfo.ClassName}>";
                    }
                  }
                  if (mi.ChildKeyPropertyName is null) {
                    mi.ChildKeyPropertyName = childKeyMIFound.MemberName;
                  }
                  break;
                }
              }
            }
            if (!isFound) {
              if (mi.MemberType==MemberTypeEnum.ParentMultipleChildrenSortedList) {
                //guarantee that there is a property linking to the parent for each child class.
                throw new GeneratorException($"{ci} '{mi}': has a SortedList<{mi.ChildTypeName}>. The corresponding " +
                  $"property with type {ci.ClassName} is missing in the class {mi.ChildTypeName}:" + Environment.NewLine + 
                  mi.MemberText);
              } else {
                //guarantee that there is a property linking to the parent for each child class.
                throw new GeneratorException($"{ci} '{mi}': has a Dictionary<{mi.ChildTypeName}>. The corresponding " +
                  $"property with type {ci.ClassName} is missing in the class {mi.ChildTypeName}:" + Environment.NewLine + 
                  mi.MemberText);
              }
            }
            break;
          }
        }
      }

      //create parent child tree
      foreach (var classInfo in topClasses.Values) {
        addParentChildTree(classInfo);
      }
      foreach (var classInfo in classes.Values) {
        if (!classInfo.IsAddedToParentChildTree) throw new Exception();
      }
    }


    private void addParentChildTree(ClassInfo classInfo) {
      if (!classInfo.IsAddedToParentChildTree && allParentsAreAddedToParentChildTree(classInfo)) {
        classInfo.IsAddedToParentChildTree = true;
        classInfo.StoreKey = parentChildTree.Count;
        parentChildTree.Add(classInfo);
        foreach (var child in classInfo.Children) {
          addParentChildTree(child);
        }
      }
    }


    private bool allParentsAreAddedToParentChildTree(ClassInfo childClass) {
      foreach (var parentClass in childClass.ParentsAll) {
        if (!parentClass.IsAddedToParentChildTree) return false;
      }
      return true;
    }
    #endregion


    #region Write Code
    //      ----------

    public void WriteClassFiles(DirectoryInfo targetDirectory, string context) {
      foreach (var classInfo in classes.Values) {
        var baseFileNameAndPath = targetDirectory!.FullName + '\\' + classInfo.ClassName + ".base.cs";
        try {
          File.Delete(baseFileNameAndPath);
        } catch {
          throw new GeneratorException($"Cannot delete file {baseFileNameAndPath}.");
        }
        using (var streamWriter = new StreamWriter(baseFileNameAndPath)) {
          Console.Write(classInfo.ClassName + ".base.cs");
          classInfo.EstimatedMaxByteSize = Math.Max(classInfo.EstimatedMaxByteSize, classInfo.HeaderLength);
          Console.WriteLine($"  Estimated bytes to store 1 instance: {classInfo.EstimatedMaxByteSize}, default");
          classInfo.WriteBaseClassFile(streamWriter, nameSpaceString!, context, IsTracing);
        }

        var fileNameAndPath = targetDirectory!.FullName + '\\' + classInfo.ClassName + ".cs";
        if (!new FileInfo(fileNameAndPath).Exists) {
          using var streamWriter = new StreamWriter(fileNameAndPath);
          Console.WriteLine(classInfo.ClassName + ".cs");
          classInfo.WriteClassFile(streamWriter, nameSpaceString!, IsFullyCommented);
        }
      }
    }


    public void WriteContextFile(DirectoryInfo targetDirectory, string context) {
      var fileNameAndPath = targetDirectory!.FullName + '\\' + context + ".base.cs";
      try {
        File.Delete(fileNameAndPath);
      } catch {
        throw new GeneratorException($"Cannot delete file {fileNameAndPath}.");
      }
      using var sw = new StreamWriter(fileNameAndPath);
      sw.WriteLine("//------------------------------------------------------------------------------");
      sw.WriteLine("// <auto-generated>");
      sw.WriteLine("//     This code was generated by StorageClassGenerator");
      sw.WriteLine("//");
      sw.WriteLine("//     Do not change code in this file, it will get lost when the file gets ");
      sw.WriteLine($"//     auto generated again. Write your code into {context}.cs.");
      sw.WriteLine("// </auto-generated>");
      sw.WriteLine("//------------------------------------------------------------------------------");
      sw.WriteLine("#nullable enable");
      sw.WriteLine("using System;");
      if (isUsingDictionary) {
        sw.WriteLine("using System.Collections.Generic;");
      }
      sw.WriteLine("using System.Threading;");
      sw.WriteLine("using StorageLib;");
      sw.WriteLine();
      sw.WriteLine();
      sw.WriteLine($"namespace {nameSpaceString} {{");
      sw.WriteLine();
      sw.WriteLine("  /// <summary>");
      sw.WriteLine($"  /// A part of {context} is static, which gives easy access to all stored data (=context) through {context}.Data. But most functionality is in the");
      sw.WriteLine($"  /// instantiatable part of {context}. Since it is instantiatable, is possible to use different contexts over the lifetime of a program. This ");
      sw.WriteLine($"  /// is helpful for unit testing. Use {context}.Init() to create a new context and dispose it with DisposeData() before creating a new one.");
      sw.WriteLine("  /// </summary>");
      sw.WriteLine($"  public partial class {context}: DataContextBase {{");
      sw.WriteLine();
      sw.WriteLine("    #region static Part");
      sw.WriteLine("    //      -----------");
      sw.WriteLine();
      sw.WriteLine("    /// <summary>");
      sw.WriteLine("    /// Provides static root access to the data context");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine($"    public static {context} Data {{");
      sw.WriteLine("      get { return data!; }");
      sw.WriteLine("    }");
      sw.WriteLine($"    private static {context}? data; //data is needed for Interlocked.Exchange(ref data, null) in DisposeData()");
      sw.WriteLine();
      sw.WriteLine();
      if (IsTracing>TracingEnum.noTracing) {
        var debugOnly = IsTracing==TracingEnum.debugOnlyTracing ? " if DEBUG is defined" : "";
        sw.WriteLine("    /// <summary>");
        sw.WriteLine($"    /// Trace gets called when an item gets created, stored, updated or removed{debugOnly}");
        sw.WriteLine("    /// </summary>");
        sw.WriteLine("    public static Action<string>? Trace;");
        sw.WriteLine();
        sw.WriteLine();
      }
      sw.WriteLine("    /// <summary>");
      sw.WriteLine("    /// Flushes all data to permanent storage location if permanent data storage is active. Compacts data storage");
      sw.WriteLine("    /// by applying all updates and removing all instances marked as deleted.");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine("    public static void DisposeData() {");
      sw.WriteLine("      var dataLocal = Interlocked.Exchange(ref data, null);");
      sw.WriteLine("      dataLocal?.Dispose();");
      sw.WriteLine("    }");
      sw.WriteLine("    #endregion");
      sw.WriteLine();
      sw.WriteLine();
      sw.WriteLine("    #region Properties");
      sw.WriteLine("    //      ----------");
      sw.WriteLine();
      sw.WriteLine("    /// <summary>");
      sw.WriteLine("    /// Configuration parameters if data gets stored in .csv files");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine("    public CsvConfig? CsvConfig { get; }");
      sw.WriteLine();
      sw.WriteLine("    /// <summary>");
      sw.WriteLine("    /// Is all data initialised");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine("    public bool IsInitialised { get; private set; }");
      foreach (var classInfo in classes.Values.OrderBy(ci => ci.ClassName)) {
        sw.WriteLine();
        sw.WriteLine("    /// <summary>");
        sw.WriteLine($"    /// Directory of all {classInfo.PluralName}");
        sw.WriteLine("    /// </summary>");
        /*
            public IReadonlyDataStore<ChildrenDictionary_Child> ChildrenDictionary_Children => _ChildrenDictionary_Children;
            protected DataStore<ChildrenDictionary_Child> _ChildrenDictionary_Children { get; private set; }
         * */
        sw.WriteLine($"    public IReadonlyDataStore<{classInfo.ClassName}> {classInfo.PluralName} => _{classInfo.PluralName};");
        sw.WriteLine($"    internal DataStore<{classInfo.ClassName}> _{classInfo.PluralName} {{ get; private set; }}");
        foreach (var mi in classInfo.Members.Values.OrderBy(mi => mi.MemberName)) {
          if (mi.NeedsDictionary) {
            sw.WriteLine();
            sw.WriteLine("    /// <summary>");
            sw.WriteLine($"    /// Directory of all {classInfo.PluralName} by {mi.MemberName}");
            sw.WriteLine("    /// </summary>");
            sw.WriteLine($"    public IReadOnlyDictionary<{mi.TypeString.Replace("?", "")}, {classInfo.ClassName}> " +
              $"{classInfo.PluralName}By{mi.MemberName} => _{classInfo.PluralName}By{mi.MemberName};");
            sw.WriteLine($"    internal Dictionary<{mi.TypeString.Replace("?", "")}, {classInfo.ClassName}> " +
              $"_{classInfo.PluralName}By{mi.MemberName} {{ get; private set; }}");
          }
        }
      }
      sw.WriteLine("    #endregion");
      sw.WriteLine();
      sw.WriteLine();
      sw.WriteLine("    #region Events");
      sw.WriteLine("    //      ------");
      sw.WriteLine();
      sw.WriteLine("    #endregion");
      sw.WriteLine();
      sw.WriteLine();
      sw.WriteLine("    #region Constructors");
      sw.WriteLine("    //      ------------");
      sw.WriteLine();
      sw.WriteLine("    /// <summary>");
      sw.WriteLine("    /// Creates a new DataContext. If csvConfig is null, the data is only stored in RAM and gets lost once the ");
      sw.WriteLine("    /// program terminates. With csvConfig defined, existing data gets read at startup, changes get immediately");
      sw.WriteLine("    /// written and Dispose() ensures by flushing that all data is permanently stored.");
      sw.WriteLine("    /// </summary>");
      sw.WriteLine($"    public {context}(CsvConfig? csvConfig): base(DataStoresCount: {classes.Count}) {{");
      sw.WriteLine("      data = this;");
      sw.WriteLine("      IsInitialised = false;");
      WriteLinesTracing(sw, IsTracing,
                   $"      Trace?.Invoke($\"Context {context} initialising\");",
                    "      var trace = Trace;",
                    "      Trace = null;");
      sw.WriteLine();
      sw.WriteLine("      string? backupResult = null;");
      sw.WriteLine("      if (csvConfig!=null) {");
      sw.WriteLine("        backupResult = Csv.Backup(csvConfig, DateTime.Now);");
      WriteLinesTracing(sw, IsTracing,
                   "        if (backupResult.Length>0) {",
                   "           Trace?.Invoke(\"Backup: \" + backupResult);",
                   "        }");
      sw.WriteLine("      }");
      sw.WriteLine();
      sw.WriteLine("      CsvConfig = csvConfig;");
      sw.WriteLine("      onConstructing(backupResult);");
      sw.WriteLine();

      foreach (var classInfo in classes.Values.OrderBy(ci => ci.ClassName)) {
        foreach (var mi in classInfo.Members.Values.OrderBy(mi => mi.MemberName)) {
          if (mi.NeedsDictionary) {
            sw.WriteLine($"      _{classInfo.PluralName}By{mi.MemberName} = new Dictionary<{mi.TypeString.Replace("?", "")}, {classInfo.ClassName}>();");
         }
        }
      }

      sw.WriteLine("      if (csvConfig==null) {");
      foreach (var classInfo in parentChildTree) {
        sw.WriteLine($"        _{classInfo.PluralName} = new DataStore<{classInfo.ClassName}>(");
        sw.WriteLine("          this,");
        sw.WriteLine($"          {classInfo.StoreKey},");
        sw.WriteLine($"          {classInfo.ClassName}.SetKey,");
        sw.WriteLine($"          {classInfo.ClassName}.RollbackItemNew,");
        sw.WriteLine($"          {classInfo.ClassName}.RollbackItemStore,");
        if (classInfo.AreInstancesUpdatable) {
          sw.WriteLine($"          {classInfo.ClassName}.RollbackItemUpdate,");
        } else {
          sw.WriteLine($"          null,");
        }
        if (classInfo.AreInstancesReleasable) {
          sw.WriteLine($"          {classInfo.ClassName}.RollbackItemRelease,");
        } else {
          sw.WriteLine($"          null,");
        }
        //if (classInfo.AreInstancesReleasable) {
        //  sw.WriteLine($"          {classInfo.ClassName}.PerformRelease,");
        //} else {
        //  sw.WriteLine($"          null,");
        //}
        sw.WriteLine($"          areInstancesUpdatable: {classInfo.AreInstancesUpdatable.ToString().ToLowerInvariant()},");
        sw.WriteLine($"          areInstancesReleasable: {classInfo.AreInstancesReleasable.ToString().ToLowerInvariant()});");
        sw.WriteLine($"        DataStores[{classInfo.StoreKey}] = _{classInfo.PluralName};");
        sw.WriteLine($"        on{classInfo.PluralName}Filled();");
        //WriteLinesTracing(sw, IsTracing,
        //             $"        Trace?.Invoke($\"{context}.Data.{classInfo.PluralName} initialised\");");
        sw.WriteLine();
      }
      sw.WriteLine("      } else {");
      sw.WriteLine("        IsPartiallyNew = false;");
      foreach (var classInfo in parentChildTree) {
        sw.WriteLine($"        _{classInfo.PluralName} = new DataStoreCSV<{classInfo.ClassName}>(");
        sw.WriteLine("          this,");
        sw.WriteLine($"          {classInfo.StoreKey},");
        sw.WriteLine("          csvConfig!,");
        sw.WriteLine($"          {classInfo.ClassName}.EstimatedLineLength,");
        sw.WriteLine($"          {classInfo.ClassName}.Headers,");
        sw.WriteLine($"          {classInfo.ClassName}.SetKey,");
        sw.WriteLine($"          {classInfo.ClassName}.Create,");
        if (classInfo.HasParents) {
          sw.WriteLine($"          {classInfo.ClassName}.Verify,");
        } else {
          sw.WriteLine("          null,");
        }
        if (classInfo.AreInstancesUpdatable) {
          sw.WriteLine($"          {classInfo.ClassName}.Update,");
        } else {
          sw.WriteLine($"          null,");
        }
        sw.WriteLine($"          {classInfo.ClassName}.Write,");
        sw.WriteLine($"          {classInfo.ClassName}.RollbackItemNew,");
        sw.WriteLine($"          {classInfo.ClassName}.RollbackItemStore,");
        if (classInfo.AreInstancesUpdatable) {
          sw.WriteLine($"          {classInfo.ClassName}.RollbackItemUpdate,");
        } else {
          sw.WriteLine($"          null,");
        }
        if (classInfo.AreInstancesReleasable) {
          sw.WriteLine($"          {classInfo.ClassName}.RollbackItemRelease,");
        } else {
          sw.WriteLine($"          null,");
        }
        sw.WriteLine($"          areInstancesUpdatable: {classInfo.AreInstancesUpdatable.ToString().ToLowerInvariant()},");
        sw.WriteLine($"          areInstancesReleasable: {classInfo.AreInstancesReleasable.ToString().ToLowerInvariant()});");
        sw.WriteLine($"        IsPartiallyNew |= _{classInfo.PluralName}.IsNew;");
        sw.WriteLine($"        IsNew &= _{classInfo.PluralName}.IsNew;");
        sw.WriteLine($"        DataStores[{classInfo.StoreKey}] = _{classInfo.PluralName};");
        sw.WriteLine($"        on{classInfo.PluralName}Filled();");
        //WriteLinesTracing(sw, IsTracing,
        //             $"        Trace?.Invoke($\"{context}.Data.{classInfo.PluralName} initialised\");");
        sw.WriteLine();
      }
      sw.WriteLine("      }");
      sw.WriteLine("      onConstructed();");
      sw.WriteLine("      IsInitialised = true;");
      WriteLinesTracing(sw, IsTracing,
                    "      Trace = trace;",
                   $"      Trace?.Invoke($\"Context {context} initialised\");");
      sw.WriteLine("    }");
      sw.WriteLine();
      sw.WriteLine("    /// <summary>}");
      sw.WriteLine("    /// Called at beginning of constructor");
      sw.WriteLine("    /// </summary>}");
      sw.WriteLine("    partial void onConstructing(string? backupResult);");
      sw.WriteLine();
      sw.WriteLine("    /// <summary>}");
      sw.WriteLine("    /// Called at end of constructor");
      sw.WriteLine("    /// </summary>}");
      sw.WriteLine("    partial void onConstructed();");
      foreach (var classInfo in parentChildTree) {
        sw.WriteLine();
        sw.WriteLine("    /// <summary>}");
        sw.WriteLine($"    /// Called once the data for {classInfo.PluralName} is read.");
        sw.WriteLine("    /// </summary>}");
        sw.WriteLine($"    partial void on{classInfo.PluralName}Filled();");
      }
      sw.WriteLine("    #endregion");
      sw.WriteLine();
      sw.WriteLine();
      sw.WriteLine("    #region Overrides");
      sw.WriteLine("    //      ---------");
      sw.WriteLine();
      if (IsTracing>TracingEnum.noTracing) {
        if (IsTracing==TracingEnum.debugOnlyTracing) {
          sw.WriteLine("#if DEBUG");
        }
        sw.WriteLine("    protected override void TraceFromBase(TraceMessageEnum traceMessageEnum) {");
        sw.WriteLine("      string message;");
        sw.WriteLine("      switch (traceMessageEnum) {");
        sw.WriteLine("      case TraceMessageEnum.none: return;");
        sw.WriteLine("      case TraceMessageEnum.StartTransaction: message = \"Start transaction\"; break;");
        sw.WriteLine("      case TraceMessageEnum.CommitTransaction: message = \"Commit transaction\"; break;");
        sw.WriteLine("      case TraceMessageEnum.RollingbackTransaction: message = \"Rolling back transaction\"; break;");
        sw.WriteLine("      case TraceMessageEnum.RolledbackTransaction: message = \"Rolled back transaction\"; break;");
        sw.WriteLine("      default:");
        sw.WriteLine("        throw new NotSupportedException();");
        sw.WriteLine("      }");
        sw.WriteLine("      Trace?.Invoke(message);");
        sw.WriteLine("    }");
        if (IsTracing==TracingEnum.debugOnlyTracing) {
          sw.WriteLine("#endif");
        }
        sw.WriteLine();
        sw.WriteLine();
      }
      sw.WriteLine("    internal new void AddTransaction(TransactionItem transactionItem) {");
      sw.WriteLine("      base.AddTransaction(transactionItem);");
      sw.WriteLine("    }");
      sw.WriteLine();
      sw.WriteLine();
      sw.WriteLine("    protected override void Dispose(bool disposing) {");
      sw.WriteLine("      if (disposing) {");
      sw.WriteLine("        onDispose();");
      foreach (var classInfo in ((IEnumerable<ClassInfo>)parentChildTree).Reverse()) {
        sw.WriteLine($"        _{classInfo.PluralName}?.Dispose();");
        sw.WriteLine($"        _{classInfo.PluralName} = null!;");
        foreach (var mi in classInfo.Members.Values.OrderBy(mi => mi.MemberName)) {
          if (mi.NeedsDictionary) {
            sw.WriteLine($"        _{classInfo.PluralName}By{mi.MemberName} = null!;");
          }
        }
      }
      sw.WriteLine("        data = null;");
      sw.WriteLine("      }");
      sw.WriteLine("      base.Dispose(disposing);");
      sw.WriteLine("    }");
      sw.WriteLine();
      sw.WriteLine("    /// <summary>}");
      sw.WriteLine("    /// Called before storageDirectories get disposed.");
      sw.WriteLine("    /// </summary>}");
      sw.WriteLine("    partial void onDispose();");
      sw.WriteLine("    #endregion");
      sw.WriteLine();
      sw.WriteLine();
      sw.WriteLine("    #region Methods");
      sw.WriteLine("    //      -------");
      sw.WriteLine();
      sw.WriteLine("    #endregion");
      sw.WriteLine();
      sw.WriteLine("  }");
      sw.WriteLine("}");
      sw.WriteLine();
    }


    public static void WriteLinesTracing(StreamWriter sw, TracingEnum isTracing, params string[] lines) {
      if (isTracing==TracingEnum.noTracing) return;

      if (isTracing==TracingEnum.debugOnlyTracing) {
        sw.WriteLine("#if DEBUG");
      }
      foreach (var line in lines) {
        sw.WriteLine(line);
      }
      if (isTracing==TracingEnum.debugOnlyTracing) {
        sw.WriteLine("#endif");
      }
    }

    public void WriteEnumsFile(DirectoryInfo targetDirectory) {
      var baseFileNameAndPath = targetDirectory!.FullName + '\\' + "Enums.base.cs";
      try {
        File.Delete(baseFileNameAndPath);
      } catch {
        throw new GeneratorException($"Cannot delete file {baseFileNameAndPath}.");
      }
      using var sw = new StreamWriter(baseFileNameAndPath);
      sw.WriteLine("using System;");
      sw.WriteLine("using System.Collections.Generic;");
      sw.WriteLine("using StorageLib;");
      sw.WriteLine();
      sw.WriteLine();
      sw.WriteLine("namespace " + nameSpaceString + " {");
      foreach (var enumInfo in enums.Values) {
        sw.WriteLine();
        sw.WriteLine();
        sw.WriteLine(enumInfo.CodeLines);
      }
      sw.WriteLine("}");
    }


    public void WriteContentToConsole() {
      foreach (var classInfo in classes.Values) {
        Console.WriteLine(classInfo);
        foreach (var memberInfo in classInfo.Members.Values) {
          Console.WriteLine("  " + memberInfo);
        }
        Console.WriteLine();
      }
    }
    #endregion
  }
}
