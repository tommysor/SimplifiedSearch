namespace SimplifiedSearch.SearchPipelines.SimilarityRankPipelines.Components;

internal interface ISimilarityRankPipelineComponent
{
    double Run(string[] fieldValueTokens, string[] searchTermTokens);
}
