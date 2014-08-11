using System.Linq;
using NUnit.Framework;

namespace GraphFramework.UnitTests.VertexUnitTests
{
    public abstract class VertexUnitTest
    {
        private Vertex _v1;
        private Vertex _v2;

        [SetUp]
        public void Init()
        {
            _v1 = new Vertex();
            _v2 = new Vertex();
        }

        [TestFixture]
        public class TheConstructor : VertexUnitTest
        {
            [Test]
            public void NewVertexHasOutDegreeZero()
            {
                Assert.AreEqual(0, _v1.OutDegree);
            }

            [Test]
            public void NewVertexHasInDegreeZero()
            {
                Assert.AreEqual(0, _v1.InDegree);
            }

            [Test]
            public void NewVertexDoesntBelongToAnyGraph()
            {
                Assert.IsNull(_v1.Graph);
            }

            [Test]
            public void NewVertexHasGuid()
            {
                Assert.IsNotNull(_v1.Guid);
            }

            [Test]
            public void DefualtNameIsGuid()
            {
                Assert.That(_v1.Name, Is.EqualTo(_v1.Guid.ToString()));
            }

            [Test]
            public void NamesAVertex()
            {
                var v = new Vertex("1");
                Assert.That(v.Name, Is.EqualTo("1"));
            }

            [Test]
            public void NewVertexHasZeroOutboundEdges()
            {
                Assert.AreEqual(0, _v1.OutboundArcs.Count);
            }

            [Test]
            public void NewVertexHasZeroInboundEdges()
            {
                Assert.AreEqual(0, _v1.InboundArcs.Count);
            }

            [Test]
            public void NewVertexIsNotInMatching()
            {
                Assert.That(_v1.IsInMatching, Is.False);
            }
        }

        [TestFixture]
        public class TheAddOutboundArcMethod : VertexUnitTest
        {
            [Test]
            public void AddsArcToStartVertexAsOutbound()
            {
                Arc a = _v1.AddOutboundArc(_v2, false);
                Assert.That(_v1.OutboundArcs.Contains(a), Is.True);
            }

            [Test]
            public void AddsArcToEndVertexAsInbound()
            {
                Arc a = _v1.AddOutboundArc(_v2, false);
                Assert.That(_v2.InboundArcs.Contains(a), Is.True);
            }

            [Test]
            public void AddingArcToVertexIsOneWayOnly()
            {
                _v1.AddOutboundArc(_v2, false);
                Assert.That(_v1.InDegree, Is.EqualTo(0));
                Assert.That(_v2.OutDegree, Is.EqualTo(0));
            }

            [Test]
            public void CanNotAddArcIfStartVertexIsAlreadyConsecutiveToEndVertex()
            {
                _v1.AddOutboundArc(_v2, false);
                Assert.Throws<NoMultiedgePermitedException>(() => _v1.AddOutboundArc(_v2, false));
            }

            [Test]
            public void VertexKnowsItsNonzeroOutDegree()
            {
                _v1.AddOutboundArc(_v2, false);
                Assert.AreEqual(1, _v1.OutDegree);
            }

            [Test]
            public void AddingInboundVertexIncreasesInDegree()
            {
                _v2.AddOutboundArc(_v1, false);
                Assert.AreEqual(1, _v1.InDegree);
            }

            [Test]
            public void VerticesWithArcBetweenThemReferToTheSameObject()
            {
                _v1.AddOutboundArc(_v2, false);
                Assert.AreSame(_v1.OutboundArcs.First(arc => arc.Start == _v1 && arc.End == _v2),
                                _v2.InboundArcs.First(arc => arc.Start == _v1 && arc.End == _v2));
            }

            [Test]
            public void AddsArcInMatchingToOutboundArcsInStartVertex()
            {
                _v1.AddOutboundArc(_v2, true);
                var a = ArcHelper.FindArc(_v1, _v2, _v1.OutboundArcs);
                Assert.That(a, Is.Not.Null);
                Assert.That(a.IsInMatching, Is.True);
            }

            [Test]
            public void AddsArcInMatchingToInboundArcsInEndVertex()
            {
                _v1.AddOutboundArc(_v2, true);
                var a = ArcHelper.FindArc(_v1, _v2, _v2.InboundArcs);
                Assert.That(a, Is.Not.Null);
                Assert.That(a.IsInMatching, Is.True);
            }

            [Test]
            public void AddingArcInMatchingMakesStartVertexInMatching()
            {
                _v1.AddOutboundArc(_v2, true);
                Assert.That(_v1.IsInMatching, Is.True);
            }

            [Test]
            public void AddingArcInMatchingMakesEndVertexInMatching()
            {
                _v1.AddOutboundArc(_v2, true);
                Assert.That(_v2.IsInMatching, Is.True);
            }

            [Test]
            public void AddsArcNotInMatchingToOutboundArcsInStartVertex()
            {
                _v1.AddOutboundArc(_v2, false);
                var a = ArcHelper.FindArc(_v1, _v2, _v1.OutboundArcs);
                Assert.That(a, Is.Not.Null);
                Assert.That(a.IsInMatching, Is.False);
            }

            [Test]
            public void AddsArcNotInMatchingToInboundArcsInEndVertex()
            {
                _v1.AddOutboundArc(_v2, false);
                var a = ArcHelper.FindArc(_v1, _v2, _v2.InboundArcs);
                Assert.That(a, Is.Not.Null);
                Assert.That(a.IsInMatching, Is.False);
            }

