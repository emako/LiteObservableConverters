using System;
using System.Runtime.Serialization;
using LiteObservableConverters.DynamicExpresso.Resources;

namespace LiteObservableConverters.DynamicExpresso.Exceptions;

[Serializable]
public class AssignmentOperatorDisabledException : ParseException
{
    public AssignmentOperatorDisabledException(string operatorString, int position)
        : base(string.Format(ErrorMessages.AssignmentOperatorNotAllowed, operatorString), position)
    {
        OperatorString = operatorString;
    }

    public string? OperatorString { get; private set; }

    protected AssignmentOperatorDisabledException(
        SerializationInfo info,
        StreamingContext context)
        : base(info, context)
    {
        OperatorString = info.GetString("OperatorString");
    }

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CS0672 // Member overrides obsolete member
#pragma warning disable SYSLIB0051 // Type or member is obsolete

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("OperatorString", OperatorString);

        base.GetObjectData(info, context);
    }

#pragma warning restore SYSLIB0051 // Type or member is obsolete
#pragma warning restore CS0672 // Member overrides obsolete member
#pragma warning restore IDE0079 // Remove unnecessary suppression
}
