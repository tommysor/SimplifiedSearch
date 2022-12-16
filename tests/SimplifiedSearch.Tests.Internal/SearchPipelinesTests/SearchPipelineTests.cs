using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimplifiedSearch.SearchPipelines;
using SimplifiedSearch.SearchPipelines.ResultSelectors;
using SimplifiedSearch.SearchPipelines.SimilarityRankPipelines;
using SimplifiedSearch.SearchPipelines.TokenPipelines;
using Xunit;

namespace SimplifiedSearch.Tests.Internal.SearchPipelineTests
{
    public class SearchPipelineTests
    {
        private readonly SearchPipeline _sut;
        private readonly IList<int> _emptyList = Array.Empty<int>();

        public SearchPipelineTests()
        {
            var tokenPipeline = new TokenPipeline();
            var similarityRankPipeline = new SimilarityRankPipeline(tokenPipeline);
            var resultSelector = new ResultSelector();
            _sut = new SearchPipeline(similarityRankPipeline, resultSelector);
        }

        [Fact]
        public void Ctor_WhenTokenPipelineIsNull_ShouldThowException()
        {
            var action = () => new SearchPipeline(null!, new ResultSelector());
            Assert.Throws<ArgumentNullException>("similarityRankPipeline", action);
        }

        [Fact]
        public async Task SearchAsync_WhenListIsNull_ShouldThowException()
        {
            var func = async () => await _sut.SearchAsync<int>(null!, "searchTerm", _ => "");
            await Assert.ThrowsAsync<ArgumentNullException>("list", func);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task SearchAsync_WhenSearchTermIsNullOrEmpty_ShouldThrowException(string? searchTermInput)
        {
            var func = async () => await _sut.SearchAsync(_emptyList, searchTermInput!, _ => "");
            await Assert.ThrowsAsync<ArgumentException>("searchTerm", func);
        }

        [Fact]
        public async Task SearchAsync_WhenFieldToSearchIsNull_ShouldThrowException()
        {
            var func = async () => await _sut.SearchAsync(_emptyList, "searchTerm", null!);
            await Assert.ThrowsAsync<ArgumentNullException>("fieldToSearch", func);
        }
    }
}