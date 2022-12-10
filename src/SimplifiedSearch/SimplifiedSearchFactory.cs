using SimplifiedSearch.SearchPipelines;
using SimplifiedSearch.SearchPipelines.SimilarityRankPipelines;
using SimplifiedSearch.SearchPipelines.SimilarityRankPipelines.Components;
using SimplifiedSearch.SearchPipelines.TokenPipelines;
using SimplifiedSearch.SearchPipelines.TokenPipelines.Components;
using SimplifiedSearch.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimplifiedSearch
{
    internal class SimplifiedSearchFactory
    {
        private readonly ISimplifiedSearch _simplifiedSearch;

        private ISimplifiedSearch BuildSimplifiedSearch()
        {
            var lowercaseFilter = new LowercaseFilter();
            var asciiFoldingFilter = new AsciiFoldingFilter();
            var tokenizeFilter = new TokenizeFilter();
            var tokenPipeline = new TokenPipeline(lowercaseFilter, asciiFoldingFilter, tokenizeFilter);

            var mainSimilarityRanker = new MainSimilarityRanker();
            var exactEqualStartBoost = new ExactEqualStartBoost();
            var similarityRankPipeline = new SimilarityRankPipeline(tokenPipeline, mainSimilarityRanker, exactEqualStartBoost);
            
            var pipeline = new SearchPipeline(similarityRankPipeline);
            var propertyBuilder = new PropertyBuilder();
            return new SimplifiedSearchImpl(pipeline, propertyBuilder);
        }

        public SimplifiedSearchFactory()
        {
            _simplifiedSearch = BuildSimplifiedSearch();
        }

        public ISimplifiedSearch GetSimplifiedSearch()
        {
            return _simplifiedSearch;
        }
    }
}
