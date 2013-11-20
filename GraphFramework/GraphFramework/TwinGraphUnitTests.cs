using NUnit.Framework;

namespace GraphFramework
{
    [TestFixture]
    public class TwinGraphUnitTests
    {
        [Test]
        public void NewTwinGraphHasStartVertex()
        {
            TwinGraph tg = new TwinGraph();
            Assert.NotNull(tg.StartVertex);
        }

        [Test]
        public void NewTwinGraphHasEndVertex()
        {
            TwinGraph tg = new TwinGraph();
            Assert.NotNull(tg.EndVertex);
        }
    }
}
