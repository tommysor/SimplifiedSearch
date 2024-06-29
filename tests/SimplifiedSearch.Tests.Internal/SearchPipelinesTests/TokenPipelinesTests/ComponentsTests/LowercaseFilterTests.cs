using System.Threading.Tasks;
using SimplifiedSearch.SearchPipelines;
using SimplifiedSearch.SearchPipelines.TokenPipelines.Components;
using SimplifiedSearch.Tests.Internal.Utils;
using Xunit;

namespace SimplifiedSearch.Tests.Internal.SearchPipelineTests.TokenPipelinesTests.ComponentsTests
{
    public class LowercaseFilterTests
    {
        private readonly LowercaseFilter _lowercaseFilter;

        public LowercaseFilterTests()
        {
            _lowercaseFilter = new LowercaseFilter();
        }

        [Fact]
        public void LowercaseFilterSimple()
        {
            var actual = _lowercaseFilter.Run("A");

            Assert.Single(actual, "a");
        }

        [Fact]
        public void LowercaseFilterSimpleList()
        {
            var actual = _lowercaseFilter.Run("aAa", "BBB");

            var expected = new[] { "aaa", "bbb" };
            AssertCollectionUtils.AssertCollectionContainsSameInSameOrder(expected, actual);
        }
    }
}