using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sedc.Server.Attributes
{
    public class RouteNameAttribute : Attribute
    {
        public string Route { get; private set; }

        public RouteNameAttribute(string route)
        {
            Route = route;
        }

    }
}
