using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageLib;
using System;
#pragma warning disable IDE0059 // Unnecessary assignment of a value

namespace ParserTest {
  public static class Generator {
    public static Compiler? Analyze(string source, string? exceptionMessage = null) {
      source = "using StorageLib;" + Environment.NewLine +
        "namespace TestContext {" + Environment.NewLine +
        source + "}" + Environment.NewLine;
      try {
        var tree = CSharpSyntaxTree.ParseText(source);
        var root = tree.GetRoot() as CompilationUnitSyntax;
        var namespaceDeclaration = root!.Members[0] as NamespaceDeclarationSyntax;
        var compiler = new Compiler(TracingEnum.debugOnlyTracing, true);
        compiler.Parse(namespaceDeclaration!, "GeneratorTest");
        compiler.AnalyzeDependencies();
        Assert.IsNull(exceptionMessage);
        return compiler;

      } catch (Exception ex) {
        Assert.IsFalse(ex is AssertFailedException, "There might be an exceptionMessage, but no exception was thrown.");
        
        exceptionMessage = exceptionMessage?.Replace(Environment.NewLine + "        ", Environment.NewLine);
        var bothMessages = exceptionMessage + Environment.NewLine + Environment.NewLine + ex.Message;
        Assert.AreEqual((string?)exceptionMessage, ex.Message);
      }

      return null;

    }

  }
}
