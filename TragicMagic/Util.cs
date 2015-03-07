using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// J. Brown @DrMelon
// 07/03/2015
// Utility functions, like a swap function.

namespace TragicMagic
{
    public static class Utilities
    {
        public static void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp;
            temp = lhs;
            lhs = rhs;
            rhs = temp;
        }
    }
}
