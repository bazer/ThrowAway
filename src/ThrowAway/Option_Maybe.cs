using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using static ThrowAway.Helpers;

namespace ThrowAway
{
    public readonly struct Option<V>
    {
        [DisallowNull]
        private readonly V value;

        [DisallowNull]
        private readonly string failure;

        public readonly bool HasValue { get; }
        public readonly bool HasFailed => !HasValue;

        public V Value => HasValue
           ? value
           : throw new HasFailedException<string>("The option has failed with", failure);

        public string Failure => HasValue
            ? throw new HasValueException<V>("The option has not failed, it has value", value!)
            : failure;

        public Option<V> ThrowOnFail()
        {
            if (HasFailed)
                throw new HasFailedException<string>("The option has failed with", failure!);

            return this;
        }

        private Option([DisallowNull] V value, [DisallowNull] string failure, bool hasValue)
        {
            this.value = value;
            this.failure = failure;
            this.HasValue = hasValue;
        }

        public static Option<V> Some([DisallowNull] V value)
        {
            if (IsNull(value))
                throw new ValueIsNullException("'Some' cannot be called with a 'null' value");

            return new Option<V>(value, string.Empty, true);
        }

        public static Option<V> Fail([DisallowNull] string reason)
            => new Option<V>(default!, reason, false);

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

        public override string ToString() => HasValue
            ? value?.ToString() ?? ""
            : failure;

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
