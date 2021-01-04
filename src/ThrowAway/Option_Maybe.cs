using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using static ThrowAway.Helpers;

namespace ThrowAway
{
    public readonly struct Option<V>
    {
        private readonly V value;
        private readonly Failure<string> failure;

        public readonly bool HasValue { get; }
        public readonly bool HasFailed => !HasValue;

        public V Value => HasValue
           ? value
           : throw new HasFailedException<string>("The option has failed with", failure);

        public Failure<string> Failure => HasValue
            ? throw new HasValueException<V>("The option has not failed, it has value", value!)
            : failure;

        public Option<V> ThrowOnFail()
        {
            if (HasFailed)
                throw new HasFailedException<string>("The option has failed with", failure!);

            return this;
        }

        private Option([DisallowNull] V value, Failure<string> failure, bool hasValue)
        {
            this.value = value;
            this.failure = failure;
            this.HasValue = hasValue;
        }

        public static Option<V> Some([DisallowNull] V value)
        {
            if (IsNull(value))
                throw new ValueIsNullException("'Some' cannot be called with a 'null' value");

            return new Option<V>(value, default, true);
        }

        public static Option<V> Fail([DisallowNull] string reason)
        {
            var failure = new Failure<string>(reason, OptionConfig.LogStackTraceOnFailure
                ? new StackTrace()
                : null);

            return new Option<V>(default!, failure, false);
        }

        public static implicit operator V(Option<V> option) => option.Value;

        public static implicit operator Option<V>([DisallowNull] V value)
        {
            if (IsNull(value))
                throw new ValueIsNullException("Cannot convert from a 'null' value. 'Null' is not allowed.");

            return Some(value);
        }

        public static implicit operator Option<V>([DisallowNull] string reason)
        {
            if (IsNull(reason))
                throw new ValueIsNullException("Cannot convert from a 'null' value. 'Null' is not allowed.");

            return Fail(reason);
        }

        public static implicit operator Option<V>(Failure<string> failure)
        {
            if (IsNull(failure.Value))
                throw new ValueIsNullException("Cannot convert from a failure with 'null' value. 'Null' is not allowed.");

            return new Option<V>(default!, failure, false);
        }

        public override string ToString() => HasValue
            ? value?.ToString() ?? ""
            : failure.Value;

        public override bool Equals(object obj)
        {
            if (!(obj is Option<V> option))
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

        public override int GetHashCode() => HasValue
            ? Value!.GetHashCode()
            : Failure.GetHashCode();

        public static bool operator ==(Option<V> left, Option<V> right) =>
            left.Equals(right);

        public static bool operator !=(Option<V> left, Option<V> right) =>
            !(left == right);
    }
}
