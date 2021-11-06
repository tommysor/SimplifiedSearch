using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedSearch.SearchPipelines
{
    internal class TokenizeFilter : ISearchPipelineComponent
    {
        private static readonly char[] _splitChars = new char[]
        {
            ' ',
            '\t',
            '\r',
            '\n'
        };

        public Task<string[]> RunAsync(params string[] value)
        {
            var results = new List<string>();
            foreach (var item in value)
            {
                var splitItem = item.Split(_splitChars, StringSplitOptions.RemoveEmptyEntries);
                results.AddRange(splitItem);
            }

            return Task.FromResult(results.ToArray());
        }
    }
}
