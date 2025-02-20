﻿using static ThrowAway.Helpers;

namespace ThrowAway.Extensions;

/// <summary>
/// Provides extension methods for attempting to unwrap an <see cref="Option{V, F}"/>,
/// extracting either the success value or the failure value.
/// </summary>
public static class EitherTryUnwrapExtensions
{
    /// <summary>
    /// Attempts to extract the success value from the specified <see cref="Option{V, F}"/>.
    /// </summary>
    /// <typeparam name="V">The type of the success value contained in the option.</typeparam>
    /// <typeparam name="F">The type of the failure value contained in the option.</typeparam>
    /// <param name="option">The <see cref="Option{V, F}"/> instance to unwrap.</param>
    /// <param name="value">
    /// When this method returns, contains the success value if the option has one; 
    /// otherwise, the default value of <typeparamref name="V"/>.
    /// </param>
    /// <param name="failure">
    /// When this method returns, contains the failure value if the option does not have a success value;
    /// otherwise, the default value of <typeparamref name="F"/>.
    /// </param>
    /// <returns>
    /// <c>true</c> if the option has a success value; otherwise, <c>false</c>.
    /// </returns>
    public static bool TryUnwrap<V, F>(this Option<V, F> option, out V value, out F failure)
    {
        if (option.HasValue)
        {
            value = option.Value;
            failure = default!;
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
