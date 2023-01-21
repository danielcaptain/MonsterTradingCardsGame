using Npgsql;
using SWE1.MonsterTradingCardsGame.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MonsterTradingCardsGame.DAL
{
    internal class DatabaseUserDao : DatabaseBaseDao, IUserDao
    {
        private const string InsertUserCommand = "INSERT INTO users(username, password) VALUES (@username, @password)";
        private const string SelectUsersCommand = "SELECT username, password FROM users";
        private const string SelectUserByCredentialsCommand = "SELECT username, password FROM users WHERE username=@username AND password=@password";


        public DatabaseUserDao(string connectionString) : base(connectionString)
        {
        }

        private List<User> GetAllUsers()
        {
            return ExecuteWithDbConnection((connection) =>
            {
                var users = new List<User>();

                using var cmd = new NpgsqlCommand(SelectUsersCommand, connection);
                
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var user = ReadUser(reader);
                    users.Add(user);
                }

                return users;
            });
        }

        public User? GetUserByAuthToken(string authToken)
        {
            return GetAllUsers().SingleOrDefault(u => u.Token == authToken);
        }

        public User? GetUserByCredentials(string username, string password)
        {
            return ExecuteWithDbConnection((connection) =>
            {
                User? user = null;

                using var cmd = new NpgsqlCommand(SelectUserByCredentialsCommand, connection);
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("password", password);

                // take the first row, if any
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    user = ReadUser(reader);
                }

                return user;
            });
        }

        public bool InsertUser(User user)
        {
            return ExecuteWithDbConnection((connection) =>
            {
                var affectedRows = 0;
                try
                {
                    using var cmd = new NpgsqlCommand(InsertUserCommand, connection);
                    cmd.Parameters.AddWithValue("username", user.Username);
                    cmd.Parameters.AddWithValue("password", user.Password);
                    affectedRows = cmd.ExecuteNonQuery();
                }
                catch (PostgresException)
                {
                    // this might happen, if the user already exists (constraint violation)
                    // we just catch it an keep affectedRows at zero
                }

                return affectedRows > 0;
            });
        }

        private User ReadUser(IDataRecord record)
        {
            return new User(Convert.ToString(record["username"])!, Convert.ToString(record["password"])!);
        }
    }
}
