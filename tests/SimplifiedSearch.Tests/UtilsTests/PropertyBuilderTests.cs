using SimplifiedSearch.Tests.Models;
using SimplifiedSearch.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimplifiedSearch.Tests.SearchPropertyBuilderTests
{
    public class PropertyBuilderTests
    {
        private readonly PropertyBuilder _propertyBuilder;

        public PropertyBuilderTests()
        {
            _propertyBuilder = new PropertyBuilder();
        }

        private Func<T, string> BuildFromAnonymousType<T>(T _)
        {
            return _propertyBuilder.BuildPropertyToSearchLambda<T>();
        }

        #region string
        [Fact]
        public void BuildFromString()
        {
            var func = _propertyBuilder.BuildPropertyToSearchLambda<string>();
            Assert.IsType<Func<string, string>>(func);
        }

        [Fact]
        public void BuildFromStringNullable()
        {
            var func = _propertyBuilder.BuildPropertyToSearchLambda<string?>();
            Assert.IsType<Func<string?, string>>(func);
        }

        [Fact]
        public void BuildFromStringGetValue()
        {
            var func = _propertyBuilder.BuildPropertyToSearchLambda<string>();
            var actual = func("abc");
            Assert.Equal("abc", actual);
        }

        [Fact]
        public void BuildFromStringGetValueNullable()
        {
            var func = _propertyBuilder.BuildPropertyToSearchLambda<string>();
            string? input = "abc";
            var actual = func(input);
            Assert.Equal("abc", actual);
        }

        [Fact]
        public void BuildFromStringGetValueNull()
        {
            var func = _propertyBuilder.BuildPropertyToSearchLambda<string?>();
            string? input = null;
            var actual = func(input);
            Assert.Equal("", actual);
        }
        #endregion

        #region int
        [Fact]
        public void BuildFromInt()
        {
            var func = _propertyBuilder.BuildPropertyToSearchLambda<int>();
            Assert.IsType<Func<int, string>>(func);
        }

        [Fact]
        public void BuildFromIntNullable()
        {
            var func = _propertyBuilder.BuildPropertyToSearchLambda<int?>();
            Assert.IsType<Func<int?, string>>(func);
        }

        [Fact]
        public void BuildFromIntGetValue()
        {
            var func = _propertyBuilder.BuildPropertyToSearchLambda<int>();
            var actual = func(2);
            Assert.Equal("2", actual);
        }

        [Fact]
        public void BuildFromIntGetValueNullable()
        {
            var func = _propertyBuilder.BuildPropertyToSearchLambda<int?>();
            int? input = 2;
            var actual = func(input);
            Assert.Equal("2", actual);
        }

        [Fact]
        public void BuildFromIntGetValueNull()
        {
            var func = _propertyBuilder.BuildPropertyToSearchLambda<int?>();
            int? input = null;
            var actual = func(input);
            Assert.Equal("", actual);
        }
        #endregion

        #region enum
        [Fact]
        public void BuildFromEnum()
        {
            var func = _propertyBuilder.BuildPropertyToSearchLambda<TestEnum>();
            Assert.IsType<Func<TestEnum, string>>(func);
        }

        [Fact]
        public void BuildFromEnumNullable()
        {
            var func = _propertyBuilder.BuildPropertyToSearchLambda<TestEnum?>();
            Assert.IsType<Func<TestEnum?, string>>(func);
        }

        [Fact]
        public void BuildFromEnumGetValue()
        {
            var func = _propertyBuilder.BuildPropertyToSearchLambda<TestEnum>();
            var actual = func(TestEnum.Second);
            Assert.Equal(nameof(TestEnum.Second), actual);
        }

        [Fact]
        public void BuildFromEnumGetValueNullable()
        {
            var func = _propertyBuilder.BuildPropertyToSearchLambda<TestEnum?>();
            TestEnum? input = TestEnum.Second;
            var actual = func(input);
            Assert.Equal(nameof(TestEnum.Second), actual);
        }

        [Fact]
        public void BuildFromEnumGetValueNull()
        {
            var func = _propertyBuilder.BuildPropertyToSearchLambda<TestEnum?>();
            TestEnum? input = null;
            var actual = func(input);
            Assert.Equal("", actual);
        }
        #endregion

        #region class
        [Fact]
        public void BuildFromClass()
        {
            var func = _propertyBuilder.BuildPropertyToSearchLambda<TestItem>();
            Assert.IsType<Func<TestItem, string>>(func);
        }

        [Fact]
        public void BuildFromClassNullable()
        {
            var func = _propertyBuilder.BuildPropertyToSearchLambda<TestItem?>();
            Assert.IsType<Func<TestItem?, string>>(func);
        }

        [Fact]
        public void BuildFromClassGivesAllProperties()
        {
            var item = new
            {
                First = "abc",
                Second = TestEnum.Second,
                Third = 5
            };
            var expectedBuilder = new StringBuilder()
                .AppendLine(item.First)
                .AppendLine(item.Second.ToString())
                .AppendLine(item.Third.ToString());

            var func = BuildFromAnonymousType(item);

            var actual = func(item);

            Assert.Equal(expectedBuilder.ToString(), actual);
        }

        [Fact]
        public void BuildFromClassGivesAllPropertiesNullable()
        {
            var item = new
            {
                First = (string?)"abc",
                Second = (TestEnum?)TestEnum.Second,
                Third = (int?)5
            };
            var expectedBuilder = new StringBuilder()
                .AppendLine(item.First)
                .AppendLine(item.Second.ToString())
                .AppendLine(item.Third.ToString());

            var func = BuildFromAnonymousType(item);

            var actual = func(item);

            Assert.Equal(expectedBuilder.ToString(), actual);
        }

        [Fact]
        public void BuildFromClassGivesAllPropertiesNull()
        {
            var item = new
            {
                First = (string?)null,
                Second = (TestEnum?)null,
                Third = (int?)null
            };
            var expectedBuilder = new StringBuilder();

            var func = BuildFromAnonymousType(item);

            var actual = func(item);

            Assert.Equal(expectedBuilder.ToString(), actual);
        }

        [Fact]
        public void BuildFromClassThatIsNull()
        {
#pragma warning disable IDE0059 // Unnecessary assignment of a value
            var item = new
#pragma warning restore IDE0059 // Unnecessary assignment of a value
            {
                First = "abc",
            };

            item = null;

            var expectedBuilder = new StringBuilder();

            var func = BuildFromAnonymousType(item);

            var actual = func(item);

            Assert.Equal(expectedBuilder.ToString(), actual);
        }

        [Fact]
        public void BuildFromNestedClassGivesShallowProperties()
        {
            var item = new
            {
                First = "abc",
                Nested = new
                {
                    NestedValue = "def"
                }
            };

            var expectedBuilder = new StringBuilder().AppendLine(item.First);

            var func = BuildFromAnonymousType(item);

            var actual = func(item);

            Assert.Equal(expectedBuilder.ToString(), actual);
        }
        #endregion
    }
}
