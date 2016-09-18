namespace WpfAnalyzers.Test.Sandbox
{
    using System.Windows;
    using System.Windows.Controls;

    public class FooControl : Control
    {
        public static readonly DependencyProperty BarProperty = DependencyProperty.Register(
            "Error", typeof(int), typeof(FooControl), new PropertyMetadata(default(int)));

        public int Bar
        {
            get { return (int) GetValue(BarProperty); }
            set { SetValue(BarProperty, value); }
        }
    }
}
