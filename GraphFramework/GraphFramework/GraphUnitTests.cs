using System.Linq;
using NUnit.Framework;

namespace GraphFramework
{
    class GraphUnitTests
    {
        private Graph _graph;
        private Vertex _v1;
        private Vertex _v2;

        [SetUp]
        public void Init()
        {
            _graph = new Graph();
            _v1 = new Vertex();
            _v2 = new Vertex();
        }

        [Test]
        public void EmptyGraphHasNoVertices()
        {
            Assert.AreEqual(0, _graph.vertices.Count);
        }


        [Test]
        public void AddsVertexToGraph()
        {
            _graph.AddVertex(_v1);
            Assert.Contains(_v1, _graph.vertices);
        }

        [Test]
        public void RemovesVertexFromGraph()
        {
            _graph.AddVertex(_v1);
            Assert.IsTrue(_graph.vertices.Contains(_v1));
            _graph.RemoveVertex(_v1);
            Assert.IsFalse(_graph.vertices.Contains(_v1));
        }

        [Test]
        public void RemovingNonexistentVertexThrowsException()
        {
            Assert.Throws<NoVertexException>( () => _graph.RemoveVertex(_v1));
        }

        [Test]
        public void RemovingVertexRemovesInboundArcs()
        {
            _graph.AddVertex(_v1);
            _graph.AddVertex(_v2);
            _v2.AddArc(_v1);
            _graph.RemoveVertex(_v1);
            Assert.AreEqual(0, _v2.OutboundArcs.Select(arc => arc.Start == _v1 && arc.End == _v1).Count());
        }

        [Test]
        public void RemovingVertexRemovesOutboundArcsToNeighbours()
        {
            _graph.AddVertex(_v1);
            _graph.AddVertex(_v2);
            _v1.AddArc(_v2);
            _graph.RemoveVertex(_v1);
            Assert.IsFalse(_v2.Inbound.Contains(_v1));
            Assert.AreEqual(0, _v2.InboundArcs.Select(arc => arc.Start == _v1 && arc.End == _v2).Count());
        }
    }
}