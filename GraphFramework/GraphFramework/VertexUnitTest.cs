using System.Linq;
using NUnit.Framework;

namespace GraphFramework
{
    [TestFixture]
    public class VertexUnitTest
    {
        private Vertex _v1;
        private Vertex _v2;

        [SetUp]
        public void Init()
        {
            _v1 = new Vertex();
            _v2 = new Vertex();
        }

        [Test]
        public void NewVertexHasZeroOutboundNeighbours()
        {
            Assert.AreEqual(0, _v1.Outbound.Count);
        }

        [Test]
        public void AddingEdgeMakesVerticesNeighboursOfEachOther()
        {
            _v1.AddEdge(_v2);
            Assert.Contains(_v2, _v1.Outbound);
            Assert.Contains(_v1, _v2.Outbound);
            Assert.Contains(_v1, _v2.Inbound);
            Assert.Contains(_v2, _v1.Inbound);
            Assert.AreEqual(1, _v1.OutboundArcs.Select(arc => arc.Start == _v1 && arc.End == _v2).Count());
            Assert.AreEqual(1, _v1.InboundArcs.Select(arc => arc.Start == _v1 && arc.End == _v2).Count());
            Assert.AreEqual(1, _v2.OutboundArcs.Select(arc => arc.Start == _v1 && arc.End == _v2).Count());
            Assert.AreEqual(1, _v2.InboundArcs.Select(arc => arc.Start == _v1 && arc.End == _v2).Count());
        }

        [Test]
        public void CanNotAddMultipleEdgesBetweenTwoVertices()
        {
            _v1.AddEdge(_v2);
            Assert.Throws<NoMultiedgePermitedException>(() => _v1.AddEdge(_v2));
        }

        [Test]
        public void AddsOutboundArcToVertex()
        {
            _v1.AddArc(_v2);
            Assert.Contains(_v2, _v1.Outbound);
            Assert.Contains(_v1, _v2.Inbound);
            Assert.AreEqual(1, _v1.OutboundArcs.Select(arc => arc.Start == _v1 && arc.End == _v2).Count());
            Assert.AreEqual(1, _v2.InboundArcs.Select(arc => arc.Start == _v1 && arc.End == _v2).Count());
        }

        [Test]
        public void AddingArcToVertexIsOneWayOnly()
        {
            _v1.AddArc(_v2);
            Assert.IsFalse(_v2.Outbound.Contains(_v1));
            Assert.IsFalse(_v1.Inbound.Contains(_v2));
            Assert.IsFalse(_v2.OutboundArcs.Select(arc => arc.Start == _v2 && arc.End == _v1).Count() == 1);
            Assert.IsFalse(_v1.InboundArcs.Select(arc => arc.Start == _v2 && arc.End == _v1).Count() == 1);
        }
        
        [Test]
        public void CanNotAddArcIfStartVertexIsAlreadyConsecutiveToEndVertex()
        {
            _v1.AddArc(_v2);
            Assert.Throws<NoMultiedgePermitedException>(() => _v1.AddArc(_v2));
        }

        [Test]
        public void CanNotAddEdgeIfEndVertexIsAlreadyConsecutiveToStartVertex()
        {
            _v2.AddArc(_v1);
            Assert.Throws<NoMultiedgePermitedException>(() => _v1.AddEdge(_v2));
        }

        [Test]
        public void NewVertexHasOutDegreeZero()
        {
            Assert.AreEqual(0, _v1.OutDegree);
        }

        [Test]
        public void VertexKnowsItsNonzeroOutDegree()
        {
            _v1.AddArc(_v2);
            Assert.AreEqual(1, _v1.OutDegree);
        }

        [Test]
        public void NewVertexHasInDegreeZero()
        {
            Assert.AreEqual(0, _v1.InDegree);
        }

        [Test]
        public void VertexKnowsItsNonzeroInDegree()
        {
            _v2.AddArc(_v1);
            Assert.AreEqual(1, _v1.InDegree);
        }

        [Test]
        public void CanRemoveArcBetweenTwoVertices()
        {
            _v1.AddArc(_v2);
            _v1.RemoveArc(_v2);
            Assert.IsFalse(_v1.Outbound.Contains(_v2));
            Assert.IsFalse(_v2.Inbound.Contains(_v1));
            Assert.AreEqual(0, _v1.OutboundArcs.Select(arc => (arc.Start == _v1 && arc.End == _v2)).Count());
            Assert.AreEqual(0, _v2.InboundArcs.Select(arc => (arc.Start == _v1 && arc.End == _v2 )).Count());
        }

        [Test]
        public void RemovingNonExistentArcThrowsException()
        {
            Assert.Throws<NoArcException>(() => _v1.RemoveArc(_v2));
        }

        [Test]
        public void CanRemoveEdgeBetweenTwoVertices()
        {
            _v1.AddEdge(_v2);
            _v1.RemoveEdge(_v2);
            Assert.IsFalse(_v1.Outbound.Contains(_v2));
            Assert.IsFalse(_v2.Outbound.Contains(_v1));
            Assert.IsFalse(_v1.Inbound.Contains(_v2));
            Assert.IsFalse(_v2.Inbound.Contains(_v1));
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
            _v2.AddArc(_v1);
            Assert.Throws<NoArcException>(() => _v1.RemoveEdge(_v2));
        }

        [Test]
        public void NewVertexHasZeroInboundNeighbours()
        {
            Assert.AreEqual(0, _v1.Inbound.Count);
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
    }
}