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
        private Vertex _precursor;
        private TwinVertex _tv1;
        private TwinVertex _tv2;

        [SetUp]
        public void Init()
        {
            _tg = new TwinGraph();
            _g = new Graph();
            _v1 = new Vertex();
            _v2 = new Vertex();
            _precursor = new Vertex("p");
            _tv1 = new TwinVertex(_precursor, _tg);
            _tv2 = new TwinVertex(_precursor, _tg);
        }

        public class TheConstructor0 : TwinGraphUnitTests
        {
            [Test]
            public void NewTwinGraphHasStartVertex()
            {
                Assert.That(_tg.StartVertex, Is.Not.Null);
            }

            [Test]
            public void NewTwinGraphHasEndVertex()
            {
                Assert.That(_tg.EndVertex, Is.Not.Null);
            }

            [Test]
            public void NewTwinGraphHasNoVertices()
            {
                Assert.That(_tg.Vertices, Is.Empty);
            }

            [Test]
            public void NewTwinGraphHasNoArcs()
            {
                Assert.That(_tg.Arcs, Is.Empty);
            }
        }

        public class TheConstructor1 : TwinGraphUnitTests
        {
            private TwinGraph _tg;

            [SetUp]
            public void DerivedInit()
            {
                base.Init();
                _g.AddVertex(_v1);
                _g.AddVertex(_v2);
                _g.AddEdge(_v1, _v2, false);
                _tg = new TwinGraph(_g);
            }

            [Test]
            public void TwinGraphCreatedFromGraphHasAllEdgesBasedOnPrecursors()
            {
                var tv1 = _tg.Vertices.FirstOrDefault(tv => tv.Precursor.Guid == _v1.Guid);
                var tv2 = _tg.Vertices.FirstOrDefault(tv => tv.Precursor.Guid == _v2.Guid);
                Assert.That(tv1, Is.Not.Null);
                Assert.That(tv2, Is.Not.Null);
                Assert.That(ArcHelper.DoesArcExist(tv1.B, tv2.A, _tg.Arcs), Is.True);
                Assert.That(ArcHelper.DoesArcExist(tv2.B, tv1.A, _tg.Arcs), Is.True);
            }

            [Test]
            public void TwinGraphCreatedFromGraphHasAllTwinVerticesBasedOnPrecursors()
            {
                Assert.AreEqual(2, _tg.Vertices.Count);
                Assert.IsTrue(VertexHelper.DoesTwinVertexExist(_v1.Guid, _tg.Vertices));
                Assert.IsTrue(VertexHelper.DoesTwinVertexExist(_v2.Guid, _tg.Vertices));
            }
        }

        public class TheOtherMethods : TwinGraphUnitTests
        {
            [Test]
            public void AddsTwinVertexToTwinGraph()
            {
                _tg.AddTwinVertex(_tv1);
                Assert.Contains(_tv1, _tg.Vertices);
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
            public void RemovesTwinVertexFromTwinGraph()
            {
                _tg.AddTwinVertex(_tv1);
                Assert.That(_tg.Vertices.Contains(_tv1), Is.True);
                _tg.RemoveTwinVertex(_tv1);
                Assert.That(_tg.Vertices.Contains(_tv1), Is.False);
            }

            [Test]
            public void RemovesNonMatchingArcBetweenTwinVertices()
            {
                _tg.AddTwinVertex(_tv1);
                _tg.AddTwinVertex(_tv2);
                _tg.AddArc(_tv1, _tv2, false);
                _tg.RemoveArc(_tv1, _tv2, false);
                Assert.That(ArcHelper.DoesArcExist(_tv1.B, _tv2.A, _tg.Arcs), Is.False);
                Assert.That(ArcHelper.DoesArcExist(_tv1.B, _tv2.A, _tv1.B.OutboundArcs), Is.False);
                Assert.That(ArcHelper.DoesArcExist(_tv1.B, _tv2.A, _tv2.A.InboundArcs), Is.False);
            }

            [Test]
            public void RemovesInMatchingArcBetweenTwinVertices()
            {
                _tg.AddTwinVertex(_tv1);
                _tg.AddTwinVertex(_tv2);
                _tg.AddArc(_tv1, _tv2, true);
                _tg.RemoveArc(_tv1, _tv2, true);
                Assert.That(ArcHelper.DoesArcExist(_tv1.A, _tv2.B, _tg.Arcs), Is.False);
                Assert.That(ArcHelper.DoesArcExist(_tv1.A, _tv2.B, _tv1.A.OutboundArcs), Is.False);
                Assert.That(ArcHelper.DoesArcExist(_tv1.A, _tv2.B, _tv2.B.InboundArcs), Is.False);
            }

            [Test]
            public void RemovingNonexistentArcThrowsException()
            {
                Assert.Throws<NoArcException>(() => _tg.RemoveArc(_tv1, _tv2, false));
            }

            [Test]
            public void RemovingNonexistentTwinVertexThrowsException()
            {
                Assert.Throws<NoVertexException>(() => _tg.RemoveTwinVertex(_tv1));
            }

            [Test]
            public void RemovesNonMatchingEdgeBetweenTwinVertices()
            {
                _tg.AddTwinVertex(_tv1);
                _tg.AddTwinVertex(_tv2);
                _tg.AddEdge(_tv1, _tv2, false);
                _tg.RemoveEdge(_tv1, _tv2, false);
                Assert.That(ArcHelper.DoesArcExist(_tv1.B, _tv2.A, _tg.Arcs), Is.False);
                Assert.That(ArcHelper.DoesArcExist(_tv1.B, _tv2.A, _tv1.B.OutboundArcs), Is.False);
                Assert.That(ArcHelper.DoesArcExist(_tv1.B, _tv2.A, _tv2.A.InboundArcs), Is.False);
                Assert.That(ArcHelper.DoesArcExist(_tv2.B, _tv1.A, _tg.Arcs), Is.False);
                Assert.That(ArcHelper.DoesArcExist(_tv2.B, _tv1.A, _tv2.B.OutboundArcs), Is.False);
                Assert.That(ArcHelper.DoesArcExist(_tv2.B, _tv1.A, _tv1.A.InboundArcs), Is.False);
            }

            [Test]
            public void RemovesInMatchingEdgeBetweenTwinVertices()
            {
                _tg.AddTwinVertex(_tv1);
                _tg.AddTwinVertex(_tv2);
                _tg.AddEdge(_tv1, _tv2, true);
                _tg.RemoveEdge(_tv1, _tv2, true);
                Assert.That(ArcHelper.DoesArcExist(_tv1.A, _tv2.B, _tg.Arcs), Is.False);
                Assert.That(ArcHelper.DoesArcExist(_tv1.A, _tv2.B, _tv1.A.OutboundArcs), Is.False);
                Assert.That(ArcHelper.DoesArcExist(_tv1.A, _tv2.B, _tv2.B.InboundArcs), Is.False);
                Assert.That(ArcHelper.DoesArcExist(_tv2.A, _tv1.B, _tg.Arcs), Is.False);
                Assert.That(ArcHelper.DoesArcExist(_tv2.A, _tv1.B, _tv2.A.OutboundArcs), Is.False);
                Assert.That(ArcHelper.DoesArcExist(_tv2.A, _tv1.B, _tv1.B.InboundArcs), Is.False);
            }
        }
    }
}
