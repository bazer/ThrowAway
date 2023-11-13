namespace ThrowAway.Extensions;

/// <summary>
/// Provides extension methods for using Option types when they are in a successful state.
/// </summary>
public static class MaybeUseExtensions
{
    /// <summary>
    /// Performs a specified action on the value of an Option if the Option is in a successful state.
    /// This method allows side-effects to be applied to the Option's value without altering the Option's state.
    /// </summary>
    /// <typeparam name="V">The type of the value in the Option.</typeparam>
    /// <param name="option">The Option instance.</param>
    /// <param name="action">The action to perform on the Option's value.</param>
    /// <returns>The original Option, unmodified.</returns>
    public static Option<V> Use<V>(this Option<V> option, Action<V> action)
    {
        option.Match(
            some: v => action(v),
            fail: _ => { /* Do nothing on failure */ }
        );
        return option;
    }
}
