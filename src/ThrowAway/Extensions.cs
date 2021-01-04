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

        public static V ValueOrFailure<V, F>(this Option<V, F> option)
            => option.Value;

        public static V ValueOrFailure<V, F>(this Option<V, F> option, string failureMessage)
        {
            if (option.HasValue)
                return option.Value;

            throw new HasFailedException<F>(failureMessage, option.Failure);
        }

        public static V ValueOrFailure<V, F>(this Option<V, F> option, Func<F, string> failureFunc)
        {
            if (failureFunc == null)
                throw new ArgumentNullException(nameof(failureFunc));

            if (option.HasValue)
                return option.Value;

            throw new HasFailedException<F>(failureFunc(option.Failure), option.Failure);
        }

        public static V ValueOrDefault<V, F>(this Option<V, F> option)
        {
            if (option.HasValue)
                return option.Value;

            return default!;
        }
    }
}
