namespace ThrowAway.Extensions;

/// <summary>
/// Provides extension methods for returning the current Option if it is in a successful state; otherwise, returns an alternative 
/// Option provided.
/// </summary>
public static class EitherElseExtensions
{
    /// <summary>
    /// Returns the current Option if it is in a successful state; otherwise, returns an alternative 
    /// Option provided. This method is useful in scenarios where you want to provide a fallback Option 
    /// in cases where the original Option is in a failed state. It allows for easy chaining of fallbacks, 
    /// enabling more complex conditional logic in a concise manner.
    /// </summary>
    /// <param name="option">The Option instance.</param>
    /// <param name="alternativeOption">The alternative Option to return if the current Option is in a failed state.</param>
    /// <returns>The current Option if it is in a successful state; otherwise, the alternative Option.</returns>
    public static Option<V, F> Else<V, F>(this Option<V, F> option, Option<V, F> alternativeOption) =>
        option.HasValue
        ? option
        : alternativeOption;


    /// <summary>
    /// Returns the current Option if it is in a successful state; otherwise, invokes a factory function 
    /// to provide an alternative Option. This method adds flexibility by allowing the alternative Option 
    /// to be dynamically created based on the current context. It is particularly useful when the 
    /// alternative Option is dependent on conditions or operations that should only be evaluated if 
    /// the original Option is in a failed state.
    /// </summary>
    /// <param name="option">The Option instance.</param>
    /// <param name="alternativeOptionFactory">A function that produces an alternative Option when the 
    /// current Option is in a failed state.</param>
    /// <returns>The current Option if it is in a successful state; otherwise, the Option produced by 
    /// the alternativeOptionFactory.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the alternativeOptionFactory is null.</exception>
    public static Option<V, F> Else<V, F>(this Option<V, F> option, Func<Option<V, F>> alternativeOptionFactory)
    {
        if (alternativeOptionFactory == null)
            throw new ArgumentNullException(nameof(alternativeOptionFactory));

        return option.HasValue
            ? option
            : alternativeOptionFactory();
    }
}
