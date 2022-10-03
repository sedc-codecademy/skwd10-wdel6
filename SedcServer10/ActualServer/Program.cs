using Sedc.Server;

Server s = new Server(new ServerOptions(Port: 668, DevMode: true));

CustomRequestParser? parser = null;

s.Configure(new ServerConfig{ RequestParserFactory = () => parser ??= new CustomRequestParser()});

s.Start();