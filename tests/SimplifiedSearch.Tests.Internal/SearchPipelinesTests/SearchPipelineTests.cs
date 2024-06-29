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
        public void SearchAsync_WhenListIsNull_ShouldThowException()
        {
            var func = () => _sut.Search<int>(null!, "searchTerm", _ => "");
            Assert.Throws<ArgumentNullException>("list", func);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void SearchAsync_WhenSearchTermIsNullOrEmpty_ShouldThrowException(string? searchTermInput)
        {
            var func = () => _sut.Search(_emptyList, searchTermInput!, _ => "");
            Assert.Throws<ArgumentException>("searchTerm", func);
        }

        [Fact]
        public void SearchAsync_WhenFieldToSearchIsNull_ShouldThrowException()
        {
            var func = () => _sut.Search(_emptyList, "searchTerm", null!);
            Assert.Throws<ArgumentNullException>("fieldToSearch", func);
        }
    }
}