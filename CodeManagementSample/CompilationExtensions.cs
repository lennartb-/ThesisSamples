using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Diagnostics;
using System.Linq;

namespace CodeManagementSample;

public static class CompilationExtensions
{
    /// <summary>
    /// From https://github.com/dotnet/roslyn/blob/main/src/Compilers/CSharp/Portable/Compilation/CSharpCompilation.cs
    /// </summary>
    /// <param name="compilation"></param>
    /// <returns></returns>
    public static bool HasSubmissionResult(this Compilation compilation)
    {
        // A submission may be empty or comprised of a single script file.
        var tree = compilation.SyntaxTrees.LastOrDefault();
        if (tree == null)
        {
            return false;
        }

        var root = tree.GetCompilationUnitRoot();

        // Are there any top-level return statements?
        if (root.DescendantNodes(n => n is GlobalStatementSyntax or StatementSyntax or CompilationUnitSyntax).Any(n => n.IsKind(SyntaxKind.ReturnStatement)))
        {
            return true;
        }

        // Is there a trailing expression?
        var lastGlobalStatement = (GlobalStatementSyntax?)root.Members.LastOrDefault(m => m.IsKind(SyntaxKind.GlobalStatement));
        if (lastGlobalStatement != null)
        {
            var statement = lastGlobalStatement.Statement;
            if (statement.IsKind(SyntaxKind.ExpressionStatement))
            {
                var expressionStatement = (ExpressionStatementSyntax)statement;
                if (expressionStatement.SemicolonToken.IsMissing)
                {
                    var model = compilation.GetSemanticModel(tree);
                    var expression = expressionStatement.Expression;
                    var info = model.GetTypeInfo(expression);
                    return info.ConvertedType?.SpecialType != SpecialType.System_Void;
                }
            }
        }

        return false;
    }
}
