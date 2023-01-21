﻿using SWE1.MonsterTradingCardsGame.BLL;
using SWE1.MonsterTradingCardsGame.Core.Response;
using SWE1.MonsterTradingCardsGame.Models;

namespace SWE1.MonsterTradingCardsGame.API.RouteCommands.Messages
{
    internal class RemoveMessageCommand : AuthenticatedRouteCommand
    {
        private readonly IMessageManager _messageManager;
        private readonly int _messageId;

        public RemoveMessageCommand(IMessageManager messageManager, User identity, int messageId) : base(identity)
        {
            _messageId = messageId;
            _messageManager = messageManager;
        }

        public override Response Execute()
        {
            var response = new Response();
            try
            {
                _messageManager.RemoveMessage(Identity, _messageId);
                response.StatusCode = StatusCode.Ok;
            }
            catch (MessageNotFoundException)
            {
                response.StatusCode = StatusCode.NotFound;
            }

            return response;
        }
    }
}
