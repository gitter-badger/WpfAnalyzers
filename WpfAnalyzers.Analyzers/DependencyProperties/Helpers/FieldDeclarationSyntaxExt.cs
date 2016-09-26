namespace WpfAnalyzers.DependencyProperties
{
    using System;
    using System.Linq;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    internal static class FieldDeclarationSyntaxExt
    {
        internal static string DependencyPropertyRegisteredName(this FieldDeclarationSyntax declaration)
        {
            MemberAccessExpressionSyntax invocation;
            if (!TryGetRegisterInvocation(declaration, out invocation))
            {
                return null;
            }

            var args = (invocation.Parent as InvocationExpressionSyntax)?.ArgumentList;
            var nameArg = args?.Arguments.FirstOrDefault();
            if (nameArg == null)
            {
                return null;
            }

            string result;
            if (TryGetStringLiteral(nameArg.Expression, out result))
            {
                return result;
            }

            if ((nameArg.Expression as InvocationExpressionSyntax)?.TryGetNameOfResult(out result) == true)
            {
                return result;
            }

            return null;
        }

        internal static TypeSyntax DependencyPropertyRegisteredType(this FieldDeclarationSyntax declaration)
        {
            MemberAccessExpressionSyntax invocation;
            if (!TryGetRegisterInvocation(declaration, out invocation))
            {
                return null;
            }

            var args = (invocation.Parent as InvocationExpressionSyntax)?.ArgumentList;
            if (args == null || args.Arguments.Count < 2)
            {
                return null;
            }

            var typeArg = args.Arguments[1];
            if (typeArg == null)
            {
                return null;
            }

            TypeSyntax result;
            if (typeArg.Expression.TryGetTypeOfResult(out result))
            {
                return result;
            }

            return null;
        }

        //internal static string DependencyPropertyRegisteredDefaultValue(this FieldDeclarationSyntax declaration, SemanticModel semanticModel)
        //{
        //    MemberAccessExpressionSyntax invocation;
        //    if (!TryGetRegisterInvocation(declaration, out invocation))
        //    {
        //        return null;
        //    }

        //    var metaDataCreation = (invocation.Parent as InvocationExpressionSyntax)
        //        ?.ArgumentList
        //        ?.Arguments
        //        .FirstOrDefault(a => IsCreateMetaDataExpression(a, semanticModel))
        //        ?.Expression as ObjectCreationExpressionSyntax;

        //    if (metaDataCreation == null)
        //    {
        //        return null;
        //    }

        //    throw new NotImplementedException();
        //}

        private static bool IsCreateMetaDataExpression(ArgumentSyntax argument, SemanticModel semanticModel)
        {
            var creation = argument?.Expression as ObjectCreationExpressionSyntax;
            if (creation == null)
            {
                return false;
            }

            var typeInfo = semanticModel.GetTypeInfo(creation);
            var conversion = semanticModel.ClassifyConversion(creation.Type, typeInfo.Type);
            return conversion.Exists;
        }

        private static bool TryGetRegisterInvocation(FieldDeclarationSyntax declaration, out MemberAccessExpressionSyntax invocation)
        {
            invocation = (declaration.Declaration
                    .Variables
                    .FirstOrDefault()
                    .Initializer.Value as InvocationExpressionSyntax)
                ?.Expression as MemberAccessExpressionSyntax;
            return invocation?.IsDependencyPropertyRegister() == true;
        }

        private static bool TryGetStringLiteral(ExpressionSyntax expression, out string result)
        {
            var literal = expression as LiteralExpressionSyntax;
            if (literal == null || literal.Kind() != SyntaxKind.StringLiteralExpression)
            {
                result = null;
                return false;
            }

            result = literal.Token.ValueText;
            return true;
        }
    }
}