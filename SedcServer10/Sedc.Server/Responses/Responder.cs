using Sedc.Server.Exceptions;
using Sedc.Server.Requests;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Sedc.Server.Responses
{
    public delegate IResponse ResponseGetter(Request request);

    public delegate bool RequestPredicate(Request request);

    public class Responder
    {
        public RequestPredicate IsApplicable { get; init; } = _ => false;

        public ResponseGetter GenerateResponse { get; init; } = _ => new ErrorTextResponse(ResponseStatus.InternalServerError);
    }

    public static class ResponderRepository
    {
        public static Responder FaviconResponder = new Responder
        {
            IsApplicable = (Request request) => request.Url.Path.RawValue == "/favicon.ico",
            GenerateResponse = (Request request) => new BinaryResponse
            {
                Body = File.ReadAllBytes(@"C:\Source\SEDC\skwd10-wdel6\SedcServer10\Files\sedc.png"),
                Headers = new Dictionary<string, string>
                    {
                        {"Content-Type", "image/png" }
                    }
            }
        };

        private static readonly Func<Request, string> GetHeadersHtml = (Request request) =>
        {
            var sb = new StringBuilder();
            sb.Append("<ul>");
            foreach (var (key, value) in request.Headers)
            {
                sb.Append($"<li>{key}: {value}</li>");
            }
            sb.Append("</ul>");

            return sb.ToString();
        };


        public static Responder DefaultResponder = new Responder
        {
            IsApplicable = _ => true,
            GenerateResponse = (Request request) =>
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
        };

        public static Responder GetFileResponder(string route, string basePath, string defaultDocument = "index.html")
        {
            if (string.IsNullOrEmpty(route))
            {
                throw new ServerException($"Parameter {nameof(route)} cannot be empty");
            }
            if (string.IsNullOrEmpty(basePath))
            {
                throw new ServerException($"Parameter {nameof(basePath)} cannot be empty");
            }
            if (!Directory.Exists(basePath))
            {
                throw new ServerException($"Directory {nameof(basePath)} cannot be accessed");
            }

            return new Responder
            {
                IsApplicable = (Request request) => request.Url.Path.Paths.Length > 0 && request.Url.Path.Paths[0] == route,
                GenerateResponse = (Request request) =>
                {
                    var filePath = request.Url.Path.Paths.Length == 1
                        ? defaultDocument
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
            };
        }

    }
}
