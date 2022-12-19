using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimplifiedSearch
{
    /// <summary>
    /// Exposes methods for performing simplified search.
    /// </summary>
    public interface ISimplifiedSearch
    {
        /// <summary>
        /// Search for items where string defined by <paramref name="propertyToSearchLambda"/> matches <paramref name="searchTerm"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="searchThisList"></param>
        /// <param name="searchTerm"></param>
        /// <param name="propertyToSearchLambda">If <see langword="null"/> than all properties will be searched.</param>
        /// <returns></returns>
        Task<IList<T>> SimplifiedSearchAsync<T>(IList<T> searchThisList, string searchTerm, Func<T, string?>? propertyToSearchLambda);

        /// <summary>
        /// Search for items in list where any property matches <paramref name="searchTerm"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="searchThisList"></param>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        Task<IList<T>> SimplifiedSearchAsync<T>(IList<T> searchThisList, string searchTerm);
    }
}
