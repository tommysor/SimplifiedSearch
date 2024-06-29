using System.Collections.Generic;

namespace SimplifiedSearch.SearchPipelines.ResultSelectors;

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
    IList<T> Run<T>(IList<SimilarityRankItem<T>> rankedList);
}
