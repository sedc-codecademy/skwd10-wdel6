using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sedc.Server.Responses
{
    internal class BinaryResponse : IResponse<byte[]>
    {
        public byte[] Body { get; set; }
        public ResponseStatus Status { get; set; } = ResponseStatus.OK;
        public Dictionary<string, string> Headers { get; set; } = new();
        public int ContentLength { get => Body.Length; }

        public byte[] GetBodyBytes() => Body;
    }
}
