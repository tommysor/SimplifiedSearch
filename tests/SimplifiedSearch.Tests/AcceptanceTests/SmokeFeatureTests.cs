using SimplifiedSearch.Tests.Models;
using SimplifiedSearch.Tests.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimplifiedSearch.Tests.AcceptanceTests
{
    /// <summary>
    /// Try each feature one at a time from top level.
    /// </summary>
    public class SmokeFeatureTests
    {
        private readonly ISimplifiedSearch _sut;
        
        public SmokeFeatureTests()
        {
            _sut = new SimplifiedSearchFactory().Create();
        }
        
        [Fact]
        public async Task MatchWholeWordExcact()
        {
            var expected = TestData.Countries.First(x => x.Name == "Taiwan");
            var actual = await _sut.SimplifiedSearchAsync(TestData.Countries, "Taiwan", x => x.Name);
            Assert.Same(expected, actual.First());
        }

        [Fact]
        public async Task MatchWholeWordExactCaseInsensitive()
        {
            var expected = TestData.Countries.First(x => x.Name == "Thailand");
            var actual = await _sut.SimplifiedSearchAsync(TestData.Countries, "tHAILAnd", x => x.Name);
            Assert.Same(expected, actual.First());
        }

        [Fact]
        public async Task MatchStartOfWordExact()
        {
            var expected = TestData.Countries.First(x => x.Name == "Albania");
            var actual = await _sut.SimplifiedSearchAsync(TestData.Countries, "Alba", x => x.Name);
            Assert.Same(expected, actual.First());
        }

        [Fact]
        public async Task MatchWholeWordFuzzy()
        {
            var expected = TestData.Countries.First(x => x.Name == "Morocco");
            var actual = await _sut.SimplifiedSearchAsync(TestData.Countries, "MZrocZo", x => x.Name);
            Assert.Same(expected, actual.First());
        }

        [Fact]
        public async Task MatchStartOfWordFuzzy()
        {
            var expected = TestData.Countries.First(x => x.Name == "Morocco");
            var actual = await _sut.SimplifiedSearchAsync(TestData.Countries, "Zoro", x => x.Name);
            Assert.Same(expected, actual.First());
        }

        [Fact]
        public async Task AsciiFoldingMatch()
        {
            var list = new[]
            {
                "cørèbréächñìñjâ"
            };

            var ascii = "corebreachninja";

            var actual = await _sut.SimplifiedSearchAsync(list, ascii);

            Assert.Single(actual);
        }
    }
}
