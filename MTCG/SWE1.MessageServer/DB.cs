using MonsterTradingCardsGame.Models;
using Npgsql;
using System.Data;
using System.Reflection.PortableExecutable;
using System.Text;
using static MonsterTradingCardsGame.Models.Card;
using static MonsterTradingCardsGame.Models.MonsterCard;

namespace MonsterTradingCardsGame
{
    public class DB
    {
        private readonly NpgsqlConnection con;
        private static readonly object _lock = new();
        public DB()
        {
            string cs = "Host=localhost;Username=postgres;Password=admin;Database=mtcg";
            con = new NpgsqlConnection(cs);
        }

        // Connects to the database.
        public void Connect()
        {
            try
            {
                con.Open();
            }
            catch (Exception)
            {
                Console.WriteLine("ÄNDERN");
                System.Environment.Exit(-1);
            }
        }

        // Clears the tokens 
        public void InitDB()
        {
            string cmd = @"UPDATE player 
                         SET token = NULL
                         WHERE token IS NOT NULL;";
            using NpgsqlCommand sqlcmd = new(cmd, con);
            sqlcmd.ExecuteNonQuery();
        }


        // Closes the database connection
        public void Close()
        {
            con.Close();
        }

        // Creates a user in the db.
        public bool CreateUser(string username, string pwd)
        {
            if (username == null || pwd == null)
            {
                Console.WriteLine("Missing username or password");
                return false;
            }

            string cmd = @"
                INSERT INTO player 
                    (id, name, coins, password)
                VALUES
                    (default, @name, 20, crypt(@password, gen_salt('bf')));";
            using NpgsqlCommand sqlcmd = new(cmd, con);
            sqlcmd.Parameters.AddWithValue("@name", username);
            sqlcmd.Parameters.AddWithValue("@password", pwd);

            return sqlcmd.ExecuteNonQuery() > 0;
        }


        // Checks if User already exists in DB -> Username has to be unique
        public virtual bool UserExists(string username)
        {
            string cmd = @"SELECT id
                            FROM player
                            WHERE name = @username;
                          ";
            lock (_lock)
            {
                NpgsqlCommand sqlcmd = new(cmd, con);
                sqlcmd.Parameters.AddWithValue("@username", username);
                using var reader = sqlcmd.ExecuteReader();
                return reader.HasRows;
            }
        }
        
        // Returns a User with his Values from DB
        public virtual User? GetUserInformation(string token)
        {
            if (string.IsNullOrEmpty(token))
                return null;

            string cmd = @"SELECT *
                            FROM player
                            WHERE token = @token;
                          ";
            lock (_lock)
            {
                using NpgsqlCommand sqlcmd = new(cmd, con);
                sqlcmd.Parameters.AddWithValue("@token", token);
                using var reader = sqlcmd.ExecuteReader();

                User constructUser = new User("", "", 0, "", "", "");
                while (reader.Read())
                {
                    constructUser = new User(reader.GetString(1), reader.GetString(4), reader.GetInt32(2), reader.IsDBNull(5) ? "" : reader.GetString(5), reader.IsDBNull(6) ? "" : reader.GetString(6), reader.IsDBNull(7) ? "" : reader.GetString(7), reader.GetString(3));
                }
                reader.Close();
                return constructUser;
            }
        }

        // Checks if Username and Password matches the DB Data 
        public bool VerifyLogin(string username, string password)
        {
            if (password == null)
            {
                return false;
            }

            string cmd = @"SELECT id
                            FROM player
                            WHERE name = @username
                            AND password = crypt(@password, password);
                           ";

            lock (_lock)
            {
                using NpgsqlCommand sqlcmd = new(cmd, con);
                sqlcmd.Parameters.AddWithValue("@username", username);
                sqlcmd.Parameters.AddWithValue("@password", password);

                using var reader = sqlcmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Close();
                    reader.Dispose();
                    string tokencmd = @"UPDATE player
                                    set token = @token
                                    WHERE name = @username
                                    ;";
                    using NpgsqlCommand sqltokenCommand = new(tokencmd, con);
                    sqltokenCommand.Parameters.AddWithValue("@token", User.CreateToken(username));
                    sqltokenCommand.Parameters.AddWithValue("@username", username);
                    return (sqltokenCommand.ExecuteNonQuery() > 0);
                }
            }
            return false;
        }

