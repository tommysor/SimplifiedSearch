using SimplifiedSearch.SearchPipelines;
using SimplifiedSearch.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimplifiedSearch
{
    using SimplifiedSearch.RankingPipelines;

    internal class SimplifiedSearchFactory
    {
        private readonly ISimplifiedSearch _simplifiedSearch;

        private ISimplifiedSearch BuildSimplifiedSearch()
        {
            // ranking pipeline
            var similarity = new TokenSimilarityRanking();
            var rankingPipeline = new RankingPipeline(similarity);

            // search pipeline
            // NOTE: would it be better to pass the `ranking` to SimplifiedSearchImpl instead?
            var lowercaseFilter = new LowercaseFilter();
            var asciiFoldingFilter = new AsciiFoldingFilter();
            var tokenizeFilter = new TokenizeFilter();
            var pipeline = new SearchPipeline(rankingPipeline, lowercaseFilter, asciiFoldingFilter, tokenizeFilter);

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
