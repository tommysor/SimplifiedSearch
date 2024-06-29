using System;
using System.Collections.Generic;

namespace SimplifiedSearch.SearchPipelines.SimilarityRankPipelines;

internal interface ISimilarityRankPipeline
{
    IList<SimilarityRankItem<T>> Run<T>(IList<T> list, string searchTerm, Func<T, string?> fieldToSearch);
}
