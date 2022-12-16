using SimplifiedSearch.SearchPipelines;
using SimplifiedSearch.SearchPipelines.ResultSelectors;
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
    internal class SimplifiedSearchFactory : ISimplifiedSearchFactory
    {
        private readonly ISimplifiedSearch _simplifiedSearchDefault;

        public static ISimplifiedSearchFactory Instance { get; } = new SimplifiedSearchFactory();

        private ISimplifiedSearch BuildSimplifiedSearch()
        {
            var lowercaseFilter = new LowercaseFilter();
            var asciiFoldingFilter = new AsciiFoldingFilter();
            var tokenizeFilter = new TokenizeFilter();
            var tokenPipeline = new TokenPipeline(lowercaseFilter, asciiFoldingFilter, tokenizeFilter);

            var mainSimilarityRanker = new MainSimilarityRanker();
            var exactEqualStartBoost = new ExactEqualStartBoost();
            var similarityRankPipeline = new SimilarityRankPipeline(tokenPipeline, mainSimilarityRanker, exactEqualStartBoost);

            var resultSelector = new ResultSelector();
            
            var pipeline = new SearchPipeline(similarityRankPipeline, resultSelector);
            var propertyBuilder = new PropertyBuilder();
            return new SimplifiedSearchImpl(pipeline, propertyBuilder);
        }

        public SimplifiedSearchFactory()
        {
            _simplifiedSearchDefault = BuildSimplifiedSearch();
        }

        public ISimplifiedSearch Create()
        {
            return _simplifiedSearchDefault;
        }
    }
}
