using NUnit.Framework;

namespace GraphFramework
{
    class GraphUnitTests
    {
        [Test]
        public void EmptyGraphHasNoVertices()
        {
            Graph graph = new Graph();
            Assert.AreEqual(0, graph.vertices.Count);
        }


        [Test]
        public void AddsVertexToGraph()
        {
            Graph graph = new Graph();
            Vertex newVertex = new Vertex();
            graph.AddVertex(newVertex);
            Assert.Contains(newVertex, graph.vertices);
        }

        [Test]
        public void RemovesVertexFromGraph()
        {
            Graph graph = new Graph();
            Vertex newVertex = new Vertex();
            graph.AddVertex(newVertex);
            Assert.IsTrue(graph.vertices.Contains(newVertex));
            graph.RemoveVertex(newVertex);
            Assert.IsFalse(graph.vertices.Contains(newVertex));
        }
    }
}