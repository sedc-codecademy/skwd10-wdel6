using Sedc.Server.Responses;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sedc.Server.Exceptions
{
    public class ServerException : ApplicationException
    {
        public ResponseStatus Status { get; set; } = new ResponseStatus();

        public ServerException() { }
        public ServerException(string message) : base(message) { }
        public ServerException(string message, Exception innerException) : base(message, innerException) { }
    }
}
