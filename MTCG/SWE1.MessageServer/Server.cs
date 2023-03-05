using MonsterTradingCardsGame.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame
{
    internal class Server
    {
        public DB database;

        public Server(DB db)
        {
            database = db;
        }

        public void ProcessClient(TcpClient tcpClient)
        {
            Request request = new();
            NetworkStream stream = tcpClient.GetStream();

            byte[] requestData = new byte[1024];
            int bytesRead = stream.Read(requestData, 0, requestData.Length);

            request.ParseRequest(Encoding.ASCII.GetString(requestData, 0, bytesRead));

            switch (request.MethodeType)
            {
                case "POST":
                    if (request.Route == "/users")
                    {
                        PostUsers(request, tcpClient);
                    }
                    if (request.Route == "/sessions")
                    {
                        PostSessions(request, tcpClient);
                    }
                    if (request.Route == "/packages")
                    {
                        PostPackages(request, tcpClient);
                    }
                    if (request.Route == "/transactions/packages")
                    {
                        PostTransactionsPackages(request, tcpClient);
                    }
                    if (request.Route == "/battles")
                    {
                        PostBattle(request, tcpClient);
                    }
                    break;
                case "GET":
                    if (request.Route == "/cards")
                    {
                        GetCards(request, tcpClient);
                    }
                    if (request.Route == "/deck")
                    {
                        GetDeck(request, tcpClient);
                    }
                    if (request.Route == "/deck?format=plain")
                    {
                        GetDeckFormat(request, tcpClient);
                    }
                    if (request.Route.StartsWith("/users/"))
                    {
                        GetUsers(request, tcpClient);
                    }
                    break;
                case "PUT":
                    if (request.Route == "/deck")
                    {
                        PutDeck(request, tcpClient);
                    }
                    if (request.Route.StartsWith("/users/"))
                    {
                        PutUsers(request, tcpClient);
                    }
                    break;
            }
        }

        public void Start()
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            TcpListener tcpListener = new TcpListener(ip, 10001);
            tcpListener.Start();

            while (true)
            {
                TcpClient tcpClient = tcpListener.AcceptTcpClient();
                Thread thread = new Thread(() => ProcessClient(tcpClient));
                thread.Start();
            }
        }

        public void PostUsers(Request request, TcpClient tcpClient)
        {
            object objt = request.ParseJson();
            if (objt is not User)
            {
                Respond.SendResponse(tcpClient, HttpStatusCode.InternalServerError, "Something went wrong while parsing the Request");
                return;
            }

            User user = (User)objt;

            if (database.UserExists(user.Username))
            {
                Respond.SendResponse(tcpClient, HttpStatusCode.Conflict, "User with same username already registered");
                return;
            }

            if (!database.CreateUser(user.Username, user.Password))
            {
                Respond.SendResponse(tcpClient, HttpStatusCode.Conflict, "Something happend");
                return;
            }
            else
            {
                Respond.SendResponse(tcpClient, HttpStatusCode.Created, "User successfully created");
            }
        }

        public void PostSessions(Request request, TcpClient tcpClient)
        {
            object objt = request.ParseJson();
            if (objt is not User)
            {
                Respond.SendResponse(tcpClient, HttpStatusCode.InternalServerError, "Something went wrong while parsing the Request");
                return;
            }

            User user = (User)objt;

            if (!database.VerifyLogin(user.Username, user.Password))
            {
                Respond.SendResponse(tcpClient, HttpStatusCode.Unauthorized, "Invalid username/password provided");
            }
            else
            {
                Respond.SendResponse(tcpClient, HttpStatusCode.OK, "User login successful" + user.Token);
            }
        }

        public void PostPackages(Request request, TcpClient tcpClient)
        {

            object objt = request.ParseJson();
            List<Card> listOfCards = (List<Card>)objt;

            if (!User.CheckIfTokenIsMissingOrInvalid(request.Token))
            {
                Respond.SendResponse(tcpClient, HttpStatusCode.Unauthorized, "Access token is missing or invalid");
                return;
            }


            if (!database.CheckToken(request.Token))
            {
                Respond.SendResponse(tcpClient, HttpStatusCode.Unauthorized, "Access token is missing or invalid");
                return;
            }


            if (request.Token == "admin-mtcgToken")
            {
                List<Guid> alreadyInUsedGuids = new List<Guid>();
                alreadyInUsedGuids = database.CardsExistAlready(listOfCards);
                if (alreadyInUsedGuids != null)
                {
                    Respond.SendResponse(tcpClient, HttpStatusCode.Conflict, "At least one card in the packages already exists");
                }

                if (!database.CreatePackage(listOfCards))
                {
                    Respond.SendResponse(tcpClient, HttpStatusCode.Conflict, "Something happend");
                    return;
                }
                else
                {
                    Respond.SendResponse(tcpClient, HttpStatusCode.Created, "Package and cards successfully created");
                }


            }
            else
            {
                Respond.SendResponse(tcpClient, HttpStatusCode.Forbidden, "Provided user is not admin");
                return;
            }
        }

        public void PostTransactionsPackages(Request request, TcpClient tcpClient)
        {
            User user = user = database.GetUserInformation(request.Token);

            if (user.Coins < 5)
            {
                Respond.SendResponse(tcpClient, HttpStatusCode.Forbidden, "Not enough money for buying a card package");
                return;
            }

            if (!User.CheckIfTokenIsMissingOrInvalid(request.Token))
            {
                Respond.SendResponse(tcpClient, HttpStatusCode.Unauthorized, "Access token is missing or invalid");
                return;
            }

            if (!database.CheckToken(request.Token))
            {
                Respond.SendResponse(tcpClient, HttpStatusCode.Unauthorized, "Access token is missing or invalid");
                return;
            }

            if (database.GetPackage(user) == false)
            {
                Respond.SendResponse(tcpClient, HttpStatusCode.NotFound, "No card package available for buying");
                return;
            }
            else
            {
                Respond.SendResponse(tcpClient, HttpStatusCode.OK, "A package has been successfully bought");
                return;
            }
        }

        public void PostBattle(Request request, TcpClient tcpClient)
        {
            if (!User.CheckIfTokenIsMissingOrInvalid(request.Token))
            {
                Respond.SendResponse(tcpClient, HttpStatusCode.Unauthorized, "Access token is missing or invalid");
                return;
            }

            if (!database.CheckToken(request.Token))
            {
                Respond.SendResponse(tcpClient, HttpStatusCode.Unauthorized, "Access token is missing or invalid");
                return;
            }

            User user = user = database.GetUserInformation(request.Token);

            Lobby.AddUserToLobby(user);
            if (Lobby.CheckCountOfLobby() < 2)
            {
                Console.WriteLine("Waiting for another Player to join the Battle Lobby");
                while (Lobby.CheckCountOfLobby() < 2)
                {

                }
                Console.WriteLine("Another Player joined the Battle Lobby - Battle starts!");
            }
            else
            {
                User firstUserToBattle = Lobby.GetFirstUserInLobby();
                Lobby.RemoveUserFromLobby(firstUserToBattle);
                string battleLog = Battle.ExecuteBattle(user, firstUserToBattle);
                Respond.SendResponse(tcpClient, HttpStatusCode.OK, "The battle has been carried out successfully.");
                return;
            }
        }

        public void GetCards(Request request, TcpClient tcpClient)
        {
            if (!User.CheckIfTokenIsMissingOrInvalid(request.Token))
            {
                Respond.SendResponse(tcpClient, HttpStatusCode.Unauthorized, "Access token is missing or invalid");
                return;
            }
            if (!database.CheckToken(request.Token))
            {
                Respond.SendResponse(tcpClient, HttpStatusCode.Unauthorized, "Access token is missing or invalid");
                return;
            }

            User user = user = database.GetUserInformation(request.Token);

            List<Card> allUserCards = new List<Card>();
            allUserCards = database.ListStackOrDeck(user.Username, "stack");

            if (allUserCards.Count == 0)
            {
                Respond.SendResponse(tcpClient, HttpStatusCode.NoContent, "The request was fine, but the user doesn't have any cards");
            }
            else
            {
                Respond.SendResponse(tcpClient, HttpStatusCode.OK, "The user has cards, the response contains these" + "\n" + JsonSerializer.Serialize(allUserCards));
            }
        }

        public void GetDeck(Request request, TcpClient tcpClient)
        {
            if (!User.CheckIfTokenIsMissingOrInvalid(request.Token))
            {
                Respond.SendResponse(tcpClient, HttpStatusCode.Unauthorized, "Access token is missing or invalid");
                return;
            }
            if (!database.CheckToken(request.Token))
            {
                Respond.SendResponse(tcpClient, HttpStatusCode.Unauthorized, "Access token is missing or invalid");
                return;
            }

            User user = user = database.GetUserInformation(request.Token);

            List<Card> allUserCards = new List<Card>();
            allUserCards = database.ListStackOrDeck(user.Username, "deck");

            if (allUserCards.Count == 0)
            {
                Respond.SendResponse(tcpClient, HttpStatusCode.NoContent, "The request was fine, but the deck doesn't have any cards");
            }
            else
            {
                Respond.SendResponse(tcpClient, HttpStatusCode.OK, "The deck has cards, the response contains these" + "\n" + JsonSerializer.Serialize(allUserCards));
            }

        }

        public void GetDeckFormat(Request request, TcpClient tcpClient)
        {
            if (!User.CheckIfTokenIsMissingOrInvalid(request.Token))
            {
                Respond.SendResponse(tcpClient, HttpStatusCode.Unauthorized, "Access token is missing or invalid");
                return;
            }
            if (!database.CheckToken(request.Token))
            {
                Respond.SendResponse(tcpClient, HttpStatusCode.Unauthorized, "Access token is missing or invalid");
                return;
            }

            User user = user = database.GetUserInformation(request.Token);

            List<Card> allUserCards = new List<Card>();
            allUserCards = database.ListStackOrDeck(user.Username, "deck");

            StringBuilder sb = new StringBuilder();
            foreach (Card card in allUserCards)
            {
                sb.Append(card.ToString() + "\n");
            }

            if (allUserCards.Count == 0)
            {
                Respond.SendResponse(tcpClient, HttpStatusCode.NoContent, "The request was fine, but the deck doesn't have any cards");
            }
            else
            {
                Respond.SendResponse(tcpClient, HttpStatusCode.OK, "The deck has cards, the response contains these" + "\n" + sb.ToString());
            }
        }

        public void GetUsers(Request request, TcpClient tcpClient)
        {
            User user = user = database.GetUserInformation(request.Token);

            if (!User.CheckIfTokenIsMissingOrInvalid(request.Token))
            {
                Respond.SendResponse(tcpClient, HttpStatusCode.Unauthorized, "Access token is missing or invalid");
                return;
            }

            if (!database.CheckToken(request.Token))
            {
                Respond.SendResponse(tcpClient, HttpStatusCode.Unauthorized, "Access token is missing or invalid");
                return;
            }

            string usernameRoute = request.Route.Replace("/users/", "");
            string usernameRouteToken = usernameRoute + "-mtcgToken";

            if (request.Token == "admin-mtcgToken" || request.Token == usernameRouteToken)
            {
                if (database.UserExists(user.Username) != true)
                {
                    Respond.SendResponse(tcpClient, HttpStatusCode.NotFound, "User not found.");
                    return;
                }

                string jsonString = $"{{\n  \"Name\": \"{user.Aliasname}\",\n  \"Bio\": \"{user.Bio}\",\n  \"Image\": \"{user.Image}\"\n}}";

                Respond.SendResponse(tcpClient, HttpStatusCode.OK, "Data successfully retrieved" + "\n" + jsonString);
            }
            else
            {
                Respond.SendResponse(tcpClient, HttpStatusCode.Forbidden, "Provided user is not admin or matching user");
                return;
            }
        }

        public void PutDeck(Request request, TcpClient tcpClient)
        {
            List<Guid> deckIds = new List<Guid>();
            User user = user = database.GetUserInformation(request.Token);

            if (!User.CheckIfTokenIsMissingOrInvalid(request.Token))
            {
                Respond.SendResponse(tcpClient, HttpStatusCode.Unauthorized, "Access token is missing or invalid");
                return;
            }
            if (!database.CheckToken(request.Token))
            {
                Respond.SendResponse(tcpClient, HttpStatusCode.Unauthorized, "Access token is missing or invalid");
                return;
            }

            string rebuildBody = request.Body.Trim('[').Trim(']');
            string[] guids = rebuildBody.Split(", ");
            foreach (string guid in guids)
            {
                deckIds.Add(Guid.Parse(guid.Trim('"')));
            }
            if (deckIds.Count != 4)
            {
                Respond.SendResponse(tcpClient, HttpStatusCode.BadRequest, "The provided deck did not include the required amount of cards");
                return;
            }

            // Check At least one of the provided cards does not belong to the user or is not available.
            database.AddCardsToDeck(deckIds, user.Username);
            Respond.SendResponse(tcpClient, HttpStatusCode.OK, "The deck has been successfully configured");
            return;
        }

        public void PutUsers(Request request, TcpClient tcpClient)
        {
            User user = user = database.GetUserInformation(request.Token);

            if (!User.CheckIfTokenIsMissingOrInvalid(request.Token))
            {
                Respond.SendResponse(tcpClient, HttpStatusCode.Unauthorized, "Access token is missing or invalid");
                return;
            }

            if (!database.CheckToken(request.Token))
            {
                Respond.SendResponse(tcpClient, HttpStatusCode.Unauthorized, "Access token is missing or invalid");
                return;
            }

            string usernameRoute = request.Route.Replace("/users/", "");
            string usernameRouteToken = usernameRoute + "-mtcgToken";

            if (request.Token == "admin-mtcgToken" || request.Token == usernameRouteToken)
            {
                if (database.UserExists(user.Username) != true)
                {
                    Respond.SendResponse(tcpClient, HttpStatusCode.NotFound, "User not found.");
                    return;
                }

                var userJsonDeserialized = JsonSerializer.Deserialize<User>(request.Body); // Vielleicht ohne userHelp

                User userHelp = new User(usernameRoute, "", 0, userJsonDeserialized.Aliasname, userJsonDeserialized.Bio, userJsonDeserialized.Image);

                database.EditUser(userHelp, userHelp.Username);

                Respond.SendResponse(tcpClient, HttpStatusCode.OK, "User sucessfully updated.");
            }
            else
            {
                Respond.SendResponse(tcpClient, HttpStatusCode.Forbidden, "Provided user is not admin or matching user");
                return;
            }
        }
    }
}
