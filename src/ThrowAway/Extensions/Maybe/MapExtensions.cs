using static ThrowAway.Helpers;
using static ThrowAway.Option;

namespace ThrowAway.Extensions;

/// <summary>
/// Provides extension methods for performing Map operations on Option types.
/// Map methods are crucial in functional programming for transforming the value contained in an Option,
/// allowing for operations that change the type or nature of the value while preserving the Option context.
/// </summary>
public static class MaybeMapExtensions
{
    /// <summary>
    /// Transforms the contained value of an Option&lt;V&gt; to Option&lt;T&gt; using a specified mapping function
    /// if the Option is in a successful state. If the Option is in a failed state, the failure is
    /// propagated without applying the mapping function. This method is key for applying transformations
    /// to the values within Options, enabling the seamless evolution of data types within the functional flow.
    /// </summary>
    /// <typeparam name="V">The type of the value in the original Option.</typeparam>
    /// <typeparam name="T">The type of the value in the resulting Option.</typeparam>
    /// <param name="option">The original Option instance.</param>
    /// <param name="mapping">A function to apply to the Option's value if it is in a successful state.</param>
    /// <returns>An Option of type T resulting from applying the mapping function to the original Option's value,
    /// if it is in a successful state. If the original Option is in a failed state, returns an Option indicating the failure.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the mapping function is null.</exception>
    public static Option<T> Map<V, T>(this Option<V> option, [DisallowNull] Func<V, T> mapping)
    {
        if (IsNull(mapping))
            throw new ArgumentNullException(nameof(mapping));

        return option.Match(
            some => Some(mapping(some)!),
            fail => Fail<T>(fail));
    }

    /// <summary>
    /// Transforms the failure of an Option&lt;V&gt; to Option&lt;V, T&gt; using a specified mapping function
    /// if the Option is in a failed state. This method is useful for changing the type of failure
    /// represented by the Option, allowing for more flexible error handling and failure representation.
    /// </summary>
    /// <typeparam name="V">The type of the value in the original Option.</typeparam>
    /// <typeparam name="T">The type of the failure in the resulting Option.</typeparam>
    /// <param name="option">The original Option instance.</param>
    /// <param name="mapping">A function to apply to the Option's failure if it is in a failed state.</param>
    /// <returns>An Option of type V, T resulting from applying the mapping function to the original Option's failure,
    /// if it is in a failed state. If the original Option is in a successful state, returns the unchanged Option.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the mapping function is null.</exception>
    public static Option<V, T> MapFail<V, T>(this Option<V> option, [DisallowNull] Func<string, T> mapping)
    {
        if (IsNull(mapping))
            throw new ArgumentNullException(nameof(mapping));

        return option.Match(
            some => Option<V, T>.Some(some!),
            fail => Option<V, T>.Fail(mapping(fail)!));
    }
}
