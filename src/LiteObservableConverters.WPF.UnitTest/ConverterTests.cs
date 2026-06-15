using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using LiteObservableConverters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LiteObservableConverters.WPF.UnitTest;

[TestClass]
public class ConverterTests
{
    private readonly CultureInfo _culture = CultureInfo.InvariantCulture;

    #region Bool Converters

    [TestMethod]
    public void BoolInverter_Convert_InvertsValue()
    {
        BoolInverter converter = new()
        {
            IsInverted = true,
        };

        object? result = converter.Convert(true, typeof(bool), null, _culture);
        Assert.AreEqual(false, result);

        result = converter.Convert(false, typeof(bool), null, _culture);
        Assert.AreEqual(true, result);
    }

    [TestMethod]
    public void BoolToVisibilityConverter_Convert_ReturnsVisibility()
    {
        BoolToVisibilityConverter converter = new();

        object? result = converter.Convert(true, typeof(Visibility), null, _culture);
        Assert.AreEqual(Visibility.Visible, result);

        result = converter.Convert(false, typeof(Visibility), null, _culture);
        Assert.AreEqual(Visibility.Collapsed, result);
    }

    [TestMethod]
    public void BoolToStringConverter_Convert_ReturnsString()
    {
        BoolToStringConverter converter = new()
        {
            TrueValue = "Yes",
            FalseValue = "No",
        };

        object? result = converter.Convert(true, typeof(string), null, _culture);
        Assert.AreEqual("Yes", result);

        result = converter.Convert(false, typeof(string), null, _culture);
        Assert.AreEqual("No", result);
    }

    [TestMethod]
    public void BoolToObjectConverter_Convert_ReturnsObject()
    {
        BoolToObjectConverter converter = new()
        {
            TrueValue = "TrueObject",
            FalseValue = "FalseObject",
        };

        object? result = converter.Convert(true, typeof(object), null, _culture);
        Assert.AreEqual("TrueObject", result);

        result = converter.Convert(false, typeof(object), null, _culture);
        Assert.AreEqual("FalseObject", result);
    }

    [TestMethod]
    public void BoolToDoubleConverter_Convert_ReturnsDouble()
    {
        BoolToDoubleConverter converter = new()
        {
            TrueValue = 1.0,
            FalseValue = 0.0,
        };

        object? result = converter.Convert(true, typeof(double), null, _culture);
        Assert.AreEqual(1.0, result);

        result = converter.Convert(false, typeof(double), null, _culture);
        Assert.AreEqual(0.0, result);
    }

    [TestMethod]
    public void BoolToThicknessConverter_Convert_ReturnsThickness()
    {
        BoolToThicknessConverter converter = new()
        {
            TrueValue = new Thickness(1),
            FalseValue = new Thickness(0),
        };

        object? result = converter.Convert(true, typeof(Thickness), null, _culture);
        Assert.AreEqual(new Thickness(1), result);

        result = converter.Convert(false, typeof(Thickness), null, _culture);
        Assert.AreEqual(new Thickness(0), result);
    }

    [TestMethod]
    public void BoolToFontWeightConverter_Convert_ReturnsFontWeight()
    {
        BoolToFontWeightConverter converter = new()
        {
            TrueValue = FontWeights.Bold,
            FalseValue = FontWeights.Normal,
        };

        object? result = converter.Convert(true, typeof(FontWeight), null, _culture);
        Assert.AreEqual(FontWeights.Bold, result);

        result = converter.Convert(false, typeof(FontWeight), null, _culture);
        Assert.AreEqual(FontWeights.Normal, result);
    }

    [TestMethod]
    public void BoolToBrushConverter_Convert_ReturnsBrush()
    {
        BoolToBrushConverter converter = new()
        {
            TrueValue = Brushes.Green,
            FalseValue = Brushes.Red,
        };

        object? result = converter.Convert(true, typeof(Brush), null, _culture);
        Assert.AreEqual(Brushes.Green, result);

        result = converter.Convert(false, typeof(Brush), null, _culture);
        Assert.AreEqual(Brushes.Red, result);
    }

