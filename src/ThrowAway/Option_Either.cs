using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using static ThrowAway.Helpers;
using static ThrowAway.Option;

namespace ThrowAway
{
    public readonly struct Option<V, F>
    {
        private readonly V value;
        private readonly Failure<F> failure;

        public readonly bool HasValue { get; }
        public readonly bool HasFailed => !HasValue;

        public V Value => HasValue
           ? value
           : throw new HasFailedException<F>("The option has failed with", failure!);

        public Failure<F> Failure => HasValue
            ? throw new HasValueException<V>("The option has not failed, it has value", value!)
            : failure;

        public Option<T, F> FlatMap<T>(Func<V, Option<T, F>> mapping)
        {
            if (mapping == null)
                throw new ArgumentNullException(nameof(mapping));

            return this.Match(
                some: mapping,
                fail => Fail<T, F>(fail!));
        }

        public Option<V, F> ThrowOnFail()
        {
            if (HasFailed)
                throw new HasFailedException<F>("The option has failed with", failure!);

            return this;
        }

        public Option<V, F> Filter(Func<V, bool> condition, [DisallowNull] F failure) =>
            HasValue && !condition(Value)
                ? Option.Fail<V, F>(failure)
                : this;

        public V ValueOr(V alternative) => HasValue
            ? value
            : alternative;

        public V ValueOr(Func<V> alternativeFactory)
        {
            if (alternativeFactory == null)
                throw new ArgumentNullException(nameof(alternativeFactory));

            return HasValue
                ? value
                : alternativeFactory();
        }

        public V ValueOr(Func<F, V> alternativeFactory)
        {
            if (alternativeFactory == null)
                throw new ArgumentNullException(nameof(alternativeFactory));

            return HasValue
                ? value
                : alternativeFactory(failure);
        }

        public Option<V, F> Else(Option<V, F> alternativeOption) => HasValue
            ? this
            : alternativeOption;

        public Option<V, F> Else(Func<Option<V, F>> alternativeOptionFactory)
        {
            if (alternativeOptionFactory == null)
                throw new ArgumentNullException(nameof(alternativeOptionFactory));

            return HasValue ? this : alternativeOptionFactory();
        }

        private Option([DisallowNull] V value, [DisallowNull] Failure<F> failure, bool hasValue)
        {
            this.value = value;
            this.failure = failure;
            this.HasValue = hasValue;
        }

        public static Option<V, F> Some([DisallowNull] V value)
        {
            if (IsNull(value))
                throw new ValueIsNullException("'Some' cannot be called with a 'null' value");

            return new Option<V, F>(value, default!, true);
        }

        public static Option<V, F> Fail([DisallowNull] F failure)
        {
            var fail = new Failure<F>(failure, OptionConfig.LogStackTraceOnFailure
                ? new StackTrace()
                : null);

            return new Option<V, F>(default!, fail, false);
        }

        public static implicit operator V(Option<V, F> option) => option.Value;

        public static implicit operator Option<V, F>([DisallowNull] V value)
        {
            if (IsNull(value))
                throw new ValueIsNullException("Cannot convert from a 'null' value. 'Null' is not allowed.");

            return Some(value);
        }

        public static implicit operator Option<V, F>([DisallowNull] F failure)
        {
            if (IsNull(failure))
                throw new ValueIsNullException("Cannot convert from a 'null' value. 'Null' is not allowed.");

            return Fail(failure);
        }

        public static implicit operator Option<V, F>(Failure<F> failure)
        {
            if (IsNull(failure.Value))
                throw new ValueIsNullException("Cannot convert from a failure with 'null' value. 'Null' is not allowed.");

            return new Option<V, F>(default!, failure, false);
        }

        public override string ToString() => HasValue
            ? value?.ToString() ?? ""
            : failure.ToString() ?? "";

        public override bool Equals(object obj)
        {
            if (!(obj is Option<V, F> option))
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
            : Failure!.GetHashCode();

        public static bool operator ==(Option<V, F> left, Option<V, F> right) =>
            left.Equals(right);

        public static bool operator !=(Option<V, F> left, Option<V, F> right) =>
            !(left == right);
    }
}
