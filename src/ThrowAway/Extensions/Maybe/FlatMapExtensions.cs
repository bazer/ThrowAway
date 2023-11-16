﻿using static ThrowAway.Helpers;
using static ThrowAway.Option;

namespace ThrowAway.Extensions;

/// <summary>
/// Provides extension methods for performing FlatMap operations on Option types.
/// FlatMap methods are essential in functional programming for chaining operations 
/// that return Options, allowing for streamlined composition of multiple potentially 
/// failing operations.
/// </summary>
public static class MaybeFlatMapExtensions
{
    /// <summary>
    /// Transforms the contained value of an Option&lt;V&gt; using a specified mapping function that returns Option&lt;T&gt;
    /// if the original Option is in a successful state. This variant of FlatMap allows for transformations to a different
    /// type of Option without introducing a new failure type. If the original Option is in a failed state, the failure is
    /// propagated without applying the mapping function.
    /// </summary>
    /// <typeparam name="V">The type of the value in the original Option.</typeparam>
    /// <typeparam name="T">The type of the value in the resulting Option.</typeparam>
    /// <param name="option">The original Option instance.</param>
    /// <param name="mapping">A function to apply to the Option's value if it is in a successful state.</param>
    /// <returns>An Option of type T resulting from applying the mapping function to the original Option's value,
    /// if it is in a successful state. If the original Option is in a failed state, returns an Option indicating the failure.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the mapping function is null.</exception>
    public static Option<T> FlatMap<V, T>(this Option<V> option, [DisallowNull] Func<V, Option<T>> mapping)
    {
        if (IsNull(mapping))
            throw new ArgumentNullException(nameof(mapping));

        return option.Match(
            some: mapping,
            Fail<T>);
    }
}