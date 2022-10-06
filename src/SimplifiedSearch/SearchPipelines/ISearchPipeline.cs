using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimplifiedSearch.SearchPipelines
{
    public interface ISearchPipeline
    {
        Task<IList<T>> SearchAsync<T>(IList<T> list, string searchTerm, Func<T, string?> fieldToSearch);
    }
}
