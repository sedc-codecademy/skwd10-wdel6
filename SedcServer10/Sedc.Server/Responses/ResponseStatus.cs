using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sedc.Server.Responses
{
    public record ResponseStatus
    {
        public int Id { get; set; }
        public string Message { get; set; }

        public static ResponseStatus OK = new ResponseStatus { Id = 200, Message = "OK" };
    }
}
