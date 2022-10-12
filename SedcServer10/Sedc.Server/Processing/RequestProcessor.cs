using Sedc.Server.Exceptions;
using Sedc.Server.Logging;
using Sedc.Server.Requests;
using Sedc.Server.Responses;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        public ILogger Logger { get; init; }
        public RequestProcessor(ILogger logger) {
            Logger = logger;
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

        internal void AddApiResponder(string route, object apiProcessor)
        {
            var responder = ResponderRepository.GetApiResponder(route, apiProcessor);
            Responders.Insert(Responders.Count - 1, responder);
        }

        internal void AddFileResponder(string route, string path)
        {
            var responder = ResponderRepository.GetFileResponder(route, path);
            Responders.Insert(Responders.Count-1, responder);
        }
    }
}
