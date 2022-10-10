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
    internal static partial class RequestHelper
    {
        private static readonly Regex RequestLineRegex = new Regex(@"([A-Z]+) (.*) HTTP\/(.*)$");

        private static (Dictionary<string, string> headers, bool status) ParseHeaders(IEnumerable<string> headerLines)
        {
            var headers = new Dictionary<string, string>();

            foreach (var header in headerLines)
            {
                // if the header body contains :<whitespace> it will be ignored
                var parts = header.Split(": ");
                if (parts.Length < 2)
                {
                    // todo: Investigate if this can be used as an attack vector
                    var result = new Dictionary<string, string>
                    {
                        { "Header", header }
                    };
                    return (result, false);
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

            return (headers, true);
        }

        public static IRequest TryParse(string requestData)
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
            var (headers, headerStatus) = ParseHeaders(headerLines);
            if (!headerStatus)
            {
                var badHeader = headers["Header"];
                return new InvalidRequest($"Invalid HTTP request received: Invalid header line {badHeader}");
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


        //public object DoStuff(object param)
        //{
        //    // guard-clause-1
        //    // guard-clause-2
        //    // guard-clause-3

        //    return DoStuffInteral(param);
        //}

        //private object DoStuffInteral(object param)
        //{
        //    // ....
        //}
    }
}
