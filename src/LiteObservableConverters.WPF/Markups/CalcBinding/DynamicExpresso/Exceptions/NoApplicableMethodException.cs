using System;
using System.Runtime.Serialization;
using LiteObservableConverters.DynamicExpresso.Resources;

namespace LiteObservableConverters.DynamicExpresso.Exceptions;

[Serializable]
public class NoApplicableMethodException : ParseException
{
    public NoApplicableMethodException(string methodName, string methodTypeName, int position)
        : base(string.Format(ErrorMessages.InvalidMethodCall2, methodName, methodTypeName), position)
    {
        MethodTypeName = methodTypeName;
        MethodName = methodName;
    }

    public string MethodTypeName { get; private set; } = null!;
    public string MethodName { get; private set; } = null!;

    protected NoApplicableMethodException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        MethodTypeName = info.GetString("MethodTypeName")!;
        MethodName = info.GetString("MethodName")!;
    }

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CS0672 // Member overrides obsolete member
#pragma warning disable SYSLIB0051 // Type or member is obsolete

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("MethodName", MethodName);
        info.AddValue("MethodTypeName", MethodTypeName);

        base.GetObjectData(info, context);
    }

#pragma warning restore SYSLIB0051 // Type or member is obsolete
#pragma warning restore CS0672 // Member overrides obsolete member
#pragma warning restore IDE0079 // Remove unnecessary suppression
}
