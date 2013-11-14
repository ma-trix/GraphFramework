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
        [Ignore]
        public void RemovingVertexRemovesInboundArcs()
        {
            _graph.AddVertex(_v1);
            _graph.AddVertex(_v2);
            _v2.AddArc(_v1);
            _graph.RemoveVertex(_v1);
            Assert.IsFalse(_v2.Neighbours.Contains(_v1));
        }
    }
}