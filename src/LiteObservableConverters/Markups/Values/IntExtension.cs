using System;
using System.Windows.Markup;

namespace LiteObservableConverters;

[MarkupExtensionReturnType(typeof(int))]
public sealed class IntExtension(int value) : MarkupExtension
{
    [ConstructorArgument(nameof(Value))]
    public int Value { get; set; } = value;

    public IntExtension() : this(default)
    {
    }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        return Value;
    }
}
