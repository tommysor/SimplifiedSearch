using System.Threading.Tasks;
using SimplifiedSearch.SearchPipelines;
using SimplifiedSearch.SearchPipelines.TokenPipelines.Components;
using SimplifiedSearch.Tests.Internal.Utils;
using Xunit;

namespace SimplifiedSearch.Tests.Internal.SearchPipelineTests.TokenPipelinesTests.ComponentsTests
{
    public class TokenizeFilterTests
    {
        private readonly TokenizeFilter _tokenizeFilter;

        public TokenizeFilterTests()
        {
            _tokenizeFilter = new TokenizeFilter();
        }

        [Fact]
        public async Task TokenizeOnSpace()
        {
            var actual = await _tokenizeFilter.RunAsync("a b");

            var expected = new[] { "a", "b" };
            AssertCollectionUtils.AssertCollectionContainsSameInSameOrder(expected, actual);
        }

        [Fact]
        public async Task TokenizeOnDash()
        {
            var actual = await _tokenizeFilter.RunAsync("a-b");

            var expected = new[] { "a", "b" };
            AssertCollectionUtils.AssertCollectionContainsSameInSameOrder(expected, actual);
        }

        [Fact]
        public async Task TokenizeOnCarriageReturn()
        {
            var actual = await _tokenizeFilter.RunAsync("a\rb");

            var expected = new[] { "a", "b" };
            AssertCollectionUtils.AssertCollectionContainsSameInSameOrder(expected, actual);
        }

        [Fact]
        public async Task TokenizeOnNewLine()
        {
            var actual = await _tokenizeFilter.RunAsync("a\nb");

            var expected = new[] { "a", "b" };
            AssertCollectionUtils.AssertCollectionContainsSameInSameOrder(expected, actual);
        }

        [Fact]
        public async Task TokenizeOnTab()
        {
            var actual = await _tokenizeFilter.RunAsync("a\tb");

            var expected = new[] { "a", "b" };
            AssertCollectionUtils.AssertCollectionContainsSameInSameOrder(expected, actual);
        }

        [Fact]
        public async Task TokenizeMultiSeparator()
        {
            var actual = await _tokenizeFilter.RunAsync("a    \r\n\t    b");

            var expected = new[] { "a", "b" };
            AssertCollectionUtils.AssertCollectionContainsSameInSameOrder(expected, actual);
        }
    }
}