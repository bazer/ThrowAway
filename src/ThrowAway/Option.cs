namespace ThrowAway;

/// <summary>
/// Represents an empty struct used as a placeholder for void return types in Option.
/// </summary>
public struct VoidResult
{
}

/// <summary>
/// Provides static methods for creating and working with Option types.
/// This class serves as a utility for managing values that may or may not exist (similar to Nullable types),
/// and provides methods for handling potential failures in a more functional way.
/// </summary>
public static class Option
{
    private static readonly VoidResult voidValue = new();

    /// <summary>
    /// Creates an Option instance representing a void result.
    /// </summary>
    /// <returns>An Option of VoidResult indicating a successful void operation.</returns>
    public static Option<VoidResult> Void() =>
        Some(voidValue);

    /// <summary>
    /// Wraps a non-null value into an Option, indicating a successful operation.
    /// </summary>
    /// <typeparam name="T">The type of the value to be wrapped.</typeparam>
    /// <param name="value">The non-null value to wrap.</param>
    /// <returns>An Option of the specified type containing the provided value.</returns>
    public static Option<T> Some<T>(T value) =>
        Option<T>.Some(value);

    /// <summary>
    /// Creates an Option representing a failure with a string reason.
    /// </summary>
    /// <param name="reason">The reason for the failure.</param>
    /// <returns>An Option of string indicating the failure reason.</returns>
    public static Option<string> Fail(string reason) =>
        Option<string>.Fail(reason);

    /// <summary>
    /// Creates an Option of a specified type, representing a failure with a string reason.
    /// </summary>
    /// <typeparam name="T">The type of the Option to create.</typeparam>
    /// <param name="reason">The reason for the failure.</param>
    /// <returns>An Option of the specified type indicating the failure reason.</returns>
    public static Option<T> Fail<T>(string reason) =>
        Option<T>.Fail(reason);

    /// <summary>
    /// Creates an Option instance with a success value and a failure type.
    /// </summary>
    /// <typeparam name="V">The success type of the Option.</typeparam>
    /// <typeparam name="F">The failure type of the Option.</typeparam>
    /// <param name="value">The success value.</param>
    /// <returns>An Option with the specified success and failure types.</returns>
    public static Option<V, F> Some<V, F>(V value) =>
        Option<V, F>.Some(value);

    /// <summary>
    /// Creates an Option instance representing a failure with a custom failure type.
    /// </summary>
    /// <typeparam name="V">The success type of the Option.</typeparam>
    /// <typeparam name="F">The failure type of the Option.</typeparam>
    /// <param name="failure">The failure value.</param>
    /// <returns>An Option with the specified success and failure types indicating the failure.</returns>
    public static Option<V, F> Fail<V, F>(F failure) =>
        Option<V, F>.Fail(failure);

    /// <summary>
    /// Attempts to execute a function that returns an Option and captures any failure.
    /// </summary>
    /// <typeparam name="V">The success type of the Option returned by the function.</typeparam>
    /// <param name="func">The function to execute.</param>
    /// <returns>The result of the function or a failure if an exception occurs.</returns>
    public static Option<V> CatchFailure<V>(Func<Option<V>> func)
    {
        try
        {
            return func();
        }
        catch (HasFailedException<string> e)
        {
            return e.Failure;
        }
    }

    /// <summary>
    /// Attempts to execute a function that returns an Option and captures any failure.
    /// </summary>
    /// <typeparam name="V">The success type of the Option returned by the function.</typeparam>
    /// <param name="func">The function to execute.</param>
    /// <returns>The result of the function or a failure if an exception occurs.</returns>
    public static Option<V> CatchAll<V>(Func<Option<V>> func)
    {
        try
        {
            return func();
        }
        catch (HasFailedException<string> e)
        {
            return e.Failure;
        }
        catch (Exception e)
        {
            return Option<V>.Fail(e.Message, e.StackTrace);
        }
    }

    /// <summary>
    /// Attempts to execute a function that returns an Option with custom failure type and captures any failure.
    /// </summary>
    /// <typeparam name="V">The success type of the Option returned by the function.</typeparam>
    /// <typeparam name="F">The failure type of the Option.</typeparam>
    /// <param name="func">The function to execute.</param>
    /// <returns>The result of the function or a custom failure if an exception occurs.</returns>
    public static Option<V, F> CatchFailure<V, F>(Func<Option<V, F>> func)
    {
        try
        {
            return func();
        }
        catch (HasFailedException<F> e)
        {
            return e.Failure!;
        }
    }

    /// <summary>
    /// Attempts to execute a function that returns an Option and captures any failure.
    /// </summary>
    /// <typeparam name="V">The success type of the Option returned by the function.</typeparam>
    /// <typeparam name="F">The failure type of the Option.</typeparam>
    /// <param name="func">The function to execute.</param>
    /// <param name="exceptionMapping">Mapping of the Exception message to the failure type F</param>
    /// <returns>The result of the function or a failure if an exception occurs.</returns>
    public static Option<V, F> CatchAll<V, F>(Func<Option<V, F>> func, Func<string, F> exceptionMapping)
    {
        if (Helpers.IsNull(exceptionMapping))
            throw new ArgumentNullException(nameof(exceptionMapping));

        try
        {
            return func();
        }
        catch (HasFailedException<F> e)
        {
            return e.Failure;
        }
        catch (Exception e)
        {
            var message = exceptionMapping(e.Message ?? "Exception has no message");

            return Option<V, F>.Fail(message, e.StackTrace);
        }
    }
}