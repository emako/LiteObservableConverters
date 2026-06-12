using System;
using System.Windows.Markup;

namespace LiteObservableConverters;

[MarkupExtensionReturnType(typeof(Type))]
public sealed class TypeofIntExtension() : MarkupExtension
{
    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        return typeof(int);
    }
}
