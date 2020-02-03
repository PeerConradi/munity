using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Util.Extenstions
{
    public static class ListExtensions
    {
        public static void Move<T>(this List<T> list, int oldIndex, int newIndex)
        {
            T item = list[oldIndex];
            list.RemoveAt(oldIndex);
            list.Insert(newIndex, item);
        }

        public static string ToNewtonsoftJson<T>(this List<T> list)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(list);
        } 
    }
}
