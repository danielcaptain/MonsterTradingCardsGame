using MonsterTradingCardsGame.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame
{
    internal class Request
    {
        public string Body = string.Empty;
        public string Route = string.Empty;
        public string Token = string.Empty;
        public string MethodeType = string.Empty;
        
        public void ParseRequest(string request)
        {
            Body = "";
            request = request.Replace("\r\n", "\n");

            string[] requestLine = request.Split ('\n');

            string[] requestPart = requestLine[0].Split(' ');
            MethodeType = requestPart[0];
            Route = requestPart[1];
            if (requestLine[5].Contains("Authorization"))
                Token = requestLine[5].Split(' ')[2];

            bool bodyFlag = false;
            for (int i = 0; i < requestLine.Count(); i++)
            {
                if (bodyFlag)
                    Body += requestLine[i];
                if (string.IsNullOrEmpty(requestLine[i]))            
                    bodyFlag = true;                          
            }
        }

        public object? ParseJson()
        {
            switch (Route)
            {
                case "/sessions":
                case "/users":
                    Console.WriteLine(Body);
                    User user = JsonSerializer.Deserialize<User>(Body);
                    return user;

                case "/packages":
                    List<Card> cards = JsonSerializer.Deserialize<List<Card>>(Body);
                    foreach (Card card in cards)
                    {
                        //card.SetType();
                    }
                    return cards;

                case "/deck":
                    List<Guid> cardguids = JsonSerializer.Deserialize<List<Guid>>(Body);
                    return cardguids;
         
                default:
                    break;
            }
            return null;
        }
    }
}
