using System.Collections.Generic;
using System.Linq;

namespace GovEventer.Core
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> FirstEmpty<T>(this IEnumerable<T> enumerable) where T : class , new()
        {
            return new T[] { null }.Concat(enumerable);
        }
    }
}