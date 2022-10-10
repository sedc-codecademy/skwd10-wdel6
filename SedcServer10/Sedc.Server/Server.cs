using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;
using Sedc.Server.Requests;
using Sedc.Server.Responses;
using Sedc.Server.Processing;

namespace Sedc.Server
{
    public class Server
    {
        public int Port { get; private set; }
        public bool DevMode { get; private set; }

        public ParseRequest RequestParser { get; private set; }

        public Server(ServerOptions options) {
            Port = options.Port;
            DevMode = options.DevMode;
        }
        public void Configure(ServerConfig configuration)
        {
            RequestParser = configuration.RequestParser;
        }

        public void Start()
        {
            Console.WriteLine("Running server");
            var processor = new RequestProcessor();

            var address = IPAddress.Any;

            TcpListener listener = new TcpListener(address, Port);
            listener.Start();

            while (true)
            {
                // wait for a request
                var client = listener.AcceptTcpClient();
                try
                {
                    // process request
                    var request = TcpSendReceive.ProcessRequest(client, RequestParser);

                    Console.WriteLine(request);
                    if (request is InvalidRequest invalidRequest)
                    {
                        TcpSendReceive.SendRequestErrorResponse(client, DevMode, invalidRequest);
                        continue;
                    }

                    var response = processor.ProcessRequest((Request)request);

                    // send response
                    TcpSendReceive.SendResponse(client, response);
                } 
                catch (Exception ex)
                {
                    TcpSendReceive.SendErrorResponse(client, DevMode, ex);
                    Console.WriteLine($"Exception {ex.GetType().FullName} occured. Message: {ex.Message}");
                }
                finally
                {
                    client?.Close();
                }
            }
        }

    }
}