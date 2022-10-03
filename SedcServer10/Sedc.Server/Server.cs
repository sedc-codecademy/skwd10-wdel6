using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;
using Sedc.Server.Requests;

namespace Sedc.Server
{
    public class Server
    {
        public int Port { get; private set; }
        public bool DevMode { get; private set; }

        public IRequestParserFactory RequestParserFactory { get; private set; }

        public Server(ServerOptions options) {
            Port = options.Port;
            DevMode = options.DevMode;
        }
        public void Configure(ServerConfig configuration)
        {
            RequestParserFactory = configuration.RequestParserFactory;
        }

        public void Start()
        {
            Console.WriteLine("Running server");

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
                    var request = TcpSendReceive.ProcessRequest(client, RequestParserFactory());

                    Console.WriteLine(request);

                    // send response
                    TcpSendReceive.SendResponse(client);
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