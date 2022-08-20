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
        [InlineData("Taiw", "Taiwan")]
        [InlineData("Alba", "Albania")]
        public async Task Countries_Top1(string search, string expectTop)
        {
            var expectedTop = TestData.Countries.First(x => x.Name == expectTop);
            var actual = await TestData.Countries.SimplifiedSearchAsync(search, x => x.Name);
            Assert.Same(expectedTop, actual.First());
        }

        [Fact]
        public async Task ShortSearch()
        {
            const string aaa = "aaa";
            const string baa = "baa";
            var list = new[] { aaa, baa, };

            var actual = await list.SimplifiedSearchAsync("ba");
            Assert.Collection(actual, 
                a => Assert.Equal(baa, a), 
                b => Assert.Equal(aaa, b));
        }

        [Fact]
        public async Task FirstCharSearch()
        {
            const string abc = "abc";
            const string bcd = "bcd";
            var list = new[] { abc, bcd, };

            var actual = await list.SimplifiedSearchAsync("b");
            Assert.Collection(actual, x => Assert.Equal(bcd, x));
        }

        [Fact]
        public async Task PrioritizeShorterExactBeforeLongerStartswith()
        {
            const string comment1 = "Duckberg";
            const string comment2 = "Duck";
            var list = new[] { comment1, comment2, };

            var actual = await list.SimplifiedSearchAsync("duck");
            Assert.Collection(actual,
                a => Assert.Equal(comment2, a),
                b => Assert.Equal(comment1, b));
        }
    }
}
