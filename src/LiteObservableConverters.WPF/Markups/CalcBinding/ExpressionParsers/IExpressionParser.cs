using DynamicExpresso;
using System.Collections.Generic;

namespace CalcBinding.ExpressionParsers;

public interface IExpressionParser
{
    public Lambda Parse(string expressionText, params Parameter[] parameters);

    public void SetReference(IEnumerable<ReferenceType> referencedTypes);
}
