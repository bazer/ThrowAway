namespace ThrowAway
{
    /// <summary>
    /// Provides configuration settings for the ThrowAway library.
    /// This class allows customization of certain behaviors within the library, 
    /// particularly those related to error handling and debugging.
    /// </summary>
    public static class OptionConfig
    {
        /// <summary>
        /// Gets or sets a boolean value indicating whether to log the stack trace when a failure occurs within the library.
        /// When set to true, this setting enables detailed logging of stack trace information, 
        /// aiding in debugging scenarios especially when dealing with issues related to the Option type operations.
        /// </summary>
        public static bool LogStackTraceOnFailure { get; set; }
    }
}
