using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Aggregatable.Storage.InMemory
{
    public class InMemoryStorage : IAggregateStorageConnector
    {
        public ConcurrentDictionary<object, object> _storage = new ConcurrentDictionary<object, object>();

        public void Add<T>(object id, T value)
        {
            _storage.AddOrUpdate(id, value, (x, y) => value);
        }

        public T Get<T>(object id)
        {
            if (!_storage.TryGetValue(id, out var value) || !(value is T typed))
            {
                throw new Exception();
            }

            return typed;
        }

        public async Task HandleUpdateAsync<T>(UpdateOf<T> updateOf)
            where T : class
        {
            if (!_storage.TryGetValue(updateOf.UpdateBy.Value, out var value)) return;

            if (value is T)
            {
                foreach (var update in updateOf.Updates)
                {
                    if (update.Property.Body is MemberExpression memberSelectorExpression)
                    {
                        var property = memberSelectorExpression.Member as PropertyInfo;
                        if (property != null)
                        {
                            property.SetValue(value, update.Value, null);
                        }
                    }
                }
            }
        }
    }
}
