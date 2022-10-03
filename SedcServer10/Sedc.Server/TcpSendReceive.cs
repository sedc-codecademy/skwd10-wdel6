using Sedc.Server.Exceptions;
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
        public static IRequest ProcessRequest(TcpClient client, IRequestParser parser)
        {
            var stream = client.GetStream();
            var bytes = new Span<byte>(new byte[8192]);
            stream.Read(bytes);
            var data = Encoding.UTF8.GetString(bytes);

            var request = parser.TryParse(data);
            return request;
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

        internal static void SendErrorResponse(TcpClient? client, bool devMode, Exception ex)
        {
            if (client == null || !client.Connected)
            {
                return;
            }
            NetworkStream stream;
            try
            {
               stream = client.GetStream();
            } 
            catch (ObjectDisposedException)
            {
                return;
            }
            var responseString = @$"HTTP/1.1 500 Internal Server Error

{GetExceptionMessage(devMode, ex)}";

            // send response
            var responseBytes = Encoding.UTF8.GetBytes(responseString);
            try
            {
                stream.Write(responseBytes);
            } catch (InvalidOperationException) {
                return;
            }
            client.Close();
        }

        private static string GetExceptionMessage(bool devMode, Exception ex)
        {
            if (!devMode)
            {
                return "Error occured (general details)";
            }
            if (ex == null)
            {
                return "Error occured (no specific details)";
            }
            var sb = new StringBuilder();
            sb.AppendLine("Error occured");
            sb.AppendLine(ex.Message);
            sb.AppendLine();
            sb.AppendLine(ex.StackTrace);
            return sb.ToString();
        }
    }
}
