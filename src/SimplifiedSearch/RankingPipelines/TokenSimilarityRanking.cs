
namespace SimplifiedSearch.RankingPipelines;

using System;
using System.Threading.Tasks;

public sealed class TokenSimilarityRanking : IRankingPipelineComponent
{
    public Task<double> Evaluate(string[] fieldValueTokens, string[] searchTermTokens)
    {
        var similarityRank = 0.0;
        foreach(var fieldValue in fieldValueTokens)
        foreach(var searchTerm in searchTermTokens)
        {
            var similarityRankForToken = GetSimilarityRankForToken(fieldValue, searchTerm);
            similarityRank += similarityRankForToken;
        }

        return Task.FromResult(similarityRank);
    }

    private static double GetSimilarityRankForToken(string fieldValue, string searchTerm)
    {
        var rank = GetSimilarityRankForTokenShort(fieldValue, searchTerm);
            
        if (searchTerm.Length > 1)
            rank += GetSimilarityRankForTokenFuzzy(fieldValue, searchTerm);

        return rank;
    }

    private static double GetSimilarityRankForTokenShort(string fieldValue, string searchTerm)
    {
        const int maxExactCheckLength = 5;
        var minLength = fieldValue.Length < searchTerm.Length ? fieldValue.Length : searchTerm.Length;
        if (maxExactCheckLength < minLength)
            minLength = maxExactCheckLength;

        var rank = 0.0;
        for (var i = 0; i < minLength; i++)
        {
            if (char.ToUpperInvariant(fieldValue[i]) == char.ToUpperInvariant(searchTerm[i]))
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

    private static double GetSimilarityRankForTokenFuzzy(string fieldValue, string searchTerm)
    {
        // Shorten fieldValue to match start of word.
        // Add char to get better match when searchTerm is missing a character.
        var maxLength = searchTerm.Length + 1;
        if (fieldValue.Length > maxLength)
            fieldValue = fieldValue.Substring(0, maxLength);

        var distance = Fastenshtein.Levenshtein.Distance(fieldValue, searchTerm);
        switch (distance)
        {
            case 0:
                return 5;
            case 1:
                return 3;
            case 2:
                return 1;
        }

        return 0;
    }
}