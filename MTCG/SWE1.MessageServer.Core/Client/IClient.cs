using SWE1.MonsterTradingCardsGame.Core.Request;

namespace SWE1.MonsterTradingCardsGame.Core.Client
{
    public interface IClient
    {
        public RequestContext? ReceiveRequest();
        public void SendResponse(Response.Response response);
        public void Close();
    }
}
