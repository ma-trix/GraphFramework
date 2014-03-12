using System;
using System.Linq;
using NUnit.Framework;

namespace GraphFramework
{
    [TestFixture]
    public class GraphUnitTests
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

        public class TheAddArcMethod : GraphUnitTests
        {
            [SetUp]
            public void DerivedInit()
            {
                base.Init();
                _graph.AddVertex(_v1);
                _graph.AddVertex(_v2);
            }

            [Test]
            public void AddsArcNotInMatchingBetweenVerticesToArcsInGraph()
            {
                var a = _graph.AddArc(_v1, _v2, false);
                Assert.That(_graph.arcs.Contains(a));
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
                Assert.That(_graph.arcs.Contains(a));
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
                base.Init();
                _graph.AddVertex(_v1);
                _graph.AddVertex(_v2);
                _arc = _graph.AddArc(_v1, _v2, false);
            }

            [Test]
            public void RemovesArcFromArcsInGraph()
            {
                _graph.RemoveArc(_v1, _v2);
                Assert.That(_graph.arcs.Contains(_arc), Is.False);
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

        public class EvertOtherTest : GraphUnitTests
        {
            [Test]
            public void AddsEdgeBetweenVertices()
            {
                _graph.AddVertex(_v1);
                _graph.AddVertex(_v2);
                _graph.AddEdge(_v1, _v2, false);
                Assert.IsTrue(ArcHelper.DoesArcExist(_v1, _v2, _graph.arcs));
                Assert.IsNotNull(_v1.OutboundArcs.Single(arc => arc.Start == _v1 && arc.End == _v2));
                Assert.IsNotNull(_v2.InboundArcs.Single(arc => arc.Start == _v1 && arc.End == _v2));
                Assert.AreSame(_graph.arcs.Single(arc => arc.Start == _v1 && arc.End == _v2), _v1.OutboundArcs.Single(arc => arc.Start == _v1 && arc.End == _v2));

                Assert.IsTrue(ArcHelper.DoesArcExist(_v2, _v1, _graph.arcs));
                Assert.IsNotNull(_v2.OutboundArcs.Single(arc => arc.Start == _v2 && arc.End == _v1));
                Assert.IsNotNull(_v1.InboundArcs.Single(arc => arc.Start == _v2 && arc.End == _v1));
                Assert.AreSame(_graph.arcs.Single(arc => arc.Start == _v2 && arc.End == _v1), _v2.OutboundArcs.Single(arc => arc.Start == _v2 && arc.End == _v1));
            }

            [Test]
            public void RemovesEdgeBetweenVertices()
            {
                _graph.AddVertex(_v1);
                _graph.AddVertex(_v2);
                _graph.AddEdge(_v1, _v2, false);
                _graph.RemoveEdge(_v1, _v2);

                Func<Arc, bool> arcV1V2 = arc => arc.Start == _v1 && arc.End == _v2;
                Assert.IsNull(_graph.arcs.FirstOrDefault(arcV1V2));
                Assert.IsNull(_v1.OutboundArcs.FirstOrDefault(arcV1V2));
                Assert.IsNull(_v2.InboundArcs.FirstOrDefault(arcV1V2));

                Func<Arc, bool> arcV2V1 = arc => arc.Start == _v2 && arc.End == _v1;
                Assert.IsNull(_graph.arcs.FirstOrDefault(arcV2V1));
                Assert.IsNull(_v1.OutboundArcs.FirstOrDefault(arcV2V1));
                Assert.IsNull(_v2.InboundArcs.FirstOrDefault(arcV2V1));
            }

            [Test]
            public void RemovingEdgeWhenOnlyArcExistsLeavesArcAndThrowsException()
            {
                _graph.AddVertex(_v1);
                _graph.AddVertex(_v2);
                _graph.AddArc(_v1, _v2, false);
                Func<Arc, bool> arcV1V2 = arc => arc.Start == _v1 && arc.End == _v2;
                Assert.Throws<NoArcException>(() => _graph.RemoveEdge(_v1, _v2));
                Assert.IsNotNull(_graph.arcs.FirstOrDefault(arcV1V2));
            }

            [Test]
            public void AddedVertexKnowsWhichGraphItBelongsTo()
            {
                _graph.AddVertex(_v1);
                Assert.AreSame(_graph, _v1.Graph);
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
                Assert.Throws<NoVertexException>(() => _graph.RemoveVertex(_v1));
            }

            [Test]
            public void RemovingVertexRemovesInboundArcs()
            {
                _graph.AddVertex(_v1);
                _graph.AddVertex(_v2);
                _v2.AddOutboundArc(_v1, false);
                _graph.RemoveVertex(_v1);
                Assert.AreEqual(0, _v2.OutboundArcs.Select(arc => arc.Start == _v1 && arc.End == _v1).Count());
            }

            [Test]
            public void RemovingVertexRemovesOutboundArcsToNeighbours()
            {
                _graph.AddVertex(_v1);
                _graph.AddVertex(_v2);
                _v1.AddOutboundArc(_v2, false);
                _graph.RemoveVertex(_v1);
                Assert.AreEqual(0, _v2.InboundArcs.Select(arc => arc.Start == _v1 && arc.End == _v2).Count());
            }

            [Test]
            public void RemovingVertexResetsItsGraphToNull()
            {
                _graph.AddVertex(_v1);
                _graph.RemoveVertex(_v1);
                Assert.IsNull(_v1.Graph);
            }

            [Test]
            public void NewGraphHasNoArcs()
            {
                Assert.AreEqual(0, _graph.arcs.Count());
            }


            [Test]
            public void VertexKnowsWhichGraphItIsAddedTo()
            {
                _graph.AddVertex(_v1);
                Assert.AreSame(_graph, _v1.Graph);
            }
        }
    }
}