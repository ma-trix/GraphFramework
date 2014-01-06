using System.Linq;
using NUnit.Framework;

namespace GraphFramework
{
    [TestFixture]
    public class TwinGraphUnitTests
    {
        private TwinGraph _tg;
        private Graph _g;
        private Vertex _v1;
        private Vertex _v2;
        private TwinVertex _tv1;
        private TwinVertex _tv2;

        [SetUp]
        public void SetUp()
        {
            _tg = new TwinGraph();
            _g = new Graph();
            _v1 = new Vertex();
            _v2 = new Vertex();
            _tv1 = new TwinVertex(null, _tg);
            _tv2 = new TwinVertex(null, _tg);
        }

        [Test]
        public void NewTwinGraphHasStartVertex()
        {
            Assert.NotNull(_tg.StartVertex);
        }

        [Test]
        public void NewTwinGraphHasEndVertex()
        {
            Assert.NotNull(_tg.EndVertex);
        }

        [Test]
        public void EmptyTwinGraphHasNoVertices()
        {
            Assert.AreEqual(0, _tg.Vertices.Count);
        }

        [Test]
        public void AddsTwinVertexToTwinGraph()
        {
            _tg.AddTwinVertex(_tv1);
            Assert.Contains(_tv1, _tg.Vertices);
        }

        [Test]
        public void TwinGraphCreatedFromGraphHasAllTwinVerticesBasedOnPrecursors()
        {
            _g.AddVertex(_v1);
            _g.AddVertex(_v2);
            var tg = new TwinGraph(_g);
            Assert.AreEqual(2, tg.Vertices.Count);
            Assert.IsTrue(VertexHelper.DoesTwinVertexExist(_v1.Guid, tg.Vertices));
            Assert.IsTrue(VertexHelper.DoesTwinVertexExist(_v2.Guid, tg.Vertices));
        }

        [Test]
        public void NewTwinGraphHasNoArcs()
        {
            Assert.That(_tg.Arcs.Count, Is.EqualTo(0));
        }

        [Test]
        public void AddsEdgeToTwinGraph()
        {
            _tg.AddTwinVertex(_tv1);
            _tg.AddTwinVertex(_tv2);
            _tg.AddEdge(_tv1, _tv2, false);
            Assert.That(ArcHelper.DoesArcExist(_tv1.B, _tv2.A, _tg.Arcs), Is.True);
            Assert.That(ArcHelper.DoesArcExist(_tv2.B, _tv1.A, _tg.Arcs), Is.True);
        }

        [Test]
        public void AddsArcToTwinGraph()
        {
            _tg.AddTwinVertex(_tv1);
            _tg.AddTwinVertex(_tv2);
            _tg.AddArc(_tv1, _tv2, false);
            Assert.That(ArcHelper.DoesArcExist(_tv1.B, _tv2.A, _tg.Arcs), Is.True);
            Assert.That(ArcHelper.DoesArcExist(_tv2.B, _tv1.A, _tg.Arcs), Is.False);
        }

        [Test]
        public void TwinGraphCreatedFromGraphHasAllEdgesBasedOnPrecursors()
        {
            _g.AddVertex(_v1);
            _g.AddVertex(_v2);
            _g.AddEdge(_v1, _v2);
            var tg = new TwinGraph(_g);
            var tv1 = tg.Vertices.FirstOrDefault(tv => tv.Precursor.Guid == _v1.Guid);
            var tv2 = tg.Vertices.FirstOrDefault(tv => tv.Precursor.Guid == _v2.Guid);
            Assert.That(tv1, Is.Not.Null);
            Assert.That(tv2, Is.Not.Null);
            Assert.That(ArcHelper.DoesArcExist(tv1.B, tv2.A, tg.Arcs), Is.True);
            Assert.That(ArcHelper.DoesArcExist(tv2.B, tv1.A, tg.Arcs), Is.True);
        }

        [Test]
        public void RemovesTwinVertexFromTwinGraph()
        {
            _tg.AddTwinVertex(_tv1);
            Assert.That(_tg.Vertices.Contains(_tv1), Is.True);
            _tg.RemoveTwinVertex(_tv1);
            Assert.That(_tg.Vertices.Contains(_tv1), Is.False);
        }

        [Test]
        public void RemovesArcBetweenTwinVertices()
        {
            _tg.AddTwinVertex(_tv1);
            _tg.AddTwinVertex(_tv2);
            _tg.AddArc(_tv1, _tv2, false);
            Assert.That(ArcHelper.DoesArcExist(_tv1.B, _tv2.A, _tg.Arcs), Is.True);
            Assert.That(ArcHelper.DoesArcExist(_tv1.B, _tv2.A, _tv1.B.OutboundArcs), Is.True);
            Assert.That(ArcHelper.DoesArcExist(_tv1.B, _tv2.A, _tv2.A.InboundArcs), Is.True);
            _tg.RemoveArc(_tv1, _tv2, false);
            Assert.That(ArcHelper.DoesArcExist(_tv1.B, _tv2.A, _tg.Arcs), Is.False);
            Assert.That(ArcHelper.DoesArcExist(_tv1.B, _tv2.A, _tv1.B.OutboundArcs), Is.False);
            Assert.That(ArcHelper.DoesArcExist(_tv1.B, _tv2.A, _tv2.A.InboundArcs), Is.False);
        }
    }
}
