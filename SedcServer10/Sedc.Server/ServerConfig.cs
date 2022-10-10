using Sedc.Server.Requests;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sedc.Server
{
    public class ServerConfig
    {
        public ParseRequest RequestParser { get; set; }
        public ServerConfig() 
        {
            RequestParser = RequestHelper.TryParse;
        }
    }
}
