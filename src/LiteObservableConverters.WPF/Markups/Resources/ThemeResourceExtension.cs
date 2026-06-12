using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace LiteObservableConverters;

[MarkupExtensionReturnType(typeof(object))]
[TypeConverter(typeof(ThemeResouceExtensionConverter))]
public sealed class ThemeResourceExtension(object? resourceKey) : StaticResourceExtension(resourceKey)
{
    public ThemeResourceExtension() : this(null!)
    {
    }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        if (ResourceKey is string key && key.StartsWith("SystemColor", StringComparison.Ordinal))
        {
            var binding = new Binding(key) { Source = SystemColorsSource.Current };
            return binding.ProvideValue(serviceProvider);
        }

        return base.ProvideValue(serviceProvider);
    }

    private class SystemColorsSource : INotifyPropertyChanged
    {
        private SystemColorsSource()
        {
            SystemParameters.StaticPropertyChanged += OnSystemParametersPropertyChanged;
        }

        public static SystemColorsSource Current { get; } = new SystemColorsSource();

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA1822 // Mark members as static

        public Color SystemColorButtonFaceColor => SystemColors.ControlColor;
        public Color SystemColorButtonTextColor => SystemColors.ControlTextColor;
        public Color SystemColorGrayTextColor => SystemColors.GrayTextColor;
        public Color SystemColorHighlightColor => SystemColors.HighlightColor;
        public Color SystemColorHighlightTextColor => SystemColors.HighlightTextColor;
        public Color SystemColorHotlightColor => SystemColors.HotTrackColor;
        public Color SystemColorWindowColor => SystemColors.WindowColor;
        public Color SystemColorWindowTextColor => SystemColors.WindowTextColor;
        public Color SystemColorActiveCaptionColor => SystemColors.ActiveCaptionColor;
        public Color SystemColorInactiveCaptionTextColor => SystemColors.InactiveCaptionTextColor;

#pragma warning restore CA1822 // Mark members as static
#pragma warning restore IDE0079 // Remove unnecessary suppression

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnSystemParametersPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SystemParameters.HighContrast) && SystemParameters.HighContrast)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null!));
            }
        }
    }

    public class ThemeResouceExtensionConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type? destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor))
            {
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA1510 // Use ArgumentNullException throw helper
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
#pragma warning restore CA1510 // Use ArgumentNullException throw helper
#pragma warning restore IDE0079 // Remove unnecessary suppression

                if (value is not ThemeResourceExtension dynamicResource)
                {
                    throw new ArgumentException($"{value} must be of type {nameof(ThemeResourceExtension)}", nameof(value));
                }

                return new InstanceDescriptor(typeof(ThemeResourceExtension).GetConstructor([typeof(object)]), new object[] { dynamicResource.ResourceKey! });
            }
            return base.ConvertTo(context, culture, value, destinationType!);
        }
    }
}
