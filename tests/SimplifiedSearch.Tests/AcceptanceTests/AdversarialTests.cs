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
            var _ = await _listOfNaughtyStrings.SimplifiedSearchAsync(new string('a', 500));
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
                var _ = await list.SimplifiedSearchAsync(naughtyString);
            }
        }

    }
}
