using System;
using System.Collections.Generic;
using System.Text;

namespace SimplifiedSearch
{
    //todo doc public record SimplifiedSearchConfiguration
    /// <summary>
    /// 
    /// </summary>
    internal record SimplifiedSearchConfiguration
    {
        /// <summary>
        /// Summary:
        ///     Specifies how different strings can be and still count as a match.
        ///     Value of 1 means search string "aaaa" will match "aaab", but not "aabb".
        /// </summary>
        /// <remarks>
        /// Alternative names:
        ///     EditDistance, LevenshteinDistance.
        /// </remarks>
        public uint FuzzySearchMaxDifferences { get; set; } = 2;
    }
}
