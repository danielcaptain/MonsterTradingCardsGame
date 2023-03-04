using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    public abstract class Card
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }

        public enum ElementEnum
        {
            Fire,
            Water,
            Normal
        }

        public ElementEnum Element { get; private set; }
        public double Damage { get; private set; }

        public Card(Guid id, string name, double damage, ElementEnum element) 
        {
            Id = id;
            Name = name;
            Damage = damage;
            Element = element;
        }
    }
}
