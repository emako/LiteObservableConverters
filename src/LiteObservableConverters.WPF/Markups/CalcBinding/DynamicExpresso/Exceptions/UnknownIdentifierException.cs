using System;
using System.Runtime.Serialization;
using LiteObservableConverters.DynamicExpresso.Resources;

namespace LiteObservableConverters.DynamicExpresso.Exceptions;

[Serializable]
public class UnknownIdentifierException : ParseException
{
    public UnknownIdentifierException(string identifier, int position)
        : base(string.Format(ErrorMessages.UnknownIdentifier, identifier), position)
    {
        Identifier = identifier;
    }

    public string Identifier { get; private set; } = null!;

    protected UnknownIdentifierException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        Identifier = info.GetString("Identifier")!;
    }

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CS0672 // Member overrides obsolete member
#pragma warning disable SYSLIB0051 // Type or member is obsolete

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("Identifier", Identifier);

        base.GetObjectData(info, context);
    }

#pragma warning restore SYSLIB0051 // Type or member is obsolete
#pragma warning restore CS0672 // Member overrides obsolete member
#pragma warning restore IDE0079 // Remove unnecessary suppression
}
