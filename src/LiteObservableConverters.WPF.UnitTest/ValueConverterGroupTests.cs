using System.Globalization;
using System.Windows;
using LiteObservableConverters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LiteObservableConverters.WPF.UnitTest;

[TestClass]
public class ValueConverterGroupTests
{
    private readonly CultureInfo _culture = CultureInfo.InvariantCulture;

    [TestMethod]
    public void Convert_ChainsConvertersInOrder()
    {
        ValueConverterGroup group = new()
        {
            Converters =
            [
                new BoolInverter(),
                new BoolToVisibilityConverter(),
            ],
        };

        object? result = group.Convert(true, typeof(Visibility), null, _culture);
        Assert.AreEqual(Visibility.Collapsed, result);
    }

    [TestMethod]
    public void ConvertBack_ReversesConverterChain()
    {
        BoolToVisibilityConverter visibilityConverter = new();
        ValueConverterGroup group = new()
        {
            Converters =
            [
                new BoolInverter(),
                visibilityConverter,
            ],
        };

        object? result = group.ConvertBack(Visibility.Visible, typeof(bool), null, _culture);
        Assert.AreEqual(false, result);
    }

    [TestMethod]
    public void Convert_WithEmptyConverters_ReturnsOriginalValue()
    {
        ValueConverterGroup group = new();

        object? result = group.Convert("value", typeof(object), null, _culture);
        Assert.AreEqual("value", result);
    }

    [TestMethod]
    public void Convert_WithNullConverters_ReturnsUnsetValue()
    {
        ValueConverterGroup group = new()
        {
            Converters = null!,
        };

        object? result = group.Convert("value", typeof(object), null, _culture);
        Assert.AreEqual(DependencyProperty.UnsetValue, result);
    }
}
