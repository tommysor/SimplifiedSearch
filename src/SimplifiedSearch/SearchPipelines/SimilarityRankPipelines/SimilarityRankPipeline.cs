using System;
using System.Collections.Generic;
using System.Linq;
using SimplifiedSearch.SearchPipelines.SimilarityRankPipelines.Components;
using SimplifiedSearch.SearchPipelines.TokenPipelines;

namespace SimplifiedSearch.SearchPipelines.SimilarityRankPipelines;

internal sealed class SimilarityRankPipeline : ISimilarityRankPipeline
{
    private readonly ITokenPipeline _tokenPipeline;
    private readonly List<ISimilarityRankPipelineComponent> _similarityRankPipelineComponents = new();

    public SimilarityRankPipeline(ITokenPipeline tokenPipeline, params ISimilarityRankPipelineComponent[] similarityRankPipelineComponents)
    {
        _tokenPipeline = tokenPipeline ?? throw new ArgumentNullException(nameof(tokenPipeline));
        _similarityRankPipelineComponents.AddRange(similarityRankPipelineComponents);
    }

    public IList<SimilarityRankItem<T>> Run<T>(IList<T> list, string searchTerm, Func<T, string?> fieldToSearch)
    {
        var listLocal = list.Select(x => new SimilarityRankItem<T>(x)).ToArray();

        var searchTermTokens = _tokenPipeline.Run(searchTerm);

        foreach (var item in listLocal)
        {
            if (item.Item is null)
                continue;
            var fieldValue = fieldToSearch(item.Item);
            if (fieldValue is null)
                continue;

            var fieldValueTokens = _tokenPipeline.Run(fieldValue);
            
            foreach (var component in _similarityRankPipelineComponents)
            {
                var similarityRank = component.Run(fieldValueTokens, searchTermTokens);
                item.SimilarityRank += similarityRank;
            }
        }

        return listLocal;
    }
}
