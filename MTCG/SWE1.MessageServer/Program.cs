using MonsterTradingCardsGame;
using MonsterTradingCardsGame.Models;
using System.Net;

DB db = new DB();
db.Connect();
Lobby lobby = new Lobby();
Server server = new Server(db);

server.Start();
