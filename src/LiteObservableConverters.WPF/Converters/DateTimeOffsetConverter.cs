using System;
using System.Windows.Data;

namespace LiteObservableConverters;

/// <inheritdoc/>
[ValueConversion(typeof(DateTimeOffset), typeof(string))]
public sealed class DateTimeOffsetConverter : DateTimeOffsetToStringConverter
{
}
