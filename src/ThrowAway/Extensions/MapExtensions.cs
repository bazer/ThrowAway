using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using static ThrowAway.Helpers;
using static ThrowAway.Option;

namespace ThrowAway
{
    public static class MapExtensions
    {
        public static Option<T> Map<V, T>(this Option<V> option, [DisallowNull] Func<V, T> mapping)
        {
            if (IsNull(mapping))
                throw new ArgumentNullException(nameof(mapping));

            return option.Match(
                some => Some(mapping(some)!),
                fail => Fail<T>(fail));
        }

        public static Option<T, F> Map<V, F, T>(this Option<V, F> option, [DisallowNull] Func<V, T> mapping)
        {
            if (IsNull(mapping))
                throw new ArgumentNullException(nameof(mapping));

            return option.Match(
                some => Option<T, F>.Some(mapping(some)!),
                fail => Option<T, F>.Fail(fail!));
        }

        public static Option<V, T> MapFail<V, F, T>(this Option<V, F> option, [DisallowNull] Func<F, T> mapping)
        {
            if (IsNull(mapping))
                throw new ArgumentNullException(nameof(mapping));

            return option.Match(
                some => Option<V, T>.Some(some!),
                fail => Option<V, T>.Fail(mapping(fail)!));
        }
    }
}
