using System;
using System.Diagnostics.CodeAnalysis;

namespace ThrowAway
{
    /// <summary>
    /// Provides extension methods for working with Option types. These methods enhance the usability
    /// and flexibility of the Option type by providing additional ways to create and manipulate Option instances.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Creates an Option instance from a value if it is not null; otherwise, returns a failure Option.
        /// This method simplifies the creation of Option instances in scenarios where null values should be treated as failures.
        /// </summary>
        /// <typeparam name="V">The type of the value.</typeparam>
        /// <typeparam name="F">The type of the failure.</typeparam>
        /// <param name="value">The value to be used for creating the Option.</param>
        /// <param name="failure">The failure to use if the value is null.</param>
        /// <returns>An Option representing success if the value is not null; otherwise, a failure Option.</returns>
        public static Option<V, F> SomeNotNull<V, F>(this V value, [DisallowNull] F failure) =>
            value.SomeWhen(val => val != null, failure);

        /// <summary>
        /// Creates an Option instance from a value based on a predicate; if the predicate returns true, a success Option is created;
        /// otherwise, a failure Option is returned. This method is useful for creating Option instances conditionally.
        /// </summary>
        /// <typeparam name="V">The type of the value.</typeparam>
        /// <typeparam name="F">The type of the failure.</typeparam>
        /// <param name="value">The value to be used for creating the Option.</param>
        /// <param name="predicate">A function that evaluates the value and returns true or false.</param>
        /// <param name="failure">The failure to use if the predicate returns false.</param>
        /// <returns>An Option representing success if the predicate returns true; otherwise, a failure Option.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the predicate is null.</exception>
        public static Option<V, F> SomeWhen<V, F>(this V value, [DisallowNull] Func<V, bool> predicate, [DisallowNull] F failure)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return predicate(value)
                ? Option.Some<V, F>(value!)
                : Option.Fail<V, F>(failure);
        }

        /// <summary>
        /// Retrieves the value of an Option or throws a HasFailedException if the Option is in a failed state.
        /// This method is useful for cases where the absence of a value should be treated as an exceptional scenario.
        /// </summary>
        /// <typeparam name="V">The type of the value.</typeparam>
        /// <typeparam name="F">The type of the failure.</typeparam>
        /// <param name="option">The Option instance.</param>
        /// <returns>The value of the Option if it is in a successful state.</returns>
        public static V ValueOrFailure<V, F>(this Option<V, F> option)
            => option.Value;

        /// <summary>
        /// Retrieves the value of an Option or throws a HasFailedException with a custom message if the Option is in a failed state.
        /// This method allows for custom failure messages, providing more context in exceptional scenarios.
        /// </summary>
        /// <typeparam name="V">The type of the value.</typeparam>
        /// <typeparam name="F">The type of the failure.</typeparam>
        /// <param name="option">The Option instance.</param>
        /// <param name="failureMessage">The custom failure message to use in the exception.</param>
        /// <returns>The value of the Option if it is in a successful state.</returns>
        public static V ValueOrFailure<V, F>(this Option<V, F> option, string failureMessage)
        {
            if (option.HasValue)
                return option.Value;

            throw new HasFailedException<F>(failureMessage, option.Failure);
        }

        /// <summary>
        /// Retrieves the value of an Option or throws a HasFailedException with a message derived from the failure reason
        /// if the Option is in a failed state. This method allows for dynamic failure messages based on the failure reason.
        /// </summary>
        /// <typeparam name="V">The type of the value.</typeparam>
        /// <typeparam name="F">The type of the failure.</typeparam>
        /// <param name="option">The Option instance.</param>
        /// <param name="failureFunc">A function that generates a failure message based on the failure reason.</param>
        /// <returns>The value of the Option if it is in a successful state.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the failureFunc is null.</exception>
        public static V ValueOrFailure<V, F>(this Option<V, F> option, Func<F, string> failureFunc)
        {
            if (failureFunc == null)
                throw new ArgumentNullException(nameof(failureFunc));

            if (option.HasValue)
                return option.Value;

            throw new HasFailedException<F>(failureFunc(option.Failure), option.Failure);
        }

        /// <summary>
        /// Retrieves the value of an Option or the default value of type V if the Option is in a failed state.
        /// This method provides a simple way to obtain a default value in case of a failure, avoiding exceptions.
        /// </summary>
        /// <typeparam name="V">The type of the value.</typeparam>
        /// <typeparam name="F">The type of the failure.</typeparam>
        /// <param name="option">The Option instance.</param>
        /// <returns>The value of the Option if it is in a successful state; otherwise, the default value of type V.</returns>
        public static V ValueOrDefault<V, F>(this Option<V, F> option)
        {
            if (option.HasValue)
                return option.Value;

            return default!;
        }
    }
}
