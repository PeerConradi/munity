using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;

namespace MUNity.Extensions.ObservableCollectionExtensions
{
    /// <summary>
    /// Extension methods that allow to work with observableCollections like linq.
    /// </summary>
    public static class ObservableCollectionTools
    {
        /// <summary>
        /// Removes all entries with the given Predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="predicate"></param>
        public static int RemoveAll<T>(this ObservableCollection<T> collection, Func<T, bool> predicate)
        {
            var toDelete = collection.Where(predicate).ToList();
            int count = toDelete.Count;
            foreach(var deleteMe in toDelete)
            {
                collection.Remove(deleteMe);
            }
            toDelete.Clear();
            return count;
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> query) where T : class
        {
            var list = new ObservableCollection<T>();
            foreach (var item in query)
            {
                list.Add(item);
            }
            return list;
        }
    }
}
