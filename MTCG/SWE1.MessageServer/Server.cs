using MonsterTradingCardsGame.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
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

            switch(request.MethodeType)
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
                    break;
                case "PUT":
                    if (request.Route == "/deck")
                    {
                        PutDeck(request, tcpClient);
                    }
                    break;
            }
        }

        public void Start()
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            TcpListener tcpListener = new TcpListener(ip, 10001);
            tcpListener.Start();

            while(true)
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
            } else
            {
                Respond.SendResponse(tcpClient, HttpStatusCode.OK, "User successfully created");
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
                Respond.SendResponse(tcpClient, HttpStatusCode.Conflict, "Invalid username/password provided");
            }
            else
            {
                Respond.SendResponse(tcpClient, HttpStatusCode.OK, "User login successful" + user.Token);
            }
        }

        public void PostPackages(Request request, TcpClient tcpClient)
        {      
            if (request.Token == "admin-mtcgToken")
            {               
                /*
                // body 
                if (!database.CreatePackage(request.Body)) // Muss Liste werden
                {
                    Respond.SendResponse(tcpClient, HttpStatusCode.Conflict, "Something happend");
                    return;
                }
                else
                {
                    Respond.SendResponse(tcpClient, HttpStatusCode.OK, "Package and cards successfully created");
                }    
                */
            }
            else
            {
                Respond.SendResponse(tcpClient, HttpStatusCode.Conflict, "Provided user is not admin");
                return;
            }           
        }

        public void PostTransactionsPackages(Request request, TcpClient tcpClient)
        {

        }

        public void PostBattle(Request request, TcpClient tcpClient)
        {

        }

        public void GetCards(Request request, TcpClient tcpClient)
        {

        }

        public void GetDeck(Request request, TcpClient tcpClient)
        {

        }

        public void GetDeckFormat(Request request, TcpClient tcpClient)
        {

        }

        public void PutDeck(Request request, TcpClient tcpClient)
        {

        }

    }
}
