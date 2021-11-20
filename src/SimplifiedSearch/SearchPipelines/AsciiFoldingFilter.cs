using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Unidecode.NET;

namespace SimplifiedSearch.SearchPipelines
{
    internal class AsciiFoldingFilter : ISearchPipelineComponent
    {
        public Task<string[]> RunAsync(params string[] value)
        {
            var len = value.Length;
            var results = new string[len];
            for (var i = 0; i < len; i++)
            {
                var asciiString = value[i].Unidecode();
                results[i] = asciiString;
            }

            return Task.FromResult(results);
        }
    }
}
