using System;

namespace SimplifiedSearch.Utils
{
    public interface IPropertyBuilder
    {
        Func<T, string> BuildPropertyToSearchLambda<T>();
    }
}
