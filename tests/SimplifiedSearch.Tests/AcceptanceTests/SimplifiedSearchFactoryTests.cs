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
        public SimplifiedSearchFactoryTests()
        {
            SimplifiedSearchFactory.Instance.ResetToDefault();
        }

        [Fact]
        public async Task Default_NewFactory_CanBeUsed()
        {
            var factory = new SimplifiedSearchFactory();
            var simplifiedSearch = factory.Create();
            var list = new[]
            {
                new TestItem {Name = "abc"},
                new TestItem {Name = "xyz"}
            };
            var actual = await simplifiedSearch.SimplifiedSearchAsync(list, "abc", x => x.Name);
            Assert.Single(actual);
        }

        [Fact]
        public async Task Default_Instance_CanBeUsed()
        {
            var simplifiedSearch = SimplifiedSearchFactory.Instance.Create();
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
            var simplifiedSearch = SimplifiedSearchFactory.Instance.Create("Default");
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
            SimplifiedSearchFactory.Instance.Add("SelectTop1", c => c.ResultSelector = new Configurations.ResultSelectorTop1());
            var simplifiedSearch = SimplifiedSearchFactory.Instance.Create("SelectTop1");
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
            SimplifiedSearchFactory.Instance.Add("SelectTop1", c => c.ResultSelector = new Configurations.ResultSelectorTop1());
            var simplifiedSearch = SimplifiedSearchFactory.Instance.Create("NonExistantName");
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
            SimplifiedSearchFactory.Instance.Add("DeFaUlT", c => c.ResultSelector = new Configurations.ResultSelectorTop1());
            var simplifiedSearch = SimplifiedSearchFactory.Instance.Create();
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
        public async Task Default_OverrideWithConfig_ExtensionMethod_CanBeUsed()
        {
            SimplifiedSearchFactory.Instance.Add("DeFaUlT", c => c.ResultSelector = new Configurations.ResultSelectorTop1());
            var list = new[]
            {
                new TestItem {Name = "aba"},
                new TestItem {Name = "abc"},
                new TestItem {Name = "xyz"}
            };
            var actual = await list.SimplifiedSearchAsync("abc", x => x.Name);
            Assert.Single(actual);
        }
    }
}
