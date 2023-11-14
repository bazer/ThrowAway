using System.Collections.Generic;
using System.Linq;

namespace ThrowAway.Extensions;

/// <summary>
/// Provides extension methods for transposing collections of Options in the ThrowAway library.
/// Transposing in this context refers to converting a collection of Options into a single Option
/// containing a collection, based on the success or failure state of the individual Options.
/// </summary>
public static class MaybeTransposeExtensions
{
    /// <summary>
    /// Transposes a collection of Options, turning an IEnumerable&lt;Option&lt;V&gt;&gt; into an Option&lt;List&lt;V&gt;, List&lt;string&gt;&gt;.
    /// If any Option in the collection has failed, the result is a failed Option containing a list of all failures.
    /// Otherwise, the result is a successful Option containing a list of all values.
    /// </summary>
    /// <typeparam name="V">The type of the value in the Option.</typeparam>
    /// <param name="options">The collection of Options to transpose.</param>
    /// <returns>An Option containing either a list of values or a list of failures.</returns>
    public static Option<List<V>, List<string>> Transpose<V>(this IEnumerable<Option<V>> options)
    {
        var list = options.ToList();

        if (list.Any(x => x.HasFailed))
        {
            return Option.Fail<List<V>, List<string>>(list
                .Where(x => x.HasFailed)
                .Select(x => x.FailureOrException())
                .ToList());
        }
        else
        {
            return Option.Some<List<V>, List<string>>(list
                .Select(x => x.Value)
                .ToList());
        }
    }
}
