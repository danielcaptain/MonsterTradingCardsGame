using MonsterTradingCardsGame.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
            int scoreUser1 = 0;
            int scoreUser2 = 0;

            List<Card> user1Deck = new List<Card>(); // Fill with User1 Deck

            List<Card> user2Deck = new List<Card>(); // Fill with User2 Deck

            // Round 1
            if (Battle.FightCards(user1Deck.First(), user2Deck.First()) == 1)
            {
                log.LogRound("The First round was won by the User: " + user1.Username);
            }
            else
            {
                log.LogRound("The First round was won by the User: " + user2.Username);
            }
            user1Deck.Remove(user1Deck.First());
            user2Deck.Remove(user2Deck.First());

            // Round 2
            if (Battle.FightCards(user1Deck.First(), user2Deck.First()) == 1)
            {
                log.LogRound("The First round was won by the User: " + user1.Username);
            }
            else
            {
                log.LogRound("The First round was won by the User: " + user2.Username);
            }
            user1Deck.Remove(user1Deck.First());
            user2Deck.Remove(user2Deck.First());

            // Round 3
            if (Battle.FightCards(user1Deck.First(), user2Deck.First()) == 1)
            {
                log.LogRound("The First round was won by the User: " + user1.Username);
            }
            else
            {
                log.LogRound("The First round was won by the User: " + user2.Username);
            }
            user1Deck.Remove(user1Deck.First());
            user2Deck.Remove(user2Deck.First());

            // Round 4
            if (Battle.FightCards(user1Deck.First(), user2Deck.First()) == 1)
            {
                log.LogRound("The First round was won by the User: " + user1.Username);
            }
            else
            {
                log.LogRound("The First round was won by the User: " + user2.Username);
            }
            user1Deck.Remove(user1Deck.First());
            user2Deck.Remove(user2Deck.First());

            if (scoreUser1 > scoreUser2)
            {
                log.LogRound("The Winner of this glorious Battle is the User: " + user1.Username + " with the score: " + scoreUser1 + " to " + scoreUser2);

            }
            else if (scoreUser1 < scoreUser2)
            {
                log.LogRound("The Winner of this glorious Battle is the User: " + user2.Username + " with the score: " + scoreUser2 + " to " + scoreUser1);

            }
            else if (scoreUser2 == scoreUser1)
            {
                log.LogRound("There is no Winner of this Battle");

            }
            return log.ReturnLog();
        }

        private static int FightCards(Card cardUser1, Card cardUser2)
        {
            if (true) // cardUser1 wins
            {
                return 1;
            }
            else // cardUser2 wins
            {
                return 0;
            }
        }
    }
}
