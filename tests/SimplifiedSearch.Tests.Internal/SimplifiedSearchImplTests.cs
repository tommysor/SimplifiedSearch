using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimplifiedSearch.Tests.Internal
{
    public class SimplifiedSearchImplTests
    {
        private readonly IList<string> _list = new[] { "abcd", "efgh" };
        private readonly string _searchTerm = "abcd";
        private readonly Func<string, string> _fieldToSearch = x => x;
        private readonly SearchPipelines.ISearchPipeline _searchPipelineMock;
        private readonly SimplifiedSearch.Utils.IPropertyBuilder _propertyBuilderMock;

        public SimplifiedSearchImplTests()
        {
            var searchPipelineMockBuilder = new Moq.Mock<SearchPipelines.ISearchPipeline>();
            searchPipelineMockBuilder
                .Setup(x => x.SearchAsync(_list, _searchTerm, _fieldToSearch))
                .Returns(() => Task.FromResult((IList<string>)_list.Take(1).ToArray()));
            _searchPipelineMock = searchPipelineMockBuilder.Object;

            var propertyBuilderMockBuilder = new Moq.Mock<SimplifiedSearch.Utils.IPropertyBuilder>();
            propertyBuilderMockBuilder
                .Setup(x => x.BuildPropertyToSearchLambda<string>())
                .Returns(_fieldToSearch);
            _propertyBuilderMock = propertyBuilderMockBuilder.Object;
        }

        private SimplifiedSearchImpl GetImpl()
        {
            return new SimplifiedSearchImpl(_searchPipelineMock, _propertyBuilderMock);
        }

        [Fact]
        public void ConstructorBasic()
        {
            var actual = GetImpl();
            Assert.NotNull(actual);
        }

        [Fact]
        public void ConstructorPipelineNullThrows()
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var action = () => new SimplifiedSearchImpl(null, _propertyBuilderMock);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            Assert.Throws<ArgumentNullException>("searchPipeline", action);
        }

        [Fact]
        public void ConstructorPropertyBuilderNullThrows()
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var action = () => new SimplifiedSearchImpl(_searchPipelineMock, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            Assert.Throws<ArgumentNullException>("propertyBuilder", action);
        }

        [Fact]
        public async Task SearchListNullThrows()
        {
            var impl = GetImpl();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var action = async () => await impl.SimplifiedSearchAsync(null, "", _fieldToSearch);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            await Assert.ThrowsAsync<ArgumentNullException>("searchThisList", action);
        }

        [Fact]
        public async Task SearchTermNullOrEmptyReturnsSameList()
        {
            var impl = GetImpl();
            var actual = await impl.SimplifiedSearchAsync(_list, "");
            Assert.Same(_list, actual);
        }

        [Fact]
        public async Task PropertyBuilderIsCalled()
        {
            var propertyBuilderMockBuilder = new Moq.Mock<SimplifiedSearch.Utils.IPropertyBuilder>();

            var propertyBuilder = propertyBuilderMockBuilder.Object;
            var impl = new SimplifiedSearchImpl(_searchPipelineMock, propertyBuilder);

            var _ = await impl.SimplifiedSearchAsync(_list, "a");

            propertyBuilderMockBuilder.Verify(x => x.BuildPropertyToSearchLambda<string>(), Moq.Times.Once);
        }

        [Fact]
        public async Task SearchPipelineIsCalled()
        {
            var searchPipelineMockBuilder = new Moq.Mock<SearchPipelines.ISearchPipeline>();

            var searchPipeline = searchPipelineMockBuilder.Object;
            var impl = new SimplifiedSearchImpl(searchPipeline, _propertyBuilderMock);

            var _ = await impl.SimplifiedSearchAsync(_list, "a");

            searchPipelineMockBuilder.Verify(x => x.SearchAsync(_list, "a", _fieldToSearch), Moq.Times.Once);
        }

        [Fact]
        public async Task Impl3Args()
        {
            var impl = GetImpl();

            var actual = await impl.SimplifiedSearchAsync(_list, _searchTerm, _fieldToSearch);

            Assert.Single(actual, _list.First());
        }

        [Fact]
        public async Task Impl2Args()
        {
            var impl = GetImpl();

            var actual = await impl.SimplifiedSearchAsync(_list, _searchTerm);

            Assert.Single(actual, _list.First());
        }
    }
}
