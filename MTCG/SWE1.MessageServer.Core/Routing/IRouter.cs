using SWE1.MonsterTradingCardsGame.Core.Request;

namespace SWE1.MonsterTradingCardsGame.Core.Routing
{
    public interface IRouter
    {
        IRouteCommand? Resolve(RequestContext request);
    }
}