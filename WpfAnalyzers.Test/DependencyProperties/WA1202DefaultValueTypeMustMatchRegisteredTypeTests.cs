namespace WpfAnalyzers.Test.DependencyProperties
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.Diagnostics;

    using NUnit.Framework;

    using WpfAnalyzers.DependencyProperties;

    public class WA1202DefaultValueTypeMustMatchRegisteredTypeTests : CodeFixVerifier
    {
        [TestCase("default(int)")]
        [TestCase("1")]
        public async Task HappyPath(string defaultValue)
        {
            var testCode = @"
using System.Windows;
using System.Windows.Controls;

public class FooControl : Control
{
    public static readonly DependencyProperty BarProperty = DependencyProperty.Register(
        nameof(Bar),
        typeof(int),
        typeof(FooControl),
        new PropertyMetadata(default(int)));

    public int Bar
    {
        get { return (int)this.GetValue(BarProperty); }
        set { this.SetValue(BarProperty, value); }
    }
}";
            testCode = testCode.Replace("new PropertyMetadata(default(int)))", $"new PropertyMetadata({defaultValue}))");
            await this.VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None)
                      .ConfigureAwait(false);
        }

        [Test]
        public async Task HappyPathWhenNoMetaData()
        {
            var testCode = @"
using System.Windows;
using System.Windows.Controls;

public class FooControl : Control
{
    public static readonly DependencyProperty BarProperty = DependencyProperty.Register(
        nameof(Bar),
        typeof(int),
        typeof(FooControl));

    public int Bar
    {
        get { return (int)this.GetValue(BarProperty); }
        set { this.SetValue(BarProperty, value); }
    }
}";
            await this.VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None)
                      .ConfigureAwait(false);
        }

        [TestCase("1.2")]
        [TestCase("null")]
        [TestCase("default(double)")]
        public async Task WhenNotMatching(string defaultValue)
        {
            var testCode = @"
using System.Windows;
using System.Windows.Controls;

public class FooControl : Control
{
    public static readonly DependencyProperty BarProperty = DependencyProperty.Register(
        nameof(Bar),
        typeof(int),
        typeof(FooControl),
        new PropertyMetadata(default(int)));

    public int Bar
    {
        get { return (int)this.GetValue(BarProperty); }
        set { this.SetValue(BarProperty, value); }
    }
}";
            testCode = testCode.Replace("new PropertyMetadata(default(int)))", $"new PropertyMetadata({defaultValue}))");
            var expected = this.CSharpDiagnostic().WithLocation(7, 9).WithArguments("BarProperty");
            await this.VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None)
                      .ConfigureAwait(false);
        }

        protected override IEnumerable<DiagnosticAnalyzer> GetCSharpDiagnosticAnalyzers()
        {
            yield return new WA1202DefaultValueTypeMustMatchRegisteredType();
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return null;
        }
    }
}