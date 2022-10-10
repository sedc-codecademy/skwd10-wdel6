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
        private List<Responder> Responders = new List<Responder>
        {
            ResponderRepository.FaviconResponder,
            ResponderRepository.GetFileResponder("site", @"C:\Source\SEDC\skwd10-wdel6\site"),
            ResponderRepository.DefaultResponder
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

        internal void AddFileResponder(string route, string path)
        {
            var responder = ResponderRepository.GetFileResponder(route, path);
            Responders.Insert(Responders.Count-1, responder);
        }
    }
}
