using MySqlConnector;
using OauthWebAPI.Models;
using System.Data;

namespace OauthWebAPI.DB
{
    public class DBRepository : IDbRepository
	{
        internal DbConnector Db { get; set; }
        public DBRepository(DbConnector db)
        {
            Db = db;
        }

        public async Task InsertAsync(User user)
        {
            Db.Connection.Open();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `users` (id,`username`, `password`,`last_access`) VALUES (@id,@username, @pass, @last_access);";
            BindParams(cmd, user);
            BindId(cmd, user.ID);
            await cmd.ExecuteNonQueryAsync();
            user.ID = (int)cmd.LastInsertedId;
            Db.Connection.Close();
        }

        public async Task UpdateAsync(User user)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `users` SET `username` = @username `Id` = @id;";
            BindParams(cmd, user);
            BindId(cmd, user.ID);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `users` WHERE `Id` = @id;";
            BindId(cmd, id);
            await cmd.ExecuteNonQueryAsync();
        }
        private void BindId(MySqlCommand cmd, int id)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
        }

        private void BindParams(MySqlCommand cmd, User user)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@username",
                DbType = DbType.String,
                Value = user.Username,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@pass",
                DbType = DbType.String,
                Value = user.Pass,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@last_access",
                DbType = DbType.DateTime,
                Value = user.LastAccess,
            });
        }
    }
}

