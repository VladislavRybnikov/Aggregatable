using System;
using System.Collections.Generic;

namespace Aggregatable
{
    public abstract class AggregateRoot<T> where T : class
    {

        internal Dictionary<Type, Func<object, IAggregateUpdate>> _dynamicUpdates;

        protected void On<TMessage>(Func<TMessage, IAggregateUpdate> update) where TMessage : class
        {
            _dynamicUpdates ??= new Dictionary<Type, Func<object, IAggregateUpdate>>();
            _dynamicUpdates.Add(typeof(TMessage), m => update(m as TMessage));
        }

        protected IAggregateUpdate Update(Action<UpdateOf<T>> updateAction)
        {
            var update = new UpdateOf<T>();
            updateAction(update);
            return update;
        }
    }

    public class UpdateFrom<TMessage, TEntity> where TEntity : class
    {
        public IAggregateUpdate Update(Action<UpdateOf<TEntity>> updateAction) 
        {
            var update = new UpdateOf<TEntity>();
            updateAction(update);
            return update;
        }
    }
}
