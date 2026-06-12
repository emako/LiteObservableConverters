using System;

namespace LiteObservableConverters;

public class ComputedServiceProvider : IServiceProvider
{
    public static IServiceProvider? Shared { get; set; }

    public object? GetService(Type serviceType)
    {
        return Shared?.GetService(serviceType);
    }
}
