using System;
using NUnit.Framework;

namespace GraphFramework
{
    [TestFixture]
    public class GraphUnitTests
    {
        private Graph _graph;
        private Vertex _v1;
        private Vertex _v2;

        public void Init()
        {
            _graph = new Graph();
            _v1 = new Vertex();
            _v2 = new Vertex();
        }

        public class TheConstructor : GraphUnitTests
        {
            [SetUp]
            public void DerivedInit()
            {
                Init();
            }

            [Test]
            public void EmptyGraphHasNoVertices()
            {
                Assert.That(_graph.Vertices.Count, Is.EqualTo(0));
            }

            [Test]
            public void NewGraphHasNoArcs()
            {
                Assert.That(_graph.Arcs, Is.Empty);
            }
        }

        public class TheAddVertexMethod : GraphUnitTests
        {
            [SetUp]
            public void DerivedInit()
            {
                Init();
                _graph.AddVertex(_v1);
            }

            [Test]
            public void AddedVertexKnowsWhichGraphItBelongsTo()
            {
                Assert.That(_v1.Graph, Is.SameAs(_graph));
            }

            [Test]
            public void AddsVertexToGraph()
            {
                Assert.That(_graph.Vertices, Contains.Item(_v1));
            }

            [Test]
            public void VertexKnowsWhichGraphItIsAddedTo()
            {
                Assert.That(_v1.Graph, Is.SameAs(_graph));
            }
        }

        public class TheAddArcMethod : GraphUnitTests
        {
            [SetUp]
            public void DerivedInit()
            {
                Init();
                _graph.AddVertex(_v1);
                _graph.AddVertex(_v2);
            }

            [Test]
            public void AddsArcNotInMatchingBetweenVerticesToArcsInGraph()
            {
                var a = _graph.AddArc(_v1, _v2, false);
                Assert.That(_graph.Arcs.Contains(a));
            }

            [Test]
            public void AddingArcBetweenVerticesAddsArcNotInMatchingToStartVertex()
            {
                var a = _graph.AddArc(_v1, _v2, false);
                Assert.That(_v1.OutboundArcs.Contains(a));
            }

            [Test]
            public void AddingArcBetweenVerticesAddsArcNotInMatchingToEndVertex()
            {
                var a = _graph.AddArc(_v1, _v2, false);
                Assert.That(_v2.InboundArcs.Contains(a));
            }

            [Test]
            public void AddsArcInMatchingBetweenVerticesToArcsInGraph()
            {
                var a = _graph.AddArc(_v1, _v2, true);
                Assert.That(_graph.Arcs.Contains(a));
            }

            [Test]
            public void AddingArcBetweenVerticesAddsArcInMatchingToStartVertex()
            {
                var a = _graph.AddArc(_v1, _v2, true);
                Assert.That(_v1.OutboundArcs.Contains(a));
            }

            [Test]
            public void AddingArcBetweenVerticesAddsArcInMatchingToEndVertex()
            {
                var a = _graph.AddArc(_v1, _v2, true);
                Assert.That(_v2.InboundArcs.Contains(a));
            }
            
            [Test]
            public void AddedArcKnowsWhichGrahpItBelongsTo()
            {
                var a = _graph.AddArc(_v1, _v2, false);
                Assert.That(a.Graph, Is.SameAs(_graph));
            }
        }

        public class TheRemoveArcMethod : GraphUnitTests
        {
            private Arc _arc;

            [SetUp]
            public void DerivedInit()
            {
                Init();
                _graph.AddVertex(_v1);
                _graph.AddVertex(_v2);
                _arc = _graph.AddArc(_v1, _v2, false);
            }

            [Test]
            public void RemovesArcFromArcsInGraph()
            {
                _graph.RemoveArc(_v1, _v2);
                Assert.That(_graph.Arcs.Contains(_arc), Is.False);
            }

            [Test]
            public void RemovesArcFromOutboundArcsInStartVertex()
            {
                _graph.RemoveArc(_v1, _v2);
                Assert.That(_v1.OutboundArcs.Contains(_arc), Is.False);
            }

            [Test]
            public void RemovesArcFromInboundArcsInEndVertex()
            {
                _graph.RemoveArc(_v1, _v2);
                Assert.That(_v2.InboundArcs.Contains(_arc), Is.False);
            }
        }

