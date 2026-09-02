using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;

namespace LiteObservableConverters;

[MarkupExtensionReturnType(typeof(object))]
public sealed class DebugExtension : MarkupExtension
{
    public bool IsInverted { get; set; }

    public FalseToVisibility FalseToVisibility { get; set; } = FalseToVisibility.Collapsed;

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        var isAttached = Debugger.IsAttached ^ IsInverted;
        var targetType = GetTargetPropertyType(serviceProvider);

        if (targetType == typeof(Visibility) || targetType == typeof(Visibility?))
        {
            if (isAttached)
            {
                return Visibility.Visible;
            }

            return FalseToVisibility == FalseToVisibility.Collapsed
                ? Visibility.Collapsed
                : Visibility.Hidden;
        }

        return isAttached;
    }

    private static Type? GetTargetPropertyType(IServiceProvider serviceProvider)
    {
        if (serviceProvider.GetService(typeof(IProvideValueTarget)) is not IProvideValueTarget provideValueTarget)
        {
            return null;
        }

        return provideValueTarget.TargetProperty switch
        {
            DependencyProperty dp => dp.PropertyType,
            PropertyInfo prop => prop.PropertyType,
            _ => null,
        };
    }
}

/// <summary>
/// Enum for BoolToVisibility converter: in which property convert "false" value?
/// </summary>
public enum FalseToVisibility
{
    Hidden = Visibility.Hidden,
    Collapsed = Visibility.Collapsed,
}
