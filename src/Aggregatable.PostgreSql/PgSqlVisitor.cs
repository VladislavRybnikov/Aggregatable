using Aggregatable.Sql;
using System;
using System.Linq;

namespace Aggregatable.PostgreSql
{
    public class PgSqlVisitor : ISqlUpdateVisitor
    {
        public ISqlCommand Visit<T>(UpdateOf<T> update) where T : class => FromUpdate(update);

        private static PgSqlCommand FromUpdate<T>(UpdateOf<T> update, string tableName = null) where T : class
        {
            var type = typeof(T);
            tableName ??= type.GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault() is TableAttribute attr
                ? attr.Name
                : type.Name;

            var byName = ExpressionHelper.GetPropertyInfo(update.UpdateBy.Property).Name;

            var @params = update.Updates
                .Select(selector => selector switch 
                {
                    JsonbSelector<T> jsonb => VisitJsonbSelector(jsonb),
                    _ => VisitSelector(selector)
                })
                .ToDictionary(key => key.Item1, value => value.Item2);

            if (string.IsNullOrEmpty(byName) || update.UpdateBy.Value == null) throw new ArgumentNullException(
                "'By' should be provided.");

            var updates = string.Join(",", @params.Select(kv => $"{AsIdentifier(kv.Key)}={AsParam(kv.Key)}"));

            if (!@params.ContainsKey(byName)) throw new ArgumentException(
                $"'By' and 'Set' values can not have same field '{byName}'.");

            @params.Add(byName, update.UpdateBy.Value);

            var sql = $"UPDATE {AsTable(tableName)} SET {updates} WHERE {AsIdentifier(byName)}={AsParam(byName)}";

            return new PgSqlCommand
            {
                Text = sql,
                Params = @params
            };
        }

        private static (string, object) VisitSelector<T>(Selector<T> selector) 
            => (ExpressionHelper.GetPropertyInfo(selector.Property).Name, selector.Value);

        private static (string, object) VisitJsonbSelector<T>(JsonbSelector<T> selector) 
        {
            throw new NotImplementedException();
        }

        private static string AsTable(string str) => $"public.\"{str}\"";

        private static string AsParam(string str) => $"@{str}";

        private static string AsIdentifier(string str) => $"\"{str}\"";
    }
}
