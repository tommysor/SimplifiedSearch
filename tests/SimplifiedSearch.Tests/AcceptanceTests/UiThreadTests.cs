using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimplifiedSearch.Tests.AcceptanceTests
{
    public class UiThreadTests
    {
        private readonly IList<string> _listToSearch;

        public UiThreadTests()
        {
            _listToSearch = new List<string>();
            for (var i = 0; i < 100; i++)
            {
                _listToSearch.Add("abcdefghijklmnopqrstuvwxyz");
            }
        }

        private static async Task RunAndAddAfterSearch(List<string> listToAddMessageTo, Task<IList<string>> searchTask)
        {
            await searchTask;
            listToAddMessageTo.Add("after search");
        }

        private async Task<bool> IsRunAsync()
        {
            // This only fails if the entire search runs synchronously.

            var list = new List<string>(2);
            var searchTask = _listToSearch.SimplifiedSearchAsync("ash dufgasuydigasuy dfguyiasfhjkas dygi asdygu aysgudtyausd fuytasd fjghasdfgujasfdtuy dasfda s addsf as d");
            var task2 = RunAndAddAfterSearch(list, searchTask);
            list.Add("first");
            await task2;

            return list[0] == "first";
        }

        [Fact]
        public async Task SearchReallyRunsOnSeparateThread()
        {
            // This test depends on the order of execution of 2 tasks.
            // Should work most of the time.

            for (var i = 0; i < 20; i++)
            {
                var actual = await IsRunAsync();
                if (actual)
                    return;
            }

            Assert.True(false, "Search is running synchronously");
        }
    }
}
