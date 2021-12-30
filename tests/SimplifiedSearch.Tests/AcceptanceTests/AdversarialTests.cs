using NaughtyStrings.Bogus;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimplifiedSearch.Tests.AcceptanceTests
{
    public class AdversarialTests
    {
        private readonly IReadOnlyList<string> _listOfNaughtyStrings;

        public AdversarialTests()
        {
            _listOfNaughtyStrings = TheNaughtyStrings.All;
        }

        [Fact]
        public async Task SearchListOfNaughtyStrings()
        {
            var actual = await _listOfNaughtyStrings.SimplifiedSearchAsync(new string('a', 500));
            // Assert is mostly to keep SonarCloud happy.
            // This test checks if anything blows up with weird values in list.
            Assert.NotNull(actual);
        }

        [Fact]
        public async Task SearchForNaughtyString()
        {
            var list = new[]
            {
                "abcd"
            };

            foreach(var naughtyString in _listOfNaughtyStrings)
            {
                var actual = await list.SimplifiedSearchAsync(naughtyString);
                // Assert is mostly to keep SonarCloud happy.
                // This test checks if anything blows up with weird values in search term.
                Assert.NotNull(actual);
            }
        }

    }
}
