using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace SimplifiedSearch.Tests.Models
{
    internal static class TestData
    {
        private const string CountriesFileName = "countries.txt";
        private const string UsStatesFileName = "UsStates.txt";

        private static IList<TestItem> TestItemFromFile(string fileName)
        {
            var baseDirectory = AppContext.BaseDirectory;
            var path = baseDirectory + @"..\..\..\..\data\" + fileName;

            var lines = File.ReadAllLines(path);
            var list = new List<TestItem>();
            for (var i = 0; i < lines.Length; i++)
                list.Add(new TestItem { Id = i + 1, Name = lines[i] });
            return list;
        }

        private static IList<T> TestItemFromFileField<T>(string fileName, Func<TestItem, T> selector)
        {
            var list = TestItemFromFile(fileName);
            var results = list.Select(selector).ToArray();
            return results;
        }

        internal static IList<TestItem> Countries { get; } = TestItemFromFile(CountriesFileName);

        internal static IList<string?> CountriesString { get; } = TestItemFromFileField(CountriesFileName, x => x.Name);

        internal static IList<int> CountriesIndexes { get; } = TestItemFromFileField(CountriesFileName, x => x.Id);

        internal static IList<TestItem> UsStates { get; } = TestItemFromFile(UsStatesFileName);

        internal static IList<TestItem> GermanDistrictsLimited { get; } = new[]
        {
            new TestItem { Id = 1, Name = "Düsseldorf" },
            new TestItem { Id = 2, Name = "Bergstraße" },
            new TestItem { Id = 3, Name = "Böblingen" },
            new TestItem { Id = 4, Name = "Ostallgäu" },
            new TestItem { Id = 5, Name = "Südliche Weinstraße"}
        };
    }
}
