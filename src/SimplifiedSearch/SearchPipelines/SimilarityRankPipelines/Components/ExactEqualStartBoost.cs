using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedSearch.SearchPipelines.SimilarityRankPipelines.Components
{
    internal sealed class ExactEqualStartBoost : ISimilarityRankPipelineComponent
    {
        public double Run(string[] fieldValueTokens, string[] searchTermTokens)
        {
            var similarityRank = 0.0;
            foreach(var fieldValue in fieldValueTokens)
                foreach(var searchTerm in searchTermTokens)
                {
                    var similarityRankForToken = GetSimilarityRankForToken(fieldValue, searchTerm);
                    similarityRank += similarityRankForToken;
                }

            return similarityRank;
        }

        private static double GetSimilarityRankForToken(string fieldValue, string searchTerm)
        {
            const int maxExactCheckLength = 5;
            var minLength = Math.Min(fieldValue.Length, searchTerm.Length);
            if (maxExactCheckLength < minLength)
                minLength = maxExactCheckLength;

            var rank = 0.0;
            for (var i = 0; i < minLength; i++)
            {
                if (fieldValue[i] == searchTerm[i])
                {
                    rank += 0.1;
                }
                else
                {
                    break;
                }
            }

            return rank;
        }
    }
}
