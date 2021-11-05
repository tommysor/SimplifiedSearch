using Bogus;
using SimplifiedSearch.Tests.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SimplifiedSearch.Tests
{
    public class SimplifiedSearchSmokePerformanceTests
    {
        private readonly ISimplifiedSearch _search;
        private readonly IList<TestItem> _persons;

        public SimplifiedSearchSmokePerformanceTests()
        {
            var factory = new SimplifiedSearchFactory();
            _search = factory.GetSimplifiedSearch();
            _persons = new Faker<TestItem>()
                .RuleFor(x => x.Id, x => x.IndexFaker)
                .RuleFor(x => x.Name, x => x.Person.FirstName)
                .Generate(100000);
        }

        [Fact(Timeout = 100, Skip = "TODO Benchmark?")]
        public async Task SimplifiedSearch_Perf01()
        {
            var sw = new Stopwatch();
            sw.Start();
            using var cancellationTokenSource = new CancellationTokenSource(100);
            var cancellationToken = cancellationTokenSource.Token;

            await _search.SimplifiedSearchAsync(_persons, "a", null);
            
            sw.Stop();
            var xxx = sw.ElapsedMilliseconds;
        }
    }
}
