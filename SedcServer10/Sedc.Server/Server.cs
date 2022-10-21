using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;
using Sedc.Server.Requests;
using Sedc.Server.Responses;
using Sedc.Server.Processing;
using Sedc.Server.Logging;

namespace Sedc.Server
{
    public class Server
    {
        public int Port { get; private set; }
        public bool DevMode { get; private set; }

        private readonly ILogger Logger = new ConsoleLogger { LogLevel = LogLevel.Info };

        private readonly RequestProcessor processor;
        

        public ParseRequest RequestParser { get; private set; }

        public Server(ServerOptions options) {
            Port = options.Port;
            DevMode = options.DevMode;

            if (DevMode)
            {
                Logger = new ConsoleLogger { LogLevel = LogLevel.Debug };
            }

            processor = new RequestProcessor(Logger);
        }
        public void Configure(ServerConfig configuration)
        {
            RequestParser = configuration.RequestParser;

        }

        public void Start()
        {
            Logger.Info("Running server");
            

            var address = IPAddress.Any;

            TcpListener listener = new TcpListener(address, Port);
            listener.Start();
            Logger.Debug($"Started listening on port {Port}");

            while (true)
            {
                // wait for a request
                Logger.Debug("Waiting for a client");
                var client = listener.AcceptTcpClient();
                try
                {
                    // process request
                    Logger.Debug("Started processing request");
                    var request = TcpSendReceive.ProcessRequest(client, RequestParser);
                    Logger.Debug($"Processed request {request}");
                    if (request is InvalidRequest invalidRequest)
                    {
                        Logger.Error($"Invalid request detected {invalidRequest}");
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
                    Logger.Error($"Exception {ex.GetType().FullName} occured. Message: {ex.Message}");
                }
                finally
                {
                    client?.Close();
                }
            }
        }

        public void RegisterStaticSite(string route, string path)
        {
            processor.AddFileResponder(route, path);
        }

        public void RegisterApi(string route, object apiProcessor)
        {
            processor.AddApiResponder(route, apiProcessor);
        }

        public void RegisterApi<T>(string route)
        {
            processor.AddApiResponder<T>(route);
        }
    }
}