using Sedc.Server.Attributes;
using Sedc.Server.Exceptions;

using System.Reflection;

namespace Sedc.Server.Responses
{

    internal class ParameterMatch
    {
        public bool Valid { get; set; }
        public List<(string value, ParameterInfo pinfo)> Params { get; set; } = new();
    }

    internal class ApiHelper
    {
        private static object CastParameterValue(ParameterInfo pinfo, string value)
        {
            if (pinfo.ParameterType == typeof(string))
            {
                return value;
            }

            if (pinfo.ParameterType == typeof(bool))
            {
                return bool.Parse(value);
            }

            if (pinfo.ParameterType == typeof(int))
            {
                return int.Parse(value);
            }

            throw new ServerException("Invalid parameter serialization");
        }

        internal static object? CallMethod(object apiProcessor, MethodInfo controllerMethod, ParameterMatch parameters)
        {
            return controllerMethod.Invoke(apiProcessor, parameters.Params.Select(p => CastParameterValue(p.pinfo, p.value)).ToArray());
        }

        internal static MethodInfo? FindMethod(object apiProcessor, string requestedMethod)
        {
            var type = apiProcessor.GetType();
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (var minfo in methods)
            {
                if (minfo.Name.ToLowerInvariant() == requestedMethod.ToLowerInvariant())
                {
                    return minfo;
                }

                var attribute = minfo.GetCustomAttribute<RouteNameAttribute>();
                if (attribute == null) {
                    continue;
                }
                if (attribute.Route.ToLowerInvariant() == requestedMethod.ToLowerInvariant())
                {
                    return minfo;
                }
            }
            return null;
        }

        internal static ParameterMatch MatchParams(MethodInfo controllerMethod, string[] parameters)
        {
            var result = new ParameterMatch
            {
                Valid = true
            };
            var methodParams = controllerMethod.GetParameters();
            if (methodParams.Length != parameters.Length)
            {
                return new ParameterMatch { Valid = false };
            }
            for (int index = 0; index < parameters.Length; index++)
            {
                var pvalue = parameters[index];
                var pinfo = methodParams[index];

                // if pinfo is string-type, it's fine
                if (pinfo.ParameterType == typeof(string))
                {
                    result.Params.Add((pvalue, pinfo));
                    continue;
                }

                if (pinfo.ParameterType == typeof(bool))
                {
                    var validBools = new string[] { "false", "true" };
                    if (validBools.Contains(pvalue.ToLowerInvariant()))
                    {
                        result.Params.Add((pvalue, pinfo));
                        continue;
                    }
                    return new ParameterMatch { Valid = false };
                }

                if (pinfo.ParameterType == typeof(int)) 
                {
                    var isValid = int.TryParse(pvalue, out _);
                    if (isValid)
                    {
                        result.Params.Add((pvalue, pinfo));
                        continue;
                    }
                    return new ParameterMatch { Valid = false };
                }

                //...

                // if we don't know what to do with the param value (the param type is a weird one)
                return new ParameterMatch { Valid = false };
            }

            return result;
        }
    }
}