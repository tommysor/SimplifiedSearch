
namespace SimplifiedSearch.RankingPipelines;

using System;
using System.Threading.Tasks;

public sealed class RankingPipeline : IRankingPipeline
{
    private readonly IRankingPipelineComponent[] _components;

    public RankingPipeline(params IRankingPipelineComponent[] components)
    {
        _components = components;
    }
    
    public async Task<double> Evaluate(string[] fieldValueTokens, string[] searchTermTokens)
    {
        var similarityRank = 0.0;
        foreach (var component in this._components)
        {
            similarityRank += await component.Evaluate(fieldValueTokens, searchTermTokens);
        }

        return similarityRank;
    }
}
