using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedSearch
{
    public static class SimplifiedSearchExtensions
    {
        private static readonly ISimplifiedSearchFactory _factory = SimplifiedSearchFactory.Instance;

        /// <inheritdoc cref="ISimplifiedSearch.SimplifiedSearchAsync{T}(IList{T}, string, Func{T, string?}?)" />
        public static async Task<IList<T>> SimplifiedSearchAsync<T>(this IList<T> searchThisList, string searchTerm, Func<T, string?>? propertyToSearchLambda)
        {
            await Task.Delay(10).ConfigureAwait(false);
            var search = _factory.Create();
            var results = await search.SimplifiedSearchAsync(searchThisList, searchTerm, propertyToSearchLambda).ConfigureAwait(false);
            return results;
        }

        /// <inheritdoc cref="ISimplifiedSearch.SimplifiedSearchAsync{T}(IList{T}, string)" />
        public static async Task<IList<T>> SimplifiedSearchAsync<T>(this IList<T> searchThisList, string searchTerm)
        {
            return await SimplifiedSearchAsync(searchThisList, searchTerm, null).ConfigureAwait(false);
        }

        /// <inheritdoc cref="ISimplifiedSearch.SimplifiedSearchAsync{T}(IList{T}, string, Func{T, string?}?)" />
        /// <remarks>
        /// This method is not as efficient as <see cref="SimplifiedSearchAsync{T}(IList{T}, string, Func{T, string?}?)"/> because it has to convert the <see cref="IEnumerable{T}"/> to a <see cref="IList{T}"/>.
        /// </remarks>
        public static async Task<IList<T>> SimplifiedSearchAsync<T>(this IEnumerable<T> searchThisList, string searchTerm, Func<T, string?>? propertyToSearchLambda)
        {
            var searchThisListAsList = searchThisList.ToArray();
            return await SimplifiedSearchAsync(searchThisListAsList, searchTerm, propertyToSearchLambda).ConfigureAwait(false);
        }

        /// <inheritdoc cref="ISimplifiedSearch.SimplifiedSearchAsync{T}(IList{T}, string)" />
        /// <remarks>
        /// This method is not as efficient as <see cref="SimplifiedSearchAsync{T}(IList{T}, string)"/> because it has to convert the <see cref="IEnumerable{T}"/> to a <see cref="IList{T}"/>.
        /// </remarks>
        public static async Task<IList<T>> SimplifiedSearchAsync<T>(this IEnumerable<T> searchThisList, string searchTerm)
        {
            return await SimplifiedSearchAsync(searchThisList, searchTerm, null).ConfigureAwait(false);
        }
    }
}
