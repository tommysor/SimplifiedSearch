using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedSearch.SearchPipelines.SimilarityRankPipelines.Components
{
    internal interface ISimilarityRankPipelineComponent
    {
        double Run(string[] fieldValueTokens, string[] searchTermTokens);
    }
}
