using System.Data;
using System.Data.SqlClient;

namespace CL.ReportGenerator
{
    public class Connection : IConnection
    {
        public Connection(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; }

        IDbConnection IConnection.Connection
        {
            get => new SqlConnection(ConnectionString);
        }
    }
}
