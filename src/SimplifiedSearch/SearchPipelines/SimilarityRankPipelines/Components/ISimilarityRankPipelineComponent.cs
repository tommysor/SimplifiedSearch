using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedSearch.SearchPipelines.SimilarityRankPipelines.Components
{
    internal interface ISimilarityRankPipelineComponent
    {
        Task<double> RunAsync(string[] fieldValueTokens, string[] searchTermTokens);
    }
}
