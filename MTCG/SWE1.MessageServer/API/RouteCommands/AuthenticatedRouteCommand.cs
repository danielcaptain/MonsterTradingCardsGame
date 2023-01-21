using SWE1.MonsterTradingCardsGame.Core.Response;
using SWE1.MonsterTradingCardsGame.Core.Routing;
using SWE1.MonsterTradingCardsGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MonsterTradingCardsGame.API.RouteCommands
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
