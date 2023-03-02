using Npgsql;
using MonsterTradingCardsGame.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.DAL
{
    internal class DatabaseMessageDao : DatabaseBaseDao, IMessageDao
    {
        private const string InsertMessageCommand = "INSERT INTO messages(content, username) VALUES (@content, @username) RETURNING id";
        private const string DeleteMessageCommand = "DELETE FROM messages WHERE id=@id AND username=@username";
        private const string UpdateMessageCommand = "UPDATE messages SET content=@content WHERE id=@id AND username=@username";
        private const string SelectMessageByIdCommand = "SELECT id, content FROM messages WHERE id=@id AND username=@username";
        private const string SelectMessagesCommand = "SELECT id, content FROM messages WHERE username=@username";

        public DatabaseMessageDao(string connectionString) : base(connectionString)
        {
        }

        public void DeleteMessage(string username, int messageId)
        {
            ExecuteWithDbConnection((connection) =>
            {
                using var cmd = new NpgsqlCommand(DeleteMessageCommand, connection);
                cmd.Parameters.AddWithValue("id", messageId);
                cmd.Parameters.AddWithValue("username", username);
                return cmd.ExecuteNonQuery();
            });
        }

        public Message? GetMessageById(string username, int messageId)
        {
            return ExecuteWithDbConnection((connection) =>
            {
                Message? message = null;

                using var cmd = new NpgsqlCommand(SelectMessageByIdCommand, connection);
                cmd.Parameters.AddWithValue("id", messageId);
                cmd.Parameters.AddWithValue("username", username);

                // take the first row, if any
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    message = ReadMessage(reader);
                }

                return message;
            });
        }

        public IEnumerable<Message> GetMessages(string username)
        {
            return ExecuteWithDbConnection((connection) =>
            {
                var messages = new List<Message>();

                using var cmd = new NpgsqlCommand(SelectMessagesCommand, connection);
                cmd.Parameters.AddWithValue("username", username);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var message = ReadMessage(reader);
                    messages.Add(message);
                }

                return messages;
            });
        }

        public void InsertMessage(string username, Message message)
        {
            ExecuteWithDbConnection((connection) =>
            {
                using var cmd = new NpgsqlCommand(InsertMessageCommand, connection);
                cmd.Parameters.AddWithValue("content", message.Content);
                cmd.Parameters.AddWithValue("username", username);

                // ExecuteScalar returns a single value (see InsertMessageCommand, it's the newly assigned ID)
                var result = cmd.ExecuteScalar();

                message.Id = Convert.ToInt32(result);
                return message;
            });
        }

        public bool UpdateMessage(string username, Message message)
        {
            return ExecuteWithDbConnection((connection) =>
            {
                using var cmd = new NpgsqlCommand(UpdateMessageCommand, connection);
                cmd.Parameters.AddWithValue("id", message.Id);
                cmd.Parameters.AddWithValue("content", message.Content);
                cmd.Parameters.AddWithValue("username", username);

                // ExecuteNonQuery returns the number of affected rows
                return cmd.ExecuteNonQuery() > 0;
            });
        }

        private Message ReadMessage(IDataRecord record)
        {
            return new Message(Convert.ToInt32(record["id"]), Convert.ToString(record["content"])!);
        }
    }
}
