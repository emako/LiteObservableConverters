// Polyfill for C# tuple syntax on .NET Framework (net462+), avoiding the System.ValueTuple NuGet package.

#if NETFRAMEWORK

namespace System;

internal struct ValueTuple<T1, T2>(T1 item1, T2 item2)
{
    public T1 Item1 = item1;
    public T2 Item2 = item2;

    public readonly void Deconstruct(out T1 item1, out T2 item2)
    {
        item1 = Item1;
        item2 = Item2;
    }
}

#endif
