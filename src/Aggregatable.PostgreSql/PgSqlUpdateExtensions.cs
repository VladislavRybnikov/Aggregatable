using Aggregatable.PostgreSql;
using System;
using System.Linq.Expressions;

namespace Aggregatable.PostgreSqlConnector
{
    public static class PgSqlUpdateExtensions
    {
        private class JsonbUpdateOf<T> : UpdateOf<T> where T : class
        {
            public JsonbUpdateOf(UpdateOf<T> wrapped, JsonbSelector<T> selector) : base()
            {
                _idSelector = wrapped.UpdateBy;
                _updates.AddRange(wrapped.Updates);
                _updates.Add(selector);
            }
        }

        public static UpdateOf<T> SetJsonb<T>(
            this UpdateOf<T> updateOf,
            Expression<Func<T, object>> propertySelector,
            string jsonbPropertyName,
            object value) where T : class 
            => new JsonbUpdateOf<T>(updateOf, new JsonbSelector<T>(propertySelector, jsonbPropertyName, value));

        public static UpdateOf<T> SetJsonb<T, TProperty>(
            this UpdateOf<T> updateOf,
            Expression<Func<T, TProperty>> propertySelector,
            Expression<Func<TProperty, object>> jsonbPropertySelector,
            object value) where T : class
        {
            return updateOf;
        }
    }
}
