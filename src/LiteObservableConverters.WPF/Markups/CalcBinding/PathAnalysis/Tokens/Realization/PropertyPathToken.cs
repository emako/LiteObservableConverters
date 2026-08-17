using System;
using System.Collections.Generic;
using System.Linq;

namespace CalcBinding.PathAnalysis;

public class PropertyPathToken : PathToken
{
    public IEnumerable<string> Properties { get; private set; }

    private readonly PathTokenId id;

    public override PathTokenId Id => id;

    public PropertyPathToken(int start, int end, IEnumerable<string> properties)
        : base(start, end)
    {
        Properties = [.. properties];
        id = new PathTokenId(PathTokenType.Property, string.Join(".", Properties));
    }
}
