using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SimplifiedSearch.SearchPipelines.SimilarityRankPipelines;

namespace SimplifiedSearch.SearchPipelines.ResultSelectors
{
    internal interface IResultSelector
    {
        Task<IList<T>> RunAsync<T>(IList<SimilarityRankItem<T>> rankedList);
    }
}
