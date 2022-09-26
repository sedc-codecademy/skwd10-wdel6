using System.ComponentModel;

namespace Sedc.Server.Requests
{
    public record Method
    {
        public string Name { get; private set; }
        internal Method(string method)
        {
            Name = method;
        }

        public static readonly Method Get = new Method("GET");
        public static readonly Method Post = new Method("POST");
        public static readonly Method Put = new Method("PUT");
        public static readonly Method Patch = new Method("PATCH");
        public static readonly Method Delete = new Method("DELETE");
        public static readonly Method Head = new Method("HEAD");
        public static readonly Method Options = new Method("OPTIONS");
    }


    public record Operation
    {
        public string Name { get; private set; } = string.Empty;

        public string Symbol { get; private set; } = string.Empty;

        public Func<int, int, int> Execute { get; private set; } = (_, _) => 0;

        private Operation()
        {

        }

        public static readonly Operation Add = new()
        {
            Name = "Addition",
            Symbol = "+",
            Execute = (x, y) => x+y
        };
    }
}