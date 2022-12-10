using SimplifiedSearch.Tests.Models;
using SimplifiedSearch.Tests.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimplifiedSearch.Tests
{
    public class SimplifiedSearchFactoryTests
    {
        [Fact]
        public async Task NoExplicitSettings_CanBeUsed()
        {
            var factory = new SimplifiedSearchFactory();
            var simplifiedSearch = factory.GetSimplifiedSearch();
            var list = new[]
            {
                new TestItem {Name = "abc"},
                new TestItem {Name = "xyz"}
            };
            var actual = await simplifiedSearch.SimplifiedSearchAsync(list, "abc", x => x.Name);
            Assert.Single(actual);
            
            await Task.CompletedTask;
        }
    }
}