        // Edits the User Data
        public bool EditUser(User jsonuser, string username)
        {
            string cmd = @"Update player
                           SET alias = @alias,
                               bio = @bio,
                               image = @image
                           WHERE name = @name;";
            using NpgsqlCommand sqlcmd = new(cmd, con);

            Console.WriteLine("IN DER DATENBANK:" + jsonuser.Aliasname + " " + jsonuser.Image);
            sqlcmd.Parameters.AddWithValue("@alias", jsonuser.Aliasname);
            sqlcmd.Parameters.AddWithValue("@bio", jsonuser.Bio);
            sqlcmd.Parameters.AddWithValue("@image", jsonuser.Image);
            sqlcmd.Parameters.AddWithValue("@name", username);

            return sqlcmd.ExecuteNonQuery() > 0;
        }

        // Adds a monstercard object into the DB
        private bool AddMonsterCard(MonsterCard monsterCard)
        {
            string cmd = @"INSERT INTO card 
                                (id, type, name, damage, ismonster)
                            VALUES
                                (@id, @type, @name, @damage, TRUE);";
            using NpgsqlCommand sqlcmd = new(cmd, con);
            sqlcmd.Parameters.AddWithValue("@id", monsterCard.Id);
            sqlcmd.Parameters.AddWithValue("@type", (int)monsterCard.Monster);
            sqlcmd.Parameters.AddWithValue("@name", monsterCard.Name);
            sqlcmd.Parameters.AddWithValue("@damage", monsterCard.Damage);

            return sqlcmd.ExecuteNonQuery() > 0;
        }

        // Adds a spellcard object into the DB
        private bool AddSpellCard(SpellCard spellCard)
        {
            string cmd = @"INSERT INTO card 
                                (id, type, name, damage, ismonster)
                            VALUES
                                (@id, NULL, @name, @damage, FALSE);";

            using NpgsqlCommand sqlcmd = new(cmd, con);
            sqlcmd.Parameters.AddWithValue("@id", spellCard.Id);
            sqlcmd.Parameters.AddWithValue("@name", spellCard.Name);
            sqlcmd.Parameters.AddWithValue("@damage", spellCard.Damage);

            return sqlcmd.ExecuteNonQuery() > 0;
        }

        // Creates a package and adds Cards after it verifies if its a Monster/Spell Card with the AddMonsterCard or AddSpellCard
        public bool CreatePackage(List<Card> cards)
        {
            bool failure = false;
            if (cards.Count != 5)
                return false;
            foreach (Card card in cards)
            {
                if (card is MonsterCard monstercard)
                {
                    AddMonsterCard(monstercard);
                }else if (card is SpellCard spellcard)
                {
                    AddSpellCard(spellcard);
                }
            }

            List<Guid> ids = cards.Select(m => m.Id).ToList();
            string cmd = @"INSERT INTO cardpackage
                                (id, c0id, c1id, c2id, c3id, c4id)
                            VALUES
                                (default, @c0id, @c1id, @c2id, @c3id, @c4id);";
            using NpgsqlCommand sqlcmd = new(cmd, con);

            // einzeln statt loop
            for (int i = 0; i < 5; i++)
            {
                sqlcmd.Parameters.AddWithValue("@c" + i + "id",
                    ids[i]); 
            }
            return (!failure && sqlcmd.ExecuteNonQuery() > 0);
        }

        // Returns cards which already exist in the DB and have the same GUID as the provided list of cards
        public List<Guid>? CardsExistAlready(List<Card> package)
        {
            List<Guid> ids = package.Select(m => m.Id).ToList();
            StringBuilder sb = new();

            for (int i = 0; i < ids.Count; i++)
            {
                sb.Append("id = @guid" + i + " OR ");       
            }

            sb.Remove(sb.Length - 4, 4); //Removes the last 4 characters (Space,O,R,Space): " OR "
            string cmd = sb.ToString();

            cmd = string.Format("SELECT id FROM card WHERE {0}", cmd);

            lock (_lock)
            {
                using NpgsqlCommand sqlcmd = new(cmd, con);
                for (int i = 0; i < ids.Count; i++)
                {
                    sqlcmd.Parameters.Add("@guid" + i, NpgsqlTypes.NpgsqlDbType.Uuid);
                    sqlcmd.Parameters["@guid" + i].Value = ids[i];
                }

                using var reader = sqlcmd.ExecuteReader();
                if (reader.HasRows)
                {
                    List<Guid> dbGuids = new();
                    while (reader.Read())
                    {
                        Guid dbGuid = reader.GetGuid(0);
                        dbGuids.Add(dbGuid);
                    }
                    reader.Close();
                    return dbGuids;
                }
                reader.Close();
                return null;
            }
        }

