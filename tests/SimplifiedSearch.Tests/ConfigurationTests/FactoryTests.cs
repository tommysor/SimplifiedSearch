using SimplifiedSearch.Tests.Models;
using SimplifiedSearch.Tests.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimplifiedSearch.Tests.ConfigurationTests
{
    public class FactoryTests
    {
        [Fact]
        public async Task NoExplicitSettings_CanBeUsed()
        {
            var factory = new SimplifiedSearchFactory();
            var simplifiedSearch = factory.GetSimplifiedSearch();
            var actual = await simplifiedSearch.SimplifiedSearchAsync(TestData.UsStates, "Pennsylvania", x => x.Name);
            Assert.Single(actual);
            
            await Task.CompletedTask;
        }
    }
}
