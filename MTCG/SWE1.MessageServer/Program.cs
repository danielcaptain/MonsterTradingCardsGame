// See https://aka.ms/new-console-template for more information
using MonsterTradingCardsGame.Models;
using SWE1.MonsterTradingCardsGame.API.RouteCommands;
using SWE1.MonsterTradingCardsGame.BLL;
using SWE1.MonsterTradingCardsGame.Core.Server;
using SWE1.MonsterTradingCardsGame.DAL;
using SWE1.MonsterTradingCardsGame.Models;
using System.Net;

var userDao = new InMemoryUserDao();
var userManager = new UserManager(userDao);

var messageDao = new InMemoryMessageDao();
var messageManager = new MessageManager(messageDao);

var router = new Router(userManager, messageManager);
var server = new HttpServer(IPAddress.Any, 10001, router);
server.Start();

//User user1 = new User("user1", "user1PW");
//User user2 = new User("user2", "user2PW");


//Battle.ExecuteBattle(user1, user2);
