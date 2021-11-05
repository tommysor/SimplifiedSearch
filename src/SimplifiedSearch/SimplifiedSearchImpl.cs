using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedSearch
{
    internal class SimplifiedSearchImpl : ISimplifiedSearch
    {
        public async Task<IList<T>> SimplifiedSearchAsync<T>(IList<T> searchThisList, string searchTerm, Func<T, string?>? fieldToSearch)
        {
            // To remove warning about not using async.
            await Task.CompletedTask;

            // Validate arguments.
            if (searchThisList is null)
                throw new ArgumentNullException(nameof(searchThisList));

            // If nothing will be filtered. Do the fast thing, return the input list.
            if (string.IsNullOrEmpty(searchTerm))
                return searchThisList;

            // If no field is specified, build field of all properties.
            if (fieldToSearch is null)
            {
                fieldToSearch = new Func<T, string?>(v =>
                {
                    var properties = typeof(T).GetProperties().Where(p => p.CanRead);
                    var stringBuilder = new StringBuilder();
                    foreach (var property in properties)
                    {
                        var propertyValue = property.GetValue(v);
                        if (propertyValue is not null)
                            stringBuilder.AppendLine(propertyValue.ToString());
                    }
                    return stringBuilder.ToString();
                });
            }

            // Build the results.
            //todo implement proper search.
            var results = new List<T>();
            foreach (var item in searchThisList)
            {
                var fieldValue = fieldToSearch(item);
                if (fieldValue is null)
                    continue;

                if (fieldValue.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase))
                    results.Add(item);
            }

            return results;
        }
    }
}
