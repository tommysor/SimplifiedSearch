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
            var actual = await _search.SimplifiedSearchAsync(TestData.UsStates, "California", x => x.Name);
            Assert.Single(actual);
        }

        [Fact]
        public async Task SimplifiedSearch_MatchWholeWordExactCaseInsensitive()
        {
            var actual = await _search.SimplifiedSearchAsync(TestData.UsStates, "arkANsas", x => x.Name);
            Assert.Single(actual);
        }

        [Fact]
        public async Task SimplifiedSearch_MatchStartOfWordExact()
        {
            var actual = await _search.SimplifiedSearchAsync(TestData.UsStates, "Maryla", x => x.Name);
            Assert.Single(actual);
        }

        [Fact(Skip = "Fuzzy matching not implemented.")]
        public async Task SimplifiedSearch_MatchWholeWordFuzzy()
        {
            //                                                                   Montana
            var actual = await _search.SimplifiedSearchAsync(TestData.UsStates, "MoZtanZ", x => x.Name);
            Assert.Single(actual);
        }

        [Fact(Skip = "Fuzzy matching not implemented.")]
        public async Task SimplifiedSearch_MatchStartOfWordFuzzy()
        {
            //                                                                   Hawaii
            var actual = await _search.SimplifiedSearchAsync(TestData.UsStates, "hZwZ", x => x.Name);
            Assert.Single(actual);
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
