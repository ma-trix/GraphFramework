using System.Linq;
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

        [Test]
        public void EmptyTwinGraphHasNoVertices()
        {
            TwinGraph tg = new TwinGraph();
            Assert.AreEqual(0, tg.Vertices.Count);
        }

        [Test]
        public void AddsTwinVertexToTwinGraph()
        {
            TwinGraph tg = new TwinGraph();
            TwinVertex tv = new TwinVertex(null, tg);
            tg.AddTwinVertex(tv);
            Assert.Contains(tv, tg.Vertices);
        }

        [Test]
        public void TwinGraphCreatedFromGraphHasAllTwinVerticesBasedOnPrecursors()
        {
            Graph g = new Graph();
            Vertex v1 = new Vertex();
            Vertex v2 = new Vertex();
            g.AddVertex(v1);
            g.AddVertex(v2);
            TwinGraph tg = new TwinGraph(g);
            Assert.AreEqual(2, tg.Vertices.Count);
            Assert.That(tg.Vertices.Count(v => v.Precursor.Guid == v1.Guid), Is.EqualTo(1));
            Assert.That(tg.Vertices.Count(v => v.Precursor.Guid == v2.Guid), Is.EqualTo(1));
        }

        [Test]
        public void NewTwinGraphHasNoArcs()
        {
            TwinGraph tg = new TwinGraph();
            Assert.That(tg.Arcs.Count, Is.EqualTo(0));
        }

        [Test]
        public void AddsEdgeToTwinGraph()
        {
            TwinGraph tg = new TwinGraph();
            TwinVertex tv1 = new TwinVertex(null, tg);
            TwinVertex tv2 = new TwinVertex(null, tg);
            tg.AddTwinVertex(tv1);
            tg.AddTwinVertex(tv2);
            tg.AddEdge(tv1, tv2, false);
            Assert.That(tg.Arcs.Count(arc => arc.Start.Guid == tv1.B.Guid && arc.End.Guid == tv2.A.Guid), Is.EqualTo(1));
            Assert.That(tg.Arcs.Count(arc => arc.Start.Guid == tv2.B.Guid && arc.End.Guid == tv1.A.Guid), Is.EqualTo(1));
        }

        [Test]
        public void AddsArcToTwinGraph()
        {
            TwinGraph tg = new TwinGraph();
            TwinVertex tv1 = new TwinVertex(null, tg);
            TwinVertex tv2 = new TwinVertex(null, tg);
            tg.AddTwinVertex(tv1);
            tg.AddTwinVertex(tv2);
            tg.AddArc(tv1, tv2, false);
            Assert.That(tg.Arcs.Count(arc => arc.Start.Guid == tv1.B.Guid && arc.End.Guid == tv2.A.Guid), Is.EqualTo(1));
            Assert.That(tg.Arcs.Count(arc => arc.Start.Guid == tv2.B.Guid && arc.End.Guid == tv1.A.Guid), Is.EqualTo(0));
        }

        [Test]
        public void TwinGraphCreatedFromGraphHasAllEdgesBasedOnPrecursors()
        {
            Graph g = new Graph();
            Vertex v1 = new Vertex();
            Vertex v2 = new Vertex();
            g.AddVertex(v1);
            g.AddVertex(v2);
            g.AddEdge(v1, v2);
            TwinGraph tg = new TwinGraph(g);
            TwinVertex tv1 = tg.Vertices.FirstOrDefault(tv => tv.Precursor.Guid == v1.Guid);
            TwinVertex tv2 = tg.Vertices.FirstOrDefault(tv => tv.Precursor.Guid == v2.Guid);
            Assert.That(tg.Arcs.Count(arc => arc.Start.Guid == tv1.B.Guid && arc.End.Guid == tv2.A.Guid), Is.EqualTo(1));
            Assert.That(tg.Arcs.Count(arc => arc.Start.Guid == tv2.B.Guid && arc.End.Guid == tv1.A.Guid), Is.EqualTo(1));
        }
    }
}
