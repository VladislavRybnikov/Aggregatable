using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Aggregatable
{
    public class Selector<T>
    {
        public readonly Expression<Func<T, object>> Property;

        public readonly object Value;

        public Selector(Expression<Func<T, object>> propertySelector, object newValue)
        {
            Property = propertySelector;
            Value = newValue;
        }
    }
}
