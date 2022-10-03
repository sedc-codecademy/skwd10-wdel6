using System.ComponentModel;

namespace Sedc.Server.Requests
{
    public record Method
    {
        public string Name { get; private set; }
        private Method(string method)
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

        //private static Dictionary<string, Method> predefinedMethods = new Dictionary<string, Method>
        //{
        //    { "GET", Get },
        //    { "POST", Post },
        //    { "PUT", Put },
        //    { "DELETE", Delete },
        //    { "HEAD", Head },
        //};

        //public static Method GetMethod(string method)
        //{
        //    if (predefinedMethods.TryGetValue(method, out Method value))
        //    {
        //        return value;
        //    }
        //    return new Method(method.ToUpper());
        //}

        public static Method GetMethod(string method)
        {
            var result = method.ToUpper() switch
            {
                "GET" => Get,
                "POST" => Post,
                "PUT" => Put,
                "DELETE" => Delete,
                "PATCH" => Patch,
                "HEAD" => Head,
                "OPTIONS" => Options,
                _ => new Method(method.ToUpper())
            };

            return result;
        }
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