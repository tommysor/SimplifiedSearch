using System;

namespace SimplifiedSearch.Utils
{
    internal interface IPropertyBuilder
    {
        Func<T, string> BuildPropertyToSearchLambda<T>();
    }
}
