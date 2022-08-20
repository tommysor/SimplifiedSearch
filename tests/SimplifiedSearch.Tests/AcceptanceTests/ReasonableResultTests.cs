using SimplifiedSearch.Tests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimplifiedSearch.Tests.AcceptanceTests
{
    public class ReasonableResultTests
    {
        [Theory]
        [InlineData("thaiwan", "Taiwan", "Thailand")]
        [InlineData("Niger", "Niger", "Nigeria")]
        public async Task Countries_Top2(string search, string expect1, string expect2)
        {
            var expected1 = TestData.Countries.First(x => x.Name == expect1);
            var expected2 = TestData.Countries.First(x => x.Name == expect2);

            var actual = await TestData.Countries.SimplifiedSearchAsync(search, x => x.Name);

            var actual1 = actual[0];
            Assert.Same(expected1, actual1);

            Assert.True(actual.Count >= 2, "Did not get a second result.");
            var actual2 = actual[1];
            Assert.Same(expected2, actual2);
        }

        [Theory]
        [InlineData("York", "New York")]
        [InlineData("Loui", "Louisiana")]
        public async Task UsStates_Top1(string search, string expectTop)
        {
            var expectedTop = TestData.UsStates.First(x => x.Name == expectTop);

            var actual = await TestData.UsStates.SimplifiedSearchAsync(search, x => x.Name);

            Assert.Same(expectedTop, actual.First());
        }
    }
}
