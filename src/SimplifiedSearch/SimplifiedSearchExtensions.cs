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

        //todo doc public static async Task<IList<T>> SimplifiedSearchAsync<T>(this IList<T> searchThisList, string searchTerm, Func<T, string?>? fieldToSearch)
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="searchThisList"></param>
        /// <param name="searchTerm"></param>
        /// <param name="fieldToSearch"></param>
        /// <returns></returns>
        public static async Task<IList<T>> SimplifiedSearchAsync<T>(this IList<T> searchThisList, string searchTerm, Func<T, string?>? fieldToSearch)
        {
            var results = await _search.SimplifiedSearchAsync(searchThisList, searchTerm, fieldToSearch).ConfigureAwait(false);
            return results;
        }

        //todo doc public static async Task<IList<T>> SimplifiedSearchAsync<T>(this IEnumerable<T> searchThisList, string searchTerm, Func<T, string?>? fieldToSearch)
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="searchThisList"></param>
        /// <param name="searchTerm"></param>
        /// <param name="fieldToSearch"></param>
        /// <returns></returns>
        public static async Task<IList<T>> SimplifiedSearchAsync<T>(this IEnumerable<T> searchThisList, string searchTerm, Func<T, string?>? fieldToSearch)
        {
            var searchThisListAsList = searchThisList.ToArray();
            return await SimplifiedSearchAsync(searchThisListAsList, searchTerm, fieldToSearch).ConfigureAwait(false);
        }
    }
}
