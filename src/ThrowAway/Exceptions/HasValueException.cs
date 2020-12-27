namespace ThrowAway
{
    public class HasValueException : ValueException
    {
        public HasValueException(string message, object value) : base(message, value)
        {
        }
    }

    public class HasValueException<T> : HasValueException
    {
        public HasValueException(string message, T value) : base(message, value)
        {
        }

        public new T Value => (T)base.Value;
    }
}