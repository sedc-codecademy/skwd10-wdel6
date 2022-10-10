using Sedc.Server;

Server s = new Server(new ServerOptions(Port: 668, DevMode: true));

s.Configure(new ServerConfig());

s.RegisterStaticSite("movie", @"C:\Source\SEDC\skwd10-wdel6\movies");

s.Start();