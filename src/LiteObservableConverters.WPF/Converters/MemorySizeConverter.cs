using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;

namespace LiteObservableConverters;

/// <summary>
/// Converts a numeric memory / file size into a human-readable string (and optionally back).
/// </summary>
/// <remarks>
/// <para>
/// Canonical storage unit is bytes. Set <see cref="InputUnit"/> when the bound value is already
/// expressed in KB/MB/... (common mistake in ad-hoc converters).
/// </para>
/// <para>
/// Defaults follow the Humanizer / Windows Explorer convention: binary scale (1024) with short
/// symbols (<c>KB</c>/<c>MB</c>/...). Use <see cref="SymbolStyle"/> = <see cref="MemorySizeSymbolStyle.Iec"/>
/// for <c>KiB</c>/<c>MiB</c>/..., or <see cref="UnitSystem"/> = <see cref="MemorySizeUnitSystem.Decimal"/>
/// for SI (1000) prefixes.
/// </para>
/// <para>
/// XAML examples:
/// <code language="xml">
/// <!-- FileInfo.Length (bytes) → "1.5 GB" -->
/// Text="{Binding Length, Converter={x:Static loc:MemorySizeConverter.Instance}}"
///
/// <!-- Value already in MB, force binary IEC symbols -->
/// &lt;loc:MemorySizeConverter InputUnit="Megabyte" SymbolStyle="Iec" Format="0.00" /&gt;
/// </code>
/// </para>
/// </remarks>
[ValueConversion(typeof(long), typeof(string))]
[ValueConversion(typeof(ulong), typeof(string))]
[ValueConversion(typeof(int), typeof(string))]
[ValueConversion(typeof(double), typeof(string))]
[ValueConversion(typeof(float), typeof(string))]
[ValueConversion(typeof(decimal), typeof(string))]
public sealed class MemorySizeConverter : SingletonValueConverterBase<MemorySizeConverter>
{
    private const string DefaultFormat = "0.##";

    public static readonly DependencyProperty InputUnitProperty =
        DependencyProperty.Register(
            nameof(InputUnit),
            typeof(MemorySizeUnit),
            typeof(MemorySizeConverter),
            new PropertyMetadata(MemorySizeUnit.Byte));

    public static readonly DependencyProperty OutputUnitProperty =
        DependencyProperty.Register(
            nameof(OutputUnit),
            typeof(MemorySizeUnit),
            typeof(MemorySizeConverter),
            new PropertyMetadata(MemorySizeUnit.Auto));

    public static readonly DependencyProperty UnitSystemProperty =
        DependencyProperty.Register(
            nameof(UnitSystem),
            typeof(MemorySizeUnitSystem),
            typeof(MemorySizeConverter),
            new PropertyMetadata(MemorySizeUnitSystem.Binary));

    public static readonly DependencyProperty SymbolStyleProperty =
        DependencyProperty.Register(
            nameof(SymbolStyle),
            typeof(MemorySizeSymbolStyle),
            typeof(MemorySizeConverter),
            new PropertyMetadata(MemorySizeSymbolStyle.Short));

    public static readonly DependencyProperty FormatProperty =
        DependencyProperty.Register(
            nameof(Format),
            typeof(string),
            typeof(MemorySizeConverter),
            new PropertyMetadata(DefaultFormat));

    public static readonly DependencyProperty MinUnitProperty =
        DependencyProperty.Register(
            nameof(MinUnit),
            typeof(MemorySizeUnit),
            typeof(MemorySizeConverter),
            new PropertyMetadata(MemorySizeUnit.Byte));

    public static readonly DependencyProperty MaxUnitProperty =
        DependencyProperty.Register(
            nameof(MaxUnit),
            typeof(MemorySizeUnit),
            typeof(MemorySizeConverter),
            new PropertyMetadata(MemorySizeUnit.Petabyte));

