using MonsterTradingCardsGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.DAL
{
    public interface IUserDao
    {
        User? GetUserByAuthToken(string authToken);
        User? GetUserByCredentials(string username, string password);
        bool InsertUser(User user);
    }
}
