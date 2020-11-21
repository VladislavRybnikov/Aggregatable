using System.Threading.Tasks;

namespace Aggregatable.Storage
{
    public interface IAggregateStorageConnector
    {
        Task HandleUpdateAsync<T>(UpdateOf<T> update) where T : class;
    }
}
