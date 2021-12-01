using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimplifiedSearch.Utils
{
    internal class PropertyBuilder
    {
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
            var propertyToSearchLambda = new Func<T, string>(x =>
            {
                var properties = typeof(T).GetProperties()
                    .Where(p => p.CanRead)
                    .Where(p => IsTypeIncludedInSearch(p.PropertyType));
                var stringBuilder = new StringBuilder();
                foreach (var property in properties)
                {
                    var propertyValue = property.GetValue(x);
                    if (propertyValue is not null)
                        stringBuilder.AppendLine(propertyValue.ToString());
                }
                return stringBuilder.ToString();
            });

            return propertyToSearchLambda;
        }

        private bool IsTypeIncludedInSearch(Type type)
        {
            if (type == typeof(string))
                return true;

            static bool isBasicTypeIncludedInSearch(Type possibleBasicType)
            {
                return possibleBasicType.IsPrimitive
                    || possibleBasicType.IsEnum;
            }

            if (isBasicTypeIncludedInSearch(type))
                return true;

            var underlying = Nullable.GetUnderlyingType(type);
            if (underlying is not null)
                return isBasicTypeIncludedInSearch(underlying);
            
            return false;
        }
    }
}
