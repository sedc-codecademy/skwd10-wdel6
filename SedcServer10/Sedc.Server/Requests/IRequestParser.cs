using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sedc.Server.Requests
{
    public interface IRequestParser
    {
        IRequest TryParse(string requestData);
    }

    public delegate IRequestParser IRequestParserFactory();
}
