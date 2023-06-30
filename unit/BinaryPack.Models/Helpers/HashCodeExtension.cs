#if NETFRAMEWORK
using System;
using System.Collections.Generic;

namespace BinaryPack.Models.Helpers
{
    public static class HashCode
    {
        public static int Combine(object one, object two)
        {
            return new { one, two }.GetHashCode();
        }
    }
}
#endif
