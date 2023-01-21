using SWE1.MonsterTradingCardsGame.Core.Request;
using HttpMethod = SWE1.MonsterTradingCardsGame.Core.Request.HttpMethod;

namespace SWE1.MonsterTradingCardsGame.API.RouteCommands
{
    internal interface IRouteParser
    {
        bool IsMatch(string resourcePath, string routePattern);
        Dictionary<string, string> ParseParameters(string resourcePath, string routePattern);
    }
}
