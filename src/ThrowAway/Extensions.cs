using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ThrowAway
{
    public static class Extensions
    {
        public static Option<V, F> SomeNotNull<V, F>(this V value, [DisallowNull] F failure) =>
            value.SomeWhen(val => val != null, failure);

        public static Option<V, F> SomeWhen<V, F>(this V value, [DisallowNull] Func<V, bool> predicate, [DisallowNull] F failure)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return predicate(value)
                ? Option.Some<V, F>(value!)
                : Option.Fail<V, F>(failure);
        }

        public static T ValueOrFailure<T, TException>(this Option<T, TException> option)
            => option.Value;

        public static T ValueOrFailure<T, TException>(this Option<T, TException> option, string errorMessage)
        {
            if (option.HasValue)
                return option.Value;

            throw new HasFailedException(errorMessage, option.Failure!);
        }

        public static V ValueOrFailure<V, F>(this Option<V, F> option, Func<F, string> errorMessageFactory)
        {
            if (errorMessageFactory == null)
                throw new ArgumentNullException(nameof(errorMessageFactory));

            if (option.HasValue)
                return option.Value;

            throw new HasFailedException<F>(errorMessageFactory(option.Failure), option.Failure!);
        }

        public static T ValueOrDefault<T, TException>(this Option<T, TException> option)
        {
            if (option.HasValue)
                return option.Value;

            return default!;
        }
    }
}
