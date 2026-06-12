using System;
using System.Windows.Markup;

namespace LiteObservableConverters;

[MarkupExtensionReturnType(typeof(double))]
public sealed class PIExtension() : MarkupExtension
{
    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        return Math.PI;
    }
}
