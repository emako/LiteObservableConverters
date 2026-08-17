namespace CalcBinding.PathAnalysis;

public abstract class PathToken(int start, int end)
{
    public int Start { get; private set; } = start;

    public int End { get; private set; } = end;

    public abstract PathTokenId Id { get; }
}
