﻿using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace SampleAnalyzer;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(NewlineAnalyzerCodeFixProvider))]
[Shared]
public class NewlineAnalyzerCodeFixProvider : CodeFixProvider
{
    public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(NewlineAnalyzer.DiagnosticId);

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
        var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<LiteralExpressionSyntax>().First();

        // Register a code action that will invoke the fix.
        context.RegisterCodeFix(
            CodeAction.Create(
                CodeFixResources.NewlineCodeFixTitle,
                c => ReplaceNewlineAsync(context.Document, declaration, c),
                nameof(CodeFixResources.NewlineCodeFixTitle)),
            diagnostic);
    }

    private async Task<Document> ReplaceNewlineAsync(Document document, LiteralExpressionSyntax expressionSyntax, CancellationToken cancellationToken)
    {
        var identifierToken = expressionSyntax.Token;
        var updatedText = identifierToken.Text.Replace(@"\n", "\" + Str.NewLine + \"");
        var updatedValue = identifierToken.ValueText.Replace("\n", "\" + Str.NewLine + \"");
        var newToken = SyntaxFactory.Literal(identifierToken.LeadingTrivia, updatedText, updatedValue, identifierToken.TrailingTrivia);

        var sourceText = await expressionSyntax.SyntaxTree.GetTextAsync(cancellationToken);
        return document.WithText(sourceText.WithChanges(new TextChange(identifierToken.FullSpan, newToken.ToFullString())));
    }
}