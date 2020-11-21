using System;
using System.Collections.Generic;
using System.Text;

namespace Aggregatable.Sql
{
    public interface ISqlCommand
    {
        string Text { get; set; }

        Dictionary<string, object> Params { get; set; }
    }
}
