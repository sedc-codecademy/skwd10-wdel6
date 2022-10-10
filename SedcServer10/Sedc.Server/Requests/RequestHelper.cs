using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sedc.Server.Requests
{
    internal static partial class RequestHelper
    {
        public static bool IsValid(this IRequest request)
        {
            return request is not InvalidRequest;
        }
    }
}
