using System.Collections.Generic;

namespace LiteObservableConverters.DynamicExpresso;

public class IdentifiersInfo(
    IEnumerable<string> unknownIdentifiers,
    IEnumerable<Identifier> identifiers,
    IEnumerable<ReferenceType> types)
{
    public IEnumerable<string> UnknownIdentifiers { get; private set; } = [.. unknownIdentifiers];
    public IEnumerable<Identifier> Identifiers { get; private set; } = [.. identifiers];
    public IEnumerable<ReferenceType> Types { get; private set; } = [.. types];
}
