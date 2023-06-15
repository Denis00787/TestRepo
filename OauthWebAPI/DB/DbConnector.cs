using MySqlConnector;

namespace OauthWebAPI.DB
{
    public class DbConnector : IDisposable
    {
        public MySqlConnection Connection { get; }

        public DbConnector(string connectionString)
        {
            Connection = new MySqlConnection(connectionString);
        }

        public void Dispose() => Connection.Dispose();
    }
}