    public static readonly DependencyProperty NullValueProperty =
        DependencyProperty.Register(
            nameof(NullValue),
            typeof(object),
            typeof(MemorySizeConverter),
            new PropertyMetadata(null));

    /// <summary>
    /// Unit of the bound numeric value. Default is <see cref="MemorySizeUnit.Byte"/>.
    /// </summary>
    public MemorySizeUnit InputUnit
    {
        get => (MemorySizeUnit)GetValue(InputUnitProperty);
        set => SetValue(InputUnitProperty, value);
    }

    /// <summary>
    /// Fixed output unit, or <see cref="MemorySizeUnit.Auto"/> to pick the largest whole unit.
    /// </summary>
    public MemorySizeUnit OutputUnit
    {
        get => (MemorySizeUnit)GetValue(OutputUnitProperty);
        set => SetValue(OutputUnitProperty, value);
    }

    /// <summary>
    /// Binary (1024) or decimal (1000) scaling.
    /// </summary>
    public MemorySizeUnitSystem UnitSystem
    {
        get => (MemorySizeUnitSystem)GetValue(UnitSystemProperty);
        set => SetValue(UnitSystemProperty, value);
    }

    /// <summary>
    /// Short (<c>MB</c>) or IEC (<c>MiB</c>) symbols.
    /// </summary>
    public MemorySizeSymbolStyle SymbolStyle
    {
        get => (MemorySizeSymbolStyle)GetValue(SymbolStyleProperty);
        set => SetValue(SymbolStyleProperty, value);
    }

    /// <summary>
    /// Numeric format string passed to <see cref="double.ToString(string, IFormatProvider)"/>.
    /// Default is <c>0.##</c> (same default as Humanizer / ByteSize).
    /// </summary>
    public string Format
    {
        get => (string)GetValue(FormatProperty);
        set => SetValue(FormatProperty, value);
    }

    /// <summary>
    /// Lower bound for automatic unit selection.
    /// </summary>
    public MemorySizeUnit MinUnit
    {
        get => (MemorySizeUnit)GetValue(MinUnitProperty);
        set => SetValue(MinUnitProperty, value);
    }

    /// <summary>
    /// Upper bound for automatic unit selection.
    /// </summary>
    public MemorySizeUnit MaxUnit
    {
        get => (MemorySizeUnit)GetValue(MaxUnitProperty);
        set => SetValue(MaxUnitProperty, value);
    }

    /// <summary>
    /// Value returned when the binding source is <c>null</c>. Defaults to <c>null</c>.
    /// </summary>
    public object? NullValue
    {
        get => GetValue(NullValueProperty);
        set => SetValue(NullValueProperty, value);
    }

    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
        {
            return NullValue;
        }

        if (!TryGetDouble(value, culture, out double magnitude))
        {
            return DependencyProperty.UnsetValue;
        }

        if (double.IsNaN(magnitude) || double.IsInfinity(magnitude))
        {
            return DependencyProperty.UnsetValue;
        }

        string format = ResolveFormat(parameter);
        double bytes = MemorySize.ToBytes(magnitude, InputUnit, UnitSystem);

        return MemorySize.Format(
            bytes,
            format,
            culture,
            OutputUnit,
            UnitSystem,
            SymbolStyle,
            MinUnit,
            MaxUnit);
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
        {
            return null;
        }

        if (value is string text)
        {
            if (!MemorySize.TryParse(text, out double bytes, UnitSystem, culture))
            {
                return DependencyProperty.UnsetValue;
            }

            double magnitude = MemorySize.FromBytes(bytes, NormalizeInputUnit(InputUnit), UnitSystem);
            return CoerceToTargetType(magnitude, targetType, culture);
        }

        if (TryGetDouble(value, culture, out double numeric))
        {
            // Already a number — interpret as bytes and project into InputUnit / targetType.
            double magnitude = MemorySize.FromBytes(numeric, NormalizeInputUnit(InputUnit), UnitSystem);
            return CoerceToTargetType(magnitude, targetType, culture);
        }

