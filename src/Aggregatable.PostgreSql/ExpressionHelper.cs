using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Aggregatable.PostgreSql
{
    internal static class ExpressionHelper
    {
        public static PropertyInfo GetPropertyInfo<T, TProperty>(Expression<Func<T, TProperty>> expression)
        {
            var type = typeof(T);

            if (!(expression.Body is MemberExpression member))
                throw new ArgumentException($"Expression '{expression}' refers to a method, not a property.");

            if (!(member.Member is PropertyInfo propInfo) || propInfo is null)
                throw new ArgumentException($"Expression '{expression}' refers to a field, not a property.");

            if (type != propInfo.ReflectedType && !type.IsSubclassOf(propInfo.ReflectedType))
                throw new ArgumentException($"Expression '{expression}' refers to a property that is not from type {type}.");

            return propInfo;
        }
    }
}
