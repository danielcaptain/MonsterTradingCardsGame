using MonsterTradingCardsGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    public class Battle
    {
        public static string ExecuteBattle(User user1, User user2)
        {
            BattleLog log = new BattleLog();



            return log.ReturnLog();
        }
    }
}
