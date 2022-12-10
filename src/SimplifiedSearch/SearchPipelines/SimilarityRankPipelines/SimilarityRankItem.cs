using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedSearch.SearchPipelines.SimilarityRankPipelines
{
    internal sealed class SimilarityRankItem<T>
    {
        public T Item { get; private set; }
        public double SimilarityRank { get; set; }

        public SimilarityRankItem(T item)
        {
            Item = item ?? throw new ArgumentNullException(nameof(item));
        }
    }
}
