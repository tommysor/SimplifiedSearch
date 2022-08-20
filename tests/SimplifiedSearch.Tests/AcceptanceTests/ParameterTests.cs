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
        [Fact]
        public async Task ListNull_ThrowsException()
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            IList<TestItem> nullList = null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

#pragma warning disable CS8604 // Possible null reference argument.
            var func = new Func<Task>(async () => await nullList.SimplifiedSearchAsync("a", x => x.Name));
#pragma warning restore CS8604 // Possible null reference argument.

            await Assert.ThrowsAsync<ArgumentNullException>("searchThisList", func);
        }

        [Fact]
        public async Task SearchTermNull_ReturnsSameList()
        {
            var expected = TestData.UsStates;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var actual = await TestData.UsStates.SimplifiedSearchAsync(null, x => x.Name);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.Same(expected, actual);
        }

        [Fact]
        public async Task SearchTermEmpty_ReturnsSameList()
        {
            var expected = TestData.UsStates;

            var actual = await TestData.UsStates.SimplifiedSearchAsync("", x => x.Name);

            Assert.Same(expected, actual);
        }

        [Fact]
        public async Task PropertyToSearch_SearchOnlyThatProperty()
        {
            var list = new[]
            {
                new TestItem { Id = 1, Name = "abcd", Description = "efgh" },
                new TestItem { Id = 2, Name = "efgh" }
            };

            var expected = list.Last();

            var actuals = await list.SimplifiedSearchAsync("efgh", x => x.Name);

            Assert.Single(actuals, expected);
        }

        [Fact]
        public async Task PropertyToSearchNull_SearchAllProperty()
        {
            var list = new[]
            {
                new TestItem { Id = 1, Name = "a" },
                new TestItem { Id = 2, Name = "1" },
                new TestItem { Id = 3, Name = "z"}
            };

            var expected = list.Take(2);

            var actual = await list.SimplifiedSearchAsync("1", null);

            AssertCollectionUtils.AssertCollectionContainsEqualIds(expected, actual);
        }

        [Fact]
        public async Task PropertyToSearchNull_Overload_searchAllProperty()
        {
            var list = new[]
{
                new TestItem { Id = 1, Name = "a" },
                new TestItem { Id = 2, Name = "1" },
                new TestItem { Id = 3, Name = "z"}
            };

            var expected = list.Take(2);

            var actual = await list.SimplifiedSearchAsync("1");

            AssertCollectionUtils.AssertCollectionContainsEqualIds(expected, actual);
        }

        [Fact]
        public async Task ListOfString()
        {
            var actual = await TestData.CountriesString.SimplifiedSearchAsync("Morocco");

            Assert.Equal("Morocco", actual.First());
        }

        [Fact]
        public async Task ListOfInt()
        {
            var ids = new List<int>();
            for (var i = 0; i < 30; i++)
                ids.Add(i);

            var actual = await ids.SimplifiedSearchAsync("23");

            Assert.Single(actual, 23);
        }

        [Fact]
        public async Task ListOfEnum()
        {
            var actual = await TestData.Enums.SimplifiedSearchAsync(nameof(TestEnum.Second));

            Assert.Single(actual, TestEnum.Second);
        }

        [Fact]
        public async Task ListOfItemWithEnum_SpesifyProperty()
        {
            var expected = TestData.ItemsWithEnum.First(x => x.TestEnum == TestEnum.Second);

            var actual = await TestData.ItemsWithEnum.SimplifiedSearchAsync(nameof(TestEnum.Second), x => x.TestEnum?.ToString());

            Assert.Single(actual, expected);
        }

        [Fact]
        public async Task ListOfItemWithEnum_AutoProperty()
        {
            var expected = TestData.ItemsWithEnum.First(x => x.TestEnum == TestEnum.Second);

            var actual = await TestData.ItemsWithEnum.SimplifiedSearchAsync(nameof(TestEnum.Second));

            Assert.Single(actual, expected);
        }
    }
}
