using NUnit.Framework;
using System.Linq;

namespace GraphFramework
{
    [TestFixture]
    public class TwinVertexTests
    {
        public class TheConstructor : TwinVertexTests
        {
            private Vertex _v;
            private TwinVertex _tv;
            private TwinGraph _tg;

            [SetUp]
            public void DerivedInit()
            {
                _v = new Vertex();
                _tg = new TwinGraph();
                _tv = new TwinVertex(_v, _tg);
            }

            [Test]
            public void SetsPrecursorOnCreate()
            {
                Assert.That(_tv.Precursor, Is.SameAs(_v));
            }

            [Test]
            public void SetsTwinGraphOnCreate()
            {
                Assert.That(_tv.Graph, Is.SameAs(_tg));
            }

            [Test]
            public void CreatesAVertexOnCreate()
            {   
                Assert.That(_tv.A, Is.Not.Null);
            }

            [Test]
            public void CreatesBVertexOnCreate()
            {
                Assert.That(_tv.B, Is.Not.Null);
            }

            [Test]
            public void SetsCorrectTypeToAVertexOnCreate()
            {
                Assert.That(_tv.A.Type, Is.EqualTo(VertexType.A));
            }

            [Test]
            public void SetsCorrectTypeToBVertexOnCreate()
            {
                Assert.That(_tv.B.Type, Is.EqualTo(VertexType.B));
            }

            [Test]
            public void SetsVertexBAsTwinOfVertexAOnCreate()
            {
                Assert.That(_tv.A.Twin, Is.EqualTo(_tv.B));
            }
            
            [Test]
            public void SetsVertexAAsTwinOfVertexBOnCreate()
            {
                Assert.That(_tv.A.Twin, Is.EqualTo(_tv.B));
            }
        }

        


        [Test]
        public void AddsNotMatchingEdgeToNeighbour()
        {
            TwinVertex tv1 = new TwinVertex(null, null);
            TwinVertex tv2 = new TwinVertex(null, null);
            tv1.AddEdge(tv2, false);
            Assert.That(tv1.B.OutboundArcs.Count(a => a.End == tv2.A), Is.EqualTo(1));
            Assert.That(tv2.B.OutboundArcs.Count(a => a.End == tv1.A), Is.EqualTo(1));
        }

        [Test]
        public void AddsMatchingEdgeToNeighbour()
        {
            TwinVertex tv1 = new TwinVertex(null, null);
            TwinVertex tv2 = new TwinVertex(null, null);
            tv1.AddEdge(tv2, true);
            Assert.That(tv1.A.OutboundArcs.Count(a => a.End == tv2.B), Is.EqualTo(1));
            Assert.That(tv2.A.OutboundArcs.Count(a => a.End == tv1.B), Is.EqualTo(1));
        }

        [Test]
        public void TwinsHaveNameOfPrecursorWithTypeAppended()
        {
            var name = "badaboom";
            Vertex v = new Vertex(name);
            TwinVertex tv = new TwinVertex(v, null);
            Assert.That(tv.A.Name, Is.EqualTo(name + ".A"));
            Assert.That(tv.B.Name, Is.EqualTo(name + ".B"));
        }

        [Test]
        public void TwinVertexHasNameOfPrecursorWithTVAppended()
        {
            var name = "badaboom";
            Vertex v = new Vertex(name);
            TwinVertex tv = new TwinVertex(v, null);
            Assert.That(tv.Name, Is.EqualTo(name + ".TV"));
        }
    }
}