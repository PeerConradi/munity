using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Services.Extensions
{
    public static class QueryableExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
        {
            var collection = new ObservableCollection<T>();
            foreach(var item in source)
            {
                collection.Add(item);
            }
            return collection;
        }
    }
}
