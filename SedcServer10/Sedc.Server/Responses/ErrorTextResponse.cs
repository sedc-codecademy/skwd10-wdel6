using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sedc.Server.Responses
{
    internal class ErrorTextResponse : TextResponse
    {
        public ErrorTextResponse(ResponseStatus errorStatus) 
        { 
            Status = errorStatus;
            Body = errorStatus.Message;
        }

    }

    internal class NotFoundResponse : ErrorTextResponse
    {
        public NotFoundResponse() : base(ResponseStatus.NotFound)
        {
        }
    }
}
