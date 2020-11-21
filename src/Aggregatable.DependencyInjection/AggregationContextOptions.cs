using Aggregatable.Sql;
using Aggregatable.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Aggregatable.DependencyInjection
{
    public class AggregationContextOptions
    {
        private readonly IServiceCollection _services;

        public AggregationContextOptions(IServiceCollection services)
        {
            _services = services;
        }

        public AggregationContextOptions UseStorageConnector<TStorageConnector>()
            where TStorageConnector : class, IAggregateStorageConnector
        {
            _services.AddSingleton<IAggregateStorageConnector, TStorageConnector>();
            return this;
        }

        public AggregationContextOptions UseSqlVisitor<TSqlVisitor>() where TSqlVisitor : class, ISqlUpdateVisitor
        {
            _services.AddSingleton<ISqlUpdateVisitor, TSqlVisitor>();
            return this;
        }
    }
}
