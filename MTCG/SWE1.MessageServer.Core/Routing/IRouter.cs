using MonsterTradingCardsGame.Core.Request;

namespace MonsterTradingCardsGame.Core.Routing
{
    public interface IRouter
    {
        IRouteCommand? Resolve(RequestContext request);
    }
}