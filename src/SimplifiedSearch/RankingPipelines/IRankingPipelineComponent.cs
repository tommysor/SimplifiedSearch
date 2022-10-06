namespace SimplifiedSearch.RankingPipelines;

using System.Threading.Tasks;

public interface IRankingPipelineComponent
{
    Task<double> Evaluate(string[] fieldValueTokens, string[] searchTermTokens);
}
