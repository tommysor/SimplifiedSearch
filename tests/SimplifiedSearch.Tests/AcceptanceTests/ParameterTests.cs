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
    public class ParameterTests
    {
        private readonly ISimplifiedSearch _search;

        public ParameterTests()
        {
            var factory = new SimplifiedSearchFactory();
            _search = factory.GetSimplifiedSearch();
        }

        [Fact]
        public async Task ListNull_ThrowsException()
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var func = new Func<Task>(async () => await _search.SimplifiedSearchAsync<TestItem>(null, "a", x => x.Name));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            await Assert.ThrowsAsync<ArgumentNullException>("searchThisList", func);
        }

        [Fact]
        public async Task SearchTermNull_ReturnsSameList()
        {
            var expected = TestData.UsStates;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var actual = await _search.SimplifiedSearchAsync(TestData.UsStates, null, x => x.Name);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.Same(expected, actual);
        }

        [Fact]
        public async Task SearchTermEmpty_ReturnsSameList()
        {
            var expected = TestData.UsStates;

            var actual = await _search.SimplifiedSearchAsync(TestData.UsStates, "", x => x.Name);

            Assert.Same(expected, actual);
        }

        [Fact]
        public async Task fieldToSearch_SearchOnlyThatField()
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
        public async Task fieldToSearchNull_SearchAllFields()
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
        public async Task fieldToSearchNull_Overload_searchAllFields()
        {
            var list = new[]
{
                new TestItem { Id = 1, Name = "a" },
                new TestItem { Id = 2, Name = "1" },
                new TestItem { Id = 3, Name = "z"}
            };

            var expected = list[0..2];

            var actual = await _search.SimplifiedSearchAsync(list, "1");

            AssertCollectionUtils.AssertCollectionContainsEqualIds(expected, actual);
        }

        [Fact]
        public async Task ListOfString()
        {
            var actual = await _search.SimplifiedSearchAsync(TestData.CountriesString, "Bahamas");

            Assert.Equal("Bahamas", actual.First());
        }

        [Fact]
        public async Task ListOfInt()
        {
            var ids = new List<int>();
            for (var i = 0; i < 30; i++)
                ids.Add(i);

            var actual = await _search.SimplifiedSearchAsync(ids, "23");

            Assert.Single(actual, 23);
        }

        [Fact]
        public async Task ListOfEnum()
        {
            var actual = await _search.SimplifiedSearchAsync(TestData.Enums, nameof(TestEnum.Second));

            Assert.Single(actual, TestEnum.Second);
        }

        [Fact]
        public async Task ListOfItemWithEnum_SpesifyField()
        {
            var expected = TestData.ItemsWithEnum.First(x => x.TestEnum == TestEnum.Second);

            var actual = await _search.SimplifiedSearchAsync(TestData.ItemsWithEnum, nameof(TestEnum.Second), x => x.TestEnum?.ToString());

            Assert.Single(actual, expected);
        }

        [Fact]
        public async Task ListOfItemWithEnum_AutoFields()
        {
            var expected = TestData.ItemsWithEnum.First(x => x.TestEnum == TestEnum.Second);

            var actual = await _search.SimplifiedSearchAsync(TestData.ItemsWithEnum, nameof(TestEnum.Second));

            Assert.Single(actual, expected);
        }
    }
}
