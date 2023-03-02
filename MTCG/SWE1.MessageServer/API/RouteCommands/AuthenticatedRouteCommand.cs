using MonsterTradingCardsGame.Core.Response;
using MonsterTradingCardsGame.Core.Routing;
using MonsterTradingCardsGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.API.RouteCommands
{
    internal abstract class AuthenticatedRouteCommand : IRouteCommand
    {
        public User Identity { get; private set; }

        public AuthenticatedRouteCommand(User identity)
        {
            Identity = identity;
        }

        public abstract Response Execute();
    }
}
