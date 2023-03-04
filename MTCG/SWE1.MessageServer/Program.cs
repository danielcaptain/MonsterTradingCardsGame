// See https://aka.ms/new-console-template for more information
using MonsterTradingCardsGame;
using MonsterTradingCardsGame.Models;
using System.Net;

DB db = new DB();
db.Connect();
Server server = new Server(db);

server.Start();
