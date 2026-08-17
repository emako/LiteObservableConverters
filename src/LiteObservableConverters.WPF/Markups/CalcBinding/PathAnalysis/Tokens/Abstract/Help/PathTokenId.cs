namespace LiteObservableConverters.CalcBinding.PathAnalysis;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CS8765

public class PathTokenId(PathTokenType pathType, string value)
{
    public PathTokenType PathType { get; private set; } = pathType;
    public string Value { get; private set; } = value;

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;

        if (obj is not PathTokenId o)
            return false;

        return (o.PathType == PathType && o.Value == Value);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode() ^ PathType.GetHashCode();
    }
}
