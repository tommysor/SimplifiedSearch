namespace SimplifiedSearch.SearchPipelines.TokenPipelines
{
    internal interface ITokenPipeline
    {
        string[] Run(string value);
    }
}