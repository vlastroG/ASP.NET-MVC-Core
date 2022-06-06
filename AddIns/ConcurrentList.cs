using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddIns
{
    public class ConcurrentList<T> : IEnumerable<T>
    {
        private readonly ConcurrentDictionary<int, T> _concurrentDict = new ConcurrentDictionary<int, T>();
        private int _lastKey = 0;
        private Object _syncObj = new();


        public void Add(T item)
        {
            lock (_syncObj)
            {
                _concurrentDict.AddOrUpdate(_lastKey, item, (k, v) => item);
                _lastKey++;
            }
        }

        public void Remove(T item)
        {
            lock (_syncObj)
            {
                var delete_value = _concurrentDict.Values.FirstOrDefault(i => i.Equals(item));
                if (delete_value != null)
                {
                    var delete_key = _concurrentDict.FirstOrDefault(i => i.Value!.Equals(item)).Key;
                    _concurrentDict.Remove(delete_key, out delete_value);
                }
            }
        }

        public void Clear()
        {
            lock (_syncObj)
            {
                _concurrentDict.Clear();
            }
        }

        public IReadOnlyCollection<T> GetAll()
        {
            lock (_syncObj)
            {
                var readonly_bag = _concurrentDict.Values as IReadOnlyCollection<T>;
                return readonly_bag;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            lock (_syncObj)
            {
                return _concurrentDict.Values.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            lock (_syncObj)
            {
                return _concurrentDict.Values.GetEnumerator();
            }
        }
    }
}
