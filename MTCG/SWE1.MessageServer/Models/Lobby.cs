using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    public class Lobby
    {
        private static List<User> battleLobby = new List<User>();
        private static readonly object _lock = new();
        public Lobby() 
        {
            battleLobby.Clear();
        }

        public static void AddUserToLobby(User user)
        {          
            lock (_lock)
            {
                battleLobby.Add(user);
            }
        }

        public static void RemoveUserFromLobby(User user) 
        {
            lock (_lock)
            {
                battleLobby.Remove(user);
            }
        }

        public static int CheckCountOfLobby()
        {
            return battleLobby.Count();
        }

        public static User GetFirstUserInLobby()
        {
            lock (_lock)
            {
                return battleLobby[0];             
            }
        }

        public static void ClearLobby()
        {
            battleLobby.Clear();
        }
    }
}