        /// Gets the first package and adds the cards to the users stack -> deletes the package afterwards
        public virtual bool GetPackage(User user)
        {
            bool packageavailable = false;
            Guid[] cardGuids = new Guid[5];
            int pid = -1;                   //package ID which is 

            string cmd = @"SELECT * FROM cardpackage
                            LIMIT 1;";

            lock (_lock)
            {
                using (NpgsqlCommand sqlcmd = new(cmd, con))
                {
                    using var reader = sqlcmd.ExecuteReader();
                    while (reader.Read())
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            cardGuids[i] = reader.GetGuid(i + 1);
                            if (cardGuids[i].Equals(Guid.Empty))
                                return false;
                        }
                        pid = reader.GetInt32(0);
                    }
                    packageavailable = reader.HasRows;
                    reader.Close();
                }
            }
            if (pid == -1)
                return false;

            // Delete the package 
            if (packageavailable)
            {
                string deletecmd = @"DELETE FROM cardpackage
                            WHERE id = @id;";

                using NpgsqlCommand sqldeletecmd = new(deletecmd, con);
                sqldeletecmd.Parameters.AddWithValue("@id", pid);
                if (sqldeletecmd.ExecuteNonQuery() < 1)
                    return false;
            }

            // Add the cards to the users stack
            if (packageavailable)
            {
                for (int i = 0; i < cardGuids.Length; i++)
                {
                    string updatecmd = @"INSERT INTO stack
                                    (id, player, card, partofdeck)
                                VALUES
                                    (default, @name, @cid, FALSE);";

                    using (NpgsqlCommand sqlupdatecmd = new(updatecmd, con))
                    {
                        sqlupdatecmd.Parameters.AddWithValue("@name", user.Username);
                        sqlupdatecmd.Parameters.AddWithValue("@cid", cardGuids[i]);
                        if (sqlupdatecmd.ExecuteNonQuery() < 0)
                            return false;
                    }
                }

                // Subtract five coins from the user:
                string updateCoincmd = @"UPDATE player
                                        SET coins = @coins
                                        WHERE name = @name;";
                using (NpgsqlCommand sqlupdateCoincmd = new(updateCoincmd, con))
                {
                    sqlupdateCoincmd.Parameters.AddWithValue("@name", user.Username);
                    sqlupdateCoincmd.Parameters.AddWithValue("@coins", user.Coins - 5);
                    sqlupdateCoincmd.ExecuteNonQuery();
                }

                return true;
            }
            return false;
        }

        // Checks if a package is available
        public bool PackageAvailable()
        {
            string cmd = @"SELECT * FROM cardpackage
                            LIMIT 1;";

            lock (_lock)
            {
                using NpgsqlCommand sqlcmd = new(cmd, con);
                using var reader = sqlcmd.ExecuteReader();
                return reader.HasRows;
            }

        }

        // This method is used to return a list of Cards either from the stack or deck
        public List<Card> ListStackOrDeck(string username, string source = "stack")
        {
            List<Card> cards = new();
            string cmd = "SELECT stack.card, card.type, card.name, card.damage, card.ismonster " +
                         "FROM stack " +
                         "INNER JOIN card ON card.id=stack.card " +
                         "WHERE stack.player = @player";

            if (source.ToLower() == "stack")
                cmd += ";";
            else if (source.ToLower() == "deck")
                cmd += " and partofdeck = TRUE;";
            else
                return cards;

            lock (_lock)
            {
                using (NpgsqlCommand sqlcmd = new(cmd, con))
                {
                    sqlcmd.Parameters.AddWithValue("@player", username);

                    using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader.GetBoolean(4))
                        {
                            MonsterCard card = new MonsterCard(reader.GetGuid(0), reader.GetString(2), reader.GetDouble(3), Card.CheckElementEnum(reader.GetString(2)), (MonsterEnum)reader.GetInt32(1));
                            cards.Add(card);
                        }
                        else
                        {
                            SpellCard card = new SpellCard(reader.GetGuid(0), reader.GetString(2), reader.GetDouble(3), Card.CheckElementEnum(reader.GetString(2)));
                            cards.Add(card);
                        }
                    }
                }
            }
            Console.WriteLine();
            return cards;
        }

        // Adds the List of GUIDs provided to the users deck
        public bool AddCardsToDeck(List<Guid> cardGuids, string username)
        {
            StringBuilder sb = new(@"Update stack
                            SET partOfDeck = TRUE
                            WHERE player = @name and (");

            for (int i = 0; i < cardGuids.Count; i++)
            {
                sb.Append($"card = @id{i} or ");
            }
            string cmd = sb.Remove(sb.Length - 4, 4).Append(");").ToString();

            using NpgsqlCommand sqlcmd = new(cmd, con);
            for (int i = 0; i < cardGuids.Count; i++)
            {
                sqlcmd.Parameters.Add("@id" + i, NpgsqlTypes.NpgsqlDbType.Uuid);
                sqlcmd.Parameters["@id" + i].Value = cardGuids[i];
            }
            sqlcmd.Parameters.AddWithValue("@name", username);

            return sqlcmd.ExecuteNonQuery() > 0;
        }

        // Verifies if the provided token matches the token stored in the DB
        public bool CheckToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return false;

            string cmd = @"SELECT token FROM player
                            WHERE token = @token;";

            lock (_lock)
            {
                using NpgsqlCommand sqlcmd = new(cmd, con);
                sqlcmd.Parameters.AddWithValue("@token", token);

                using var reader = sqlcmd.ExecuteReader();
                return reader.HasRows;
            }
        }

        // After a battle to move the cards from one users deck to the other stack
        public void MoveCards(Dictionary<Card, User> movingCards)
        {
            int i = 0;                              //Iterator used in the foreach loop of the dictionary
            StringBuilder sb = new(@"UPDATE stack
                                   SET player = @user1,
                                   partofdeck = false
                                   WHERE ");
            StringBuilder sb2 = new(sb.ToString());
            sb2.Replace('1', '2');

            if (movingCards.Count == 0)
                return;
            string username1 = movingCards.First().Value.Username;
            string username2 = "";

            foreach (var item in movingCards)
            {
                if (item.Value.Username != username1)
                {
                    sb2.Append(@"card = @card" + i + " or ");
                    username2 = item.Value.Username;
                }
                else
                {
                    sb.Append(@"card = @card" + i + " or ");
                }
                i++;

            }
            string cmd1 = sb.Remove(sb.Length - 4, 4).Append(';').ToString();
            string cmd2 = sb2.Remove(sb2.Length - 4, 4).Append(';').ToString();

            using NpgsqlCommand sqlUser1cmd = new(cmd1, con);
            using NpgsqlCommand sqlUser2cmd = new(cmd2, con);
            sqlUser1cmd.Parameters.AddWithValue("@user1", username1);

            i = 0;
            foreach (KeyValuePair<Card, User> item in movingCards)
            {
                if (item.Value.Username != username1)
                {
                    sqlUser2cmd.Parameters.Add("@card" + i, NpgsqlTypes.NpgsqlDbType.Uuid);
                    sqlUser2cmd.Parameters["@card" + i].Value = item.Key.Id;
                }
                else
                {
                    sqlUser1cmd.Parameters.Add("@card" + i, NpgsqlTypes.NpgsqlDbType.Uuid);
                    sqlUser1cmd.Parameters["@card" + i].Value = item.Key.Id;
                }
                i++;
            }

            sqlUser1cmd.ExecuteNonQuery();
            if (username2 != String.Empty)
            {
                sqlUser2cmd.Parameters.AddWithValue("@user2", username2);
                sqlUser2cmd.ExecuteNonQuery();
            }
            return;
        }
    }
}
