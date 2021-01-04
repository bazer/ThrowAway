using System;
using System.Diagnostics.CodeAnalysis;

namespace ThrowAway
{
    public class HasValueException : ThrowAwayException
    {
        public object Value { get; }

        public HasValueException([DisallowNull] string message, [DisallowNull] object value) : base($"{message} '{value}'")
        {
            if (Helpers.IsNull(message))
                throw new ArgumentNullException(nameof(message));

            if (Helpers.IsNull(value))
                throw new ArgumentNullException(nameof(value));

            Value = value;
        }
    }

    public class HasValueException<T> : HasValueException
    {
        public HasValueException([DisallowNull] string message, [DisallowNull] T value) : base(message, value)
        {
        }

        public new T Value => (T)base.Value;
    }
}