﻿namespace WpfAnalyzers.DependencyProperties
{
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    internal static class FieldDeclarationSyntaxExt
    {
        internal static string Name(this FieldDeclarationSyntax declaration)
        {
            var variables = declaration?.Declaration?.Variables;
            if (variables?.Count != 1)
            {
                return null;
            }

            return variables.Value[0].Identifier.ValueText;
        }
    }
}