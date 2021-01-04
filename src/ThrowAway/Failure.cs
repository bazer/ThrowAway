using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using static ThrowAway.Helpers;

namespace ThrowAway
{
    public readonly struct Failure<F>
    {
        public readonly F Value { get; }
        public readonly StackTrace? StackTrace { get; }

        public Failure([DisallowNull] F value, [AllowNull] StackTrace stackTrace)
        {
            if (IsNull(value))
                throw new ArgumentNullException(nameof(value));

            Value = value;
            StackTrace = stackTrace;
        }

        public override string ToString() =>
            Value?.ToString() ?? "";

        public override bool Equals(object obj)
        {
            if (Value == null)
                return obj == null;

            if (obj is F v)
                return Value.Equals(v);

            if (obj is Failure<F> f)
                return Value.Equals(f.Value);

            return false;
        }

        public override int GetHashCode() => Value == null
            ? 0
            : Value.GetHashCode();

        public static bool operator ==(Failure<F> left, Failure<F> right) =>
            left.Equals(right);

        public static bool operator !=(Failure<F> left, Failure<F> right) =>
            !(left == right);

        public static implicit operator F(Failure<F> option) =>
            option.Value;

        public static explicit operator Failure<object>(Failure<F> failure) =>
            new Failure<object>(failure.Value!, failure.StackTrace);

        public static explicit operator Failure<F>(Failure<object> failure) =>
            new Failure<F>((F)failure.Value, failure.StackTrace);
    }
}
