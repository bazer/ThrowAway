using System;
using System.Diagnostics.CodeAnalysis;

namespace ThrowAway
{
    public class ValueException : ThrowAwayException
    {
        [DisallowNull]
        public object Value { get; }

        public ValueException(string message, object value) : base($"{message} '{value}'")
        {
            if (Helpers.IsNull(message))
                throw new ArgumentNullException(nameof(message));

            if (Helpers.IsNull(value))
                throw new ArgumentNullException(nameof(value));

            Value = value;
        }
    }
}