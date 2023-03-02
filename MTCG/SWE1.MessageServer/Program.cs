// See https://aka.ms/new-console-template for more information
using MonsterTradingCardsGame;
using MonsterTradingCardsGame.API.RouteCommands;
using MonsterTradingCardsGame.BLL;
using MonsterTradingCardsGame.Core.Server;
using MonsterTradingCardsGame.DAL;
using MonsterTradingCardsGame.Models;
using System.Net;

DB db = new DB();
db.Connect();
db.CreateUser("test1", "tetstestets");

var userDao = new InMemoryUserDao();
var userManager = new UserManager(userDao);

var messageDao = new InMemoryMessageDao();
var messageManager = new MessageManager(messageDao);

var router = new Router(userManager, messageManager);
var server = new HttpServer(IPAddress.Any, 10001, router);
server.Start();
