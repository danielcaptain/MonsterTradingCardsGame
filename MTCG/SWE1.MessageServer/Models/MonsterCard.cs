using SWE1.MonsterTradingCardsGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    class MonsterCard : Card
    {
        public enum MonsterEnum
        {
            Goblin,
            Dragon,
            Wizzard,
            Ork,
            Knight,
            Kraken,
            Elve,
            Troll
        }
        public MonsterEnum Monster { get; private set; }
        public MonsterCard(Guid id, int damage, ElementEnum element, MonsterEnum monster) : base(id, damage, element)
        {
            Monster = monster;
        }
    }
}