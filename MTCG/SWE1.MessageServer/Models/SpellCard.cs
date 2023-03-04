using MonsterTradingCardsGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    public class SpellCard : Card
    {
        public SpellCard(Guid id, string name, double damage, ElementEnum element) : base(id, name, damage, element)
        {
        }
    }
}
