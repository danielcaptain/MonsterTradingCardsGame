using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MonsterTradingCardsGame.Models
{
    abstract class Card
    {
        public Guid Id { get; private set; }

        public enum ElementEnum
        {
            Fire,
            Water,
            Normal
        }

        public ElementEnum Element { get; private set; }
        public int Damage { get; private set; }

        public Card(Guid id, int damage, ElementEnum element) 
        {
            Id = id;
            Damage = damage;
            Element = element;
        }
    }
}
