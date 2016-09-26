namespace WpfAnalyzers.DependencyProperties
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    internal static class PropertyDeclarationSyntaxExt
    {
        internal static AccessorDeclarationSyntax GetAccessorDeclaration(this PropertyDeclarationSyntax property)
        {
            var accessors = property?.AccessorList?.Accessors;
            if (accessors == null)
            {
                return null;
            }

            foreach (var accessor in accessors)
            {
                if (accessor.IsKind(SyntaxKind.GetAccessorDeclaration))
                {
                    return accessor;
                }
            }

            return null;
        }

        internal static AccessorDeclarationSyntax SetAccessorDeclaration(this PropertyDeclarationSyntax property)
        {
            var accessors = property?.AccessorList?.Accessors;
            if (accessors == null)
            {
                return null;
            }

            foreach (var accessor in accessors)
            {
                if (accessor.IsKind(SyntaxKind.SetAccessorDeclaration))
                {
                    return accessor;
                }
            }

            return null;
        }
    }
}