using System.Threading.Tasks;
using SimplifiedSearch.SearchPipelines;
using SimplifiedSearch.SearchPipelines.TokenPipelines.Components;
using SimplifiedSearch.Tests.Internal.Utils;
using Xunit;

namespace SimplifiedSearch.Tests.Internal.SearchPipelineTests.TokenPipelinesTests.ComponentsTests
{
    public class AsciiFoldingTests
    {
        private readonly AsciiFoldingFilter _asciiFoldingFilter;

        public AsciiFoldingTests()
        {
            _asciiFoldingFilter = new AsciiFoldingFilter();
        }

        [Fact]
        public async Task AsciiFoldingSimple()
        {
            var input = "ü";

            var actual = await _asciiFoldingFilter.RunAsync(input);

            Assert.Single(actual, "u");
        }

        [Fact]
        public async Task AsciiFoldingSimpleList()
        {
            var input = new[] { "â", "ß" };

            var actual = await _asciiFoldingFilter.RunAsync(input);

            var expected = new[] { "a", "ss" };
            AssertCollectionUtils.AssertCollectionContainsSameInSameOrder(expected, actual);
        }
    }
}