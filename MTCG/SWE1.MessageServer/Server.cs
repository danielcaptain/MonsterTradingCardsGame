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

                    }
                    if (request.Route == "/packages")
                    {

                    }
                    if (request.Route == "/transactions/packages")
                    {

                    }
                    if (request.Route == "/battles")
                    {

                    }
                    break;
                case "GET":
                    if (request.Route == "/cards")
                    {

                    }
                    if (request.Route == "/deck")
                    {

                    }
                    if (request.Route == "/deck?format=plain")
                    {

                    }
                    break;
                case "PUT":
                    if (request.Route == "/deck")
                    {

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
                Respond.SendResponse(tcpClient, HttpStatusCode.Conflict, "The Username: " + user.Username + " is already in use");
                return;
            }

            if (!database.CreateUser(user.Username, user.Password))
            {
                Respond.SendResponse(tcpClient, HttpStatusCode.Conflict, "Something happend");
            } else
            {
                Respond.SendResponse(tcpClient, HttpStatusCode.OK, "User was created");
            }
        }
    }
}
