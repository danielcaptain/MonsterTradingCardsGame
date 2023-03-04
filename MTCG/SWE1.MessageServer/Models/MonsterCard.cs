using MonsterTradingCardsGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    public class MonsterCard : Card
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
        public MonsterEnum Monster { get; set; }
        public MonsterCard(Guid id, string name, double damage, ElementEnum element, MonsterEnum monster) : base(id, name, damage, element)
        {
            Monster = monster;
        }
    }
}