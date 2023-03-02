using MonsterTradingCardsGame.Core.Request;
using HttpMethod = MonsterTradingCardsGame.Core.Request.HttpMethod;

namespace MonsterTradingCardsGame.API.RouteCommands
{
    internal interface IRouteParser
    {
        bool IsMatch(string resourcePath, string routePattern);
        Dictionary<string, string> ParseParameters(string resourcePath, string routePattern);
    }
}
