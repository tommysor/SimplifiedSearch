using SimplifiedSearch.SearchPipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedSearch
{
    internal class SimplifiedSearchImpl : ISimplifiedSearch
    {
        private readonly SearchPipeline _searchPipeline;

        internal SimplifiedSearchImpl(SearchPipeline searchPipeline)
        {
            _searchPipeline = searchPipeline ?? throw new ArgumentNullException(nameof(searchPipeline));
        }

        public async Task<IList<T>> SimplifiedSearchAsync<T>(IList<T> searchThisList, string searchTerm, Func<T, string?>? fieldToSearch)
        {
            // Validate arguments.
            if (searchThisList is null)
                throw new ArgumentNullException(nameof(searchThisList));

            // If nothing will be filtered. Do the fast thing, return the input list.
            if (string.IsNullOrEmpty(searchTerm))
                return searchThisList;

            // If no field is specified, build field of all properties.
            if (fieldToSearch is null)
            {
                fieldToSearch = new Func<T, string?>(x =>
                {
                    var properties = typeof(T).GetProperties().Where(p => p.CanRead);
                    var stringBuilder = new StringBuilder();
                    foreach (var property in properties)
                    {
                        var propertyValue = property.GetValue(x);
                        if (propertyValue is not null)
                            stringBuilder.AppendLine(propertyValue.ToString());
                    }
                    return stringBuilder.ToString();
                });
            }

            // Build the results.
            var results = await _searchPipeline.SearchAsync(searchThisList, searchTerm, fieldToSearch).ConfigureAwait(false);

            return results;
        }
    }
}
