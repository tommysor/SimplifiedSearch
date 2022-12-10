using System.Threading.Tasks;
using SimplifiedSearch.SearchPipelines;
using SimplifiedSearch.SearchPipelines.TokenPipelines.Components;
using SimplifiedSearch.Tests.Utils;
using Xunit;

namespace SimplifiedSearch.Tests.SearchPipelineTests.TokenPipelinesTests.ComponentsTests
{
    public class LowercaseFilterTests
    {
        private readonly LowercaseFilter _lowercaseFilter;

        public LowercaseFilterTests()
        {
            _lowercaseFilter = new LowercaseFilter();
        }

        [Fact]
        public async Task LowercaseFilterSimple()
        {
            var actual = await _lowercaseFilter.RunAsync("A");

            Assert.Single(actual, "a");
        }

        [Fact]
        public async Task LowercaseFilterSimpleList()
        {
            var actual = await _lowercaseFilter.RunAsync("aAa", "BBB");

            var expected = new[] { "aaa", "bbb" };
            AssertCollectionUtils.AssertCollectionContainsSameInSameOrder(expected, actual);
        }
    }
}