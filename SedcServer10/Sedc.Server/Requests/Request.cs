using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sedc.Server.Requests
{
    internal record Request
    {
        public Method Method { get; init; }

        public string ProtocolVersion { get; private set; } = "1.1";

        public string Url { get; init; }

        public ReadOnlyDictionary<string, string> Headers { get; init; }

        public string Body { get; init; }

        public string RawRequest { get; init; }

        public Request() {

        }
    }

}
