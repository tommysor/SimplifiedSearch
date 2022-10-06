
namespace SimplifiedSearch.ConsumerTests.AcceptanceTests;

using SimplifiedSearch.RankingPipelines;
using SimplifiedSearch.SearchPipelines;
using SimplifiedSearch.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

public class ExtensibilityTests
{
    [Fact]
    public async Task ConsumerCanCustomizeTheTokenizerPipeline()
    {
        // developers can create their own pipeline factory
        var factory = new MyFactoryForCustomTokenization();
        
        // they can get the search engine
        var search = factory.GetSimplifiedSearch();

        // and they can search
        var source = new List<string>() { "cgt", "abc", "cde", "efg", };
        var result = await search.SimplifiedSearchAsync(source, "c");
        Assert.Collection(
            result,
            x => Assert.Equal("cgt", x),
            x => Assert.Equal("cde", x));
    }

    [Fact]
    public async Task ConsumerCanCustomizeTheRankingPipeline()
    {
        // developers can create their own pipeline factory
        var factory = new MyFactoryForCustomRanking();
        
        // they can get the search engine
        var search = factory.GetSimplifiedSearch();

        // and they can search
        var source = new List<string>() { "cgt", "abc", "cde", "efg", };
        var result = await search.SimplifiedSearchAsync(source, "c");
        Assert.Collection(
            result,
            x => Assert.Equal("cde", x),
            x => Assert.Equal("cgt", x));
    }
    
    // this is a copy of SimplifiedSearchFactory
    public sealed class MyFactoryForCustomTokenization
    {
        private readonly ISimplifiedSearch _simplifiedSearch;

        public MyFactoryForCustomTokenization()
        {
            _simplifiedSearch = BuildSimplifiedSearch();
        }

        public ISimplifiedSearch GetSimplifiedSearch()
        {
            return _simplifiedSearch;
        }

        private ISimplifiedSearch BuildSimplifiedSearch()
        {
            var similarity = new TokenSimilarityRanking();
            var rankingPipeline = new RankingPipeline(similarity);
            
            var lowercaseFilter = new UppercaseFilter(); // customize this
            var asciiFoldingFilter = new AsciiFoldingFilter();
            var tokenizeFilter = new TokenizeFilter();
            var pipeline = new SearchPipeline(rankingPipeline, lowercaseFilter, asciiFoldingFilter, tokenizeFilter);
            
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
    
    // this is a copy of SimplifiedSearchFactory
    public sealed class MyFactoryForCustomRanking
    {
        private readonly ISimplifiedSearch _simplifiedSearch;

        public MyFactoryForCustomRanking()
        {
            _simplifiedSearch = BuildSimplifiedSearch();
        }

        public ISimplifiedSearch GetSimplifiedSearch()
        {
            return _simplifiedSearch;
        }

        private ISimplifiedSearch BuildSimplifiedSearch()
        {
            // ranking pipeline
            var voyels = new VowelsAreBetterRanking(); // customize this
            var similarity = new TokenSimilarityRanking();
            var rankingPipeline = new RankingPipeline(voyels, similarity);

            // search pipeline
            // NOTE: would it be better to pass the `ranking` to SimplifiedSearchImpl instead?
            var lowercaseFilter = new UppercaseFilter(); // customize this
            var asciiFoldingFilter = new AsciiFoldingFilter();
            var tokenizeFilter = new TokenizeFilter();
            var pipeline = new SearchPipeline(rankingPipeline, lowercaseFilter, asciiFoldingFilter, tokenizeFilter);

            var propertyBuilder = new PropertyBuilder();

            return new SimplifiedSearchImpl(pipeline, propertyBuilder);
        }
    }
    
    internal sealed class VowelsAreBetterRanking : IRankingPipelineComponent
    {
        private static readonly char[] vowels = new char[] { 'a', 'e', 'i', 'o', 'u', 'y', 'A', 'E', 'I', 'O', 'U', 'Y', };
        
        public Task<double> Evaluate(string[] fieldValueTokens, string[] searchTermTokens)
        {
            var similarityRank = 0.0;
            foreach(var fieldValue in fieldValueTokens)
            foreach(var searchTerm in searchTermTokens)
            {
                var similarityRankForToken = GetRankForToken(fieldValue, searchTerm);
                similarityRank += similarityRankForToken;
            }

            return Task.FromResult(similarityRank);
        }

        private double GetRankForToken(string fieldValue, string searchTerm)
        {
            var similarityRank = 0.0;
            for (int i = 0; i < fieldValue.Length; i++)
            {
                var c = fieldValue[i];
                if (vowels.Contains(c))
                {
                    similarityRank += 1D;
                }
            }

            return similarityRank;
        }
    }
}
