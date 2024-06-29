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
        public void TokenizeOnSpace()
        {
            var actual = _tokenizeFilter.Run("a b");

            var expected = new[] { "a", "b" };
            AssertCollectionUtils.AssertCollectionContainsSameInSameOrder(expected, actual);
        }

        [Fact]
        public void TokenizeOnDash()
        {
            var actual = _tokenizeFilter.Run("a-b");

            var expected = new[] { "a", "b" };
            AssertCollectionUtils.AssertCollectionContainsSameInSameOrder(expected, actual);
        }

        [Fact]
        public void TokenizeOnCarriageReturn()
        {
            var actual = _tokenizeFilter.Run("a\rb");

            var expected = new[] { "a", "b" };
            AssertCollectionUtils.AssertCollectionContainsSameInSameOrder(expected, actual);
        }

        [Fact]
        public void TokenizeOnNewLine()
        {
            var actual = _tokenizeFilter.Run("a\nb");

            var expected = new[] { "a", "b" };
            AssertCollectionUtils.AssertCollectionContainsSameInSameOrder(expected, actual);
        }

        [Fact]
        public void TokenizeOnTab()
        {
            var actual = _tokenizeFilter.Run("a\tb");

            var expected = new[] { "a", "b" };
            AssertCollectionUtils.AssertCollectionContainsSameInSameOrder(expected, actual);
        }

        [Fact]
        public void TokenizeMultiSeparator()
        {
            var actual = _tokenizeFilter.Run("a    \r\n\t    b");

            var expected = new[] { "a", "b" };
            AssertCollectionUtils.AssertCollectionContainsSameInSameOrder(expected, actual);
        }
    }
}