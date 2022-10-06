
namespace SimplifiedSearch.ConsumerTests.AcceptanceTests;

using SimplifiedSearch.SearchPipelines;
using SimplifiedSearch.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class ExtensibilityTests
{
    [Fact]
    public async Task ConsumerCanCustomizeTheTokenizerPipeline()
    {
        // developers can create their own pipeline factory
        var factory = new MyFactory();
        
        // they can get the search engine
        var search = factory.GetSimplifiedSearch();

        // and they can search
        var source = new List<string>() { "abc", "cde", "efg", };
        var result = await search.SimplifiedSearchAsync(source, "c");
        Assert.NotEmpty(result);
    }

    ////[Fact]
    public void ConsumerCanCustomizeTheRankingPipeline()
    {
    }
    
    // this is a copy of SimplifiedSearchFactory
    public sealed class MyFactory
    {
        private readonly ISimplifiedSearch _simplifiedSearch;

        public MyFactory()
        {
            _simplifiedSearch = BuildSimplifiedSearch();
        }

        public ISimplifiedSearch GetSimplifiedSearch()
        {
            return _simplifiedSearch;
        }

        private ISimplifiedSearch BuildSimplifiedSearch()
        {
            var lowercaseFilter = new UppercaseFilter(); // customize this
            var asciiFoldingFilter = new AsciiFoldingFilter();
            var tokenizeFilter = new TokenizeFilter();
            var pipeline = new SearchPipeline(lowercaseFilter, asciiFoldingFilter, tokenizeFilter);
            var propertyBuilder = new PropertyBuilder();
            return new SimplifiedSearchImpl(pipeline, propertyBuilder);
        }
    }

    internal class UppercaseFilter : ISearchPipelineComponent
    {
        public Task<string[]> RunAsync(params string[] value)
        {
            var results = new string[value.Length];

            for (var i = 0; i < value.Length; i++)
                results[i] = value[i].ToUpperInvariant();

            return Task.FromResult(results);
        }
    }
}
