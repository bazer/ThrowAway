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

    public class FailException : ThrowAwayException
    {
        public FailException(string message) : base(message)
        {
        }
    }

    public class ValueException : ThrowAwayException
    {
        public ValueException(string message) : base(message)
        {
        }
    }

    public class ValueIsNullException : ThrowAwayException
    {
        public ValueIsNullException(string message) : base(message)
        {

        }
    }
}
