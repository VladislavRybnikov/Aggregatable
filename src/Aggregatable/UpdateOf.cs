using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Aggregatable
{
    public interface IAggregateUpdate { }

    public class UpdateOf<T> : IAggregateUpdate where T : class 
    {
        protected Selector<T> _idSelector;

        protected readonly List<Selector<T>> _updates;

        public Selector<T> UpdateBy => _idSelector;

        public IEnumerable<Selector<T>> Updates => _updates;

        internal protected UpdateOf() 
        {
            _updates = new List<Selector<T>>();
        }

        protected UpdateOf<T> AddSelector(Selector<T> value) 
        {
            _updates.Add(value);
            return this;
        }

        public UpdateOf<T> Set(Expression<Func<T, object>> propertySelector, object value) 
            => AddSelector(new Selector<T>(propertySelector, value));

        public UpdateOf<T> By(Expression<Func<T, object>> idSelector, object value)
        {
            _idSelector = new Selector<T>(idSelector, value);
            return this;
        }
    }
}
