using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimplifiedSearch.SearchPipelines;
using Xunit;

namespace SimplifiedSearch.Tests.SearchPipelineTests
{
    public class SearchPipelineTests
    {
        private readonly SearchPipeline _sut;
        private readonly IList<int> _emptyList = Array.Empty<int>();

        public SearchPipelineTests()
        {
            var tokenPipeline = new TokenPipeline();
            _sut = new SearchPipeline(tokenPipeline);
        }

        [Fact]
        public void Ctor_WhenTokenPipelineIsNull_ShouldThowException()
        {
            var action = () => new SearchPipeline(null!);
            Assert.Throws<ArgumentNullException>("tokenPipeline", action);
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
        public async Task SearchAsync_WhenSearchTermIsNullOrEmpty_ShouldThrowException(string? searchTerm)
        {
            var func = async () => await _sut.SearchAsync(_emptyList, searchTerm!, _ => "");
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