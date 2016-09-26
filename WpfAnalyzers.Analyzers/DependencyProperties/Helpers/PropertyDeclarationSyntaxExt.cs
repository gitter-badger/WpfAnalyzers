namespace WpfAnalyzers.DependencyProperties
{
    using System.Linq;
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

        internal static bool TryGetDependencyPropertyFromGetter(
            this PropertyDeclarationSyntax propertyDeclaration,
            out IdentifierNameSyntax dependencyProperty)
        {
            var getter = propertyDeclaration.GetAccessorDeclaration();
            foreach (var invocation in getter.DescendantNodes().OfType<InvocationExpressionSyntax>())
            {
                if ((invocation?.Expression as IdentifierNameSyntax)?.Identifier.Text == "GetValue")
                {
                    var arguments = invocation.ArgumentList?.Arguments;
                    if (arguments?.Count == 1)
                    {
                        dependencyProperty = arguments.Value[0].Expression as IdentifierNameSyntax;
                        return dependencyProperty != null;
                    }
                }
            }

            dependencyProperty = null;
            return false;
        }
    }
}