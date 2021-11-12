using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedSearch
{
    public static class SimplifiedSearchExtensions
    {
        private static readonly ISimplifiedSearch _search = new SimplifiedSearchFactory().GetSimplifiedSearch();

        /// <summary>
        /// Search for items where string defined by <paramref name="propertyToSearchLambda"/> matches <paramref name="searchTerm"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="searchThisList"></param>
        /// <param name="searchTerm"></param>
        /// <param name="propertyToSearchLambda">If <see langword="null"/> than all properties will be searched.</param>
        /// <returns></returns>
        public static async Task<IList<T>> SimplifiedSearchAsync<T>(this IList<T> searchThisList, string searchTerm, Func<T, string?>? propertyToSearchLambda)
        {
            var results = await _search.SimplifiedSearchAsync(searchThisList, searchTerm, propertyToSearchLambda).ConfigureAwait(false);
            return results;
        }

        /// <summary>
        /// Search for items in list where any property matches <paramref name="searchTerm"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="searchThisList"></param>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public static async Task<IList<T>> SimplifiedSearchAsync<T>(this IList<T> searchThisList, string searchTerm)
        {
            return await SimplifiedSearchAsync(searchThisList, searchTerm, null).ConfigureAwait(false);
        }

        /// <summary>
        /// Search for items where string defined by <paramref name="propertyToSearchLambda"/> matches <paramref name="searchTerm"/>.
        /// IEnumerable will be enumerated in the process.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="searchThisList"></param>
        /// <param name="searchTerm"></param>
        /// <param name="propertyToSearchLambda">If <see langword="null"/> than all properties will be searched.</param>
        /// <returns></returns>
        public static async Task<IList<T>> SimplifiedSearchAsync<T>(this IEnumerable<T> searchThisList, string searchTerm, Func<T, string?>? propertyToSearchLambda)
        {
            var searchThisListAsList = searchThisList.ToArray();
            return await SimplifiedSearchAsync(searchThisListAsList, searchTerm, propertyToSearchLambda).ConfigureAwait(false);
        }

        /// <summary>
        /// Search for items in list where any property matches <paramref name="searchTerm"/>.
        /// IEnumerable will be enumerated in the process.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="searchThisList"></param>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public static async Task<IList<T>> SimplifiedSearchAsync<T>(this IEnumerable<T> searchThisList, string searchTerm)
        {
            return await SimplifiedSearchAsync(searchThisList, searchTerm, null).ConfigureAwait(false);
        }
    }
}
