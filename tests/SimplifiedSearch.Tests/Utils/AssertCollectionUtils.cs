using SimplifiedSearch.Tests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SimplifiedSearch.Tests.Utils
{
    internal class AssertCollectionUtils
    {
        internal static void AssertCollectionContainsEqualIds(IEnumerable<TestItem> expected, IEnumerable<TestItem> actual)
        {
            var expectedIds = expected.Select(x => x.Id).ToArray();
            var actualIds = actual.Select(x => x.Id).ToArray();
            Assert.True(expectedIds.Length == actualIds.Length, $"Lists were different lengths. Expected: {expectedIds.Length}, Actual: {actualIds.Length}");
            var intersect = expectedIds.Intersect(actualIds).ToArray();
            Assert.True(intersect.Length == expectedIds.Length, $"Lists have different content. Expected: {expectedIds.Length}, Actual: {intersect.Length}");
        }

        internal static void AssertCollectionContainsSameInSameOrder<T>(IList<T> expected, IList<T> actual)
        {
            var len = expected.Count;
            Assert.True(len == actual.Count, $"Expected length: {len}, got: {actual.Count}");
            for (var i = 0; i < len; i++)
            {
                Assert.True(object.Equals(expected[i], actual[i]), $"Diff on index: {i}. Expected: {expected[i]}, Got: {actual[i]}");
            }
        }
    }
}
