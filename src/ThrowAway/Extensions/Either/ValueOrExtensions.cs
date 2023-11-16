namespace ThrowAway.Extensions;
/// <summary>
/// Provides extension methods for the Option class that allow for seamless value retrieval 
/// without explicit checks for the presence of the value.
/// </summary>
public static class EitherValueOrExtensions
{
    /// <summary>
    /// Retrieves the contained value of the Option if it is in a successful state; otherwise, returns 
    /// an alternative value provided. This method is useful for providing a default or fallback value 
    /// in cases where the Option does not contain a value, allowing for seamless value retrieval 
    /// without explicit checks for the presence of the value.
    /// </summary>
    /// <param name="option">The Option instance to retrieve the value from.</param>
    /// <param name="alternative">The alternative value to return if the Option is in a failed state.</param>
    /// <returns>The contained value if the Option is in a successful state; otherwise, the alternative value.</returns>
    public static V ValueOr<V, F>(this Option<V, F> option, V alternative) => option.HasValue
        ? option.Value
        : alternative;

    /// <summary>
    /// Retrieves the contained value of the Option if it is in a successful state; otherwise, invokes 
    /// a factory function to provide an alternative value. This method offers flexibility by deferring 
    /// the creation of the alternative value until it is needed, which can be beneficial for scenarios 
    /// where the alternative value is resource-intensive to create or compute.
    /// </summary>
    /// <param name="option">The Option instance to retrieve the value from.</param>
    /// <param name="alternativeFactory">A function that produces the alternative value when the Option 
    /// is in a failed state.</param>
    /// <returns>The contained value if the Option is in a successful state; otherwise, the value 
    /// produced by the alternativeFactory.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the option or alternativeFactory is null.</exception>
    public static V ValueOr<V, F>(this Option<V, F> option, Func<V> alternativeFactory)
    {
        if (alternativeFactory == null)
            throw new ArgumentNullException(nameof(alternativeFactory));

        return option.HasValue
            ? option.Value
            : alternativeFactory();
    }

    /// <summary>
    /// Retrieves the contained value of the Option if it is in a successful state; otherwise, invokes 
    /// a factory function with the failure reason to provide an alternative value. This variant of 
    /// ValueOr allows the alternative value to be determined based on the specific reason for the 
    /// Option's failure, providing a way to tailor the fallback value to the nature of the failure.
    /// </summary>
    /// <typeparam name="V">The type of the value contained in the Option.</typeparam>
    /// <typeparam name="F">The type of the failure reason contained in the Option.</typeparam>
    /// <param name="option">The Option instance to retrieve the value from.</param>
    /// <param name="alternativeFactory">A function that takes the failure reason and produces the 
    /// alternative value when the Option is in a failed state.</param>
    /// <returns>The contained value if the Option is in a successful state; otherwise, the value 
    /// produced by the alternativeFactory based on the failure reason.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the option or alternativeFactory is null.</exception>
    public static V ValueOr<V, F>(this Option<V, F> option, Func<F, V> alternativeFactory)
    {
        if (alternativeFactory == null)
            throw new ArgumentNullException(nameof(alternativeFactory));

        return option.HasValue
            ? option.Value
            : alternativeFactory(option.Failure);
    }
}