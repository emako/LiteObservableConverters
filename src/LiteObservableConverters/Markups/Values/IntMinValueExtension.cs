using System;
using System.Windows.Markup;

namespace LiteObservableConverters;

[MarkupExtensionReturnType(typeof(int))]
public sealed class IntMinValueExtension : MarkupExtension
{
    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        return int.MinValue;
    }
}
