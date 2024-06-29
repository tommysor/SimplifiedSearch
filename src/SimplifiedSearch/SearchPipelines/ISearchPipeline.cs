using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimplifiedSearch.SearchPipelines
{
    internal interface ISearchPipeline
    {
        IList<T> Search<T>(IList<T> list, string searchTerm, Func<T, string?> fieldToSearch);
    }
}
