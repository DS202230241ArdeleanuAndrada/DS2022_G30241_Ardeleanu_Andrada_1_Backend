using System.Data;

namespace App.Data
{
    public class DbConnectionFactory<TDbConnection>
         where TDbConnection : IDbConnection, new()
    {
        private readonly string _connectionString;
        public DbConnectionFactory(string connectionString)
        {
            this._connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public TDbConnection GetConnection()
        {
            try
            {
                var dbConn = Connect(_connectionString);
                if (dbConn.State.HasFlag(ConnectionState.Open))
                {
                    return dbConn;
                }

                dbConn.Close();

            }
            catch (Exception ex)
            {
                throw;
            }

            throw new InvalidOperationException("Failed to get connection");
        }

        public TDbConnection Connect(string connectionString)
        {
            try
            {

                TDbConnection dbConn = Activator.CreateInstance<TDbConnection>();
                dbConn.ConnectionString = connectionString;
                dbConn.Open();

                return dbConn;

            }

            catch (Exception ex)
            {
                throw;
            }
        }
    }
}