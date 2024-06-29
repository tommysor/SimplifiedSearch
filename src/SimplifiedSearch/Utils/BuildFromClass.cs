using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace SimplifiedSearch.Utils;

internal class BuildFromClass<T>
{
    private readonly ParameterExpression _inputObject;
    private readonly ParameterExpression _stringBuilderForReturnValue;
    private readonly Action<Expression, BinaryExpression?> _appendToReturnValue;
    private readonly List<Expression> _bodyExpressions = new();
    private readonly Type _stringBuilderType = typeof(StringBuilder);
    private readonly LabelTarget _labelTarget;
    private readonly GotoExpression _returnExpression;

    public BuildFromClass()
    {
        _inputObject = Expression.Parameter(typeof(T));
        _stringBuilderForReturnValue = Expression.Parameter(_stringBuilderType);
        _labelTarget = Expression.Label(typeof(string));
        BuildStringBuilderForReturnValue();
        _appendToReturnValue = GetAppendToReturnValue();
        _returnExpression = GetReturnExpression();
        BuildQuickReturnIfInputObjectIsNull();
        BuildExpressionsFromProperties();
        BuildReturnStatement();
    }

    public Func<T, string> Build()
    {
        var body = Expression.Block(new[] { _stringBuilderForReturnValue }, _bodyExpressions);
        var lambda = Expression.Lambda<Func<T, string>>(body, _inputObject);
        var compiledLambda = lambda.Compile();
        return compiledLambda;
    }

    private void BuildStringBuilderForReturnValue()
    {
        var stringBuilderCreate = Expression.New(_stringBuilderType);
        var assignStringBuilder = Expression.Assign(_stringBuilderForReturnValue, stringBuilderCreate);
        _bodyExpressions.Add(assignStringBuilder);
    }

    private Action<Expression, BinaryExpression?> GetAppendToReturnValue()
    {
        var stringBuilderAppendLineMethod = _stringBuilderType.GetMethod("AppendLine", [typeof(string)]);

        void stringBuilderAppend(Expression valueToAppend, BinaryExpression? ifTest)
        {
            var append = Expression.Call(_stringBuilderForReturnValue, stringBuilderAppendLineMethod, valueToAppend);

            if (ifTest is null)
            {
                _bodyExpressions.Add(append);
            }
            else
            {
                var appendIf = Expression.IfThen(ifTest, append);
                _bodyExpressions.Add(appendIf);
            }
        }

        return stringBuilderAppend;
    }

    private GotoExpression GetReturnExpression()
    {
        var stringBuilderToStringMethod = _stringBuilderType.GetMethod("ToString", Array.Empty<Type>());
        var stringBuilderToString = Expression.Call(_stringBuilderForReturnValue, stringBuilderToStringMethod);

        var returnExp = Expression.Return(_labelTarget, stringBuilderToString);
        return returnExp;
    }

    private void BuildQuickReturnIfInputObjectIsNull()
    {
        var isNullTest = Expression.Equal(_inputObject, Expression.Constant(null, typeof(T)));
        _bodyExpressions.Add(Expression.IfThen(isNullTest, _returnExpression));
    }

    private PropertyInfo[] GetPropertiesToInclude()
    {
        var properties = typeof(T).GetProperties()
            .Where(p => p.CanRead)
            .Where(p => IsTypeIncludedInSearch(p.PropertyType))
            .ToArray();
        return properties;
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

    private void BuildExpressionsFromProperties()
    {
        var properties = GetPropertiesToInclude();

        foreach (var property in properties)
        {
            var propertyExp = Expression.Property(_inputObject, property);
            var underlyingType = Nullable.GetUnderlyingType(property.PropertyType);

            var propertyToStringMethod = property.PropertyType.GetMethod("ToString", Array.Empty<Type>());
            var propertyToString = Expression.Call(propertyExp, propertyToStringMethod);

            if (property.PropertyType == typeof(string) || underlyingType is not null)
            {
                var isNotNullTest = Expression.NotEqual(propertyExp, Expression.Constant(null, property.PropertyType));
                _appendToReturnValue(propertyToString, isNotNullTest);
            }
            else
            {
                _appendToReturnValue(propertyToString, null);
            }
        }
    }

    private void BuildReturnStatement()
    {
        _bodyExpressions.Add(_returnExpression);

        // Stryker disable once string : Expression.Constant is needed to compile, but the value is irrelevant.
        var label = Expression.Label(_labelTarget, Expression.Constant(""));
        _bodyExpressions.Add(label);
    }
}
