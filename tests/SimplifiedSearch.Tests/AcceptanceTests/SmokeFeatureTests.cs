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
        [Fact]
        public async Task MatchWholeWordExcact()
        {
            var expected = TestData.UsStates.First(x => x.Name == "California");

            var actual = await TestData.UsStates.SimplifiedSearchAsync("California", x => x.Name);

            Assert.Same(expected, actual.First());
        }

        [Fact]
        public async Task MatchWholeWordExactCaseInsensitive()
        {
            var expected = TestData.UsStates.First(x => x.Name == "Arkansas");

            var actual = await TestData.UsStates.SimplifiedSearchAsync("arkANsas", x => x.Name);

            Assert.Same(expected, actual.First());
        }

        [Fact]
        public async Task MatchStartOfWordExact()
        {
            var expected = TestData.UsStates.First(x => x.Name == "Maryland");

            var actual = await TestData.UsStates.SimplifiedSearchAsync("Maryla", x => x.Name);

            Assert.Same(expected, actual.First());
        }

        [Fact]
        public async Task MatchWholeWordFuzzy()
        {
            var expected = TestData.UsStates.First(x => x.Name == "Montana");

            //                                                                   Montana
            var actual = await TestData.UsStates.SimplifiedSearchAsync("MoZtanZ", x => x.Name);

            Assert.Same(expected, actual.First());
        }

        [Fact]
        public async Task MatchStartOfWordFuzzy()
        {
            var expected = TestData.UsStates.First(x => x.Name == "Hawaii");

            //                                                                   Hawaii
            var actual = await TestData.UsStates.SimplifiedSearchAsync("hZwa", x => x.Name);

            Assert.Same(expected, actual.First());
        }
    }
}
