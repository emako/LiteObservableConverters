using LiteObservableConverters.DynamicExpresso;
using System.Collections.Generic;

namespace LiteObservableConverters.CalcBinding.ExpressionParsers;

public interface IExpressionParser
{
    public Lambda Parse(string expressionText, params Parameter[] parameters);

    public void SetReference(IEnumerable<ReferenceType> referencedTypes);
}
