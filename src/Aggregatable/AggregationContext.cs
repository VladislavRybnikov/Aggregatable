using Aggregatable.Storage;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Aggregatable
{

    public interface IAggregationContext 
    {
        Task SendAsync<TAggregate, TMessage>(TMessage message)
            where TAggregate : class
            where TMessage : class;
    }

    public class AggregationContext : IAggregationContext
    {
        private readonly IAggregateStorageConnector storageConnector;
        private readonly ConcurrentDictionary<Type, Lazy<object>> _aggregateRoots;

        public AggregationContext(Assembly assembly, IAggregateStorageConnector storageConnector) 
        {
            _aggregateRoots = new ConcurrentDictionary<Type, Lazy<object>>();
            var aggregateRootTypes = assembly.GetTypes().Where(t =>
            {
                return t.BaseType.IsGenericType && t.BaseType.GetGenericTypeDefinition() == typeof(AggregateRoot<>);
            });

            foreach (var root in aggregateRootTypes) 
            {
                
                var aggregateType = root.BaseType.GetGenericArguments().FirstOrDefault();
                if (aggregateType != null) 
                {
                    _aggregateRoots.TryAdd(aggregateType, new Lazy<object>(() => Activator.CreateInstance(root)));
                }
            }

            this.storageConnector = storageConnector;
        }

        public async Task SendAsync<TAggregate, TMessage>(TMessage message) 
            where TAggregate : class
            where TMessage : class
        {
            if (!_aggregateRoots.TryGetValue(typeof(TAggregate), out var lazyAggregateRoot))
            {
                throw new Exception();
            }
            if (!(lazyAggregateRoot.Value is IAmUpdateFrom<TMessage> updateHandler))
            {
                throw new Exception();
            }
            if (!(updateHandler.Handle(message) is UpdateOf<TAggregate> update))
            {
                throw new Exception();
            }

            await storageConnector.HandleUpdateAsync(update).ConfigureAwait(false);
        }
    }
}
