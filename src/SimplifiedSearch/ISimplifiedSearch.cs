using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimplifiedSearch
{
    internal interface ISimplifiedSearch
    {
        Task<IList<T>> SimplifiedSearchAsync<T>(IList<T> searchThisList, string searchTerm, Func<T, string?>? propertyToSearchLambda);
        Task<IList<T>> SimplifiedSearchAsync<T>(IList<T> searchThisList, string searchTerm);
    }
}