        public class TheAddEdgeMethod : GraphUnitTests
        {
            [SetUp]
            public void DerivedInit()
            {
                Init(); 
                _graph.AddVertex(_v1);
                _graph.AddVertex(_v2);
            }
            
            [Test]
            public void AddsEdgeNotInMatchingToArcsInGraph()
            {
                var e = _graph.AddEdge(_v1, _v2, false);
                Assert.That(_graph.Arcs.Contains(e.Item1));
                Assert.That(_graph.Arcs.Contains(e.Item2));
            }

            [Test]
            public void AddsEdgeNotInMatchingToArcsInStartVertex()
            {
                var e = _graph.AddEdge(_v1, _v2, false);
                Assert.That(_v1.OutboundArcs.Contains(e.Item1));
                Assert.That(_v1.InboundArcs.Contains(e.Item2));
            }

            [Test]
            public void AddsEdgeNotInMatchingToArcsInEndVertex()
            {
                var e = _graph.AddEdge(_v1, _v2, false);
                Assert.That(_v2.OutboundArcs.Contains(e.Item2));
                Assert.That(_v2.InboundArcs.Contains(e.Item1));
            }
        }

        public class TheRemoveEdgeMethod : GraphUnitTests
        {
            private Tuple<Arc, Arc> _e;
                
            [SetUp]
            public void DerivedInit()
            {
                Init();
                _graph.AddVertex(_v1);
                _graph.AddVertex(_v2);
                _e = _graph.AddEdge(_v1, _v2, false);
            }

            [Test]
            public void RemovesEdgeFromArcsInGraph()
            {
                _graph.RemoveEdge(_v1, _v2);
                Assert.That(_graph.Arcs.Contains(_e.Item1), Is.False);
                Assert.That(_graph.Arcs.Contains(_e.Item2), Is.False);
            }

            [Test]
            public void RemovesEdgeFromArcsInStartVertex()
            {
                _graph.RemoveEdge(_v1, _v2);
                Assert.That(_v1.OutboundArcs.Contains(_e.Item1), Is.False);
                Assert.That(_v1.InboundArcs.Contains(_e.Item2), Is.False);
            }

            [Test]
            public void RemovesEdgeFromArcsInEndVertex()
            {
                _graph.RemoveEdge(_v1, _v2);
                Assert.That(_v2.OutboundArcs.Contains(_e.Item2), Is.False);
                Assert.That(_v2.InboundArcs.Contains(_e.Item1), Is.False);
            }

            [Test]
            public void RemovingEdgeWhenOnlyArcExistsLeavesArc()
            {
                _graph.RemoveArc(_e.Item2.Start, _e.Item2.End);
                Assert.That(_graph.Arcs.Contains(_e.Item1));
            }

            [Test]
            public void RemovingEdgeWhenOnlyArcExistsThrowsException()
            {
                _graph.RemoveArc(_e.Item2.Start, _e.Item2.End);
                Assert.Throws<NoArcException>(() => _graph.RemoveEdge(_v1, _v2));
            }
        }

        public class TheRemoveVertexMethod : GraphUnitTests
        {
            [SetUp]
            public void DerivedInit()
            {
                Init();
                _graph.AddVertex(_v1);
                _graph.AddVertex(_v2);
            }

            [Test]
            public void RemovesVertexFromGraph()
            {
                _graph.RemoveVertex(_v1);
                Assert.That(_graph.Vertices, Has.No.Member(_v1));
            }

            [Test]
            public void RemovingNonexistentVertexThrowsException()
            {
                var v3 = new Vertex();
                Assert.Throws<NoVertexException>(() => _graph.RemoveVertex(v3));
            }

            [Test]
            public void RemovingVertexRemovesInboundArcs()
            {

                var a = _v2.AddOutboundArc(_v1, false);
                _graph.RemoveVertex(_v1);
                Assert.That(_v2.OutboundArcs, Has.No.Member(a));
            }

            [Test]
            public void RemovingVertexRemovesOutboundArcsToNeighbours()
            {
                var a =_v1.AddOutboundArc(_v2, false);
                _graph.RemoveVertex(_v1);
                Assert.That(_v2.InboundArcs, Has.No.Member(a));
            }

            [Test]
            public void RemovingVertexResetsItsGraphToNull()
            {
                _graph.RemoveVertex(_v1);
                Assert.That(_v1.Graph, Is.Null);
            }
        }
    }
}