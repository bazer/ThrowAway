using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThrowAway
{
    internal static class Helpers
    {
        internal static bool IsNull<T>(T value)
        {
            return value == null;
        }

        
    }
}
