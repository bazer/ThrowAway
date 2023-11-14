using static ThrowAway.Helpers;

namespace ThrowAway;

/// <summary>
/// Represents a value of type V that may or may not exist. This struct provides a way to handle
/// optional values and failures in a functional programming style. The failure type is always a string.
/// </summary>
/// <typeparam name="V">The type of the value that may be contained in the Option.</typeparam>
public readonly struct Option<V>
{
    private readonly V value;
    private readonly Failure<string> failure;

    /// <summary>
    /// Indicates whether the Option contains a value. This property is true when the Option 
    /// represents a successful outcome and contains a valid value.
    /// </summary>
    public readonly bool HasValue { get; }

    /// <summary>
    /// Indicates whether the Option represents a failure. This property is true when the Option 
    /// does not contain a value, representing an unsuccessful outcome or an operation that 
    /// has failed.
    /// </summary>
    public readonly bool HasFailed => !HasValue;

    /// <summary>
    /// Retrieves the contained value of the Option if it is in a successful state (i.e., it has a value).
    /// This property is a key aspect of the Option type, allowing direct access to the underlying value
    /// when the Option represents a successful outcome. Accessing this property in a failed state
    /// (when the Option does not have a value) will result in a HasFailedException, signifying that the
    /// operation or computation represented by the Option did not succeed.
    /// </summary>
    /// <returns>The contained value of type V if the Option is in a successful state.</returns>
    /// <exception cref="HasFailedException&lt;@string&gt;">Thrown when the Option is in a failed state, indicating
    /// that there is no value to retrieve.</exception>
    public V Value => HasValue
        ? value
        : throw new HasFailedException<string>("The option has failed with", failure);

    /// <summary>
    /// Retrieves the failure information of the Option if it is in a failed state (i.e., it represents a failure).
    /// This property is essential for understanding why an operation or computation represented by the Option
    /// has failed. Accessing this property in a successful state (when the Option has a value) will result in a
    /// HasValueException, indicating that the operation did not fail and a value is present.
    /// </summary>
    /// <returns>The failure information of type Failure&lt;F&gt; if the Option is in a failed state.</returns>
    /// <exception cref="HasValueException&lt;V&gt;">Thrown when the Option is in a successful state, indicating
    /// that there is no failure information to retrieve, as the operation was successful.</exception>
    public Failure<string> Failure => HasValue
        ? throw new HasValueException<V>("The option has not failed, it has value", value!)
        : failure;



    private Option([DisallowNull] V value, Failure<string> failure, bool hasValue)
    {
        this.value = value;
        this.failure = failure;
        this.HasValue = hasValue;
    }

    /// <summary>
    /// Creates an Option with a value, indicating success.
    /// </summary>
    /// <param name="value">The value to be contained in the Option.</param>
    /// <returns>An Option containing the specified value.</returns>
    public static Option<V> Some([DisallowNull] V value)
    {
        if (IsNull(value))
            throw new ValueIsNullException("'Some' cannot be called with a 'null' value");

        return new Option<V>(value, default, true);
    }

    /// <summary>
    /// Creates an Option indicating failure with a given reason.
    /// </summary>
    /// <param name="reason">The reason for the failure.</param>
    /// <returns>An Option indicating the failure with the specified reason.</returns>
    public static Option<V> Fail([DisallowNull] string reason)
    {
        var failure = new Failure<string>(reason, OptionConfig.LogStackTraceOnFailure
            ? new StackTrace()
            : null);

        return new Option<V>(default!, failure, false);
    }

    internal static Option<V> Fail([DisallowNull] string reason, [AllowNull] string? stackTrace)
    {
        var failure = new Failure<string>(reason, stackTrace);

        return new Option<V>(default!, failure, false);
    }

    /// <summary>
    /// Provides an implicit conversion from an Option&lt;V&gt; to its contained value of type V.
    /// This conversion allows the Option&lt;V&gt; to be used in contexts where a value of type V is expected,
    /// seamlessly unwrapping the contained value. It simplifies the usage of Option types by enabling
    /// automatic conversion to the underlying value, but should be used with caution as it will throw
    /// an exception if the Option is in a failed state (i.e., does not contain a value).
    /// </summary>
    /// <param name="option">The Option&lt;V&gt; instance to convert.</param>
    /// <returns>The value contained within the Option&lt;V&gt; if it exists.</returns>
    /// <exception cref="HasFailedException">Thrown if the Option&lt;V&gt; does not contain a value.</exception>
    public static implicit operator V(Option<V> option) => option.Value;



    /// <summary>
    /// Converts a value to an Option containing that value.
    /// </summary>
    /// <param name="value">The value to convert to Option.</param>
    public static implicit operator Option<V>([DisallowNull] V value)
    {
        if (IsNull(value))
            throw new ValueIsNullException("Cannot convert from a 'null' value. 'Null' is not allowed.");

        return Some(value);
    }

    /// <summary>
    /// Converts a failure reason to an Option indicating failure with that reason.
    /// </summary>
    /// <param name="reason">The failure reason to convert to Option.</param>
    public static implicit operator Option<V>([DisallowNull] string reason)
    {
        if (IsNull(reason))
            throw new ValueIsNullException("Cannot convert from a 'null' value. 'Null' is not allowed.");

        return Fail(reason);
    }

    /// <summary>
    /// Converts a Failure to an Option indicating failure with that Failure.
    /// </summary>
    /// <param name="failure">The Failure to convert to Option.</param>
    public static implicit operator Option<V>(Failure<string> failure)
    {
        if (IsNull(failure.Value))
            throw new ValueIsNullException("Cannot convert from a failure with 'null' value. 'Null' is not allowed.");

        return new Option<V>(default!, failure, false);
    }

    /// <summary>
    /// Provides a string representation of the Option. If the Option is in a successful state, 
    /// the method returns the string representation of the contained value. If the Option is 
    /// in a failed state, it returns the string representation of the failure. This method is 
    /// useful for debugging and logging purposes, as it gives a clear and concise description 
    /// of the Option's current state, whether it holds a value or represents a failure.
    /// </summary>
    /// <returns>A string representation of the Option's current state, either the value 
    /// if it is in a successful state or the failure if it is in a failed state.</returns>
    public override string ToString() => HasValue
        ? value?.ToString() ?? ""
        : failure.Value;

    /// <summary>
    /// Determines whether the specified object is equal to the current Option&lt;V&gt; instance.
    /// Equality is defined as having the same state of presence or absence of a value, and if present,
    /// the values themselves being equal. This method is crucial for ensuring that two Option instances
    /// represent the same conceptual value or failure state, which is key in functional programming patterns.
    /// </summary>
    /// <param name="obj">The object to compare with the current Option&lt;V&gt; instance.</param>
    /// <returns>true if the specified object is equal to the current Option&lt;V&gt; instance; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is not Option<V> option)
            return false;

        if (option.HasValue && !HasValue)
            return false;

        if (!option.HasValue && HasValue)
            return false;

        if (option.HasValue && option.Value!.Equals(Value))
            return true;

        if (option.HasFailed && option.Failure!.Equals(Failure))
            return true;

        return false;
    }

    /// <summary>
    /// Serves as the default hash function. The hash code is derived from the Option's value if it has one,
    /// or from its failure state if it does not. This ensures that the hash code is consistent with the 
    /// Option's state and can be reliably used in hash-based collections like dictionaries or hash sets.
    /// </summary>
    /// <returns>A hash code for the current Option&lt;V&gt; instance.</returns>
    public override int GetHashCode() => HasValue
        ? Value!.GetHashCode()
        : Failure.GetHashCode();


    /// <summary>
    /// Determines whether two instances of Option&lt;V&gt; are equal. This operator provides a convenient
    /// way to compare two Option instances directly, respecting the rules of equality defined in the
    /// Equals method. It simplifies comparisons in code, adhering to the functional programming paradigm
    /// of immutability and value-based equality.
    /// </summary>
    /// <param name="left">The first Option&lt;V&gt; to compare.</param>
    /// <param name="right">The second Option&lt;V&gt; to compare.</param>
    /// <returns>true if the two Option&lt;V&gt; instances are equal; otherwise, false.</returns>
    public static bool operator ==(Option<V> left, Option<V> right) =>
        left.Equals(right);

    /// <summary>
    /// Determines whether two instances of Option&lt;V&gt; are not equal. This operator complements the equality
    /// operator, providing a straightforward way to check for inequality between two Option instances.
    /// As with the equality operator, it respects the value-based comparison rules, fitting well within
    /// the principles of functional programming.
    /// </summary>
    /// <param name="left">The first Option&lt;V&gt; to compare.</param>
    /// <param name="right">The second Option&lt;V&gt; to compare.</param>
    /// <returns>true if the two Option&lt;V&gt; instances are not equal; otherwise, false.</returns>
    public static bool operator !=(Option<V> left, Option<V> right) =>
        !(left == right);
}
