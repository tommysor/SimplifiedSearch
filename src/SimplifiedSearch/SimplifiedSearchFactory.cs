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
    /// <summary>
    /// Factory that creates simplified search instances.
    /// </summary>
    public sealed class SimplifiedSearchFactory : ISimplifiedSearchFactory
    {
        /// <summary>
        /// Name of the default configuration.
        /// </summary>
        public static string DefaultName { get; } = "default";

        private readonly Dictionary<string, ISimplifiedSearch> _simplifiedSearches = new();

        /// <summary>
        /// Static instance of <see cref="SimplifiedSearchFactory"/>.
        /// This is used by the extension methods, with the "default" configuration.
        /// </summary>
        /// <returns></returns>
        public static SimplifiedSearchFactory Instance { get; } = new SimplifiedSearchFactory();

        private void AddToDictionary(string name, Action<SimplifiedSearchConfiguration> configurationBuilder)
        {
            name = name.ToLower();

            var configuration = new SimplifiedSearchConfiguration();
            configurationBuilder(configuration);
            var search = BuildSimplifiedSearch(configuration);

            _simplifiedSearches[name] = search;
        }

        private ISimplifiedSearch GetSimplifiedSearch(string name)
        {
            name = name.ToLower();

            if (_simplifiedSearches.TryGetValue(name, out var search))
            {
                return search;
            }

            // Fallback to default
            return _simplifiedSearches[DefaultName];
        }

        private ISimplifiedSearch BuildSimplifiedSearch(SimplifiedSearchConfiguration configuration)
        {
            var lowercaseFilter = new LowercaseFilter();
            var asciiFoldingFilter = new AsciiFoldingFilter();
            var tokenizeFilter = new TokenizeFilter();
            var tokenPipeline = new TokenPipeline(lowercaseFilter, asciiFoldingFilter, tokenizeFilter);

            var mainSimilarityRanker = new MainSimilarityRanker();
            var exactEqualStartBoost = new ExactEqualStartBoost();
            var similarityRankPipeline = new SimilarityRankPipeline(tokenPipeline, mainSimilarityRanker, exactEqualStartBoost);

            var resultSelector = configuration.ResultSelector;
            
            var pipeline = new SearchPipeline(similarityRankPipeline, resultSelector);
            var propertyBuilder = new PropertyBuilder();
            return new SimplifiedSearchImpl(pipeline, propertyBuilder);
        }

        /// <summary>
        /// Resets the factory to the default configuration.
        /// This removes all custom configurations.
        /// </summary>
        public void ResetToDefault()
        {
            _simplifiedSearches.Clear();
            AddToDictionary(DefaultName, _ => {});
        }

        /// <inheritdoc cref="SimplifiedSearchFactory" />
        public SimplifiedSearchFactory()
        {
            ResetToDefault();
        }

        /// <inheritdoc/>
        public ISimplifiedSearch Create()
        {
            return GetSimplifiedSearch(DefaultName);
        }

        /// <inheritdoc/>
        public ISimplifiedSearch Create(string name)
        {
            return GetSimplifiedSearch(name);
        }

        /// <summary>
        /// Add a named searcher with a custom configuration.
        /// </summary>
        /// <param name="name">Name of the searcher. Case insensitive.</param>
        /// <param name="configurationBuilder"></param>
        public void Add(string name, Action<SimplifiedSearchConfiguration> configurationBuilder)
        {
            AddToDictionary(name, configurationBuilder);
        }
    }
}
