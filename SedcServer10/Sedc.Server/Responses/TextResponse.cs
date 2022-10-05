using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sedc.Server.Responses
{
    internal class TextResponse : IResponse<string>
    {
        public ResponseStatus Status { get; set; } = ResponseStatus.OK;

        public Dictionary<string, string> Headers { get; set; } = new();
        public string Body { get; set; }
        public int ContentLength { get => Body.Length; }

        public byte[] GetBodyBytes()
        {
            return Encoding.UTF8.GetBytes(Body);
        }
    }

}
