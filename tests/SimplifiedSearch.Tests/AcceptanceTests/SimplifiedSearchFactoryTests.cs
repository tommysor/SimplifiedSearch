using SimplifiedSearch.Tests.Models;
using SimplifiedSearch.Tests.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimplifiedSearch.Tests
{
    public class SimplifiedSearchFactoryTests
    {
        private readonly SimplifiedSearchFactory _sut;

        public SimplifiedSearchFactoryTests()
        {
            _sut = new SimplifiedSearchFactory();
        }

        [Fact]
        public async Task Default_NewFactory_CanBeUsed()
        {
            var simplifiedSearch = _sut.Create();
            var list = new[]
            {
                new TestItem {Name = "abc"},
                new TestItem {Name = "xyz"}
            };
            var actual = await simplifiedSearch.SimplifiedSearchAsync(list, "abc", x => x.Name);
            Assert.Single(actual);
        }

        [Fact]
        public async Task Default_WithParam_CanBeUsed()
        {
            var simplifiedSearch = _sut.Create("Default");
            var list = new[]
            {
                new TestItem {Name = "abc"},
                new TestItem {Name = "xyz"}
            };
            var actual = await simplifiedSearch.SimplifiedSearchAsync(list, "abc", x => x.Name);
            Assert.Single(actual);
        }

        [Fact]
        public async Task ResultSelectorTop1_WithParam_CanBeUsed()
        {
            _sut.Add("SelectTop1", c => c.ResultSelector = new Configurations.ResultSelectorTop1());
            var simplifiedSearch = _sut.Create("SelectTop1");
            var list = new[]
            {
                new TestItem {Name = "aba"},
                new TestItem {Name = "abc"},
                new TestItem {Name = "xyz"}
            };
            var actual = await simplifiedSearch.SimplifiedSearchAsync(list, "abc", x => x.Name);
            Assert.Single(actual);
        }

        [Fact]
        public async Task Default_GetWithNonExistantName_FallsBackToDefault()
        {
            _sut.Add("SelectTop1", c => c.ResultSelector = new Configurations.ResultSelectorTop1());
            var simplifiedSearch = _sut.Create("NonExistantName");
            var list = new[]
            {
                new TestItem {Name = "aba"},
                new TestItem {Name = "abc"},
                new TestItem {Name = "xyz"}
            };
            var actual = await simplifiedSearch.SimplifiedSearchAsync(list, "abc", x => x.Name);
            const int expectedLength = 2;
            Assert.True(expectedLength == actual.Count, $"Length Expected: {expectedLength}, Actual: {actual.Count}");
        }

        [Fact]
        public async Task Default_OverrideWithConfig_FromFactory_CanBeUsed()
        {
            _sut.Add("default", c => c.ResultSelector = new Configurations.ResultSelectorTop1());
            var simplifiedSearch = _sut.Create();
            var list = new[]
            {
                new TestItem {Name = "aba"},
                new TestItem {Name = "abc"},
                new TestItem {Name = "xyz"}
            };
            var actual = await simplifiedSearch.SimplifiedSearchAsync(list, "abc", x => x.Name);
            Assert.Single(actual);
        }

        [Theory]
        [InlineData("default")]
        [InlineData("DEFAULT")]
        [InlineData("Default")]
        [InlineData("DeFaUlT")]
        public async Task OverrideDefault_IsCaseInsensitive(string defaultName)
        {
            _sut.Add(defaultName, c => c.ResultSelector = new Configurations.ResultSelectorTop1());
            var simplifiedSearch = _sut.Create();
            var list = new[]
            {
                new TestItem {Name = "aba"},
                new TestItem {Name = "abc"},
                new TestItem {Name = "xyz"}
            };
            var actual = await simplifiedSearch.SimplifiedSearchAsync(list, "abc", x => x.Name);
            Assert.Single(actual);
        }

        [Theory]
        [InlineData("name")]
        [InlineData("NAME")]
        [InlineData("Name")]
        [InlineData("NaMe")]
        public async Task Create_Name_IsCaseInsensitive(string name)
        {
            _sut.Add("name", c => c.ResultSelector = new Configurations.ResultSelectorTop1());
            var simplifiedSearch = _sut.Create(name);
            var list = new[]
            {
                new TestItem {Name = "aba"},
                new TestItem {Name = "abc"},
                new TestItem {Name = "xyz"}
            };
            var actual = await simplifiedSearch.SimplifiedSearchAsync(list, "abc", x => x.Name);
            Assert.Single(actual);
        }
    }
}
