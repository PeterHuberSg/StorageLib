/**************************************************************************************

StorageLib.Compiler
===================

Compiles a Model into a DataContext

Written in 2020 by Jürgpeter Huber 
Contact: https://github.com/PeterHuberSg/StorageLib

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

    public IReadOnlyDictionary<string, ClassInfo> Classes => classes;
    readonly Dictionary<string, ClassInfo> classes;

    public IReadOnlyList<ClassInfo> ParentChildTree => parentChildTree;
    readonly List<ClassInfo> parentChildTree;

    public IReadOnlyDictionary<string, EnumInfo> Enums => enums;
    readonly Dictionary<string, EnumInfo> enums;
    #endregion


    #region Constructor
    //      -----------

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

        if (namespaceMember is not ClassDeclarationSyntax classDeclaration) {
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
          if (attribute.Name is not IdentifierNameSyntax attributeName || attributeName.Identifier.Text!="StorageClass") {
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
          if (classMember is not FieldDeclarationSyntax field) {
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

          if (field.Declaration is not VariableDeclarationSyntax variableDeclaration) {
            throw new GeneratorException($"Class {className} {onlyAcceptableConsts} '{field.Declaration}'.");
          }
          var propertyType = variableDeclaration.Type.ToString();
          foreach (var property in variableDeclaration.Variables) {
            string? defaultValue = null;
            bool? isLookupOnly = null;
            bool isParentOneChild = false;
            bool needsDictionary = false;
            string? toLower = null;
            string? childPropertyName = null;
            string? childKeyPropertyName = null;
            string? childKey2PropertyName = null;
            if (field.AttributeLists.Count==0) {
              if (isPropertyWithDefaultValueFound && !propertyType.StartsWith("List<")) {
                throw new GeneratorException($"Property {className}.{property.Identifier.Text} should have a " +
                  "StorageProperty(defaultValue: \"xxx\") attribute, because the previous one had one too. Once a " +
                  "property has a default value, all following properties need to have one too.");
              }
              //use the default values
            } else if (field.AttributeLists.Count>1) {
              throw new GeneratorException($"Property {className}.{property.Identifier.Text} should contain at most " +
                $"1 attribute, i.e. StorageProperty attribute, but has '{field.AttributeLists.Count}' attributes: " +
                $"'{field.AttributeLists}'");

            } else {
              var attributes = field.AttributeLists[0].Attributes;
              if (attributes.Count!=1) throw new GeneratorException($"Property {className}.{property.Identifier.Text} " +
                $"should contain at most 1 attribute, i.e. StorageProperty attribute, but has " +
                $"'{field.AttributeLists.Count}' attributes: '{attributes}'");

              var attribute = attributes[0];
              if (attribute.Name is not IdentifierNameSyntax attributeName || 
                attributeName.Identifier.Text!="StorageProperty") 
              {
                throw new GeneratorException($"Property {className}.{property.Identifier.Text} should contain only " +
                  $"a StorageProperty attribute, but has: '{classDeclaration.AttributeLists}'");
              }
              foreach (var argument in attribute.ArgumentList!.Arguments) {
                if (argument.NameColon is null) throw new GeneratorException($"Property {className}." +
                  $"{property.Identifier.Text} Attribute{attribute}: the argument name is missing, like " +
                  "'defaultValue: null'.");

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
                  case "childPropertyName": childPropertyName = value[1..^1]; break;
                  case "childKeyPropertyName": childKeyPropertyName = value[1..^1]; break;
                  case "childKey2PropertyName": childKey2PropertyName = value[1..^1]; break;
                  default: isUnknown = true; break;
                  }
                } catch (Exception ex) {
                  throw new GeneratorException($"Property {className}.{property.Identifier.Text}: Exception " +
                    $"'{ex.Message}' was thrown when processing attribute '{name}' in '{attribute}'.");
                }
                if (isUnknown) {
                  throw new GeneratorException($"Property {className}.{property.Identifier.Text}: Illegal attribute " +
                    $"name '{name}' in '{attribute}'. It can only be: defaultValue, isLookupOnly, isParentOneChild, " +
                    "lowerFrom or needsDictionary.");
                }
              }
            }
            if ((isLookupOnly??false) && isParentOneChild) {
              throw new GeneratorException($"Property {className}.{property.Identifier.Text} cannot have " + 
                "isLookupOnly: true and isParentOneChild: true in its StorageProperty attribute.");
            }
            classInfo.AddMember(classMember.ToString(), property.Identifier.Text, propertyType, propertyComment, 
              defaultValue, isLookupOnly, isParentOneChild, toLower, needsDictionary, childPropertyName, 
              childKeyPropertyName, childKey2PropertyName, isReadOnly);
          }
        }
      }
    }


    private void parseEnum(EnumDeclarationSyntax enumDeclaration) {
      var enumLeadingComment = getXmlComment(enumDeclaration.GetLeadingTrivia());
      //var enumDeclarationWithLeadingComment = enumDeclaration.ToFullString();
      //var enumDeclarationOnly = removeRegionAndLeadingSimpleComments(enumDeclarationWithLeadingComment);
      var indentation = enumDeclaration.GetLastToken().LeadingTrivia.ToString();
      var enumValues = new List<string>();
      foreach (var enumMember in enumDeclaration.Members) {
        enumValues.Add(enumMember.ToString());
      }
      enums.Add(enumDeclaration.Identifier.Text, 
        new EnumInfo(enumDeclaration.Identifier.Text, enumLeadingComment + indentation + enumDeclaration.ToString(), enumValues));
    }


    private static string addLeadingSpaces(string declaration, int pos) {
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


    private static string? getXmlComment(SyntaxTriviaList syntaxTriviaList) {
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


    private static string? getComment(SyntaxTriviaList syntaxTriviaList) {
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

      //in the first loop, process all members except LinkToParent. The first loop establishes the link between
      //parent and children, while the second loop (LinkToParent only) verifies that every child has a parent.
      foreach (var ci in classes.Values) {
        foreach (var mi in ci.Members.Values) {
          if (mi.ChildTypeName==ci.ClassName) {
            throw new GeneratorException($"In the class {ci}, the property '{mi}' references its own class, " +
              $"which StorageLib cannot support. The reason is that 2 instances of {ci.ClassName} reference " + 
              "each other. When the data gets read from a file, the first instance needs to get created with a " +
              "reference to the other instance, which does not exist yet. A not existing reference throws " +
              "an exception." + Environment.NewLine +
              mi.MemberText);
          }
          if (mi.MemberType>=MemberTypeEnum.ToLower) {
            var isFound = false;
            switch (mi.MemberType) {

            case MemberTypeEnum.ToLower:
              //                --------
              foreach (var member in ci.Members.Values) {
                if (member.MemberName==mi.PropertyForToLower) {
                  if (member.NeedsDictionary && mi.NeedsDictionary) {
                    throw new GeneratorException(ci, mi, $"{mi.MemberName} is a lower case copy of {member.MemberName}. " +
                      "Both have the attribute parameter NeedsDictionary, but only one of them at a time can have a " +
                      "dictionary. StorageLib could be extended to allow both having a Dictionary.");
                  }
                  isFound = true;
                  member.ToLowerTarget = mi;
                  //mi.SetIsNullable(member.IsNullable); it seems IsNullable is already properly set, but not IsReadonly ?
                  mi.IsReadOnly = member.IsReadOnly;
                  break;
                }
              }
              if (!isFound)
                throw new GeneratorException(ci, mi, $"{mi.MemberName} is supposed to be a lower case copy of a " +
                  $"{mi.PropertyForToLower} property in the same class, which cannot be found.");

              break;

            case MemberTypeEnum.LinkToParent:
              //                -------------
              //will be processed in the next loop

              //if (classes.TryGetValue(mi.TypeStringNotNullable, out mi.ParentClassInfo)) {
              //  ci.ParentsAll.Add(mi.ParentClassInfo);
              //  topClasses.Remove(ci.ClassName);
              //  mi.ParentClassInfo.Children.Add(ci);
              //  if (mi.IsLookupOnly) {
              //    if (mi.ParentClassInfo.AreInstancesReleasable) {
              //      throw new GeneratorException($"{ci.ClassName}.{mi.MemberName}: cannot use the deletable instances of class " +
              //        $"{mi.ParentClassInfo.ClassName} as lookup:" + Environment.NewLine + mi.MemberText);
              //    }
              //  } else {
              //    ci.HasParents = true;
              //    if (!mi.IsReadOnly && !mi.IsLookupOnly) {
              //      ci.HasNotReadOnlyAndNotLookupParents = true;
              //    }
              //    //if (ci.ClassName=="Cw2PChild") {
              //    if (ci.ClassName=="DictionaryChild") {
              //      System.Diagnostics.Debugger.Break();
              //    }
              //    foreach (var parentMember in mi.ParentClassInfo.Members.Values) {
              //      if (parentMember.ChildPropertyName is not null) {
              //        //parent member knows which child property to use
              //        if (parentMember.ChildPropertyName==mi.MemberName) {
              //          mi.ParentMemberInfo = parentMember;
              //          parentMembersCount = 1;
              //          if (parentMember.MemberType!=MemberTypeEnum.ParentOneChild) {
              //            parentMember.ChildCount++;
              //          }
              //          break;
              //        }
              //      } else {
              //        //parent member does not know which child property to use, find the one with the proper type
              //        if (parentMember.ChildTypeName==mi.ClassInfo.ClassName) {
              //          parentMembersCount++;
              //          mi.ParentMemberInfo = parentMember;
              //          if (parentMember.MemberType!=MemberTypeEnum.ParentOneChild) {
              //            parentMember.ChildCount++;
              //          }
              //        }
              //      }
              //    }
              //    if (parentMembersCount==1) break;

              //    if (parentMembersCount<1) {
              //      throw new GeneratorException(
              //        $"Property {mi.MemberName} from child class {ci.ClassName} links to parent {mi.ParentClassInfo.ClassName}. " + 
              //        $"But the parent does not have a property which links to the child. Add a collection (list, dictionary or sortedList) " + 
              //        $"to the parent if many children are allowed or a property with the [StorageProperty(isParentOneChild: true)] attribute " + 
              //        $"if only 1 child is allowed. Add [StorageProperty(isLookupOnly: true)] to the child property if the parent " +
              //        "should not have a relation with the child:" + Environment.NewLine + mi.MemberText);
              //    } else {
              //      throw new GeneratorException(
              //        $"Property {mi.MemberName} from child class {ci.ClassName} links to parent {mi.ParentClassInfo.ClassName}. " +
              //        $"But the parent has more than 1 property which links to the child:" + Environment.NewLine + mi.MemberText);
              //    }
              //    //if (!mi.ClassInfo.AreInstancesReleasable && mi.ParentClassInfo.AreInstancesReleasable) {
              //    //  //todo: Compiler.AnalyzeDependencies() Add tests if child is at least updatable, parent property not readonly and nullable
              //    //  throw new GeneratorException($"Child {mi.ClassInfo.ClassName} does not support deletion. Therefore, the " + 
              //    //    $"parent {mi.ParentClassInfo.ClassName} can neither support deletion, because it can not delete its children:" 
              //    //    + Environment.NewLine + mi.MemberText);
              //    //}
              //  }

              //} else if (enums.TryGetValue(mi.TypeStringNotNullable, out mi.EnumInfo)) {
              //  mi.MemberType = MemberTypeEnum.Enum;
              //  mi.ToStringFunc = "";
              //} else {
              //  throw new GeneratorException($"{ci.ClassName}.{mi.MemberName}: cannot find '{mi.TypeStringNotNullable}'. Should this be a data type " +
              //    "defined by Storage, a data model defined enum or a data model defined class ?" + Environment.NewLine + 
              //    mi.MemberText);
              //}
              break;

            case MemberTypeEnum.ParentOneChild:
              //                --------------
              findLinkToParentPropertyInChildClassAndSetupParentChildLinks(mi, topClasses);

              //if (!classes.TryGetValue(mi.ChildTypeName!, out mi.ChildClassInfo))
              //  throw new GeneratorException($"{ci} '{mi}': cannot find class {mi.ChildTypeName}:" + Environment.NewLine + 
              //    mi.MemberText);

              //foreach (var childMI in mi.ChildClassInfo.Members.Values) {
              //  if (childMI.MemberType==MemberTypeEnum.LinkToParent && childMI.TypeStringNotNullable==ci.ClassName) {
              //    isFound = true;
              //    mi.ChildMemberInfo = childMI;
              //    mi.IsChildReadOnly = childMI.IsReadOnly;
              //    break;
              //  }
              //}
              //if (!isFound) {
              //  //guarantee that there is a property linking to the parent for each child class.
              //  throw new GeneratorException($"{ci} '{mi}' is a property which links to 0 or 1 child. A corresponding " +
              //    $"property with type {ci.ClassName} is missing in the class {mi.ChildTypeName}:" + Environment.NewLine + 
              //    mi.MemberText);
              //}
              break;

            case MemberTypeEnum.ParentMultipleChildrenList:
              //                --------------------------
              findLinkToParentPropertyInChildClassAndSetupParentChildLinks(mi, topClasses);
              if (mi.ChildClassInfo!.AreInstancesReleasable) {
                mi.TypeString = $"StorageList<{mi.ChildClassInfo.ClassName}>";
                mi.ReadOnlyTypeString = $"IStorageReadOnlyList<{mi.ChildClassInfo.ClassName}>";
              } else {
                mi.TypeString = $"List<{mi.ChildClassInfo.ClassName}>";
                mi.ReadOnlyTypeString = $"IReadOnlyList<{mi.ChildClassInfo.ClassName}>";
              }

              //if (!classes.TryGetValue(mi.ChildTypeName!, out mi.ChildClassInfo))
              //  throw new GeneratorException($"{ci} '{mi}': can not find class {mi.ChildTypeName}:" + Environment.NewLine + 
              //    mi.MemberText);

              //foreach (var childMI in mi.ChildClassInfo.Members.Values) {
              //  if (childMI.MemberType==MemberTypeEnum.LinkToParent && childMI.TypeStringNotNullable==ci.ClassName) {
              //    isFound = true;
              //    mi.IsChildReadOnly |= childMI.IsReadOnly;
              //  }
              //}
              //if (!isFound) {
              //  //guarantee that there is a property linking to the parent for each child class.
              //  throw new GeneratorException($"{ci} '{mi}': has a List<{mi.ChildTypeName}>. The corresponding " +
              //    $"property with type {ci.ClassName} is missing in the class {mi.ChildTypeName}:" + Environment.NewLine + 
              //    mi.MemberText);
              //}
              break;

            case MemberTypeEnum.ParentMultipleChildrenHashSet:
              findLinkToParentPropertyInChildClassAndSetupParentChildLinks(mi, topClasses);
              if (mi.ChildClassInfo!.AreInstancesReleasable) {
                mi.TypeString = $"StorageHashSet<{mi.ChildClassInfo.ClassName}>";
                mi.ReadOnlyTypeString = $"IStorageReadOnlySet<{mi.ChildClassInfo.ClassName}>";
              } else {
                mi.TypeString = $"HashSet<{mi.ChildClassInfo.ClassName}>";
                mi.ReadOnlyTypeString = $"IReadOnlySet<{mi.ChildClassInfo.ClassName}>";
              }
              break;

            case MemberTypeEnum.ParentMultipleChildrenDictionary:
            case MemberTypeEnum.ParentMultipleChildrenSortedList:
              findLinkToParentPropertyInChildClassAndSetupParentChildLinks(mi, topClasses);
              findKeyPropertyInChildClass(mi, mi.ChildKeyPropertyName, mi.ChildKeyTypeString, isSecondKey: false);

              var childKeyTypeString = mi.ChildKeyTypeString;
              var childKeyClassName = mi.SingleChildMI!.ClassInfo.ClassName;
              if (mi.MemberType==MemberTypeEnum.ParentMultipleChildrenDictionary) {
                //memberTypeString = $"Dictionary<{keyTypeName}, {itemTypeName}>";
                if (mi.ChildClassInfo!.AreInstancesReleasable) {
                  mi.TypeString = $"StorageDictionary<{childKeyTypeString}, {childKeyClassName}>";
                  mi.ReadOnlyTypeString = $"IStorageReadOnlyDictionary<{childKeyTypeString}, {childKeyClassName}>";
                } else {
                  mi.TypeString = $"Dictionary<{childKeyTypeString}, {childKeyClassName}>";
                  mi.ReadOnlyTypeString = $"IReadOnlyDictionary<{childKeyTypeString}, {childKeyClassName}>";
                }
              } else {
                //memberTypeString = $"SortedList<{keyTypeName}, {itemTypeName}>";
                if (mi.ChildClassInfo!.AreInstancesReleasable) {
                  mi.TypeString = $"StorageSortedList<{childKeyTypeString}, {childKeyClassName}>";
                  mi.ReadOnlyTypeString = $"IStorageReadOnlyDictionary<{childKeyTypeString}, {childKeyClassName}>";
                } else {
                  mi.TypeString = $"SortedList<{childKeyTypeString}, {childKeyClassName}>";
                  mi.ReadOnlyTypeString = $"IReadOnlyDictionary<{childKeyTypeString}, {childKeyClassName}>";
                }
              }
              mi.TypeStringNotNullable = mi.TypeString;
              break;

            //case MemberTypeEnum.ParentMultipleChildrenSortedList:
            //                --------------------------------

            //Dictionary, SortedList

            //if (!classes.TryGetValue(mi.ChildTypeName!, out mi.ChildClassInfo))
            //  throw new GeneratorException($"{ci} '{mi}': cannot find class {mi.ChildTypeName}:" + Environment.NewLine + 
            //    mi.MemberText);

            ////search for member in child class which has a parent linking to mi
            //foreach (var childMI in mi.ChildClassInfo.Members.Values) {
            //  if (childMI.MemberType==MemberTypeEnum.LinkToParent && childMI.TypeStringNotNullable==ci.ClassName) {
            //    //child property found pointing to parent. 
            //    mi.IsChildReadOnly = childMI.IsReadOnly;
            //    MemberInfo? childKeyMIFound = null;
            //    //Find another child property which is used as key into the Dictionary or SortedList
            //    foreach (var childKeyMI in mi.ChildClassInfo.Members.Values) {
            //      if (mi.ChildKeyTypeString==childKeyMI.CsvTypeString || mi.ChildKeyTypeString==childKeyMI.TypeString) {
            //        if (mi.ChildKeyPropertyName is null) {
            //          //parent class does not know the name of the child property used as key. Use a property with the proper key type.
            //          if (childKeyMIFound is null) {
            //            //first property found with the expected key type
            //            childKeyMIFound = childKeyMI;
            //          } else {
            //            //second property found with the expected key type, throw exception
            //            throw new GeneratorException($"{ci}.{mi.MemberName} {mi.TypeString}: found " +
            //              $"{childMI.ClassInfo.ClassName}.{childMI.MemberName}, but found two properties in {childMI.ClassInfo.ClassName} " +
            //              $"with the type {mi.ChildKeyTypeString}: {childKeyMIFound.MemberName}, {childKeyMI.MemberName}. Use " +
            //              $"[StorageProperty(childKeyPropertyName: \"Xyz\")] in the parent to indicate which property should be used." +
            //              Environment.NewLine + mi.MemberText);
            //          }
            //        } else {
            //          //parent collection class knows the name of the child property to be used as key
            //          if (mi.ChildKeyPropertyName==childKeyMI.MemberName) {
            //            //property found in child class which matches the expected key name
            //            childKeyMIFound = childKeyMI;
            //            //the c# compiler enforces that each property name of a class is unique. No need to check if there is another.
            //            break;
            //          }
            //        }
            //      } else {
            //        //child property has wrong key type. Throw exception if it has the expected key name.
            //        if (mi.ChildKeyPropertyName is not null && mi.ChildKeyPropertyName==childKeyMI.MemberName) {
            //          throw new GeneratorException($"{ci}.{mi.MemberName} {mi.TypeString}: found " +
            //            $"{childKeyMI.ClassInfo.ClassName}.{childKeyMI.MemberName}, but it has wrong type: " +
            //            $"{childKeyMI.CsvTypeString}:" + Environment.NewLine + mi.MemberText);
            //        }
            //      }
            //    }

            //    if (childKeyMIFound is null) {
            //      if (mi.ChildKeyPropertyName is null) {
            //        throw new GeneratorException($"{ci}.{mi.MemberName} {mi.TypeString}: found " +
            //          $"{childMI.ClassInfo.ClassName}.{childMI.MemberName}, but could not find another property in {childMI.ClassInfo.ClassName} " +
            //          $"with the type {mi.ChildKeyTypeString} needed as key." + Environment.NewLine + mi.MemberText);
            //      } else {
            //        throw new GeneratorException($"{ci}.{mi.MemberName} {mi.TypeString}: found " +
            //          $"{childMI.ClassInfo.ClassName}.{childMI.MemberName}, but could not find another property with the name" +
            //          $"{mi.ChildKeyPropertyName} and type {mi.ChildKeyTypeString} needed as key." + Environment.NewLine + mi.MemberText);
            //      }

            //    } else {
            //      isFound = true;
            //      mi.ChildMemberInfo = childMI;
            //      if (mi.MemberType==MemberTypeEnum.ParentMultipleChildrenSortedList) {
            //        //memberTypeString = $"SortedList<{keyTypeName}, {itemTypeName}>";
            //        if (childMI.ClassInfo.AreInstancesReleasable) {
            //          mi.TypeString = $"StorageSortedList<{childKeyMIFound.TypeString}, {childKeyMIFound.ClassInfo.ClassName}>";
            //          mi.ReadOnlyTypeString = $"IStorageReadOnlyDictionary<{childKeyMIFound.TypeString}, {childKeyMIFound.ClassInfo.ClassName}>";
            //        } else {
            //          mi.TypeString = $"SortedList<{childKeyMIFound.TypeString}, {childKeyMIFound.ClassInfo.ClassName}>";
            //          mi.ReadOnlyTypeString = $"IReadOnlyDictionary<{childKeyMIFound.TypeString}, {childKeyMIFound.ClassInfo.ClassName}>";
            //        }
            //      } else {
            //        //Dictionary
            //        //memberTypeString = $"Dictionary<{keyTypeName}, {itemTypeName}>";
            //        if (childMI.ClassInfo.AreInstancesReleasable) {
            //          mi.TypeString = $"StorageDictionary<{childKeyMIFound.TypeString}, {childKeyMIFound.ClassInfo.ClassName}>";
            //          mi.ReadOnlyTypeString = $"IStorageReadOnlyDictionary<{childKeyMIFound.TypeString}, {childKeyMIFound.ClassInfo.ClassName}>";
            //        } else {
            //          mi.TypeString = $"Dictionary<{childKeyMIFound.TypeString}, {childKeyMIFound.ClassInfo.ClassName}>";
            //          mi.ReadOnlyTypeString = $"IReadOnlyDictionary<{childKeyMIFound.TypeString}, {childKeyMIFound.ClassInfo.ClassName}>";
            //        }
            //      }
            //      if (mi.ChildKeyPropertyName is null) {
            //        mi.ChildKeyPropertyName = childKeyMIFound.MemberName;
            //        mi.LowerChildKeyPropertyName = childKeyMIFound.MemberName.ToCamelCase();
            //      }
            //      break;
            //    }
            //  }
            //}
            //if (!isFound) {
            //  //guarantee that there is a property linking to the parent for each child class.
            //  if (mi.MemberType==MemberTypeEnum.ParentMultipleChildrenSortedList) {
            //    throw new GeneratorException($"{ci} '{mi}': has a SortedList<{mi.ChildTypeName}>. The corresponding " +
            //      $"property with type {ci.ClassName} is missing in the class {mi.ChildTypeName}:" + Environment.NewLine + 
            //      mi.MemberText);
            //  } else {
            //    throw new GeneratorException($"{ci} '{mi}': has a Dictionary<{mi.ChildTypeName}>. The corresponding " +
            //      $"property with type {ci.ClassName} is missing in the class {mi.ChildTypeName}:" + Environment.NewLine + 
            //      mi.MemberText);
            //  }
            //}

            //break;

            case MemberTypeEnum.ParentMultipleChildrenSortedBucket:
              //                ----------------------------------

              findLinkToParentPropertyInChildClassAndSetupParentChildLinks(mi, topClasses);

              //if (!classes.TryGetValue(mi.ChildTypeName!, out mi.ChildClassInfo))
              //  throw new GeneratorException($"{ci} '{mi}': cannot find class {mi.ChildTypeName}:" + Environment.NewLine +
              //    mi.MemberText);

              ////search for member in child class which has a parent property linking to mi
              //foreach (var childMI in mi.ChildClassInfo.Members.Values) {
              //  if (childMI.MemberType==MemberTypeEnum.LinkToParent && childMI.TypeStringNotNullable==ci.ClassName) {
              //    //child property found pointing to parent.
              //    if (mi.ChildMemberInfo is null) {
              //      mi.ChildMemberInfo = childMI;
              //      mi.IsChildReadOnly = childMI.IsReadOnly;
              //    } else {
              //      throw new GeneratorException($"{ci}.{mi.MemberName} {mi.TypeString}: The 2 properties " + 
              //        $"{mi.ChildMemberInfo.MemberName} and {childMI.MemberName} in class {mi.ChildClassInfo.ClassName} link " +
              //        $"to parent {ci}, but StorageLib supports only 1 child property doing so." + Environment.NewLine + mi.MemberText);
              //    }
              //  }
              //}
              //if (mi.ChildMemberInfo is null) {
              //  throw new GeneratorException($"{ci} '{mi}': has a SortedBucketCollection<{mi.ChildTypeName}>. The corresponding " +
              //    $"property with type {ci.ClassName} is missing in the class {mi.ChildTypeName}:" + Environment.NewLine +
              //    mi.MemberText);
              //}

              //findParentRelatedPropertiesInChild(
              //  ci,
              //  mi,
              //  mi.ChildKeyTypeString!,
              //  "ChildKeyPropertyName",
              //  ref mi.ChildKeyPropertyName,
              //  ref mi.LowerChildKeyPropertyName,
              //  out var childKey1TypeString);
              //findParentRelatedPropertiesInChild(
              //  ci,
              //  mi,
              //  mi.ChildKey2TypeString!,
              //  "ChildKey2PropertyName",
              //  ref mi.ChildKey2PropertyName,
              //  ref mi.LowerChildKey2PropertyName,
              //  out var childKey2TypeString);
              findKeyPropertyInChildClass(mi, mi.ChildKeyPropertyName, mi.ChildKeyTypeString, isSecondKey: false);
              findKeyPropertyInChildClass(mi, mi.ChildKey2PropertyName, mi.ChildKey2TypeString, isSecondKey: true);

              //memberTypeString = $"SortedBucketCollection<{key1TypeName}, {key2TypeName}, {itemTypeName}>";
              var genericParameters = $"{mi.ChildKeyTypeString}, {mi.ChildKey2TypeString}, {mi.ChildClassInfo!.ClassName}";
              if (mi.ChildClassInfo.AreInstancesReleasable) {
                mi.TypeString = $"StorageSortedBucketCollection<{genericParameters}>";
                mi.ReadOnlyTypeString = $"IStorageReadOnlySortedBucketCollection<{genericParameters}>";
              } else {
                mi.TypeString = $"SortedBucketCollection<{genericParameters}>";
                mi.ReadOnlyTypeString = $"IReadOnlySortedBucketCollection<{genericParameters}>";
              }
              mi.TypeStringNotNullable = mi.TypeString;
              break;

            default: throw new NotSupportedException($"Compiler.AnalyzeDependencies(mi.MemberType: {mi.MemberType})");
            }
          }
        }
      }

      //second loop over all classes, processing only MemberTypeEnum.LinkToParent, verifying that every child member
      //has a parent
      foreach (var ci in classes.Values) {
        foreach (var mi in ci.Members.Values) {
          if (mi.MemberType==MemberTypeEnum.LinkToParent) {
            if (mi.ParentClassInfo is null) {
              if (enums.TryGetValue(mi.TypeStringNotNullable, out mi.EnumInfo)) {
                mi.MemberType = MemberTypeEnum.Enum;
                mi.ToStringFunc = "";

              } else if (mi.IsLookupOnly) {
                if (classes.TryGetValue(mi.TypeStringNotNullable, out var parentClassInfo)) {
                  if (parentClassInfo.AreInstancesReleasable) {
                    throw new GeneratorException(ci, mi, $"Cannot use the deletable instances of class {parentClassInfo.ClassName} " +
                      "as lookup.");
                  }
                  setupParentClassChildPropertyLinks(parentClassInfo, mi, topClasses);
                } else {
                  throw new GeneratorException(ci, mi, $"{mi.MemberName} links to the class {mi.TypeStringNotNullable}, which is " +
                    "missing in the data model.");
                }

              } else {
                if (classes.TryGetValue(mi.TypeStringNotNullable, out var parentClassInfo)) {
                  foreach (var parentMi in parentClassInfo.Members.Values) {
                    if (parentMi.ChildTypeName==ci.ClassName) {
                      if (parentMi.MemberType is MemberTypeEnum.ParentMultipleChildrenList &&
                        parentMi.MemberText.Contains("childPropertyName")) 
                      {
                        throw new GeneratorException(ci, mi,
                          $"Parent class: {parentMi.ClassInfo.ClassName}" + Environment.NewLine +
                          $"Parent property: {parentMi.MemberText}" + Environment.NewLine +
                          $"Property {mi.MemberName} in child class {mi.ClassInfo.ClassName} references property " +
                          $"{parentMi.MemberName} in parent class {parentMi.ClassInfo.ClassName}, which links " +
                          $"explicitely to {parentMi.SingleChildMI!.MemberName} in class " +
                          $"{parentMi.ChildClassInfo!.ClassName}. Remove StoragePropertyAttribute." +
                          $"childPropertyName from {parentMi.MemberName} if more than 1 property in child class " +
                          $"{mi.ClassInfo.ClassName} should reference {parentMi.ClassInfo.ClassName}." +
                          $"{parentMi.MemberName}, in which case the List<> will get replaced by a HashSet<> " +
                          "in the generated code.");
                      }else if (parentMi.MemberType is MemberTypeEnum.ParentMultipleChildrenHashSet &&
                        parentMi.MemberText.Contains("childPropertyName")) 
                      {
                        throw new GeneratorException(parentMi.ClassInfo, parentMi,
                         $"Remove StorageProperty attribute from {parentMi.ClassInfo.ClassName}.{parentMi.MemberName} " +
                         $"which is a HashSet. A HashSet links to every property " +
                         $"which has the type {parentMi.ClassInfo.ClassName} in child class {mi.ClassInfo.ClassName}.");
                      } else {
                        throw new GeneratorException(ci, mi, $"The parent class {mi.TypeStringNotNullable} is not linking back to " +
                          $"{mi.MemberName} property. This can happen if 2 or more properties of {ci.ClassName} link to " +
                          $"{mi.TypeStringNotNullable} class. In this case, several collections or properties for singel child " +
                          $"are needed in the {mi.TypeStringNotNullable} class and they need to use " +
                          $"StoragePropertyAttribute.ChildPropertyName to indicate which of their property links to which " +
                          $"{ci.ClassName} property.");
                      }
                    }
                  }
                  throw new GeneratorException(ci, mi, $"Property {mi.MemberName} in child class {ci.ClassName} " +
                    $"references parent class {parentClassInfo.ClassName}, which does not have a property referencing " +
                    $"{ci.ClassName}.");


                } else {
                  throw new GeneratorException(ci, mi, $"The declaration of {mi.TypeStringNotNullable} is missing. Should this be a data " +
                    "type defined by StorageLib, a data model defined enum or a data model defined class ?");
                }
              }

            } else {
              //parent has allready set up the links to the child, nothing to do

              //if (!mi.ClassInfo.AreInstancesReleasable && mi.ParentClassInfo.AreInstancesReleasable) {
              //  //todo: Compiler.AnalyzeDependencies() Add tests if child is at least updatable, parent property not readonly and nullable
              //  throw new GeneratorException($"Child {mi.ClassInfo.ClassName} does not support deletion. Therefore, the " + 
              //    $"parent {mi.ParentClassInfo.ClassName} can neither support deletion, because it can not delete its children:" 
              //    + Environment.NewLine + mi.MemberText);
              //}
            }
          }
        }
      }

      //create parent child tree. Classes which have no parents are the roots of the tree. Classes which have 
      //parents are added to the parent class and are not roots.
      foreach (var classInfo in topClasses.Values) {
        addToParentChildTree(classInfo);
      }
      //verify that every class is part of the parent child tree. This test should never fail.
      foreach (var classInfo in classes.Values) {
        if (!classInfo.IsAddedToParentChildTree) throw new Exception();
      }
    }


    /// <summary>
    /// A member of the parent class links to a member in a child class. Find that member and update link properties of 
    /// the parent and child accordingly.
    /// </summary>
    private void findLinkToParentPropertyInChildClassAndSetupParentChildLinks(
      MemberInfo parentMI,
      Dictionary<string, ClassInfo> topClasses) 
    {
      //the property in the child class referencing the parent class can be found:
      //a) if the parent class property knows the child class property's name based on
      //   StoragePropertyAttribute.childPropertyName. This is needed if 2 properties in the child class each have
      //   the type of the parent class
      //b) if the child class has only one property with the type of the parent class, use it.
      var parentCI = parentMI.ClassInfo;
      if (!classes.TryGetValue(parentMI.ChildTypeName!, out parentMI.ChildClassInfo))
        throw new GeneratorException(parentCI, parentMI, $"Cannot find class {parentMI.ChildTypeName}.");

      MemberInfo? childMemberInfo = null;
      List<MemberInfo>? childMemberInfos = null;
      if (parentMI.ChildPropertyName is null) {
        //the data model does not specify which child property the parent should use. Search a matching one.
        foreach (var childMI in parentMI.ChildClassInfo.Members.Values) {
          if (childMI.MemberType==MemberTypeEnum.LinkToParent && childMI.TypeStringNotNullable==parentCI.ClassName) {
            if (childMI.ParentMemberInfo is not null) {
              throw new GeneratorException(parentCI, parentMI,
                $"Property {parentMI.MemberName} in parent class {parentCI.ClassName} references child class " +
                $"{parentMI.ChildTypeName}, which has the property {childMI.MemberName} with the proper type " +
                $"{parentCI.ClassName}, but it is used already by {childMI.ParentMemberInfo.ClassInfo!.ClassName}." +
                $"{childMI.ParentMemberInfo.MemberName}. If 2 or more properties in the parent class {parentCI.ClassName} " +
                $"references 2 or more properties in the child class {parentMI.ChildTypeName}, use StoragePropertyAttribute." + 
                $"childPropertyName to define which parent properties reference which child properties.");
            }

            if (childMemberInfos is null) {
              if (childMemberInfo is null) {
                //first time we come here, so far only one child property linkg to parent class
                childMemberInfo = childMI;
              } else {
                //second time we come here, 2 child properties link to parent class
                if (parentMI.MemberType!=MemberTypeEnum.ParentMultipleChildrenHashSet) {
                  throw new GeneratorException(parentCI, parentMI, 
                    $"The child class {parentMI.ChildClassInfo.ClassName} has 2 properties linking to parent class " + 
                    $"{parentCI.ClassName}:" + Environment.NewLine + 
                    $"{childMemberInfo.MemberText}" + Environment.NewLine + 
                    $"{childMI.MemberText}" + Environment.NewLine +
                    $"Use HashSet<{childMI.ClassInfo.ClassName}> if more than one child property links to " +
                    $"{parentMI.MemberName} or add to {parentCI.ClassName} one List<{childMI.ClassInfo.ClassName}> " +
                    $"for each child property with the type {parentCI.ClassName} and specify with attribut " +
                    $"StorageProperty.ChildPropertyName which child property links to which List<> in Parent.");
                }
                childMemberInfos = new();
                childMemberInfos.Add(childMemberInfo);
                childMemberInfo = null;
                childMemberInfos.Add(childMI);
              }
            } else {
              //there are more than 2 child properties linking to the parent
              childMemberInfos.Add(childMI);
            }
            //The second loop in findParentRelatedPropertyInChildClass() dealing with LinkToParent will check and
            //raise an exception if a child links to a not existing parent. The code here gets executed in the first
            //loop of AnalyzeDependencies() dealing with all none LinkToParent properties.
          }
        }

        if (childMemberInfo is not null) {
          //setup links between parent property and single child property
          if (parentMI.MemberType==MemberTypeEnum.ParentMultipleChildrenHashSet) {
            throw new GeneratorException(parentMI.ClassInfo, parentMI,
              $"{parentMI.MemberName} is of type HashSet, which is used when the child class hass several properties " +
              $"linking to the parent class. But the child class {childMemberInfo.ClassInfo.ClassName} has only " +
              $"the property {childMemberInfo.MemberName} with they type {parentMI.ClassInfo.ClassName}. With only " + 
              $"one child class property linking to parent class use List<> instead of HashSet<>.");
          }
          setupParentPropertyChildPropertyLinks(parentMI, childMemberInfo!, topClasses);
        } else if (childMemberInfos is not null) {
          //setup links between parent property and multiple child properties
          setupParentPropertyChildPropertiesLinks(parentMI, childMemberInfos!, topClasses);
        } else {
            //There is no property in the child class linking to the parent
            throw new GeneratorException(parentCI, parentMI,
              $"{parentMI.MemberName} references the class {parentMI.ChildTypeName}. A corresponding property with " +
              $"type {parentCI.ClassName} is missing in {parentMI.ChildTypeName}.");
        }

      } else {
        //parent knows the child's property name already
        if (parentMI.ChildClassInfo.Members.TryGetValue(parentMI.ChildPropertyName, out childMemberInfo)) {
          if (childMemberInfo.MemberType!=MemberTypeEnum.LinkToParent || 
            childMemberInfo.TypeStringNotNullable!=parentCI.ClassName) 
          {
            //property in child class with the searched name has wrong type
            //Note: TypeStringNotNullable for LinkToParent members has the same value like TypeString, but
            //without the trailing '?' if nullable.
            throw new GeneratorException(parentCI, parentMI,
              $"Property {parentMI.ChildPropertyName} in child class {parentMI.ChildClassInfo.ClassName} should " +
              $"have the type {parentCI.ClassName}, but its type is {childMemberInfo.TypeString}:" + Environment.NewLine + 
              $"Class: {childMemberInfo.ClassInfo.ClassName}" + Environment.NewLine +
              $"Property: {childMemberInfo.MemberText}");
          }
        } else {
          //cannot find property in child with the name==ChildPropertyName
          throw new GeneratorException(parentCI, parentMI,
            $"Can not find property {parentMI.ChildPropertyName} in child class {parentMI.ChildTypeName}.");
        }
        //setup links between parent property and single child property
        setupParentPropertyChildPropertyLinks(parentMI, childMemberInfo, topClasses);
      }
    }


    ///// <summary>
    ///// Used by ParentOneChild, collections like List, Dictionary, etc. to set the properties in the parent and 
    ///// child. Not used by IsLookupOnly child, because in that case there is no parentMI, only a parentCI.
    ///// </summary>
    //private static void setupParentChildLinks(
    //  MemberInfo parentMI, 
    //  MemberInfo? singleChildMI,
    //  List<MemberInfo>? multipleChildrenMIs,
    //  Dictionary<string, ClassInfo> topClasses) 
    //{
    //  //parentMI.SingleChildMI = singleChildMI;
    //  //parentMI.MultipleChildrenMIs = multipleChildrenMIs;
    //  //parentMI.IsChildReadOnly |= childMemberInfo.IsReadOnly;
    //  //childMemberInfo!.ParentMemberInfo = parentMI;
    //  //setupParentChildLinks(parentMI.ClassInfo, childMemberInfo!, topClasses);
    //  //if (!childMemberInfo!.IsReadOnly && !childMemberInfo!.IsLookupOnly) {
    //  //  childMemberInfo!.ClassInfo.HasNotReadOnlyAndNotLookupParents = true;
    //  //}
    //  parentMI.SingleChildMI = singleChildMI;
    //  parentMI.MultipleChildrenMIs = multipleChildrenMIs;
    //  if (singleChildMI is not null) {
    //    parentMI.IsChildReadOnly |= singleChildMI.IsReadOnly;
    //    singleChildMI.ParentMemberInfo = parentMI;
    //    singleChildMI.ClassInfo.HasNotReadOnlyAndNotLookupParents =
    //      !singleChildMI.IsReadOnly && !singleChildMI.IsLookupOnly;
    //    setupParentSingleChildLinks(parentMI.ClassInfo, singleChildMI, topClasses);
    //  } else {
    //    //List<> which will get changed to HashSet
    //    ClassInfo? childCI = null;
    //    foreach (var childMI in multipleChildrenMIs!) {
    //      if (childCI is null) {
    //        childCI = childMI.ClassInfo;
    //      }
    //      parentMI.IsChildReadOnly |= childMI.IsReadOnly;
    //      childMI.ParentMemberInfo = parentMI;
    //      childCI.HasNotReadOnlyAndNotLookupParents |= !childMI.IsReadOnly && !childMI.IsLookupOnly;
    //    }
    //  }
    //}


    /// <summary>
    /// Used by ParentOneChild, collections like List, Dictionary, etc. to set the properties in the parent and 
    /// child. Not used by IsLookupOnly child, because in that case there is no parentMI, only a parentCI. Not
    /// used by List<Child>, where child has multiple properties linking to parent.
    /// </summary>
    private static void setupParentPropertyChildPropertyLinks(
      MemberInfo parentMI,
      MemberInfo singleChildMI,
      Dictionary<string, ClassInfo> topClasses) 
    {
      parentMI.SingleChildMI = singleChildMI;
      parentMI.IsChildReadOnly |= singleChildMI.IsReadOnly;
      singleChildMI.ParentMemberInfo = parentMI;
      singleChildMI.ClassInfo.HasNotReadOnlyAndNotLookupParents |=
        !singleChildMI.IsReadOnly && !singleChildMI.IsLookupOnly;
      setupParentClassChildPropertyLinks(parentMI.ClassInfo, singleChildMI, topClasses);
    }


    /// <summary>
    /// Used by List<>, if there are several properties in the child class referencing the parent class. List<>
    /// will get replaced with a HasSet<>
    /// </summary>
    private static void setupParentPropertyChildPropertiesLinks(
      MemberInfo parentMI,
      List<MemberInfo> multipleChildrenMIs,
      Dictionary<string, ClassInfo> topClasses) 
    {
      parentMI.MultipleChildrenMIs = multipleChildrenMIs;
      ClassInfo? childCI = null;
      foreach (var childMI in multipleChildrenMIs!) {
        if (childCI is null) {
          childCI = childMI.ClassInfo;
        }
        parentMI.IsChildReadOnly |= childMI.IsReadOnly;
        childMI.ParentMemberInfo = parentMI;
        childCI.HasNotReadOnlyAndNotLookupParents |= !childMI.IsReadOnly && !childMI.IsLookupOnly;
      }
      setupParentClassChildPropertiesLinks(parentMI.ClassInfo, multipleChildrenMIs, topClasses);
    }


    /// <summary>
    /// Used by ParentOneChild, IsLookupOnly child and collections like Dictionary, SortedList, etc. It is only 
    /// used by List<>, if there is only one property in the child class referencing the parent class
    /// </summary>
    private static void setupParentClassChildPropertyLinks(
      ClassInfo parentCI,
      MemberInfo singelChildMI,
      Dictionary<string, ClassInfo> topClasses) 
    {
      singelChildMI.ParentClassInfo = parentCI;
      setupParentClassChildClassLinks(parentCI, singelChildMI.ClassInfo, topClasses);
    }


    /// <summary>
    /// Used by List<>, if there are several properties in the child class referencing the parent class. List<>
    /// will get replaced with a HasSet<>
    /// </summary>
    private static void setupParentClassChildPropertiesLinks(
      ClassInfo parentCI,
      List<MemberInfo> multipleChildrenMIs,
      Dictionary<string, ClassInfo> topClasses) 
    {
      var isFirst = true;
      foreach (var childMI in multipleChildrenMIs!) {
        if (isFirst) {
          isFirst = false;
          setupParentClassChildClassLinks(parentCI, childMI.ClassInfo, topClasses);
        }
        childMI.ParentClassInfo = parentCI;
      }
    }


    /// <summary>
    /// Used by ParentOneChild, IsLookupOnly child and collections like Dictionary, SortedList, etc. to setup
    /// links between parent class and child class
    /// </summary>
    private static void setupParentClassChildClassLinks(
       ClassInfo parentCI,
       ClassInfo childCI,
       Dictionary<string, ClassInfo> topClasses) 
    {
      parentCI.Children.Add(childCI);
      childCI.ParentsAll.Add(parentCI);
      childCI.HasParents = true;
      topClasses.Remove(childCI.ClassName);
    }


    /// <summary>
    /// Finds the child class property which should be used as key for a collection in the parent class. Returns
    /// null if isSecondKey and the property child property Key is used as key. Key exisists for every class, but
    /// has no MemberInfo.
    /// </summary>
    private MemberInfo? findKeyPropertyInChildClass(
      MemberInfo parentMI,//has a value for parentMI.ChildClassInfo
      string? childKeyPropertyName,
      string? childKeyPropertyType,
      bool isSecondKey) 
    {
      //the property in the child class used as key into the parent children collection can be found:
      //a) if the parent class property knows the child class property's name based on
      //   StoragePropertyAttribute.childPropertyName. This is needed if 2 properties in the child class each have
      //   the type the parent class is looking for
      //b) if the child class has only one property with the type the parent class is looking for.
      var parentCI = parentMI.ClassInfo;
      MemberInfo? childKeyMI = null;
      if (childKeyPropertyName is null) {
        //the data model does not specify which child property the parent should use. Search a property with the
        //type the parent class is looking for.
        foreach (var childMI in parentMI.ChildClassInfo!.Members.Values) {
          if (childKeyPropertyType!=childMI.CsvTypeString && childKeyPropertyType==childMI.TypeStringNotNullable) {
            System.Diagnostics.Debugger.Break();//Todo: Remove this test if Break is ever reached, otherwise simplify if()
          }

          if (childKeyPropertyType==childMI.CsvTypeString || childKeyPropertyType==childMI.TypeStringNotNullable) {
            //if (childMI.TypeStringNotNullable==childKeyPropertyType) {
            if (childKeyMI is null) {
              childKeyMI = childMI;
            } else {
              var attribute = isSecondKey ? "ChildKeyPropertyName" : "ChildKey2PropertyName";
              var second = isSecondKey ? "second " : "";
              throw new GeneratorException(parentCI, parentMI,
                $"The collection {parentMI.MemberName} in parent class {parentCI.ClassName} needs a {second}key " +
                $"with the type {childKeyPropertyType}. There are 2 properties in {parentMI.ChildClassInfo.ClassName} " +
                $"with the type {childKeyPropertyType}. Use StoragePropertyAttribute.{attribute} to indicate which " +
                "property should be used.");
            }
          }
        }

        if (childKeyMI is null)
          if (isSecondKey && childKeyPropertyType=="int") {
            //use the Key property as second key for SortedBucketCollection 
            parentMI.ChildKey2PropertyName = "Key";
            parentMI.LowerChildKey2PropertyName = "key";
            parentMI.ChildKey2TypeString = "int";
            return null;

          } else {
            //No property was found in the child class with the type the parent class is looking for
            var second = isSecondKey ? "second " : "";
            throw new GeneratorException(parentCI, parentMI,
                $"{parentMI.MemberName} references the class {parentMI.ChildTypeName}. A corresponding property with " +
                $"type {childKeyPropertyType} used as {second}key into {parentMI.MemberName} is missing in {parentMI.ChildTypeName}.");
          }

      } else {
        //parent knows the child's property name already
        if (parentMI.ChildClassInfo!.Members.TryGetValue(childKeyPropertyName, out childKeyMI)) {
          if (childKeyPropertyType!=childKeyMI.CsvTypeString && childKeyPropertyType==childKeyMI.TypeStringNotNullable) {
            System.Diagnostics.Debugger.Break();//Todo: Remove this test if Break is ever reached, otherwise simplify if()
          }
          if (childKeyPropertyType!=childKeyMI.CsvTypeString && childKeyPropertyType!=childKeyMI.TypeStringNotNullable) {
            //property in child class with the searched name has wrong type
            throw new GeneratorException(parentCI, parentMI,
              $"Property {childKeyPropertyName} in child class {parentMI.ChildClassInfo.ClassName} should " +
              $"have the type {childKeyPropertyType}, but its type is {childKeyMI.TypeString}:" + Environment.NewLine +
              $"Class: {childKeyMI.ClassInfo.ClassName}" + Environment.NewLine +
              $"Property: {childKeyMI.MemberText}");
          }

        } else {
          if (isSecondKey && childKeyPropertyName=="Key") {
            //use the Key property as second key for SortedBucketCollection 
            parentMI.ChildKey2PropertyName = "Key";
            parentMI.LowerChildKey2PropertyName = "key";
            parentMI.ChildKey2TypeString = "int";
            return null;

          } else {
            //cannot find property in child with the name==ChildPropertyName
            throw new GeneratorException(parentCI, parentMI,
              $"Can not find property {childKeyPropertyName} in child class {parentMI.ChildTypeName}.");
          }
        }
      }

      //setup link between parent collection and key in child
      if (isSecondKey) {
        parentMI.ChildKey2PropertyName = childKeyMI.MemberName;
        parentMI.LowerChildKey2PropertyName = childKeyMI.LowerMemberName;
        parentMI.ChildKey2TypeString = childKeyMI.TypeStringNotNullable;
      } else {
        parentMI.ChildKeyPropertyName = childKeyMI.MemberName;
        parentMI.LowerChildKeyPropertyName = childKeyMI.LowerMemberName;
        parentMI.ChildKeyTypeString = childKeyMI.TypeStringNotNullable;
      }
      return childKeyMI;
    }


    //private static void findParentRelatedPropertiesInChild(
    //  ClassInfo ci,
    //  MemberInfo mi,
    //  string childKeyExpectedTypeString,
    //  string StoragePropertyKey,
    //  ref string? childPropertyName,
    //  ref string? lowerChildPropertyName,
    //  out string childKeyFoundTypeString) 
    //{
    //  childKeyFoundTypeString = null!;
    //  var childMI = mi.ChildMemberInfo!;
    //  if (childPropertyName is null) {
    //    //parent does not define name of key property
    //    foreach (var childKeyMI in mi.ChildClassInfo!.Members.Values) {
    //      if (childKeyExpectedTypeString==childKeyMI.CsvTypeString || childKeyExpectedTypeString==childKeyMI.TypeString) {
    //        if (childKeyFoundTypeString is null) {
    //          //first property found with the expected key type
    //          childKeyFoundTypeString = childKeyMI.TypeString;
    //          childPropertyName = childKeyMI.MemberName;
    //          lowerChildPropertyName = childKeyMI.MemberName.ToCamelCase();
    //        } else {
    //          //second property found with the expected key type, throw exception
    //          throw new GeneratorException($"{ci}.{mi.MemberName} {mi.TypeString}: found " +
    //            $"{childMI.ClassInfo.ClassName}.{childMI.MemberName}, but found two properties in {childMI.ClassInfo.ClassName} " +
    //            $"with the type {childKeyExpectedTypeString}: {childPropertyName}, {childKeyMI.MemberName}. Use " +
    //            $"[StorageProperty({StoragePropertyKey}: \"Xyz\")] in the parent to indicate which property should be used." +
    //            Environment.NewLine + mi.MemberText);
    //        }
    //      }
    //    }

    //    if (childKeyFoundTypeString is null) {
    //      if (childKeyExpectedTypeString=="int" && StoragePropertyKey=="ChildKey2PropertyName") {
    //        //use the Key property as second key for SortedBucketCollection 
    //        childKeyFoundTypeString = "int";
    //        childPropertyName = "Key";
    //        lowerChildPropertyName = "key";

    //      } else {
    //        throw new GeneratorException($"{ci}.{mi.MemberName} {mi.TypeString}: found " +
    //          $"{childMI.ClassInfo.ClassName}.{childMI.MemberName}, but could not find another property in {childMI.ClassInfo.ClassName} " +
    //          $"with the type {childKeyExpectedTypeString} needed as key." + Environment.NewLine + mi.MemberText);
    //      }
    //    }

    //  } else {
    //    //parent defines name of key property
    //    if (mi.ChildClassInfo!.Members.TryGetValue(childPropertyName, out var childKeyMIFound)) {
    //      if (childKeyExpectedTypeString==childKeyMIFound.CsvTypeString || childKeyExpectedTypeString==childKeyMIFound.TypeString) {
    //        childKeyFoundTypeString = childKeyMIFound.TypeString;
    //        childPropertyName = childKeyMIFound.MemberName;
    //        lowerChildPropertyName = childKeyMIFound.MemberName.ToCamelCase();
    //      } else {
    //        throw new GeneratorException($"{ci}.{mi.MemberName} {mi.TypeString}: found " +
    //          $"{childKeyMIFound.ClassInfo.ClassName}.{childKeyMIFound.MemberName}, but it has wrong type: " +
    //          $"{childKeyMIFound.CsvTypeString}:" + Environment.NewLine + mi.MemberText);
    //      }
    //    } else {
    //      if (childPropertyName=="Key") {
    //        childKeyFoundTypeString = "int";
    //        childPropertyName = "Key";
    //        lowerChildPropertyName = "key";
    //      } else {
    //        throw new GeneratorException($"{ci}.{mi.MemberName} {mi.TypeString}: found class {mi.ChildClassInfo.ClassName} " +
    //          $"but could not find property {childPropertyName} in that class." + Environment.NewLine + mi.MemberText);
    //      }
    //    }
    //  }
    //}


    private void addToParentChildTree(ClassInfo classInfo) {
      if (!classInfo.IsAddedToParentChildTree && allParentsAreAddedToParentChildTree(classInfo)) {
        classInfo.IsAddedToParentChildTree = true;
        classInfo.StoreKey = parentChildTree.Count;
        parentChildTree.Add(classInfo);
        foreach (var child in classInfo.Children) {
          addToParentChildTree(child);
        }
      }
    }


    private static bool allParentsAreAddedToParentChildTree(ClassInfo childClass) {
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
        if (classInfo.AreInstancesReleasable) {
          sw.WriteLine($"          {classInfo.ClassName}.Disconnect,");
        } else {
          sw.WriteLine($"          null,");
        }
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
