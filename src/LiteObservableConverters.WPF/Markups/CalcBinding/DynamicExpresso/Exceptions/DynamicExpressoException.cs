using System;
using System.Runtime.Serialization;

namespace LiteObservableConverters.DynamicExpresso.Exceptions;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SYSLIB0051 // Type or member is obsolete

[Serializable]
#pragma warning restore IDE0079 // Remove unnecessary suppression
public class DynamicExpressoException : Exception
{
    public DynamicExpressoException()
    {
    }

    public DynamicExpressoException(string message) : base(message)
    {
    }

    public DynamicExpressoException(string message, Exception inner) : base(message, inner)
    {
    }

    protected DynamicExpressoException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
