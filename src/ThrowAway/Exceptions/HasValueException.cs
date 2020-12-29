using System.Diagnostics.CodeAnalysis;

namespace ThrowAway
{
    public class HasValueException : ValueException
    {
        public HasValueException([DisallowNull] string message, [DisallowNull] object value) : base(message, value)
        {
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