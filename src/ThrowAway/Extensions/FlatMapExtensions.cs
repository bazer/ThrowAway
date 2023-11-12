using System;
using System.Diagnostics.CodeAnalysis;
using static ThrowAway.Helpers;
using static ThrowAway.Option;

namespace ThrowAway;

/// <summary>
/// Provides extension methods for performing FlatMap operations on Option types.
/// FlatMap methods are essential in functional programming for chaining operations 
/// that return Options, allowing for streamlined composition of multiple potentially 
/// failing operations.
/// </summary>
public static class FlatMapExtensions
{
    /// <summary>
    /// Transforms the contained value of an Option&lt;V&gt; using a specified mapping function that returns Option&lt;T, F&gt;
    /// if the original Option is in a successful state. If the original Option is in a failed state, the failure is
    /// propagated without applying the mapping function. This method is essential for chaining operations on Option
    /// values while maintaining the ability to handle failures in a consistent manner.
    /// </summary>
    /// <typeparam name="V">The type of the value in the original Option.</typeparam>
    /// <typeparam name="F">The type of the failure in the resulting Option.</typeparam>
    /// <typeparam name="T">The type of the value in the resulting Option.</typeparam>
    /// <param name="option">The original Option instance.</param>
    /// <param name="mapping">A function to apply to the Option's value if it is in a successful state.</param>
    /// <returns>An Option of type T, F resulting from applying the mapping function to the original Option's value,
    /// if it is in a successful state. If the original Option is in a failed state, returns an Option indicating the failure.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the mapping function is null.</exception>
    public static Option<T> FlatMap<V, F, T>(this Option<V> option, [DisallowNull] Func<V, Option<T, F>> mapping)
    {
        if (IsNull(mapping))
            throw new ArgumentNullException(nameof(mapping));

        return option.FlatMap(some => mapping(some));
    }

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

    /// <summary>
    /// Transforms the contained value of the Option using a specified mapping function if it is in a successful state.
    /// If the Option is in a successful state, the mapping function is applied to the value, and 
    /// the result (which must also be an Option of potentially a different type) is returned. If the Option is in a 
    /// failed state, the failure is propagated without applying the mapping function. This method is essential for 
    /// chaining operations on Option values while maintaining the ability to handle failures in a consistent manner.
    /// </summary>
    /// <typeparam name="V">The type of the value in the original Option.</typeparam>
    /// <typeparam name="F">The type of the failure in the original Option.</typeparam>
    /// <typeparam name="T">The type of the value in the new Option returned after applying the mapping function.</typeparam>
    /// <param name="option">The original Option instance.</param>
    /// <param name="mapping">A function to apply to the Option's value if it is in a successful state. The function
    /// should return an Option of the new type.</param>
    /// <returns>An Option of type T, F resulting from applying the mapping function to the current Option's value,
    /// if it is in a successful state. If the current Option is in a failed state, returns an Option indicating the 
    /// failure, with the failure type F.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the mapping function is null.</exception>
    public static Option<T, F> FlatMap<V, F, T>(this Option<V, F> option, Func<V, Option<T, F>> mapping)
    {
        if (mapping == null)
            throw new ArgumentNullException(nameof(mapping));

        return option.Match(
            some: mapping,
            fail => Fail<T, F>(fail!));
    }
}
