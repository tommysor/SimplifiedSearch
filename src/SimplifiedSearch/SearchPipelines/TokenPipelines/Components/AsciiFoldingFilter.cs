using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Unidecode.NET;

namespace SimplifiedSearch.SearchPipelines.TokenPipelines.Components
{
    internal class AsciiFoldingFilter : ITokenPipelineComponent
    {
        public Task<string[]> RunAsync(params string[] value)
        {
            var len = value.Length;
            for (var i = 0; i < len; i++)
            {
                value[i] = value[i].Unidecode();
            }

            return Task.FromResult(value);
        }
    }
}
