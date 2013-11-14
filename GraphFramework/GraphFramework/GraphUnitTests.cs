using NUnit.Framework;

namespace GraphFramework
{
    class GraphUnitTests
    {
        private Graph _graph;
        private Vertex _newVertex;

        [SetUp]
        public void Init()
        {
            _graph = new Graph();
            _newVertex = new Vertex();
        }

        [Test]
        public void EmptyGraphHasNoVertices()
        {
            Assert.AreEqual(0, _graph.vertices.Count);
        }


        [Test]
        public void AddsVertexToGraph()
        {
            _graph.AddVertex(_newVertex);
            Assert.Contains(_newVertex, _graph.vertices);
        }

        [Test]
        public void RemovesVertexFromGraph()
        {
            _graph.AddVertex(_newVertex);
            Assert.IsTrue(_graph.vertices.Contains(_newVertex));
            _graph.RemoveVertex(_newVertex);
            Assert.IsFalse(_graph.vertices.Contains(_newVertex));
        }
    }
}