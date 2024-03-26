using System.Runtime.CompilerServices;

namespace WildberriesUserClient.Extensions;

/// <summary>
/// Extension Methods
/// </summary>
internal static class ObjectExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static T ThrowIfNull<T>(
        this T? value,
        [CallerArgumentExpression(nameof(value))] string? parameterName = default
    ) =>
        value ?? throw new ArgumentNullException(parameterName);
}