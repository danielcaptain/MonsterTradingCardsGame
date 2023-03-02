using Newtonsoft.Json;
using MonsterTradingCardsGame.API.RouteCommands.Messages;
using MonsterTradingCardsGame.API.RouteCommands.Users;
using MonsterTradingCardsGame.BLL;
using MonsterTradingCardsGame.Core.Request;
using MonsterTradingCardsGame.Core.Routing;
using MonsterTradingCardsGame.Models;
using HttpMethod = MonsterTradingCardsGame.Core.Request.HttpMethod;

namespace MonsterTradingCardsGame.API.RouteCommands
{
    internal class Router : IRouter
    {
        private readonly IUserManager _userManager;
        private readonly IMessageManager _messageManager;
        private readonly IdentityProvider _identityProvider;
        private readonly IRouteParser _routeParser = new IdRouteParser();

        public Router(IUserManager userManager, IMessageManager messageManager)
        {
            _userManager = userManager;
            _messageManager = messageManager;

            // better: define IIdentityProvider interface and get concrete implementation passed in as dependency
            _identityProvider = new IdentityProvider(userManager);
        }

        public IRouteCommand? Resolve(RequestContext request)
        {
            var identity = (RequestContext request) => _identityProvider.GetIdentityForRequest(request) ?? throw new RouteNotAuthenticatedException();
            var isMatch = (string path) => _routeParser.IsMatch(path, "/messages/{id}");
            var parseId = (string path) => int.Parse(_routeParser.ParseParameters(path, "/messages/{id}")["id"]);

            IRouteCommand? command = request switch
            {
                { Method: HttpMethod.Post, ResourcePath: "/users" } => new RegisterCommand(_userManager, Deserialize<Credentials>(request.Payload)),
                { Method: HttpMethod.Post, ResourcePath: "/sessions"} => new LoginCommand(_userManager, Deserialize<Credentials>(request.Payload)),

                { Method: HttpMethod.Post, ResourcePath: "/messages"} => new AddMessageCommand(_messageManager, identity(request), EnsureBody(request.Payload)),
                { Method: HttpMethod.Get, ResourcePath: "/messages" } => new ListMessagesCommand(_messageManager, identity(request)),

                { Method: HttpMethod.Get, ResourcePath: var path} when isMatch(path) => new ShowMessageCommand(_messageManager, identity(request), parseId(path)),
                { Method: HttpMethod.Put, ResourcePath: var path } when isMatch(path) => new UpdateMessageCommand(_messageManager, identity(request), parseId(path), EnsureBody(request.Payload)),
                { Method: HttpMethod.Delete, ResourcePath: var path } when isMatch(path) => new RemoveMessageCommand(_messageManager, identity(request), parseId(path)),

                _ => null
            };

            return command;
        }

        private string EnsureBody(string? body)
        {
            if (body == null)
            {
                throw new InvalidDataException();
            }
            return body;
        }

        private T Deserialize<T>(string? body) where T : class
        {
            var data = body != null ? JsonConvert.DeserializeObject<T>(body) : null;
            if (data == null)
            {
                throw new InvalidDataException();
            }
            return data;
        }
    }
}
