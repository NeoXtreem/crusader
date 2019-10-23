using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Xtreem.Crusader.Utilities
{
    public static class EnumerableExtensions
    {
        public static ReadOnlyCollection<T> AsReadOnly<T>(this IEnumerable<T> items)
        {
            return Array.AsReadOnly(items.ToArrayOrCast());
        }

        private static T[] ToArrayOrCast<T>(this IEnumerable<T> items)
        {
            return items as T[] ?? items.ToArray();
        }
    }
}
