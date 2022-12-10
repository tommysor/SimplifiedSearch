using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedSearch.SearchPipelines
{
    internal class LowercaseFilter : ITokenPipelineComponent
    {
        public Task<string[]> RunAsync(params string[] value)
        {
            var results = new string[value.Length];

            for (var i = 0; i < value.Length; i++)
                results[i] = value[i].ToLower();

            return Task.FromResult(results);
        }
    }
}
