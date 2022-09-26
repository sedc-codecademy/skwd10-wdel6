using Sedc.Server;

Server s = new Server(new ServerOptions(Port: 668));

s.Configure();

s.Start();