using System;

namespace ThrowAway
{
    public class ThrowAwayException : Exception
    {
        public ThrowAwayException(string message) : base(message)
        {

        }
    }

    public class NoneException : ThrowAwayException
    {
        public NoneException(string message) : base(message)
        {
        }
    }

    public class ValueException : ThrowAwayException
    {
        public object Value { get; }

        public ValueException(string message, object value) : base($"{message} '{value}'")
        {
            Value = value;
        }
    }

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

    public class ValueIsNullException : ThrowAwayException
    {
        public ValueIsNullException(string message) : base(message)
        {

        }
    }
}
