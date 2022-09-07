#if NETFRAMEWORK
using System;
using System.Collections.Generic;

namespace BinaryPack.Models.Helpers
{
    public static class LinQExtension
    {
        public static IEnumerable<(TFirst First, TSecond Second)> Zip<TFirst, TSecond>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second)
        {
            if (first is null)
            {
                throw new ArgumentException("IEnumerable was null.", nameof(first));
            }

            if (second is null)
            {
                throw new ArgumentException("IEnumerable was null.", nameof(second));
            }

            using (IEnumerator<TFirst> e1 = first.GetEnumerator())
            using (IEnumerator<TSecond> e2 = second.GetEnumerator())
            {
                while (e1.MoveNext() && e2.MoveNext())
                {
                    yield return (e1.Current, e2.Current);
                }
            }
        }
    }
}
#endif
