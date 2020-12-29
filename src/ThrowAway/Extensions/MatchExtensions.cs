using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using static ThrowAway.Helpers;

namespace ThrowAway
{
    public static class MatchExtensions
    {
        public static T Match<V, T>(this Option<V> option, [DisallowNull] Func<V, T> some, [DisallowNull] Func<string, T> fail)
        {
            if (IsNull(some))
                throw new ArgumentNullException(nameof(some));

            if (IsNull(fail))
                throw new ArgumentNullException(nameof(fail));

            return option.HasValue
                ? some(option.Value)
                : fail(option.Failure);
        }

        public static T Match<V, F, T>(this Option<V, F> option, [DisallowNull] Func<V, T> some, [DisallowNull] Func<F, T> fail)
        {
            if (IsNull(some))
                throw new ArgumentNullException(nameof(some));

            if (IsNull(fail))
                throw new ArgumentNullException(nameof(fail));

            return option.HasValue
                ? some(option.Value)
                : fail(option.Failure);
        }

        public static void Match<V, F>(this Option<V, F> option, [DisallowNull] Action<V> some, [DisallowNull] Action<F> fail)
        {
            if (IsNull(some))
                throw new ArgumentNullException(nameof(some));

            if (IsNull(fail))
                throw new ArgumentNullException(nameof(fail));

            if (option.HasValue)
                some(option.Value);
            else
                fail(option.Failure);
        }
    }
}
