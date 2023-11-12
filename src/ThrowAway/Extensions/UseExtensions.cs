using System;

namespace ThrowAway.Extensions;

/// <summary>
/// Provides extension methods for using Option types when they are in a successful state.
/// </summary>
public static class UseExtensions
{
    /// <summary>
    /// Performs a specified action on the value of an Option if the Option is in a successful state.
    /// This method allows side-effects to be applied to the Option's value without altering the Option's state.
    /// </summary>
    /// <typeparam name="V">The type of the value in the Option.</typeparam>
    /// <typeparam name="F">The type of the failure.</typeparam>
    /// <param name="option">The Option instance.</param>
    /// <param name="action">The action to perform on the Option's value.</param>
    /// <returns>The original Option, unmodified.</returns>
    public static Option<V, F> Use<V, F>(this Option<V, F> option, Action<V> action)
    {
        option.Match(
            some: v => action(v),
            fail: _ => { /* Do nothing on failure */ }
        );
        return option;
    }
}
