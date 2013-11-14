using NUnit.Framework;

namespace GraphFramework
{
    [TestFixture]
    public class NodeUnitTest
    {
        private Vertex _vertex1;
        private Vertex _vertex2;

        [SetUp]
        public void Init()
        {
            _vertex1 = new Vertex();
            _vertex2 = new Vertex();
        }

        [Test]
        public void NewVertexHasNoNeighbours()
        {
            Vertex vertex = new Vertex();
            Assert.AreEqual(0, vertex.Neighbours.Count);
        }

        [Test]
        public void AddsEdgeToVertex()
        {
            _vertex1.AddEdge(_vertex2);
            Assert.Contains(_vertex2, _vertex1.Neighbours);
        }

        [Test]
        public void AddingEdgeMakesVerticesNeighboursOfEachOther()
        {
            _vertex1.AddEdge(_vertex2);
            Assert.Contains(_vertex1, _vertex2.Neighbours);
        }

        [Test]
        public void CanNotAddMultipleEdgesBetweenTwoVertices()
        {
            _vertex1.AddEdge(_vertex2);
            Assert.Throws<NoMultiedgePermitedException>(() => _vertex1.AddEdge(_vertex2));
        }

        [Test]
        public void AddsArcToVertex()
        {
            _vertex1.AddArc(_vertex2);
            Assert.Contains(_vertex2, _vertex1.Neighbours);
        }

        [Test]
        public void AddingArcToVertexIsOneWayOnly()
        {
            _vertex1.AddArc(_vertex2);
            Assert.IsFalse(_vertex2.Neighbours.Contains(_vertex1));
        }
        
        [Test]
        public void CanNotAddArcIfStartVertexIsAlreadyConsecutiveToEndVertex()
        {
            _vertex1.AddArc(_vertex2);
            Assert.Throws<NoMultiedgePermitedException>(() => _vertex1.AddArc(_vertex2));
        }

        [Test]
        public void CanNotAddEdgeIfEndVertexIsAlreadyConsecutiveToStartVertex()
        {
            _vertex2.AddArc(_vertex1);
            Assert.Throws<NoMultiedgePermitedException>(() => _vertex1.AddEdge(_vertex2));
        }

        [Test]
        public void NewVertexHasOutDegreeZero()
        {
            Assert.AreEqual(0, _vertex1.OutDegree);
        }

        [Test]
        public void VertexKnowsItsNonzeroOutDegree()
        {
            _vertex1.AddArc(_vertex2);
            Assert.AreEqual(1, _vertex1.OutDegree);
        }

        [Test]
        public void CanRemoveArcBetweenTwoVertices()
        {
            _vertex1.AddArc(_vertex2);
            _vertex1.RemoveArc(_vertex2);
            Assert.IsFalse(_vertex1.Neighbours.Contains(_vertex2));
        }

        [Test]
        public void RemovingNonExistentArcThrowsException()
        {
            Assert.Throws<NoArcException>(() => _vertex1.RemoveArc(_vertex2));
        }

        [Test]
        public void CanRemoveEdgeBetweenTwoVertices()
        {
            _vertex1.AddEdge(_vertex2);
            _vertex1.RemoveEdge(_vertex2);
            Assert.IsFalse(_vertex1.Neighbours.Contains(_vertex2));
            Assert.IsFalse(_vertex2.Neighbours.Contains(_vertex1));
        }

        [Test]
        public void RemovingNonExistenEdgeThrowsException()
        {
            Assert.Throws<NoArcException>(() => _vertex1.RemoveEdge(_vertex2));
        }

        [Test]
        public void RemovingEdgeWhenArcFromEndVertexToStartVertexExistsThrowsException()
        {
            _vertex2.AddArc(_vertex1);
            Assert.Throws<NoArcException>(() => _vertex1.RemoveEdge(_vertex2));
        }
    }
}