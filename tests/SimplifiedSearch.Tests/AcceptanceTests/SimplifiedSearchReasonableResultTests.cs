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

        [Theory]
        [InlineData("tailand", "Thailand", "Taiwan")]
        //todo Prefer closer to total match ? [InlineData("Guinea", "Equatorial Guinea", "Guinea")]
        [InlineData("Guinea", "Equatorial Guinea", "Guinea")]
        [InlineData("Niger", "Niger", "Nigeria")]
        [InlineData("Nigeria", "Nigeria", "Algeria")]
        public async Task SimplifiedSearch_Countries_Top2(string search, string expect1, string expect2)
        {
            var expected1 = TestData.Countries.First(x => x.Name == expect1);
            var expected2 = TestData.Countries.First(x => x.Name == expect2);

            var actual = await _search.SimplifiedSearchAsync(TestData.Countries, search, x => x.Name);

            var actual1 = actual[0];
            Assert.Same(expected1, actual1);

            Assert.True(actual.Count >= 2, "Did not get a second result.");
            var actual2 = actual[1];
            Assert.Same(expected2, actual2);
        }

        [Theory]
        [InlineData("York", "New York")]
        [InlineData("Loui", "Louisiana")]
        public async Task SimplifiedSearch_UsStates_Top1(string search, string expectTop)
        {
            var expectedTop = TestData.UsStates.First(x => x.Name == expectTop);

            var actual = await _search.SimplifiedSearchAsync(TestData.UsStates, search, x => x.Name);

            Assert.Same(expectedTop, actual.First());
        }

        [Theory]
        [InlineData("naruto ideas", "naruto and really original anime ideas like rakugo", "this naruto joke  link    http  imgurcomk8sjgwg")]
        [InlineData("joker favorite", "the  actual dialog  joke continues to be my favorite bit", "fanart corner    post your favorite anime related fanart")]
        [InlineData("main character", "in rewrite  the main character s fake name is suzuki bond", "it s supposed to be one of her cute character quirks")]
        public async Task SimplifiedSearch_ShortText_Top2(string search, string expect1, string expect2)
        {
            var actual = await _search.SimplifiedSearchAsync(TestData.RedditAnimeShortPosts, search);

            var actual1 = actual[0];
            Assert.Equal(expect1, actual1);

            Assert.True(actual.Count >= 2, "Did not get a second result.");
            var actual2 = actual[1];
            Assert.Equal(expect2, actual2);
        }

        [Theory]
        [InlineData("finally demand a season 3",
            "medaka box  so it gets enough viewers to finally demand a season 3 it ended literally as the best arc was about to begin",
            "spice and wolf then with the increased demand for the series we can finally get season 3")]
        [InlineData("potential plotprogression ideas",
            "_cross ange_  i felt like at every major plot intersection  they wrote six potential plotprogression ideas and then rolled a die to determine which one they d go with",
            "stands  jojo s bizarre adventure  are my favorite power there s so much potential for what they can do  and there are no signs of araki slowing down with the unique ideas")]
        public async Task SimplifiedSearch_LongText_Top2(string search, string expect1, string expect2)
        {
            var actual = await _search.SimplifiedSearchAsync(TestData.RedditAnimeLongPosts, search);

            var actual1 = actual[0];
            Assert.Equal(expect1, actual1);

            Assert.True(actual.Count >= 2, "Did not get a second result.");
            var actual2 = actual[1];
            Assert.Equal(expect2, actual2);
        }
    }
}
