using System;
using System.Collections.Generic;
using System.Text;
using SimplifiedSearch.SearchPipelines.ResultSelectors;

namespace SimplifiedSearch
{
    /// <summary>
    /// The configuration for simplified search.
    /// </summary>
    /// <returns></returns>
    public sealed class SimplifiedSearchConfiguration
    {
        /// <summary>
        /// The selector to use to select which results to return 
        /// based on the ranked items.
        /// </summary>
        /// <returns></returns>
        public IResultSelector ResultSelector { get; set; } = new ResultSelector();
    }
}
