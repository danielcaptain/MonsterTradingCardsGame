using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    public class Card
    {
        [JsonPropertyName("Id")]
        public Guid Id { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        public enum ElementEnum
        {
            Fire,
            Water,
            Normal
        }
        public ElementEnum Element { get; set; }

        [JsonPropertyName("Damage")]
        public double Damage { get; set; }

        public Card()
        {
            Id = Guid.Empty;
            Name = "";
            Damage = 0.0;
            Element = ElementEnum.Normal;
        }

        public Card(Guid id, string name, double damage, ElementEnum element)
        {
            Id = id;
            Name = name;
            Damage = damage;
            Element = element;
        }

        public static ElementEnum CheckElementEnum(string checkName)
        {
            if (checkName.ToLower().Contains("water"))
            {
                return ElementEnum.Water;
            }
            else if (checkName.ToLower().Contains("fire"))
            {
                return ElementEnum.Fire;
            }

            return ElementEnum.Normal;
        }
    }
}
