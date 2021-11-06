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
        public async Task SimplifiedSearch_ListNull_ThrowsException()
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var func = new Func<Task>(async () => await _search.SimplifiedSearchAsync<TestItem>(null, "a", x => x.Name));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            await Assert.ThrowsAsync<ArgumentNullException>("searchThisList", func);
        }

        [Fact]
        public async Task SimplifiedSearch_SearchTermNull_ReturnsSameList()
        {
            var expected = TestData.UsStates;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var actual = await _search.SimplifiedSearchAsync(TestData.UsStates, null, x => x.Name);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.Same(expected, actual);
        }

        [Fact]
        public async Task SimplifiedSearch_SearchTermEmpty_ReturnsSameList()
        {
            var expected = TestData.UsStates;

            var actual = await _search.SimplifiedSearchAsync(TestData.UsStates, "", x => x.Name);

            Assert.Same(expected, actual);
        }

        [Fact]
        public async Task SimplifiedSearch_fieldToSearch_SearchOnlyThatField()
        {
            var list = new[]
            {
                new TestItem { Id = 1, Name = "abcd", Description = "efgh" },
                new TestItem { Id = 2, Name = "efgh" }
            };

            var expected = list.Last();

            var actuals = await _search.SimplifiedSearchAsync(list, "efgh", x => x.Name);

            Assert.Single(actuals, expected);
        }

        [Fact]
        public async Task SimplifiedSearch_fieldToSearchNull_SearchAllFields()
        {
            var list = new[]
            {
                new TestItem { Id = 1, Name = "a" },
                new TestItem { Id = 2, Name = "1" },
                new TestItem { Id = 3, Name = "z"}
            };

            var expected = list[0..2];

            var actual = await _search.SimplifiedSearchAsync(list, "1", null);

            AssertCollectionUtils.AssertCollectionContainsEqualIds(expected, actual);
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
