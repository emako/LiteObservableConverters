using System;

namespace LiteObservableConverters.Services;

internal interface ITimeZoneInfo
{
    public TimeZoneInfo Utc { get; }

    public TimeZoneInfo Local { get; }
}
