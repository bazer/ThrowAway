using System;

namespace ThrowAway
{
    public class ThrowAwayException : Exception
    {
        public ThrowAwayException(string message) : base(message)
        {
        }
    }
}