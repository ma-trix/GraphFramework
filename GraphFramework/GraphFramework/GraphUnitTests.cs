using NUnit.Framework;

namespace GraphFramework
{
    class GraphUnitTests
    {
        [Test]
        public void EmptyGraphHasNoNodes()
        {
            Graph graph = new Graph();
            Assert.AreEqual(0, graph.nodes.Count);
        }


        [Test]
        public void AddsNodeToGraph()
        {
            Graph graph = new Graph();
            Node newNode = new Node();
            graph.AddNode(newNode);
            Assert.Contains(newNode, graph.nodes);
        }
    }
}