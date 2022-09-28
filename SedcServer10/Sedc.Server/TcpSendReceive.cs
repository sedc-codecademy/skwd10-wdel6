using Sedc.Server.Requests;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Sedc.Server
{
    internal static class TcpSendReceive
    {
        public static void ProcessRequest(TcpClient client)
        {
            var stream = client.GetStream();
            var bytes = new Span<byte>(new byte[8192]);
            var byteCount = stream.Read(bytes);
            Console.WriteLine($"Read out {byteCount} bytes");
            var data = Encoding.UTF8.GetString(bytes);
            Console.WriteLine(data);

            var parser = new RequestParser();
            var request = parser.TryParse(data);
            if (!request.IsValid()) {
                Console.WriteLine("Bad, bad request");
            } 
            else
            {
                Console.WriteLine("Nice request");
            }
        }

        public static void SendResponse(TcpClient client)
        {
            var stream = client.GetStream();
            var responseString = @$"HTTP/1.1 200 OK

Hello from SEDC Server";

            // send response
            var responseBytes = Encoding.UTF8.GetBytes(responseString);
            stream.Write(responseBytes);
            client.Close();
        }
    }
}
