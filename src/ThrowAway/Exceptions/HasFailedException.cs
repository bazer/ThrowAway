using System;
using System.Diagnostics.CodeAnalysis;

namespace ThrowAway;

/// <summary>
/// Represents a base exception indicating that an operation represented by an Option has failed.
/// This exception serves as a mechanism to throw an exception in contexts where an Option's failure
/// is considered an exceptional case. It encapsulates a Failure object providing details about the failure.
/// </summary>
public class HasFailedException : ThrowAwayException
{
    /// <summary>
    /// Gets the Failure object associated with this exception.
    /// </summary>
    public Failure<object> Failure { get; }

    /// <summary>
    /// Initializes a new instance of the HasFailedException class with a specific message and Failure object.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="value">The Failure object associated with this exception.</param>
    public HasFailedException([DisallowNull] string message, [DisallowNull] Failure<object> value) : base($"{message} '{value}'")
    {
        if (Helpers.IsNull(message))
            throw new ArgumentNullException(nameof(message));

        if (Helpers.IsNull(value))
            throw new ArgumentNullException(nameof(value));

        Failure = value;
    }
}

/// <summary>
/// Represents a generic exception indicating that an operation represented by an Option&lt;F&gt; has failed.
/// This generic version allows for specifying the type of the failure, providing a strongly-typed Failure object.
/// </summary>
/// <typeparam name="F">The type of the failure.</typeparam>
public class HasFailedException<F> : HasFailedException
{
    /// <summary>
    /// Initializes a new instance of the HasFailedException&lt;F&gt; class with a specific message and Failure&lt;F&gt; object.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="failure">The Failure&lt;F&gt; object associated with this exception.</param>
    public HasFailedException([DisallowNull] string message, [DisallowNull] Failure<F> failure) : base(message, (Failure<object>)failure)
    {
    }

    /// <summary>
    /// Gets the strongly-typed Failure object associated with this exception.
    /// </summary>
    new public Failure<F> Failure => (Failure<F>)base.Failure;
}
