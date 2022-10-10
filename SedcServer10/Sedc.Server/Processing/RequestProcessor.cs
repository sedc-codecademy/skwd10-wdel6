using Sedc.Server.Exceptions;
using Sedc.Server.Requests;
using Sedc.Server.Responses;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sedc.Server.Processing
{
    internal class RequestProcessor
    {
        public List<Responder> Responders { get; set; } = new List<Responder>
        {
            new FaviconResponder(),
            new FileResponder(),
            new DefaultResponder(),
        };
        public RequestProcessor() { 
        
        }

        public IResponse ProcessRequest(Request request)
        {
            foreach (var responder in Responders)
            {
                if (responder.IsApplicable(request))
                {
                    return responder.GenerateResponse(request);
                }
            };
            throw new ServerException("First responder failed to appear");
        }


    }
}
