namespace ThrowAway
{
    public class ValueException : ThrowAwayException
    {
        public object Value { get; }

        public ValueException(string message, object value) : base($"{message} '{value}'")
        {
            Value = value;
        }
    }
}