    #endregion Bool Converters

    #region Double Converters

    [TestMethod]
    public void DoubleAddConverter_Convert_AddsValue()
    {
        DoubleAddConverter converter = new();

        object? result = converter.Convert(10.0, typeof(double), "5", _culture);
        Assert.AreEqual(15.0, result);
    }

    [TestMethod]
    public void DoubleSubtractConverter_Convert_SubtractsValue()
    {
        DoubleSubtractConverter converter = new();

        object? result = converter.Convert(10.0, typeof(double), "5", _culture);
        Assert.AreEqual(5.0, result);
    }

    [TestMethod]
    public void DoubleMultiplyConverter_Convert_MultipliesValue()
    {
        DoubleMultiplyConverter converter = new()
        {
            By = 2.0,
        };

        object? result = converter.Convert(5.0, typeof(double), null, _culture);
        Assert.AreEqual(10.0, result);
    }

    [TestMethod]
    public void DoubleClampConverter_Convert_ClampsValue()
    {
        DoubleClampConverter converter = new()
        {
            Minimum = 0.0,
            Maximum = 10.0,
        };

        object? result = converter.Convert(15.0, typeof(double), null, _culture);
        Assert.AreEqual(10.0, result);

        result = converter.Convert(-5.0, typeof(double), null, _culture);
        Assert.AreEqual(0.0, result);

        result = converter.Convert(5.0, typeof(double), null, _culture);
        Assert.AreEqual(5.0, result);
    }

    [TestMethod]
    public void DoubleToIntConverter_Convert_ConvertsToInt()
    {
        DoubleToIntConverter converter = new();

        object? result = converter.Convert(5.6, typeof(int), null, _culture);
        Assert.AreEqual(6, result);

        result = converter.Convert(5.4, typeof(int), null, _culture);
        Assert.AreEqual(5, result);
    }

