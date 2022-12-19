using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SimplifiedSearch.SearchPipelines.SimilarityRankPipelines;

namespace SimplifiedSearch.SearchPipelines.ResultSelectors
{
    /// <summary>
    /// The selector to use to select which results to return 
    /// based on the ranked items.
    /// </summary>
    public interface IResultSelector
    {
        /// <summary>
        /// Select which results to return and in what order.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rankedList">Unsorted list of items with rank.</param>
        /// <returns>The final result of the search. Sorted and filtered.</returns>
        Task<IList<T>> RunAsync<T>(IList<SimilarityRankItem<T>> rankedList);
    }
}
