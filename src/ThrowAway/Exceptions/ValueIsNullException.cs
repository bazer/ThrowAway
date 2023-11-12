namespace ThrowAway
{
    /// <summary>
    /// Represents an exception that is thrown in the ThrowAway library when an operation encounters a null value where a non-null value is required.
    /// This exception is particularly relevant in the context of the Option type, where explicit handling of null and non-null values is critical.
    /// </summary>
    public class ValueIsNullException : ThrowAwayException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueIsNullException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error. This message typically explains why a non-null value was expected.</param>
        public ValueIsNullException(string message) : base(message)
        {
        }
    }
}
