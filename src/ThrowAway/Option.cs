using System;
using static ThrowAway.Helpers;

namespace ThrowAway
{
    public static class Option
    {
        public static Option<T> Some<T>(T value) =>
            Option<T>.Some(value);

        public static Option<Nothing> None() =>
            Option<Nothing>.None;

        public static Option<T> None<T>() =>
            Option<T>.None;
    }

    public readonly struct Option<T>
    {
        private readonly T value;
        public readonly bool HasValue { get; }
        public T Value => HasValue
           ? value
           : throw new NoneException();

        private Option(T value, bool hasValue)
        {
            this.value = value;
            this.HasValue = hasValue;
        }

        public static Option<T> Some(T value)
        {
            if (IsNull(value))
                throw new ArgumentNullException(nameof(value), "'Some' cannot be called with a 'null' value");

            return new Option<T>(value, true);
        }

        public static Option<T> None
            => new Option<T>(default, false);

        public static implicit operator T(Option<T> option) => option.Value;
        public static implicit operator Option<T>(T value) => IsNull(value)
            ? None
            : Some(value);

        public override string ToString() => HasValue
            ? value.ToString()
            : "None";
    }
}
