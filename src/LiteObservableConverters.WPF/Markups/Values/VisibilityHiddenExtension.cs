using System;
using System.Windows;
using System.Windows.Markup;

namespace LiteObservableConverters;

[MarkupExtensionReturnType(typeof(Visibility))]
public sealed class VisibilityHiddenExtension() : MarkupExtension
{
    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        return Visibility.Hidden;
    }
}
