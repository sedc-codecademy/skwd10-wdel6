using Sedc.Server;

Server s = new Server(new ServerOptions(Port: 668, DevMode: false));

s.Configure(new ServerConfig());

s.RegisterApi("calculate", new Calculator());

s.RegisterStaticSite("movie", @"C:\Source\SEDC\skwd10-wdel6\movies");
s.RegisterStaticSite("calc", @"C:\Source\SEDC\skwd10-wdel6\SedcServer10\Files\calculator");

s.Start();