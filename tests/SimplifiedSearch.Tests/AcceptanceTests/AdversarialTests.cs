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

        [Fact]
        public async Task SearchWhereFieldValueIsNull()
        {
            var list = new [] { "Abc", null };

            var actual = await list.SimplifiedSearchAsync("abc");

            Assert.Single(actual);
        }

        [Fact]
        public async Task SearchWhereFieldValuePropertyToSearchLambdaReturnsNull()
        {
            var list = new [] { "Abc", null };

            var actual = await list.SimplifiedSearchAsync("abc", x => x);

            Assert.Single(actual);
        }

        [Fact]
        public async Task SearchWhereObjectIsNull()
        {
            var list = new [] 
            {
                new 
                {
                    Abc = "abc"
                },
                null
            };

            var actual = await list.SimplifiedSearchAsync("abc");

            Assert.Single(actual);
        }

        [Fact]
        public async Task SearchWhereObjectIsNullWhenUsingFieldToSearchLambda()
        {
            var list = new [] 
            {
                new 
                {
                    Abc = "abc"
                },
                null
            };

            var actual = await list.SimplifiedSearchAsync("abc", x => x!.Abc);

            Assert.Single(actual);
        }

        [Fact]
        public async Task SearchWhereFieldValueIsEmpty()
        {
            var list = new [] { "Abc", "" };

            var actual = await list.SimplifiedSearchAsync("abc");

            Assert.Single(actual);
        }

        [Fact]
        public async Task SearchWhereFieldValuePropertyToSearchLambdaReturnsEmpty()
        {
            var list = new [] { "Abc", "" };

            var actual = await list.SimplifiedSearchAsync("abc", x => x);

            Assert.Single(actual);
        }
    }
}
