using NUnit.Framework;

namespace GraphFramework
{
    [TestFixture]
    public class NodeUnitTest
    {
        private Node node1;
        private Node node2;

        [SetUp]
        public void Init()
        {
            node1 = new Node();
            node2 = new Node();
        }

        [Test]
        public void NewNodeHasNoNeighbours()
        {
            Node node = new Node();
            Assert.AreEqual(0, node.Neighbours.Count);
        }

        [Test]
        public void AddsEdgeToNode()
        {
            node1.AddEdge(node2);
            Assert.Contains(node2, node1.Neighbours);
        }

        [Test]
        public void AddingEdgeMakesNodesNeighboursOfEachOther()
        {
            node1.AddEdge(node2);
            Assert.Contains(node1, node2.Neighbours);
        }

        [Test]
        public void CanNotAddMultipleEdgesBetweenTwoNodes()
        {
            node1.AddEdge(node2);
            Assert.Throws<NoMultiedgePermitedException>(() => node1.AddEdge(node2));
        }

        [Test]
        public void AddsArcToNode()
        {
            node1.AddArc(node2);
            Assert.Contains(node2, node1.Neighbours);
        }

        [Test]
        public void AddingArcToNodeIsOneWayOnly()
        {
            node1.AddArc(node2);
            Assert.IsFalse(node2.Neighbours.Contains(node1));
        }
        
        [Test]
        public void CanNotAddArcIfStartIsAlreadyIncidentToEndNode()
        {
            node1.AddArc(node2);
            Assert.Throws<NoMultiedgePermitedException>(() => node1.AddArc(node2));
        }
    }
}