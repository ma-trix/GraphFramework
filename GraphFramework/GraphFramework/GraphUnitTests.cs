using NUnit.Framework;

namespace GraphFramework
{
    class GraphUnitTests
    {
        [Test]
        public void EmptyGraphHasNoNodes()
        {
            Graph graph = new Graph();
            Assert.AreEqual(graph.nodes.Count, 0);
        }
    }
}
