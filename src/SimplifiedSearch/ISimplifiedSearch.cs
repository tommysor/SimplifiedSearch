using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimplifiedSearch
{
    //todo doc public interface ISimplifiedSearch
    /// <summary>
    /// 
    /// </summary>
    public interface ISimplifiedSearch
    {
        //todo doc Task<IList<T>> SimplifiedSearchAsync<T>(IList<T> searchThisList, string searchTerm, Func<T, string?>? fieldToSearch);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="searchThisList"></param>
        /// <param name="searchTerm"></param>
        /// <param name="fieldToSearch"></param>
        /// <returns></returns>
        Task<IList<T>> SimplifiedSearchAsync<T>(IList<T> searchThisList, string searchTerm, Func<T, string?>? fieldToSearch);
    }
}
