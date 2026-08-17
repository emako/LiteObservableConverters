using LiteObservableConverters.DynamicExpresso;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LiteObservableConverters.CalcBinding.ExpressionParsers;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA1067 // Override Object.Equals(object) when implementing IEquatable<T>
#pragma warning disable CA1854 // Prefer the 'IDictionary.TryGetValue(TKey, out TValue)' method
#pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).

public sealed class CachedExpressionParser(IExpressionParser innerParser) : IExpressionParser
#pragma warning restore IDE0079 // Remove unnecessary suppression
{
    public Lambda Parse(string expressionText, Parameter[] parameters)
    {
        var expressionKey = new ExpressionKey(expressionText, parameters);

        var cachedLambda = FindInCache(expressionKey);
        if (cachedLambda != null)
            return cachedLambda;

        var lambda = innerParser.Parse(expressionText, parameters);
        SaveInCache(expressionKey, lambda);

        return lambda;
    }

    private void SaveInCache(ExpressionKey key, Lambda lambda)
    {
        _cachedExpressions[key] = new WeakReference(lambda);
    }

    private Lambda FindInCache(ExpressionKey expressionKey)
    {
        if (_cachedExpressions.ContainsKey(expressionKey))
        {
            var expressionRef = _cachedExpressions[expressionKey];

            if (expressionRef.Target is Lambda lambda)
            {
                return lambda;
            }
            else
            {
                _cachedExpressions.Remove(expressionKey);
                RemoveDeadExpressions();
            }
        }

        return null!;
    }

    private void RemoveDeadExpressions()
    {
        foreach (var key in _cachedExpressions.Keys.ToList())
        {
            if (!_cachedExpressions[key].IsAlive)
                _cachedExpressions.Remove(key);
        }
    }

    public void SetReference(IEnumerable<ReferenceType> referencedTypes)
    {
        innerParser.SetReference(referencedTypes);
    }

    private readonly Dictionary<ExpressionKey, WeakReference> _cachedExpressions = [];

    private readonly struct ExpressionKey(string expressionText, Parameter[] parameters) : IEquatable<ExpressionKey>
    {
        private readonly string _expressionText = expressionText;
        private readonly Parameter[] _parameters = parameters;

        public override readonly int GetHashCode()
        {
            return (_expressionText.GetHashCode() * 397) ^ (_parameters.Length);
        }

        public readonly bool Equals(ExpressionKey other)
        {
            return string.Equals(_expressionText, other._expressionText)
                && _parameters.SequenceEqual(other._parameters, _parameterComparer);
        }

        private static readonly ParameterComparer _parameterComparer = new();
    }

    private class ParameterComparer : IEqualityComparer<Parameter>
    {
        public bool Equals(Parameter x, Parameter y)
        {
            return string.Equals(x.Name, y.Name) && x.Type == y.Type;
        }

        public int GetHashCode(Parameter parameter)
        {
            return (parameter.Name.GetHashCode() * 397) ^ (parameter.Type.GetHashCode());
        }
    }
}
