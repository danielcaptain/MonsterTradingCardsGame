using MonsterTradingCardsGame.Core.Request;

namespace MonsterTradingCardsGame.Core.Client
{
    public interface IClient
    {
        public RequestContext? ReceiveRequest();
        public void SendResponse(Response.Response response);
        public void Close();
    }
}
