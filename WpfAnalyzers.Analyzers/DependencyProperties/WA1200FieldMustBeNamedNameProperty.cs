namespace WpfAnalyzers.DependencyProperties
{
    using System;
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;

    /// <summary>
    /// DependencyProperty field must be named &lt;Name&gt;Property
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal class WA1200FieldMustBeNamedNameProperty : DiagnosticAnalyzer
    {
        /// <summary>
        /// The ID for diagnostics produced by the <see cref="WA1200FieldMustBeNamedNameProperty"/> analyzer.
        /// </summary>
        public const string DiagnosticId = "WA1200";
        private const string Title = "DependencyProperty field must be named <Name>Property";
        private const string MessageFormat = "DependencyProperty '{0}' field must be named <Name>Property";
        private const string Description = "DependencyProperty field must be named <Name>Property";
        private const string HelpLink = "http://stackoverflow.com/";

        private static readonly DiagnosticDescriptor Descriptor = new DiagnosticDescriptor(
            DiagnosticId,
            Title,
            MessageFormat,
            AnalyzerCategory.DependencyProperties,
            DiagnosticSeverity.Warning,
            AnalyzerConstants.EnabledByDefault,
            Description,
            HelpLink);

        /// <inheritdoc/>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(Descriptor);

        /// <inheritdoc/>
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(HandleFieldDeclaration, SyntaxKind.FieldDeclaration);
        }

        private static void HandleFieldDeclaration(SyntaxNodeAnalysisContext context)
        {
            var fieldSymbol = (IFieldSymbol)context.ContainingSymbol;
            if (fieldSymbol.Type.Name != "DependencyProperty")
            {
                return;
            }

            //if (contextContainingSymbol)
            //{
            //    return;
            //}
        }

        private static void CheckElementNameToken(SyntaxNodeAnalysisContext context, VariableDeclarationSyntax variable)
        {


            throw new NotImplementedException();
            //var symbolInfo = context.SemanticModel.GetDeclaredSymbol(variable.Parent);
            //if (symbolInfo != null)
            //{
            //    return;
            //}

            //context.ReportDiagnostic(Diagnostic.Create(Descriptor, variable.GetLocation(), variable.ValueText));
        }
    }
}
