using System.Globalization;
using System.Windows;
using LiteObservableConverters;

namespace LiteObservableConverters.WPF.UnitTest;

[TestClass]
public class MemorySizeConverterTests
{
    private readonly CultureInfo _culture = CultureInfo.InvariantCulture;

    [TestMethod]
    public void Format_AutoScalesBinaryShortSymbols()
    {
        Assert.AreEqual("512 B", MemorySize.Format(512, formatProvider: _culture));
        Assert.AreEqual("1 KB", MemorySize.Format(1024, formatProvider: _culture));
        Assert.AreEqual("1.5 KB", MemorySize.Format(1536, formatProvider: _culture));
        Assert.AreEqual("1 MB", MemorySize.Format(1024d * 1024, formatProvider: _culture));
        Assert.AreEqual("1 GB", MemorySize.Format(1024d * 1024 * 1024, formatProvider: _culture));
        Assert.AreEqual("0 B", MemorySize.Format(0, formatProvider: _culture));
    }

    [TestMethod]
    public void Format_UsesIecSymbolsWhenRequested()
    {
        string result = MemorySize.Format(
            1536,
            formatProvider: _culture,
            symbolStyle: MemorySizeSymbolStyle.Iec);

        Assert.AreEqual("1.5 KiB", result);
    }

    [TestMethod]
    public void Format_UsesDecimalSiScale()
    {
        string result = MemorySize.Format(
            1_500_000,
            formatProvider: _culture,
            unitSystem: MemorySizeUnitSystem.Decimal);

        Assert.AreEqual("1.5 MB", result);
    }

    [TestMethod]
    public void Format_RespectsFixedOutputUnit()
    {
        string result = MemorySize.Format(
            1024d * 1024 * 1024,
            numberFormat: "0.00",
            formatProvider: _culture,
            outputUnit: MemorySizeUnit.Megabyte);

        Assert.AreEqual("1024.00 MB", result);
    }

    [TestMethod]
    public void TryParse_ParsesCommonForms()
    {
        Assert.IsTrue(MemorySize.TryParse("1.5 KB", out double bytes, formatProvider: _culture));
        Assert.AreEqual(1536d, bytes);

        Assert.IsTrue(MemorySize.TryParse("2MiB", out bytes, formatProvider: _culture));
        Assert.AreEqual(2d * 1024 * 1024, bytes);

        Assert.IsTrue(MemorySize.TryParse("1024", out bytes, formatProvider: _culture));
        Assert.AreEqual(1024d, bytes);
    }

    [TestMethod]
    public void Converter_Convert_FromBytes()
    {
        MemorySizeConverter converter = new();

        object? result = converter.Convert(1536L, typeof(string), null, _culture);
        Assert.AreEqual("1.5 KB", result);
    }

    [TestMethod]
    public void Converter_Convert_RespectsInputUnitMegabyte()
    {
        // Reproduces the common bug in ad-hoc converters: value is already MB.
        MemorySizeConverter converter = new()
        {
            InputUnit = MemorySizeUnit.Megabyte,
            Format = "0.##",
        };

        object? result = converter.Convert(1536, typeof(string), null, _culture);
        Assert.AreEqual("1.5 GB", result);

        result = converter.Convert(512, typeof(string), null, _culture);
        Assert.AreEqual("512 MB", result);
    }

    [TestMethod]
    public void Converter_Convert_NullUsesNullValue()
    {
        MemorySizeConverter converter = new()
        {
            NullValue = string.Empty,
        };

        object? result = converter.Convert(null, typeof(string), null, _culture);
        Assert.AreEqual(string.Empty, result);
    }

    [TestMethod]
    public void Converter_Convert_InvalidReturnsUnset()
    {
        MemorySizeConverter converter = new();

        object? result = converter.Convert("not-a-number", typeof(string), null, _culture);
        Assert.AreEqual(DependencyProperty.UnsetValue, result);
    }

    [TestMethod]
    public void Converter_ConvertBack_ParsesToBytes()
    {
        MemorySizeConverter converter = new();

        object? result = converter.ConvertBack("1.5 KB", typeof(long), null, _culture);
        Assert.AreEqual(1536L, result);
    }

    [TestMethod]
    public void Converter_ConvertBack_RespectsInputUnit()
    {
        MemorySizeConverter converter = new()
        {
            InputUnit = MemorySizeUnit.Megabyte,
        };

        object? result = converter.ConvertBack("1.5 GB", typeof(double), null, _culture);
        Assert.AreEqual(1536d, result);
    }

    [TestMethod]
    public void Converter_ParameterOverridesFormat()
    {
        MemorySizeConverter converter = new()
        {
            Format = "0.##",
        };

        object? result = converter.Convert(1536L, typeof(string), "0.000", _culture);
        Assert.AreEqual("1.500 KB", result);
    }

    [TestMethod]
    public void Converter_MinMaxUnitClampsAutoScale()
    {
        MemorySizeConverter converter = new()
        {
            MinUnit = MemorySizeUnit.Kilobyte,
            MaxUnit = MemorySizeUnit.Kilobyte,
            Format = "0.##",
        };

        // 5 MB would normally become "5 MB"; clamp forces KB.
        object? result = converter.Convert(5L * 1024 * 1024, typeof(string), null, _culture);
        Assert.AreEqual("5120 KB", result);
    }
}
