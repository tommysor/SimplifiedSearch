using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedSearch.SearchPipelines.SimilarityRankPipelines
{
    internal interface ISimilarityRankPipeline
    {
        Task<IList<SimilarityRankItem<T>>> RunAsync<T>(IList<T> list, string searchTerm, Func<T, string?> fieldToSearch);
    }
}
