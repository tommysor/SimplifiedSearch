
namespace SimplifiedSearch.RankingPipelines;

using System;
using System.Threading.Tasks;

public interface IRankingPipeline
{
    Task<double> Evaluate(string[] fieldValueTokens, string[] searchTermTokens);
}
