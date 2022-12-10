using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimplifiedSearch.SearchPipelines;
using SimplifiedSearch.SearchPipelines.SimilarityRankPipelines;
using SimplifiedSearch.SearchPipelines.SimilarityRankPipelines.Components;
using Xunit;

namespace SimplifiedSearch.Tests.SearchPipelineTests.SimilarityRankPipelinesTests.Components
{
    public class MainSimilarityRankerTests
    {
        private readonly MainSimilarityRanker _sut = new();

        [Fact]
        public async void RunAsync_WhenValuesMatches_ShouldGiveSomeValue()
        {
            var fieldValues = new [] { "aaaa" };
            var searchTerm = new [] { "aaaa" };
            var actual = await _sut.RunAsync(fieldValues, searchTerm);
            Assert.True(actual > 0, $"SimilarityRank was not greater than zero. Got: {actual}");
        }

        [Fact]
        public async void RunAsync_WhenFirstCharDoesNotMatch_ShouldGiveZero()
        {
            var fieldValues = new [] { "abcd" };
            var searchTerm = new [] { "zyxw" };
            var actual = await _sut.RunAsync(fieldValues, searchTerm);
            Assert.True(actual == 0, $"SimilarityRank was not zero. Got: {actual}");
        }
    }
}
