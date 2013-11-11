using NUnit.Framework;

namespace GraphFramework
{
    public class NodeUnitTest
    {
        [Test]
        public void NewNodeHasNoNeighbours()
        {
            Node node = new Node();
            Assert.AreEqual(0, node.Neighbours.Count);
        }

        [Test]
        public void AddsEdgeToNode()
        {
            Node node1 = new Node();
            Node node2 = new Node();
            node1.AddEdge(node2);
            Assert.Contains(node2, node1.Neighbours);
        }

        [Test]
        public void AddingEdgeMakesNodesNeighboursOfEachOther()
        {
            Node node1 = new Node();
            Node node2 = new Node();
            node1.AddEdge(node2);
            Assert.Contains(node1, node2.Neighbours);
        }
    }
}