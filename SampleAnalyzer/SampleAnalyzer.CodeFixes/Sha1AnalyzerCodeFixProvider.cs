using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SampleAnalyzer;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(Sha1AnalyzerCodeFixProvider))]
[Shared]
public class Sha1AnalyzerCodeFixProvider : CodeFixProvider
{
    public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(Sha1Analyzer.DiagnosticId);

    public sealed override FixAllProvider GetFixAllProvider()
    {
        // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
        return WellKnownFixAllProviders.BatchFixer;
    }

    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

        var diagnostic = context.Diagnostics.First();
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        // Find the type declaration identified by the diagnostic.
        var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<InvocationExpressionSyntax>().First();

        // Register a code action that will invoke the fix.
        context.RegisterCodeFix(
            CodeAction.Create(
                CodeFixResources.Sha1CodeFixTitle,
                c => ReplaceNewlineAsync(context.Document, declaration, c),
                nameof(CodeFixResources.Sha1CodeFixTitle)),
            diagnostic);
    }

    private async Task<Document> ReplaceNewlineAsync(Document document, InvocationExpressionSyntax invocationExpr, CancellationToken cancellationToken)
    {
        var invokedExpression = invocationExpr.Expression as MemberAccessExpressionSyntax;

        var invokedObjectName = invokedExpression.Expression;
        var newInvokedMethod = SyntaxFactory.IdentifierName("HashAsSha512");
        var updatedExpression = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, invokedObjectName, newInvokedMethod);

        updatedExpression = updatedExpression.WithLeadingTrivia(invokedExpression.GetLeadingTrivia());
        updatedExpression = updatedExpression.WithTrailingTrivia(invokedExpression.GetTrailingTrivia());

        return document.WithSyntaxRoot((await document.GetSyntaxRootAsync(cancellationToken)).ReplaceNode(invokedExpression, updatedExpression));
    }
}

