using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Xtreem.Crusader.Utilities.Extensions
{
    public static class EnumerableExtensions
    {
        public static ReadOnlyCollection<T> AsReadOnly<T>(this IEnumerable<T> items) => Array.AsReadOnly(items.ToArrayOrCast());

        public static T[] ToArrayOrCast<T>(this IEnumerable<T> items) => items as T[] ?? items.ToArray();

        public static IEnumerable<TResult> Zip<T, TResult>(this IEnumerable<IEnumerable<T>> collections, Func<IEnumerable<T>, TResult> resultSelector)
        {
            var enumerators = collections.Select(s => s.GetEnumerator()).ToArray();
            while (enumerators.All(e => e.MoveNext()))
            {
                yield return resultSelector(enumerators.Select(e => e.Current));
            }
        }
    }
}
