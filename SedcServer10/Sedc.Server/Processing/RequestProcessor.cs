using Sedc.Server.Requests;
using Sedc.Server.Responses;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sedc.Server.Processing
{
    internal class RequestProcessor
    {
        public IResponse ProcessRequest(Request request)
        {
            if (request.Url.Path.RawValue == "/favicon.ico")
            {
                return new BinaryResponse
                {
                    Body = File.ReadAllBytes(@"C:\Source\SEDC\skwd10-wdel6\SedcServer10\Files\sedc.png"),
                    Headers = new Dictionary<string, string>
                    {
                        {"Content-Type", "image/png" }
                    }
                };
            }

            var result = new TextResponse
            {
                Headers = new Dictionary<string, string>
                {
                    {"Content-Type", "text/html" }
                },
                Body = $"""
    <h1>Hello from SEDC Server!</h1>
    <h2>The requested method was {request.Method.Name}</h2>
    <h2>The requested path was {request.Url}</h2>
    <h2>Headers: </h2>
    {GetHeadersHtml(request)}
    <h2>Body:</h2>
    <div>{request.Body}</div>
"""
            };
            return result;
        }

        private static string GetHeadersHtml(Request request)
        {
            var sb = new StringBuilder();
            sb.Append("<ul>");
            foreach (var (key, value) in request.Headers)
            {
                sb.Append($"<li>{key}: {value}</li>");
            }
            sb.Append("</ul>");

            return sb.ToString();

        }
    }
}
