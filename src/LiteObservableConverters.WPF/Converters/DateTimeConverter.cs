using System;
using System.Windows.Data;

namespace LiteObservableConverters;

/// <inheritdoc/>
[ValueConversion(typeof(DateTime), typeof(string))]
public sealed class DateTimeConverter : DateTimeToStringConverter
{
}
