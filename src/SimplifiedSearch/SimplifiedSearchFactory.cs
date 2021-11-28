using SimplifiedSearch.SearchPipelines;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimplifiedSearch
{
    //todo doc public class SimplifiedSearchFactory
    /// <summary>
    /// 
    /// </summary>
    internal class SimplifiedSearchFactory
    {
        private readonly SimplifiedSearchConfiguration _configuration;
        private readonly ISimplifiedSearch _simplifiedSearch;

        private ISimplifiedSearch BuildSimplifiedSearch()
        {
            var lowercaseFilter = new LowercaseFilter();
            var asciiFoldingFilter = new AsciiFoldingFilter();
            var tokenizeFilter = new TokenizeFilter();
            var pipeline = new SearchPipeline(lowercaseFilter, asciiFoldingFilter, tokenizeFilter);
            return new SimplifiedSearchImpl(pipeline);
        }

        //todo doc public SimplifiedSearchFactory()
        /// <summary>
        /// 
        /// </summary>
        public SimplifiedSearchFactory()
        {
            _configuration = new SimplifiedSearchConfiguration();
            _simplifiedSearch = BuildSimplifiedSearch();
        }

        //todo doc public SimplifiedSearchFactory(Action<SimplifiedSearchConfiguration> configure)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configure"></param>
        public SimplifiedSearchFactory(Action<SimplifiedSearchConfiguration> configure)
        {
            _configuration = new SimplifiedSearchConfiguration();
            configure(_configuration);
            _simplifiedSearch = BuildSimplifiedSearch();
        }

        //todo doc public ISimplifiedSearch GetSimplifiedSearch()
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ISimplifiedSearch GetSimplifiedSearch()
        {
            return _simplifiedSearch;
        }
    }
}
