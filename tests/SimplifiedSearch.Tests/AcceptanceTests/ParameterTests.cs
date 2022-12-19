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
        private readonly ISimplifiedSearch _sut;

        public ParameterTests()
        {
            _sut = new SimplifiedSearchFactory().Create();
        }

        [Fact]
        public async Task ListNull_ThrowsException()
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            IList<TestItem> nullList = null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

#pragma warning disable CS8604 // Possible null reference argument.
            var func = new Func<Task>(async () => await _sut.SimplifiedSearchAsync(nullList, "a", x => x.Name));
#pragma warning restore CS8604 // Possible null reference argument.

            await Assert.ThrowsAsync<ArgumentNullException>("searchThisList", func);
        }

        [Fact]
        public async Task SearchTermNull_ReturnsSameList()
        {
            var expected = TestData.Countries;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var actual = await _sut.SimplifiedSearchAsync(TestData.Countries, null, x => x.Name);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.Same(expected, actual);
        }

        [Fact]
        public async Task SearchTermEmpty_ReturnsSameList()
        {
            var expected = TestData.Countries;

            var actual = await _sut.SimplifiedSearchAsync(TestData.Countries, "", x => x.Name);

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

            var actuals = await _sut.SimplifiedSearchAsync(list, "efgh", x => x.Name);

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

            var actual = await _sut.SimplifiedSearchAsync(list, "1", null);

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

            var actual = await _sut.SimplifiedSearchAsync(list, "1");

            AssertCollectionUtils.AssertCollectionContainsEqualIds(expected, actual);
        }

        [Fact]
        public async Task ListOfString()
        {
            var actual = await _sut.SimplifiedSearchAsync(TestData.CountriesString, "Morocco");

            Assert.Equal("Morocco", actual.First());
        }

        [Fact]
        public async Task ListOfInt()
        {
            var ids = new List<int>();
            for (var i = 0; i < 30; i++)
                ids.Add(i);

            var actual = await _sut.SimplifiedSearchAsync(ids, "23");

            Assert.Single(actual, 23);
        }

        [Fact]
        public async Task ListOfEnum()
        {
            var actual = await _sut.SimplifiedSearchAsync(TestData.Enums, nameof(TestEnum.Second));

            Assert.Single(actual, TestEnum.Second);
        }

        [Fact]
        public async Task ListOfItemWithEnum_SpesifyProperty()
        {
            var expected = TestData.ItemsWithEnum.First(x => x.TestEnum == TestEnum.Second);

            var actual = await _sut.SimplifiedSearchAsync(TestData.ItemsWithEnum, nameof(TestEnum.Second), x => x.TestEnum?.ToString());

            Assert.Single(actual, expected);
        }

        [Fact]
        public async Task ListOfItemWithEnum_AutoProperty()
        {
            var expected = TestData.ItemsWithEnum.First(x => x.TestEnum == TestEnum.Second);

            var actual = await _sut.SimplifiedSearchAsync(TestData.ItemsWithEnum, nameof(TestEnum.Second));

            Assert.Single(actual, expected);
        }
    }
}