            [Test]
            public void AddingArcNotInMatchingMakesStartVertexInMatching()
            {
                _v1.AddOutboundArc(_v2, false);
                Assert.That(_v1.IsInMatching, Is.False);
            }

            [Test]
            public void AddingArcNotInMatchingMakesEndVertexInMatching()
            {
                _v1.AddOutboundArc(_v2, false);
                Assert.That(_v2.IsInMatching, Is.False);
            }
        }

        [TestFixture]
        public class TheRemoveArcMethod : VertexUnitTest
        {
            [Test]
            public void CanRemoveArcBetweenTwoVertices()
            {
                _v1.AddOutboundArc(_v2, false);
                _v1.RemoveArc(_v2);
                Assert.AreEqual(0, _v1.OutboundArcs.Select(arc => (arc.Start == _v1 && arc.End == _v2)).Count());
                Assert.AreEqual(0, _v2.InboundArcs.Select(arc => (arc.Start == _v1 && arc.End == _v2)).Count());
            }

            [Test]
            public void RemovingNonExistentArcThrowsException()
            {
                Assert.Throws<NoArcException>(() => _v1.RemoveArc(_v2));
            }
        }

        [TestFixture]
        public class TheAddEdgeMethod : VertexUnitTest
        {
            [Test]
            public void AddingEdgeMakesVerticesNeighboursOfEachOther()
            {
                _v1.AddEdge(_v2, false);
                Assert.AreEqual(1, _v1.OutboundArcs.Select(arc => arc.Start == _v1 && arc.End == _v2).Count());
                Assert.AreEqual(1, _v1.InboundArcs.Select(arc => arc.Start == _v1 && arc.End == _v2).Count());
                Assert.AreEqual(1, _v2.OutboundArcs.Select(arc => arc.Start == _v1 && arc.End == _v2).Count());
                Assert.AreEqual(1, _v2.InboundArcs.Select(arc => arc.Start == _v1 && arc.End == _v2).Count());
            }

            [Test]
            public void CanNotAddMultipleEdgesBetweenTwoVertices()
            {
                _v1.AddEdge(_v2, false);
                Assert.Throws<NoMultiedgePermitedException>(() => _v1.AddEdge(_v2, false));
            }

            [Test]
            public void CanNotAddEdgeIfEndVertexIsAlreadyConsecutiveToStartVertex()
            {
                _v2.AddOutboundArc(_v1, false);
                Assert.Throws<NoMultiedgePermitedException>(() => _v1.AddEdge(_v2, false));
            }
        }

        [TestFixture]
        public class TheRemoveEdgeMethod : VertexUnitTest
        {
            [Test]
            public void CanRemoveEdgeBetweenTwoVertices()
            {
                _v1.AddEdge(_v2, false);
                _v1.RemoveEdge(_v2);
                Assert.AreEqual(0, _v1.OutboundArcs.Select(arc => (arc.Start == _v1 && arc.End == _v2)).Count());
                Assert.AreEqual(0, _v1.InboundArcs.Select(arc => (arc.Start == _v1 && arc.End == _v2)).Count());
                Assert.AreEqual(0, _v2.OutboundArcs.Select(arc => (arc.Start == _v1 && arc.End == _v2)).Count());
                Assert.AreEqual(0, _v2.InboundArcs.Select(arc => (arc.Start == _v1 && arc.End == _v2)).Count());
            }

            [Test]
            public void RemovingNonExistenEdgeThrowsException()
            {
                Assert.Throws<NoArcException>(() => _v1.RemoveEdge(_v2));
            }

            [Test]
            public void RemovingEdgeWhenArcFromEndVertexToStartVertexExistsThrowsException()
            {
                _v2.AddOutboundArc(_v1, false);
                Assert.Throws<NoArcException>(() => _v1.RemoveEdge(_v2));
            }
        }

        [TestFixture]
        public class TheArcRevertedMethod : VertexUnitTest
        {
            [Test]
            public void ArcRevertedAddsNonMatchingVertexToMatching()
            {
                var a = _v2.AddOutboundArc(_v1, false);
                a.Revert();
                Assert.That(_v1.IsInMatching, Is.True);
            }

            [Test]
            public void InboundArcRevertedIsAddedToOutboundArcsOfOldEndVertex()
            {
                var a = _v2.AddOutboundArc(_v1, false);
                a.Revert();
                Assert.That(_v1.OutboundArcs, Has.Exactly(1).SameAs(a));
            }

            [Test]
            public void InboundArcRevertedIsRemovedFromInboundArcsOfOldEndVertex()
            {
                var a = _v2.AddOutboundArc(_v1, false);
                a.Revert();
                Assert.That(_v1.InboundArcs, Has.No.Member(a));
            }

            [Test]
            public void InboundArcRevertedIsAddedToInbounArcsOfOldStartVertex()
            {
                var a = _v2.AddOutboundArc(_v1, false);
                a.Revert();
                Assert.That(_v2.InboundArcs, Has.Exactly(1).SameAs(a));
            }

            [Test]
            public void InboundArcRevertedIsRemovedFromOutboundArcsOfOldStartVertex()
            {
                var a = _v2.AddOutboundArc(_v1, false);
                a.Revert();
                Assert.That(_v2.OutboundArcs, Has.No.Member(a));
            }
        }
    }
}