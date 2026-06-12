using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Markup;

namespace LiteObservableConverters;

[MarkupExtensionReturnType(typeof(object))]
[DefaultValue(null)]
public sealed class SetServiceLocatorExtension : MarkupExtension
{
    /// <summary>
    /// Use `object` to avoid defining `TypeConverter`
    /// /// </summary>
    [ConstructorArgument(nameof(Value))]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<Pending>")]
    public object? Value
    {
        get => ComputedServiceProvider.Shared;
        set => ComputedServiceProvider.Shared = value as IServiceProvider;
    }

    public SetServiceLocatorExtension() : this(null)
    {
    }

    public SetServiceLocatorExtension(object? value)
    {
        Value = value;
    }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        return null;
    }
}
