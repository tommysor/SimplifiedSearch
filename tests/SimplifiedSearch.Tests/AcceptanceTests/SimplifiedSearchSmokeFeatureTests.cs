using SimplifiedSearch.Tests.Models;
using SimplifiedSearch.Tests.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimplifiedSearch.Tests.AcceptanceTests
{
    /// <summary>
    /// Try each feature one at a time from top level.
    /// </summary>
    public class SimplifiedSearchSmokeFeatureTests
    {
        private readonly ISimplifiedSearch _search;

        public SimplifiedSearchSmokeFeatureTests()
        {
            var factory = new SimplifiedSearchFactory();
            _search = factory.GetSimplifiedSearch();
        }

        [Fact]
        public async Task SimplifiedSearch_MatchWholeWordExcact()
        {
            var expected = TestData.UsStates.First(x => x.Name == "California");

            var actual = await _search.SimplifiedSearchAsync(TestData.UsStates, "California", x => x.Name);

            Assert.Same(expected, actual.First());
        }

        [Fact]
        public async Task SimplifiedSearch_MatchWholeWordExactCaseInsensitive()
        {
            var expected = TestData.UsStates.First(x => x.Name == "Arkansas");

            var actual = await _search.SimplifiedSearchAsync(TestData.UsStates, "arkANsas", x => x.Name);

            Assert.Same(expected, actual.First());
        }

        [Fact]
        public async Task SimplifiedSearch_MatchStartOfWordExact()
        {
            var expected = TestData.UsStates.First(x => x.Name == "Maryland");

            var actual = await _search.SimplifiedSearchAsync(TestData.UsStates, "Maryla", x => x.Name);

            Assert.Same(expected, actual.First());
        }

        [Fact]
        public async Task SimplifiedSearch_MatchWholeWordFuzzy()
        {
            var expected = TestData.UsStates.First(x => x.Name == "Montana");

            //                                                                   Montana
            var actual = await _search.SimplifiedSearchAsync(TestData.UsStates, "MoZtanZ", x => x.Name);

            Assert.Same(expected, actual.First());
        }

        [Fact]
        public async Task SimplifiedSearch_MatchStartOfWordFuzzy()
        {
            var expected = TestData.UsStates.First(x => x.Name == "Hawaii");

            //                                                                   Hawaii
            var actual = await _search.SimplifiedSearchAsync(TestData.UsStates, "hZwa", x => x.Name);

            Assert.Same(expected, actual.First());
        }

        [Fact(Skip = "Ascii folding not implemented.")]
        public async Task SimplifiedSearch_MatchAsciiFoldedWordExact_SearchList()
        {
            //                                                                                 Düsseldorf
            var actual = await _search.SimplifiedSearchAsync(TestData.GermanDistrictsLimited, "Dusseldorf", x => x.Name);
            Assert.Single(actual);
        }

        [Fact(Skip = "Ascii folding not implemented.")]
        public async Task SimplifiedSearch_MatchAsciiFoldedWordExact_SearchTerm()
        {
            //                                                                                 Böblingen
            var actual = await _search.SimplifiedSearchAsync(TestData.GermanDistrictsLimited, "Böblingeñ", x => x.Name);
            Assert.Single(actual);
        }

        [Fact(Skip = "Ascii folding not implemented.")]
        public async Task SimplifiedSearch_MatchAsciiFoldedWordExact_SearchList_DoubleAscii()
        {
            // string.Contains with CurrentCultureIgnoreCase will pass this test.
            //                                                                                 Bergstraße
            var actual = await _search.SimplifiedSearchAsync(TestData.GermanDistrictsLimited, "Bergstrasse", x => x.Name);
            Assert.Single(actual);
        }

        [Fact(Skip = "Ascii folding not implemented.")]
        public async Task SimplifiedSearch_MatchAsciiFoldedWordExact_SearchTerm_DoubleAscii()
        {
            // string.Contains with CurrentCultureIgnoreCase will pass this test.
            //                                                                                 Düsseldorf
            var actual = await _search.SimplifiedSearchAsync(TestData.GermanDistrictsLimited, "Düßeldorf", x => x.Name);
            Assert.Single(actual);
        }
    }
}
