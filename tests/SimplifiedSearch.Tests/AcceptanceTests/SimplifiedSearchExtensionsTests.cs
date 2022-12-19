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
    public class SimplifiedSearchExtensionsTests : IDisposable
    {
        public SimplifiedSearchExtensionsTests()
        {
            SimplifiedSearchFactory.Instance.ResetToDefault();
        }

        public void Dispose()
        {
            SimplifiedSearchFactory.Instance.ResetToDefault();
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task OverrideDefault_WithConfig_ExtensionMethod_CanBeUsed()
        {
            SimplifiedSearchFactory.Instance.Add("default", c => c.ResultSelector = new Configurations.ResultSelectorTop1());
            var list = new[]
            {
                new TestItem {Name = "aba"},
                new TestItem {Name = "abc"},
                new TestItem {Name = "xyz"}
            };
            var actual = await list.SimplifiedSearchAsync("abc", x => x.Name);
            Assert.Single(actual);
        }

        [Fact]
        public  async Task List_PassesList()
        {
            var list = TestData.Countries.ToList();
            var expected = list;
            var actual = await list.SimplifiedSearchAsync("", null);
            Assert.Same(expected, actual);
        }

        [Fact]
        public async Task Enumerable_PassesList()
        {
            var list = TestData.Countries.AsEnumerable();
            var expected = list;
            var actual = await list.SimplifiedSearchAsync("", null);
            // Enumerable version is enumerated and therefore is not "Same".
            AssertCollectionUtils.AssertCollectionContainsEqualIds(expected, actual);
        }

        [Fact]
        public async Task List_PassesSearchTerm()
        {
            var list = TestData.Countries.ToList();
            var expected = TestData.Countries.First(x => x.Name == "Albania");
            var actual = await list.SimplifiedSearchAsync("albania", x => x.Name);
            Assert.Single(actual, expected);
        }

        [Fact]
        public async Task Enumerable_PassesSearchTerm()
        {
            var list = TestData.Countries.AsEnumerable();
            var expected = TestData.Countries.First(x => x.Name == "Albania");
            var actual = await list.SimplifiedSearchAsync("albania", x => x.Name);
            Assert.Single(actual, expected);
        }

        [Fact]
        public async Task List_PassesProperty()
        {
            var list = new[]
{
                new TestItem { Id = 1, Name = "a" },
                new TestItem { Id = 2, Name = "1" },
                new TestItem { Id = 3, Name = "z"}
            };

            var expected = list[0];

            var actual = await list.SimplifiedSearchAsync("1", x => x.Id.ToString());

            Assert.Single(actual, expected);
        }

        [Fact]
        public async Task Enumerable_PassesProperty()
        {
            var listTmp = new[]
{
                new TestItem { Id = 1, Name = "a" },
                new TestItem { Id = 2, Name = "1" },
                new TestItem { Id = 3, Name = "z"}
            };

            var expected = listTmp[0];

            var list = listTmp.AsEnumerable();

            var actual = await list.SimplifiedSearchAsync("1", x => x.Id.ToString());

            Assert.Single(actual, expected);
        }

        [Fact]
        public async Task List_PropertyToSearchOptional()
        {
            var actual = await TestData.CountriesString.SimplifiedSearchAsync("Morocco");

            Assert.Single(actual, "Morocco");
        }

        [Fact]
        public async Task Enumerable_PropertyToSearchOptional()
        {
            var enumerableToSearch = TestData.CountriesString.AsEnumerable();

            var actual = await enumerableToSearch.SimplifiedSearchAsync("Morocco");

            Assert.Single(actual, "Morocco");
        }
    }
}
