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

        public class TheAddTwinVertexMethod : TwinGraphUnitTests
        {
            [SetUp]
            public void DerivedInit()
            {
                base.Init();
            }

            [Test]
            public void AddsTwinVertexToTwinGraph()
            {
                _tg.AddTwinVertex(_tv1);
                Assert.That(_tg.Vertices, Has.Member(_tv1));
            }
        }

        public class TheAddArcMethod : TwinGraphUnitTests
        {
            private Arc _a;

            [SetUp]
            public void DerivedInit()
            {
                base.Init();
                _tg.AddTwinVertex(_tv1);
                _tg.AddTwinVertex(_tv2);
            }

            [Test]
            public void AddsArcInMatchingToTwinGraph()
            {
                _a = _tg.AddArc(_tv1, _tv2, true);
                Assert.That(_tg.Arcs, Has.Member(_a));
            }

            [Test]
            public void AddsArcNotMatchingToTwinGraph()
            {
                _a = _tg.AddArc(_tv1, _tv2, false);
                Assert.That(_tg.Arcs, Has.Member(_a));
            }
        }

        public class TheAddEdgeMethod : TwinGraphUnitTests
        {
            [SetUp]
            public void DerivedInit()
            {
                base.Init();
                _tg.AddTwinVertex(_tv1);
                _tg.AddTwinVertex(_tv2);
            }
            
            [Test]
            public void AddsEdgeInMatchingToTwinGraph()
            {
                var e = _tg.AddEdge(_tv1, _tv2, true);
                Assert.That(_tg.Arcs, Has.Member(e.Item1));
                Assert.That(_tg.Arcs, Has.Member(e.Item2));
            }
            
            [Test]
            public void AddsEdgeNotInMatchingToTwinGraph()
            {
                var e = _tg.AddEdge(_tv1, _tv2, false);
                Assert.That(_tg.Arcs, Has.Member(e.Item1));
                Assert.That(_tg.Arcs, Has.Member(e.Item2));
            }
            
        }
        
        public class TheRemoveTwinVertexMethod : TwinGraphUnitTests
        {
            [SetUp]
            public void DerivedInit()
            {
                base.Init();
                _tg.AddTwinVertex(_tv1);
            }

            [Test]
            public void RemovesTwinVertexFromTwinGraph()
            {
                _tg.RemoveTwinVertex(_tv1);
                Assert.That(_tg.Vertices, Has.No.Member(_tv1));
            }

            [Test]
            public void RemovingNonexistentTwinVertexThrowsException()
            {
                Assert.Throws<NoVertexException>(() => _tg.RemoveTwinVertex(_tv2));
            }
        }

        public class TheRemoveArcMethod : TwinGraphUnitTests
        {
            private Arc _aNotInMatching;
            private Arc _aInMatching;

            [SetUp]
            public void DerivedInit()
            {
                base.Init();
                _tg.AddTwinVertex(_tv1);
                _tg.AddTwinVertex(_tv2);
                _aNotInMatching = _tg.AddArc(_tv2, _tv1, false);
                _aInMatching = _tg.AddArc(_tv1, _tv2, true);
            }

            [Test]
            public void RemovesNonMatchingArcBetweenTwinVerticesFromTwinGraph()
            {
                
                _tg.RemoveArc(_tv2, _tv1, false);
                Assert.That(_tg.Arcs, Has.No.Member(_aNotInMatching));
            }
            
            [Test]
            public void RemovesNonMatchingArcBetweenTwinVerticesFromStartVertex()
            {
                _tg.RemoveArc(_tv2, _tv1, false);
                Assert.That(_tv2.B.OutboundArcs, Has.No.Member(_aNotInMatching));
            }
            
            [Test]
            public void RemovesNonMatchingArcBetweenTwinVerticesFromEndVertex()
            {
                _tg.RemoveArc(_tv2, _tv1, false);
                Assert.That(_tv1.A.InboundArcs, Has.No.Member(_aNotInMatching));
            }

            [Test]
            public void RemovesInMatchingArcBetweenTwinVerticesFromTwinGraph()
            {
                _tg.RemoveArc(_tv1, _tv2, true);
                Assert.That(_tg.Arcs, Has.No.Member(_aInMatching));
            }
            
            [Test]
            public void RemovesInMatchingArcBetweenTwinVerticesFromStartVertex()
            {
                _tg.RemoveArc(_tv1, _tv2, true);
                Assert.That(_tv1.A.OutboundArcs, Has.No.Member(_aInMatching));
            }

            [Test]
            public void RemovesInMatchingArcBetweenTwinVerticesFromEndVertex()
            {
                _tg.RemoveArc(_tv1, _tv2, true);
                Assert.That(_tv2.B.InboundArcs, Has.No.Member(_aInMatching));
            }

            [Test]
            public void RemovingNonexistentArcThrowsException()
            {
                Assert.Throws<NoArcException>(() => _tg.RemoveArc(_tv1, _tv2, false));
            }
        }

        public class TheRemoveEdgeMethod : TwinGraphUnitTests
        {
            [SetUp]
            public void DerivedInit()
            {
                base.Init();
                _tg.AddTwinVertex(_tv1);
                _tg.AddTwinVertex(_tv2);
            }

            [Test]
            public void RemovesNonMatchingEdgeBetweenTwinVerticesFromTwinGraph()
            {

                var eNotInMatching = _tg.AddEdge(_tv1, _tv2, false);
                _tg.RemoveEdge(_tv1, _tv2, false);
                Assert.That(_tg.Arcs, Has.No.Member(eNotInMatching.Item1));
                Assert.That(_tg.Arcs, Has.No.Member(eNotInMatching.Item1));
            }

            [Test]
            public void RemovesNonMatchingEdgeBetweenTwinVerticesFromStartVertex()
            {

                var eNotInMatching = _tg.AddEdge(_tv1, _tv2, false);
                _tg.RemoveEdge(_tv1, _tv2, false);
                Assert.That(_tv1.B.OutboundArcs, Has.No.Member(eNotInMatching.Item1));
                Assert.That(_tv1.A.InboundArcs, Has.No.Member(eNotInMatching.Item1));
            }

            [Test]
            public void RemovesNonMatchingEdgeBetweenTwinVerticesFromEndVertex()
            {

                var eNotInMatching = _tg.AddEdge(_tv1, _tv2, false);
                _tg.RemoveEdge(_tv1, _tv2, false);
                Assert.That(_tv2.A.InboundArcs, Has.No.Member(eNotInMatching.Item2));
                Assert.That(_tv2.B.OutboundArcs, Has.No.Member(eNotInMatching.Item2));
            }

            [Test]
            public void RemovesInMatchingEdgeBetweenTwinVerticesFromTwinGraph()
            {
                var eInMatching = _tg.AddEdge(_tv1, _tv2, true);
                _tg.RemoveEdge(_tv1, _tv2, true);
                Assert.That(_tg.Arcs, Has.No.Member(eInMatching.Item1));
                Assert.That(_tg.Arcs, Has.No.Member(eInMatching.Item2));
            }

            [Test]
            public void RemovesInMatchingEdgeBetweenTwinVerticesFromStartVertex()
            {
                var eInMatching = _tg.AddEdge(_tv1, _tv2, true);
                _tg.RemoveEdge(_tv1, _tv2, true);
                Assert.That(_tv1.A.OutboundArcs, Has.No.Member(eInMatching.Item1));
                Assert.That(_tv1.B.InboundArcs, Has.No.Member(eInMatching.Item2));
            }

            [Test]
            public void RemovesInMatchingEdgeBetweenTwinVerticesFromEndVertex()
            {
                var eInMatching = _tg.AddEdge(_tv1, _tv2, true);
                _tg.RemoveEdge(_tv1, _tv2, true);
                Assert.That(_tv2.B.InboundArcs, Has.No.Member(eInMatching.Item1));
                Assert.That(_tv2.A.OutboundArcs, Has.No.Member(eInMatching.Item2));
            }
        }
    }
}