    [TestMethod]
    public void DoubleEqualsConverter_Convert_ChecksEquality()
    {
        DoubleEqualsConverter converter = new();

        object? result = converter.Convert(5.0, typeof(bool), "5.0", _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert(5.0, typeof(bool), "6.0", _culture);
        Assert.AreEqual(false, result);
    }

    [TestMethod]
    public void DoubleGreaterThanConverter_Convert_ChecksGreaterThan()
    {
        DoubleGreaterThanConverter converter = new();

        object? result = converter.Convert(10.0, typeof(bool), "5.0", _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert(5.0, typeof(bool), "10.0", _culture);
        Assert.AreEqual(false, result);
    }

    [TestMethod]
    public void DoubleGreaterThanOrEqualConverter_Convert_ChecksGreaterThanOrEqual()
    {
        DoubleGreaterThanOrEqualConverter converter = new();

        object? result = converter.Convert(10.0, typeof(bool), "5.0", _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert(5.0, typeof(bool), "5.0", _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert(4.0, typeof(bool), "5.0", _culture);
        Assert.AreEqual(false, result);
    }

    [TestMethod]
    public void DoubleLessThanConverter_Convert_ChecksLessThan()
    {
        DoubleLessThanConverter converter = new();

        object? result = converter.Convert(5.0, typeof(bool), "10.0", _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert(10.0, typeof(bool), "5.0", _culture);
        Assert.AreEqual(false, result);
    }

    [TestMethod]
    public void DoubleLessThanOrEqualConverter_Convert_ChecksLessThanOrEqual()
    {
        DoubleLessThanOrEqualConverter converter = new();

        object? result = converter.Convert(5.0, typeof(bool), "10.0", _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert(5.0, typeof(bool), "5.0", _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert(6.0, typeof(bool), "5.0", _culture);
        Assert.AreEqual(false, result);
    }

    [TestMethod]
    public void DoubleToBoolConverter_Convert_ConvertsToBool()
    {
        DoubleToBoolConverter converter = new()
        {
            TrueValue = 1.0,
        };

        object? result = converter.Convert(1.0, typeof(bool), null, _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert(0.0, typeof(bool), null, _culture);
        Assert.AreEqual(false, result);
    }

    [TestMethod]
    public void DoubleCompareConverter_Convert_ComparesValues()
    {
        DoubleCompareConverter converter = new()
        {
            TargetValue = 10.0,
            Comparison = NumberComparison.Equal,
        };

        object? result = converter.Convert(10.0, typeof(bool), null, _culture);
        Assert.AreEqual(true, result);

        converter.Comparison = NumberComparison.GreaterThan;
        result = converter.Convert(15.0, typeof(bool), null, _culture);
        Assert.AreEqual(true, result);
    }

    [TestMethod]
    public void DoubleMultiplyConverter_Convert_UsesParameterMultiplier()
    {
        DoubleMultiplyConverter converter = new();

        object? result = converter.Convert(5.0, typeof(double), 2.0, _culture);
        Assert.AreEqual(10.0, result);
    }

    #endregion Double Converters

    #region String Converters

    [TestMethod]
    public void StringCaseConverter_Convert_ChangesCase()
    {
        StringCaseConverter converter = new();

        object? result = converter.Convert("hello", typeof(string), "U", _culture);
        Assert.AreEqual("HELLO", result);

        result = converter.Convert("HELLO", typeof(string), "L", _culture);
        Assert.AreEqual("hello", result);

        result = converter.Convert("hello world", typeof(string), "T", _culture);
        Assert.AreEqual("Hello World", result);
    }

    #endregion String Converters

    #region Equality Converters

    [TestMethod]
    public void IsEqualConverter_Convert_ChecksEquality()
    {
        IsEqualConverter converter = new();

        object? result = converter.Convert("test", typeof(bool), "test", _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert("test", typeof(bool), "other", _culture);
        Assert.AreEqual(false, result);

        result = converter.Convert(null, typeof(bool), null, _culture);
        Assert.AreEqual(true, result);
    }

    [TestMethod]
    public void IsNotEqualConverter_Convert_ChecksInequality()
    {
        IsNotEqualConverter converter = new();

        object? result = converter.Convert("test", typeof(bool), "other", _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert("test", typeof(bool), "test", _culture);
        Assert.AreEqual(false, result);
    }

    [TestMethod]
    public void IsNullConverter_Convert_DetectsNull()
    {
        IsNullConverter converter = new();

        object? result = converter.Convert(null, typeof(bool), null, _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert("value", typeof(bool), null, _culture);
        Assert.AreEqual(false, result);
    }

    [TestMethod]
    public void IsNotNullConverter_Convert_DetectsNotNull()
    {
        IsNotNullConverter converter = new();

        object? result = converter.Convert("value", typeof(bool), null, _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert(null, typeof(bool), null, _culture);
        Assert.AreEqual(false, result);
    }

    #endregion Equality Converters

    #region Enum Converters

    [TestMethod]
    public void EnumToBoolConverter_Convert_ConvertsToBool()
    {
        EnumToBoolConverter converter = new();

        object? result = converter.Convert(DayOfWeek.Monday, typeof(bool), DayOfWeek.Monday, _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert(DayOfWeek.Monday, typeof(bool), DayOfWeek.Tuesday, _culture);
        Assert.AreEqual(false, result);
    }

    [TestMethod]
    public void EnumToIntConverter_Convert_ConvertsToInt()
    {
        EnumToIntConverter converter = new();

        object? result = converter.Convert(DayOfWeek.Monday, typeof(int), null, _culture);
        Assert.AreEqual(1, result);

        result = converter.Convert(DayOfWeek.Sunday, typeof(int), null, _culture);
        Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void EnumToObjectConverter_Convert_ConvertsEnumAndStringKeys()
    {
        ResourceDictionary items = new()
        {
            [nameof(DayOfWeek.Monday)] = "MondayObject",
            ["CustomKey"] = "CustomObject",
        };
        EnumToObjectConverter converter = new()
        {
            Items = items,
        };

        object? result = converter.Convert(DayOfWeek.Monday, typeof(object), null, _culture);
        Assert.AreEqual("MondayObject", result);

        result = converter.Convert("CustomKey", typeof(object), null, _culture);
        Assert.AreEqual("CustomObject", result);
    }

    [TestMethod]
    public void EnumToObjectConverter_Convert_UsesDictionaryParameterWithoutChangingItems()
    {
        Dictionary<string, object?> items = new()
        {
            [nameof(DayOfWeek.Tuesday)] = "TuesdayObject",
        };
        EnumToObjectConverter converter = new();

        object? result = converter.Convert(DayOfWeek.Tuesday, typeof(object), items, _culture);

        Assert.AreEqual("TuesdayObject", result);
        Assert.IsNull(converter.Items);
    }

    [TestMethod]
    public void EnumToObjectConverter_Convert_UsesStringParameterMapping()
    {
        EnumToObjectConverter converter = new();

        object? result = converter.Convert(DayOfWeek.Wednesday, typeof(object), "Wednesday:Weekday;Sunday:Weekend", _culture);

        Assert.AreEqual("Weekday", result);
        Assert.IsNull(converter.Items);
    }

    #endregion Enum Converters

    #region Type Converters

    [TestMethod]
    public void IntToBoolConverter_Convert_ConvertsToBool()
    {
        IntToBoolConverter converter = new()
        {
            TrueValue = 1,
        };

        object? result = converter.Convert(1, typeof(bool), null, _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert(0, typeof(bool), null, _culture);
        Assert.AreEqual(false, result);
    }

    [TestMethod]
    public void VersionToStringConverter_Convert_ConvertsToString()
    {
        VersionToStringConverter converter = new();
        Version version = new(1, 2, 3, 4);

        object? result = converter.Convert(version, typeof(string), null, _culture);
        Assert.AreEqual("1.2.3.4", result);

        result = converter.Convert(version, typeof(string), "2", _culture);
        Assert.AreEqual("1.2", result);
    }

    #endregion Type Converters

    #region Collection Converters

    [TestMethod]
    public void FirstOrDefaultConverter_Convert_ReturnsFirstElement()
    {
        FirstOrDefaultConverter converter = new();

        object? result = converter.Convert(new int[] { 1, 2, 3 }, typeof(int), null, _culture);
        Assert.AreEqual(1, result);

        result = converter.Convert(Array.Empty<int>(), typeof(int), null, _culture);
        Assert.AreEqual(DependencyProperty.UnsetValue, result);
    }

    [TestMethod]
    public void ValueToEnumerableConverter_Convert_ConvertsToEnumerable()
    {
        ValueToEnumerableConverter converter = new();

        object[]? result = converter.Convert("test", typeof(IEnumerable<object>), null, _culture) as object[];
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Length);
        Assert.AreEqual("test", result[0]);
    }

    #endregion Collection Converters

    #region DateTime Converters

    [TestMethod]
    public void DateTimeToStringConverter_Convert_ConvertsToString()
    {
        DateTimeToStringConverter converter = new()
        {
            Format = "yyyy-MM-dd",
        };
        DateTime dateTime = new(2024, 1, 15, 0, 0, 0, DateTimeKind.Local);

        object? result = converter.Convert(dateTime, typeof(string), null, _culture);
        Assert.AreEqual("2024-01-15", result);
    }

    #endregion DateTime Converters

    #region Visibility Converters

    [TestMethod]
    public void VisibilityInverter_Convert_InvertsVisibility()
    {
        BoolToVisibilityConverter converter = new()
        {
            TrueValue = Visibility.Visible,
            FalseValue = Visibility.Collapsed,
            IsInverted = true,
        };

        object? result = converter.Convert(true, typeof(Visibility), null, _culture);
        Assert.AreEqual(Visibility.Collapsed, result);

        result = converter.Convert(false, typeof(Visibility), null, _culture);
        Assert.AreEqual(Visibility.Visible, result);
    }

    #endregion Visibility Converters

    #region Color Converters

    [TestMethod]
    public void ColorToSolidBrushConverter_Convert_ConvertsToBrush()
    {
        ColorToSolidBrushConverter converter = new();
        Color color = Colors.Red;

        SolidColorBrush? result = converter.Convert(color, typeof(SolidColorBrush), null, _culture) as SolidColorBrush;
        Assert.IsNotNull(result);
        Assert.AreEqual(Colors.Red, result.Color);
    }

    [TestMethod]
    public void StringToColorConverter_Convert_ConvertsToColor()
    {
        StringToColorConverter converter = new();

        object? result = converter.Convert("#FF0000", typeof(Color), null, _culture);
        Assert.IsInstanceOfType(result, typeof(Color));
    }

    #endregion Color Converters

    #region Utility Converters

    [TestMethod]
    public void DebugConverter_Convert_ReturnsValue()
    {
        DebugConverter converter = new();
        const string testValue = "test";

        object? result = converter.Convert(testValue, typeof(object), null, _culture);
        Assert.AreEqual(testValue, result);
    }

    [TestMethod]
    public void TraceConverter_Convert_ReturnsValue()
    {
        TraceConverter converter = new();
        const string testValue = "test";

        object? result = converter.Convert(testValue, typeof(object), null, _culture);
        Assert.AreEqual(testValue, result);
    }

    [TestMethod]
    public void IfConverter_Convert_ReturnsConditionalValue()
    {
        IfConverter converter = new()
        {
            Condition = true,
            TrueValue = "TrueResult",
            FalseValue = "FalseResult",
        };

        object? result = converter.Convert(null, typeof(object), null, _culture);
        Assert.AreEqual("TrueResult", result);

        converter.Condition = false;
        result = converter.Convert(null, typeof(object), null, _culture);
        Assert.AreEqual("FalseResult", result);
    }

    [TestMethod]
    public void ObjectAddConverter_Convert_AddsValues()
    {
        ObjectAddConverter converter = new();

        object? result = converter.Convert("Hello", typeof(string), " World", _culture);
        Assert.AreEqual("Hello World", result);
    }

    [TestMethod]
    public void TypeToBoolConverter_Convert_ChecksType()
    {
        TypeToBoolConverter converter = new();

        object? result = converter.Convert("test", typeof(bool), typeof(string), _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert(123, typeof(bool), typeof(string), _culture);
        Assert.AreEqual(false, result);
    }

    [TestMethod]
    public void IsInCollectionConverter_Convert_ChecksInCollection()
    {
        IsInCollectionConverter converter = new();

        object? result = converter.Convert("b", typeof(bool), new string[] { "a", "b", "c" }, _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert("d", typeof(bool), new string[] { "a", "b", "c" }, _culture);
        Assert.AreEqual(false, result);
    }

    [TestMethod]
    public void IsInCollectionConverter_Convert_WithStringParameter()
    {
        IsInCollectionConverter converter = new();

        object? result = converter.Convert("b", typeof(bool), "a,b,c", _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert("d", typeof(bool), "a,b,c", _culture);
        Assert.AreEqual(false, result);
    }

    [TestMethod]
    public void SingletonValueConverterBase_ExposesSharedInstance()
    {
        Assert.AreSame(BoolInverter.Instance, BoolInverter.Instance);
        Assert.IsNotNull(EqualityConverter.Instance);
    }

    #endregion Utility Converters
}
