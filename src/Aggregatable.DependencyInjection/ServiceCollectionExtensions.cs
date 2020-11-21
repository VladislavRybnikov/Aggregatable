using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aggregatable.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAggregationContext<TContext>(this IServiceCollection services,
            Action<AggregationContextOptions> options = null) where TContext : IAggregationContext
        {

            return services;
        }
    }
}
