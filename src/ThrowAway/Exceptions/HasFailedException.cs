using System.Diagnostics.CodeAnalysis;

namespace ThrowAway
{
    public class HasFailedException : ValueException
    {
        public HasFailedException([DisallowNull] string message, [DisallowNull] object value) : base(message, value)
        {
        }
    }

    public class HasFailedException<T> : HasFailedException
    {
        public HasFailedException([DisallowNull] string message, [DisallowNull] T value) : base(message, value)
        {
        }

        public T Failure => (T)base.Value;
    }
}