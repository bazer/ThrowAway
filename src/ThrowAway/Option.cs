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

        public static Option<V, F> Some<V, F>(V value) =>
            Option<V, F>.Some(value);

        public static Option<V, F> Fail<V, F>(F failure) =>
            Option<V, F>.Fail(failure);

        public static Option<V> Catch<V>(Func<Option<V>> func)
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

        public static Option<V, F> Catch<V, F>(Func<Option<V, F>> func)
        {
            try
            {
                return func();
            }
            catch (HasFailedException<F> e)
            {
                return e.Failure;
            }
        }
    }

    public readonly struct Option<V>
    {
        private readonly V value;
        private readonly string failure;
        public readonly bool HasValue { get; }
        public readonly bool HasFailed => !HasValue;

        public V Value => HasValue
           ? value
           : throw new HasFailedException<string>("The option has failed with", failure);

        public string Failure => HasValue
            ? throw new HasValueException<V>("The option has not failed, it has value", value)
            : failure;

        public T Match<T>(Func<V, T> value, Func<string, T> failure) => HasValue
            ? value(this.value)
            : failure(this.failure);

        private Option(V value, string failure, bool hasValue)
        {
            this.value = value;
            this.failure = failure;
            this.HasValue = hasValue;
        }

        public static Option<V> Some(V value)
        {
            if (IsNull(value))
                throw new ValueIsNullException("'Some' cannot be called with a 'null' value");

            return new Option<V>(value, null, true);
        }

        public static Option<V> Fail(string reason)
            => new Option<V>(default, reason, false);

        public static implicit operator V(Option<V> option) => option.Value;

        public static implicit operator Option<V>(V value)
        {
            if (IsNull(value))
                throw new ValueIsNullException("Cannot convert from a 'null' value. 'Null' is not allowed.");

            return Some(value);
        }

        public static implicit operator Option<V>(string reason)
        {
            if (IsNull(reason))
                throw new ValueIsNullException("Cannot convert from a 'null' value. 'Null' is not allowed.");

            return Fail(reason);
        }

        public override string ToString() => HasValue
            ? value.ToString()
            : failure;
    }

    public readonly struct Option<V, F>
    {
        private readonly V value;
        private readonly F failure;
        public readonly bool HasValue { get; }
        public readonly bool HasFailed => !HasValue;

        public V Value => HasValue
           ? value
           : throw new HasFailedException<F>("The option has failed with", failure);

        public F Failure => HasValue
            ? throw new HasValueException<V>("The option has not failed, it has value", value)
            : failure;

        public T Match<T>(Func<V, T> value, Func<F, T> failure) => HasValue
            ? value(this.value)
            : failure(this.failure);

        private Option(V value, F failure, bool hasValue)
        {
            this.value = value;
            this.failure = failure;
            this.HasValue = hasValue;
        }

        public static Option<V, F> Some(V value)
        {
            if (IsNull(value))
                throw new ValueIsNullException("'Some' cannot be called with a 'null' value");

            return new Option<V, F>(value, default, true);
        }

        public static Option<V, F> Fail(F failure)
            => new Option<V, F>(default, failure, false);

        public static implicit operator V(Option<V, F> option) => option.Value;

        public static implicit operator Option<V, F>(V value)
        {
            if (IsNull(value))
                throw new ValueIsNullException("Cannot convert from a 'null' value. 'Null' is not allowed.");

            return Some(value);
        }

        public static implicit operator Option<V, F>(F failure)
        {
            if (IsNull(failure))
                throw new ValueIsNullException("Cannot convert from a 'null' value. 'Null' is not allowed.");

            return Fail(failure);
        }

        public override string ToString() => HasValue
            ? value.ToString()
            : failure.ToString();
    }
}