using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace LiteObservableConverters.DynamicExpresso.Reflection;

internal class MethodData
{
    public MethodBase MethodBase = null!;
    public IList<ParameterInfo> Parameters = null!;
    public IList<Expression> PromotedParameters = null!;
    public bool HasParamsArray;

    public static MethodData Gen(MethodBase method)
    {
        return new MethodData
        {
            MethodBase = method,
            Parameters = method.GetParameters()
        };
    }

    public override string ToString()
    {
        return MethodBase.ToString()!;
    }
}
