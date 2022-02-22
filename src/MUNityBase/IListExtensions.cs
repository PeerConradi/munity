using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Base
{
    public static class IListExtensions
    {
        public static void Swap<T>(this IList<T> list, int index1, int index2)
        {
            T temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
        }
    }
}
