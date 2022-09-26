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

    // process request
    using var stream = client.GetStream();
    Span<byte> bytes = new Span<byte>(new byte[8192]);
    var byteCount = stream.Read(bytes);
    Console.WriteLine($"Read out {byteCount} bytes");
    var data = Encoding.UTF8.GetString(bytes);
    Console.WriteLine(data);

    // get response
    var responseString = @$"HTTP/1.1 200 OK
Hello from SEDC Server";

    // send response
    var responseBytes = Encoding.UTF8.GetBytes(responseString);
    stream.Write(responseBytes);
    client.Close();
}

