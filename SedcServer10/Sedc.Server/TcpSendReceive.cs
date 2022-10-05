using Sedc.Server.Exceptions;
using Sedc.Server.Requests;
using Sedc.Server.Responses;

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
            var bytes = new byte[8192];
            var byteCount = stream.Read(bytes);
            var data = Encoding.UTF8.GetString(bytes, 0, byteCount);

            var request = parser.TryParse(data);
            return request;
        }

        public static void SendResponse(TcpClient client, IResponse response)
        {
            var stream = client.GetStream();
            var responseString = @$"HTTP/1.1 {response.Status.Id} {response.Status.Message}
{GenerateHeaders(response)}
";
            // send response
            var responseHeaderBytes = Encoding.UTF8.GetBytes(responseString);
            stream.Write(responseHeaderBytes);
            var bodyBytes = response.GetBodyBytes();
            stream.Write(bodyBytes);
            client.Close();
        }

        private static string GenerateHeaders(IResponse response)
        {
            var sb = new StringBuilder();
            foreach (var (key, value) in response.Headers)
            {
                sb.AppendLine($"{key}: {value}");
            }
            sb.AppendLine($"Content-Length: {response.ContentLength}");
            return sb.ToString();

            // return string.Join(Environment.NewLine, headers.Select((key, value) => $"{key}: {value}"));
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

        internal static void SendRequestErrorResponse(TcpClient client, bool devMode, InvalidRequest request)
        {
            var stream = client.GetStream();
            var responseText = devMode
                ? $"Bad request, {request.Message}"
                : "Bad request (general details)";
            var responseString = @$"HTTP/1.1 400 Bad Request

{responseText}";

            // send response
            var responseBytes = Encoding.UTF8.GetBytes(responseString);
            stream.Write(responseBytes);
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
