namespace LiteObservableConverters.CalcBinding.ExpressionParsers;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA1822 // Mark members as static

public sealed class ParserFactory
{
    public IExpressionParser CreateCachedParser(IExpressionParser innerParser = null!)
    {
        return new CachedExpressionParser(innerParser ?? new ExpressionParser());
    }
}
