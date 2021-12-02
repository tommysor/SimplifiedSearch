using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            var inputObject = Expression.Parameter(typeof(T), "inputObject");

            var bodyOfFuncExpressions = new List<Expression>();

            // Setup StringBuilder for return value.
            var stringBuilderType = typeof(StringBuilder);
            var stringBuilderCreate = Expression.New(stringBuilderType);
            var stringBuilder = Expression.Parameter(stringBuilderType, "stringBuilder");
            var assignStringBuilder = Expression.Assign(stringBuilder, stringBuilderCreate);
            bodyOfFuncExpressions.Add(assignStringBuilder);

            // Setup method to AppendLine to return value.
            var stringBuilderAppendLineMethod = stringBuilderType.GetMethod("AppendLine", new[] { typeof(string) });
            if (stringBuilderAppendLineMethod is null)
                throw new Exception("Internal error. Unable to find method AppendLine on StringBuilder.");

            // Setup method to Finalize return value.
            var stringBuilderToStringMethod = stringBuilderType.GetMethod("ToString", Array.Empty<Type>());
            if (stringBuilderToStringMethod is null)
                throw new Exception("Internal error. Unable to find method ToString on StringBuilder.");
            var stringBuilderToString = Expression.Call(stringBuilder, stringBuilderToStringMethod);

            // Setup variable for return value.
            var labelTarget = Expression.Label(typeof(string), "labelTarget");
            var returnExp = Expression.Return(labelTarget, stringBuilderToString);

            // Check if the inputObject is null.
            // If so return immediately.
            var isTNullTest = Expression.Equal(inputObject, Expression.Constant(null, typeof(T)));
            bodyOfFuncExpressions.Add(Expression.IfThen(isTNullTest, returnExp));

            // Get properties that should be included in search.
            var properties = typeof(T).GetProperties()
                .Where(p => p.CanRead)
                .Where(p => IsTypeIncludedInSearch(p.PropertyType));

            // Get value for each property.
            foreach (var property in properties)
            {
                var propertyExp = Expression.Property(inputObject, property);

                var underlyingType = Nullable.GetUnderlyingType(property.PropertyType);

                var propertyToStringMethod = property.PropertyType.GetMethod("ToString", Array.Empty<Type>());
                if (propertyToStringMethod is null)
                    throw new Exception($"Internal error. Unable to find method ToString on Property. {property.PropertyType} {property.Name}");
                var propertyToString = Expression.Call(propertyExp, propertyToStringMethod);
                var append = Expression.Call(stringBuilder, stringBuilderAppendLineMethod, propertyToString);

                if (property.PropertyType == typeof(string) || underlyingType is not null)
                {
                    var isNotNullTest = Expression.NotEqual(propertyExp, Expression.Constant(null, property.PropertyType));
                    bodyOfFuncExpressions.Add(Expression.IfThen(isNotNullTest, append));
                }
                else
                {
                    bodyOfFuncExpressions.Add(append);
                }
            }

            // Return.
            bodyOfFuncExpressions.Add(returnExp);

            // GoTo label at end of Func.
            var label = Expression.Label(labelTarget, Expression.Constant(""));
            bodyOfFuncExpressions.Add(label);

            // Build.
            var body = Expression.Block(new[] { stringBuilder }, bodyOfFuncExpressions);
            var lambda = Expression.Lambda<Func<T, string>>(body, inputObject);
            var compiledLambda = lambda.Compile();
            return compiledLambda;
        }

        private static bool IsTypeIncludedInSearch(Type type)
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
