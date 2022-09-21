using System.Net;
using System.Net.Sockets;
using System.Text;

var address = IPAddress.Any;
var port = 668; //the neighbour of the beast

TcpListener listener = new TcpListener(address, port);
listener.Start();

Console.WriteLine("Starting server loop");
while (true)
{
    // wait for a request
    Console.WriteLine($"Waiting for tcp client");
    var client = listener.AcceptTcpClient();
    Console.WriteLine($"Accepted tcp client");
    // process request and get response
    using var stream = client.GetStream();
    byte[] buffer = new byte[8192];
    Span<byte> bytes = new Span<byte>(buffer);
    var byteCount = stream.Read(bytes);
    // send out response
    Console.WriteLine($"Read out {byteCount} bytes");
    var data = Encoding.UTF8.GetString(bytes);
    Console.WriteLine(data);
    var responseString = @$"HTTP/1.1 200 OK

Hello from SEDC Server";
    var responseBytes = Encoding.UTF8.GetBytes(responseString);
    stream.Write(responseBytes);
    client.Close();
}

