namespace ThrowAway.Extensions;

/// <summary>
/// Provides extension methods for attempting to unwrap an <see cref="Option{T}"/>.
/// </summary>
public static class MaybeTryUnwrapExtensions
{
    /// <summary>
    /// Attempts to extract the value from the specified <see cref="Option{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value contained in the option.</typeparam>
    /// <param name="option">The <see cref="Option{T}"/> instance to unwrap.</param>
    /// <param name="value">
    /// When this method returns, contains the unwrapped value if the option has one;
    /// otherwise, the default value of <typeparamref name="T"/>.
    /// </param>
    /// <param name="failure">
    /// When this method returns, contains the failure message if the option does not have a value;
    /// otherwise, <c>null</c>.
    /// </param>
    /// <returns>
    /// <c>true</c> if the option has a value; otherwise, <c>false</c>.
    /// </returns>
    public static bool TryUnwrap<T>(this Option<T> option, out T value, out string failure)
    {
        if (option.HasValue)
        {
            value = option.Value;
            failure = null!;
            return true;
        }
        else
        {
            value = default!;
            failure = option.Failure;
            return false;
        }
    }
}
