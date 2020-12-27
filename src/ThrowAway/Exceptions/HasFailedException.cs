namespace ThrowAway
{
    public class HasFailedException : ValueException
    {
        public HasFailedException(string message, object value) : base(message, value)
        {
        }
    }

    public class HasFailedException<T> : HasFailedException
    {
        public HasFailedException(string message, T value) : base(message, value)
        {
        }

        public T Failure => (T)base.Value;
    }
}