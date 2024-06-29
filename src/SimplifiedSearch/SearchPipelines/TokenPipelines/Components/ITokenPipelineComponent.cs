namespace SimplifiedSearch.SearchPipelines.TokenPipelines.Components;

internal interface ITokenPipelineComponent
{
    string[] Run(string[] value);
}
