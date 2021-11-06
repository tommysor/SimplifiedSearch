using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedSearch.SearchPipelines
{
    internal class AsciiFoldingFilter : ISearchPipelineComponent
    {
        public Task<string[]> RunAsync(params string[] value)
        {
            //todo implement ascii folding.
            return Task.FromResult(value);
        }
    }
}
