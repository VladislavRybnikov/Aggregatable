using System;
using System.Collections.Generic;
using System.Text;

namespace Aggregatable
{
    public interface IAmUpdateFrom<TMessage>
    {
        IAggregateUpdate Handle(TMessage message);
    }
}
