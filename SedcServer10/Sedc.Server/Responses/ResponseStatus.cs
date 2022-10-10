using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sedc.Server.Responses
{
    public record ResponseStatus
    {
        public int Id { get; init; }
        public string Message { get; init; } = string.Empty;

        public static ResponseStatus OK = new ResponseStatus { Id = 200, Message = "OK" };
        public static ResponseStatus NotFound = new ResponseStatus { Id = 404, Message = "Not Found" };
    }
}
