using Sedc.Server;

Server s = new Server(new ServerOptions(Port: 668, DevMode: true));


s.Configure(new ServerConfig());

s.Start();