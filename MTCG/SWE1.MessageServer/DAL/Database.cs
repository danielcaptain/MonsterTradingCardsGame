using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MonsterTradingCardsGame.DAL
{
    internal class Database
    {
        // see also https://www.postgresql.org/docs/current/ddl-constraints.html
        private const string CreateTablesCommand = @"
CREATE TABLE IF NOT EXISTS users (username varchar PRIMARY KEY, password varchar);
CREATE TABLE IF NOT EXISTS messages (id serial PRIMARY KEY, content varchar, username varchar REFERENCES users ON DELETE CASCADE);
";

        public IMessageDao MessageDao { get; private set; }
        public IUserDao UserDao { get; private set; }

        public Database(string connectionString)
        {
            try
            {
                try
                {
                    // https://github.com/npgsql/npgsql/issues/1837https://github.com/npgsql/npgsql/issues/1837
                    // https://www.npgsql.org/doc/basic-usage.html
                    // https://www.npgsql.org/doc/connection-string-parameters.html#pooling
                    EnsureTables(connectionString);
                }
                catch (NpgsqlException e)
                {
                    // provide our own custom exception
                    throw new DataAccessFailedException("Could not connect to or initialize database", e);
                }

                UserDao = new DatabaseUserDao(connectionString);
                MessageDao = new DatabaseMessageDao(connectionString);
            }
            catch (NpgsqlException e)
            {
                // provide our own custom exception
                throw new DataAccessFailedException("Could not connect to or initialize database", e);
            }
        }

        private void EnsureTables(string connectionString)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(CreateTablesCommand, connection);
            cmd.ExecuteNonQuery();
        }
    }
}
