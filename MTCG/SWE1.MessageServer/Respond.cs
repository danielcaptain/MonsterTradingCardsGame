using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame
{
    internal class Respond
    {
        public static void SendResponse(TcpClient client, HttpStatusCode statusCode, string Body)
        {
            string data = "HTTP/1.1 " + (int)statusCode + " " + statusCode + $"\nContent-Length: {Body.Length}" + $"\n\n{Body}";

            byte[] dbuf = Encoding.ASCII.GetBytes(data);
            client.GetStream().Write(dbuf, 0, dbuf.Length);                   

            client.GetStream().Close();
            client.Dispose();
        }

        public static void SendJSONResponse(TcpClient client, HttpStatusCode statusCode, string Body)
        {
            string data = "HTTP/1.1 " + (int)statusCode + " " + statusCode + "\nContent-Type: application/json" + $"\nContent-Length: {Body.Length}" + $"\n\n{Body}";

            byte[] dbuf = Encoding.ASCII.GetBytes(data);
            client.GetStream().Write(dbuf, 0, dbuf.Length);

            client.GetStream().Close();
            client.Dispose();
        }
    }
}
