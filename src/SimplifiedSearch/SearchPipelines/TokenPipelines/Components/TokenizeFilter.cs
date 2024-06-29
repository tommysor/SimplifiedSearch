using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedSearch.SearchPipelines.TokenPipelines.Components
{
    internal class TokenizeFilter : ITokenPipelineComponent
    {
        private static readonly char[] _splitChars =
        [
            ' ',
            '\t',
            '\r',
            '\n',
            '-',
            ',',
            '.',
        ];

        public string[] Run(params string[] value)
        {
            var results = new HashSet<string>();
            foreach (var item in value)
            {
                var splitItem = item.Split(_splitChars, StringSplitOptions.RemoveEmptyEntries);
                foreach (var result in splitItem)
                    results.Add(result);
            }

            return [.. results];
        }
    }
}
