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

        [Test]
        public void RemovesNodeFromGraph()
        {
            Graph graph = new Graph();
            Node newNode = new Node();
            graph.AddNode(newNode);
            Assert.IsTrue(graph.nodes.Contains(newNode));
            graph.RemoveNode(newNode);
            Assert.IsFalse(graph.nodes.Contains(newNode));
        }
    }
}