using SWE1.MonsterTradingCardsGame.BLL;
using SWE1.MonsterTradingCardsGame.Core.Response;
using SWE1.MonsterTradingCardsGame.Models;
using System.Text;

namespace SWE1.MonsterTradingCardsGame.API.RouteCommands.Messages
{
    internal class ListMessagesCommand : AuthenticatedRouteCommand
    {
        private readonly IMessageManager _messageManager;

        public ListMessagesCommand(IMessageManager messageManager, User identity) : base(identity)
        {
            _messageManager = messageManager;
        }

        public override Response Execute()
        {
            var messages = _messageManager.ListMessages(Identity).ToList();

            var response = new Response();

            if (messages.Any())
            {
                var payload = new StringBuilder();
                foreach (var message in messages)
                {
                    payload.Append(message.Id);
                    payload.Append(": ");
                    payload.Append(message.Content);
                    payload.Append('\n');
                }
                response.StatusCode = StatusCode.Ok;
                response.Payload = payload.ToString();
            }
            else
            {
                response.StatusCode = StatusCode.NoContent;
            }

            return response;
        }
    }
}
