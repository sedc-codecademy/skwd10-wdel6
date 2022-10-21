using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sedc.Server.Responses
{
    internal static class JsonSerializer
    {
        private static string Jsonize(string name)
        {
            string first = name[..1].ToLowerInvariant();
            string rest = name[1..];
            return first + rest;
        }

        public static string Serialize(object? obj) {
            if (obj == null) { 
                throw new ArgumentNullException(nameof(obj)); 
            }
            var type = obj.GetType();

            if (type == typeof(string))
            {
                return $"\"{(string)obj}\"";
            }
            if (type == typeof(bool))
            {
                var b = (bool)obj;
                if (b)
                {
                    return "true";
                } 
                else
                {
                    return "false";
                }
            }
            if (type == typeof(int)) {
                var i = (int)obj;
                return i.ToString();
            }

            // if the object is a collection?
            if (type.IsArray) { 
                var arr = (object[])obj;
                var listValues = new List<string>();
                foreach (var item in arr)
                {
                    listValues.Add(Serialize(item));
                }
                var result = $"[{string.Join(", ", listValues)}]";
                return result;
            }

            var propValues = new List<string>();
            var props = type.GetProperties();
            foreach (var prop in props)
            {
                var value = prop.GetValue(obj, null);
                propValues.Add($"\"{Jsonize(prop.Name)}\": {Serialize(value)}");
            }

            return $"{{ {string.Join(", ", propValues)} }}";
        }
    }
}
