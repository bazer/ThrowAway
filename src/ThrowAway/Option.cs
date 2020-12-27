using System;
using static ThrowAway.Helpers;

namespace ThrowAway
{
    public static class Option
    {
        public static Option<T> Some<T>(T value) =>
            Option<T>.Some(value);

        public static Option<string> Fail(string reason) =>
            Option<string>.Fail(reason);

        public static Option<T> Fail<T>(string reason) =>
            Option<T>.Fail(reason);
    }

    public readonly struct Option<T>
    {
        private readonly T value;
        private readonly string failure;
        public readonly bool HasValue { get; }
        public T Value => HasValue
           ? value
           : throw new HasFailedException<string>("The option has failed with", failure);

        public string Failure => HasValue
            ? throw new HasValueException<T>("The option has not failed, it has value", value)
            : failure;

        private Option(T value, string failure, bool hasValue)
        {
            this.value = value;
            this.failure = failure;
            this.HasValue = hasValue;
        }

        public static Option<T> Some(T value)
        {
            if (IsNull(value))
                throw new ValueIsNullException("'Some' cannot be called with a 'null' value");

            return new Option<T>(value, null, true);
        }

        public static Option<T> Fail(string reason)
            => new Option<T>(default, reason, false);

        public static implicit operator T(Option<T> option) => option.Value;
        public static implicit operator Option<T>(T value)
        {
            if (IsNull(value))
                throw new ValueIsNullException("Cannot convert from a 'null' value. 'Null' is not allowed.");

            return Some(value);
        }

        public static implicit operator Option<T>(string reason)
        {
            if (IsNull(reason))
                throw new ValueIsNullException("Cannot convert from a 'null' value. 'Null' is not allowed.");

            return Fail(reason);
        }

        public override string ToString() => HasValue
            ? value.ToString()
            : failure;
    }
}
