using System;
using System.Runtime.Serialization;
using LiteObservableConverters.DynamicExpresso.Resources;

namespace LiteObservableConverters.DynamicExpresso.Exceptions;

[Serializable]
public class ReflectionNotAllowedException : ParseException
{
    public ReflectionNotAllowedException()
        : base(ErrorMessages.ReflectionNotAllowed, 0)
    {
    }

    protected ReflectionNotAllowedException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CS0672 // Member overrides obsolete member
#pragma warning disable SYSLIB0051 // Type or member is obsolete

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
    }

#pragma warning restore SYSLIB0051 // Type or member is obsolete
#pragma warning restore CS0672 // Member overrides obsolete member
#pragma warning restore IDE0079 // Remove unnecessary suppression
}
