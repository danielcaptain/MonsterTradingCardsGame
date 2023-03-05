using MonsterTradingCardsGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

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

        public static MonsterEnum CheckMonsterEnum(string checkName)
        {
            if (checkName.ToLower().Contains("goblin"))
            {
                return MonsterEnum.Goblin;
            }
            else if (checkName.ToLower().Contains("dragon"))
            {
                return MonsterEnum.Dragon;
            }
            else if (checkName.ToLower().Contains("wizzard"))
            {
                return MonsterEnum.Wizzard;
            }
            else if (checkName.ToLower().Contains("ork"))
            {
                return MonsterEnum.Ork;
            }
            else if (checkName.ToLower().Contains("knight"))
            {
                return MonsterEnum.Knight;
            }
            else if (checkName.ToLower().Contains("kraken"))
            {
                return MonsterEnum.Kraken;
            }
            else if (checkName.ToLower().Contains("elve"))
            {
                return MonsterEnum.Elve;
            }
            else
            {
                return MonsterEnum.Troll;
            }
        }

        public override string ToString()
        {
            return "ID: " + this.Id.ToString() + " Name: " + this.Name + " Damage: " + this.Damage.ToString() + " Element: " + this.Element.ToString() + " Monster: " + Monster.ToString();
        }
    }
}