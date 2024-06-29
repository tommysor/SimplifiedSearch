using System.Collections.Generic;
using SimplifiedSearch.SearchPipelines.TokenPipelines.Components;

namespace SimplifiedSearch.SearchPipelines.TokenPipelines;

internal sealed class TokenPipeline : ITokenPipeline
{
    private readonly List<ITokenPipelineComponent> _tokenPipelineComponents = [];

    public TokenPipeline(params ITokenPipelineComponent[] tokenPipelineComponents)
    {
        _tokenPipelineComponents.AddRange(tokenPipelineComponents);
    }

    public string[] Run(string value)
    {
        var valueLocal = new[] { value };
        foreach (var component in _tokenPipelineComponents)
        {
            valueLocal = component.Run(valueLocal);
        }

        return valueLocal;
    }
}
