using System.Windows.Data;

namespace LiteObservableConverters;

[ValueConversion(typeof(object), typeof(bool))]
public class IsEqualConverter : EqualityConverter;
