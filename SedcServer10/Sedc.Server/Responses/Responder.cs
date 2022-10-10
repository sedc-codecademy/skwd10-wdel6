using Sedc.Server.Requests;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sedc.Server.Responses
{
    internal abstract class Responder
    {
        public Responder() { }

        public abstract bool IsApplicable(Request request);

        public abstract IResponse GenerateResponse(Request request);
    }

    internal class FaviconResponder : Responder
    {
        public override IResponse GenerateResponse(Request request)
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

        public override bool IsApplicable(Request request)
        {
            return request.Url.Path.RawValue == "/favicon.ico";
        }
    }

    internal class FileResponder: Responder
    {
        public override IResponse GenerateResponse(Request request)
        {
            var basePath = @"C:\Source\SEDC\skwd10-wdel6\site";
            var filePath = request.Url.Path.Paths.Length == 1
                ? "index.html"
                : request.Url.Path.Paths[1];
            var fullPath = Path.Combine(basePath, filePath);
            if (!File.Exists(fullPath))
            {
                return new NotFoundResponse();
            }
            var bytes = File.ReadAllBytes(fullPath);
            return new BinaryResponse
            {
                Body = bytes,
            };
        }

        public override bool IsApplicable(Request request)
        {
            return request.Url.Path.Paths.Length > 0 && request.Url.Path.Paths[0] == "site";
        }
    }

    internal class DefaultResponder: Responder
    {
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
        public override IResponse GenerateResponse(Request request)
        {
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

        public override bool IsApplicable(Request request)
        {
            return true;
        }
    }
}
