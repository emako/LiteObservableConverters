using System;
using System.Runtime.Serialization;
using LiteObservableConverters.DynamicExpresso.Resources;

namespace LiteObservableConverters.DynamicExpresso.Exceptions;

[Serializable]
public class ParseException : DynamicExpressoException
{
    public ParseException(string message, int position)
        : base(string.Format(ErrorMessages.Format, message, position))
    {
        Position = position;
    }

    public ParseException(string message, int position, Exception innerException)
        : base(string.Format(ErrorMessages.Format, message, position), innerException)
    {
        Position = position;
    }

    public int Position { get; private set; }

    public static ParseException Create(int pos, string format, params object[] args)
    {
        return new ParseException(string.Format(format, args), pos);
    }

    protected ParseException(
        SerializationInfo info,
        StreamingContext context)
        : base(info, context)
    {
        Position = info.GetInt32("Position");
    }

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CS0672 // Member overrides obsolete member
#pragma warning disable SYSLIB0051 // Type or member is obsolete

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("Position", Position);

        base.GetObjectData(info, context);
    }

#pragma warning restore SYSLIB0051 // Type or member is obsolete
#pragma warning restore CS0672 // Member overrides obsolete member
#pragma warning restore IDE0079 // Remove unnecessary suppression
}
