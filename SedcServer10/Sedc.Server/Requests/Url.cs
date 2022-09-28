using System.Text.RegularExpressions;

namespace Sedc.Server.Requests
{
    public record Url
    {
        private static readonly Regex UrlRegex = new Regex(@"^\/([^?]*)(?:\?(.*))?$");
        public UrlPath Path { get; private set; } = new ();
        public Dictionary<string, string> Query { get; private set; } = new();

        private Url() { }

        public static Url Parse(string urlString)
        {
            var result = new Url();
            result.Path.RawValue = urlString;
            var match = UrlRegex.Match(urlString);
            if (!match.Success)
            {
                return result;
            }

            var paths = match.Groups[1].Value;
            result.Path.Paths = paths.Split("/", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            result.Path.Length = result.Path.Paths.Length;

            if (match.Groups.Count > 2) {
                var queryString = match.Groups[2].Value;
                var queries = queryString.Split("&", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                result.Query = queries.Select(q => {
                    var qparts = q.Split("=", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    return new
                    {
                        Key = qparts[0],
                        Value = qparts[1]
                    };
                 }).ToDictionary(q => q.Key, q => q.Value);
            }
            return result;
        }
    }

    public record UrlPath
    {
        public string RawValue { get; internal set; } = string.Empty;

        public int Length { get; internal set; } = 0;
        public string[] Paths { get; internal set; } = Array.Empty<string>();
        internal UrlPath() { }
    }
}