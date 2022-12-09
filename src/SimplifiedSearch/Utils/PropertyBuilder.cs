using System;
using System.Collections.Generic;

namespace SimplifiedSearch.Utils
{
    public sealed class PropertyBuilder : IPropertyBuilder
    {
        private readonly Dictionary<Type, object> _compiledFuncs = new();

        public Func<T, string> BuildPropertyToSearchLambda<T>()
        {
            var type = typeof(T);
            if (type == typeof(string) || type.IsPrimitive || Nullable.GetUnderlyingType(type)?.IsPrimitive == true)
                return BuildFromPrimitiveOrString<T>();
            else if (type.IsEnum || Nullable.GetUnderlyingType(type)?.IsEnum == true)
                return BuildFromEnum<T>();
            else
                return BuildFromClass<T>();
        }

        private Func<T, string> BuildFromPrimitiveOrString<T>()
        {
            return new Func<T, string>(x => x?.ToString() ?? "");
        }

        private Func<T, string> BuildFromEnum<T>()
        {
            // This roundtrip is in expectation of future optimization.
            return BuildFromPrimitiveOrString<T>();
        }

        private Func<T, string> BuildFromClass<T>()
        {
            var funcType = typeof(Func<T, string>);
            if (_compiledFuncs.TryGetValue(funcType, out var func))
                return (Func<T, string>)func;

            var builder = new BuildFromClass<T>();
            var compiledLambda = builder.Build();

            _compiledFuncs.Add(funcType, compiledLambda);
            return compiledLambda;
        }
    }
}
