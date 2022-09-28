using Sedc.Server.Requests;

namespace Sedc.Server.Tests
{
    public class UrlTests
    {
        private class UrlLike
        {
            public UrlLike(string rawPath, string[] paths, Dictionary<string, string> query)
            {
                RawPath = rawPath;
                Paths = paths;
                Query = query;
            }

            public string RawPath { get; }
            public string[] Paths { get; }
            public Dictionary<string, string> Query { get; }
        }

        private bool CompareUrl(Url actual, UrlLike expected) 
        { 
            if (actual.Path.RawValue != expected.RawPath)
            {
                return false;
            }
            if (actual.Path.Length != expected.Paths.Length)
            {
                return false;
            }
            for (int i = 0; i < actual.Path.Length; i++)
            {
                var act = actual.Path.Paths[i];
                var exp = expected.Paths[i];
                if (act != exp)
                {
                    return false;
                }
            }
            if (actual.Query.Count != expected.Query.Count)
            {
                return false;
            }
            foreach (var (key, evalue) in expected.Query)
            {
                if (!actual.Query.TryGetValue(key, out var avalue)) {
                    return false;
                }
                if (avalue != evalue)
                {
                    return false;
                }
            }
            return true;
        }

        [Fact(DisplayName = "Parse empty string returns empty url")]
        public void Test_1()
        {
            // 1. Arrange
            var urlString = string.Empty;
            var expected = new UrlLike(
                rawPath: string.Empty,
                paths: Array.Empty<string>(),
                query: new Dictionary<string, string>()
            );
            // 2. Act
            var actual = Url.Parse(urlString);
            // 3. Assert
            Assert.True(CompareUrl(actual, expected));
        }

        [Fact(DisplayName = "Parse of simple slash should return empty query and empty path")]
        public void Test_2()
        {
            // 1. Arrange
            var urlString = "/";
            var expected = new UrlLike(
                rawPath: "/",
                paths: Array.Empty<string>(),
                query: new Dictionary<string, string>()
            );
            // 2. Act
            var actual = Url.Parse(urlString);
            // 3. Assert
            Assert.True(CompareUrl(actual, expected));
        }

        [Fact(DisplayName = "Parse of simple slash-text should return empty query and single path")]
        public void Test_3()
        {
            // 1. Arrange
            var urlString = "/one";
            var expected = new UrlLike(
                rawPath: "/one",
                paths: new string[] {"one"},
                query: new Dictionary<string, string>()
            );
            // 2. Act
            var actual = Url.Parse(urlString);
            // 3. Assert
            Assert.True(CompareUrl(actual, expected));
        }

        [Fact(DisplayName = "Parse of simple two-level path return empty query and two paths")]
        public void Test_4()
        {
            // 1. Arrange
            var urlString = "/one/two";
            var expected = new UrlLike(
                rawPath: "/one/two",
                paths: new string[] { "one", "two" },
                query: new Dictionary<string, string>()
            );
            // 2. Act
            var actual = Url.Parse(urlString);
            // 3. Assert
            Assert.True(CompareUrl(actual, expected));
        }

        [Fact(DisplayName = "Parse of slash-terminated two-level path return empty query and two paths")]
        public void Test_5()
        {
            // 1. Arrange
            var urlString = "/one/two/";
            var expected = new UrlLike(
                rawPath: "/one/two/",
                paths: new string[] { "one", "two" },
                query: new Dictionary<string, string>()
            );
            // 2. Act
            var actual = Url.Parse(urlString);
            // 3. Assert
            Assert.True(CompareUrl(actual, expected));
        }

        [Fact(DisplayName = "Parse of double-slash two-level path return empty query and two paths")]
        public void Test_6()
        {
            // 1. Arrange
            var urlString = "/one//two/";
            var expected = new UrlLike(
                rawPath: "/one//two/",
                paths: new string[] { "one", "two" },
                query: new Dictionary<string, string>()
            );
            // 2. Act
            var actual = Url.Parse(urlString);
            // 3. Assert
            Assert.True(CompareUrl(actual, expected));
        }

        [Fact(DisplayName = "Parse of leading space two-level path return empty query and two paths")]
        public void Test_7()
        {
            // 1. Arrange
            var urlString = "/one/ two";
            var expected = new UrlLike(
                rawPath: "/one/ two",
                paths: new string[] { "one", "two" },
                query: new Dictionary<string, string>()
            );
            // 2. Act
            var actual = Url.Parse(urlString);
            // 3. Assert
            Assert.True(CompareUrl(actual, expected));
        }

        [Fact(DisplayName = "Parse of question mark starting text shoud return empty path and valid query")]
        public void Test_8()
        {
            // 1. Arrange
            var urlString = "/?one=two";
            var expected = new UrlLike(
                rawPath: "/?one=two",
                paths: Array.Empty<string>(),
                query: new Dictionary<string, string>
                {
                    {"one", "two"},
                });
            // 2. Act
            var actual = Url.Parse(urlString);
            // 3. Assert
            Assert.True(CompareUrl(actual, expected));
        }

        [Fact(DisplayName = "Parse of question mark starting text containing a question mark shoud return empty path and valid query")]
        public void Test_9()
        {
            // 1. Arrange
            var urlString = "/?one=two?three";
            var expected = new UrlLike(
                rawPath: "/?one=two?three",
                paths: Array.Empty<string>(),
                query: new Dictionary<string, string>
                {
                    {"one", "two?three"},
                });
            // 2. Act
            var actual = Url.Parse(urlString);
            // 3. Assert
            Assert.True(CompareUrl(actual, expected));
        }
    }


}