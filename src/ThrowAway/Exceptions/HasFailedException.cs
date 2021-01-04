using System;
using System.Diagnostics.CodeAnalysis;

namespace ThrowAway
{
    public class HasFailedException : ThrowAwayException
    {
        public Failure<object> Failure { get; }

        public HasFailedException([DisallowNull] string message, [DisallowNull] Failure<object> value) : base($"{message} '{value}'")
        {
            if (Helpers.IsNull(message))
                throw new ArgumentNullException(nameof(message));

            if (Helpers.IsNull(value))
                throw new ArgumentNullException(nameof(value));

            Failure = value;
        }
    }

    public class HasFailedException<F> : HasFailedException
    {
        public HasFailedException([DisallowNull] string message, [DisallowNull] Failure<F> failure) : base(message, (Failure<object>)failure)
        {
        }

        new public Failure<F> Failure => (Failure<F>)base.Failure;
    }
}