using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimplifiedSearch.SearchPipelines;
using SimplifiedSearch.SearchPipelines.SimilarityRankPipelines;
using SimplifiedSearch.SearchPipelines.SimilarityRankPipelines.Components;
using Xunit;

namespace SimplifiedSearch.Tests.Internal.SearchPipelineTests.SimilarityRankPipelinesTests.Components
{
    public class ExactEqualStartBoostTests
    {
        private readonly ExactEqualStartBoost _sut = new();

        [Fact]
        public void RunAsync_WhenOnlyFirstCharMatches_ShouldGiveSomeValue()
        {
            var fieldValues = new [] { "abbb" };
            var searchTerm = new [] { "accc" };
            var actual = _sut.Run(fieldValues, searchTerm);
            Assert.True(actual > 0, $"SimilarityRank was not greater than zero. Got: {actual}");
        }

        [Fact]
        public void RunAsync_WhenFirstCharDoesNotMatch_ShouldGiveZero()
        {
            var fieldValues = new [] { "azzz" };
            var searchTerm = new [] { "bzzz" };
            var actual = _sut.Run(fieldValues, searchTerm);
            Assert.True(actual == 0, $"SimilarityRank was not zero. Got: {actual}");
        }
    }
}
