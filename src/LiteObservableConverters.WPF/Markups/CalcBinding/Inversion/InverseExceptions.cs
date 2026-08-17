using System;
using System.Runtime.Serialization;

namespace LiteObservableConverters.CalcBinding.Inversion;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SYSLIB0051 // Type or member is obsolete

[Serializable]
public class InverseException : Exception
{
    public InverseException()
    {
    }

    public InverseException(string message) : base(message)
    {
    }

    public InverseException(string message, Exception inner) : base(message, inner)
    {
    }

    protected InverseException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
