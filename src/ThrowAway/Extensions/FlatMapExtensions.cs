using System;
using System.Diagnostics.CodeAnalysis;
using static ThrowAway.Helpers;
using static ThrowAway.Option;

namespace ThrowAway
{
    public static class FlatMapExtensions
    {
        public static Option<T> FlatMap<V, F, T>(this Option<V> option, [DisallowNull] Func<V, Option<T, F>> mapping)
        {
            if (IsNull(mapping))
                throw new ArgumentNullException(nameof(mapping));

            return option.FlatMap(some => mapping(some));
        }

        public static Option<T> FlatMap<V, T>(this Option<V> option, [DisallowNull] Func<V, Option<T>> mapping)
        {
            if (IsNull(mapping))
                throw new ArgumentNullException(nameof(mapping));

            return option.Match(
                some: mapping,
                fail => Fail<T>(fail));
        }
    }
}
