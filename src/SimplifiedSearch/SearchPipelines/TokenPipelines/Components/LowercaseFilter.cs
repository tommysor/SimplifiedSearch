using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedSearch.SearchPipelines.TokenPipelines.Components
{
    internal class LowercaseFilter : ITokenPipelineComponent
    {
        public Task<string[]> RunAsync(params string[] value)
        {
            for (var i = 0; i < value.Length; i++)
                value[i] = value[i].ToLower();

            return Task.FromResult(value);
        }
    }
}
