using System;
using System.Collections.Generic;
using System.Text;

namespace Aggregatable.Sql
{
    public interface ISqlUpdateVisitor
    {
        ISqlCommand Visit<T>(UpdateOf<T> update) where T : class;
    }
}
