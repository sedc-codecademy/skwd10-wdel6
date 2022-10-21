using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActualServer
{
    internal class DbOrderAttribute : Attribute
    {
        public int Order { get; set; }
        public DbOrderAttribute(int order) { 
            Order = order;
        }
    }
}
