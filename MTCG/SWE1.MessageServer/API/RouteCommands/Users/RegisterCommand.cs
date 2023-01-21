﻿using SWE1.MonsterTradingCardsGame.BLL;
using SWE1.MonsterTradingCardsGame.Core.Response;
using SWE1.MonsterTradingCardsGame.Core.Routing;
using SWE1.MonsterTradingCardsGame.Models;

namespace SWE1.MonsterTradingCardsGame.API.RouteCommands.Users
{
    internal class RegisterCommand : IRouteCommand
    {
        private readonly Credentials _credentials;
        private readonly IUserManager _userManager;

        public RegisterCommand(IUserManager userManager, Credentials credentials)
        {
            _credentials = credentials;
            _userManager = userManager;
        }

        public Response Execute()
        {
            var response = new Response();
            try
            {
                _userManager.RegisterUser(_credentials);
                response.StatusCode = StatusCode.Created;
            }
            catch(DuplicateUserException)
            {
                response.StatusCode = StatusCode.Conflict;
            }
            return response;
        }
    }
}
