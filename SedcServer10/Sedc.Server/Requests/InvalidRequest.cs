using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sedc.Server.Requests
{
    internal record InvalidRequest : IRequest
    {
        public string Message { get; init; } = string.Empty;

        public InvalidRequest() { }
        public InvalidRequest(string message)
        {
            Message = message;
        }
    }
}
