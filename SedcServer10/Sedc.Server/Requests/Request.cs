using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sedc.Server.Requests
{
    internal record Request: IRequest
    {
        public Method Method { get; init; }

        public string ProtocolVersion { get; private set; } = "1.1";

        public Url Url { get; init; }

        public ReadOnlyDictionary<string, string> Headers { get; init; }

        public string Body { get; init; }

        public string RawRequest { get; init; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Method: {Method.Name}");
            sb.AppendLine($"URL: {Url.Path.RawValue}");
            if (Url.Query.Count > 0)
            {
                sb.AppendLine("Query:");
                foreach (var (key, value) in Url.Query)
                {
                    sb.AppendLine($"  {key} = {value}");
                }
            }
            if (Headers.Count > 0)
            {
                sb.AppendLine("Headers:");
                foreach (var (key, value) in Headers)
                {
                    sb.AppendLine($"  {key}: {value}");
                }
            }

            sb.AppendLine("Body:");
            sb.AppendLine(Body);

            return sb.ToString();
        }

    }

}
