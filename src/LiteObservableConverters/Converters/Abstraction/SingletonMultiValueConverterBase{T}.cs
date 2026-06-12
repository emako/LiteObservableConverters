using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LiteObservableConverters;

public abstract class SingletonMultiValueConverterBase<TSelf> : DependencyObject, IMultiValueConverter where TSelf : SingletonValueConverterBase<TSelf>, new()
{
    private static TSelf? _instance = null;

    public static TSelf Instance => _instance ??= new();

    public abstract object? Convert(object?[]? values, Type targetType, object? parameter, CultureInfo culture);

    public abstract object?[]? ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo culture);
}
