using static ThrowAway.Helpers;

namespace ThrowAway;

/// <summary>
/// Represents a failure with a specific type, encapsulating the failure information and optionally a stack trace.
/// This struct is fundamental in functional programming within this library, providing a structured way to handle
/// failures, errors, or exceptions. It allows for robust error handling and propagation, enabling clear and explicit
/// failure representation in a type-safe manner.
/// </summary>
/// <typeparam name="F">The type of the failure.</typeparam>
public readonly struct Failure<F>
{
    /// <summary>
    /// The failure value of type F, representing the specific failure information.
    /// </summary>
    public readonly F Value { get; }

    /// <summary>
    /// An optional stack trace associated with the failure, providing additional debugging information.
    /// </summary>
    public readonly string? StackTrace { get; }

    /// <summary>
    /// Initializes a new instance of the Failure struct with the specified failure value and optional stack trace.
    /// </summary>
    /// <param name="value">The failure value of type F.</param>
    /// <param name="stackTrace">The optional stack trace associated with the failure.</param>
    public Failure(F value, StackTrace? stackTrace)
    {
        if (IsNull(value))
            throw new ArgumentNullException(nameof(value));

        Value = value;
        StackTrace = stackTrace?.ToString();
    }

    internal Failure(F value, string? stackTrace)
    {
        if (IsNull(value))
            throw new ArgumentNullException(nameof(value));

        Value = value;
        StackTrace = stackTrace;
    }

    /// <summary>
    /// Provides a string representation of the failure, typically the string representation of the failure value.
    /// </summary>
    /// <returns>A string representation of the failure.</returns>
    public override string ToString() =>
        Value?.ToString() ?? "";

    /// <summary>
    /// Determines whether the specified object is equal to the current failure instance.
    /// Equality is based on the equality of the failure values.
    /// </summary>
    /// <param name="obj">The object to compare with the current failure instance.</param>
    /// <returns>true if the specified object is equal to the current failure instance; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        if (Value == null)
            return obj == null;

        if (obj is F v)
            return Value.Equals(v);

        if (obj is Failure<F> f)
            return Value.Equals(f.Value);

        return false;
    }

    /// <summary>
    /// Serves as the default hash function, returning the hash code of the failure value.
    /// </summary>
    /// <returns>A hash code for the current failure instance.</returns>
    public override int GetHashCode() => Value == null
        ? 0
        : Value.GetHashCode();

    /// <summary>
    /// Determines whether two Failure instances are equal.
    /// </summary>
    /// <param name="left">The first Failure instance to compare.</param>
    /// <param name="right">The second Failure instance to compare.</param>
    /// <returns>true if the two Failure instances are equal; otherwise, false.</returns>
    public static bool operator ==(Failure<F> left, Failure<F> right) =>
        left.Equals(right);

    /// <summary>
    /// Determines whether two Failure instances are not equal.
    /// </summary>
    /// <param name="left">The first Failure instance to compare.</param>
    /// <param name="right">The second Failure instance to compare.</param>
    /// <returns>true if the two Failure instances are not equal; otherwise, false.</returns>
    public static bool operator !=(Failure<F> left, Failure<F> right) =>
        !(left == right);

    /// <summary>
    /// Provides an implicit conversion from a Failure&lt;F&gt; to its contained failure value of type F.
    /// </summary>
    /// <param name="option">The Failure&lt;F&gt; instance to convert.</param>
    /// <returns>The failure value contained within the Failure&lt;F&gt;.</returns>
    public static implicit operator F(Failure<F> option) =>
        option.Value;

    /// <summary>
    /// Provides an explicit conversion from a Failure&lt;F&gt; to a Failure&lt;object&gt;, allowing for conversions
    /// to a more general failure type.
    /// </summary>
    /// <param name="failure">The Failure&lt;F&gt; instance to convert.</param>
    /// <returns>A Failure&lt;object&gt; containing the same failure information and stack trace.</returns>
    public static explicit operator Failure<object>(Failure<F> failure) =>
        new(failure.Value!, failure.StackTrace);

    /// <summary>
    /// Provides an explicit conversion from a Failure&lt;object&gt; to a Failure&lt;F&gt;, allowing for conversions
    /// from a general failure type to a more specific one.
    /// </summary>
    /// <param name="failure">The Failure&lt;object&gt; instance to convert.</param>
    /// <returns>A Failure&lt;F&gt; containing the failure information cast to type F and the same stack trace.</returns>
    public static explicit operator Failure<F>(Failure<object> failure) =>
        new((F)failure.Value, failure.StackTrace);
}
