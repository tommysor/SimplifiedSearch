using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimplifiedSearch.SearchPipelines;
using SimplifiedSearch.SearchPipelines.SimilarityRankPipelines;
using Xunit;

namespace SimplifiedSearch.Tests.SearchPipelineTests.SimilarityRankPipelinesTests
{
    public class SimilarityRankItemTests
    {
        [Fact]
        public void Ctor_WhenItemIsNull_ShouldThrowException()
        {
            var action = () => new SimilarityRankItem<object>(null!);
            Assert.Throws<ArgumentNullException>("item", action);
        }

        [Fact]
        public void Ctor_WhenItemIsSet_ShouldSameObjectBeAvailable()
        {
            var obj = new object();
            var sut = new SimilarityRankItem<object>(obj);
            Assert.Same(obj, sut.Item);
        }
    }
}