        return DependencyProperty.UnsetValue;
    }

    private string ResolveFormat(object? parameter)
    {
        if (parameter is string parameterFormat && !string.IsNullOrWhiteSpace(parameterFormat))
        {
            return parameterFormat;
        }

        return string.IsNullOrWhiteSpace(Format) ? DefaultFormat : Format;
    }

    private static MemorySizeUnit NormalizeInputUnit(MemorySizeUnit unit)
        => unit == MemorySizeUnit.Auto ? MemorySizeUnit.Byte : unit;

    private static bool TryGetDouble(object value, CultureInfo culture, out double result)
    {
        switch (value)
        {
            case double d:
                result = d;
                return true;

            case float f:
                result = f;
                return true;

            case decimal m:
                result = (double)m;
                return true;

            case long l:
                result = l;
                return true;

            case ulong ul:
                result = ul;
                return true;

            case int i:
                result = i;
                return true;

            case uint ui:
                result = ui;
                return true;

            case short s:
                result = s;
                return true;

            case ushort us:
                result = us;
                return true;

            case byte b:
                result = b;
                return true;

            case sbyte sb:
                result = sb;
                return true;

            case string str:
                return double.TryParse(str, NumberStyles.Float | NumberStyles.AllowThousands, culture, out result)
                    || double.TryParse(str, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out result);

            case IConvertible convertible:
                try
                {
                    result = convertible.ToDouble(culture);
                    return true;
                }
                catch
                {
                    result = 0;
                    return false;
                }
            default:
                result = 0;
                return false;
        }
    }

    private static object? CoerceToTargetType(double magnitude, Type targetType, CultureInfo culture)
    {
        Type type = Nullable.GetUnderlyingType(targetType) ?? targetType;

        if (type == typeof(object) || type == typeof(double))
        {
            return magnitude;
        }

        if (type == typeof(float))
        {
            return (float)magnitude;
        }

        if (type == typeof(decimal))
        {
            return (decimal)magnitude;
        }

        if (type == typeof(long))
        {
            return (long)Math.Round(magnitude, MidpointRounding.AwayFromZero);
        }

        if (type == typeof(ulong))
        {
            if (magnitude < 0)
            {
                return DependencyProperty.UnsetValue;
            }

            return (ulong)Math.Round(magnitude, MidpointRounding.AwayFromZero);
        }

        if (type == typeof(int))
        {
            return (int)Math.Round(magnitude, MidpointRounding.AwayFromZero);
        }

        if (type == typeof(uint))
        {
            if (magnitude < 0)
            {
                return DependencyProperty.UnsetValue;
            }

            return (uint)Math.Round(magnitude, MidpointRounding.AwayFromZero);
        }

        if (type == typeof(string))
        {
            return magnitude.ToString(culture);
        }

        try
        {
            return System.Convert.ChangeType(magnitude, type, culture);
        }
        catch
        {
            return DependencyProperty.UnsetValue;
        }
    }
}

/// <summary>
/// Formats and parses byte sizes as human-readable strings.
/// Inspired by <see href="https://github.com/Humanizr/Humanizer">Humanizer</see> / <see href="https://github.com/omar/ByteSize">ByteSize</see>,
/// but kept dependency-free for this library.
/// </summary>
public static class MemorySize
{
    private const double BinaryRadix = 1024d;
    private const double DecimalRadix = 1000d;

