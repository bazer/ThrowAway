using System;
using System.Diagnostics.CodeAnalysis;

namespace ThrowAway;

/// <summary>
/// Represents an exception indicating that an operation represented by an Option unexpectedly has a value.
/// This exception is thrown in contexts where the absence of a value is expected, and the presence of a value
/// is considered an exceptional case. It encapsulates the unexpected value, providing details for debugging and error handling.
/// </summary>
public class HasValueException : ThrowAwayException
{
    /// <summary>
    /// Gets the unexpected value associated with this exception.
    /// </summary>
    public object Value { get; }

    /// <summary>
    /// Initializes a new instance of the HasValueException class with a specific message and the unexpected value.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="value">The unexpected value associated with this exception.</param>
    public HasValueException([DisallowNull] string message, [DisallowNull] object value) : base($"{message} '{value}'")
    {
        if (Helpers.IsNull(message))
            throw new ArgumentNullException(nameof(message));

        if (Helpers.IsNull(value))
            throw new ArgumentNullException(nameof(value));

        Value = value;
    }
}

/// <summary>
/// Represents a generic exception indicating that an operation represented by an Option&lt;T&gt; unexpectedly has a value.
/// This generic version allows for specifying the type of the value, providing a strongly-typed value object.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
public class HasValueException<T> : HasValueException
{
    /// <summary>
    /// Initializes a new instance of the HasValueException&lt;T&gt; class with a specific message and the unexpected value.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="value">The unexpected value of type T associated with this exception.</param>
    public HasValueException([DisallowNull] string message, [DisallowNull] T value) : base(message, value)
    {
    }

    /// <summary>
    /// Gets the strongly-typed value associated with this exception.
    /// </summary>
    public new T Value => (T)base.Value;
}
