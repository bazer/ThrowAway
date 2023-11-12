using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using static ThrowAway.Helpers;

namespace ThrowAway;

/// <summary>
/// Represents an Option type that can contain either a value of type V or a failure of type F.
/// This struct is a fundamental part of functional programming within the library, providing
/// a robust way to handle optional values and operational failures. It encapsulates the concept
/// of a computation that can either result in a valid value or a specific failure, thus enhancing
/// error handling and control flow management in a type-safe manner.
/// </summary>
/// <typeparam name="V">The type of the value that the Option may contain.</typeparam>
/// <typeparam name="F">The type of the failure that the Option may represent.</typeparam>
public readonly struct Option<V, F>
{
    private readonly V value;
    private readonly Failure<F> failure;

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
    /// <exception cref="HasFailedException&lt;F&gt;">Thrown when the Option is in a failed state, indicating
    /// that there is no value to retrieve.</exception>
    public V Value => HasValue
        ? value
        : throw new HasFailedException<F>("The option has failed with", failure!);


    /// <summary>
    /// Retrieves the failure information of the Option if it is in a failed state (i.e., it represents a failure).
    /// This property is essential for understanding why an operation or computation represented by the Option
    /// has failed. Accessing this property in a successful state (when the Option has a value) will result in a
    /// HasValueException, indicating that the operation did not fail and a value is present.
    /// </summary>
    /// <returns>The failure information of type Failure&lt;F&gt; if the Option is in a failed state.</returns>
    /// <exception cref="HasValueException&lt;V&gt;">Thrown when the Option is in a successful state, indicating
    /// that there is no failure information to retrieve, as the operation was successful.</exception>
    public Failure<F> Failure => HasValue
        ? throw new HasValueException<V>("The option has not failed, it has value", value!)
        : failure;

    /// <summary>
    /// Throws an exception if the Option is in a failed state, otherwise returns the Option itself.
    /// When invoked, it allows an Option in a failed state to propagate the failure as an exception, thereby 
    /// transitioning from a functional error handling approach to a more traditional exception-based 
    /// mechanism. This is useful in contexts where failure of the Option represents an unexpected or
    /// critical condition that cannot be ignored or where traditional exception handling is more 
    /// appropriate.
    /// </summary>
    /// <returns>The same Option instance if it is in a successful state.</returns>
    /// <exception cref="HasFailedException&lt;F&gt;">Thrown when the Option is in a failed state, providing
    /// details about the failure.</exception>
    public Option<V, F> ThrowOnFail()
    {
        if (HasFailed)
            throw new HasFailedException<F>("The option has failed with", failure!);

        return this;
    }

    /// <summary>
    /// Filters the Option based on a provided condition. If the Option is in a successful state and the value 
    /// does not satisfy the specified condition, the method returns a new Option in a failed state with the 
    /// provided failure reason. If the Option is already in a failed state or if the value satisfies the condition,
    /// the original Option is returned unchanged. This method is useful for applying conditional checks to the 
    /// value within an Option in a functional way, allowing for the continuation or alteration of the Option's 
    /// state based on specific criteria.
    /// </summary>
    /// <param name="condition">A predicate to apply to the Option's value if it is in a successful state.</param>
    /// <param name="failure">The failure reason to be used if the condition is not satisfied by the value.</param>
    /// <returns>An Option that is either the original Option if it is in a failed state or if the value satisfies 
    /// the condition, or a new failed Option with the provided failure reason if the condition is not satisfied.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the condition is null.</exception>
    public Option<V, F> Filter(Func<V, bool> condition, [DisallowNull] F failure) =>
        HasValue && !condition(Value)
            ? Option.Fail<V, F>(failure)
            : this;


    /// <summary>
    /// Retrieves the contained value of the Option if it is in a successful state; otherwise, returns 
    /// an alternative value provided. This method is useful for providing a default or fallback value 
    /// in cases where the Option does not contain a value, allowing for seamless value retrieval 
    /// without explicit checks for the presence of the value.
    /// </summary>
    /// <param name="alternative">The alternative value to return if the Option is in a failed state.</param>
    /// <returns>The contained value if the Option is in a successful state; otherwise, the alternative value.</returns>
    public V ValueOr(V alternative) => HasValue
        ? value
        : alternative;


    /// <summary>
    /// Retrieves the contained value of the Option if it is in a successful state; otherwise, invokes 
    /// a factory function to provide an alternative value. This method offers flexibility by deferring 
    /// the creation of the alternative value until it is needed, which can be beneficial for scenarios 
    /// where the alternative value is resource-intensive to create or compute.
    /// </summary>
    /// <param name="alternativeFactory">A function that produces the alternative value when the Option 
    /// is in a failed state.</param>
    /// <returns>The contained value if the Option is in a successful state; otherwise, the value 
    /// produced by the alternativeFactory.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the alternativeFactory is null.</exception>
    public V ValueOr(Func<V> alternativeFactory)
    {
        if (alternativeFactory == null)
            throw new ArgumentNullException(nameof(alternativeFactory));

        return HasValue
            ? value
            : alternativeFactory();
    }


    /// <summary>
    /// Retrieves the contained value of the Option if it is in a successful state; otherwise, invokes 
    /// a factory function with the failure reason to provide an alternative value. This variant of 
    /// ValueOr allows the alternative value to be determined based on the specific reason for the 
    /// Option's failure, providing a way to tailor the fallback value to the nature of the failure.
    /// </summary>
    /// <param name="alternativeFactory">A function that takes the failure reason and produces the 
    /// alternative value when the Option is in a failed state.</param>
    /// <returns>The contained value if the Option is in a successful state; otherwise, the value 
    /// produced by the alternativeFactory based on the failure reason.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the alternativeFactory is null.</exception>
    public V ValueOr(Func<F, V> alternativeFactory)
    {
        if (alternativeFactory == null)
            throw new ArgumentNullException(nameof(alternativeFactory));

        return HasValue
            ? value
            : alternativeFactory(failure);
    }





    private Option([DisallowNull] V value, [DisallowNull] Failure<F> failure, bool hasValue)
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
    public static Option<V, F> Some([DisallowNull] V value)
    {
        if (IsNull(value))
            throw new ValueIsNullException("'Some' cannot be called with a 'null' value");

        return new Option<V, F>(value, default!, true);
    }

    /// <summary>
    /// Creates an Option indicating failure with a given failure type.
    /// </summary>
    /// <param name="failure">The failure type to be contained in the Option.</param>
    /// <returns>An Option indicating the failure with the specified type.</returns>
    public static Option<V, F> Fail([DisallowNull] F failure)
    {
        var fail = new Failure<F>(failure, OptionConfig.LogStackTraceOnFailure
            ? new StackTrace()
            : null);

        return new Option<V, F>(default!, fail, false);
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
    public static implicit operator V(Option<V, F> option) => option.Value;

    /// <summary>
    /// Provides an implicit conversion from a value of type V to an Option&lt;V, F&gt;. This conversion 
    /// simplifies the creation of successful Option instances from values directly. It ensures that 
    /// Option types can be seamlessly integrated into code that works with values of type V, enhancing 
    /// readability and reducing boilerplate code.
    /// </summary>
    /// <param name="value">The value to convert to an Option.</param>
    /// <returns>An Option&lt;V, F&gt; representing a successful state with the specified value.</returns>
    /// <exception cref="ValueIsNullException">Thrown if the value is null, as null values are not 
    /// allowed for successful Option states.</exception>
    public static implicit operator Option<V, F>([DisallowNull] V value)
    {
        if (IsNull(value))
            throw new ValueIsNullException("Cannot convert from a 'null' value. 'Null' is not allowed.");

        return Some(value);
    }


    /// <summary>
    /// Provides an implicit conversion from a failure of type F to an Option&lt;V, F&gt;. This conversion 
    /// enables straightforward creation of failed Option instances from failure reasons. It allows 
    /// for the natural and intuitive handling of failures, making the creation of failed Option instances 
    /// as simple as using the failure type directly.
    /// </summary>
    /// <param name="failure">The failure reason to convert to an Option.</param>
    /// <returns>An Option&lt;V, F&gt; representing a failed state with the specified failure reason.</returns>
    /// <exception cref="ValueIsNullException">Thrown if the failure reason is null, as null values 
    /// are not allowed for failed Option states.</exception>
    public static implicit operator Option<V, F>([DisallowNull] F failure)
    {
        if (IsNull(failure))
            throw new ValueIsNullException("Cannot convert from a 'null' value. 'Null' is not allowed.");

        return Fail(failure);
    }


    /// <summary>
    /// Provides an implicit conversion from a Failure&lt;F&gt; to an Option&lt;V, F&gt;. This conversion facilitates 
    /// the direct use of Failure&lt;F&gt; instances in contexts where an Option&lt;V, F&gt; is expected, streamlining 
    /// the process of handling and propagating failures within the functional flow. It ensures that failures 
    /// encapsulated in a Failure&lt;F&gt; type can be seamlessly integrated into the Option paradigm.
    /// </summary>
    /// <param name="failure">The Failure&lt;F&gt; instance to convert to an Option.</param>
    /// <returns>An Option&lt;V, F&gt; representing a failed state with the specified failure encapsulated in Failure&lt;F&gt;.</returns>
    /// <exception cref="ValueIsNullException">Thrown if the failure's value is null, maintaining the integrity of 
    /// the Option type by disallowing null values in its failure state.</exception>
    public static implicit operator Option<V, F>(Failure<F> failure)
    {
        if (IsNull(failure.Value))
            throw new ValueIsNullException("Cannot convert from a failure with 'null' value. 'Null' is not allowed.");

        return new Option<V, F>(default!, failure, false);
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
        : failure.ToString() ?? "";


    /// <summary>
    /// Determines whether the specified object is equal to the current Option instance. 
    /// Equality is based on the state of the Option (success or failure) and, if successful, 
    /// the equality of the contained values, or if failed, the equality of the failure reasons.
    /// This method is crucial for ensuring that two Option instances are considered equal 
    /// only if they represent the same conceptual outcome (either both successful with equal values, 
    /// or both failed with equal failure reasons).
    /// </summary>
    /// <param name="obj">The object to compare with the current Option instance.</param>
    /// <returns>true if the specified object is equal to the current Option instance; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is not Option<V, F> option)
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
    /// Serves as the default hash function. The hash code is derived based on the Option's state: 
    /// if it contains a value, the hash code of the value is used; if it represents a failure, 
    /// the hash code of the failure is used. This ensures that the hash code is consistent with 
    /// the Option's equality definition and can be reliably used in collections and algorithms 
    /// that depend on hash codes.
    /// </summary>
    /// <returns>A hash code for the current Option instance.</returns>
    public override int GetHashCode() => HasValue
        ? Value!.GetHashCode()
        : Failure!.GetHashCode();


    /// <summary>
    /// Determines whether two Option instances are equal. This operator provides a convenient 
    /// way to compare two Option instances directly, using the equality logic defined in the 
    /// Equals method. It simplifies comparisons in code by abstracting away the explicit method call.
    /// </summary>
    /// <param name="left">The first Option instance to compare.</param>
    /// <param name="right">The second Option instance to compare.</param>
    /// <returns>true if the two Option instances are equal; otherwise, false.</returns>
    public static bool operator ==(Option<V, F> left, Option<V, F> right) =>
        left.Equals(right);


    /// <summary>
    /// Determines whether two Option instances are not equal. This operator is the inverse of the 
    /// equality operator and provides a straightforward way to check for inequality between two 
    /// Option instances, using the same equality logic but inverting the result.
    /// </summary>
    /// <param name="left">The first Option instance to compare.</param>
    /// <param name="right">The second Option instance to compare.</param>
    /// <returns>true if the two Option instances are not equal; otherwise, false.</returns>
    public static bool operator !=(Option<V, F> left, Option<V, F> right) =>
        !(left == right);

}
