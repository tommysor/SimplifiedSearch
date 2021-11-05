using SimplifiedSearch.Tests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimplifiedSearch.Tests.AcceptanceTests
{
    public class SimplifiedSearchReasonableResultTests
    {
        private readonly ISimplifiedSearch _search;

        public SimplifiedSearchReasonableResultTests()
        {
            var factory = new SimplifiedSearchFactory();
            _search = factory.GetSimplifiedSearch();
        }

        [Theory(Skip = "Not implemented Fuzzy search and Ranking")]
        [InlineData("tailand", "Thailand", "Taiwan", "placeholder value")]
        public async Task SimplifiedSearch_Countries_Top3(string search, string expect1, string expect2, string expect3)
        {
            var expected1 = TestData.Countries.First(x => x.Name == expect1);
            var expected2 = TestData.Countries.First(x => x.Name == expect2);
            var expected3 = TestData.Countries.First(x => x.Name == expect3);

            var actual = await _search.SimplifiedSearchAsync(TestData.Countries, search, x => x.Name);

            var actual1 = actual[0];
            var actual2 = actual[1];
            var actual3 = actual[2];

            Assert.Same(expected1, actual1);
            Assert.Same(expected2, actual2);
            Assert.Same(expected3, actual3);
        }

        [Theory]
        [InlineData("York", "New York")]
        [InlineData("Loui", "Louisiana")]
        public async Task SimplifiedSearch_UsStatesTopResult(string search, string expectTop)
        {
            var a = TestData.Countries;
            var expectedTop = TestData.UsStates.First(x => x.Name == expectTop);
            var actual = await _search.SimplifiedSearchAsync(TestData.UsStates, search, x => x.Name);

            Assert.Same(expectedTop, actual.First());
        }
    }
}
