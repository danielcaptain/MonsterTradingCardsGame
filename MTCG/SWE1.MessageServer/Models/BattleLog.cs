using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    public class BattleLog
    {
        public static string log;

        public BattleLog() 
        {
            log = "";
        }  

        public void LogRound(string round)
        {
            log += "\n" + round;
        }

        public string ReturnLog()
        {
            return log;
        }
    }
}
