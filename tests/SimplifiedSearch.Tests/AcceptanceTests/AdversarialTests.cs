using Bogus;
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
        private readonly Randomizer _randomizer;
        private readonly string _manyWords;
        private readonly string _longWord;
        private readonly List<string> _listOfManyWords;
        private readonly IReadOnlyList<string> _listOfNaughtyStrings;

        public AdversarialTests()
        {
            _randomizer = new Randomizer();
            _manyWords = _randomizer.Words(1000);
            _longWord = _randomizer.String2(50000);

            _listOfManyWords = new List<string>(20000);
            for (var i = 0; i < 20000; i++)
                _listOfManyWords.Add(_manyWords);

            _listOfNaughtyStrings = TheNaughtyStrings.All;
        }

        [Fact]
        public async Task ManyWordsInList()
        {
            var sw = new Stopwatch();
            sw.Start();
            var _ = await _listOfManyWords.SimplifiedSearchAsync("abcde");
            sw.Stop();

            var timeLimit = TimeSpan.FromSeconds(4);
            Assert.True(sw.Elapsed < timeLimit, $"Search took too long. Elapsed: {sw.Elapsed}");
        }

        [Fact]
        public async Task ManyWordsInListAndAFewInSearchTerm()
        {
            var searchTerm = _randomizer.Words(5);

            var sw = new Stopwatch();
            sw.Start();
            var _ = await _listOfManyWords.SimplifiedSearchAsync(searchTerm);
            sw.Stop();

            var timeLimit = TimeSpan.FromSeconds(20);
            Assert.True(sw.Elapsed < timeLimit, $"Search took too long. Elapsed: {sw.Elapsed}");
        }

        [Fact]
        public async Task ManyWordsInSearchTerm()
        {
            var word = _randomizer.Word();
            var list = new List<string>();
            for (var i = 0; i < 20000; i++)
                list.Add(word);

            var searchTerm = _manyWords;

            var sw = new Stopwatch();
            sw.Start();
            var _ = await list.SimplifiedSearchAsync(searchTerm);
            sw.Stop();

            var timeLimit = TimeSpan.FromSeconds(12);
            Assert.True(sw.Elapsed < timeLimit, $"Search took too long. Elapsed: {sw.Elapsed}");
        }

        [Fact]
        public async Task VeryLongWordInList()
        {
            var list = new List<string>();
            for (var i = 0; i < 20000; i++)
                list.Add(_longWord);

            var sw = new Stopwatch();
            sw.Start();
            var _ = await list.SimplifiedSearchAsync("abcde");
            sw.Stop();

            var timeLimit = TimeSpan.FromSeconds(1);
            Assert.True(sw.Elapsed < timeLimit, $"Search took too long: Elapsed: {sw.Elapsed}");
        }

        [Fact]
        public async Task VeryLongWordInSearchString()
        {
            var list = new List<string>();
            for (var i = 0; i < 20000; i++)
                list.Add("abcde");

            var searchTerm = _longWord;

            var sw = new Stopwatch();
            sw.Start();
            var _ = await list.SimplifiedSearchAsync(searchTerm);
            sw.Stop();

            var timeLimit = TimeSpan.FromSeconds(20);
            Assert.True(sw.Elapsed < timeLimit, $"Search took too long: Elapsed: {sw.Elapsed}");
        }

        [Fact]
        public async Task SearchListOfNaughtyStrings()
        {
            var _ = await _listOfNaughtyStrings.SimplifiedSearchAsync("abcde");
        }

        [Fact]
        public async Task SearchForNaughtyString()
        {
            var list = new[]
            {
                "abcde"
            };

            foreach(var naughtyString in _listOfNaughtyStrings)
            {
                var _ = await list.SimplifiedSearchAsync(naughtyString);
            }
        }

    }
}
