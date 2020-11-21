using Aggregatable.Sql;
using Aggregatable.Storage;
using Dapper;
using System.Data;
using System.Threading.Tasks;

namespace Aggregatable.Integrations.Dapper
{
    public class DapperStorageConnector : IAggregateStorageConnector
    {
        private readonly IDbConnection _dbConnection;
        private readonly ISqlUpdateVisitor _sqlVisitor;

        public DapperStorageConnector(IDbConnection dbConnection, ISqlUpdateVisitor sqlVisitor)
        {
            _dbConnection = dbConnection;
            _sqlVisitor = sqlVisitor;
        }

        public async Task HandleUpdateAsync<T>(UpdateOf<T> update) where T : class
        {
            var sqlCommand = _sqlVisitor.Visit(update);
            var commandDefinition = new CommandDefinition(sqlCommand.Text, new DynamicParameters(sqlCommand.Params));
            await _dbConnection.ExecuteAsync(commandDefinition);
        }
    }
}
