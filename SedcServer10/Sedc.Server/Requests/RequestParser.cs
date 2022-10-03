using Sedc.Server.Exceptions;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sedc.Server.Requests
{
    internal class RequestParser: IRequestParser
    {
        private static readonly Regex RequestLineRegex = new Regex(@"([A-Z]+) (.*) HTTP\/(.*)$");

        public RequestParser() { }

        public IRequest TryParse(string requestData)
        {
            var requestLines = requestData.Split(Environment.NewLine);

            var requestLine = requestLines[0];
            var rmatch = RequestLineRegex.Match(requestLine);
            if (!rmatch.Success)
            {
                return new InvalidRequest("Invalid HTTP request received");
            }

            var method = rmatch.Groups[1].Value;
            var url = rmatch.Groups[2].Value;

            var headerLines = requestLines.Skip(1).TakeWhile(line => !string.IsNullOrEmpty(line));

            var headers = new Dictionary<string, string>();

            foreach (var header in headerLines)
            {
                // if the header body contains :<whitespace> it will be ignored
                var parts = header.Split(": ");
                if (parts.Length < 2) {
                    // todo: Investigate if this can be used as an attack vector
                    return new InvalidRequest($"Invalid HTTP request received: Invalid header line {header}");
                }
                var key = parts[0];
                var value = parts[1];
                if (headers.ContainsKey(key))
                {
                    headers[key] += $";{value}";
                } 
                else
                {
                    headers.Add(key, value);
                }
            }

            var body = requestLines.SkipWhile(line => !string.IsNullOrEmpty(line)).Skip(1);

            var result = new Request
            {
                Body = string.Join(Environment.NewLine, body),
                Headers = new ReadOnlyDictionary<string, string>(headers),
                Method = Method.GetMethod(method),
                RawRequest = requestData,
                Url = Url.Parse(url)
            };
            return result;
        }
    }
}
