namespace LiteObservableConverters.CalcBinding.PathAnalysis;

public class MathToken : PathToken
{
    public string MathMember { get; private set; }

    private readonly PathTokenId id;

    public override PathTokenId Id => id;

    public MathToken(int start, int end, string mathMember)
        : base(start, end)
    {
        MathMember = mathMember;
        id = new PathTokenId(PathTokenType.Math, string.Join(".", "Math", MathMember));
    }
}
