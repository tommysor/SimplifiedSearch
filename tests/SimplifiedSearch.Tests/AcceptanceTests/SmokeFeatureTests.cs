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
        public SmokeFeatureTests()
        {
            SimplifiedSearchFactory.Instance.ResetToDefault();
        }
        
        [Fact]
        public async Task MatchWholeWordExcact()
        {
            var expected = TestData.Countries.First(x => x.Name == "Taiwan");
            var actual = await TestData.Countries.SimplifiedSearchAsync("Taiwan", x => x.Name);
            Assert.Same(expected, actual.First());
        }

        [Fact]
        public async Task MatchWholeWordExactCaseInsensitive()
        {
            var expected = TestData.Countries.First(x => x.Name == "Thailand");
            var actual = await TestData.Countries.SimplifiedSearchAsync("tHAILAnd", x => x.Name);
            Assert.Same(expected, actual.First());
        }

        [Fact]
        public async Task MatchStartOfWordExact()
        {
            var expected = TestData.Countries.First(x => x.Name == "Albania");
            var actual = await TestData.Countries.SimplifiedSearchAsync("Alba", x => x.Name);
            Assert.Same(expected, actual.First());
        }

        [Fact]
        public async Task MatchWholeWordFuzzy()
        {
            var expected = TestData.Countries.First(x => x.Name == "Morocco");
            var actual = await TestData.Countries.SimplifiedSearchAsync("MZrocZo", x => x.Name);
            Assert.Same(expected, actual.First());
        }

        [Fact]
        public async Task MatchStartOfWordFuzzy()
        {
            var expected = TestData.Countries.First(x => x.Name == "Morocco");
            var actual = await TestData.Countries.SimplifiedSearchAsync("Zoro", x => x.Name);
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

            var actual = await list.SimplifiedSearchAsync(ascii);

            Assert.Single(actual);
        }
    }
}
