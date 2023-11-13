namespace ThrowAway.Extensions;

/// <summary>
/// Provides extension methods for filtering Option types when they are in a successful state.
/// </summary>
public static class EitherFilterExtensions
{
    /// <summary>
    /// Filters the Option based on a provided condition. If the Option is in a successful state and the value 
    /// does not satisfy the specified condition, the method returns a new Option in a failed state with the 
    /// provided failure reason. If the Option is already in a failed state or if the value satisfies the condition,
    /// the original Option is returned unchanged. This method is useful for applying conditional checks to the 
    /// value within an Option in a functional way, allowing for the continuation or alteration of the Option's 
    /// state based on specific criteria.
    /// </summary>
    /// <param name="option">The Option instance.</param>
    /// <param name="condition">A predicate to apply to the Option's value if it is in a successful state.</param>
    /// <param name="failure">The failure reason to be used if the condition is not satisfied by the value.</param>
    /// <returns>An Option that is either the original Option if it is in a failed state or if the value satisfies 
    /// the condition, or a new failed Option with the provided failure reason if the condition is not satisfied.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the condition is null.</exception>
    public static Option<V, F> Filter<V, F>(this Option<V, F> option, [DisallowNull] Func<V, bool> condition, [DisallowNull] F failure)
    {
        return option.HasValue && !condition(option.Value)
            ? Option.Fail<V, F>(failure)
            : option;
    }
}
