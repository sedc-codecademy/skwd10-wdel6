using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sedc.Server.Responses
{
    public interface IResponse
    {
        ResponseStatus Status { get; set; }

        Dictionary<string, string> Headers { get; set; }

        int ContentLength { get; }

        byte[] GetBodyBytes();
    }

    public interface IResponse<T>: IResponse
    {
        T Body { get; set; }
    }
}
