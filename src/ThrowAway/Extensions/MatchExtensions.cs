using System;
using System.Diagnostics.CodeAnalysis;
using static ThrowAway.Helpers;

namespace ThrowAway;

/// <summary>
/// Provides extension methods for performing Match operations on Option types.
/// Match methods are essential for handling the two possible states (success or failure)
/// of an Option in a functional and expressive manner. They allow for the implementation
/// of conditional logic based on the state of the Option without explicitly checking its state.
/// </summary>
public static class MatchExtensions
{
    /// <summary>
    /// Matches the state of an Option&lt;V&gt; and executes a corresponding function based on that state.
    /// If the Option is in a successful state, the 'some' function is executed with its value. 
    /// If the Option is in a failed state, the 'fail' function is executed with the failure reason.
    /// This method allows for concise and expressive handling of both success and failure scenarios.
    /// </summary>
    /// <typeparam name="V">The type of the value in the Option.</typeparam>
    /// <typeparam name="T">The return type of the functions.</typeparam>
    /// <param name="option">The Option instance.</param>
    /// <param name="some">A function to execute if the Option is in a successful state.</param>
    /// <param name="fail">A function to execute if the Option is in a failed state.</param>
    /// <returns>The result of the executed function based on the state of the Option.</returns>
    /// <exception cref="ArgumentNullException">Thrown if either the 'some' or 'fail' function is null.</exception>
    public static T Match<V, T>(this Option<V> option, [DisallowNull] Func<V, T> some, [DisallowNull] Func<string, T> fail)
    {
        if (IsNull(some))
            throw new ArgumentNullException(nameof(some));

        if (IsNull(fail))
            throw new ArgumentNullException(nameof(fail));

        return option.HasValue
            ? some(option.Value)
            : fail(option.Failure);
    }

    /// <summary>
    /// Matches the state of an Option&lt;V, F&gt; and executes a corresponding function based on that state.
    /// This variant of Match allows for handling Options with custom failure types. The 'some' function is
    /// executed if the Option is successful, while the 'fail' function is executed if the Option is failed.
    /// </summary>
    /// <typeparam name="V">The type of the value in the Option.</typeparam>
    /// <typeparam name="F">The type of the failure in the Option.</typeparam>
    /// <typeparam name="T">The return type of the functions.</typeparam>
    /// <param name="option">The Option instance.</param>
    /// <param name="some">A function to execute if the Option is in a successful state.</param>
    /// <param name="fail">A function to execute if the Option is in a failed state.</param>
    /// <returns>The result of the executed function based on the state of the Option.</returns>
    /// <exception cref="ArgumentNullException">Thrown if either the 'some' or 'fail' function is null.</exception>
    public static T Match<V, F, T>(this Option<V, F> option, [DisallowNull] Func<V, T> some, [DisallowNull] Func<F, T> fail)
    {
        if (IsNull(some))
            throw new ArgumentNullException(nameof(some));

        if (IsNull(fail))
            throw new ArgumentNullException(nameof(fail));

        return option.HasValue
            ? some(option.Value)
            : fail(option.Failure);
    }

    /// <summary>
    /// Matches the state of an Option&lt;V, F&gt; and executes a corresponding action based on that state.
    /// This variant of Match executes actions rather than returning a value, allowing for side effects 
    /// based on the state of the Option. The 'some' action is executed if the Option is successful, 
    /// while the 'fail' action is executed if the Option is failed.
    /// </summary>
    /// <typeparam name="V">The type of the value in the Option.</typeparam>
    /// <typeparam name="F">The type of the failure in the Option.</typeparam>
    /// <param name="option">The Option instance.</param>
    /// <param name="some">An action to execute if the Option is in a successful state.</param>
    /// <param name="fail">An action to execute if the Option is in a failed state.</param>
    /// <exception cref="ArgumentNullException">Thrown if either the 'some' or 'fail' action is null.</exception>
    public static void Match<V, F>(this Option<V, F> option, [DisallowNull] Action<V> some, [DisallowNull] Action<F> fail)
    {
        if (IsNull(some))
            throw new ArgumentNullException(nameof(some));

        if (IsNull(fail))
            throw new ArgumentNullException(nameof(fail));

        if (option.HasValue)
            some(option.Value);
        else
            fail(option.Failure);
    }
}
