using System;
using System.Windows.Markup;

namespace LiteObservableConverters;

[MarkupExtensionReturnType(typeof(double))]
public sealed class DoubleExtension(double value) : MarkupExtension
{
    [ConstructorArgument(nameof(Value))]
    public double Value { get; set; } = value;

    public DoubleExtension() : this(default)
    {
    }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        return Value;
    }
}
