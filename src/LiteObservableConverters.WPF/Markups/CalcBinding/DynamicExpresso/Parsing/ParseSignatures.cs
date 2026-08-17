using System;
using System.Collections.Generic;
using System.Reflection;
using LiteObservableConverters.DynamicExpresso.Reflection;

namespace LiteObservableConverters.DynamicExpresso.Parsing;

/// <summary>
/// Contains all the signatures for the binary and unary operators supported by DynamicExpresso.
/// It allows to reuse the existing method resolutions logic in <see cref="Resolution.MethodResolution"/>.
/// </summary>
internal static class ParseSignatures
{
    private static MethodBase[] MakeUnarySignatures(params Type[] possibleOperandTypes)
    {
        var signatures = new MethodBase[possibleOperandTypes.Length];
        for (var i = 0; i < possibleOperandTypes.Length; i++)
        {
            signatures[i] = new SimpleMethodSignature(possibleOperandTypes[i]);
        }
        return signatures;
    }

    private static MethodBase[] MakeBinarySignatures(IList<(Type, Type)> possibleOperandTypes)
    {
        var signatures = new MethodBase[possibleOperandTypes.Count];
        for (var i = 0; i < possibleOperandTypes.Count; i++)
        {
            var (left, right) = possibleOperandTypes[i];
            signatures[i] = new SimpleMethodSignature(left, right);
        }
        return signatures;
    }

    /// <summary>
    /// Signatures for the binary logical operators.
    /// </summary>
    public static MethodBase[] LogicalSignatures = MakeBinarySignatures(
    [
        (typeof(bool),  typeof(bool) ),
        (typeof(bool?), typeof(bool?)),
    ]);

    /// <summary>
    /// Signatures for the binary arithmetic operators.
    /// </summary>
    public static MethodBase[] ArithmeticSignatures = MakeBinarySignatures(
    [
        (typeof(int),      typeof(int)     ),
        (typeof(uint),     typeof(uint)    ),
        (typeof(long),     typeof(long)    ),
        (typeof(ulong),    typeof(ulong)   ),
        (typeof(float),    typeof(float)   ),
        (typeof(double),   typeof(double)  ),
        (typeof(decimal),  typeof(decimal) ),
        (typeof(int?),     typeof(int?)    ),
        (typeof(uint?),    typeof(uint?)   ),
        (typeof(long?),    typeof(long?)   ),
        (typeof(ulong?),   typeof(ulong?)  ),
        (typeof(float?),   typeof(float?)  ),
        (typeof(double?),  typeof(double?) ),
        (typeof(decimal?), typeof(decimal?)),
    ]);

    /// <summary>
    /// Signatures for the binary relational operators.
    /// </summary>
    public static MethodBase[] RelationalSignatures =
    [
        .. ArithmeticSignatures,
        .. MakeBinarySignatures(
        [
            (typeof(string),    typeof(string)   ),
            (typeof(char),      typeof(char)     ),
            (typeof(DateTime),  typeof(DateTime) ),
            (typeof(TimeSpan),  typeof(TimeSpan) ),
            (typeof(char?),     typeof(char?)    ),
            (typeof(DateTime?), typeof(DateTime?)),
            (typeof(TimeSpan?), typeof(TimeSpan?)),
        ]),
    ];

    /// <summary>
    /// Signatures for the binary equality operators.
    /// </summary>
    public static MethodBase[] EqualitySignatures = [.. RelationalSignatures, .. LogicalSignatures];

    /// <summary>
    /// Signatures for the binary + operators.
    /// </summary>
    public static MethodBase[] AddSignatures =
    [
        .. ArithmeticSignatures,
        .. MakeBinarySignatures(
        [
            (typeof(DateTime),  typeof(TimeSpan) ),
            (typeof(TimeSpan),  typeof(TimeSpan) ),
            (typeof(DateTime?), typeof(TimeSpan?)),
            (typeof(TimeSpan?), typeof(TimeSpan?)),
        ]),
    ];

    /// <summary>
    /// Signatures for the binary - operators.
    /// </summary>
    public static MethodBase[] SubtractSignatures =
    [
        .. AddSignatures,
        .. MakeBinarySignatures(
        [
            (typeof(DateTime),  typeof(DateTime)),
            (typeof(DateTime?), typeof(DateTime?)),
        ]),
    ];

    /// <summary>
    /// Signatures for the unary - operators.
    /// </summary>
    public static MethodBase[] NegationSignatures = MakeUnarySignatures(
        typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal),
        typeof(int?), typeof(uint?), typeof(long?), typeof(ulong?), typeof(float?), typeof(double?), typeof(decimal?)
    );

    /// <summary>
    /// Signatures for the unary not (!) operator.
    /// </summary>
    public static MethodBase[] NotSignatures = MakeUnarySignatures(typeof(bool), typeof(bool?));

    /// <summary>
    /// Signatures for the bitwise completement operators.
    /// </summary>
    public static MethodBase[] BitwiseComplementSignatures = MakeUnarySignatures(
        typeof(int), typeof(uint), typeof(long), typeof(ulong),
        typeof(int?), typeof(uint?), typeof(long?), typeof(ulong?)
    );

    /// <summary>
    /// Signatures for the left and right shift operators.
    /// </summary>
    public static MethodBase[] ShiftSignatures = MakeBinarySignatures(
    [
        (typeof(int),   typeof(int) ),
        (typeof(uint),  typeof(int) ),
        (typeof(long),  typeof(int) ),
        (typeof(ulong), typeof(int) ),
        (typeof(int?),  typeof(int?) ),
        (typeof(uint?), typeof(int?) ),
        (typeof(long?), typeof(int?) ),
        (typeof(ulong?),typeof(int?) ),
    ]);
}
