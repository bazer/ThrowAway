using System;
using System.Diagnostics.CodeAnalysis;

namespace ThrowAway
{
    public static class Option
    {
        public static Option<T> Some<T>([DisallowNull] T value) =>
            Option<T>.Some(value);

        public static Option<string> Fail(string reason) =>
            Option<string>.Fail(reason);

        public static Option<T> Fail<T>(string reason) =>
            Option<T>.Fail(reason);

        public static Option<V, F> Some<V, F>([DisallowNull] V value) =>
            Option<V, F>.Some(value);

        public static Option<V, F> Fail<V, F>([DisallowNull] F failure) =>
            Option<V, F>.Fail(failure);

        public static Option<V> Catch<V>(Func<Option<V>> func)
        {
            try
            {
                return func();
            }
            catch (HasFailedException<string> e)
            {
                return e.Failure;
            }
        }

        public static Option<V, F> Catch<V, F>(Func<Option<V, F>> func)
        {
            try
            {
                return func();
            }
            catch (HasFailedException<F> e)
            {
                return e.Failure!;
            }
        }
    }
}