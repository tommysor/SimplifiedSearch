using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedSearch.SearchPipelines
{
    internal interface ITokenPipeline
    {
        Task<string[]> RunAsync(string value);
    }
}