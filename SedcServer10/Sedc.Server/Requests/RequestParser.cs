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
    internal class RequestParser
    {
        private static readonly Regex RequestLineRegex = new Regex(@"([A-Z]+) (.*) HTTP\/(.*)$");

        public RequestParser() { }

        public IRequest TryParse(string requestData)
        {
            var requestLines = requestData.Split(Environment.NewLine);

            var requestLine = requestLines[0];
            var rmatch = RequestLineRegex.Match(requestLine);
            //if (!rmatch.Success)
            //{
            // throw new ServerException;
            return new InvalidRequest("Invalid HTTP request received");
            // }

            // requestLines = requestLines.Skip(1);

            var result = new Request
            {
                Body = requestData,
                Headers = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>()),
                Method = Method.Get,
                RawRequest = requestData,
                Url = Url.Parse(requestData)
            };
            return result;
        }
    }
}
