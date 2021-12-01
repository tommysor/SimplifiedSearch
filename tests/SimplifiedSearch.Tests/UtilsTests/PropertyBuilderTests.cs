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

        [Fact]
        public void BuildFromString()
        {
            var func = _propertyBuilder.BuildPropertyToSearchLambda<string>();
            Assert.IsType<Func<string, string>>(func);
        }

        [Fact]
        public void BuildFromInt()
        {
            var func = _propertyBuilder.BuildPropertyToSearchLambda<int>();
            Assert.IsType<Func<int, string>>(func);
        }

        [Fact]
        public void BuildFromEnum()
        {
            var func = _propertyBuilder.BuildPropertyToSearchLambda<Models.TestEnum>();
            Assert.IsType<Func<Models.TestEnum, string>>(func);
        }

        [Fact]
        public void BuildFromClass()
        {
            var func = _propertyBuilder.BuildPropertyToSearchLambda<Models.TestItem>();
            Assert.IsType<Func<Models.TestItem, string>>(func);
        }

        [Fact]
        public void BuildFromClassGivesAllProperties()
        {
            var item = new
            {
                First = "abc",
                Second = Models.TestEnum.Second,
                Third = 5
            };
            var expectedBuilder = new StringBuilder();
            expectedBuilder.AppendLine(item.First);
            expectedBuilder.AppendLine(item.Second.ToString());
            expectedBuilder.AppendLine(item.Third.ToString());

            var func = BuildFromAnonymousType(item);

            var actual = func(item);

            Assert.Equal(expectedBuilder.ToString(), actual);
        }
    }
}
