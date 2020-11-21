using Aggregatable.Sql;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aggregatable.PostgreSql
{
    public class PgSqlCommand : ISqlCommand
    {
        public string Text { get; set; }

        public Dictionary<string, object> Params { get; set; }
    }
}
