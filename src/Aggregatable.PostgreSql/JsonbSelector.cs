using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Aggregatable.PostgreSql
{
    public class JsonbSelector<T> : Selector<T>
    {
        public string JsonbProperty { get; set; }

        public JsonbSelector(Expression<Func<T, object>> propertySelector, string name, object newValue) : base(propertySelector, newValue)
        {
            JsonbProperty = name;
        }
    }
}
