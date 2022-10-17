using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sedc.Server.Responses
{
    internal class JsonResponse: TextResponse
    {
        public JsonResponse() {
            Headers.Add("Content-Type", "application/json");
        }
    }
}
