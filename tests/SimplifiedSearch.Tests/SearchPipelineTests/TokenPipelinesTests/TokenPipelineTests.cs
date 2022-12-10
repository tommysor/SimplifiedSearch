using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimplifiedSearch.SearchPipelines;
using SimplifiedSearch.SearchPipelines.TokenPipelines;
using SimplifiedSearch.SearchPipelines.TokenPipelines.Components;
using Xunit;

namespace SimplifiedSearch.Tests.SearchPipelineTests.TokenPipelinesTests
{
    public class TokenPipelineTests
    {
        private readonly TokenPipeline _sut;

        public TokenPipelineTests()
        {
            var lowercaseFilter = new LowercaseFilter();
            _sut = new TokenPipeline(lowercaseFilter);
        }

        [Fact]
        public async Task RunAsync_ShouldRunComponents()
        {
            var actual = await _sut.RunAsync("AbCd");
            Assert.Equal("abcd", actual[0]);
        }
    }
}
