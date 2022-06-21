using System.Collections;
using System.Collections.Concurrent;

namespace AddIns
{
    public class ConcurrentList<T> : IEnumerable<T>
    {
        private readonly ConcurrentDictionary<Guid, T> _concurrentDict = new ConcurrentDictionary<Guid, T>();


        public void Add(T item)
        {
            _concurrentDict.AddOrUpdate(Guid.NewGuid(), item, (k, v) => item);
        }

        public void Remove(T item)
        {
            var deleteValue = _concurrentDict.Values.FirstOrDefault(i => i.Equals(item));
            if (deleteValue != null)
            {
                var deleteKey = _concurrentDict.FirstOrDefault(i => i.Value!.Equals(item)).Key;
                _concurrentDict.Remove(deleteKey, out deleteValue);
            }
        }

        public void Clear()
        {
            _concurrentDict.Clear();
        }

        public IReadOnlyCollection<T> GetAll()
        {
            var readonlyBag = _concurrentDict.Values as IReadOnlyCollection<T>;
            return readonlyBag;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _concurrentDict.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _concurrentDict.Values.GetEnumerator();
        }
    }
}
