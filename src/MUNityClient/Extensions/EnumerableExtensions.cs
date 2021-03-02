using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityClient.Extensions
{
    public static class EnumerableExtensions
    {
        public static void Move<T>(this IList<T> list, int iIndexToMove,
        MoveDirection direction)
        {

            if (direction == MoveDirection.Up)
            {
                var old = list[iIndexToMove - 1];
                list[iIndexToMove - 1] = list[iIndexToMove];
                list[iIndexToMove] = old;
            }
            else
            {
                var old = list[iIndexToMove + 1];
                list[iIndexToMove + 1] = list[iIndexToMove];
                list[iIndexToMove] = old;
            }
        }

        public static void Swap<T>(this IList<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }
    }

    

    public enum MoveDirection
    {
        Up,
        Down
    }
}
