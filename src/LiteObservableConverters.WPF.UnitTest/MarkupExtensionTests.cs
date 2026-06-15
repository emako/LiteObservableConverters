using LiteObservableConverters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LiteObservableConverters.WPF.UnitTest;

[TestClass]
public class MarkupExtensionTests
{
    [TestMethod]
    public void TrueExtension_ProvideValue_ReturnsTrue()
    {
        TrueExtension extension = new();

        object? value = extension.ProvideValue(null!);

        Assert.AreEqual(true, value);
    }

    [TestMethod]
    public void FalseExtension_ProvideValue_ReturnsFalse()
    {
        FalseExtension extension = new();

        object? value = extension.ProvideValue(null!);

        Assert.AreEqual(false, value);
    }

    [TestMethod]
    public void VisibilityVisibleExtension_ProvideValue_ReturnsVisible()
    {
        VisibilityVisibleExtension extension = new();

        object? value = extension.ProvideValue(null!);

        Assert.AreEqual(System.Windows.Visibility.Visible, value);
    }

    [TestMethod]
    public void VisibilityCollapsedExtension_ProvideValue_ReturnsCollapsed()
    {
        VisibilityCollapsedExtension extension = new();

        object? value = extension.ProvideValue(null!);

        Assert.AreEqual(System.Windows.Visibility.Collapsed, value);
    }

    [TestMethod]
    public void StringEmptyExtension_ProvideValue_ReturnsEmptyString()
    {
        StringEmptyExtension extension = new();

        object? value = extension.ProvideValue(null!);

        Assert.AreEqual(string.Empty, value);
    }
}
