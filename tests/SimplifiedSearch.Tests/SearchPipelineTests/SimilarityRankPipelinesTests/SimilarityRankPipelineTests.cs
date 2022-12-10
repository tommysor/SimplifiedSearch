using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimplifiedSearch.SearchPipelines;
using SimplifiedSearch.SearchPipelines.SimilarityRankPipelines;
using Xunit;

namespace SimplifiedSearch.Tests.SearchPipelineTests.SimilarityRankPipelinesTests
{
    public class SimilarityRankPipelineTests
    {
        [Fact]
        public void Ctor_WhenTokenPipelineIsNull_ShouldThrowException()
        {
            var action = () => new SimilarityRankPipeline(null!);
            Assert.Throws<ArgumentNullException>("tokenPipeline", action);
        }
    }
}
