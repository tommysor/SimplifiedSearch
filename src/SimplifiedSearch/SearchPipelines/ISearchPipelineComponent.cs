using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedSearch.SearchPipelines
{
    public interface ISearchPipelineComponent
    {
        Task<string[]> RunAsync(params string[] value);
    }
}
