using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Xtreem.Crusader.Utilities.Extensions
{
    public static class EnumerableExtensions
    {
        public static ReadOnlyCollection<T> AsReadOnly<T>(this IEnumerable<T> items) => Array.AsReadOnly(items.ToArrayOrCast());

        private static T[] ToArrayOrCast<T>(this IEnumerable<T> items) => items as T[] ?? items.ToArray();
    }
}
