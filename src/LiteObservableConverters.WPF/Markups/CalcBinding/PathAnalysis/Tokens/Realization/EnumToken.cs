using System;

namespace LiteObservableConverters.CalcBinding.PathAnalysis;

public class EnumToken : PathToken
{
    public Type Enum { get; private set; }
    public string EnumMember { get; private set; }
    public string Namespace { get; private set; }

    private readonly PathTokenId id;
    public override PathTokenId Id => id;

    public EnumToken(int start, int end, string @namespace, Type @enum, string enumMember)
        : base(start, end)
    {
        Enum = @enum;
        EnumMember = enumMember;
        Namespace = @namespace;

        id = new PathTokenId(PathTokenType.Enum, string.Format("{0}:{1}.{2}", Namespace, @enum.Name, EnumMember));
    }
}
