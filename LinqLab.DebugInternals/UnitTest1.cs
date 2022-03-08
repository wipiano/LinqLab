using System.Diagnostics;
using System.Linq;
using Xunit;

namespace LinqLab.DebugInternals
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            if (Enumerable.Empty<int>() is IPartition<int>)
            {
                Assert.Equal(true, true);
            }
        }
    }
}
