using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplifiedSearch.SearchPipelines.SimilarityRankPipelines.Components;
using SimplifiedSearch.SearchPipelines.TokenPipelines;

namespace SimplifiedSearch.SearchPipelines.SimilarityRankPipelines
{
    internal sealed class SimilarityRankPipeline : ISimilarityRankPipeline
    {
        private readonly ITokenPipeline _tokenPipeline;
        private readonly List<ISimilarityRankPipelineComponent> _similarityRankPipelineComponents = new();

        public SimilarityRankPipeline(ITokenPipeline tokenPipeline, params ISimilarityRankPipelineComponent[] similarityRankPipelineComponents)
        {
            _tokenPipeline = tokenPipeline;
            _similarityRankPipelineComponents.AddRange(similarityRankPipelineComponents);
        }

        public async Task<IList<SimilarityRankItem<T>>> RunAsync<T>(IList<T> list, string searchTerm, Func<T, string?> fieldToSearch)
        {
            var listLocal = list.Select(x => new SimilarityRankItem<T>(x)).ToArray();

            var searchTermTokens = await _tokenPipeline.RunAsync(searchTerm).ConfigureAwait(false);

            foreach (var item in listLocal)
            {
                var fieldValue = fieldToSearch(item.Item);
                if (fieldValue is null)
                    continue;
                if (fieldValue == "")
                    continue;

                var fieldValueTokens = await _tokenPipeline.RunAsync(fieldValue).ConfigureAwait(false);
                
                foreach (var component in _similarityRankPipelineComponents)
                {
                    var similarityRank = await component.RunAsync(fieldValueTokens, searchTermTokens);
                    item.SimilarityRank += similarityRank;
                }
            }

            return listLocal;
        }
    }
}
