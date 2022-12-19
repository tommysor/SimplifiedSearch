using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedSearch.SearchPipelines
{
    /// <summary>
    /// Represents an item with a similarity rank.
    /// </summary>
    public sealed class SimilarityRankItem<T>
    {
        /// <summary>
        /// The item being ranked.
        /// </summary>
        /// <returns></returns>
        public T Item { get; private set; }

        /// <summary>
        /// The similarity rank of the item.
        /// Higher is better.
        /// </summary>
        /// <returns></returns>
        public double SimilarityRank { get; set; }

        public SimilarityRankItem(T item)
        {
            Item = item;
        }
    }
}
