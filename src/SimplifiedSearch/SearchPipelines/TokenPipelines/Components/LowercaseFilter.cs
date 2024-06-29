namespace SimplifiedSearch.SearchPipelines.TokenPipelines.Components;

internal class LowercaseFilter : ITokenPipelineComponent
{
    public string[] Run(params string[] value)
    {
        for (var i = 0; i < value.Length; i++)
            value[i] = value[i].ToLower();

        return value;
    }
}
