using SWE1.MonsterTradingCardsGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    class SpellCard : Card
    {
        public SpellCard(Guid id, int damage, ElementEnum element) : base(id, damage, element)
        {
        }
    }
}
