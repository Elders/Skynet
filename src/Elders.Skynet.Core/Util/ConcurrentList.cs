using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Elders.Skynet.Core.Util
{
    /// <summary>
    /// We are using a ConcurrentDictionary as a list because .NET does not provide a thread safe list;
    /// </summary>
    public class ConcurrentList<T> : IEnumerable<T>
    {
        ConcurrentDictionary<T, T> items;

        public ConcurrentList()
        {
            items = new ConcurrentDictionary<T, T>();
        }

        public void Add(T item)
        {
            items.TryAdd(item, item);
        }

        public void Remove(T item)
        {
            T outItem;
            items.TryRemove(item, out outItem);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return items.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.Values.GetEnumerator();
        }
    }
}
