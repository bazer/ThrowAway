using System;

namespace ThrowAway;

/// <summary>
/// Represents a base exception type for the ThrowAway library. This custom exception class is 
/// the root for other more specific exceptions within the library, providing a common base 
/// for all custom exceptions. It enhances the ability to catch and handle library-specific 
/// exceptions distinctly from other .NET exceptions.
/// </summary>
public class ThrowAwayException : Exception
{
    /// <summary>
    /// Initializes a new instance of the ThrowAwayException class with a specific error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public ThrowAwayException(string message) : base(message)
    {
    }
}
