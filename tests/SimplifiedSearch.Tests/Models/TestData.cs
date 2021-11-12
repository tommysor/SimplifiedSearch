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
        private const string CountriesFileName = "countries.txt";
        private const string UsStatesFileName = "UsStates.txt";
        
        /// <summary>
        /// This file is from https://github.com/linanqiu/reddit-dataset
        /// </summary>
        private const string RedditAnimeFileName = "entertainment_anime.csv";

        private const int RedditShortLongCutoffPoint = 60;

        private static string GetPath(string fileName)
        {
            var baseDirectory = AppContext.BaseDirectory;
            var path = Path.Combine(baseDirectory, @"..\..\..\..\data\", fileName);
            return path;
        }

        private static IList<TestItem> TestItemFromFile(string fileName)
        {
            var path = GetPath(fileName);

            var lines = File.ReadAllLines(path);
            var list = new List<TestItem>();
            for (var i = 0; i < lines.Length; i++)
                list.Add(new TestItem { Id = i + 1, Name = lines[i] });
            return list;
        }

        private static IList<T> TestItemFromFileProperty<T>(string fileName, Func<TestItem, T> selector)
        {
            var list = TestItemFromFile(fileName);
            var results = list.Select(selector).ToArray();
            return results;
        }

        private static ICollection<string> GetRedditAnime()
        {
            var path = GetPath(RedditAnimeFileName);

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

        private static IList<string> GetRedditAnimeShortPosts()
        {
            var lines = GetRedditAnime();
            var list = lines.Where(x => x.Length <= RedditShortLongCutoffPoint).ToArray();
            return list;
        }

        private static IList<string> GetRedditAnimeLongPosts()
        {
            var lines = GetRedditAnime();
            var list = lines.Where(x => x.Length > RedditShortLongCutoffPoint && x.Length <= 700).ToArray();
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

        internal static IList<TestItem> Countries { get; } = TestItemFromFile(CountriesFileName);

        internal static IList<string?> CountriesString { get; } = TestItemFromFileProperty(CountriesFileName, x => x.Name);

        internal static IList<TestItem> UsStates { get; } = TestItemFromFile(UsStatesFileName);

        internal static IList<TestItem> GermanDistrictsLimited { get; } = new[]
        {
            new TestItem { Id = 1, Name = "Düsseldorf" },
            new TestItem { Id = 2, Name = "Bergstraße" },
            new TestItem { Id = 3, Name = "Böblingen" },
            new TestItem { Id = 4, Name = "Ostallgäu" },
            new TestItem { Id = 5, Name = "Südliche Weinstraße"}
        };

        internal static IList<string> RedditAnimeShortPosts { get; } = GetRedditAnimeShortPosts();

        internal static IList<string> RedditAnimeLongPosts { get; } = GetRedditAnimeLongPosts();

        internal static IList<TestEnum> Enums { get; } = Enum.GetValues(typeof(TestEnum)).Cast<TestEnum>().ToArray();

        internal static IList<TestItem> ItemsWithEnum { get; } = GetTestItemWithEnum();
    }
}
