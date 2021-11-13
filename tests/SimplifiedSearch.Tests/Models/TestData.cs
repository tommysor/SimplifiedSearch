using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Globalization;
using CsvHelper;

namespace SimplifiedSearch.Tests.Models
{
    internal static class TestData
    {
        private const int RedditShortLongCutoffPoint = 60;

        static TestData()
        {
            Countries = GetCountries();
            CountriesString = Countries.Select(x => x.Name).ToArray();

            var redditAnimes = GetRedditAnime();
            RedditAnimeShortPosts = GetRedditAnimeShortPosts(redditAnimes);
            RedditAnimeLongPosts = GetRedditAnimeLongPosts(redditAnimes);
        }

        private static string GetPathToDataDirectory()
        {
            var baseDirectory = AppContext.BaseDirectory;
            //var path = Path.Combine(baseDirectory, @"..\..\..\..\data\");
            var path = Path.Combine(baseDirectory, "..", "..", "..", "..", "data");
            return path;
        }

        private static IList<TestItem> GetCountries()
        {
            var pathData = GetPathToDataDirectory();
            var path = Path.Combine(pathData, "annexare", "Countries", "countries.json");

            using var streamReader = new StreamReader(path);
            var fileContent = streamReader.ReadToEnd();
            var deserializedObject = Newtonsoft.Json.JsonConvert.DeserializeObject(fileContent);
            if (deserializedObject is not Newtonsoft.Json.Linq.JObject countries)
                throw new InvalidOperationException($"Failed to get testdata 'Countries'. Expected result of type '{typeof(Newtonsoft.Json.Linq.JObject)}', got object of type '{deserializedObject?.GetType()}'");

            var results = new List<TestItem>();
            var index = 0;
            foreach (var country in countries)
            {
                if (country.Value is not Newtonsoft.Json.Linq.JObject countryValues)
                    continue;

                foreach (var countryValue in countryValues)
                {
                    if ("name".Equals(countryValue.Key, StringComparison.InvariantCultureIgnoreCase))
                    {
                        var countryName = countryValue.Value?.ToString();
                        if (!string.IsNullOrEmpty(countryName))
                        {
                            index++;
                            var testItem = new TestItem { Id = index, Name = countryName };
                            results.Add(testItem);
                        }
                        break;
                    }
                }
            }

            return results;
        }

        private static IList<TestItem> GetUsStates()
        {
            var pathData = GetPathToDataDirectory();
            var path = Path.Combine(pathData, "CivilServiceUSA", "us-states", "states.json");

            using var streamReader = new StreamReader(path);
            var fileContent = streamReader.ReadToEnd();
            var states = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(fileContent, new[] { new { state = "" } });
            if (states is null)
                throw new InvalidOperationException($"Failed to get testdata 'UsStates'.");

            var results = new List<TestItem>();
            var index = 0;
            foreach (var state in states)
            {
                if (!string.IsNullOrEmpty(state.state))
                {
                    index++;
                    var testItem = new TestItem { Id = index, Name = state.state };
                    results.Add(testItem);
                }
            }

            return results;
        }

        private static ICollection<string> GetRedditAnime()
        {
            var pathData = GetPathToDataDirectory();
            var path = Path.Combine(pathData, "linanqiu", "reddit-dataset", "entertainment_anime.csv");

            using var streamReader = new StreamReader(path);
            using var csvReader = new CsvReader(streamReader, CultureInfo.GetCultureInfo("en-US"));

            var list = new HashSet<string>();
            while (csvReader.Read())
            {
                var fieldValue = csvReader.GetField(2).Trim();
                if (!string.IsNullOrEmpty(fieldValue))
                    list.Add(fieldValue);
            }

            return list;
        }

        private static IList<string> GetRedditAnimeShortPosts(ICollection<string> redditAnimes)
        {
            var list = redditAnimes.Where(x => x.Length <= RedditShortLongCutoffPoint).ToArray();
            return list;
        }

        private static IList<string> GetRedditAnimeLongPosts(ICollection<string> redditAnimes)
        {
            var list = redditAnimes.Where(x => x.Length > RedditShortLongCutoffPoint && x.Length <= 700).ToArray();
            return list;
        }

        private static IList<TestItem> GetTestItemWithEnum()
        {
            var list = new[]
            {
                new TestItem { Id = 1, TestEnum = TestEnum.First },
                new TestItem { Id = 2, TestEnum = TestEnum.Second }
            };

            return list;
        }

        internal static IList<TestItem> Countries { get; }

        internal static IList<string?> CountriesString { get; }

        internal static IList<TestItem> UsStates { get; } = GetUsStates();

        internal static IList<string> RedditAnimeShortPosts { get; }

        internal static IList<string> RedditAnimeLongPosts { get; }

        internal static IList<TestEnum> Enums { get; } = Enum.GetValues(typeof(TestEnum)).Cast<TestEnum>().ToArray();

        internal static IList<TestItem> ItemsWithEnum { get; } = GetTestItemWithEnum();
    }
}