    // "1.5 MB", "-512KiB", "1024", "2,048.5 GiB"
    [SuppressMessage("Performance", "SYSLIB1045:Convert to 'GeneratedRegexAttribute'.")]
    [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression")]
    private static readonly Regex ParseRegex = new(
        @"^\s*(?<number>[\+\-]?\d+(?:[.,]\d+)?(?:[eE][\+\-]?\d+)?)\s*(?<unit>[a-zA-Z]+)?\s*$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private static readonly MemorySizeUnit[] OrderedUnits =
    [
        MemorySizeUnit.Byte,
        MemorySizeUnit.Kilobyte,
        MemorySizeUnit.Megabyte,
        MemorySizeUnit.Gigabyte,
        MemorySizeUnit.Terabyte,
        MemorySizeUnit.Petabyte,
    ];

    /// <summary>
    /// Converts a magnitude expressed in <paramref name="unit"/> into bytes.
    /// </summary>
    public static double ToBytes(double value, MemorySizeUnit unit, MemorySizeUnitSystem unitSystem = MemorySizeUnitSystem.Binary)
    {
        if (unit is MemorySizeUnit.Auto or MemorySizeUnit.Byte)
        {
            return value;
        }

        return value * GetFactor(unit, unitSystem);
    }

    /// <summary>
    /// Converts bytes into a magnitude expressed in <paramref name="unit"/>.
    /// </summary>
    public static double FromBytes(double bytes, MemorySizeUnit unit, MemorySizeUnitSystem unitSystem = MemorySizeUnitSystem.Binary)
    {
        if (unit is MemorySizeUnit.Auto or MemorySizeUnit.Byte)
        {
            return bytes;
        }

        return bytes / GetFactor(unit, unitSystem);
    }

    /// <summary>
    /// Picks the largest unit whose absolute scaled value is ≥ 1 (clamped to min/max).
    /// </summary>
    public static MemorySizeUnit GetLargestWholeUnit(
        double bytes,
        MemorySizeUnitSystem unitSystem = MemorySizeUnitSystem.Binary,
        MemorySizeUnit minUnit = MemorySizeUnit.Byte,
        MemorySizeUnit maxUnit = MemorySizeUnit.Petabyte)
    {
        if (minUnit == MemorySizeUnit.Auto)
        {
            minUnit = MemorySizeUnit.Byte;
        }

        if (maxUnit == MemorySizeUnit.Auto)
        {
            maxUnit = MemorySizeUnit.Petabyte;
        }

        if (minUnit > maxUnit)
        {
            (maxUnit, minUnit) = (minUnit, maxUnit);
        }

        double abs = Math.Abs(bytes);
        MemorySizeUnit selected = minUnit;

        foreach (MemorySizeUnit unit in OrderedUnits)
        {
            if (unit < minUnit || unit > maxUnit)
            {
                continue;
            }

            if (abs >= GetFactor(unit, unitSystem))
            {
                selected = unit;
            }
        }

        return selected;
    }

    /// <summary>
    /// Formats a byte quantity as a human-readable string, e.g. <c>1.5 GB</c>.
    /// </summary>
    public static string Format(
        double bytes,
        string? numberFormat = null,
        IFormatProvider? formatProvider = null,
        MemorySizeUnit outputUnit = MemorySizeUnit.Auto,
        MemorySizeUnitSystem unitSystem = MemorySizeUnitSystem.Binary,
        MemorySizeSymbolStyle symbolStyle = MemorySizeSymbolStyle.Short,
        MemorySizeUnit minUnit = MemorySizeUnit.Byte,
        MemorySizeUnit maxUnit = MemorySizeUnit.Petabyte)
    {
        formatProvider ??= CultureInfo.CurrentCulture;
        numberFormat ??= "0.##";

        if (double.IsNaN(bytes) || double.IsInfinity(bytes))
        {
            return bytes.ToString(formatProvider);
        }

        MemorySizeUnit unit = outputUnit == MemorySizeUnit.Auto
            ? GetLargestWholeUnit(bytes, unitSystem, minUnit, maxUnit)
            : outputUnit;

        if (unit == MemorySizeUnit.Auto)
        {
            unit = MemorySizeUnit.Byte;
        }

        double scaled = FromBytes(bytes, unit, unitSystem);
        string number = scaled.ToString(numberFormat, formatProvider);
        string symbol = GetSymbol(unit, unitSystem, symbolStyle);
        return string.Concat(number, " ", symbol);
    }

    /// <summary>
    /// Tries to parse strings such as <c>1.5 GB</c>, <c>512KiB</c>, or a bare number (treated as bytes).
    /// </summary>
    public static bool TryParse(
        string? text,
        out double bytes,
        MemorySizeUnitSystem unitSystem = MemorySizeUnitSystem.Binary,
        IFormatProvider? formatProvider = null)
    {
        bytes = 0;
        if (string.IsNullOrWhiteSpace(text))
        {
            return false;
        }

        formatProvider ??= CultureInfo.CurrentCulture;
        Match match = ParseRegex.Match(text);
        if (!match.Success)
        {
            return false;
        }

        string numberPart = match.Groups["number"].Value;
        // Normalize culture-invariant / mixed separators for TryParse
        if (!TryParseNumber(numberPart, formatProvider, out double number))
        {
            return false;
        }

        string unitPart = match.Groups["unit"].Value;
        if (string.IsNullOrEmpty(unitPart))
        {
            bytes = number;
            return true;
        }

        if (!TryResolveUnit(unitPart, out MemorySizeUnit unit, out bool forceBinary, out bool forceDecimal))
        {
            return false;
        }

        MemorySizeUnitSystem effectiveSystem = unitSystem;
        if (forceBinary)
        {
            effectiveSystem = MemorySizeUnitSystem.Binary;
        }
        else if (forceDecimal)
        {
            effectiveSystem = MemorySizeUnitSystem.Decimal;
        }

        bytes = ToBytes(number, unit, effectiveSystem);
        return true;
    }

    public static string GetSymbol(
        MemorySizeUnit unit,
        MemorySizeUnitSystem unitSystem = MemorySizeUnitSystem.Binary,
        MemorySizeSymbolStyle symbolStyle = MemorySizeSymbolStyle.Short)
    {
        if (unit == MemorySizeUnit.Auto)
        {
            unit = MemorySizeUnit.Byte;
        }

        bool useIec = symbolStyle == MemorySizeSymbolStyle.Iec && unitSystem == MemorySizeUnitSystem.Binary;

        if (unit == MemorySizeUnit.Byte)
        {
            return "B";
        }

        if (useIec)
        {
            switch (unit)
            {
                case MemorySizeUnit.Kilobyte: return "KiB";
                case MemorySizeUnit.Megabyte: return "MiB";
                case MemorySizeUnit.Gigabyte: return "GiB";
                case MemorySizeUnit.Terabyte: return "TiB";
                case MemorySizeUnit.Petabyte: return "PiB";
            }
        }

        return unit switch
        {
            MemorySizeUnit.Kilobyte => "KB",
            MemorySizeUnit.Megabyte => "MB",
            MemorySizeUnit.Gigabyte => "GB",
            MemorySizeUnit.Terabyte => "TB",
            MemorySizeUnit.Petabyte => "PB",
            _ => "B",
        };
    }

    private static double GetFactor(MemorySizeUnit unit, MemorySizeUnitSystem unitSystem)
    {
        double radix = unitSystem == MemorySizeUnitSystem.Decimal ? DecimalRadix : BinaryRadix;
        int power = unit switch
        {
            MemorySizeUnit.Kilobyte => 1,
            MemorySizeUnit.Megabyte => 2,
            MemorySizeUnit.Gigabyte => 3,
            MemorySizeUnit.Terabyte => 4,
            MemorySizeUnit.Petabyte => 5,
            _ => 0,
        };

        return Math.Pow(radix, power);
    }

    private static bool TryParseNumber(string text, IFormatProvider formatProvider, out double number)
    {
        if (double.TryParse(text, NumberStyles.Float | NumberStyles.AllowThousands, formatProvider, out number))
        {
            return true;
        }

        // Fallback: treat '.' as decimal separator (common in XAML / invariant data)
        return double.TryParse(text, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out number);
    }

    private static bool TryResolveUnit(string unitText, out MemorySizeUnit unit, out bool forceBinary, out bool forceDecimal)
    {
        unit = MemorySizeUnit.Byte;
        forceBinary = false;
        forceDecimal = false;

        switch (unitText.Trim().ToUpperInvariant())
        {
            case "B":
            case "BYTE":
            case "BYTES":
                unit = MemorySizeUnit.Byte;
                return true;

            case "KIB":
            case "KIBIBYTE":
            case "KIBIBYTES":
                unit = MemorySizeUnit.Kilobyte;
                forceBinary = true;
                return true;

            case "KB":
            case "K":
            case "KILOBYTE":
            case "KILOBYTES":
                unit = MemorySizeUnit.Kilobyte;
                return true;

            case "MIB":
            case "MEBIBYTE":
            case "MEBIBYTES":
                unit = MemorySizeUnit.Megabyte;
                forceBinary = true;
                return true;

            case "MB":
            case "M":
            case "MEGABYTE":
            case "MEGABYTES":
                unit = MemorySizeUnit.Megabyte;
                return true;

            case "GIB":
            case "GIBIBYTE":
            case "GIBIBYTES":
                unit = MemorySizeUnit.Gigabyte;
                forceBinary = true;
                return true;

            case "GB":
            case "G":
            case "GIGABYTE":
            case "GIGABYTES":
                unit = MemorySizeUnit.Gigabyte;
                return true;

            case "TIB":
            case "TEBIBYTE":
            case "TEBIBYTES":
                unit = MemorySizeUnit.Terabyte;
                forceBinary = true;
                return true;

            case "TB":
            case "T":
            case "TERABYTE":
            case "TERABYTES":
                unit = MemorySizeUnit.Terabyte;
                return true;

            case "PIB":
            case "PEBIBYTE":
            case "PEBIBYTES":
                unit = MemorySizeUnit.Petabyte;
                forceBinary = true;
                return true;

            case "PB":
            case "P":
            case "PETABYTE":
            case "PETABYTES":
                unit = MemorySizeUnit.Petabyte;
                return true;

            default:
                return false;
        }
    }
}

/// <summary>
/// Memory / file size magnitude.
/// </summary>
/// <remarks>
/// <see cref="Auto"/> is only meaningful for output scaling (pick the largest whole unit).
/// For input, <see cref="Auto"/> is treated as <see cref="Byte"/>.
/// </remarks>
public enum MemorySizeUnit
{
    /// <summary>Automatically choose the largest unit whose absolute magnitude is ≥ 1.</summary>
    Auto = 0,

    Byte = 1,
    Kilobyte = 2,
    Megabyte = 3,
    Gigabyte = 4,
    Terabyte = 5,
    Petabyte = 6,
}

/// <summary>
/// Base used when converting between bytes and larger units.
/// </summary>
public enum MemorySizeUnitSystem
{
    /// <summary>
    /// Powers of 1024 (IEC binary). Typical for RAM, process working set, and most file-length UIs.
    /// </summary>
    Binary = 0,

    /// <summary>
    /// Powers of 1000 (SI decimal). Typical for disk marketing and network throughput.
    /// </summary>
    Decimal = 1,
}

/// <summary>
/// How unit symbols are rendered.
/// </summary>
public enum MemorySizeSymbolStyle
{
    /// <summary>
    /// Common UI labels: B, KB, MB, GB, TB, PB.
    /// Used even when <see cref="MemorySizeUnitSystem.Binary"/> (same convention as Windows Explorer / Humanizer defaults).
    /// </summary>
    Short = 0,

    /// <summary>
    /// IEC labels when binary: B, KiB, MiB, GiB, TiB, PiB.
    /// Falls back to KB/MB/... when <see cref="MemorySizeUnitSystem.Decimal"/>.
    /// </summary>
    Iec = 1,
}
