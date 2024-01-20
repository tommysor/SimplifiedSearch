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
        private readonly ISimplifiedSearch _sut;

        public ReasonableResultTests()
        {
            _sut = new SimplifiedSearchFactory().Create();
        }

        [Theory]
        [InlineData("thaiwan", "Taiwan", "Thailand")]
        [InlineData("Niger", "Niger", "Nigeria")]
        public async Task Countries_Top2(string search, string expect1, string expect2)
        {
            var expected1 = TestData.Countries.First(x => x.Name == expect1);
            var expected2 = TestData.Countries.First(x => x.Name == expect2);

            var actual = await _sut.SimplifiedSearchAsync(TestData.Countries, search, x => x.Name);

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
            var actual = await _sut.SimplifiedSearchAsync(TestData.Countries, search, x => x.Name);
            Assert.Same(expectedTop, actual.First());
        }

        [Fact]
        public async Task ShortSearch()
        {
            const string aaa = "aaa";
            const string baa = "baa";
            var list = new[] { aaa, baa, };

            var actual = await _sut.SimplifiedSearchAsync(list, "ba");
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

            var actual = await _sut.SimplifiedSearchAsync(list, "b");
            Assert.Collection(actual, x => Assert.Equal(bcd, x));
        }

        [Fact]
        public async Task PrioritizeShorterExactBeforeLongerStartswith()
        {
            const string comment1 = "Duckberg";
            const string comment2 = "Duck";
            var list = new[] { comment1, comment2, };

            var actual = await _sut.SimplifiedSearchAsync(list, "duck");
            Assert.Collection(actual,
                a => Assert.Equal(comment2, a),
                b => Assert.Equal(comment1, b));
        }

        [Fact]
        public async Task MatchShort_PrioritizedOverEqualMatchOnLonger()
        {
            string[] list = [
                "Emmettshire",
                "Emma",
            ];

            var actual = await _sut.SimplifiedSearchAsync(list, "emm");
            Assert.Collection(actual,
                a => Assert.Equal("Emma", a),
                b => Assert.Equal("Emmettshire", b)
            );
        }

        [Fact]
        public async Task PrioritizeExactMatchInStartOfWord()
        {
            const string n30p = "30%";
            const string n30 = "30";
            const string n50p = "50%";
            const string n50 = "50";

            var list = new[] { n30p, n30, n50p, n50 };
            var actual = await _sut.SimplifiedSearchAsync(list, "50");
            Assert.Equal(n50, actual[0]);
            Assert.Equal(n50p, actual[1]);
        }

        [Fact]
        public async Task PrioritizeExactMatchInStartOfWordIgnoreCapitalization()
        {
            const string n30p = "a0z";
            const string n30 = "a0";
            const string n50p = "b0z";
            const string n50 = "B0";

            var list = new[] { n30p, n30, n50p, n50 };
            var actual = await _sut.SimplifiedSearchAsync(list, "b0");
            Assert.Equal(n50, actual[0]);
            Assert.Equal(n50p, actual[1]);
        }

        [Fact]
        public async Task PrioritizeBetterStartOfWord_WhenSearchTermIsMissingChar()
        {
            string[] list = [
                "abcghi",
                "abcdef",
            ];

            var actual = await _sut.SimplifiedSearchAsync(list, "abd");
            Assert.Equal("abcdef", actual[0]);
            Assert.Equal("abcghi", actual[1]);
        }
    }
}
