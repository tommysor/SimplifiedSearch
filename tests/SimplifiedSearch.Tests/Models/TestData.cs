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
        static TestData()
        {
            CountriesString = GetCountriesString();
            Countries = GetCountries(CountriesString);
        }

        private static string[] GetCountriesString()
        {
            var countries = new[]
            {
                "Taiwan",
                "Thailand",
                "Niger",
                "Nigeria",
                "Albania",
                "Morocco"
            };
            return countries;
        }

        private static List<TestItem> GetCountries(IList<string> countryNames)
        {
            var i = 0;
            var countries = new List<TestItem>();
            foreach (var countryName in countryNames)
            {
                i++;
                var country = new TestItem
                {
                    Id = i,
                    Name = countryName,
                };
                countries.Add(country);
            }

            return countries;
        }

        private static TestItem[] GetTestItemWithEnum()
        {
            var list = new[]
            {
                new TestItem { Id = 1, TestEnum = TestEnum.First },
                new TestItem { Id = 2, TestEnum = TestEnum.Second }
            };

            return list;
        }

        internal static IList<TestItem> Countries { get; }

        internal static IList<string> CountriesString { get; }

        internal static IList<TestEnum> Enums { get; } = Enum.GetValues(typeof(TestEnum)).Cast<TestEnum>().ToArray();

        internal static IList<TestItem> ItemsWithEnum { get; } = GetTestItemWithEnum();
    }
}
