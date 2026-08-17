using System;
using System.Linq.Expressions;

namespace LiteObservableConverters.DynamicExpresso;

#pragma warning disable IDE0011
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable IDE0056 // Use index operator
#pragma warning disable CA1510

/// <summary>
/// An expression parameter. This class is thread safe.
/// </summary>
public class Parameter
#pragma warning restore IDE0079 // Remove unnecessary suppression
{
    public Parameter(string name, object value)
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value));

        Name = name;
        Type = value.GetType();
        Value = value;

        Expression = System.Linq.Expressions.Expression.Parameter(Type, name);
    }

    public Parameter(ParameterExpression parameterExpression)
    {
        Name = parameterExpression.Name!;
        Type = parameterExpression.Type;
        Value = null!;

        Expression = parameterExpression;
    }

    public Parameter(string name, Type type, object value = null!)
    {
        Name = name;
        Type = type;
        Value = value;

        Expression = System.Linq.Expressions.Expression.Parameter(type, name);
    }

    public static Parameter Create<T>(string name, T value)
    {
        return new Parameter(name, typeof(T), value!);
    }

    public string Name { get; private set; }
    public Type Type { get; private set; }
    public object Value { get; private set; }

    public ParameterExpression Expression { get; private set; }
}

/// <summary>
/// Parameter with its position in the expression.
/// </summary>
internal class ParameterWithPosition(int pos, string name, Type type) : Parameter(name, type)
{
    public int Position { get; } = pos;
}
